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
}
