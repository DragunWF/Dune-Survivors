using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [Tooltip("Sprites representing the player health state")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private const int PLAYER_HEART_COUNT = 3;
    private GameObject[] playerHearts;

    private void Awake()
    {
        playerHearts = new GameObject[3];
        for (int i = 0; i < PLAYER_HEART_COUNT; i++)
        {
            GameObject heart = GameObject.Find($"Heart-{i + 1}");
            if (heart == null)
            {
                Debug.LogError($"Could not find Heart-{i + 1} GameObject in the scene.");
            }
            playerHearts[i] = heart;
        }
    }

    public void UpdatePlayerHealth(int currentHealth)
    {
        if (fullHeartSprite == null || emptyHeartSprite == null)
        {
            Debug.LogError("Heart sprites are not assigned in the inspector!");
            return;
        }

        for (int i = 0; i < PLAYER_HEART_COUNT; i++)
        {
            if (playerHearts[i] == null)
            {
                Debug.LogError($"Heart-{i + 1} GameObject not found!");
                continue;
            }

            Image heartImage = playerHearts[i].GetComponentInChildren<Image>();
            if (heartImage == null)
            {
                Debug.LogError($"Image component not found on Heart-{i + 1} or its children!");
                continue;
            }

            if (i < currentHealth)
            {
                heartImage.sprite = fullHeartSprite;
            }
            else
            {
                heartImage.sprite = emptyHeartSprite;
            }
        }
    }
}
