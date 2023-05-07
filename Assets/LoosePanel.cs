using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LooseBluePrint
{
    public string name;
    public string type;
    public int price;
    public int itemId;
}

public class LoosePanel : MonoBehaviour
{
    public Image imgLooseItemThumb;
    public Text txtName, txtType, txtPrice, txtMessage;

    public Text videoAdText, txtLooseDescription;
    public GameObject buttonBuy, buttonNo, buttonWatchVideo, returnMenu, looseItem;

    //public List<GameObject> rewardUiItemList;

    LooseBluePrint looseBluePrint;

    public void Init(LooseBluePrint looseBluePrint = null)
    {
        //Advertisement.Instance.HideBannerAd();
        if (looseBluePrint != null)
        {
            this.looseBluePrint = looseBluePrint;

            txtName.text = looseBluePrint.name;
            txtType.text = looseBluePrint.type;
            txtPrice.text = ((int)(looseBluePrint.price - (looseBluePrint.price * 0.7f))).ToString();

            if (looseBluePrint.type.ToUpper() == "ROBOT")
            {
                imgLooseItemThumb.sprite = GameManage.instance.AllBasicRobotImages[looseBluePrint.itemId];
            }
            else
            {
                if (looseBluePrint.type.ToUpper() == "HEAD")
                {
                    imgLooseItemThumb.sprite = GameManage.instance.AllHeadPieceImages[looseBluePrint.itemId];
                }
                else if (looseBluePrint.type.ToUpper() == "LEFT ARM")
                {
                    imgLooseItemThumb.sprite = GameManage.instance.AllLeftArmPieceImages[looseBluePrint.itemId];
                }
                else if (looseBluePrint.type.ToUpper() == "RIGHT ARM")
                {
                    imgLooseItemThumb.sprite = GameManage.instance.AllRightArmPieceImages[looseBluePrint.itemId];
                }
                else if (looseBluePrint.type.ToUpper() == "LEG")
                {
                    imgLooseItemThumb.sprite = GameManage.instance.AllLegPieceImages[looseBluePrint.itemId];
                }
            }

            if (GameManage.User.gems >= looseBluePrint.price)
            {
                txtLooseDescription.text = "We give you a discount to\nbuy your robot or piece,\nif not, you will loose it.";
                buttonBuy.SetActive(true);
                buttonNo.SetActive(true);
                returnMenu.SetActive(false);
            }
            else
            {
                if (looseBluePrint.type == "Robot")
                {
                    txtLooseDescription.text = "You will loose Robot";
                }
                else
                {
                    txtLooseDescription.text = "You will loose Piece";
                }
                buttonBuy.SetActive(false);
                buttonNo.SetActive(false);
                returnMenu.SetActive(true);
            }
            looseItem.SetActive(true);

            if (PlayManage.instance.minePartsDestroyed > 0)
            {
                // if (Advertisement.Instance.IsRewardedAdReady())
                // {
                //     buttonWatchVideo.SetActive(true);
                //     videoAdText.text = "Watch 2 Videos";
                // }
            }
            else
            {
                buttonWatchVideo.SetActive(false);
            }
        }
        else
        {
            looseItem.SetActive(false);

            buttonBuy.SetActive(false);
            buttonNo.SetActive(false);
            returnMenu.SetActive(true);

            if (PlayManage.instance.minePartsDestroyed > 0)
            {
                buttonWatchVideo.SetActive(true);
                videoAdText.text = "Watch " + PlayManage.instance.videoAdCount + " Videos";
            }
            else
            {
                buttonWatchVideo.SetActive(false);
            }
        }
        txtMessage.gameObject.SetActive(false);
    }

    public void OnBuyButton()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //GameManage.User.gems -= (int)(looseBluePrint.price - (looseBluePrint.price * 0.7f));
        GameManage.instance.UpdateUserGems(GB.g_MyID, -(int)(looseBluePrint.price - (looseBluePrint.price * 0.7f)), callbackUserInfo);
        //GameManage.User.UpdateLocalFile();
        PlayManage.instance.BreakPieces();

        StartCoroutine(WaitForReturnToMenu());
    }

    public void OnNoButton()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        PlayManage.instance.Loose();

        StartCoroutine(WaitForReturnToMenu());
    }

    IEnumerator WaitForReturnToMenu()
    {
        while (LoadingPanel.instance.taskCompleted != LoadingPanel.instance.totalTask)
        {
            yield return new WaitForSeconds(0.5f);
        }

        PlayManage.instance.playcs.ReturnToMenu();
    }

    public void OnVideoAdButton()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //Advertisement.Instance.ShowVideoAd();
    }

    public void OnReturnMenuButton()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        PlayManage.instance.playcs.ReturnToMenu();
    }

    public void WatchedVideoAd()
    {
        if (PlayManage.instance.videoAdCount == 0)
        {
            buttonWatchVideo.SetActive(false);
        }
        else
        {
            videoAdText.text = "Watch " + PlayManage.instance.videoAdCount + " Videos";
        }
    }

    public void ShowMedicamentMessage()
    {
        StartCoroutine(ShowMessageRoutine());
    }

    IEnumerator ShowMessageRoutine()
    {
        txtMessage.text = "You got 1 medicament!";
        txtMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        txtMessage.gameObject.SetActive(false);
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
                Debug.Log("GetUserInfo: Done");
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
            }
            else if (status == "fail")
            {
                Debug.LogError("GetUserInfo: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("GetUserInfo: Error");
            Debug.Log(www.error);
        }
        return 0;
    }
    #endregion
}
