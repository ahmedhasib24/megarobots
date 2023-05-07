using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamInventoryRobotItem : MonoBehaviour
{
    public Text txtName, txtLevel;
    //public Image imgLifeProgress, imgAttackProgress, imgDefenceProgress, imgVelocityProgress;
    //public Slider sliderLifeProgress, sliderAttackProgress, sliderDefenceProgress, sliderVelocityProgress;
    //public Text txtLifeProgress, txtAttackProgress, txtDefenceProgress, txtVelocityProgress;

    public Image imgBG;

    public Robot robot = new Robot();
    public int id;

    private MyTeamSubPanel mySubPanel;

    public void Init(int id, Robot robot, MyTeamSubPanel mySubPanel)
    {
        this.mySubPanel = mySubPanel;
        this.id = id;

        //this.robot = robot;
        //this.robot.Head = GameManage.AllHeads.Find(x => x.object_id == this.robot.head_object_id);
        //this.robot.Head.level = 1;
        //this.robot.LeftArm = GameManage.AllLeftArms.Find(x => x.object_id == this.robot.larm_object_id);
        //this.robot.LeftArm.level = 1;
        //this.robot.RightArm = GameManage.AllRightArms.Find(x => x.object_id == this.robot.rarm_object_id);
        //this.robot.RightArm.level = 1;
        //this.robot.Leg = GameManage.AllLegs.Find(x => x.object_id == this.robot.leg_object_id);
        //this.robot.Leg.level = 1;

        txtName.text = this.robot.name;

        txtLevel.text = this.robot.Level.ToString();

        DeselectItem();
    }

    public void SelectItem()
    {
        if (mySubPanel)
        {
            imgBG.sprite = mySubPanel.memberItemSelectedSprite;
        }
    }

    public void DeselectItem()
    {
        if (mySubPanel)
        {
            imgBG.sprite = mySubPanel.memberItemDeselectedSprite;
        }
    }

    public void OnClick()
    {
        if(mySubPanel)
        {
            mySubPanel.SelectItem(id, true);
        }
    }
}
