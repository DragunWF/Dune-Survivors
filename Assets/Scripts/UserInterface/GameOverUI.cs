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

    private GameStats gameStats;

    private void Awake()
    {
        // TODO: Get points text game objects and enemies defeated text
    }

    private void Start()
    {
        gameStats = FindObjectOfType<GameStats>();

        // TODO: Set points text and enemies defeated text here
    }
}
