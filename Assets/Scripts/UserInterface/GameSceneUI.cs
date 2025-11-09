using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [Tooltip("Sprites representing the player health state")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    // Player related UI elements
    private const int PLAYER_HEART_COUNT = 3;
    private GameObject[] playerHearts;

    // Wave related UI elements
    private TextMeshProUGUI waveText;
    private TextMeshProUGUI waveTimerText;

    // Game Stats related UI elements
    private TextMeshProUGUI enemiesDefeatedText;

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

        waveText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
        waveTimerText = GameObject.Find("WaveTimerText").GetComponent<TextMeshProUGUI>();
        enemiesDefeatedText = GameObject.Find("EnemiesDefeatedText").GetComponent<TextMeshProUGUI>();
    }

    #region UI Update Methods

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

    public void UpdateWaveUI(int waveNumber, int totalWaves)
    {
        waveText.text = $"Wave: {waveNumber} of {totalWaves}";
    }

    public void UpdateWaveTimerUI(int timeRemaining)
    {
        waveTimerText.text = $"Time Left: {timeRemaining}";
    }

    public void UpdateEnemiesDefeatedUI(int enemiesDefeated)
    {
        enemiesDefeatedText.text = $"Enemies Defeated: {enemiesDefeated}";
    }

    #endregion
}
