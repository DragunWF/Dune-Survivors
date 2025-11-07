using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
    #region Animator Constant Parameters

    private const string IS_MOVING = "isMoving";
    private const string IS_DEAD = "isDead";

    #endregion

    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float bulletSpeed = 25f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;

    private SpriteRenderer playerSpriteRenderer;
    private GameObject playerWeaponObj;
    private SpriteRenderer weaponSpriteRenderer;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector2 inputVector;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();

        playerWeaponObj = GameObject.Find("Gun");
        weaponSpriteRenderer = playerWeaponObj.GetComponent<SpriteRenderer>();
        gunTip = playerWeaponObj.transform.Find("GunTip");

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Aim();

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
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

        var isMouseToTheLeft = mousePosition.x < transform.position.x;
        playerSpriteRenderer.flipX = isMouseToTheLeft;
        weaponSpriteRenderer.flipY = isMouseToTheLeft;

        // Transform weapon position based on mouse side
        float weaponPosX = Mathf.Abs(playerWeaponObj.transform.localPosition.x);
        Vector2 weaponPos = new Vector2(
            isMouseToTheLeft ? -weaponPosX : weaponPosX,
            playerWeaponObj.transform.localPosition.y
        );
        playerWeaponObj.transform.localPosition = weaponPos;
    }

    private void Shoot()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        var direction = (Vector2)(mousePosition - gunTip.position).normalized;
        bulletRigidbody.velocity = direction * bulletSpeed;
    }
}
