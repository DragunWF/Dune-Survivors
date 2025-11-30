using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FlashEffect))]
[RequireComponent(typeof(PlayerUpgrades))]
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

    #region Attribute Constants

    // Amount of spread that increases when multishot is upgraded
    private const float spreadIncreaseBylevel = 1.75f;
    private const float initialSpread = 15f;

    // Amount of bullet speed that increases when fire rate is upgraded
    private const float bulletSpeedIncreaseByLevel = 1.5f;
    private const float initialBulletSpeed = 12f;

    #endregion

    #region Serializer Fields

    [Header("Player Stats")]
    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private int multiShotCount = 1;
    [SerializeField] private float bulletSpeed = initialBulletSpeed;
    [SerializeField] private float damageCooldownDuration = 3.5f;

    [Header("Shotgun Attack Settings")]
    [SerializeField] private float shotgunSpreadAngle = initialSpread;

    [Header("Object Dependencies")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;

    #endregion

    #region Player State Fields

    public bool IsDead => animator.GetBool(IS_DEAD);

    private bool isInDamageCooldown = false;
    private float nextFireTime = 0f;

    #endregion

    #region Component & Game Object References

    private SpriteRenderer playerSpriteRenderer;
    private GameObject playerWeaponObj;
    private SpriteRenderer weaponSpriteRenderer;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector2 inputVector;

    private PlayerUpgrades playerUpgrades;
    private FlashEffect flashEffect;
    private AudioPlayer audioPlayer;
    private GameSceneUI gameSceneUI;
    private PauseMenuUI pauseMenuUI;
    private SceneFader sceneFader;
    private GameStats gameStats;

    #endregion

    private void Awake()
    {
        // Ensure time scale is reset when the game starts, especially for editor runs.
        Time.timeScale = 1f;

        rigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        gameSceneUI = FindObjectOfType<GameSceneUI>();
        pauseMenuUI = FindObjectOfType<PauseMenuUI>();
        playerUpgrades = GetComponent<PlayerUpgrades>();
        sceneFader = FindObjectOfType<SceneFader>();

        playerWeaponObj = GameObject.Find(PLAYER_WEAPON);
        weaponSpriteRenderer = playerWeaponObj.GetComponent<SpriteRenderer>();
        gunTip = playerWeaponObj.transform.Find(GUN_TIP);

        flashEffect = GetComponent<FlashEffect>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameStats = GameStats.Instance;
        gameSceneUI.UpdatePlayerHealth(health);
    }

    private void Update()
    {
        // Handle pause/resume input using Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f) // If game is currently paused
            {
                pauseMenuUI.OnResumeButtonClick();
            }
            else // If game is not paused
            {
                pauseMenuUI.OnPauseButtonClick();
            }
        }

        if (Time.timeScale == 0f) // If game is paused, do not process input for movement or aiming
        {
            return;
        }

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
        Vector2 weaponPos = new(
            isMouseToTheLeft ? -weaponPosX : weaponPosX,
            playerWeaponObj.transform.localPosition.y
        );
        playerWeaponObj.transform.localPosition = weaponPos;
    }

    private void Shoot()
    {
        audioPlayer.PlayPlayerShootClip();

        for (int i = 0; i < multiShotCount; i++)
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = (Vector2)(mousePosition - gunTip.position);
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var spread = UnityEngine.Random.Range(-shotgunSpreadAngle / 2, shotgunSpreadAngle / 2);
            var rotation = Quaternion.Euler(0, 0, angle + spread);

            var bullet = Instantiate(bulletPrefab, gunTip.position, rotation);
            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = (rotation * Vector2.right).normalized * bulletSpeed;
        }
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
        StartCoroutine(StartDelayedGameOverTransition());

        // Disable player controls
        enabled = false;
    }

    private IEnumerator StartDelayedGameOverTransition()
    {
        yield return new WaitForSeconds(1.5f);
        gameStats.OnGameEnd(false);
        sceneFader.FadeToGameOverScene();
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
        bulletSpeed = initialBulletSpeed + (bulletSpeedIncreaseByLevel - 1) * playerUpgrades.FireRateLevel;
    }

    public void SetMultiShot(int projectileCount)
    {
        multiShotCount = projectileCount;
        shotgunSpreadAngle = initialSpread + (spreadIncreaseBylevel - 1) * playerUpgrades.MultiShotLevel;
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        health = newMaxHealth;
        gameSceneUI.UpdatePlayerHealth(health);
    }

    public void HealToFull()
    {
        health = playerUpgrades.MaxHealthCapacity;
        gameSceneUI.UpdatePlayerHealth(health);
    }

    public int GetHealth() => health;

    #endregion
}
