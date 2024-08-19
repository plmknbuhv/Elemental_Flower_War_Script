using System;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeEnum
{
    ShootSpeedIncrease,
    DamageIncrease,
    RangeIncrease,
    IncreaseBulletCount,
    MoreExp,
    GuidedBullet,
    SpeedIncreaseExp,
}   

public class FlowerUpgrade : MonoBehaviour
{
    public Dictionary<UpgradeEnum, int> UpgradeDictionary = new Dictionary<UpgradeEnum, int>();
    public Dictionary<UpgradeEnum, bool> UpgradeCanDictionary = new Dictionary<UpgradeEnum, bool>();

    private Flower _flower;

    private void Awake()
    {
        foreach (UpgradeEnum item in System.Enum.GetValues(typeof(UpgradeEnum)))
        {
            UpgradeCanDictionary.Add(item, true);
            UpgradeDictionary.Add(item, 0);
        }
        _flower = GetComponent<Flower>();
    }

    public bool CheckUpgrade(UpgradeEnum upgradeEnum, bool onlyCheck = false)
    {
        int currentLevel = UpgradeDictionary[upgradeEnum];
        int maxLevel = FlowerManager.Instance.maxLevelDictionary[upgradeEnum];
        
        if(onlyCheck) return currentLevel < maxLevel? true: false;

        if (currentLevel < maxLevel)
        {
            UpgradeDictionary[upgradeEnum]++;
            Upgrade(upgradeEnum);
            return true;
        }

       
        return false;
    }

    private void Upgrade(UpgradeEnum upgradeEnum)
    {
        switch (upgradeEnum)
        {
            case UpgradeEnum.ShootSpeedIncrease:
                _flower.attackCooldown -= 0.2f;
                break;
            case UpgradeEnum.DamageIncrease:
                _flower.attackDamage += 5;
                break;
            case UpgradeEnum.RangeIncrease:
                _flower.detectRadius += 0.4f;
                break;
            case UpgradeEnum.IncreaseBulletCount:
                _flower.bulletCount += 2;
                break;
            case UpgradeEnum.MoreExp:
                _flower.expAmount++;
                break;
            case UpgradeEnum.GuidedBullet:
                _flower.isGuidedBullet = true;
                break;
            case UpgradeEnum.SpeedIncreaseExp:
                _flower.expGetTime -= 0.5f;
                break;
        }

        if (UpgradeDictionary[upgradeEnum] == FlowerManager.Instance.maxLevelDictionary[upgradeEnum])
        {
            UpgradeCanDictionary[upgradeEnum] = false;
        }
    }
}
