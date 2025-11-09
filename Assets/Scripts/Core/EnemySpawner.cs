using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float minSpawnOffset = 2f;
    [SerializeField] private float maxSpawnOffset = 5f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            StartCoroutine(SpawnEnemies());
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            float randomInterval = Random.Range(spawnInterval * 0.75f, spawnInterval * 1.25f);
            yield return new WaitForSeconds(randomInterval);

            Vector2 spawnPosition = GetRandomSpawnPosition();
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        Vector2 camPosition = mainCamera.transform.position;

        // Pick a side: 0 = top, 1 = bottom, 2 = left, 3 = right
        int side = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;

        switch (side)
        {
            case 0: // Top spawn zone
                spawnPoint.x = Random.Range(camPosition.x - camWidth - maxSpawnOffset, camPosition.x + camWidth + maxSpawnOffset);
                spawnPoint.y = Random.Range(camPosition.y + camHeight + minSpawnOffset, camPosition.y + camHeight + maxSpawnOffset);
                break;
            case 1: // Bottom spawn zone
                spawnPoint.x = Random.Range(camPosition.x - camWidth - maxSpawnOffset, camPosition.x + camWidth + maxSpawnOffset);
                spawnPoint.y = Random.Range(camPosition.y - camHeight - maxSpawnOffset, camPosition.y - camHeight - minSpawnOffset);
                break;
            case 2: // Left spawn zone
                spawnPoint.x = Random.Range(camPosition.x - camWidth - maxSpawnOffset, camPosition.x - camWidth - minSpawnOffset);
                spawnPoint.y = Random.Range(camPosition.y - camHeight - maxSpawnOffset, camPosition.y + camHeight + maxSpawnOffset);
                break;
            case 3: // Right spawn zone
                spawnPoint.x = Random.Range(camPosition.x + camWidth + minSpawnOffset, camPosition.x + camWidth + maxSpawnOffset);
                spawnPoint.y = Random.Range(camPosition.y - camHeight - maxSpawnOffset, camPosition.y + camHeight + maxSpawnOffset);
                break;
        }

        return spawnPoint;
    }
}
