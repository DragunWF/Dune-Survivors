using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Player))]
public class PlayerUpgrades : MonoBehaviour
{
    #region Upgrade Limits

    private const int MAX_FIRE_RATE_LEVEL = 5;
    private const int MAX_MULTI_SHOT_LEVEL = 3;
    private const int MAX_HEALTH_CAPACITY = 5;

    #endregion

    #region Upgrade Properties

    public int FireRateLevel { get; private set; } = 1;
    public int MultiShotLevel { get; private set; } = 1;
    public int MaxHealthCapacity { get; private set; } = 3;

    #endregion

    private Player player;
    private GameStats gameStats;

    private void Awake()
    {
        player = GetComponent<Player>();
        gameStats = FindObjectOfType<GameStats>();
    }

    #region Fire Rate Methods

    public void SetFireRateByLevel()
    {
        // Indicates the current fire rate value per level for the player
        switch (FireRateLevel)
        {
            case 1:
                player.SetFireRate(0.7f);
                break;
            case 2:
                player.SetFireRate(0.5f);
                break;
            case 3:
                player.SetFireRate(0.35f);
                break;
            case 4:
                player.SetFireRate(0.15f);
                break;
            case 5:
                player.SetFireRate(0.05f);
                break;
            default:
                Debug.LogError("Invalid Fire Rate Level");
                break;
        }
    }

    public int GetFireRateUpgradeCost()
    {
        // Returns the cost in points for the next fire rate upgrade
        switch (FireRateLevel)
        {
            case 1:
                return 25;
            case 2:
                return 50;
            case 3:
                return 75;
            case 4:
                return 100;
            default:
                Debug.LogError("Invalid Fire Rate Level for Cost Calculation");
                return int.MaxValue;
        }
    }

    public void UpgradeFireRate()
    {
        if (FireRateLevel < MAX_FIRE_RATE_LEVEL)
        {
            FireRateLevel++;
            SetFireRateByLevel();
            gameStats.SubtractPoints(GetFireRateUpgradeCost());
        }
    }

    #endregion

    #region Multi-Shot Methods

    public void SetMultiShotByLevel()
    {
        // Indicates the current multi-shot projectile count per level for the player
        switch (MultiShotLevel)
        {
            case 1:
                player.SetMultiShot(1);
                break;
            case 2:
                player.SetMultiShot(3);
                break;
            case 3:
                player.SetMultiShot(5);
                break;
            default:
                Debug.LogError("Invalid Multi-Shot Level");
                break;
        }
    }

    public void UpgradeMultiShot()
    {
        if (MultiShotLevel < MAX_MULTI_SHOT_LEVEL)
        {
            MultiShotLevel++;
            SetMultiShotByLevel();
            gameStats.SubtractPoints(GetMultiShotUpgradeCost());
        }
    }

    public int GetMultiShotUpgradeCost()
    {
        // Returns the cost in points for the next multi-shot upgrade
        switch (MultiShotLevel)
        {
            case 1:
                return 75;
            case 2:
                return 200;
            default:
                Debug.LogError("Invalid Multi-Shot Level for Cost Calculation");
                return int.MaxValue;
        }
    }

    #endregion

    #region Max Health Upgrade and Healing Methods

    public int GetMaxHealthUpgradeCost() => 30;
    public int GetHealCost() => 10;

    public void HealToFull()
    {
        player.HealToFull();
        gameStats.SubtractPoints(GetHealCost());
    }

    public void UpgradeMaxHealth()
    {
        if (MaxHealthCapacity < MAX_HEALTH_CAPACITY)
        {
            MaxHealthCapacity++;
            player.SetMaxHealth(MaxHealthCapacity);
            gameStats.SubtractPoints(GetMaxHealthUpgradeCost());
        }
    }

    #endregion

    #region

    public int GetMaxFireRateLevel() => MAX_FIRE_RATE_LEVEL;
    public int GetMaxMultiShotLevel() => MAX_MULTI_SHOT_LEVEL;
    public int GetMaxHealthCapacity() => MAX_HEALTH_CAPACITY;

    #endregion
}
