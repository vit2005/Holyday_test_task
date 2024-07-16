using System;
using System.Collections;
using UnityEngine;

#region Server Function List    
public enum FunctionsSent {
    //user management
    GetUserData,
    ChangeLanguage,
    //game management
    GetGold,
    Upgrade,
    AddAchievementProgress,
}

public enum FunctionsReceived {
    //user management    
    SetUserData,
}
#endregion

public class SocketConnection {

    private bool isConnectedToSocket = false;

    //credentials
    private const string userID = "jLgluRhMym";
    private const string gameID = "HolydayStudiosTest";
    private const string socketUri = "wss://ws-test.holydaygames.org:7351";

    private WebSocket webSocket;

    #region Connect
    /// <summary>
    /// Connects to socket
    /// </summary>
    /// <returns></returns>
    public IEnumerator ConnectToSocket() {

        if (isConnectedToSocket) {
            yield break;
        }
                
        webSocket = new WebSocket(new Uri(socketUri));

        yield return GameEntities.GameController.StartCoroutine(webSocket.Connect());

        if (webSocket.error == null) {

            isConnectedToSocket = true;

            GetUserData(false);

            GameEntities.GameController.StartCoroutine(ReceiveMessages());
        }
        else {
            Debug.Log(webSocket.error);
        }
    }

    /// <summary>
    /// This is function receives all messages that arrive from the socket
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReceiveMessages() {

        while (isConnectedToSocket) {
            try {
                // if connection error, break the loop
                if (webSocket == null || webSocket.error != null) {

                    isConnectedToSocket = false;
                    Debug.Log(webSocket.error);
                    break;
                }
                //if the websocket exists
                if (webSocket != null) {
                    // read message            
                    string message = webSocket.RecvString();
                    // check if message is not empty
                    if (message != null) {
                        HandleSocketData(message);
                    }
                }
            }
            catch (Exception ex) 
            {
                Debug.Log(ex.Message); 
            }

            yield return null;
        }
    }
    #endregion

    #region Handle Socket Incoming Data
    /// <summary>
    /// It handles the data that arrive from socket
    /// </summary>
    /// <param name="message"></param>
    private void HandleSocketData(string message) {
        try {

            SocketData data = JsonHelper.CreateFromJson<SocketData>(message);
            if (data == null) {
                return;
            }

            //USER MANAGEMENT
            if (data.Function == FunctionsReceived.SetUserData.ToString()) {

                if (data.Data == null || data.Data.Length < 2) {
                    ServerErrorMessage(data.Error);
                    return;
                }

                UpgradeServerList upgradesData = JsonHelper.CreateFromJson<UpgradeServerList>(data.Data[0]);
                foreach (UpgradeServer upgradeServer in upgradesData.list)
                {
                    GameEntities.Upgrades.upgradesMade[upgradeServer.code] = upgradeServer.number;
                }

                if (int.TryParse(data.Data[1], out int gold)) {
                    GameEntities.GoldController.SetGold(gold);
                }

                GameEntities.GameController.OnUserDataSet();
            }
        }
        catch (Exception ex) { Debug.Log(ex.Message + "\n" + ex.StackTrace); }
    }
    #endregion

    #region Handle Socket Outgoing Data
    /// <summary>
    /// Used at the start of the game to retrieve user's progress and info
    /// </summary>
    /// <param name="message"></param>
    public void GetUserData(bool WithLanguageAndAchievements = false) {
        SendSocketMessage(ComposeSocketMessage(FunctionsSent.GetUserData.ToString(), WithLanguageAndAchievements.ToString()));
    }

    /// <summary>
    /// Only to be used when the user gets gold from clicking on bananas
    /// </summary>
    /// <param name="message"></param>
    public void GetGold() {
        SendSocketMessage(ComposeSocketMessage(FunctionsSent.GetGold.ToString()));
    }

    /// <summary>
    /// Only to be used when the user does a banana upgrade
    /// </summary>
    /// <param name="message"></param>
    public void BananaUpgrade(Upgrade Upgrade) {
        SendSocketMessage(ComposeSocketMessage(FunctionsSent.Upgrade.ToString(), ((int)Upgrade).ToString()));
    }

    /// <summary>
    /// Send Message to the socket
    /// </summary>
    /// <param name="message"></param>
    private void SendSocketMessage(string message) {
        try {
            if (webSocket != null && isConnectedToSocket) {
                webSocket.SendString(message);
            }
        }
        catch (Exception ex) { Debug.Log(ex.Message); }
    }

    private const string Delimiter = "|==|";

    /// <summary>
    /// Adds delimiter between variables
    /// </summary>
    /// <param name="pars"></param>
    /// <returns></returns>
    public static string ComposeSocketMessage(string FunctionName, params string[] pars) {

        string newParam = FunctionName + Delimiter;

        newParam += userID + Delimiter;
        newParam += gameID + Delimiter;
        foreach(string par in pars) {
            newParam += par + Delimiter;
        }
        newParam = newParam.Remove(newParam.LastIndexOf(Delimiter));
        return newParam;
    }

    /// <summary>
    /// call that when there is an error on the server
    /// </summary>
    private void ServerErrorMessage(string Error) {
        Debug.Log(Error);
    }
    #endregion
}

/// <summary>
/// Socket data is the model that is created when we get an answer from sockets
/// </summary>
public class SocketData {
    public string Function;
    public string SubFunction;
    public string[] Data; // set data of any type for any kind of use
    public string Error;
}