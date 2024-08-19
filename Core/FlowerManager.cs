using System;
using System.Collections.Generic;

public class FlowerManager : MonoSingleton<FlowerManager>
{
    public FlowerElementDataListSO flowerElementDataListSo;
    public Dictionary<UpgradeEnum, int> maxLevelDictionary = new Dictionary<UpgradeEnum, int>();
    public Dictionary<UpgradeEnum, bool> UpgradeCanDictionary = new Dictionary<UpgradeEnum, bool>();
    public List<Flower> flowerList = new List<Flower>();
    public event Action knockBackEvent;

    private void Awake()
    {
        maxLevelDictionary.Add(UpgradeEnum.ShootSpeedIncrease, 3);
        maxLevelDictionary.Add(UpgradeEnum.DamageIncrease, 4);
        maxLevelDictionary.Add(UpgradeEnum.RangeIncrease, 2);
        maxLevelDictionary.Add(UpgradeEnum.IncreaseBulletCount, 3);
        maxLevelDictionary.Add(UpgradeEnum.MoreExp, 4); // 경험치 많이 주는거
        maxLevelDictionary.Add(UpgradeEnum.GuidedBullet, 1);
        maxLevelDictionary.Add(UpgradeEnum.SpeedIncreaseExp, 4); // 경험치 빨리 주는거
        
        foreach (UpgradeEnum item in Enum.GetValues(typeof(UpgradeEnum)))
        {
            UpgradeCanDictionary.Add(item, false);
        }
    }

    public Dictionary<UpgradeEnum, bool> CheckCanUpgrade()
    {
        foreach (UpgradeEnum item in Enum.GetValues(typeof(UpgradeEnum)))
        {
            UpgradeCanDictionary[item] = false;
        }
        
        foreach (var flower in flowerList)
        {
            foreach (UpgradeEnum item in Enum.GetValues(typeof(UpgradeEnum)))
            {
                if (flower.FlowerUpgrade.UpgradeCanDictionary[item] == true)
                {
                    UpgradeCanDictionary[item] = true;
                }
            }
        }

        return UpgradeCanDictionary;
    }

    public void StopAim()
    {
        knockBackEvent?.Invoke();
    }
}
