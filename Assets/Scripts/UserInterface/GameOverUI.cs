using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    #region Game Object Names

    private const string POINTS_TEXT = "PointsText";
    private const string HIGH_SCORE_TEXT = "HighScoreText"; // Still points
    private const string NOTIFICATION_TEXT = "NotificationText";
    private const string ENEMIES_DEFEATED_TEXT = "EnemiesDefeatedText";

    #endregion

    private TextMeshProUGUI pointsText;
    private TextMeshProUGUI enemiesDefeatedText;
    private TextMeshProUGUI highScoreText;

    private GameStats gameStats;

    private void Awake()
    {
        pointsText = GameObject.Find(POINTS_TEXT).GetComponent<TextMeshProUGUI>();
        enemiesDefeatedText = GameObject.Find(ENEMIES_DEFEATED_TEXT).GetComponent<TextMeshProUGUI>();
        highScoreText = GameObject.Find(HIGH_SCORE_TEXT).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameStats = FindObjectOfType<GameStats>();

        pointsText.text = $"Points: {gameStats.GetPoints()}";
        enemiesDefeatedText.text = $"Enemies Defeated: {gameStats.GetEnemiesDefeated()}";

        highScoreText.gameObject.SetActive(false);
        if (gameStats.IsNewHighScore)
        {
            highScoreText.gameObject.SetActive(true);
            highScoreText.text = $"New High Score: {gameStats.HighestPointsEarned}";
        }
    }
}
