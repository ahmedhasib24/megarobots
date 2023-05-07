using UnityEngine;
using System.Collections;
using Photon;
using UnityEngine.UI;
using HashTable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PhotonRoomInfoManager : PunBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static PhotonRoomInfoManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static PhotonRoomInfoManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PhotonRoomInfoManager object in the scene.
                s_Instance = FindObjectOfType(typeof(PhotonRoomInfoManager)) as PhotonRoomInfoManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate an PhotonRoomInfoManager object. \n You have to have exactly one PhotonRoomInfoManager in the scene.");
            }
            return s_Instance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        s_Instance = null;
    }
    #endregion

    public Hashtable dataTable = new Hashtable();
    private int totalPlayers = 0;
    private int totalRooms = 0;

    private void OnEnable()
    {
        //Photon_Events.onPhotonNetworkConnected += onPhotonNetworkConnected;
        //Photon_Events.onJoinedLobby += onJoinedLobby;
        Photon_Events.onJoinedRoom += onCreatedRoom;
        Photon_Events.onFailedJoinRandomRoom += onFailedJoinRandomRoom;
    }

    private void OnDisable()
    {
        //Photon_Events.onPhotonNetworkConnected -= onPhotonNetworkConnected;
        //Photon_Events.onJoinedLobby -= onJoinedLobby;
        Photon_Events.onJoinedRoom -= onCreatedRoom;
        Photon_Events.onFailedJoinRandomRoom -= onFailedJoinRandomRoom;
    }

    public void JoinOrCreateRoom()
    {
        //if (!PhotonNetwork.connectedAndReady)
        //{
        //    LoadingPanel.instance.totalTask++;
        //    LoadingPanel.instance.Show();
        //    PhotonNetwork.ConnectToBestCloudServer(Application.version);
        //}
        //else if (!PhotonNetwork.insideLobby)
        //{
        //    LoadingPanel.instance.totalTask++;
        //    LoadingPanel.instance.Show();
        //    PhotonNetwork.JoinLobby();
        //}
        //else
        //{
            
        //}

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.PublishUserId = true;

        //Search and join any dice room
        string[] strRoomProperties = { "robot_count" };
        roomOptions.CustomRoomPropertiesForLobby = strRoomProperties;

        //Add global minimum bet amount
        HashTable roomData = new HashTable();
        roomData.Clear();

        //Also add the locale information for the room not to mess up with other players from the world
        roomData[PhotonEnums.Room.AttackRobotOrder] = "";
        roomData[PhotonEnums.Room.RobotCount] = GameManage.instance.myTeam.Count;
        roomOptions.CustomRoomProperties = roomData;

        string curRoomName = "Robot" + 100.ToString();
        PhotonNetwork.JoinOrCreateRoom(curRoomName, roomOptions, TypedLobby.Default);
    }

    /////////////////////
    //Photon callbacks
    /////////////////////
    
    private void onPhotonNetworkConnected(bool val)
    {
        LoadingPanel.instance.taskCompleted++;
        PhotonNetwork.JoinLobby();
    }

    private void onJoinedLobby(bool val)
    {
        LoadingPanel.instance.taskCompleted++;
        JoinOrCreateRoom();
    }

    private void onFailedJoinRandomRoom(bool val)
    {
        Debug.Log("Joining random failed! Possible of no room exist and now creating a new room");

        //Now create a new room 
        //Create a new room with the previously defined room options
        JoinOrCreateRoom();
    }

    private void onCreatedRoom(bool val)
    {
        Debug.Log("On Room Created");

        if (PhotonNetwork.player != null)
        {
            ClearPlayerProperties(PhotonNetwork.player);
            ResetPlayerProperties(PhotonNetwork.player);
            SetRobotProperties(PhotonNetwork.player);
        }

        //Default timeout is 15 seconds from now
        PhotonNetwork.BackgroundTimeout = 20;

        StartCoroutine(LoadGame());
    }

    public virtual void ClearPlayerProperties(PhotonPlayer player)
    {
        //Need to set null to each entry to reset player's custom properties
        ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
        List<string> keys = new List<string>();

        foreach (DictionaryEntry entry in properties)
        {
            string strKey = entry.Key.ToString();
            keys.Add(strKey);
        }

        foreach (string strKey in keys)
        {
            properties[strKey] = null;
        }

        player.SetCustomProperties(properties);
    }

    public virtual void ResetPlayerProperties(PhotonPlayer player)
    {
        Debug.Log("ResetPlayerProperties");
        ExitGames.Client.Photon.Hashtable properties = player.CustomProperties;
        properties[PhotonEnums.Player.AvaterID] = 0;
        properties[PhotonEnums.Player.Name] = "";
        properties[PhotonEnums.Player.JoinedGame] = false;
        //properties[PhotonEnums.Player.Money] = (long)PlayerUtility.GetPlayerCreditsLeft();
        properties[PhotonEnums.Player.Scores] = 0;
        properties[PhotonEnums.Player.RaceComplete] = false;
        properties[PhotonEnums.Player.PictureURL] = "";
        properties[PhotonEnums.Player.FacebookID] = "";
        properties[PhotonEnums.Player.UserID] = "";
        properties[PhotonEnums.Player.Role] = "";
        properties[PhotonEnums.Player.PlayerID] = "";
        properties[PhotonEnums.Player.AttackSelected] = false;
        properties[PhotonEnums.Player.RobotCount] = 0;

        properties[PhotonEnums.Player.Robot1] = "";
        properties[PhotonEnums.Player.Robot2] = "";
        properties[PhotonEnums.Player.Robot3] = "";

        properties[PhotonEnums.Player.AttackTargetString1] = "";
        properties[PhotonEnums.Player.AttackTargetString2] = "";
        properties[PhotonEnums.Player.AttackTargetString3] = "";

        properties[PhotonEnums.Player.Head1] = "";
        properties[PhotonEnums.Player.Head2] = "";
        properties[PhotonEnums.Player.Head3] = "";

        properties[PhotonEnums.Player.LeftArm1] = "";
        properties[PhotonEnums.Player.LeftArm2] = "";
        properties[PhotonEnums.Player.LeftArm3] = "";

        properties[PhotonEnums.Player.RightArm1] = "";
        properties[PhotonEnums.Player.RightArm2] = "";
        properties[PhotonEnums.Player.RightArm3] = "";

        properties[PhotonEnums.Player.Leg1] = "";
        properties[PhotonEnums.Player.Leg2] = "";
        properties[PhotonEnums.Player.Leg3] = "";

        player.SetCustomProperties(properties);
    }

    public virtual void SetRobotProperties(PhotonPlayer player)
    {
        PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.AvaterID, GameManage.User.avatar_id);
        PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Name, GameManage.User.username);
        PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.RobotCount, GameManage.instance.myTeam.Count);

        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Head1, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Head2, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Head3, UnityEngine.Random.Range(0, 4));

        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.LeftArm1, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.LeftArm2, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.LeftArm3, UnityEngine.Random.Range(0, 4));

        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.RightArm1, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.RightArm2, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.RightArm3, UnityEngine.Random.Range(0, 4));

        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Leg1, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Leg2, UnityEngine.Random.Range(0, 4));
        //PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Leg3, UnityEngine.Random.Range(0, 4));

        if (GameManage.instance.myTeam.Count > 0)
        {
            Robot robot = GameManage.instance.myTeam[0];
            RobotBluePrint rbp = new RobotBluePrint();
            rbp.object_id = robot.object_id;
            rbp.type = robot.type;
            rbp.name = robot.name;
            rbp.price = robot.price;
            rbp.head_object_id = robot.head_object_id;
            rbp.larm_object_id = robot.larm_object_id;
            rbp.rarm_object_id = robot.rarm_object_id;
            rbp.leg_object_id = robot.leg_object_id;
            rbp.onSale = robot.onSale;
            rbp.salePrice = robot.salePrice;

            rbp.head = robot.Head;
            rbp.larm = robot.LeftArm;
            rbp.rarm = robot.RightArm;
            rbp.leg = robot.Leg;

            PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Robot1, rbp.toJson());
        }
        if (GameManage.instance.myTeam.Count > 1)
        {
            Robot robot = GameManage.instance.myTeam[1];
            RobotBluePrint rbp = new RobotBluePrint();
            rbp.object_id = robot.object_id;
            rbp.type = robot.type;
            rbp.name = robot.name;
            rbp.price = robot.price;
            rbp.head_object_id = robot.head_object_id;
            rbp.larm_object_id = robot.larm_object_id;
            rbp.rarm_object_id = robot.rarm_object_id;
            rbp.leg_object_id = robot.leg_object_id;
            rbp.onSale = robot.onSale;
            rbp.salePrice = robot.salePrice;

            rbp.head = robot.Head;
            rbp.larm = robot.LeftArm;
            rbp.rarm = robot.RightArm;
            rbp.leg = robot.Leg;

            PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Robot2, rbp.toJson());
        }
        if (GameManage.instance.myTeam.Count > 2)
        {
            Robot robot = GameManage.instance.myTeam[2];
            RobotBluePrint rbp = new RobotBluePrint();
            rbp.object_id = robot.object_id;
            rbp.type = robot.type;
            rbp.name = robot.name;
            rbp.price = robot.price;
            rbp.head_object_id = robot.head_object_id;
            rbp.larm_object_id = robot.larm_object_id;
            rbp.rarm_object_id = robot.rarm_object_id;
            rbp.leg_object_id = robot.leg_object_id;
            rbp.onSale = robot.onSale;
            rbp.salePrice = robot.salePrice;

            rbp.head = robot.Head;
            rbp.larm = robot.LeftArm;
            rbp.rarm = robot.RightArm;
            rbp.leg = robot.Leg;

            PhotonUtility.SetPlayerProperties(player, PhotonEnums.Player.Robot3, rbp.toJson());
        }
    }

    private IEnumerator LoadGame()
    {
        //Now Load the game
        Debug.Log("Loading multiplayer scene");
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(LoadMultiplayerScene());
    }

    IEnumerator LoadMultiplayerScene()
    {
        yield return null;

        LoadingPanel.instance.taskCompleted++;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("05-MultiGamePlay");
        SceneLoadingBar.instance.Show();

        while (!asyncOperation.isDone)
        {
            SceneLoadingBar.instance.SetProgressBar(asyncOperation.progress);
            yield return null;
        }
    }
}
