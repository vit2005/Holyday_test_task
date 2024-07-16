using UnityEngine;
using UnityEngine.UI;

public class GoldController
{
    private const int startingBananaGold = 1;

    private int currentGold;
    public int CurrentGold {
        get { return currentGold; }
        private set
        {
            currentGold = value;

            GameObject.Find(GameController.menusPath + "upgrades/upgrade (0)/upgradeButton").GetComponent<Button>().interactable =
                currentGold >= GameEntities.Upgrades.GetUpgradeCost(Upgrade.BananaGold);
            GameObject.Find(GameController.menusPath + "upgrades/upgrade (1)/upgradeButton").GetComponent<Button>().interactable =
                currentGold >= GameEntities.Upgrades.GetUpgradeCost(Upgrade.BananaSpawnTime);
        } 
    }

    public void SetGold(int Gold) {
        CurrentGold = Gold;
    }

    public void AddGold() {
        int goldGained = startingBananaGold + (int)GameEntities.Upgrades.GetUpgradeEffect(Upgrade.BananaGold);
        CurrentGold += goldGained;
    }

    public void RemoveGold(int Gold) {
        if (Gold > 0) { 
            CurrentGold -= Gold;
        }
    }
}
