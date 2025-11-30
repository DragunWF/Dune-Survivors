using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";

    public static GameStats Instance { get; private set; }
    public bool IsGameWin { get; private set; }
    public int HighestPointsEarned { get; private set; }
    public bool IsNewHighScore { get; private set; } // For points

    [SerializeField] private int points = 0; // Used for purchasing upgrades
    private int enemiesDefeated = 0;

    public int CurrentWave { get; private set; } = 1;

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
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            HighestPointsEarned = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        }
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            OnGameReset();
        }

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

    public void OnGameEnd(bool hasWon)
    {
        IsGameWin = hasWon;
        if (points > HighestPointsEarned)
        {
            HighestPointsEarned = points;
            IsNewHighScore = true;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighestPointsEarned);
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
