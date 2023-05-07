using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfileSubPanel : MonoBehaviour
{
    public Image imgUserAvatar;
    public Text txtUserName, txtNoOfRobots, txtNoOfPieces, txtWin, txtLoss;

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        imgUserAvatar.sprite = MenuUIManager.instance.AvatarList[GameManage.User.avatar_id];

        txtUserName.text = GameManage.User.username;
        txtNoOfRobots.text = GameManage.User.Robots.Count.ToString();
        txtNoOfPieces.text = (GameManage.User.Heads.Count + GameManage.User.LeftArms.Count + GameManage.User.RightArms.Count + GameManage.User.Legs.Count).ToString();
        txtWin.text = GameManage.User.win.ToString();
        txtLoss.text = GameManage.User.loss.ToString();
    }

    public void CloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        gameObject.SetActive(false);
    }
}
