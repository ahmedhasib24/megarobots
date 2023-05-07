using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelupSubPanel : MonoBehaviour
{
    public Text txtTotalLevel;

    public Image imgHeadLife, imgHeadAttack, imgHeadDefence;
    public Slider sliderHeadLife, sliderHeadAttack, sliderHeadDefence;
    public Text txtHeadCurLife, txtHeadCurAttack, txtHeadCurDefence;
    public Text txtHeadNextLife, txtHeadNextAttack, txtHeadNextDefence;
    public Text txtHeadCurLevel, txtHeadNextLevel, txtHeadPrice;
    public Button btnHeadUpgrade;

    public Image imgLeftArmLife, imgLeftArmAttack, imgLeftArmDefence;
    public Slider sliderLeftArmLife, sliderLeftArmAttack, sliderLeftArmDefence;
    public Text txtLeftArmCurLife, txtLeftArmCurAttack, txtLeftArmCurDefence;
    public Text txtLeftArmNextLife, txtLeftArmNextAttack, txtLeftArmNextDefence;
    public Text txtLeftArmCurLevel, txtLeftArmNextLevel, txtLeftArmPrice;
    public Button btnLeftArmUpgrade;

    public Image imgRightArmLife, imgRightArmAttack, imgRightArmDefence;
    public Slider sliderRightArmLife, sliderRightArmAttack, sliderRightArmDefence;
    public Text txtRightArmCurLife, txtRightArmCurAttack, txtRightArmCurDefence;
    public Text txtRightArmNextLife, txtRightArmNextAttack, txtRightArmNextDefence;
    public Text txtRightArmCurLevel, txtRightArmNextLevel, txtRightArmPrice;
    public Button btnRightArmUpgrade;

    public Image imgLegLife, imgLegAttack, imgLegDefence, imgLegVelocity;
    public Slider sliderLegLife, sliderLegAttack, sliderLegDefence, sliderLegVelocity;
    public Text txtLegCurLife, txtLegCurAttack, txtLegCurDefence, txtLegCurVelocity;
    public Text txtLegNextLife, txtLegNextAttack, txtLegNextDefence, txtLegNextVelocity;
    public Text txtLegCurLevel, txtLegNextLevel, txtLegPrice;
    public Button btnLegUpgrade;

    private Robot robot;
    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        robot = Inventory.instance.CurrentRobot;

        txtTotalLevel.text = 5.ToString();

        InitHead();
        InitLeftArm();
        InitRightArm();
        InitLeg();
    }

    private void InitHead()
    {
        if (robot.Head == null)
        {

            btnHeadUpgrade.interactable = false;

            imgHeadLife.fillAmount = 0;
            imgHeadAttack.fillAmount = 0;
            imgHeadDefence.fillAmount = 0;

            sliderHeadLife.value = 0;
            sliderHeadAttack.value = 0;
            sliderHeadDefence.value = 0;

            txtHeadCurLife.text = 0.ToString();
            txtHeadCurAttack.text = 0.ToString();
            txtHeadCurDefence.text = 0.ToString();

            txtHeadNextLife.text = 0.ToString();
            txtHeadNextAttack.text = 0.ToString();
            txtHeadNextDefence.text = 0.ToString();

            txtHeadCurLevel.text = "Lost or Sold";
            txtHeadCurLevel.color = Color.red;
            txtHeadNextLevel.text = 0.ToString();
            txtHeadPrice.text = 0.ToString();
        }
        else if (robot.Head.is_broken == 1)
        {
            btnHeadUpgrade.interactable = false;
            int curLevel = robot.Head.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.Head.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgHeadLife.fillAmount = robot.Head.life / ((robot.Head.life / curLevel) * 5f);
            imgHeadAttack.fillAmount = robot.Head.attack / ((robot.Head.attack / curLevel) * 5f);
            imgHeadDefence.fillAmount = robot.Head.defence / ((robot.Head.defence / curLevel) * 5f);

            sliderHeadLife.value = ((robot.Head.life / curLevel) * nextLevel) / ((robot.Head.life / curLevel) * 5f);
            sliderHeadAttack.value = ((robot.Head.attack / curLevel) * nextLevel) / ((robot.Head.attack / curLevel) * 5f);
            sliderHeadDefence.value = ((robot.Head.defence / curLevel) * nextLevel) / ((robot.Head.defence / curLevel) * 5f);

            txtHeadCurLife.text = robot.Head.life.ToString();
            txtHeadCurAttack.text = robot.Head.attack.ToString();
            txtHeadCurDefence.text = robot.Head.defence.ToString();

            txtHeadNextLife.text = ((robot.Head.life / curLevel) * nextLevel).ToString();
            txtHeadNextAttack.text = ((robot.Head.attack / curLevel) * nextLevel).ToString();
            txtHeadNextDefence.text = ((robot.Head.defence / curLevel) * nextLevel).ToString();

            txtHeadCurLevel.text = "Broken";
            txtHeadCurLevel.color = Color.red;
            txtHeadNextLevel.text = (nextLevel - curLevel).ToString();
            txtHeadPrice.text = 99.ToString();
        }
        else
        {
            int curLevel = robot.Head.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.Head.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgHeadLife.fillAmount = robot.Head.life / ((robot.Head.life / curLevel) * 5f);
            imgHeadAttack.fillAmount = robot.Head.attack / ((robot.Head.attack / curLevel) * 5f);
            imgHeadDefence.fillAmount = robot.Head.defence / ((robot.Head.defence / curLevel) * 5f);

            sliderHeadLife.value = ((robot.Head.life / curLevel) * nextLevel) / ((robot.Head.life / curLevel) * 5f);
            sliderHeadAttack.value = ((robot.Head.attack / curLevel) * nextLevel) / ((robot.Head.attack / curLevel) * 5f);
            sliderHeadDefence.value = ((robot.Head.defence / curLevel) * nextLevel) / ((robot.Head.defence / curLevel) * 5f);

            txtHeadCurLife.text = robot.Head.life.ToString();
            txtHeadCurAttack.text = robot.Head.attack.ToString();
            txtHeadCurDefence.text = robot.Head.defence.ToString();

            txtHeadNextLife.text = ((robot.Head.life / curLevel) * nextLevel).ToString();
            txtHeadNextAttack.text = ((robot.Head.attack / curLevel) * nextLevel).ToString();
            txtHeadNextDefence.text = ((robot.Head.defence / curLevel) * nextLevel).ToString();

            if (robot.onSale == 1)
            {
                txtHeadCurLevel.text = "On Sale";
                txtHeadCurLevel.color = Color.yellow;
                btnHeadUpgrade.interactable = false;
            }
            else
            {
                if (robot.Head.onSale == 0)
                {
                    txtHeadCurLevel.text = "Level : " + curLevel.ToString();
                    txtHeadCurLevel.color = Color.white;
                    if (curLevel < 5 && GameManage.User.gems >= 99)
                    {
                        btnHeadUpgrade.interactable = true;
                    }
                    else
                    {
                        btnHeadUpgrade.interactable = false;
                    }
                }
                else
                {
                    txtHeadCurLevel.text = "On Sale";
                    txtHeadCurLevel.color = Color.yellow;
                    btnHeadUpgrade.interactable = false;
                }
            }

            txtHeadNextLevel.text = (nextLevel - curLevel).ToString();
            txtHeadPrice.text = 99.ToString();
        }
    }

    private void InitLeftArm()
    {
        if (robot.LeftArm == null)
        {
            btnLeftArmUpgrade.interactable = false;

            imgLeftArmLife.fillAmount = 0;
            imgLeftArmAttack.fillAmount = 0;
            imgLeftArmDefence.fillAmount = 0;

            sliderLeftArmLife.value = 0;
            sliderLeftArmAttack.value = 0;
            sliderLeftArmDefence.value = 0;

            txtLeftArmCurLife.text = 0.ToString();
            txtLeftArmCurAttack.text = 0.ToString();
            txtLeftArmCurDefence.text = 0.ToString();

            txtLeftArmNextLife.text = 0.ToString();
            txtLeftArmNextAttack.text = 0.ToString();
            txtLeftArmNextDefence.text = 0.ToString();

            txtLeftArmCurLevel.text = "Lost or Sold";
            txtLeftArmCurLevel.color = Color.red;
            txtLeftArmNextLevel.text = 0.ToString();
            txtLeftArmPrice.text = 0.ToString();
        }
        else if (robot.LeftArm.is_broken == 1)
        {
            btnLeftArmUpgrade.interactable = false;
            int curLevel = robot.LeftArm.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.LeftArm.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgLeftArmLife.fillAmount = robot.LeftArm.life / ((robot.LeftArm.life / curLevel) * 5f);
            imgLeftArmAttack.fillAmount = robot.LeftArm.attack / ((robot.LeftArm.attack / curLevel) * 5f);
            imgLeftArmDefence.fillAmount = robot.LeftArm.defence / ((robot.LeftArm.defence / curLevel) * 5f);

            sliderLeftArmLife.value = ((robot.LeftArm.life / curLevel) * nextLevel) / ((robot.LeftArm.life / curLevel) * 5f);
            sliderLeftArmAttack.value = ((robot.LeftArm.attack / curLevel) * nextLevel) / ((robot.LeftArm.attack / curLevel) * 5f);
            sliderLeftArmDefence.value = ((robot.LeftArm.defence / curLevel) * nextLevel) / ((robot.LeftArm.defence / curLevel) * 5f);

            txtLeftArmCurLife.text = robot.LeftArm.life.ToString();
            txtLeftArmCurAttack.text = robot.LeftArm.attack.ToString();
            txtLeftArmCurDefence.text = robot.LeftArm.defence.ToString();

            txtLeftArmNextLife.text = ((robot.LeftArm.life / curLevel) * nextLevel).ToString();
            txtLeftArmNextAttack.text = ((robot.LeftArm.attack / curLevel) * nextLevel).ToString();
            txtLeftArmNextDefence.text = ((robot.LeftArm.defence / curLevel) * nextLevel).ToString();

            txtLeftArmCurLevel.text = "Broken";
            txtLeftArmCurLevel.color = Color.red;
            txtLeftArmNextLevel.text = (nextLevel - curLevel).ToString();
            txtLeftArmPrice.text = 100.ToString();
        }
        else
        {
            int curLevel = robot.LeftArm.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.LeftArm.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgLeftArmLife.fillAmount = robot.LeftArm.life / ((robot.LeftArm.life / curLevel) * 5f);
            imgLeftArmAttack.fillAmount = robot.LeftArm.attack / ((robot.LeftArm.attack / curLevel) * 5f);
            imgLeftArmDefence.fillAmount = robot.LeftArm.defence / ((robot.LeftArm.defence / curLevel) * 5f);

            sliderLeftArmLife.value = ((robot.LeftArm.life / curLevel) * nextLevel) / ((robot.LeftArm.life / curLevel) * 5f);
            sliderLeftArmAttack.value = ((robot.LeftArm.attack / curLevel) * nextLevel) / ((robot.LeftArm.attack / curLevel) * 5f);
            sliderLeftArmDefence.value = ((robot.LeftArm.defence / curLevel) * nextLevel) / ((robot.LeftArm.defence / curLevel) * 5f);

            txtLeftArmCurLife.text = robot.LeftArm.life.ToString();
            txtLeftArmCurAttack.text = robot.LeftArm.attack.ToString();
            txtLeftArmCurDefence.text = robot.LeftArm.defence.ToString();

            txtLeftArmNextLife.text = ((robot.LeftArm.life / curLevel) * nextLevel).ToString();
            txtLeftArmNextAttack.text = ((robot.LeftArm.attack / curLevel) * nextLevel).ToString();
            txtLeftArmNextDefence.text = ((robot.LeftArm.defence / curLevel) * nextLevel).ToString();

            if (robot.onSale == 1)
            {
                txtLeftArmCurLevel.text = "On Sale";
                txtLeftArmCurLevel.color = Color.yellow;
                btnLeftArmUpgrade.interactable = false;
            }
            else
            {
                if (robot.LeftArm.onSale == 0)
                {
                    txtLeftArmCurLevel.text = "Level : " + curLevel.ToString();
                    txtLeftArmCurLevel.color = Color.white;
                    if (curLevel < 5 && GameManage.User.gems >= 100)
                    {
                        btnLeftArmUpgrade.interactable = true;
                    }
                    else
                    {
                        btnLeftArmUpgrade.interactable = false;
                    }
                }
                else
                {
                    txtLeftArmCurLevel.text = "On Sale";
                    txtLeftArmCurLevel.color = Color.yellow;
                    btnLeftArmUpgrade.interactable = false;
                }
            }

            txtLeftArmNextLevel.text = (nextLevel - curLevel).ToString();
            txtLeftArmPrice.text = 100.ToString();
        }
    }

    private void InitRightArm()
    {
        if (robot.RightArm == null)
        {
            btnRightArmUpgrade.interactable = false;

            imgRightArmLife.fillAmount = 0;
            imgRightArmAttack.fillAmount = 0;
            imgRightArmDefence.fillAmount = 0;

            sliderRightArmLife.value = 0;
            sliderRightArmAttack.value = 0;
            sliderRightArmDefence.value = 0;

            txtRightArmCurLife.text = 0.ToString();
            txtRightArmCurAttack.text = 0.ToString();
            txtRightArmCurDefence.text = 0.ToString();

            txtRightArmNextLife.text = 0.ToString();
            txtRightArmNextAttack.text = 0.ToString();
            txtRightArmNextDefence.text = 0.ToString();

            txtRightArmCurLevel.text = "Lost or Sold";
            txtRightArmCurLevel.color = Color.red;
            txtRightArmNextLevel.text = 0.ToString();
            txtRightArmPrice.text = 0.ToString();
        }
        else if (robot.RightArm.is_broken == 1)
        {
            btnRightArmUpgrade.interactable = false;
            int curLevel = robot.RightArm.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.RightArm.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgRightArmLife.fillAmount = robot.RightArm.life / ((robot.RightArm.life / curLevel) * 5f);
            imgRightArmAttack.fillAmount = robot.RightArm.attack / ((robot.RightArm.attack / curLevel) * 5f);
            imgRightArmDefence.fillAmount = robot.RightArm.defence / ((robot.RightArm.defence / curLevel) * 5f);

            sliderRightArmLife.value = ((robot.RightArm.life / curLevel) * nextLevel) / ((robot.RightArm.life / curLevel) * 5f);
            sliderRightArmAttack.value = ((robot.RightArm.attack / curLevel) * nextLevel) / ((robot.RightArm.attack / curLevel) * 5f);
            sliderRightArmDefence.value = ((robot.RightArm.defence / curLevel) * nextLevel) / ((robot.RightArm.defence / curLevel) * 5f);

            txtRightArmCurLife.text = robot.RightArm.life.ToString();
            txtRightArmCurAttack.text = robot.RightArm.attack.ToString();
            txtRightArmCurDefence.text = robot.RightArm.defence.ToString();

            txtRightArmNextLife.text = ((robot.RightArm.life / curLevel) * nextLevel).ToString();
            txtRightArmNextAttack.text = ((robot.RightArm.attack / curLevel) * nextLevel).ToString();
            txtRightArmNextDefence.text = ((robot.RightArm.defence / curLevel) * nextLevel).ToString();

            txtRightArmCurLevel.text = "Broken";
            txtRightArmCurLevel.color = Color.red;
            txtRightArmNextLevel.text = (nextLevel - curLevel).ToString();
            txtRightArmPrice.text = 150.ToString();
        }
        else
        {
            int curLevel = robot.RightArm.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.RightArm.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgRightArmLife.fillAmount = robot.RightArm.life / ((robot.RightArm.life / curLevel) * 5f);
            imgRightArmAttack.fillAmount = robot.RightArm.attack / ((robot.RightArm.attack / curLevel) * 5f);
            imgRightArmDefence.fillAmount = robot.RightArm.defence / ((robot.RightArm.defence / curLevel) * 5f);

            sliderRightArmLife.value = ((robot.RightArm.life / curLevel) * nextLevel) / ((robot.RightArm.life / curLevel) * 5f);
            sliderRightArmAttack.value = ((robot.RightArm.attack / curLevel) * nextLevel) / ((robot.RightArm.attack / curLevel) * 5f);
            sliderRightArmDefence.value = ((robot.RightArm.defence / curLevel) * nextLevel) / ((robot.RightArm.defence / curLevel) * 5f);

            txtRightArmCurLife.text = robot.RightArm.life.ToString();
            txtRightArmCurAttack.text = robot.RightArm.attack.ToString();
            txtRightArmCurDefence.text = robot.RightArm.defence.ToString();

            txtRightArmNextLife.text = ((robot.RightArm.life / curLevel) * nextLevel).ToString();
            txtRightArmNextAttack.text = ((robot.RightArm.attack / curLevel) * nextLevel).ToString();
            txtRightArmNextDefence.text = ((robot.RightArm.defence / curLevel) * nextLevel).ToString();

            if (robot.onSale == 1)
            {
                txtRightArmCurLevel.text = "On Sale";
                txtRightArmCurLevel.color = Color.yellow;
                btnRightArmUpgrade.interactable = false;
            }
            else
            {
                if (robot.RightArm.onSale == 0)
                {
                    txtRightArmCurLevel.text = "Level : " + curLevel.ToString();
                    txtRightArmCurLevel.color = Color.white;
                    if (curLevel < 5 && GameManage.User.gems >= 150)
                    {
                        btnRightArmUpgrade.interactable = true;
                    }
                    else
                    {
                        btnRightArmUpgrade.interactable = false;
                    }
                }
                else
                {
                    txtRightArmCurLevel.text = "On Sale";
                    txtRightArmCurLevel.color = Color.yellow;
                    btnRightArmUpgrade.interactable = false;
                }
            }

            txtRightArmNextLevel.text = (nextLevel - curLevel).ToString();
            txtRightArmPrice.text = 150.ToString();
        }
    }

    private void InitLeg()
    {
        if (robot.Leg == null)
        {
            btnLegUpgrade.interactable = false;

            imgLegLife.fillAmount = 0;
            imgLegAttack.fillAmount = 0;
            imgLegDefence.fillAmount = 0;
            imgLegVelocity.fillAmount = 0;

            sliderLegLife.value = 0;
            sliderLegAttack.value = 0;
            sliderLegDefence.value = 0;
            sliderLegDefence.value = 0;

            txtLegCurLife.text = 0.ToString();
            txtLegCurAttack.text = 0.ToString();
            txtLegCurDefence.text = 0.ToString();
            txtLegCurVelocity.text = 0.ToString();

            txtLegNextLife.text = 0.ToString();
            txtLegNextAttack.text = 0.ToString();
            txtLegNextDefence.text = 0.ToString();
            txtLegNextVelocity.text = 0.ToString();

            txtLegCurLevel.text = "Lost or Sold";
            txtLegCurLevel.color = Color.red;
            txtLegNextLevel.text = 0.ToString();
            txtLegPrice.text = 0.ToString();
        }
        else if (robot.Leg.is_broken == 1)
        {
            btnLegUpgrade.interactable = false;
            int curLevel = robot.Leg.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.Leg.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgLegLife.fillAmount = robot.Leg.life / ((robot.Leg.life / curLevel) * 5f);
            imgLegAttack.fillAmount = robot.Leg.attack / ((robot.Leg.attack / curLevel) * 5f);
            imgLegDefence.fillAmount = robot.Leg.defence / ((robot.Leg.defence / curLevel) * 5f);
            imgLegVelocity.fillAmount = robot.Leg.velocity / ((robot.Leg.velocity / curLevel) * 5f);

            sliderLegLife.value = ((robot.Leg.life / curLevel) * nextLevel) / ((robot.Leg.life / curLevel) * 5f);
            sliderLegAttack.value = ((robot.Leg.attack / curLevel) * nextLevel) / ((robot.Leg.attack / curLevel) * 5f);
            sliderLegDefence.value = ((robot.Leg.defence / curLevel) * nextLevel) / ((robot.Leg.defence / curLevel) * 5f);
            sliderLegDefence.value = ((robot.Leg.velocity / curLevel) * nextLevel) / ((robot.Leg.velocity / curLevel) * 5f);

            txtLegCurLife.text = robot.Leg.life.ToString();
            txtLegCurAttack.text = robot.Leg.attack.ToString();
            txtLegCurDefence.text = robot.Leg.defence.ToString();
            txtLegCurVelocity.text = robot.Leg.velocity.ToString();

            txtLegNextLife.text = ((robot.Leg.life / curLevel) * nextLevel).ToString();
            txtLegNextAttack.text = ((robot.Leg.attack / curLevel) * nextLevel).ToString();
            txtLegNextDefence.text = ((robot.Leg.defence / curLevel) * nextLevel).ToString();
            txtLegNextVelocity.text = ((robot.Leg.velocity / curLevel) * nextLevel).ToString();

            txtLegCurLevel.text = "Broken";
            txtLegCurLevel.color = Color.red;
            txtLegNextLevel.text = (nextLevel - curLevel).ToString();
            txtLegPrice.text = 250.ToString();
        }
        else
        {
            int curLevel = robot.Leg.level;
            int nextLevel = 0;
            if (curLevel < 5)
            {
                nextLevel = robot.Leg.level + 1;
            }
            else
            {
                nextLevel = curLevel;
            }

            imgLegLife.fillAmount = robot.Leg.life / ((robot.Leg.life / curLevel) * 5f);
            imgLegAttack.fillAmount = robot.Leg.attack / ((robot.Leg.attack / curLevel) * 5f);
            imgLegDefence.fillAmount = robot.Leg.defence / ((robot.Leg.defence / curLevel) * 5f);
            imgLegVelocity.fillAmount = robot.Leg.velocity / ((robot.Leg.velocity / curLevel) * 5f);

            sliderLegLife.value = ((robot.Leg.life / curLevel) * nextLevel) / ((robot.Leg.life / curLevel) * 5f);
            sliderLegAttack.value = ((robot.Leg.attack / curLevel) * nextLevel) / ((robot.Leg.attack / curLevel) * 5f);
            sliderLegDefence.value = ((robot.Leg.defence / curLevel) * nextLevel) / ((robot.Leg.defence / curLevel) * 5f);
            sliderLegDefence.value = ((robot.Leg.velocity / curLevel) * nextLevel) / ((robot.Leg.velocity / curLevel) * 5f);

            txtLegCurLife.text = robot.Leg.life.ToString();
            txtLegCurAttack.text = robot.Leg.attack.ToString();
            txtLegCurDefence.text = robot.Leg.defence.ToString();
            txtLegCurVelocity.text = robot.Leg.velocity.ToString();

            txtLegNextLife.text = ((robot.Leg.life / curLevel) * nextLevel).ToString();
            txtLegNextAttack.text = ((robot.Leg.attack / curLevel) * nextLevel).ToString();
            txtLegNextDefence.text = ((robot.Leg.defence / curLevel) * nextLevel).ToString();
            txtLegNextVelocity.text = ((robot.Leg.velocity / curLevel) * nextLevel).ToString();

            if (robot.onSale == 1)
            {
                txtLegCurLevel.text = "On Sale";
                txtLegCurLevel.color = Color.yellow;
                btnLegUpgrade.interactable = false;
            }
            else
            {
                if (robot.Leg.onSale == 0)
                {
                    txtLegCurLevel.text = "Level : " + curLevel.ToString();
                    txtLegCurLevel.color = Color.white;
                    if (curLevel < 5 && GameManage.User.gems >= 250)
                    {
                        btnLegUpgrade.interactable = true;
                    }
                    else
                    {
                        btnLegUpgrade.interactable = false;
                    }
                }
                else
                {
                    txtLegCurLevel.text = "On Sale";
                    txtLegCurLevel.color = Color.yellow;
                    btnLegUpgrade.interactable = false;
                }
            }

            txtLegNextLevel.text = (nextLevel - curLevel).ToString();
            txtLegPrice.text = 250.ToString();
        }
    }

    private int priceOfPart = 0;

    public void UpgradeHead()
    {
        float price = float.Parse(txtHeadPrice.text);
        if (GameManage.User.gems >= price)
        {
            robot.Head.life = robot.Head.life + (robot.Head.life / robot.Head.level);
            robot.Head.attack = robot.Head.attack + (robot.Head.attack / robot.Head.level);
            robot.Head.defence = robot.Head.defence + (robot.Head.defence / robot.Head.level);
            robot.Head.level = robot.Head.level + 1;

            priceOfPart = (int)price;
            GameManage.instance.ChangeHead(GB.g_MyID, robot.Head, callbackUserInfo);
            //Init();
            //GameManage.User.UpdateLocalFile();
            //GameManage.User.gems -= (int) price;
            //Inventory.instance.Init();
        }
        else
        {
            // Open dialog 2
        }
    }

    public void UpgradeLeftArm()
    {
        float price = float.Parse(txtLeftArmPrice.text);
        if (GameManage.User.gems >= price)
        {
            robot.LeftArm.life = robot.LeftArm.life + (robot.LeftArm.life / robot.LeftArm.level);
            robot.LeftArm.attack = robot.LeftArm.attack + (robot.LeftArm.attack / robot.LeftArm.level);
            robot.LeftArm.defence = robot.LeftArm.defence + (robot.LeftArm.defence / robot.LeftArm.level);
            robot.LeftArm.level = robot.LeftArm.level + 1;

            priceOfPart = (int)price;
            GameManage.instance.ChangeLeftArm(GB.g_MyID, robot.LeftArm, callbackUserInfo);
            //Init();
            //GameManage.User.UpdateLocalFile();
            //GameManage.User.gems -= (int)price;
            //Inventory.instance.Init();
        }
        else
        {
            // Open dialog 2
        }
    }

    public void UpgradeRightArm()
    {
        float price = float.Parse(txtRightArmPrice.text);
        if (GameManage.User.gems >= price)
        {
            robot.RightArm.life = robot.RightArm.life + (robot.RightArm.life / robot.RightArm.level);
            robot.RightArm.attack = robot.RightArm.attack + (robot.RightArm.attack / robot.RightArm.level);
            robot.RightArm.defence = robot.RightArm.defence + (robot.RightArm.defence / robot.RightArm.level);
            robot.RightArm.level = robot.RightArm.level + 1;

            priceOfPart = (int)price;
            GameManage.instance.ChangeRightArm(GB.g_MyID, robot.RightArm, callbackUserInfo);
            //Init();
            //GameManage.User.UpdateLocalFile();
            //GameManage.User.gems -= (int)price;
            //Inventory.instance.Init();
        }
        else
        {
            // Open dialog 2
        }
    }

    public void UpgradeLeg()
    {
        float price = float.Parse(txtLegPrice.text);
        if (GameManage.User.gems >= price)
        {
            robot.Leg.life = robot.Leg.life + (robot.Leg.life / robot.Leg.level);
            robot.Leg.attack = robot.Leg.attack + (robot.Leg.attack / robot.Leg.level);
            robot.Leg.defence = robot.Leg.defence + (robot.Leg.defence / robot.Leg.level);
            robot.Leg.velocity = robot.Leg.velocity + (robot.Leg.velocity / robot.Leg.level);
            robot.Leg.level = robot.Leg.level + 1;

            priceOfPart = (int)price;
            GameManage.instance.ChangeLegs(GB.g_MyID, robot.Leg, callbackUserInfo);
            //Init();
            //GameManage.User.UpdateLocalFile();
            //GameManage.User.gems -= (int)price;
            //Inventory.instance.Init();
        }
        else
        {
            // Open dialog 2
        }
    }

    #region API Callbacks
    private int callbackUserInfo(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("ChangePart: Done");
                Debug.Log(www.text);
                GameManage.User.username = json["result"]["username"].ToString();
                GameManage.User.gems = int.Parse(json["result"]["gems"].ToString());
                GameManage.User.rank = int.Parse(json["result"]["rank"].ToString());
                GameManage.User.win = int.Parse(json["result"]["win"].ToString());
                GameManage.User.loss = int.Parse(json["result"]["loss"].ToString());
                GameManage.User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                GameManage.User.Robots.Clear();
                for (int i = 0; i < json["result"]["robots"].Count; i++)
                {
                    Robot r = new Robot();
                    r.object_id = int.Parse(json["result"]["robots"][i]["object_id"].ToString());
                    r.type = json["result"]["robots"][i]["type"].ToString();
                    r.name = json["result"]["robots"][i]["name"].ToString();
                    r.price = float.Parse(json["result"]["robots"][i]["price"].ToString());
                    r.head_object_id = int.Parse(json["result"]["robots"][i]["head_object_id"].ToString());
                    r.larm_object_id = int.Parse(json["result"]["robots"][i]["larm_object_id"].ToString());
                    r.rarm_object_id = int.Parse(json["result"]["robots"][i]["rarm_object_id"].ToString());
                    r.leg_object_id = int.Parse(json["result"]["robots"][i]["leg_object_id"].ToString());
                    r.onSale = int.Parse(json["result"]["robots"][i]["onSale"].ToString());
                    r.salePrice = int.Parse(json["result"]["robots"][i]["salePrice"].ToString());

                    GameManage.User.AddRobot(r);
                }
                GameManage.User.Heads.Clear();
                for (int i = 0; i < json["result"]["heads"].Count; i++)
                {
                    Head hc = new Head();
                    hc.object_id = int.Parse(json["result"]["heads"][i]["object_id"].ToString());
                    hc.head_id = int.Parse(json["result"]["heads"][i]["head_id"].ToString());
                    hc.type = json["result"]["heads"][i]["type"].ToString();
                    hc.name = json["result"]["heads"][i]["name"].ToString();
                    hc.level = int.Parse(json["result"]["heads"][i]["level"].ToString());
                    hc.price = float.Parse(json["result"]["heads"][i]["price"].ToString());
                    hc.medicament = int.Parse(json["result"]["heads"][i]["medicament"].ToString());
                    hc.life = float.Parse(json["result"]["heads"][i]["life"].ToString());
                    hc.attack = float.Parse(json["result"]["heads"][i]["attack"].ToString());
                    hc.defence = float.Parse(json["result"]["heads"][i]["defence"].ToString());
                    hc.velocity = float.Parse(json["result"]["heads"][i]["velocity"].ToString());
                    hc.onSale = int.Parse(json["result"]["heads"][i]["onSale"].ToString());
                    hc.salePrice = int.Parse(json["result"]["heads"][i]["salePrice"].ToString());
                    hc.is_broken = int.Parse(json["result"]["heads"][i]["is_broken"].ToString());
                    hc.break_timestamp = Int64.Parse(json["result"]["heads"][i]["break_timestamp"].ToString());

                    GameManage.User.AddHead(hc);
                }
                GameManage.User.LeftArms.Clear();
                for (int i = 0; i < json["result"]["larms"].Count; i++)
                {
                    LeftArm lac = new LeftArm();
                    lac.object_id = int.Parse(json["result"]["larms"][i]["object_id"].ToString());
                    lac.larm_id = int.Parse(json["result"]["larms"][i]["larm_id"].ToString());
                    lac.type = json["result"]["larms"][i]["type"].ToString();
                    lac.name = json["result"]["larms"][i]["name"].ToString();
                    lac.level = int.Parse(json["result"]["larms"][i]["level"].ToString());
                    lac.price = float.Parse(json["result"]["larms"][i]["price"].ToString());
                    lac.medicament = int.Parse(json["result"]["larms"][i]["medicament"].ToString());
                    lac.life = float.Parse(json["result"]["larms"][i]["life"].ToString());
                    lac.attack = float.Parse(json["result"]["larms"][i]["attack"].ToString());
                    lac.defence = float.Parse(json["result"]["larms"][i]["defence"].ToString());
                    lac.velocity = float.Parse(json["result"]["larms"][i]["velocity"].ToString());
                    lac.onSale = int.Parse(json["result"]["larms"][i]["onSale"].ToString());
                    lac.salePrice = int.Parse(json["result"]["larms"][i]["salePrice"].ToString());
                    lac.is_broken = int.Parse(json["result"]["larms"][i]["is_broken"].ToString());
                    lac.break_timestamp = Int64.Parse(json["result"]["larms"][i]["break_timestamp"].ToString());

                    GameManage.User.AddLeftArm(lac);
                }
                GameManage.User.RightArms.Clear();
                for (int i = 0; i < json["result"]["rarms"].Count; i++)
                {
                    RightArm rac = new RightArm();
                    rac.object_id = int.Parse(json["result"]["rarms"][i]["object_id"].ToString());
                    rac.rarm_id = int.Parse(json["result"]["rarms"][i]["rarm_id"].ToString());
                    rac.type = json["result"]["rarms"][i]["type"].ToString();
                    rac.name = json["result"]["rarms"][i]["name"].ToString();
                    rac.level = int.Parse(json["result"]["rarms"][i]["level"].ToString());
                    rac.price = float.Parse(json["result"]["rarms"][i]["price"].ToString());
                    rac.medicament = int.Parse(json["result"]["rarms"][i]["medicament"].ToString());
                    rac.life = float.Parse(json["result"]["rarms"][i]["life"].ToString());
                    rac.attack = float.Parse(json["result"]["rarms"][i]["attack"].ToString());
                    rac.defence = float.Parse(json["result"]["rarms"][i]["defence"].ToString());
                    rac.velocity = float.Parse(json["result"]["rarms"][i]["velocity"].ToString());
                    rac.onSale = int.Parse(json["result"]["rarms"][i]["onSale"].ToString());
                    rac.salePrice = int.Parse(json["result"]["rarms"][i]["salePrice"].ToString());
                    rac.is_broken = int.Parse(json["result"]["rarms"][i]["is_broken"].ToString());
                    rac.break_timestamp = Int64.Parse(json["result"]["rarms"][i]["break_timestamp"].ToString());

                    GameManage.User.AddRightArm(rac);
                }
                GameManage.User.Legs.Clear();
                for (int i = 0; i < json["result"]["legs"].Count; i++)
                {
                    Leg lc = new Leg();
                    lc.object_id = int.Parse(json["result"]["legs"][i]["object_id"].ToString());
                    lc.leg_id = int.Parse(json["result"]["legs"][i]["leg_id"].ToString());
                    lc.type = json["result"]["legs"][i]["type"].ToString();
                    lc.name = json["result"]["legs"][i]["name"].ToString();
                    lc.level = int.Parse(json["result"]["legs"][i]["level"].ToString());
                    lc.price = float.Parse(json["result"]["legs"][i]["price"].ToString());
                    lc.life = float.Parse(json["result"]["legs"][i]["life"].ToString());
                    lc.medicament = int.Parse(json["result"]["legs"][i]["medicament"].ToString());
                    lc.attack = float.Parse(json["result"]["legs"][i]["attack"].ToString());
                    lc.defence = float.Parse(json["result"]["legs"][i]["defence"].ToString());
                    lc.velocity = float.Parse(json["result"]["legs"][i]["velocity"].ToString());
                    lc.onSale = int.Parse(json["result"]["legs"][i]["onSale"].ToString());
                    lc.salePrice = int.Parse(json["result"]["legs"][i]["salePrice"].ToString());
                    lc.is_broken = int.Parse(json["result"]["legs"][i]["is_broken"].ToString());
                    lc.break_timestamp = Int64.Parse(json["result"]["legs"][i]["break_timestamp"].ToString());

                    GameManage.User.AddLeg(lc);
                }
                Init();
                Inventory.instance.Init();

                GameManage.instance.UpdateUserGems(GB.g_MyID, -priceOfPart, callbackUserGems);
            }
            else if (status == "fail")
            {
                Debug.LogError("ChangePart: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("ChangePart: Error");
            Debug.Log(www.error);
        }
        return 0;
    }

    private int callbackUserGems(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("UpdateGems: Done");
                Debug.Log(www.text);
                GameManage.User.username = json["result"]["username"].ToString();
                GameManage.User.gems = int.Parse(json["result"]["gems"].ToString());
                GameManage.User.rank = int.Parse(json["result"]["rank"].ToString());
                GameManage.User.win = int.Parse(json["result"]["win"].ToString());
                GameManage.User.loss = int.Parse(json["result"]["loss"].ToString());
                GameManage.User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                GameManage.User.Robots.Clear();
                for (int i = 0; i < json["result"]["robots"].Count; i++)
                {
                    Robot r = new Robot();
                    r.object_id = int.Parse(json["result"]["robots"][i]["object_id"].ToString());
                    r.type = json["result"]["robots"][i]["type"].ToString();
                    r.name = json["result"]["robots"][i]["name"].ToString();
                    r.price = float.Parse(json["result"]["robots"][i]["price"].ToString());
                    r.head_object_id = int.Parse(json["result"]["robots"][i]["head_object_id"].ToString());
                    r.larm_object_id = int.Parse(json["result"]["robots"][i]["larm_object_id"].ToString());
                    r.rarm_object_id = int.Parse(json["result"]["robots"][i]["rarm_object_id"].ToString());
                    r.leg_object_id = int.Parse(json["result"]["robots"][i]["leg_object_id"].ToString());
                    r.onSale = int.Parse(json["result"]["robots"][i]["onSale"].ToString());
                    r.salePrice = int.Parse(json["result"]["robots"][i]["salePrice"].ToString());

                    GameManage.User.AddRobot(r);
                }
                GameManage.User.Heads.Clear();
                for (int i = 0; i < json["result"]["heads"].Count; i++)
                {
                    Head hc = new Head();
                    hc.object_id = int.Parse(json["result"]["heads"][i]["object_id"].ToString());
                    hc.head_id = int.Parse(json["result"]["heads"][i]["head_id"].ToString());
                    hc.type = json["result"]["heads"][i]["type"].ToString();
                    hc.name = json["result"]["heads"][i]["name"].ToString();
                    hc.level = int.Parse(json["result"]["heads"][i]["level"].ToString());
                    hc.price = float.Parse(json["result"]["heads"][i]["price"].ToString());
                    hc.medicament = int.Parse(json["result"]["heads"][i]["medicament"].ToString());
                    hc.life = float.Parse(json["result"]["heads"][i]["life"].ToString());
                    hc.attack = float.Parse(json["result"]["heads"][i]["attack"].ToString());
                    hc.defence = float.Parse(json["result"]["heads"][i]["defence"].ToString());
                    hc.velocity = float.Parse(json["result"]["heads"][i]["velocity"].ToString());
                    hc.onSale = int.Parse(json["result"]["heads"][i]["onSale"].ToString());
                    hc.salePrice = int.Parse(json["result"]["heads"][i]["salePrice"].ToString());
                    hc.is_broken = int.Parse(json["result"]["heads"][i]["is_broken"].ToString());
                    hc.break_timestamp = Int64.Parse(json["result"]["heads"][i]["break_timestamp"].ToString());

                    GameManage.User.AddHead(hc);
                }
                GameManage.User.LeftArms.Clear();
                for (int i = 0; i < json["result"]["larms"].Count; i++)
                {
                    LeftArm lac = new LeftArm();
                    lac.object_id = int.Parse(json["result"]["larms"][i]["object_id"].ToString());
                    lac.larm_id = int.Parse(json["result"]["larms"][i]["larm_id"].ToString());
                    lac.type = json["result"]["larms"][i]["type"].ToString();
                    lac.name = json["result"]["larms"][i]["name"].ToString();
                    lac.level = int.Parse(json["result"]["larms"][i]["level"].ToString());
                    lac.price = float.Parse(json["result"]["larms"][i]["price"].ToString());
                    lac.medicament = int.Parse(json["result"]["larms"][i]["medicament"].ToString());
                    lac.life = float.Parse(json["result"]["larms"][i]["life"].ToString());
                    lac.attack = float.Parse(json["result"]["larms"][i]["attack"].ToString());
                    lac.defence = float.Parse(json["result"]["larms"][i]["defence"].ToString());
                    lac.velocity = float.Parse(json["result"]["larms"][i]["velocity"].ToString());
                    lac.onSale = int.Parse(json["result"]["larms"][i]["onSale"].ToString());
                    lac.salePrice = int.Parse(json["result"]["larms"][i]["salePrice"].ToString());
                    lac.is_broken = int.Parse(json["result"]["larms"][i]["is_broken"].ToString());
                    lac.break_timestamp = Int64.Parse(json["result"]["larms"][i]["break_timestamp"].ToString());

                    GameManage.User.AddLeftArm(lac);
                }
                GameManage.User.RightArms.Clear();
                for (int i = 0; i < json["result"]["rarms"].Count; i++)
                {
                    RightArm rac = new RightArm();
                    rac.object_id = int.Parse(json["result"]["rarms"][i]["object_id"].ToString());
                    rac.rarm_id = int.Parse(json["result"]["rarms"][i]["rarm_id"].ToString());
                    rac.type = json["result"]["rarms"][i]["type"].ToString();
                    rac.name = json["result"]["rarms"][i]["name"].ToString();
                    rac.level = int.Parse(json["result"]["rarms"][i]["level"].ToString());
                    rac.price = float.Parse(json["result"]["rarms"][i]["price"].ToString());
                    rac.medicament = int.Parse(json["result"]["rarms"][i]["medicament"].ToString());
                    rac.life = float.Parse(json["result"]["rarms"][i]["life"].ToString());
                    rac.attack = float.Parse(json["result"]["rarms"][i]["attack"].ToString());
                    rac.defence = float.Parse(json["result"]["rarms"][i]["defence"].ToString());
                    rac.velocity = float.Parse(json["result"]["rarms"][i]["velocity"].ToString());
                    rac.onSale = int.Parse(json["result"]["rarms"][i]["onSale"].ToString());
                    rac.salePrice = int.Parse(json["result"]["rarms"][i]["salePrice"].ToString());
                    rac.is_broken = int.Parse(json["result"]["rarms"][i]["is_broken"].ToString());
                    rac.break_timestamp = Int64.Parse(json["result"]["rarms"][i]["break_timestamp"].ToString());

                    GameManage.User.AddRightArm(rac);
                }
                GameManage.User.Legs.Clear();
                for (int i = 0; i < json["result"]["legs"].Count; i++)
                {
                    Leg lc = new Leg();
                    lc.object_id = int.Parse(json["result"]["legs"][i]["object_id"].ToString());
                    lc.leg_id = int.Parse(json["result"]["legs"][i]["leg_id"].ToString());
                    lc.type = json["result"]["legs"][i]["type"].ToString();
                    lc.name = json["result"]["legs"][i]["name"].ToString();
                    lc.level = int.Parse(json["result"]["legs"][i]["level"].ToString());
                    lc.price = float.Parse(json["result"]["legs"][i]["price"].ToString());
                    lc.life = float.Parse(json["result"]["legs"][i]["life"].ToString());
                    lc.medicament = int.Parse(json["result"]["legs"][i]["medicament"].ToString());
                    lc.attack = float.Parse(json["result"]["legs"][i]["attack"].ToString());
                    lc.defence = float.Parse(json["result"]["legs"][i]["defence"].ToString());
                    lc.velocity = float.Parse(json["result"]["legs"][i]["velocity"].ToString());
                    lc.onSale = int.Parse(json["result"]["legs"][i]["onSale"].ToString());
                    lc.salePrice = int.Parse(json["result"]["legs"][i]["salePrice"].ToString());
                    lc.is_broken = int.Parse(json["result"]["legs"][i]["is_broken"].ToString());
                    lc.break_timestamp = Int64.Parse(json["result"]["legs"][i]["break_timestamp"].ToString());

                    GameManage.User.AddLeg(lc);
                }
                priceOfPart = 0;

                Init();
                Inventory.instance.Init();
                MainMenuPanel.instance.Refresh();
                Inventory.instance.ShowSubPanel(0);
            }
            else if (status == "fail")
            {
                Debug.LogError("UpdateGems: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("UpdateGems: Error");
            Debug.Log(www.error);
        }
        return 0;
    }
    #endregion
}
