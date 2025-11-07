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

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 inputVector;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        SpriteDirection();
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

    private void SpriteDirection()
    {
        if (inputVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
