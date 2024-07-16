using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public const string canvasPath = "canvas/";
    public const string menusPath = canvasPath + "menus/";

    [Serializable]
    public class UIData {
        public MenuName menu;
        public GameObject prefab;
        [HideInInspector] public bool hasInitialized;
    }

    public UIData[] menuPrefabs;

    private List<UIData> allUI = new List<UIData>();

    private bool gameInitialised = false;

    private GameObject bananaPrefab;
    private Vector4 bananaSpawnEdges = Vector4.zero;
    private const int spawnPosModifier = 200;

    public void Start() {
        
        foreach (UIData menuPrefab in menuPrefabs) {
            allUI.Add(menuPrefab);
        }

        Vector3 minPosition = Camera.main.ScreenToWorldPoint(new Vector3(spawnPosModifier, spawnPosModifier));
        Vector3 maxPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - spawnPosModifier, Screen.height - spawnPosModifier));

        bananaSpawnEdges = new Vector4(minPosition.x, minPosition.y, maxPosition.x, maxPosition.y);

        StartCoroutine(GameEntities.SocketConnection.ConnectToSocket());
    }

    public void Update() {
        if (gameInitialised) { 

            GameEntities.BananaController.UpdateSpawnTime(); 

            if (Input.GetMouseButtonDown(0))
            {
                //Raycast to see if colliders are hit.
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);

                if (hit.collider) //Something was hit
                {
                    GameObject hitGameObject = hit.collider.gameObject; //banana game object
                    GameEntities.GoldController.AddGold();
                    Destroy(hitGameObject);
                }
            }
        }
    }

    public void OnUserDataSet() {

        GameObject.Find(canvasPath + "infoButton").GetComponent<Button>().onClick.AddListener(() => {
            GameEntities.InfoPopup.OpenPopup();
        });

        GameObject.Find(menusPath + "upgrades/upgrade (0)/upgradeButton").GetComponent<Button>().onClick.AddListener(() => {
            GameEntities.Upgrades.PerformUpgrade(Upgrade.BananaGold);
        });
        GameObject.Find(menusPath + "upgrades/upgrade (0)/upgradeButton").GetComponent<Button>().interactable = GameEntities.GoldController.CurrentGold >= GameEntities.Upgrades.GetUpgradeCost(Upgrade.BananaGold);
        GameObject.Find(menusPath + "upgrades/upgrade (0)/upgradeButton").SetActive(!GameEntities.Upgrades.CheckIfUpgradeMaxed(Upgrade.BananaGold));
        GameObject.Find(menusPath + "upgrades/upgrade (0)/upgradeButton/upgradeCost/quantity").GetComponent<TextMeshProUGUI>().text = GameEntities.Upgrades.GetUpgradeCost(Upgrade.BananaGold).ToString();
        GameObject.Find(menusPath + "upgrades/upgrade (0)/upgradeResult/quantity").GetComponent<TextMeshProUGUI>().text = "+" + GameEntities.Upgrades.GetUpgradeEffect(Upgrade.BananaGold);

        GameObject.Find(menusPath + "upgrades/upgrade (1)/upgradeButton").GetComponent<Button>().onClick.AddListener(() => {
            GameEntities.Upgrades.PerformUpgrade(Upgrade.BananaSpawnTime);
        });
        GameObject.Find(menusPath + "upgrades/upgrade (1)/upgradeButton").GetComponent<Button>().interactable = GameEntities.GoldController.CurrentGold >= GameEntities.Upgrades.GetUpgradeCost(Upgrade.BananaSpawnTime);
        GameObject.Find(menusPath + "upgrades/upgrade (1)/upgradeButton").SetActive(!GameEntities.Upgrades.CheckIfUpgradeMaxed(Upgrade.BananaSpawnTime));
        GameObject.Find(menusPath + "upgrades/upgrade (1)/upgradeButton/upgradeCost/quantity").GetComponent<TextMeshProUGUI>().text = GameEntities.Upgrades.GetUpgradeCost(Upgrade.BananaSpawnTime).ToString();
        GameObject.Find(menusPath + "upgrades/upgrade (1)/upgradeResult/quantity").GetComponent<TextMeshProUGUI>().text = "-" + GameEntities.Upgrades.GetUpgradeEffect(Upgrade.BananaSpawnTime);

        bananaPrefab = transform.Find("banana").gameObject;

        GameEntities.BananaController.SetSpawnTime();

        gameInitialised = true;
    }

    public void SpawnBanana() {
        GameObject newBanana = GameObject.Instantiate(bananaPrefab, new Vector2(UnityEngine.Random.Range(bananaSpawnEdges.x, bananaSpawnEdges.z), UnityEngine.Random.Range(bananaSpawnEdges.y, bananaSpawnEdges.w)), Quaternion.identity, null);
        newBanana.SetActive(true);
    }

    public IEnumerator InitializeMenu(MenuName MenuName, Action Initialize, Action Open, Action OnInitializeComplete = null) {

        UIData menu = allUI.SingleOrDefault(x => x.menu == MenuName);

        if (menu != null) {

            if (!menu.hasInitialized) {

                Initialize?.Invoke();

                yield return null;

                menu.hasInitialized = true;

                OnInitializeComplete?.Invoke();
            }

            Open?.Invoke();
        }
    }

    public bool CheckMenuInitialized(MenuName MenuName) {
        
        UIData menu = allUI.SingleOrDefault(x => x.menu == MenuName);

        if (menu != null) {

            return menu.hasInitialized;
        }

        Debug.LogError("This Menu Does Not Exist");
        
        return false;
    }
}

public enum MenuName {
    InfoPopup = 0,
}