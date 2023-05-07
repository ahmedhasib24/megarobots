using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellSubPanel : MonoBehaviour
{
    public Text txtRobotLevel, txtHeadLevel, txtLeftArmLevel, txtRightArmLevel, txtLegLevel;
    public InputField ifRobotPrice, ifHeadPrice, ifLeftArmPrice, ifRightArmPrice, ifLegPrice;
    public Button btnRobotSell, btnHeadSell, btnLeftArmSell, btnRightArmSell, btnLegSell;

    Robot robot;

    public float robotPrice, headPrice, lArmPrice, rArmPrice, legPrice;

    PieceType type;
    public void ShowItemsData(Robot robot)
    {
        this.robot = robot;

        //Head
        if (robot.Head == null)
        {
            txtHeadLevel.text = "Lost or Sold";
            txtHeadLevel.color = Color.red;
            ifHeadPrice.text = 0.ToString();
            btnHeadSell.interactable = false;
        }
        else if (robot.Head.is_broken == 1)
        {
            txtHeadLevel.text = "Broken";
            txtHeadLevel.color = Color.red;
            ifHeadPrice.text = 0.ToString();
            btnHeadSell.interactable = false;
        }
        else
        {
            txtHeadLevel.text = "Level : " + robot.Head.level.ToString();
            txtHeadLevel.color = Color.white;
            if (robot.Head.salePrice == 0)
            {
                ifHeadPrice.text = robot.Head.price.ToString();
            }
            else
            {
                ifHeadPrice.text = robot.Head.salePrice.ToString();
            }
            
            if (robot.Head.onSale == 1)
            {
                txtHeadLevel.text = "On Sale";
                txtHeadLevel.color = Color.yellow;
                btnHeadSell.interactable = false;
            }
            else
            {
                btnHeadSell.interactable = true;
            }
        }
        //LeftArm
        if (robot.LeftArm == null)
        {
            txtLeftArmLevel.text = "Lost or Sold";
            txtLeftArmLevel.color = Color.red;
            ifLeftArmPrice.text = 0.ToString();
            btnLeftArmSell.interactable = false;
        }
        else if (robot.LeftArm.is_broken == 1)
        {
            txtLeftArmLevel.text = "Broken";
            txtLeftArmLevel.color = Color.red;
            ifLeftArmPrice.text = 0.ToString();
            btnLeftArmSell.interactable = false;
        }
        else
        {
            txtLeftArmLevel.text = "Level : " + robot.LeftArm.level.ToString();
            txtLeftArmLevel.color = Color.white;
            if (robot.LeftArm.salePrice == 0)
            {
                ifLeftArmPrice.text = robot.LeftArm.price.ToString();
            }
            else
            {
                ifLeftArmPrice.text = robot.LeftArm.salePrice.ToString();
            }

            if (robot.LeftArm.onSale == 1)
            {
                txtLeftArmLevel.text = "On Sale";
                txtLeftArmLevel.color = Color.yellow;
                btnLeftArmSell.interactable = false;
            }
            else
            {
                btnLeftArmSell.interactable = true;
            }
        }
        //RightArm
        if (robot.RightArm == null)
        {
            txtRightArmLevel.text = "Lost or Sold";
            txtRightArmLevel.color = Color.red;
            ifRightArmPrice.text = 0.ToString();
            btnRightArmSell.interactable = false;
        }
        else if (robot.RightArm.is_broken == 1)
        {
            txtRightArmLevel.text = "Broken";
            txtRightArmLevel.color = Color.red;
            ifRightArmPrice.text = 0.ToString();
            btnRightArmSell.interactable = false;
        }
        else
        {
            txtRightArmLevel.text = "Level : " + robot.RightArm.level.ToString();
            txtRightArmLevel.color = Color.white;
            if (robot.RightArm.salePrice == 0)
            {
                ifRightArmPrice.text = robot.RightArm.price.ToString();
            }
            else
            {
                ifRightArmPrice.text = robot.RightArm.salePrice.ToString();
            }

            if (robot.RightArm.onSale == 1)
            {
                txtRightArmLevel.text = "On Sale";
                txtRightArmLevel.color = Color.yellow;
                btnRightArmSell.interactable = false;
            }
            else
            {
                btnRightArmSell.interactable = true;
            }
        }
        //Leg
        if (robot.Leg == null)
        {
            txtLegLevel.text = "Lost or Sold";
            txtLegLevel.color = Color.red;
            ifLegPrice.text = 0.ToString();
            btnLegSell.interactable = false;
        }
        else if (robot.Leg.is_broken == 1)
        {
            txtLegLevel.text = "Broken";
            txtLegLevel.color = Color.red;
            ifLegPrice.text = 0.ToString();
            btnLegSell.interactable = false;
        }
        else
        {
            txtLegLevel.text = "Level : " + robot.Leg.level.ToString();
            txtLegLevel.color = Color.white;
            if (robot.Leg.salePrice == 0)
            {
                ifLegPrice.text = robot.Leg.price.ToString();
            }
            else
            {
                ifLegPrice.text = robot.Leg.salePrice.ToString();
            }

            if (robot.Leg.onSale == 1)
            {
                txtLegLevel.text = "On Sale";
                txtLegLevel.color = Color.yellow;
                btnLegSell.interactable = false;
            }
            else
            {
                btnLegSell.interactable = true;
            }
        }
        //Robot
        if (robot.Level == 0)
        {
            txtRobotLevel.text = "Not Complete";
            txtRobotLevel.color = Color.red;
            ifRobotPrice.text = 0.ToString();
            btnRobotSell.interactable = false;
        }
        else if (robot.Head.is_broken == 1 || robot.LeftArm.is_broken == 1 || robot.RightArm.is_broken == 1 || robot.Leg.is_broken == 1)
        {
            txtRobotLevel.text = "Broken";
            txtRobotLevel.color = Color.red;
            ifRobotPrice.text = 0.ToString();
            btnRobotSell.interactable = false;
        }
        else
        {
            if (robot.Head.onSale == 0 && robot.LeftArm.onSale == 0 && robot.RightArm.onSale == 0 && robot.Leg.onSale == 0)
            {
                txtRobotLevel.text = "Level : " + robot.Level.ToString();
                txtRobotLevel.color = Color.white;
                if (robot.salePrice == 0)
                {
                    ifRobotPrice.text = robot.price.ToString();
                }
                else
                {
                    ifRobotPrice.text = robot.salePrice.ToString();
                }

                if (robot.onSale == 1)
                {
                    txtRobotLevel.text = "On Sale";
                    txtRobotLevel.color = Color.yellow;

                    btnRobotSell.interactable = false;
                    btnHeadSell.interactable = false;
                    btnLeftArmSell.interactable = false;
                    btnRightArmSell.interactable = false;
                    btnLegSell.interactable = false;
                }
                else
                {
                    btnRobotSell.interactable = true;
                }
            }
            else
            {
                txtRobotLevel.text = "Parts On Sale";
                txtRobotLevel.color = Color.yellow;
                ifRobotPrice.text = 0.ToString();
                btnRobotSell.interactable = false;
            }
        }

        if ((GameManage.User.Robots.Count < 2))
        {
            btnRobotSell.interactable = false;
            btnHeadSell.interactable = false;
            btnLeftArmSell.interactable = false;
            btnRightArmSell.interactable = false;
            btnLegSell.interactable = false;
        }
    }

    public void SellRobot()
    {
        robotPrice = float.Parse(ifRobotPrice.text);
        RobotMallManager.instance.SellRobot(robot, robotPrice, callbackUserInfo);
    }

    public void SellHead()
    {
        type = PieceType.Head;

        Head head = GameManage.User.GetHead(robot.head_object_id);
        headPrice = float.Parse(ifHeadPrice.text);
        RobotMallManager.instance.SellHead(head, headPrice, callbackUserInfo);
    }

    public void SellLeftArm()
    {
        type = PieceType.LeftArm;

        LeftArm larm = GameManage.User.GetLeftArm(robot.larm_object_id);
        lArmPrice = float.Parse(ifLeftArmPrice.text);
        RobotMallManager.instance.SellLeftArm(larm, lArmPrice, callbackUserInfo);
    }

    public void SellRightArm()
    {
        type = PieceType.RightArm;

        RightArm rarm = GameManage.User.GetRightArm(robot.rarm_object_id);
        rArmPrice = float.Parse(ifRightArmPrice.text);
        RobotMallManager.instance.SellRightArm(rarm, rArmPrice, callbackUserInfo);
    }

    public void SellLeg()
    {
        type = PieceType.Leg;

        Leg leg = GameManage.User.GetLeg(robot.leg_object_id);
        legPrice = float.Parse(ifLegPrice.text);
        RobotMallManager.instance.SellLeg(leg, legPrice, callbackUserInfo);
    }

    #region callbacks
    private int callbackSellRobot(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("SellRobot: Done");
                Debug.Log(www.text);
                robot.onSale = int.Parse(json["result"]["onSale"].ToString());
                robot.salePrice = int.Parse(json["result"]["price"].ToString());
                Inventory.instance.ShowSubPanel(0);
            }
            else if (status == "fail")
            {
                Debug.Log("SellRobot: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("SellRobot: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                //Add item to store
                robot.onSale = 1;
                robot.salePrice = robotPrice;
                GameManage.User.UpdateLocalFile();
                Inventory.instance.ShowSubPanel(0);
            }
        }
        return 0;
    }

    private int callbackSellPiece(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("SellPiece: Done");
                Debug.Log(www.text);
                PieceType type = (PieceType)Enum.Parse(typeof(PieceType), json["result"]["type"].ToString());
                switch (type)
                {
                    case PieceType.Head:
                        robot.Head.onSale = int.Parse(json["result"]["onSale"].ToString());
                        robot.Head.salePrice = float.Parse(json["result"]["price"].ToString());
                        break;
                    case PieceType.LeftArm:
                        robot.LeftArm.onSale = int.Parse(json["result"]["onSale"].ToString());
                        robot.LeftArm.salePrice = float.Parse(json["result"]["price"].ToString());
                        break;
                    case PieceType.RightArm:
                        robot.RightArm.onSale = int.Parse(json["result"]["onSale"].ToString());
                        robot.RightArm.salePrice = float.Parse(json["result"]["price"].ToString());
                        break;
                    case PieceType.Leg:
                        robot.Leg.onSale = int.Parse(json["result"]["onSale"].ToString());
                        robot.Leg.salePrice = float.Parse(json["result"]["price"].ToString());
                        break;
                }

                Inventory.instance.ShowSubPanel(0);
            }
            else if (status == "fail")
            {
                Debug.Log("SellPiece: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("SellPiece: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                //Add item to store
                switch (type)
                {
                    case PieceType.Head:
                        robot.Head.onSale = 1;
                        robot.Head.salePrice = headPrice;
                        break;
                    case PieceType.LeftArm:
                        robot.LeftArm.onSale = 1;
                        robot.LeftArm.salePrice = lArmPrice;
                        break;
                    case PieceType.RightArm:
                        robot.RightArm.onSale = 1;
                        robot.RightArm.salePrice = rArmPrice;
                        break;
                    case PieceType.Leg:
                        robot.Leg.onSale = 1;
                        robot.Leg.salePrice = legPrice;
                        break;
                }
                GameManage.User.UpdateLocalFile();
                Inventory.instance.ShowSubPanel(0);
            }
        }
        return 0;
    }

    private int callbackUserInfo(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("Sell robot or piece: Done");
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

                MainMenuPanel.instance.Refresh();
                Inventory.instance.ShowSubPanel(0);
                Inventory.instance.Init();
            }
            else if (status == "fail")
            {
                Debug.Log("Sell robot or piece: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("Sell robot or piece: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
        }
        return 0;
    }
    #endregion
}
