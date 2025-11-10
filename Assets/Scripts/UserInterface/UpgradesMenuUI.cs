using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
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
    private const string POINTS_TEXT = "PointsText";

    #endregion

    private GameObject upgradesMenuPanel;

    private TextMeshProUGUI fireRateLevelText;
    private TextMeshProUGUI multiShotLevelText;
    private TextMeshProUGUI maxHealthText;
    private TextMeshProUGUI healthStatusText;

    private TextMeshProUGUI priceText;
    private TextMeshProUGUI pointsText;

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
    }

    private void Start()
    {
        HideUpgradesMenu();
    }

    #region Update UI Methods

    public void ShowUpgradesMenu()
    {
        if (upgradesMenuPanel != null)
        {
            upgradesMenuPanel.SetActive(true);
        }
    }

    public void HideUpgradesMenu()
    {
        if (upgradesMenuPanel != null)
        {
            upgradesMenuPanel.SetActive(false);
        }
    }

    public void UpdateFireRateUpgradeText(int level)
    {
        if (fireRateLevelText != null)
        {
            fireRateLevelText.text = $"Level {level}";
        }
    }

    public void UpdateMultiShotUpgradeText(int level)
    {
        if (multiShotLevelText != null)
        {
            multiShotLevelText.text = $"Level {level}";
        }
    }

    public void UpdateMaxHealthUpgradeText(int level)
    {
        if (maxHealthText != null)
        {
            maxHealthText.text = $"Level {level}";
        }
    }

    public void UpdateHealthStatusText(int currentHealth, int maxHealth)
    {
        if (healthStatusText != null)
        {
            if (currentHealth >= maxHealth)
            {
                healthStatusText.text = "Full";
                return;
            }
            healthStatusText.text = $"{currentHealth}/{maxHealth}";
        }
    }

    public void UpdatePointsText(int points)
    {
        if (pointsText != null)
        {
            string pointsWord = points == 1 ? "Point" : "Points";
            pointsText.text = $"{points} {pointsWord}";
        }
    }

    public void UpdatePriceText(int price = 0)
    {
        if (priceText != null)
        {
            if (price == 0)
            {
                priceText.text = "Hover over a button to view price";
                return;
            }
            priceText.text = $"Price: {price} Points";
        }
    }

    #endregion

    #region Button Click Methods

    public void OnNextWaveButtonClick()
    {

    }

    public void OnFireRateUpgradeButtonClick()
    {

    }

    public void OnMultiShotUpgradeButtonClick()
    {

    }

    public void OnMaxHealthUpgradeButtonClick()
    {

    }

    public void OnHealToFullButtonClick()
    {

    }

    #endregion
}
