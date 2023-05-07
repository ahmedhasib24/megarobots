using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine.SceneManagement;
using System;

public class MenuPhotonNetworkManager : PunBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static MenuPhotonNetworkManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static MenuPhotonNetworkManager instance {
        get {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first MenuPhotonNetworkManager object in the scene.
                s_Instance = FindObjectOfType(typeof(MenuPhotonNetworkManager)) as MenuPhotonNetworkManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate an MenuPhotonNetworkManager object. \n You have to have exactly one MenuPhotonNetworkManager in the scene.");
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

    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        Connect();
    }

    public void Connect() 
    {
        if (!PhotonNetwork.connectedAndReady)
        {
            LoadingPanel.instance.totalTask++;
            LoadingPanel.instance.Show();
            PhotonNetwork.ConnectUsingSettings("v1.0");
        }
    }
    public void Disconnect() { PhotonNetwork.Disconnect(); }

    public override void OnConnectedToPhoton()
    {
        Photon_Events.FirePhotonNetworkConnectedEvent(true);
    }

    #region Photon Connections
    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        if (LoadingPanel.instance.totalTask > LoadingPanel.instance.taskCompleted)
        {
            LoadingPanel.instance.taskCompleted++;
        }
        Debug.Log("Failed to connect to Photon");
    }

    /// <summary>
    /// Called when something causes the connection to fail (after it was established), followed by a call to OnDisconnectedFromPhoton().
    /// </summary>
    /// <remarks>
    /// If the server could not be reached in the first place, OnFailedToConnectToPhoton is called instead.
    /// The reason for the error is provided as DisconnectCause.
    /// </remarks>
    public override void OnConnectionFail(DisconnectCause cause)
    {
        if (LoadingPanel.instance.totalTask > LoadingPanel.instance.taskCompleted)
        {
            LoadingPanel.instance.taskCompleted++;
        }
        Debug.Log("Photon connection failed");
    }


    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Photon_Events.FireOnFailedCreateRoomEvent(true);
    }

    // When connected to Photon
    public override void OnConnectedToMaster()
    {
        //Photon_Events.FirePhotonNetworkConnectedEvent(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnectedFromPhoton()
    {
        //If the player's in game then quit message
        Debug.Log("Disconnected to Photon");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Photon_Events.FireOnFailedJoinRoomEvent(true);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Photon_Events.FireOnFailedJoinRandomRoomEvent(true);
    }

    // After succesfully joined to the main lobby
    public override void OnJoinedLobby()
    {
        if (LoadingPanel.instance.totalTask > LoadingPanel.instance.taskCompleted)
        {
            LoadingPanel.instance.taskCompleted++;
        }
        Debug.Log("Lobby joined!");
    }

    // if we join (or create) a room, no need for the create button anymore;
    public override void OnJoinedRoom()
    {
        //SetCustomProperties(PhotonNetwork.player, 0, PhotonNetwork.playerList.Length - 1);
        Photon_Events.FireOnJoinedRoomEvent(true);
    }

    // (masterClient only) enables start race button
    public override void OnCreatedRoom()
    {
        //SetCustomProperties(PhotonNetwork.player, 0, PhotonNetwork.playerList.Length - 1);
        Photon_Events.FireOnCreatedRoomEvent(true);
    }

    // If master client, for every newly connected player, sets the custom properties for him
    // car = 0, position = last (size of player list)
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Photon_Events.FireOnPhotonPlayerConnectedEvent(newPlayer);
        //if (PhotonNetwork.isMasterClient)
        //{
        //    //SetCustomProperties(newPlayer, 0, PhotonNetwork.playerList.Length - 1);
        //    Photon_Events.FireOnPhotonPlayerConnectedEvent(newPlayer);
        //    photonView.RPC(PhotonEnums.RPC.ShowConnectedBoxRPC, PhotonTargets.All, newPlayer.NickName);
        //}
    }

    // when a player disconnects from the room, update the spawn/position order for all
    public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer)
    {
        Photon_Events.FireOnPhotonPlayerDisconnectedEvent(disconnetedPlayer);
    }
    #endregion

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            //if (SceneManager.GetActiveScene().name == "03-Main")
            //{
                if (!PhotonNetwork.connectedAndReady)
                {
                    //Connect();
                    //LoadingPanel.instance.totalTask++;
                    //LoadingPanel.instance.Show();
                    //PhotonNetwork.Reconnect();
                    SceneManager.LoadScene("01-Splash");
                }
            //}
        }
    }
}
