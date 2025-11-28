using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    #region Game Object Name Constants

    private const string HEART_PREFIX = "Heart"; // Primarily used for finding heart GameObjects
    private const string POINTS_TEXT = "PointsText";
    private const string WAVE_TEXT = "WaveText";
    private const string WAVE_TIMER_TEXT = "WaveTimerText";
    private const string ENEMIES_DEFEATED_TEXT = "EnemiesDefeatedText";
    private const string WAVE_COMPLETE_TEXT = "WaveCompleteText";

    #endregion

    [Header("General Settings")]
    [SerializeField] private int secondsToOpenUpgradeMenu = 5;

    [Header("Player Health State")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    // Player related UI elements
    private GameObject[] playerHearts;

    // Wave related UI elements
    private TextMeshProUGUI waveText;
    private TextMeshProUGUI waveTimerText;
    private TextMeshProUGUI waveCompleteText;

    // Game Stats related UI elements
    private TextMeshProUGUI pointsText;
    private TextMeshProUGUI enemiesDefeatedText;

    private Player player;
    private PlayerUpgrades playerUpgrades;
    private UpgradesMenuUI upgradesMenuUI;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        playerUpgrades = FindObjectOfType<PlayerUpgrades>();

        playerHearts = new GameObject[playerUpgrades.GetMaxHealthCapacity()];
        for (int i = 0, nth = 1; i < playerUpgrades.GetMaxHealthCapacity(); i++, nth++)
        {
            GameObject heart = GameObject.Find($"{HEART_PREFIX}-{nth}");
            if (heart == null)
            {
                Debug.LogError($"Could not find {HEART_PREFIX}-{nth} GameObject in the scene.");
            }
            playerHearts[i] = heart;

            if (nth > player.GetHealth())
            {
                playerHearts[i].SetActive(false);
            }
        }

        pointsText = GameObject.Find(POINTS_TEXT).GetComponent<TextMeshProUGUI>();
        waveText = GameObject.Find(WAVE_TEXT).GetComponent<TextMeshProUGUI>();
        waveTimerText = GameObject.Find(WAVE_TIMER_TEXT).GetComponent<TextMeshProUGUI>();
        enemiesDefeatedText = GameObject.Find(ENEMIES_DEFEATED_TEXT).GetComponent<TextMeshProUGUI>();
        waveCompleteText = GameObject.Find(WAVE_COMPLETE_TEXT).GetComponent<TextMeshProUGUI>();

        upgradesMenuUI = FindObjectOfType<UpgradesMenuUI>();
    }

    private void Start()
    {
        waveCompleteText.gameObject.SetActive(false);
    }

    #region Wave Completion UI Methods

    public void ShowAllWaveCompletedText()
    {
        waveCompleteText.text = "All waves completed!";
        waveCompleteText.gameObject.SetActive(true);
    }

    public void ShowWaveCompleteText()
    {
        waveCompleteText.gameObject.SetActive(true);
        StartCoroutine(StartUpgradeMenuTimer());
    }

    public void HideWaveCompleteText()
    {
        StopAllCoroutines();
        waveCompleteText.gameObject.SetActive(false);
    }

    public IEnumerator StartUpgradeMenuTimer()
    {
        for (int second = secondsToOpenUpgradeMenu; second >= 1; second--)
        {
            waveCompleteText.text = $"Wave complete! Opening upgrade menu in {second}...";
            yield return new WaitForSeconds(1f);
        }

        waveCompleteText.gameObject.SetActive(false);
        upgradesMenuUI.EnableUpgradesMenu();
    }

    #endregion

    #region UI Update Methods

    public void UpdatePlayerHealth(int currentHealth)
    {
        if (fullHeartSprite == null || emptyHeartSprite == null)
        {
            Debug.LogError("Heart sprites are not assigned in the inspector!");
            return;
        }

        for (int i = 0; i < playerUpgrades.GetMaxHealthCapacity(); i++)
        {
            if (playerHearts[i] == null)
            {
                Debug.LogError($"Heart-{i + 1} GameObject not found!");
                continue;
            }

            Image heartImage = playerHearts[i].GetComponentInChildren<Image>();
            if (heartImage == null)
            {
                Debug.LogError($"Image component not found on Heart-{i + 1} or its children!");
                continue;
            }

            if (i < currentHealth)
            {
                heartImage.sprite = fullHeartSprite;
            }
            else
            {
                heartImage.sprite = emptyHeartSprite;
            }
        }
    }

    public void UpdateMaxHealthHearts()
    {
        for (int i = 0; i < playerUpgrades.MaxHealthCapacity; i++)
        {
            playerHearts[i].SetActive(true);
        }
    }

    public void UpdateWaveUI(int waveNumber, int totalWaves)
    {
        waveText.text = $"Wave: {waveNumber} of {totalWaves}";
    }

    public void UpdateWaveTimerUI(int timeRemaining)
    {
        waveTimerText.text = $"Time Left: {timeRemaining}";
    }

    public void UpdateEnemiesDefeatedUI(int enemiesDefeated)
    {
        enemiesDefeatedText.text = $"Enemies Defeated: {enemiesDefeated}";
    }

    public void UpdatePointsText(int points)
    {
        pointsText.text = $"Points: {points}";
    }

    #endregion
}
