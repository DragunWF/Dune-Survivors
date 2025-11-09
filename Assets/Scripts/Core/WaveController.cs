using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WaveController : MonoBehaviour
{
    [SerializeField] private int currentWave = 1;

    private Wave[] waves;
    private GameSceneUI gameSceneUI;

    private void Awake()
    {
        gameSceneUI = FindObjectOfType<GameSceneUI>();

        // Wave difficulty settings
        waves = new Wave[]
        {
            new(15, 2.5f, 2, 1),  // Wave 1 (~12 enemies)
            new(30, 2.5f, 3, 1),  // Wave 2 (~24 enemies)
            new(45, 2.2f, 4, 2),  // Wave 3 (~40 enemies)
            new(60, 2.5f, 4, 2),  // Wave 4 (~48 enemies)
            new(75, 2.2f, 4, 3),  // Wave 5 (~68 enemies)
            new(90, 2.0f, 4, 3),  // Wave 6 (90 enemies)
            new(105, 1.8f, 4, 4), // Wave 7 (~116 enemies)
            new(120, 1.8f, 4, 4), // Wave 8 (~133 enemies)
            new(135, 1.8f, 4, 5), // Wave 9 (150 enemies)
            new(150, 1.6f, 4, 5)  // Wave 10 (~187 enemies)
        };
    }

    private void Start()
    {
        StartCoroutine(WaveProgressionCoroutine());
    }

    private IEnumerator WaveProgressionCoroutine()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            currentWave = i + 1; // Update currentWave for the UI and GetCurrentWave()
            Wave wave = GetCurrentWave(); // Get the current wave data

            gameSceneUI.UpdateWaveUI(currentWave, waves.Length);

            int secondsPassed = 0;
            while (secondsPassed < wave.TimeLimit)
            {
                gameSceneUI.UpdateWaveTimerUI(wave.TimeLimit - secondsPassed);
                yield return new WaitForSeconds(1f);
                secondsPassed++;
            }
            // After a wave ends, there might be a brief pause before the next one starts.
            // For now, we'll just proceed to the next wave immediately.
            // If there's a "wave complete" screen or pause, it would go here.
        }
        // All waves completed. Handle game win condition or endless mode.
        Debug.Log("All waves completed!");
    }

    public Wave GetCurrentWave()
    {
        if (currentWave - 1 < waves.Length)
        {
            return waves[currentWave - 1];
        }
        else if (currentWave <= 0)
        {
            return waves[0];
        }
        else
        {
            return waves[^1]; // Return last wave if exceeded
        }
    }
}
