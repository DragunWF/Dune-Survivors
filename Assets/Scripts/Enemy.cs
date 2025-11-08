using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(FlashEffect))]
public sealed class Enemy : MonoBehaviour
{
    #region Game Object Name Constants

    private const string PLAYER = "Player";

    #endregion

    #region Serializer Fields

    [SerializeField] private int health = 3;
    [SerializeField] private float moveSpeed = 1.0f;

    #endregion

    private GameObject playerObj;
    private Transform playerTransform;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private FlashEffect flashEffect;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashEffect = GetComponent<FlashEffect>();

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
        if (playerTransform != null)
        {
            FlipSprite();
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
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
}
