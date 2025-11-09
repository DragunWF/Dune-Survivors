using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WaveMechanic : MonoBehaviour
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
            new(15, 2f, 1), // Wave 1
            new(30, 1.5f, 2), // Wave 2
            new(45, 1f, 3), // Wave 3
            new(60, 0.8f, 4, 2), // Wave 4
            new(75, 0.5f, 5, 3), // Wave 5
            new(90, 0.5f, 6, 4), // Wave 6
            new(105, 0.3f, 7, 5), // Wave 7
            new(120, 0.3f, 8, 5), // Wave 8
            new(135, 0.2f, 9, 6), // Wave 9
            new(150, 0.2f, 10, 6) // Wave 10
        };
    }

    private void Start()
    {
        StartCoroutine(StartWave(GetCurrentWave()));
    }

    private IEnumerator StartWave(Wave wave)
    {
        gameSceneUI.UpdateWaveUI(currentWave, waves.Length);

        int secondsPassed = 0;
        while (secondsPassed < wave.TimeLimit)
        {
            gameSceneUI.UpdateWaveTimerUI(wave.TimeLimit - secondsPassed);
            yield return new WaitForSeconds(1f);
            secondsPassed++;
        }

        currentWave++;
        StartCoroutine(StartWave(GetCurrentWave()));
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
