using UnityEngine;

public sealed class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private float despawnTime = 5f;
    [SerializeField] private bool isEnemyProjectile = false;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    private void OnDestroy()
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyProjectile && collision.CompareTag(GameTag.Player.ToString()))
        {
            Destroy(gameObject);
        }
        else if (!isEnemyProjectile && collision.CompareTag(GameTag.Enemy.ToString()))
        {
            Destroy(gameObject);
        }
    }
}
