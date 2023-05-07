using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class PhotonMultiplayerManager : PunBehaviour
{
    #region SINGLETON
    private static PhotonMultiplayerManager instance;

    public static PhotonMultiplayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PhotonMultiplayerManager>() as PhotonMultiplayerManager;
            }
            return instance;
        }
    }
    #endregion

    bool bRobotsAssigned = false;
    private Coroutine checkPlayersRoutine;
    PlayCS playcs;
    public float attackSelectionTime = 25f;

    float currentAttackSelectionTime = 0;

    #region Unity_Callbacks
    private void OnEnable()
    {
        Photon_Events.onPhotonPlayerConnected += OnPlayerConnected;
        Photon_Events.onPhotonPlayerDisconnected += OnPlayerDisconnected;
    }

    private void OnDisable()
    {
        Photon_Events.onPhotonPlayerConnected -= OnPlayerConnected;
        Photon_Events.onPhotonPlayerDisconnected -= OnPlayerDisconnected;
    }

    private void Start()
    {
        if (!PlayManage.instance.isMultiplayer)
        {
            return;
        }
        playcs = FindObjectOfType<PlayCS>();
        //Disable all ui panels
        playcs.ShowSubPanel(-1);
        //Initialize Robots
        StartCoroutine(InitializeRoutine());
        //playcs.ShowSubPanel(7);

        //Check for opponents
    }
    #endregion

    IEnumerator InitializeRoutine()
    {
        playcs.ShowSubPanel(8);
        yield return new WaitForSeconds(1);
        PlayManage.instance.MakeMineRobots();
        yield return new WaitForSeconds(1);
        PrepareGame();
    }

    public void PrepareGame()
    {
        if (PhotonNetwork.room != null)
        {
            Debug.Log("Preparing Game!");


            if (PhotonNetwork.isMasterClient)
            {
                Debug.Log("Master client: " + PhotonNetwork.player.UserId);
                PhotonUtility.SetRoomProperties(PhotonEnums.Room.MasterClientID, PhotonNetwork.player.UserId);
                if (checkPlayersRoutine != null)
                    StopCoroutine(checkPlayersRoutine);

                checkPlayersRoutine = StartCoroutine(CheckPlayers());

            }

            PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.JoinedGame, true);
        }
    }

    protected IEnumerator CheckPlayers()
    {
        if (PhotonNetwork.room == null)
            yield break;

        //Check the game status
        yield return new WaitForSeconds(1.0f);

        //Comment to debug single player mode
        #region Real-time Multiplayer mode

        //Get the player count who's ready
        int readycount = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            bool bReady = PhotonUtility.GetPlayerProperties<bool>(player, PhotonEnums.Player.JoinedGame);
            if (bReady)
                readycount++;
        }

        if (readycount < 2)
        {
            //playcs.ShowSubPanel(7);
            //ResetPlayerRoundProperties();
        }

        while (PhotonNetwork.room != null && readycount < 2)
        {
            yield return new WaitForSeconds(1.0f);

            readycount = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                bool bReady = PhotonUtility.GetPlayerProperties<bool>(player, PhotonEnums.Player.JoinedGame);

                if (bReady)
                    readycount++;
            }

            if (readycount > 1)
            {
                Debug.Log("Ready Count : " + readycount);
                yield return new WaitForSeconds(0.5f);
            }
        }
        #endregion

        //Now start the game when atleast two players are connected
        if (PhotonNetwork.room != null)
        {
            yield return new WaitForSeconds(3.0f);
            playcs.ShowSubPanel(-1);
            OptionManager.instance.PlayMultiPlayerBG();
            //yield return new WaitForSeconds(2.0f);
            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("PrepareRound", PhotonTargets.All);
            }
        }
    }

    public void WaitForEndRace()
    {
        StartCoroutine(WaitForEndRaceRoutine());
    }

    private IEnumerator WaitForEndRaceRoutine()
    {
        if (PhotonNetwork.room == null)
            yield break;

        //Get the player count who's ready
        int raceCompleteCount = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            bool bComplete = PhotonUtility.GetPlayerProperties<bool>(player, PhotonEnums.Player.RaceComplete);
            if (bComplete)
                raceCompleteCount++;
        }

        while (PhotonNetwork.room != null && raceCompleteCount < 2)
        {
            yield return new WaitForSeconds(1.0f);

            raceCompleteCount = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                bool bComplete = PhotonUtility.GetPlayerProperties<bool>(player, PhotonEnums.Player.RaceComplete);

                if (bComplete)
                    raceCompleteCount++;
            }

            if (raceCompleteCount > 1)
            {
                Debug.Log("RaceComplete Count : " + raceCompleteCount);
                yield return new WaitForSeconds(0.5f);

                
            }
        }

        //Now start attack order selecting for all connected players
        if (PhotonNetwork.room != null)
        {
            yield return new WaitForSeconds(1.0f);

            if (!PhotonNetwork.isMasterClient)
            {
                PlayManage.instance.attackRobotOrder.Clear();
                string attackRobotOrderString = PhotonUtility.GetRoomProperties<string>(PhotonEnums.Room.AttackRobotOrder);
                //Debug.LogError("Attack order: " + attackRobotOrderString);
                for (int i = 0; i < PlayManage.instance.allRobotControllers.Count; i++)
                {
                    for (int j = 0; j < PlayManage.instance.allRobotControllers.Count; j++)
                    {
                        int id = -1;
                        if (int.Parse(attackRobotOrderString.Substring(i, 1)) < PlayManage.instance.MineActiveRobots.Count)
                        {
                            id = int.Parse(attackRobotOrderString.Substring(i, 1)) + PlayManage.instance.MineActiveRobots.Count;
                        }
                        else
                        {
                            id = int.Parse(attackRobotOrderString.Substring(i, 1)) - PlayManage.instance.MineActiveRobots.Count;
                        }
                        if (PlayManage.instance.allRobotControllers[j].id == id)
                        {
                            //Debug.LogError("Adding robot to attack order: " + attackRobotOrderString[i]);
                            PlayManage.instance.attackRobotOrder.Add(PlayManage.instance.allRobotControllers[j]);
                        }
                    }

                }
            }

            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("StartSelectingAttackOrder", PhotonTargets.All);
            }
        }
    }

    public void ResetRound()
    {
        //Debug.LogError("Reseting multiplayer round.");

        //PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.RaceComplete, false);
        PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackSelected, false);
        PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1, "");
        PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2, "");
        PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3, "");

        if (PhotonNetwork.isMasterClient)
        {
            //PhotonUtility.SetRoomProperties(PhotonEnums.Room.AttackRobotOrder, "");
            //photonView.RPC("DetermineAttackOrderAgain", PhotonTargets.All);
            StartCoroutine(DetermineAttackOrderAgainRoutine());
        }
    }

    public void StartAttackSelectionTimer()
    {
        StartCoroutine(AttackSelectionTimerRoutine());
    }

    
    IEnumerator AttackSelectionTimerRoutine()
    {
        currentAttackSelectionTime = attackSelectionTime;
        while (currentAttackSelectionTime > 0)
        {
            playcs.UpdateRadialTimeSlider(currentAttackSelectionTime, attackSelectionTime);
            currentAttackSelectionTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        currentAttackSelectionTime = 0;
        playcs.DeactivateRadialTimeSlider();
    }

    private IEnumerator WaitForAllAttackSelection()
    {
        if (PhotonNetwork.room == null)
            yield break;

        //Get the player count who's ready
        int attackSelected = 0;
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            bool bComplete = PhotonUtility.GetPlayerProperties<bool>(player, PhotonEnums.Player.AttackSelected);
            if (bComplete)
                attackSelected++;
        }

        while (PhotonNetwork.room != null && attackSelected < 2)
        {
            yield return new WaitForSeconds(1.0f);

            attackSelected = 0;
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
            {
                bool bComplete = PhotonUtility.GetPlayerProperties<bool>(player, PhotonEnums.Player.AttackSelected);

                if (bComplete)
                    attackSelected++;
            }

            if (attackSelected == 2)
            {
                Debug.Log("Attack Selected Count : " + attackSelected);
                yield return new WaitForSeconds(0.1f);
            }
        }

        //Now start attack order selecting for all connected players
        if (PhotonNetwork.room != null)
        {
            yield return new WaitForSeconds(0.1f);

            if (PhotonNetwork.isMasterClient)
            {
                photonView.RPC("StartFight", PhotonTargets.All);
            }
        }
    }

    private IEnumerator DetermineAttackOrderAgainRoutine()
    {
        yield return new WaitForSeconds(2f);
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("DetermineAttackOrderAgain", PhotonTargets.All);
        }
    }

    public void StopAllRoutines()
    {
        StopAllCoroutines();
    }

    #region RPCs
    [PunRPC]
    public void PrepareRound()
    {
        PlayManage.instance.MakeEnemyRobots();
        //yield return new WaitForSeconds(1);
        PlayManage.instance.StartMatchRound();
    }

    [PunRPC]
    public void DetermineAttackOrderAgain()
    {
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("StartSelectingAttackOrder", PhotonTargets.All);
        }
        //PlayManage.instance.DetermineAttackOrder();
        
        //yield return new WaitForSeconds(1);
        //PlayManage.instance.StartMatchRound();
    }

    [PunRPC]
    public void StartSelectingAttackOrder()
    {
        Debug.Log("Selecting attack order");
        PlayManage.instance.StartSelectingAttackTarget();

        if (PhotonNetwork.isMasterClient)
        {
            StartCoroutine(WaitForAllAttackSelection());
        }
    }

    [PunRPC]
    public void StartFight()
    {
        Debug.Log("Starting Fight");
        PlayManage.instance.StartFight();
    }
    #endregion

    #region Photon_Callbacks
    private void OnPlayerConnected(PhotonPlayer player)
    {
        Debug.Log(player.NickName + " connected");
    }

    private void OnPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log(player.NickName + " disconnected");

        if (player.UserId != PhotonNetwork.player.UserId)
        {
            PlayManage.instance.OpponentDisconnected();
        }
        else
        {
            PlayManage.instance.LocalPlayerDisconnected();
        }
    }
    #endregion

    private DateTime pauseTime = DateTime.UtcNow;
    private DateTime focusTime = DateTime.UtcNow;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Application paused");
            pauseTime = DateTime.UtcNow;
        }
        else
        {
            Debug.Log("Application unpaused");
            if (PhotonNetwork.inRoom)
            {
                if (PhotonNetwork.otherPlayers.Length == 0)
                {
                    playcs.ShowSubPanel(-1);
                    PlayManage.instance.OpponentDisconnected();
                }
                else
                {
                    //if (pauseTime.Year != DateTime.UtcNow.Year)
                    //{
                    //    pauseTime = DateTime.UtcNow;
                    //}

                    focusTime = DateTime.UtcNow;

                    TimeSpan ts = focusTime - pauseTime;
                    int pauseTimeInSeconds = (int)ts.TotalSeconds;
                    Debug.Log("Focus time: " + focusTime.ToString());
                    Debug.Log("Pause time: " + pauseTime.ToString());
                    Debug.Log("Pause seconds: " + pauseTimeInSeconds);
                    currentAttackSelectionTime -= pauseTimeInSeconds;
                    PlayManage.instance.currentMatchTime -= pauseTimeInSeconds;
                }
            }
            else
            {
                playcs.ShowSubPanel(-1);
                PlayManage.instance.LocalPlayerDisconnected();
            }
        }
    }
}
