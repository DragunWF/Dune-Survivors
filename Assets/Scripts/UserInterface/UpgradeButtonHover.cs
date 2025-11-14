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
    private Player player;

    private void Awake()
    {
        upgradesMenuUI = FindObjectOfType<UpgradesMenuUI>();
        playerUpgrades = FindObjectOfType<PlayerUpgrades>();
        player = FindObjectOfType<Player>();
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
                isMaxed = playerUpgrades.MultiShotLevel >= playerUpgrades.GetMaxMultiShotLevel();
                break;
            case UpgradeType.MaxHealth:
                cost = playerUpgrades.GetMaxHealthUpgradeCost();
                isMaxed = playerUpgrades.MaxHealthCapacity >= playerUpgrades.GetMaxHealthCapacity();
                break;
            case UpgradeType.Heal:
                cost = playerUpgrades.GetHealCost();
                isMaxed = player.GetHealth() >= playerUpgrades.MaxHealthCapacity;
                break;
        }

        if (isMaxed)
        {
            upgradesMenuUI.UpdatePriceText("Already Maxed!");
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
