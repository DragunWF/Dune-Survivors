using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }

    [SerializeField] private int points = 0; // Used for purchasing upgrades
    private int enemiesDefeated = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
    }

    #region Points Management Methods

    public int GetPoints() => points;
    public int GetEnemiesDefeated() => enemiesDefeated;

    public void AddPoints(int amount)
    {
        points += amount;
    }

    public void SubtractPoints(int amount)
    {
        points -= amount;
        if (points < 0) points = 0;
    }

    #endregion
}
