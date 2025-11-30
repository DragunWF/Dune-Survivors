using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Player))]
public class PlayerUpgrades : MonoBehaviour
{
    #region Upgrade Limits

    private const int MAX_FIRE_RATE_LEVEL = 8;
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
    private WaveController waveController ;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        gameStats = GameStats.Instance;
        waveController  = FindObjectOfType<WaveController>();
    }

    #region Fire Rate Methods

    public void SetFireRateByLevel()
    {
        // Indicates the current fire rate value per level for the player
        switch (FireRateLevel)
        {
            case 1:
                player.SetFireRate(0.9f);
                break;
            case 2:
                player.SetFireRate(0.85f);
                break;
            case 3:
                player.SetFireRate(0.725f);
                break;
            case 4:
                player.SetFireRate(0.6f);
                break;
            case 5:
                player.SetFireRate(0.5f);
                break;
            case 6:
                player.SetFireRate(0.4f);
                break;
            case 7:
                player.SetFireRate(0.3f);
                break;
            case 8:
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
            1 => 25,
            2 => 60,
            3 => 100,
            4 => 400,
            5 => 800,
            6 => 1000,
            7 => 5000,
            _ => 0,
        };
    }

    public void UpgradeFireRate()
    {
        if (FireRateLevel < MAX_FIRE_RATE_LEVEL)
        {
            gameStats.SubtractPoints(GetFireRateUpgradeCost());
            FireRateLevel++;
            SetFireRateByLevel();
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
            gameStats.SubtractPoints(GetMultiShotUpgradeCost());
            MultiShotLevel++;
            SetMultiShotByLevel();
        }
    }

    public int GetMultiShotUpgradeCost()
    {
        // Returns the cost in points for the next multi-shot upgrade
        return MultiShotLevel switch
        {
            1 => 250,
            2 => 700,
            3 => 1200,
            4 => 3000,
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
    public int GetHealCost()
    {
        int wave = waveController.CurrentWaveNumber;

        return wave switch
        {
        1 => 10,
        2 => 10,
        3 => 20,
        4 => 50,
        5 => 100,
        6 => 100,
        7 => 200,
        8 => 500,
        9 => 500,
        _ => 500 
        };
    }

    public void HealToFull()
    {
        player.HealToFull();
        gameStats.SubtractPoints(GetHealCost());
    }

    public void UpgradeMaxHealth()
    {
        if (MaxHealthCapacity < MAX_HEALTH_CAPACITY)
        {
            gameStats.SubtractPoints(GetMaxHealthUpgradeCost());
            MaxHealthCapacity++;
            player.SetMaxHealth(MaxHealthCapacity);
        }
    }

    #endregion

    #region

    public int GetMaxFireRateLevel() => MAX_FIRE_RATE_LEVEL;
    public int GetMaxMultiShotLevel() => MAX_MULTI_SHOT_LEVEL;
    public int GetMaxHealthCapacity() => MAX_HEALTH_CAPACITY;

    #endregion
}
