using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject impactEffectPrefab;
    [SerializeField] private float despawnTime = 5f;

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
