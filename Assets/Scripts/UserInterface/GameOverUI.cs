using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    #region Game Object Names

    private const string POINTS_TEXT = "PointsText";
    private const string HIGH_SCORE_TEXT = "HighScoreText"; // Relates to points
    private const string NOTIFICATION_TEXT = "NotificationText";
    private const string ENEMIES_DEFEATED_TEXT = "EnemiesDefeatedText";
    private const string GAME_OVER_ART = "GameOverArt";
    private const string PLAYER_WIN_ART = "PlayerWinArt";
    private const string GAME_OVER_TEXT = "GameOverText";

    #endregion

    private TextMeshProUGUI pointsText;
    private TextMeshProUGUI enemiesDefeatedText;
    private TextMeshProUGUI highScoreText;
    private TextMeshProUGUI gameOverText;
    private GameObject notificationTextObj;
    private GameObject gameOverArt;
    private GameObject playerWinArt;

    private GameStats gameStats;

    private void Awake()
    {
        gameOverText = GameObject.Find(GAME_OVER_TEXT).GetComponent<TextMeshProUGUI>();
        pointsText = GameObject.Find(POINTS_TEXT).GetComponent<TextMeshProUGUI>();
        enemiesDefeatedText = GameObject.Find(ENEMIES_DEFEATED_TEXT).GetComponent<TextMeshProUGUI>();
        highScoreText = GameObject.Find(HIGH_SCORE_TEXT).GetComponent<TextMeshProUGUI>();
        gameOverArt = GameObject.Find(GAME_OVER_ART);
        playerWinArt = GameObject.Find(PLAYER_WIN_ART);

        notificationTextObj = GameObject.Find(NOTIFICATION_TEXT);
        notificationTextObj.SetActive(false);
    }

    private void Start()
    {
        gameStats = GameStats.Instance;
        SetGameStats();
    }

    private void SetGameStats()
    {
        pointsText.text = $"Points: {gameStats.GetPoints()}";
        enemiesDefeatedText.text = $"Enemies Defeated: {gameStats.GetEnemiesDefeated()}";
        highScoreText.text = $"High Score: {gameStats.HighestPointsEarned}";

        if (gameStats.IsNewHighScore)
        {
            notificationTextObj.SetActive(true);
        }

        gameOverArt.SetActive(false);
        playerWinArt.SetActive(false);

        if (gameStats.IsGameWin)
        {
            gameOverText.text = "You Win!";
            playerWinArt.SetActive(true);
        }
        else
        {
            gameOverText.text = "Game Over!";
            gameOverArt.SetActive(true);
        }
    }
}
