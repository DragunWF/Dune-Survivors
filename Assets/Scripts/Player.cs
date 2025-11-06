using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public sealed class Player : MonoBehaviour
{
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
