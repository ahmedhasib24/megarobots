using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedicamentSubPanel : MonoBehaviour
{
    public Image iconHead, iconLeftArm, iconRightArm, iconLeg;
    public Image imgHead, imgLeftArm, imgRightArm, imgLeg;
    public Text txtHeadMessage, txtLeftArmMessage, txtRightArmMessage, txtLegMessage;
    public Text txtHeadMedic, txtLeftArmMedic, txtRightArmMedic, txtLegMedic;
    public Text txtHeadImagineTime, txtLeftArmImagineTime, txtRightArmImagineTime, txtLegImagineTime;
    public Button btnBuyHeadMedic, btnBuyLeftArmMedic, btnBuyRightArmMedic, btnBuyLegMedic;

    Robot curRobot;

    void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (curRobot.Head != null && curRobot.Head.is_broken == 1)
        {
            if (curRobot.Head.ImagineTime() > 0)
            {
                Int64 totalSeconds = curRobot.Head.break_timestamp + ((int)(curRobot.Head.ImagineTime() * 3600));
                Int64 timeDifference = totalSeconds - (Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                if (timeDifference > 0)
                {
                    int h = (int)(timeDifference / 3600);
                    int m = (int)(timeDifference % 3600) / 60;
                    int s = (int)((timeDifference % 3600) % 60);
                    txtHeadImagineTime.text = string.Format("Healing In: {0:00}:{1:00}:{2:00}", h, m, s);
                    txtHeadImagineTime.color = Color.yellow;
                }
                else
                {
                    curRobot.Head.is_broken = 0;
                    curRobot.Head.break_timestamp = 0;
                    int medicament_needed = (int)(curRobot.Head.ActualImagineTime() / 0.5f);
                    if ((curRobot.Head.medicament - medicament_needed) > 0)
                    {
                        curRobot.Head.medicament = curRobot.Head.medicament - medicament_needed;
                    }
                    else
                    {
                        curRobot.Head.medicament = 0;
                    }
                    GameManage.instance.ChangeHead(GB.g_MyID, curRobot.Head, callbackUserGems);
                }
            }
        }

        if (curRobot.LeftArm != null && curRobot.LeftArm.is_broken == 1)
        {
            if (curRobot.LeftArm.ImagineTime() > 0)
            {
                Int64 totalSeconds = curRobot.LeftArm.break_timestamp + ((int)(curRobot.LeftArm.ImagineTime() * 3600));
                Int64 timeDifference = totalSeconds - (Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                if (timeDifference > 0)
                {
                    int h = (int)(timeDifference / 3600);
                    int m = (int)(timeDifference % 3600) / 60;
                    int s = (int)((timeDifference % 3600) % 60);
                    txtLeftArmImagineTime.text = string.Format("Healing In: {0:00}:{1:00}:{2:00}", h, m, s);
                    txtLeftArmImagineTime.color = Color.yellow;
                }
                else
                {
                    curRobot.LeftArm.is_broken = 0;
                    curRobot.LeftArm.break_timestamp = 0;
                    int medicament_needed = (int)(curRobot.LeftArm.ActualImagineTime() / 0.5f);
                    if ((curRobot.LeftArm.medicament - medicament_needed) > 0)
                    {
                        curRobot.LeftArm.medicament = curRobot.LeftArm.medicament - medicament_needed;
                    }
                    else
                    {
                        curRobot.LeftArm.medicament = 0;
                    }
                    GameManage.instance.ChangeLeftArm(GB.g_MyID, curRobot.LeftArm, callbackUserGems);
                }
            }
        }

        if (curRobot.RightArm != null && curRobot.RightArm.is_broken == 1)
        {
            if (curRobot.RightArm.ImagineTime() > 0)
            {
                //Debug.Log("Imagine: " + curRobot.RightArm.ImagineTime());
                Int64 totalSeconds = curRobot.RightArm.break_timestamp + ((int)(curRobot.RightArm.ImagineTime() * 3600));
                Int64 timeDifference = totalSeconds - (Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                if (timeDifference > 0)
                {
                    int h = (int)(timeDifference / 3600);
                    int m = (int)(timeDifference % 3600) / 60;
                    int s = (int)((timeDifference % 3600) % 60);
                    txtRightArmImagineTime.text = string.Format("Healing In: {0:00}:{1:00}:{2:00}", h, m, s);
                    txtRightArmImagineTime.color = Color.yellow;
                }
                else
                {
                    curRobot.RightArm.is_broken = 0;
                    curRobot.RightArm.break_timestamp = 0;
                    int medicament_needed = (int)(curRobot.RightArm.ActualImagineTime() / 0.5f);
                    if ((curRobot.RightArm.medicament - medicament_needed) > 0)
                    {
                        curRobot.RightArm.medicament = curRobot.RightArm.medicament - medicament_needed;
                    }
                    else
                    {
                        curRobot.RightArm.medicament = 0;
                    }
                    GameManage.instance.ChangeRightArm(GB.g_MyID, curRobot.RightArm, callbackUserGems);
                }
            }
        }

        if (curRobot.Leg != null && curRobot.Leg.is_broken == 1)
        {
            if (curRobot.Leg.ImagineTime() > 0)
            {
                Int64 totalSeconds = curRobot.Leg.break_timestamp + ((int)(curRobot.Leg.ImagineTime() * 3600));
                Int64 timeDifference = totalSeconds - (Int64)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                if (timeDifference > 0)
                {
                    int h = (int)(timeDifference / 3600);
                    int m = (int)(timeDifference % 3600) / 60;
                    int s = (int)((timeDifference % 3600) % 60);
                    txtLegImagineTime.text = string.Format("Healing In: {0:00}:{1:00}:{2:00}", h, m, s);
                    txtLegImagineTime.color = Color.yellow;
                }
                else
                {
                    curRobot.Leg.is_broken = 0;
                    curRobot.Leg.break_timestamp = 0;
                    int medicament_needed = (int)(curRobot.Leg.ActualImagineTime() / 0.5f);
                    if ((curRobot.Leg.medicament - medicament_needed) > 0)
                    {
                        curRobot.Leg.medicament = curRobot.Leg.medicament - medicament_needed;
                    }
                    else
                    {
                        curRobot.Leg.medicament = 0;
                    }
                    GameManage.instance.ChangeLegs(GB.g_MyID, curRobot.Leg, callbackUserGems);
                }
            }
        }
    }

    void Init()
    {
        curRobot = Inventory.instance.CurrentRobot;

        //Head
        if (curRobot.Head == null)
        {
            iconHead.gameObject.SetActive(false);
            imgHead.gameObject.SetActive(true);
            txtHeadMessage.text = "Lost or Sold";
            txtHeadMessage.color = Color.red;
            txtHeadMessage.gameObject.SetActive(true);
            txtHeadMedic.gameObject.SetActive(false);
            txtHeadImagineTime.gameObject.SetActive(false);
            btnBuyHeadMedic.interactable = false;
        }
        else if (curRobot.Head.is_broken == 1)
        {
            iconHead.gameObject.SetActive(true);
            imgHead.gameObject.SetActive(false);
            iconHead.sprite = GameManage.instance.AllHeadPieceImages[curRobot.Head.head_id];
            txtHeadMessage.text = "Broken";
            txtHeadMessage.color = Color.red;
            txtHeadMedic.text = "Medic : " + curRobot.Head.medicament;
            txtHeadImagineTime.text = "Imagine Time: " + curRobot.Head.ImagineTime() + "h";
            txtHeadImagineTime.color = Color.white;
            txtHeadMessage.gameObject.SetActive(true);
            txtHeadMedic.gameObject.SetActive(true);
            txtHeadImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyHeadMedic.interactable = true;
            }
            else
            {
                btnBuyHeadMedic.interactable = false;
            }
        }
        else if (curRobot.Head.onSale == 1)
        {
            iconHead.gameObject.SetActive(true);
            imgHead.gameObject.SetActive(false);
            iconHead.sprite = GameManage.instance.AllHeadPieceImages[curRobot.Head.head_id];
            txtHeadMessage.text = " On Sale";
            txtHeadMessage.color = Color.yellow;
            txtHeadMedic.text = "Medic : " + curRobot.Head.medicament;
            txtHeadImagineTime.text = "Imagine Time: " + curRobot.Head.ImagineTime() + "h";
            txtHeadImagineTime.color = Color.white;
            txtHeadMessage.gameObject.SetActive(true);
            txtHeadMedic.gameObject.SetActive(true);
            txtHeadImagineTime.gameObject.SetActive(true);
            btnBuyHeadMedic.interactable = false;
        }
        else
        {
            iconHead.gameObject.SetActive(true);
            imgHead.gameObject.SetActive(false);
            iconHead.sprite = GameManage.instance.AllHeadPieceImages[curRobot.Head.head_id];
            txtHeadMessage.text = "Med : " + curRobot.Head.medicament.ToString();
            txtHeadMessage.color = Color.white;
            txtHeadMedic.text = "Medic : " + curRobot.Head.medicament;
            txtHeadImagineTime.text = "Imagine Time: " + curRobot.Head.ImagineTime() + "h";
            txtHeadImagineTime.color = Color.white;
            txtHeadMessage.gameObject.SetActive(false);
            txtHeadMedic.gameObject.SetActive(true);
            txtHeadImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyHeadMedic.interactable = true;
            }
            else
            {
                btnBuyHeadMedic.interactable = false;
            }
        }
        //LeftArm
        if (curRobot.LeftArm == null)
        {
            iconLeftArm.gameObject.SetActive(false);
            imgLeftArm.gameObject.SetActive(true);
            txtLeftArmMessage.text = "Lost or Sold";
            txtLeftArmMessage.color = Color.red;
            txtLeftArmMessage.gameObject.SetActive(true);
            txtLeftArmMedic.gameObject.SetActive(false);
            txtLeftArmImagineTime.gameObject.SetActive(false);
            btnBuyLeftArmMedic.interactable = false;
        }
        else if (curRobot.LeftArm.is_broken == 1)
        {
            iconLeftArm.gameObject.SetActive(true);
            imgLeftArm.gameObject.SetActive(false);
            iconLeftArm.sprite = GameManage.instance.AllLeftArmPieceImages[curRobot.LeftArm.larm_id];
            txtLeftArmMessage.text = "Broken";
            txtLeftArmMessage.color = Color.red;
            txtLeftArmMedic.text = "Medic : " + curRobot.LeftArm.medicament;
            txtLeftArmImagineTime.text = "Imagine Time: " + curRobot.LeftArm.ImagineTime() + "h";
            txtLeftArmImagineTime.color = Color.white;
            txtLeftArmMessage.gameObject.SetActive(true);
            txtLeftArmMedic.gameObject.SetActive(true);
            txtLeftArmImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyLeftArmMedic.interactable = true;
            }
            else
            {
                btnBuyLeftArmMedic.interactable = false;
            }
        }
        else if (curRobot.LeftArm.onSale == 1)
        {
            iconLeftArm.gameObject.SetActive(true);
            imgLeftArm.gameObject.SetActive(false);
            iconLeftArm.sprite = GameManage.instance.AllLeftArmPieceImages[curRobot.LeftArm.larm_id];
            txtLeftArmMessage.text = " On Sale";
            txtLeftArmMessage.color = Color.yellow;
            txtLeftArmMedic.text = "Medic : " + curRobot.LeftArm.medicament;
            txtLeftArmImagineTime.text = "Imagine Time: " + curRobot.LeftArm.ImagineTime() + "h";
            txtLeftArmImagineTime.color = Color.white;
            txtLeftArmMessage.gameObject.SetActive(true);
            txtLeftArmMedic.gameObject.SetActive(true);
            txtLeftArmImagineTime.gameObject.SetActive(true);
            btnBuyLeftArmMedic.interactable = false;
        }
        else
        {
            iconLeftArm.gameObject.SetActive(true);
            imgLeftArm.gameObject.SetActive(false);
            iconLeftArm.sprite = GameManage.instance.AllLeftArmPieceImages[curRobot.LeftArm.larm_id];
            txtLeftArmMessage.text = "Med : " + curRobot.LeftArm.medicament.ToString();
            txtLeftArmMessage.color = Color.white;
            txtLeftArmMedic.text = "Medic : " + curRobot.LeftArm.medicament;
            txtLeftArmImagineTime.text = "Imagine Time: " + curRobot.LeftArm.ImagineTime() + "h";
            txtLeftArmImagineTime.color = Color.white;
            txtLeftArmMessage.gameObject.SetActive(false);
            txtLeftArmMedic.gameObject.SetActive(true);
            txtLeftArmImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyLeftArmMedic.interactable = true;
            }
            else
            {
                btnBuyLeftArmMedic.interactable = false;
            }
        }
        //RightArm
        if (curRobot.RightArm == null)
        {
            iconRightArm.gameObject.SetActive(false);
            imgRightArm.gameObject.SetActive(true);
            txtRightArmMessage.text = "Lost or Sold";
            txtRightArmMessage.color = Color.red;
            txtRightArmMessage.gameObject.SetActive(true);
            txtRightArmMedic.gameObject.SetActive(false);
            txtRightArmImagineTime.gameObject.SetActive(false);
            btnBuyRightArmMedic.interactable = false;
        }
        else if (curRobot.RightArm.is_broken == 1)
        {
            iconRightArm.gameObject.SetActive(true);
            imgRightArm.gameObject.SetActive(false);
            iconRightArm.sprite = GameManage.instance.AllRightArmPieceImages[curRobot.RightArm.rarm_id];
            txtRightArmMessage.text = "Broken";
            txtRightArmMessage.color = Color.red;
            txtRightArmMedic.text = "Medic : " + curRobot.RightArm.medicament;
            txtRightArmImagineTime.text = "Imagine Time: " + curRobot.RightArm.ImagineTime() + "h";
            txtRightArmImagineTime.color = Color.white;
            txtRightArmMessage.gameObject.SetActive(true);
            txtRightArmMedic.gameObject.SetActive(true);
            txtRightArmImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyRightArmMedic.interactable = true;
            }
            else
            {
                btnBuyRightArmMedic.interactable = false;
            }
        }
        else if (curRobot.RightArm.onSale == 1)
        {
            iconRightArm.gameObject.SetActive(true);
            imgRightArm.gameObject.SetActive(false);
            iconRightArm.sprite = GameManage.instance.AllRightArmPieceImages[curRobot.RightArm.rarm_id];
            txtRightArmMessage.text = " On Sale";
            txtRightArmMessage.color = Color.yellow;
            txtRightArmMedic.text = "Medic : " + curRobot.RightArm.medicament;
            txtRightArmImagineTime.text = "Imagine Time: " + curRobot.RightArm.ImagineTime() + "h";
            txtRightArmImagineTime.color = Color.white;
            txtRightArmMessage.gameObject.SetActive(true);
            txtRightArmMedic.gameObject.SetActive(true);
            txtRightArmImagineTime.gameObject.SetActive(true);
            btnBuyRightArmMedic.interactable = false;
        }
        else
        {
            iconRightArm.gameObject.SetActive(true);
            imgRightArm.gameObject.SetActive(false);
            iconRightArm.sprite = GameManage.instance.AllRightArmPieceImages[curRobot.RightArm.rarm_id];
            txtRightArmMessage.text = "Med : " + curRobot.RightArm.medicament.ToString();
            txtRightArmMessage.color = Color.white;
            txtRightArmMedic.text = "Medic : " + curRobot.RightArm.medicament;
            txtRightArmImagineTime.text = "Imagine Time: " + curRobot.RightArm.ImagineTime() + "h";
            txtRightArmImagineTime.color = Color.white;
            txtRightArmMessage.gameObject.SetActive(false);
            txtRightArmMedic.gameObject.SetActive(true);
            txtRightArmImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyRightArmMedic.interactable = true;
            }
            else
            {
                btnBuyRightArmMedic.interactable = false;
            }
        }
        //Leg
        if (curRobot.Leg == null)
        {
            iconLeg.gameObject.SetActive(false);
            imgLeg.gameObject.SetActive(true);
            txtLegMessage.text = "Lost or Sold";
            txtLegMessage.color = Color.red;
            txtLegMessage.gameObject.SetActive(true);
            txtLegMedic.gameObject.SetActive(false);
            txtLegImagineTime.gameObject.SetActive(false);
            btnBuyLegMedic.interactable = false;
        }
        else if (curRobot.Leg.is_broken == 1)
        {
            iconLeg.gameObject.SetActive(true);
            imgLeg.gameObject.SetActive(false);
            iconLeg.sprite = GameManage.instance.AllLegPieceImages[curRobot.Leg.leg_id];
            txtLegMessage.text = "Broken";
            txtLegMessage.color = Color.red;
            txtLegMedic.text = "Medic : " + curRobot.Leg.medicament;
            txtLegImagineTime.text = "Imagine Time: " + curRobot.Leg.ImagineTime() + "h";
            txtLegImagineTime.color = Color.white;
            txtLegMessage.gameObject.SetActive(true);
            txtLegMedic.gameObject.SetActive(true);
            txtLegImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyLegMedic.interactable = true;
            }
            else
            {
                btnBuyLegMedic.interactable = false;
            }
        }
        else if (curRobot.Leg.onSale == 1)
        {
            iconLeg.gameObject.SetActive(true);
            imgLeg.gameObject.SetActive(false);
            iconLeg.sprite = GameManage.instance.AllLegPieceImages[curRobot.Leg.leg_id];
            txtLegMessage.text = " On Sale";
            txtLegMessage.color = Color.yellow;
            txtLegMedic.text = "Medic : " + curRobot.Leg.medicament;
            txtLegImagineTime.text = "Imagine Time: " + curRobot.Leg.ImagineTime() + "h";
            txtLegImagineTime.color = Color.white;
            txtLegMessage.gameObject.SetActive(true);
            txtLegMedic.gameObject.SetActive(true);
            txtLegImagineTime.gameObject.SetActive(true);
            btnBuyLegMedic.interactable = false;
        }
        else
        {
            iconLeg.gameObject.SetActive(true);
            imgLeg.gameObject.SetActive(false);
            iconLeg.sprite = GameManage.instance.AllLegPieceImages[curRobot.Leg.leg_id];
            txtLegMessage.text = "Med : " + curRobot.Leg.medicament.ToString();
            txtLegMessage.color = Color.white;
            txtLegMedic.text = "Medic : " + curRobot.Leg.medicament;
            txtLegImagineTime.text = "Imagine Time: " + curRobot.Leg.ImagineTime() + "h";
            txtLegImagineTime.color = Color.white;
            txtLegMessage.gameObject.SetActive(false);
            txtLegMedic.gameObject.SetActive(true);
            txtLegImagineTime.gameObject.SetActive(true);
            if (GameManage.User.gems >= 99)
            {
                btnBuyLegMedic.interactable = true;
            }
            else
            {
                btnBuyLegMedic.interactable = false;
            }
        }
        //Robot
        if (curRobot.onSale == 1)
        {
            txtHeadMessage.text = "On Sale";
            txtHeadMessage.color = Color.yellow;
            txtLeftArmMessage.text = "On Sale";
            txtLeftArmMessage.color = Color.yellow;
            txtRightArmMessage.text = "On Sale";
            txtRightArmMessage.color = Color.yellow;
            txtLegMessage.text = "On Sale";
            txtLegMessage.color = Color.yellow;

            btnBuyHeadMedic.interactable = false;
            btnBuyLeftArmMedic.interactable = false;
            btnBuyRightArmMedic.interactable = false;
            btnBuyLegMedic.interactable = false;
        }
    }

    public void OnBuyHeadMedic()
    {
        if (GameManage.User.gems >= 99)
        {
            //GameManage.User.gems -= 99;

            curRobot.Head.medicament++;
            GameManage.instance.ChangeHead(GB.g_MyID, curRobot.Head, callbackUserInfo);
        }

        //GameManage.User.UpdateLocalFile();
        //Init();
    }

    public void OnBuyLeftArmMedic()
    {
        if (GameManage.User.gems >= 99)
        {
            //GameManage.User.gems -= 99;

            curRobot.LeftArm.medicament++;
            GameManage.instance.ChangeLeftArm(GB.g_MyID, curRobot.LeftArm, callbackUserInfo);
        }

        //GameManage.User.UpdateLocalFile();
        //Init();
    }

    public void OnBuyRightArmMedic()
    {
        if (GameManage.User.gems >= 99)
        {
            //GameManage.User.gems -= 99;

            curRobot.RightArm.medicament++;
            GameManage.instance.ChangeRightArm(GB.g_MyID, curRobot.RightArm, callbackUserInfo);
        }

        //GameManage.User.UpdateLocalFile();
        //Init();
    }

    public void OnBuyLegMedic()
    {
        if (GameManage.User.gems >= 99)
        {
            //GameManage.User.gems -= 99;

            curRobot.Leg.medicament++;
            GameManage.instance.ChangeLegs(GB.g_MyID, curRobot.Leg, callbackUserInfo);
        }

        //GameManage.User.UpdateLocalFile();
        //Init();
    }
    //PlayerPrefs

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
                Debug.Log("GetUserInfo: Done");
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

                GameManage.instance.UpdateUserGems(GB.g_MyID, -99, callbackUserGems);
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
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
