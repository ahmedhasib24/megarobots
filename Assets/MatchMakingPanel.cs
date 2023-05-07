using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchMakingPanel : MonoBehaviour
{
    public Image imgPlayerAvater, imgOpponentAvater;
    public Text txtPlayerName, txtOpponentName;
    public Text txtPlayerRobotCount, txtOpponentRobotCount;
    public Button btnReturn;

    private bool isOpponentConnected = false;

    public List<Sprite> AvaterSprites;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        btnReturn.interactable = true;
        OptionManager.instance.StopMusic();
        imgPlayerAvater.sprite = AvaterSprites[GameManage.User.avatar_id];
        txtPlayerName.text = GameManage.User.username;
        txtPlayerRobotCount.text = "Robot Count : " + GameManage.instance.myTeam.Count;

        StartCoroutine(OpponentImageAnimation());
    }

    IEnumerator OpponentImageAnimation()
    {
        int avaterId = 0;
        txtOpponentRobotCount.text = "";
        while (isOpponentConnected == false)
        {
            imgOpponentAvater.sprite = AvaterSprites[avaterId];
            txtOpponentName.text = "Searching...";
            avaterId++;
            if (avaterId == AvaterSprites.Count)
            {
                avaterId = 0;
            }
            yield return new WaitForSeconds(0.5f);
            if (PhotonNetwork.room.PlayerCount > 1)
            {
                isOpponentConnected = true;
            }
        }
        PhotonPlayer player = null;
        btnReturn.interactable = false;

        player = PhotonNetwork.player.GetNext();

        yield return new WaitForSeconds(1f);
        imgOpponentAvater.sprite = AvaterSprites[PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.AvaterID)];
        txtOpponentName.text = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Name);
        txtOpponentRobotCount.text = "Robot Count : " + PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.RobotCount);
    }

    public void OnReturnButtonClick()
    {
        StopAllCoroutines();
        PlayManage.instance.playcs.ReturnToMenu();
    }
}
