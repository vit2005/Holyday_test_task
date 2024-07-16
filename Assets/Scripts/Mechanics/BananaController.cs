using UnityEngine;

public class BananaController
{
    private const double maxSpawnTime = 4;

    private double spawnTimeLeft;
    public double SpawnTimeLeft {
        get { return spawnTimeLeft; }
        private set
        {
            spawnTimeLeft = value;
        } 
    }

    public void UpdateSpawnTime() {
        SpawnTimeLeft -= Time.deltaTime;
        if (SpawnTimeLeft <= 0) {
            GameEntities.GameController.SpawnBanana();
            SetSpawnTime();
        }
    }

    public void SetSpawnTime() {
        double spawnTime = maxSpawnTime - GameEntities.Upgrades.GetUpgradeEffect(Upgrade.BananaSpawnTime);
        SpawnTimeLeft = spawnTime;
    }
}
