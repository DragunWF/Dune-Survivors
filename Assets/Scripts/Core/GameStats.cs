using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance { get; private set; }
    public bool IsGameWin { get; private set; }
    public int HighestPointsEarned { get; private set; }
    public bool IsNewHighScore { get; private set; } // For points

    [SerializeField] private int points = 0; // Used for purchasing upgrades
    private int enemiesDefeated = 0;

    private GameSceneUI gameSceneUI;

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

    private void Start() => OnGameReset();
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameSceneUI = FindObjectOfType<GameSceneUI>();
    }

    public void IncrementEnemiesDefeated()
    {
        enemiesDefeated++;
        gameSceneUI.UpdateEnemiesDefeatedUI(enemiesDefeated);
    }

    public void OnGameReset()
    {
        IsNewHighScore = false;
        points = 0;
        enemiesDefeated = 0;
    }

    public void OnGameWin()
    {
        IsGameWin = true;
        if (points > HighestPointsEarned)
        {
            HighestPointsEarned = points;
            IsNewHighScore = true;
        }
    }

    #region Points Management Methods

    public int GetPoints() => points;
    public int GetEnemiesDefeated() => enemiesDefeated;

    public void AddPoints(int amount)
    {
        points += amount;

        if (gameSceneUI != null)
        {
            gameSceneUI.UpdatePointsText(points);
        }
        else
        {
            Debug.LogError("GameSceneUI is null");
        }
    }

    public void SubtractPoints(int amount)
    {
        points -= amount;
        if (points < 0) points = 0;

        if (gameSceneUI != null)
        {
            gameSceneUI.UpdatePointsText(points);
        }
        else
        {
            Debug.LogError("GameSceneUI is null");
        }
    }

    #endregion
}
