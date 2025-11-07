using UnityEngine;

public sealed class Bullet : MonoBehaviour
{
    [SerializeField] private float despawnTime = 5f;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }
}
