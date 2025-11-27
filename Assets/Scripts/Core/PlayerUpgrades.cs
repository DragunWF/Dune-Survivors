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
    private const int MAX_MULTI_SHOT_LEVEL = 5;
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
                player.SetFireRate(0.75f);
                break;
            case 2:
                player.SetFireRate(0.65f);
                break;
            case 3:
                player.SetFireRate(0.50f);
                break;
            case 4:
                player.SetFireRate(0.325f);
                break;
            case 5:
                player.SetFireRate(0.125f);
                break;
            default:
                break;
        }
    }

    public int GetFireRateUpgradeCost()
    {
        // Returns the cost in points for the next fire rate upgrade
        return FireRateLevel switch
        {
            1 => 50,
            2 => 100,
            3 => 175,
            4 => 250,
            _ => 0,
        };
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
        // One level = 1 projectile
        if (MultiShotLevel > MAX_MULTI_SHOT_LEVEL)
        {
            MultiShotLevel = MAX_MULTI_SHOT_LEVEL;
        }
        player.SetMultiShot(MultiShotLevel);
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
        return MultiShotLevel switch
        {
            1 => 150,
            2 => 300,
            3 => 500,
            4 => 750,
            _ => 0,
        };
    }

    #endregion

    #region Max Health Upgrade and Healing Methods

    public int GetMaxHealthUpgradeCost()
    {
        return MaxHealthCapacity switch
        {
            3 => 50,
            4 => 100,
            _ => 0,
        };
    }
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
