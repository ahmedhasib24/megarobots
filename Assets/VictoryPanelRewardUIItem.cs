using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanelRewardUIItem : MonoBehaviour
{
    public Image imgItemThumb;
    public Text txtName, txtType, txtLevel;
	

    public void Init(string name, string type, int level, int id)
    {
        txtName.text = name;
        txtType.text = type;
        txtLevel.text = level.ToString();

        if (type.ToUpper() == "ROBOT")
        {
            imgItemThumb.sprite = GameManage.instance.AllBasicRobotImages[id];
        }
        else
        {
            if (type.ToUpper() == "HEAD")
            {
                imgItemThumb.sprite = GameManage.instance.AllHeadPieceImages[id];
            }
            else if (type.ToUpper() == "LEFT ARM")
            {
                imgItemThumb.sprite = GameManage.instance.AllLeftArmPieceImages[id];
            }
            else if (type.ToUpper() == "RIGHT ARM")
            {
                imgItemThumb.sprite = GameManage.instance.AllRightArmPieceImages[id];
            }
            else if (type.ToUpper() == "LEG")
            {
                imgItemThumb.sprite = GameManage.instance.AllLegPieceImages[id];
            }
        }
    }
}
