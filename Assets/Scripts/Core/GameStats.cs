using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    [SerializeField] private int points = 0; // Used for purchasing upgrades
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

    #region Points Management Methods

    public int GetPoints() => points;

    public void AddPoints(int amount)
    {
        points += amount;
        gameSceneUI.UpdatePointsText(points);
    }

    public void SubtractPoints(int amount)
    {
        points -= amount;
        if (points < 0) points = 0;
        gameSceneUI.UpdatePointsText(points);
    }

    #endregion
}
