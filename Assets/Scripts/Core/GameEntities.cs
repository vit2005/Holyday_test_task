using UnityEngine;

public class GameEntities {

    private static GameController gameController;
    public static GameController GameController {
        get {
            if (gameController == null) {
                gameController = GameObject.Find("GameController").GetComponent<GameController>();
            }
            return gameController;
        }
    }

    private static SocketConnection socketConnection;
    public static SocketConnection SocketConnection {
        get {
            if (socketConnection == null) {
                socketConnection = new SocketConnection();
            }
            return socketConnection;
        }
    }

    private static GoldController goldController;
    public static GoldController GoldController {
        get {
            if (goldController == null) {
                goldController = new GoldController();
            }
            return goldController;
        }
    }

    private static BananaController bananaController;
    public static BananaController BananaController {
        get {
            if (bananaController == null) {
                bananaController = new BananaController();
            }
            return bananaController;
        }
    }

    private static Upgrades upgrades;
    public static Upgrades Upgrades {
        get {
            if (upgrades == null) {
                upgrades = new Upgrades();
            }
            return upgrades;
        }
    }

    private static InfoPopup infoPopup;
    public static InfoPopup InfoPopup {
        get {
            if (infoPopup == null) {
                infoPopup = new InfoPopup();
            }
            return infoPopup;
        }
    }
}
