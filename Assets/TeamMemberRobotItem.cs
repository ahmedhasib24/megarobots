using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamMemberRobotItem : MonoBehaviour
{
    public Text txtName, txtLevel;
    public Image robotThumb;
    public Image imgLifeProgress, imgAttackProgress, imgDefenceProgress, imgVelocityProgress;
    public Slider sliderLifeProgress, sliderAttackProgress, sliderDefenceProgress, sliderVelocityProgress;
    public Text txtLifeProgress, txtAttackProgress, txtDefenceProgress, txtVelocityProgress;

    public Image imgBG;

    public Robot robot = new Robot();
    public int id;

    private OpponentTeamSubPanel opponentSubPanel;
    private MyTeamSubPanel mySubPanel;
    private MultiplayerTeamSubPanel multiplayerTeamSubPanel;
    private bool isInventory = false;

    public void Init(int id, bool isInventory, Robot robot, OpponentTeamSubPanel opponentSubPanel, MyTeamSubPanel mySubPanel, MultiplayerTeamSubPanel multiplayerTeamSubPanel)
    {
        this.opponentSubPanel = opponentSubPanel;
        this.mySubPanel = mySubPanel;
        this.multiplayerTeamSubPanel = multiplayerTeamSubPanel;
        this.id = id;
        this.isInventory = isInventory;

        this.robot = robot;
        if (opponentSubPanel)
        {
            this.robot.Head = GameManage.AllHeads.Find(x => x.object_id == this.robot.head_object_id);
            this.robot.Head.level = 1;
            //this.robot.Head.life = (this.robot.Head.life / 5) * this.robot.Head.level;
            //this.robot.Head.attack = (this.robot.Head.attack / 5) *this.robot.Head.level;
            //this.robot.Head.defence = (this.robot.Head.defence / 5) *this.robot.Head.level;
            //this.robot.Head.velocity = (this.robot.Head.velocity / 5) *this.robot.Head.level;

            this.robot.LeftArm = GameManage.AllLeftArms.Find(x => x.object_id == this.robot.larm_object_id);
            this.robot.LeftArm.level = 1;
            //this.robot.LeftArm.life = (this.robot.LeftArm.life / 5) *this.robot.LeftArm.level;
            //this.robot.LeftArm.attack = (this.robot.LeftArm.attack / 5) *this.robot.LeftArm.level;
            //this.robot.LeftArm.defence = (this.robot.LeftArm.defence / 5) *this.robot.LeftArm.level;
            //this.robot.LeftArm.velocity = (this.robot.LeftArm.velocity / 5) *this.robot.LeftArm.level;

            this.robot.RightArm = GameManage.AllRightArms.Find(x => x.object_id == this.robot.rarm_object_id);
            this.robot.RightArm.level = 1;
            //this.robot.RightArm.life = (this.robot.RightArm.life / 5) *this.robot.RightArm.level;
            //this.robot.RightArm.attack = (this.robot.RightArm.attack / 5) *this.robot.RightArm.level;
            //this.robot.RightArm.defence = (this.robot.RightArm.defence / 5) *this.robot.RightArm.level;
            //this.robot.RightArm.velocity = (this.robot.RightArm.velocity / 5) *this.robot.RightArm.level;

            this.robot.Leg = GameManage.AllLegs.Find(x => x.object_id == this.robot.leg_object_id);
            this.robot.Leg.level = 1;
            //this.robot.Leg.life = (this.robot.Leg.life / 5) *this.robot.Leg.level;
            //this.robot.Leg.attack = (this.robot.Leg.attack / 5) *this.robot.Leg.level;
            //this.robot.Leg.defence = (this.robot.Leg.defence / 5) *this.robot.Leg.level;
            //this.robot.Leg.velocity = (this.robot.Leg.velocity / 5) *this.robot.Leg.level;
        }

        txtName.text = this.robot.name;

        robotThumb.sprite = GameManage.instance.AllBasicRobotImages[this.robot.Head.head_id];

        txtLevel.text = this.robot.Level.ToString();

        imgLifeProgress.fillAmount = (this.robot.MaxLife() / 5f) / this.robot.MaxLife();
        imgAttackProgress.fillAmount = (this.robot.MaxAttack() / 5f) / this.robot.MaxAttack();
        imgDefenceProgress.fillAmount = (this.robot.MaxDefence() / 5f) / this.robot.MaxDefence();
        imgVelocityProgress.fillAmount = (this.robot.MaxVelocity() / 5f) / this.robot.MaxVelocity();

        //imgLifeProgress.fillAmount = (this.robot.Life) / this.robot.MaxLife();
        //imgAttackProgress.fillAmount = (this.robot.Attack) / this.robot.MaxAttack();
        //imgDefenceProgress.fillAmount = (this.robot.Defence) / this.robot.MaxDefence();
        //imgVelocityProgress.fillAmount = (this.robot.Velocity) / this.robot.MaxVelocity();

        sliderLifeProgress.value = (this.robot.MaxLife() / 5f) / this.robot.MaxLife();
        sliderAttackProgress.value = (this.robot.MaxAttack() / 5f) / this.robot.MaxAttack();
        sliderDefenceProgress.value = (this.robot.MaxDefence() / 5f) / this.robot.MaxDefence();
        sliderVelocityProgress.value = (this.robot.MaxVelocity() / 5f) / this.robot.MaxVelocity();

        txtLifeProgress.text = (this.robot.MaxLife() / 5f).ToString();
        txtAttackProgress.text = (this.robot.MaxAttack() / 5f).ToString();
        txtDefenceProgress.text = (this.robot.MaxDefence() / 5f).ToString();
        txtVelocityProgress.text = (this.robot.MaxVelocity() / 5f).ToString();

        DeselectItem();
    }

    public void SelectItem()
    {
        if (opponentSubPanel)
        {
            imgBG.sprite = opponentSubPanel.itemSelected;
        }
        else if (mySubPanel)
        {
            imgBG.sprite = mySubPanel.memberItemSelectedSprite;
        }
        else if (multiplayerTeamSubPanel)
        {
            imgBG.sprite = multiplayerTeamSubPanel.memberItemSelectedSprite;
        }
    }

    public void DeselectItem()
    {
        if (opponentSubPanel)
        {
            imgBG.sprite = opponentSubPanel.itemDeselected;
        }
        else if (mySubPanel)
        {
            imgBG.sprite = mySubPanel.memberItemDeselectedSprite;
        }
        else if (multiplayerTeamSubPanel)
        {
            imgBG.sprite = multiplayerTeamSubPanel.memberItemDeselectedSprite;
        }
    }

    public void OnClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        if (opponentSubPanel)
        {
            opponentSubPanel.SelectItem(id);
        }
        else if(mySubPanel)
        {
            mySubPanel.SelectItem(id, isInventory);
        }
        else if (multiplayerTeamSubPanel)
        {
            multiplayerTeamSubPanel.SelectItem(id, isInventory);
        }
    }
}
