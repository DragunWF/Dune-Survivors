using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    #region Fire Rate Methods

    public void GetFireRate()
    {
        // Indicates the current fire rate value per level for the player
        switch (FireRateLevel)
        {
            case 1:
                player.SetFireRate(1.0f);
                break;
            case 2:
                player.SetFireRate(0.8f);
                break;
            case 3:
                player.SetFireRate(0.6f);
                break;
            case 4:
                player.SetFireRate(0.4f);
                break;
            case 5:
                player.SetFireRate(0.2f);
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
            GetFireRate();
        }
    }

    #endregion

    #region Multi-Shot Methods

    public void GetMultiShot()
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
            GetMultiShot();
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
}
