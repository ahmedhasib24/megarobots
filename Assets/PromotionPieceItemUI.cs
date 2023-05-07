using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionPieceItemUI : MonoBehaviour
{
    public Text txtPrice;
    public Text oldPrice;
    public Text newPrice;
    public Image imgItemThumb;

    public GameObject badge1, badge2, buyButton1, buyButton2;

    public PromotionPieceItem config;

    public void Init(PromotionPieceItem config)
    {
        this.config = config;

        buyButton1.GetComponent<Button>().onClick.AddListener(OnBuyNewButtonClick);
        buyButton2.GetComponent<Button>().onClick.AddListener(OnBuyNewButtonClick);

        switch ((int)config.piece_type)
        {
            case 0:
                imgItemThumb.sprite = GameManage.instance.AllHeadPieceImages[config.piece_id];
                break;
            case 1:
                imgItemThumb.sprite = GameManage.instance.AllLeftArmPieceImages[config.piece_id];
                break;
            case 2:
                imgItemThumb.sprite = GameManage.instance.AllRightArmPieceImages[config.piece_id];
                break;
            case 3:
                imgItemThumb.sprite = GameManage.instance.AllLegPieceImages[config.piece_id];
                break;
        }

        if (config.is_new == 1 && config.is_discounted == 0)
        {
            badge1.SetActive(true);
            badge2.SetActive(false);

            buyButton1.SetActive(true);
            buyButton2.SetActive(false);

            txtPrice.text = ((int)config.new_price).ToString();
            oldPrice.gameObject.SetActive(false);
            newPrice.gameObject.SetActive(false);

            if (GameManage.User.gems >= config.new_price)
            {
                buyButton1.GetComponent<Button>().interactable = true;
                buyButton2.GetComponent<Button>().interactable = true;
            }
            else
            {
                buyButton1.GetComponent<Button>().interactable = false;
                buyButton2.GetComponent<Button>().interactable = false;
            }
        }
        else if (config.is_new == 0 && config.is_discounted == 1)
        {
            badge1.SetActive(false);
            badge2.SetActive(true);

            buyButton1.SetActive(false);
            buyButton2.SetActive(true);

            txtPrice.text = ((int)config.new_price).ToString();
            oldPrice.gameObject.SetActive(true);
            newPrice.gameObject.SetActive(true);
            oldPrice.text = ((int)config.old_price).ToString();
            newPrice.text = ((int)config.new_price).ToString();

            if (GameManage.User.gems >= config.new_price)
            {
                buyButton1.GetComponent<Button>().interactable = true;
                buyButton2.GetComponent<Button>().interactable = true;
            }
            else
            {
                buyButton1.GetComponent<Button>().interactable = false;
                buyButton2.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void OnBuyNewButtonClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (config.new_price <= GameManage.User.gems)
        {
            BuyPiecePromotion(config, callbackUserInfo);
            //GameManage.User.gems -= (int)config.new_price;


            //PieceType type = (PieceType)config.piece_type;
            //switch(type)
            //{
            //    case PieceType.Head:
            //        Head basicHead = GameManage.AllHeads.Find(x => x.head_id == config.piece_id);
            //        Head head = new Head();
            //        head.object_id = GameManage.User.Heads.Count;
            //        head.head_id = basicHead.head_id;
            //        head.type = basicHead.type;
            //        head.name = basicHead.name;
            //        head.level = config.level;
            //        head.price = basicHead.price;
            //        head.medicament = basicHead.medicament;
            //        head.life = (basicHead.life / 5) * config.level;
            //        head.attack = (basicHead.attack / 5) * config.level;
            //        head.defence = (basicHead.defence / 5) * config.level;
            //        head.onSale = basicHead.onSale;
            //        head.salePrice = basicHead.salePrice;

            //        GameManage.User.Heads.Add(head);
            //        break;

            //    case PieceType.LeftArm:
            //        LeftArm basicLarm = GameManage.AllLeftArms.Find(x => x.larm_id == config.piece_id);
            //        LeftArm larm = new LeftArm();
            //        larm.object_id = GameManage.User.LeftArms.Count;
            //        larm.larm_id = basicLarm.larm_id;
            //        larm.type = basicLarm.type;
            //        larm.name = basicLarm.name;
            //        larm.level = config.level;
            //        larm.price = basicLarm.price;
            //        larm.medicament = basicLarm.medicament;
            //        larm.life = (basicLarm.life / 5) * config.level;
            //        larm.attack = (basicLarm.attack / 5) * config.level;
            //        larm.defence = (basicLarm.defence / 5) * config.level;
            //        larm.onSale = basicLarm.onSale;
            //        larm.salePrice = basicLarm.salePrice;
            //        GameManage.User.LeftArms.Add(larm);

            //        GameManage.User.LeftArms.Add(larm);
            //        break;

            //    case PieceType.RightArm:
            //        RightArm basicRarm = GameManage.AllRightArms.Find(x => x.rarm_id == config.piece_id);
            //        RightArm rarm = new RightArm();
            //        rarm.object_id = GameManage.User.RightArms.Count;
            //        rarm.rarm_id = basicRarm.rarm_id;
            //        rarm.type = basicRarm.type;
            //        rarm.name = basicRarm.name;
            //        rarm.level = config.level;
            //        rarm.price = basicRarm.price;
            //        rarm.medicament = basicRarm.medicament;
            //        rarm.life = (basicRarm.life / 5) * config.level;
            //        rarm.attack = (basicRarm.attack / 5) * config.level;
            //        rarm.defence = (basicRarm.defence / 5) * config.level;
            //        rarm.onSale = basicRarm.onSale;
            //        rarm.salePrice = basicRarm.salePrice;

            //        GameManage.User.RightArms.Add(rarm);
            //        break;

            //    case PieceType.Leg:
            //        Leg basicLeg = GameManage.AllLegs.Find(x => x.leg_id == config.piece_id);
            //        Leg leg = new Leg();
            //        leg.object_id = GameManage.User.Legs.Count;
            //        leg.leg_id = basicLeg.leg_id;
            //        leg.type = basicLeg.type;
            //        leg.name = basicLeg.name;
            //        leg.level = config.level;
            //        leg.price = basicLeg.price;
            //        leg.medicament = basicLeg.medicament;
            //        leg.life = (basicLeg.life / 5) * config.level;
            //        leg.attack = (basicLeg.attack / 5) * config.level;
            //        leg.defence = (basicLeg.defence / 5) * config.level;
            //        leg.velocity = (basicLeg.velocity / 5) * config.level;
            //        leg.onSale = basicLeg.onSale;
            //        leg.salePrice = basicLeg.salePrice;

            //        GameManage.User.Legs.Add(leg);
            //        break;
            //}
            //GameManage.User.UpdateLocalFile();
            //StoreManager.instance.DailyPanel.GetComponent<Daily>().ShowPieceBoughtText("Successfully added to inventory.");
            //StoreManager.instance.DailyPanel.SetActive(false);
        }
        else
        {
            StoreManager.instance.DailyPanel.GetComponent<Daily>().ShowPieceBoughtText("Not Enough gems.");
        }
    }

    public void BuyPiecePromotion(PromotionPieceItem p_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("BuyPiecePromotion");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("item_id", p_mall.item_id);


        GameManage.Get("/" + GB.g_APIBuyPiecePromotion, data, callbackFunction);
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

                    GameManage.User.AddLeg(lc);
                }

                MainMenuPanel.instance.Refresh();
                StoreManager.instance.DailyPanel.GetComponent<Daily>().ShowRobotBoughtText("Successfully added to inventory.");
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        return 0;
    }
}
