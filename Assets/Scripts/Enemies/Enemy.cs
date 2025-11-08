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

    [Tooltip("Enemy Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float deathAnimationDuration = 0.5f;

    [Tooltip("Knockback Settings")]
    [SerializeField] private float takeDamageKnockbackSpeed = 4.0f;
    [SerializeField] private float takeDamageKnockbackDuration = 0.1f;
    [SerializeField] private float hitPlayerKnockbackSpeed = 8.5f;
    [SerializeField] private float hitPlayerknockbackDuration = 0.25f;

    [Tooltip("Prefab Dependencies")]
    [SerializeField] private GameObject deathEffectPrefab;

    #endregion

    private GameObject playerObj;
    private Transform playerTransform;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private FlashEffect flashEffect;
    private AudioPlayer audioPlayer;
    private bool isKnockedBack = false;
    private bool isDead = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashEffect>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        animator = GetComponent<Animator>();

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
        if (isDead) return;

        if (playerTransform != null)
        {
            FlipSprite();
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (playerTransform != null && !isKnockedBack)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rigidBody.velocity = direction * moveSpeed;
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
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
                StartCoroutine(Knockback(false));
            }
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
