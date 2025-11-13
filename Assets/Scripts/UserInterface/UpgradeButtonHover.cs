using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum UpgradeType
    {
        FireRate,
        MultiShot,
        MaxHealth,
        Heal
    }

    public UpgradeType upgradeType;

    private UpgradesMenuUI upgradesMenuUI;
    private PlayerUpgrades playerUpgrades;

    private void Awake()
    {
        upgradesMenuUI = FindObjectOfType<UpgradesMenuUI>();
        playerUpgrades = FindObjectOfType<PlayerUpgrades>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int cost = 0;
        bool isMaxed = false;

        switch (upgradeType)
        {
            case UpgradeType.FireRate:
                cost = playerUpgrades.GetFireRateUpgradeCost();
                isMaxed = playerUpgrades.FireRateLevel >= playerUpgrades.GetMaxFireRateLevel();
                break;
            case UpgradeType.MultiShot:
                cost = playerUpgrades.GetMultiShotUpgradeCost();
                break;
            case UpgradeType.MaxHealth:
                cost = playerUpgrades.GetMaxHealthUpgradeCost();
                break;
            case UpgradeType.Heal:
                cost = playerUpgrades.GetHealCost();
                break;
        }

        if (isMaxed)
        {

        }
        else
        {
            upgradesMenuUI.UpdatePriceText(cost);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        upgradesMenuUI.UpdatePriceText();
    }
}
