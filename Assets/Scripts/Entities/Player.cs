using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FlashEffect))]
[RequireComponent(typeof(CapsuleCollider2D))]
public sealed class Player : MonoBehaviour
{
    #region Animator Constant Parameters

    private const string IS_MOVING = "isMoving";
    private const string IS_DEAD = "isDead";

    #endregion

    #region Game Object Name Constants

    private const string PLAYER_WEAPON = "Gun";
    private const string GUN_TIP = "GunTip";

    #endregion

    #region Serializer Fields

    [Header("Player Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float fireRate = 0.25f;
    [SerializeField] private int multiShotCount = 1;
    [SerializeField] private float bulletSpeed = 25f;
    [SerializeField] private float damageCooldownDuration = 3.5f;

    [Header("Object Dependencies")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;

    #endregion

    #region Player State Fields

    private bool isInDamageCooldown = false;
    private float nextFireTime = 0f;

    #endregion

    #region Component & Game Object References

    private SpriteRenderer playerSpriteRenderer;
    private GameObject playerWeaponObj;
    private SpriteRenderer weaponSpriteRenderer;

    private CapsuleCollider2D playerCollider;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector2 inputVector;

    private FlashEffect flashEffect;
    private AudioPlayer audioPlayer;
    private GameSceneUI gameSceneUI;

    #endregion

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        gameSceneUI = FindObjectOfType<GameSceneUI>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        playerWeaponObj = GameObject.Find(PLAYER_WEAPON);
        weaponSpriteRenderer = playerWeaponObj.GetComponent<SpriteRenderer>();
        gunTip = playerWeaponObj.transform.Find(GUN_TIP);

        flashEffect = GetComponent<FlashEffect>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameSceneUI.UpdatePlayerHealth(health);
    }

    private void Update()
    {
        Move();
        Aim();

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = inputVector * moveSpeed;
    }

    private void Move()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
        animator.SetBool(IS_MOVING, inputVector != Vector2.zero);
        inputVector.Normalize();
    }

    private void Aim()
    {
        // Aim weapon towards mouse position
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var aimDirection = mousePosition - transform.position;
        var aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        weaponSpriteRenderer.transform.eulerAngles = new Vector3(0, 0, aimAngle);

        // Flip player and weapon sprites based on mouse position
        var isMouseToTheLeft = mousePosition.x < transform.position.x;
        playerSpriteRenderer.flipX = isMouseToTheLeft;
        weaponSpriteRenderer.flipY = isMouseToTheLeft;

        // Transform weapon position based on mouse side
        var weaponPosX = Mathf.Abs(playerWeaponObj.transform.localPosition.x);
        Vector2 weaponPos = new Vector2(
            isMouseToTheLeft ? -weaponPosX : weaponPosX,
            playerWeaponObj.transform.localPosition.y
        );
        playerWeaponObj.transform.localPosition = weaponPos;
    }

    private void Shoot()
    {
        audioPlayer.PlayPlayerShootClip();

        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        var direction = (Vector2)(mousePosition - gunTip.position);
        bulletRigidbody.velocity = direction.normalized * bulletSpeed;
    }

    public void TakeDamage()
    {
        if (!isInDamageCooldown && health > 0)
        {
            flashEffect.Flash();
            audioPlayer.PlayDamageClip();
            health--;
            gameSceneUI.UpdatePlayerHealth(health);
            isInDamageCooldown = true;
            Invoke(nameof(ResetDamageCooldown), damageCooldownDuration);

            if (health <= 0)
            {
                Death();
            }
        }
    }

    private void ResetDamageCooldown()
    {
        isInDamageCooldown = false;
    }

    private void Death()
    {
        rigidBody.velocity = Vector2.zero;
        animator.SetBool(IS_DEAD, true);
        // Disable player controls
        enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.EnemyProjectile.ToString()))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag(GameTag.Enemy.ToString()))
        {
            TakeDamage();
        }
    }

    #region Public Setter Methods for Upgrade Menu

    public void SetFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }

    public void SetMultiShot(int projectileCount)
    {
        multiShotCount = projectileCount;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        health = newMaxHealth;
        gameSceneUI.UpdatePlayerHealth(health);
    }

    public void HealToFull()
    {
        gameSceneUI.UpdatePlayerHealth(health);
    }

    public int GetHealth() => health;

    #endregion
}
