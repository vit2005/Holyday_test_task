using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Upgrades {

    private const int goldIncreaseStep = 1;
    private const double timeDecreaseStep = 0.1;

    public void PerformUpgrade(Upgrade Upgrade)
    {
        if (GameEntities.GoldController.CurrentGold >= GetUpgradeCost(Upgrade) && !CheckIfUpgradeMaxed(Upgrade)) {
            upgradesMade[Upgrade]++;
            GameObject.Find(GameController.menusPath + "upgrades/upgrade (" + (int)Upgrade + ")/upgradeButton/upgradeCost/quantity").GetComponent<TextMeshProUGUI>().text = GetUpgradeCost(Upgrade).ToString();
            GameObject.Find(GameController.menusPath + "upgrades/upgrade (" + (int)Upgrade + ")/upgradeResult/quantity").GetComponent<TextMeshProUGUI>().text = (Upgrade == Upgrade.BananaGold ? "+" : "-") + GameEntities.Upgrades.GetUpgradeEffect(Upgrade);

            GameEntities.Achievements.achievementsMade[Achievement.BananaUpgrades]++;
            GameEntities.GoldController.RemoveGold(upgradeCosts[Upgrade][upgradesMade[Upgrade] - 1]);
            GameEntities.SocketConnection.BananaUpgrade(Upgrade);

            if (CheckIfUpgradeMaxed(Upgrade)) {
                GameObject.Find(GameController.menusPath + "upgrades/upgrade (" + (int)Upgrade + ")/upgradeButton").SetActive(false);
            }
        } 
    }

    public double GetUpgradeEffect(Upgrade Upgrade) {
        
        int upgradesDone = upgradesMade[Upgrade];
        
        switch (Upgrade)
        {
            case Upgrade.BananaGold:
                return upgradesDone * goldIncreaseStep;
            case Upgrade.BananaSpawnTime:
                return upgradesDone * timeDecreaseStep;
        }

        return 0;
    }

    public int GetUpgradeCost(Upgrade Upgrade) {
        if (!CheckIfUpgradeMaxed(Upgrade)) { 
            return upgradeCosts[Upgrade][upgradesMade[Upgrade]];
        }
        else {
            return 0;
        }
    }

    public bool CheckIfUpgradeMaxed(Upgrade Upgrade) {
        return upgradesMade[Upgrade] >= upgradeCosts[Upgrade].Count;
    }

    public Dictionary<Upgrade, int> upgradesMade = new Dictionary<Upgrade, int>() {
        { Upgrade.BananaGold,       0 },
        { Upgrade.BananaSpawnTime,  0 },
    };

    private static readonly List<int> bananaGold = new List<int>()
    {
        10,
        20,
        30,
        40,
        50,
        60,
        70,
        80,
        90,
        100,
        150,
        200,
        250,
        300,
        350,
        400,
        450,
        500,
        600,
        700,
        800,
        900,
        1000,
        1500,
        2000,
        2500,
        3000,
        3500,
        4000,
        4500,
        5000,
        6000,
        7000,
        8000,
        9000,
        10000,
    };

    private static readonly List<int> bananaSpawnTime = new List<int>()
    {
        10,
        20,
        30,
        40,
        50,
        60,
        70,
        80,
        90,
        100,
        150,
        200,
        250,
        300,
        350,
        400,
        450,
        500,
        600,
        700,
        800,
        900,
        1000,
        1500,
        2000,
        2500,
        3000,
        3500,
        4000,
        4500,
        5000,
        6000,
        7000,
        8000,
        9000,
        10000,
    };

    public static readonly Dictionary<Upgrade, List<int>> upgradeCosts = new Dictionary<Upgrade, List<int>>() {
        { Upgrade.BananaGold,       bananaGold },
        { Upgrade.BananaSpawnTime,  bananaSpawnTime },
    };
}

public enum Upgrade { 
    BananaGold,
    BananaSpawnTime
}

[Serializable]
public class UpgradeServerList {
    public List<UpgradeServer> list = new List<UpgradeServer>();
}

[Serializable]
public class UpgradeServer {
    public Upgrade code;
    public int number;
}