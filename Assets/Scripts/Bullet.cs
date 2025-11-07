using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float despawnTime = 5f;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }
}
