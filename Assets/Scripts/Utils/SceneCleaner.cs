using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCleaner : MonoBehaviour
{
    // For clearing projectiles and effects before entering the next scene
    public void DestroyAllProjectilesAndEffects()
    {
        string[] tags =
        {
            GameTag.EnemyProjectile.ToString(),
            GameTag.Bullet.ToString(),
            GameTag.ImpactEffect.ToString()
        };
        foreach (string tag in tags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }
        }
    }
}
