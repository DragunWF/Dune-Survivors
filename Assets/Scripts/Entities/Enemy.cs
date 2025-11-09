using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(FlashEffect))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public sealed class Enemy : MonoBehaviour
{
    #region Game Object Name Constants and Animator Parameters

    private const string PLAYER = "Player"; // GameObject tag
    private const string IS_DEAD = "isDead"; // Animator parameter

    #endregion

    #region Serializer Fields

    [Header("Enemy Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float deathAnimationDuration = 0.5f;

    [Header("Knockback Settings")]
    [SerializeField] private float takeDamageKnockbackSpeed = 4.0f;
    [SerializeField] private float takeDamageKnockbackDuration = 0.1f;
    [SerializeField] private float hitPlayerKnockbackSpeed = 8.5f;
    [SerializeField] private float hitPlayerknockbackDuration = 0.25f;

    [Header("Prefab Dependencies")]
    [SerializeField] private GameObject deathEffectPrefab;
    [SerializeField] private GameObject magicProjectilePrefab;

    [Header("Enemy Type")]
    [SerializeField] private EnemyType enemyType = EnemyType.Melee;

    [Header("Ranged Attack Settings")]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackInterval = 2f;
    [SerializeField] private float projectileSpeed = 2f;

    [Header("Burst Attack Settings")]
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float timeBetweenBursts = 0.2f;

    [Header("Shotgun Attack Settings")]
    [SerializeField] private int shotgunPelletCount = 5;
    [SerializeField] private float shotgunSpreadAngle = 75f;

    #endregion

    private GameObject playerObj;
    private Transform playerTransform;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private FlashEffect flashEffect;
    private bool isKnockedBack = false;
    private bool isDead = false;

    private AudioPlayer audioPlayer;
    private GameStats gameStats;

    private float nextAttackTime = 0f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashEffect>();
        animator = GetComponent<Animator>();

        audioPlayer = FindObjectOfType<AudioPlayer>();
        gameStats = FindObjectOfType<GameStats>();

        playerObj = GameObject.FindWithTag(PLAYER);
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found. Make sure your player is tagged 'Player'.");
        }
    }

    private void Update()
    {
        if (isDead || playerTransform == null) return;

        FlipSprite();

        if (enemyType == EnemyType.Melee) return;

        if (Time.time >= nextAttackTime)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                PerformRangedAttack();
                nextAttackTime = Time.time + attackInterval;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead || playerTransform == null || isKnockedBack) return;

        if (enemyType != EnemyType.Melee && Vector2.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            rigidBody.velocity = Vector2.zero; // Stop moving if in attack range for ranged enemies
            return;
        }

        MoveTowardsPlayer();
    }

    private void PerformRangedAttack()
    {
        switch (enemyType)
        {
            case EnemyType.Ranged:
                Shoot(GetDirectionToPlayer());
                break;
            case EnemyType.Burst:
                StartCoroutine(BurstAttack());
                break;
            case EnemyType.Shotgun:
                ShotgunAttack();
                break;
        }
    }

    private void Shoot(Vector2 direction)
    {
        if (magicProjectilePrefab == null)
        {
            Debug.LogError("Magic Projectile Prefab is not assigned in the inspector!");
            return;
        }

        GameObject projectile = Instantiate(magicProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.velocity = direction.normalized * projectileSpeed;
        }
    }

    private IEnumerator BurstAttack()
    {
        for (int i = 0; i < burstCount; i++)
        {
            Shoot(GetDirectionToPlayer());
            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    private void ShotgunAttack()
    {
        for (int i = 0; i < shotgunPelletCount; i++)
        {
            Vector2 direction = GetDirectionToPlayer();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float spread = Random.Range(-shotgunSpreadAngle / 2, shotgunSpreadAngle / 2);
            Quaternion rotation = Quaternion.Euler(0, 0, angle + spread);
            Shoot(rotation * Vector2.right);
        }
    }

    private Vector2 GetDirectionToPlayer()
    {
        if (playerTransform == null) return Vector2.zero;
        return (playerTransform.position - transform.position).normalized;
    }

    private void MoveTowardsPlayer()
    {
        rigidBody.velocity = GetDirectionToPlayer() * moveSpeed;
    }

    private void FlipSprite()
    {
        bool isPlayerToTheRight = playerTransform.position.x < transform.position.x;
        spriteRenderer.flipX = isPlayerToTheRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag(GameTag.Bullet.ToString()))
        {
            Destroy(collision.gameObject);
            health--;
            flashEffect.Flash();
            StartCoroutine(Knockback(true));
            audioPlayer.PlayDamageClip();

            if (health <= 0)
            {
                Death();
            }
        }
        else if (collision.CompareTag(GameTag.Player.ToString()))
        {
            StartCoroutine(Knockback(false));
        }
    }

    private IEnumerator Knockback(bool isBulletTaken)
    {
        isKnockedBack = true;
        if (playerTransform != null)
        {
            Vector2 knockbackDirection = (transform.position - playerTransform.position).normalized;
            float randomSpeed = isBulletTaken
                ? Random.Range(takeDamageKnockbackSpeed * 0.8f, takeDamageKnockbackSpeed * 1.2f)
                : Random.Range(hitPlayerKnockbackSpeed * 0.8f, hitPlayerKnockbackSpeed * 1.2f);
            rigidBody.velocity = knockbackDirection * randomSpeed;
        }
        yield return new WaitForSeconds(hitPlayerknockbackDuration);
        isKnockedBack = false;
        rigidBody.velocity = Vector2.zero;
    }

    private void Death()
    {
        if (isDead) return;
        isDead = true;

        rigidBody.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        animator.SetBool(IS_DEAD, true);
        gameStats.IncrementEnemiesDefeated();

        StartCoroutine(DestroyAfterAnimation());
    }

    private IEnumerator DestroyAfterAnimation()
    {
        const float stopFlashDelay = 0.1f;
        yield return new WaitForSeconds(stopFlashDelay);
        flashEffect.StopFlash();

        yield return new WaitForSeconds(deathAnimationDuration);

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
