using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesMenuUI : MonoBehaviour
{
    #region Game Object Name Constants

    private const string UPGRADES_MENU_PANEL = "UpgradesMenuPanel";

    private const string UPGRADE_FIRE_RATE_TEXT = "FireRateLevelText";
    private const string UPGRADE_MULTI_SHOT_TEXT = "MultiShotLevelText";
    private const string UPGRADE_MAX_HEALTH_TEXT = "MaxHealthText";
    private const string HEALTH_STATUS_TEXT = "HealthStatusText";

    private const string PRICE_TEXT = "PriceText";
    private const string POINTS_TEXT = "UpgradePointsText";

    #endregion

    private GameObject upgradesMenuPanel;

    private TextMeshProUGUI fireRateLevelText;
    private TextMeshProUGUI multiShotLevelText;
    private TextMeshProUGUI maxHealthText;
    private TextMeshProUGUI healthStatusText;

    private TextMeshProUGUI priceText;
    private TextMeshProUGUI pointsText;

    private WaveController waveController;
    private PlayerUpgrades playerUpgrades;
    private Player player;
    private GameStats gameStats;
    private GameSceneUI gameSceneUI;
    private AudioPlayer audioPlayer;

    private void Awake()
    {
        upgradesMenuPanel = GameObject.Find(UPGRADES_MENU_PANEL);
        if (upgradesMenuPanel == null)
        {
            Debug.LogError($"Could not find {UPGRADES_MENU_PANEL} GameObject in the scene.");
        }

        fireRateLevelText = GameObject.Find(UPGRADE_FIRE_RATE_TEXT).GetComponent<TextMeshProUGUI>();
        multiShotLevelText = GameObject.Find(UPGRADE_MULTI_SHOT_TEXT).GetComponent<TextMeshProUGUI>();
        maxHealthText = GameObject.Find(UPGRADE_MAX_HEALTH_TEXT).GetComponent<TextMeshProUGUI>();
        healthStatusText = GameObject.Find(HEALTH_STATUS_TEXT).GetComponent<TextMeshProUGUI>();

        priceText = GameObject.Find(PRICE_TEXT).GetComponent<TextMeshProUGUI>();
        pointsText = GameObject.Find(POINTS_TEXT).GetComponent<TextMeshProUGUI>();

        playerUpgrades = FindObjectOfType<PlayerUpgrades>();
        player = FindObjectOfType<Player>();
        waveController = FindObjectOfType<WaveController>();
        gameStats = FindObjectOfType<GameStats>();
        gameSceneUI = FindObjectOfType<GameSceneUI>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    private void Start()
    {
        HideUpgradesMenu();
    }

    #region Update UI Methods

    public void EnableUpgradesMenu()
    {
        if (player.IsDead)
        {
            return;
        }

        if (upgradesMenuPanel != null)
        {
            upgradesMenuPanel.SetActive(true);
            Time.timeScale = 0f; // Pause the game

            UpdatePointsText(gameStats.GetPoints());
            UpdateFireRateUpgradeText(playerUpgrades.FireRateLevel);
            UpdateMultiShotUpgradeText(playerUpgrades.MultiShotLevel);
            UpdateMaxHealthUpgradeText(playerUpgrades.MaxHealthCapacity);
            UpdateHealthStatusText(player.GetHealth(), playerUpgrades.MaxHealthCapacity);
        }
        else
        {
            Debug.LogError($"Could not find {UPGRADES_MENU_PANEL} GameObject in the scene.");
        }
    }

    public void HideUpgradesMenu()
    {
        upgradesMenuPanel.SetActive(false);
    }

    public void UpdateFireRateUpgradeText(int level)
    {
        if (level >= playerUpgrades.GetMaxFireRateLevel())
        {
            fireRateLevelText.text = "Maxed";
            return;
        }
        fireRateLevelText.text = $"Level {level}";
    }

    public void UpdateMultiShotUpgradeText(int level)
    {
        if (level >= playerUpgrades.GetMaxMultiShotLevel())
        {
            multiShotLevelText.text = "Maxed";
            return;
        }
        multiShotLevelText.text = $"Level {level}";
    }

    public void UpdateMaxHealthUpgradeText(int capacity)
    {
        if (capacity >= playerUpgrades.GetMaxHealthCapacity())
        {
            maxHealthText.text = "Maxed";
            return;
        }
        maxHealthText.text = $"Max: {capacity}";
    }

    public void UpdateHealthStatusText(int currentHealth, int maxHealth)
    {
        if (currentHealth >= maxHealth)
        {
            healthStatusText.text = "Full";
            return;
        }
        healthStatusText.text = $"{currentHealth}/{maxHealth}";
    }

    public void UpdatePointsText(int points)
    {
        if (pointsText != null)
        {
            string pointsWord = points == 1 ? "Point" : "Points";
            pointsText.text = $"You have {points} {pointsWord}";
        }
    }

    public void UpdatePriceText(int price = 0)
    {
        priceText.color = Color.black;
        if (price == 0)
        {
            priceText.text = "Hover over a button to view price";
            return;
        }
        priceText.text = $"Price: {price} Points";
    }

    public void UpdatePriceText(string text)
    {
        priceText.color = Color.black;
        priceText.text = text;
    }

    #endregion

    #region Button Click Methods

    public void OnNextWaveButtonClick()
    {
        Time.timeScale = 1f; // Resume the game
        waveController.StartNextWave();
        gameSceneUI.HideWaveCompleteText();
        audioPlayer.PlayClickClip();
        HideUpgradesMenu();
    }

    public void OnFireRateUpgradeButtonClick()
    {
        if (gameStats.GetPoints() < playerUpgrades.GetFireRateUpgradeCost())
        {
            DisplayErrorText("Not enough points");
            return;
        }
        if (playerUpgrades.FireRateLevel >= playerUpgrades.GetMaxFireRateLevel())
        {
            DisplayErrorText("Max Level Reached");
            return;
        }
        audioPlayer.PlayUpgradeClip();
        playerUpgrades.UpgradeFireRate();
        UpdateFireRateUpgradeText(playerUpgrades.FireRateLevel);
        UpdatePriceText(playerUpgrades.GetFireRateUpgradeCost());
        UpdateAllPointsText();
    }

    public void OnMultiShotUpgradeButtonClick()
    {
        if (gameStats.GetPoints() < playerUpgrades.GetMultiShotUpgradeCost())
        {
            DisplayErrorText("Not enough points");
            return;
        }
        if (playerUpgrades.MultiShotLevel >= playerUpgrades.GetMaxMultiShotLevel())
        {
            DisplayErrorText("Max Level Reached");
            return;
        }
        audioPlayer.PlayUpgradeClip();
        playerUpgrades.UpgradeMultiShot();
        UpdateMultiShotUpgradeText(playerUpgrades.MultiShotLevel);
        UpdatePriceText(playerUpgrades.GetMultiShotUpgradeCost());
        UpdateAllPointsText();
    }

    public void OnMaxHealthUpgradeButtonClick()
    {
        if (gameStats.GetPoints() < playerUpgrades.GetMaxHealthUpgradeCost())
        {
            DisplayErrorText("Not enough points");
            return;
        }
        if (playerUpgrades.MaxHealthCapacity >= playerUpgrades.GetMaxHealthCapacity())
        {
            DisplayErrorText("Max Level Reached");
            return;
        }

        audioPlayer.PlayUpgradeClip();
        playerUpgrades.UpgradeMaxHealth();
        UpdateMaxHealthUpgradeText(playerUpgrades.MaxHealthCapacity);
        UpdatePriceText(playerUpgrades.GetMaxHealthUpgradeCost());
        UpdateHealthStatusText(player.GetHealth(), playerUpgrades.MaxHealthCapacity);
        UpdateAllPointsText();
        gameSceneUI.UpdateMaxHealthHearts();
    }

    public void OnHealToFullButtonClick()
    {
        if (gameStats.GetPoints() < playerUpgrades.GetHealCost())
        {
            DisplayErrorText("Not enough points");
            return;
        }
        if (player.GetHealth() >= playerUpgrades.MaxHealthCapacity)
        {
            DisplayErrorText("Health is already full");
            return;
        }
        audioPlayer.PlayUpgradeClip();
        playerUpgrades.HealToFull();
        UpdateHealthStatusText(player.GetHealth(), playerUpgrades.MaxHealthCapacity);
        UpdateAllPointsText();
    }

    #endregion

    private void DisplayErrorText(string errorMessage)
    {
        audioPlayer.PlayErrorClip();
        priceText.text = errorMessage;
        priceText.color = Color.red;
    }

    private void UpdateAllPointsText()
    {
        gameSceneUI.UpdatePointsText(gameStats.GetPoints());
        UpdatePointsText(gameStats.GetPoints());
    }
}
