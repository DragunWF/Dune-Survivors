using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    private int enemiesDefeated = 0;
    private GameSceneUI gameSceneUI;

    private void Awake()
    {
        gameSceneUI = FindObjectOfType<GameSceneUI>();
    }

    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
        if (gameSceneUI != null)
        {
            gameSceneUI.UpdateEnemiesDefeatedUI(enemiesDefeated);
        }
        else
        {
            Debug.LogError("GameSceneUI reference is missing in GameStats!");
        }
    }
}
