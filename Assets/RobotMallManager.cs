using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum MallItemType
{
    Robot,
    Piece
}

[Serializable]
public class RobotMallItem
{
    public int item_id;
    public int object_id;
    public int seller_id;
    public string seller_name;
    public int seller_avater_id;
    public float price;

    public int head_id;
    public int larm_id;
    public int rarm_id;
    public int leg_id;

    public string name;
    public int level;
    public float imagine_time;
    public float life;
    public float attack;
    public float defence;
    public float velocity;

}

[Serializable]
public class PieceMallItem
{
    public int item_id;
    public int object_id;
    public PieceType piece_type;
    public int piece_id;
    public int seller_id;
    public string seller_name;
    public int seller_avater_id;
    public float price;

    public string name;
    public int level;
    public float imagine_time;
    public float life;
    public float attack;
    public float defence;
    public float velocity;
}

[Serializable]
public class RobotMallData
{
    public List<RobotMallItem> robots;
    public List<PieceMallItem> pieces;
}

public class RobotMallManager : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static RobotMallManager s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static RobotMallManager instance {
        get {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(RobotMallManager)) as RobotMallManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate an RobotMallManager object. \n You have to have exactly one PR_Utility in the scene.");
            }
            return s_Instance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        s_Instance = null;
    }
    #endregion

    public TextAsset mallItemJson;

    public List<RobotMallItem> robotMallItems = new List<RobotMallItem>();
    public List<PieceMallItem> pieceMallItems = new List<PieceMallItem>();

    #region utils
    public string toJson()
    {
        RobotMallData data = new RobotMallData();
        data.robots = robotMallItems;
        data.pieces = pieceMallItems;

        //string strRobotsJSON = JsonUtility.ToJson(robotMallItems, true);
        //string strPiecesJSON = JsonUtility.ToJson(pieceMallItems, true);
        string strJSON = JsonUtility.ToJson(data, true);
        //string strJSON = @"{""status"":""success"", ""result"":{""robots"":" + strRobotsJSON + "," + @"""pieces"":" + strPiecesJSON + "}}";
        strJSON = @"{""status"":""success"", ""result"":" + strJSON + "}";
        //Debug.LogError(strJSON);
        return strJSON;
    }

    public void UpdateLocalFile()
    {
        string filepath = GameManage.DataPath("TextAssets/mall_items.json");
        string result = string.Empty;
        using (StreamReader r = new StreamReader(filepath))
        {
            result = toJson();
            //Debug.Log(result);
        }
        File.WriteAllText(filepath, result);
    }
    #endregion

    private void Start()
    {
        
    }

    public void RefreshMallItems()
    {
        GetMallItems(callbackGetMallItems);
    }

    public void GetMallItems(Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetMallItems");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);

        GameManage.Get("/" + GB.g_APIGetMallItems, data, callbackFunction);
    }

    private int callbackGetMallItems(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetMallItems: Done");
                Debug.Log(www.text);
                robotMallItems = new List<RobotMallItem>();
                for (int i = 0; i < json/*["result"]*/["robots"].Count; i++)
                {
                    RobotMallItem rmi = new RobotMallItem();
                    rmi.item_id = int.Parse(json["robots"][i]["item_id"].ToString());
                    rmi.object_id = int.Parse(json["robots"][i]["object_id"].ToString());
                    rmi.seller_id = int.Parse(json["robots"][i]["seller_id"].ToString());
                    rmi.seller_name = json["robots"][i]["seller_name"].ToString();
                    rmi.seller_avater_id = int.Parse(json["robots"][i]["seller_avater_id"].ToString());
                    rmi.price = float.Parse(json["robots"][i]["price"].ToString());

                    rmi.head_id = int.Parse(json["robots"][i]["head_id"].ToString());
                    rmi.larm_id = int.Parse(json["robots"][i]["larm_id"].ToString());
                    rmi.rarm_id = int.Parse(json["robots"][i]["rarm_id"].ToString());
                    rmi.leg_id = int.Parse(json["robots"][i]["leg_id"].ToString());

                    rmi.name = json["robots"][i]["name"].ToString();
                    rmi.level = int.Parse(json["robots"][i]["level"].ToString());
                    rmi.imagine_time = int.Parse(json["robots"][i]["imagine_time"].ToString());
                    rmi.life = float.Parse(json["robots"][i]["life"].ToString());
                    rmi.attack = float.Parse(json["robots"][i]["attack"].ToString());
                    rmi.defence = float.Parse(json["robots"][i]["defence"].ToString());
                    rmi.velocity = float.Parse(json["robots"][i]["velocity"].ToString());

                    robotMallItems.Add(rmi);
                }

                pieceMallItems = new List<PieceMallItem>();
                for (int i = 0; i < json["pieces"].Count; i++)
                {
                    PieceMallItem pmi = new PieceMallItem();
                    pmi.item_id = int.Parse(json["pieces"][i]["item_id"].ToString());
                    pmi.object_id = int.Parse(json["pieces"][i]["object_id"].ToString());
                    pmi.piece_type = (PieceType)Enum.Parse(typeof(PieceType), json["pieces"][i]["piece_type"].ToString());
                    pmi.piece_id = int.Parse(json["pieces"][i]["piece_id"].ToString());
                    pmi.seller_id = int.Parse(json["pieces"][i]["seller_id"].ToString());
                    pmi.seller_name = json["pieces"][i]["seller_name"].ToString();
                    pmi.seller_avater_id = int.Parse(json["pieces"][i]["seller_avater_id"].ToString());
                    pmi.price = float.Parse(json["pieces"][i]["price"].ToString());

                    pmi.name = json["pieces"][i]["name"].ToString();
                    pmi.level = int.Parse(json["pieces"][i]["level"].ToString());
                    pmi.imagine_time = int.Parse(json["pieces"][i]["imagine_time"].ToString());
                    pmi.life = float.Parse(json["pieces"][i]["life"].ToString());
                    pmi.attack = float.Parse(json["pieces"][i]["attack"].ToString());
                    pmi.defence = float.Parse(json["pieces"][i]["defence"].ToString());
                    pmi.velocity = float.Parse(json["pieces"][i]["velocity"].ToString());

                    pieceMallItems.Add(pmi);
                }

                Mall.instance.ConfigureMallItems();
            }
            else if (status == "fail")
            {
                Debug.Log("GetMallItems: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetMallItems: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(mallItemJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    robotMallItems = new List<RobotMallItem>();
                    for (int i = 0; i < json["result"]["robots"].Count; i++)
                    {
                        RobotMallItem rmi = new RobotMallItem();
                        rmi.object_id = int.Parse(json["result"]["robots"][i]["object_id"].ToString());
                        rmi.seller_id = int.Parse(json["result"]["robots"][i]["seller_id"].ToString());
                        rmi.seller_name = json["result"]["robots"][i]["seller_name"].ToString();
                        rmi.seller_avater_id = int.Parse(json["result"]["robots"][i]["seller_avater_id"].ToString());
                        rmi.price = float.Parse(json["result"]["robots"][i]["price"].ToString());

                        rmi.head_id = int.Parse(json["result"]["robots"][i]["head_id"].ToString());
                        rmi.larm_id = int.Parse(json["result"]["robots"][i]["larm_id"].ToString());
                        rmi.rarm_id = int.Parse(json["result"]["robots"][i]["rarm_id"].ToString());
                        rmi.leg_id = int.Parse(json["result"]["robots"][i]["leg_id"].ToString());

                        rmi.name = json["result"]["robots"][i]["robot_name"].ToString();
                        rmi.level = int.Parse(json["result"]["robots"][i]["level"].ToString());
                        rmi.imagine_time = int.Parse(json["result"]["robots"][i]["imagine_time"].ToString());
                        rmi.life = float.Parse(json["result"]["robots"][i]["life"].ToString());
                        rmi.attack = float.Parse(json["result"]["robots"][i]["attack"].ToString());
                        rmi.defence = float.Parse(json["result"]["robots"][i]["defence"].ToString());
                        rmi.velocity = float.Parse(json["result"]["robots"][i]["velocity"].ToString());

                        robotMallItems.Add(rmi);
                    }

                    pieceMallItems = new List<PieceMallItem>();
                    for (int i = 0; i < json["result"]["pieces"].Count; i++)
                    {
                        PieceMallItem pmi = new PieceMallItem();
                        pmi.object_id = int.Parse(json["result"]["pieces"][i]["object_id"].ToString());
                        pmi.piece_type = (PieceType)Enum.Parse(typeof(PieceType), json["result"]["pieces"][i]["piece_type"].ToString());
                        pmi.piece_id = int.Parse(json["result"]["pieces"][i]["piece_id"].ToString());
                        pmi.seller_id = int.Parse(json["result"]["pieces"][i]["seller_id"].ToString());
                        pmi.seller_name = json["result"]["pieces"][i]["seller_name"].ToString();
                        pmi.seller_avater_id = int.Parse(json["result"]["pieces"][i]["seller_avater_id"].ToString());
                        pmi.price = float.Parse(json["result"]["pieces"][i]["price"].ToString());

                        pmi.level = int.Parse(json["result"]["pieces"][i]["level"].ToString());
                        pmi.imagine_time = int.Parse(json["result"]["pieces"][i]["imagine_time"].ToString());
                        pmi.life = float.Parse(json["result"]["pieces"][i]["life"].ToString());
                        pmi.attack = float.Parse(json["result"]["pieces"][i]["attack"].ToString());
                        pmi.defence = float.Parse(json["result"]["pieces"][i]["defence"].ToString());
                        pmi.velocity = float.Parse(json["result"]["pieces"][i]["velocity"].ToString());

                        pieceMallItems.Add(pmi);
                    }
                }
            }
        }
        return 0;
    }

    private int callbackGetMallItemsRefresh(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetMallItems: Done");
                Debug.Log(www.text);
                robotMallItems = new List<RobotMallItem>();
                for (int i = 0; i < json/*["result"]*/["robots"].Count; i++)
                {
                    RobotMallItem rmi = new RobotMallItem();
                    rmi.item_id = int.Parse(json["robots"][i]["item_id"].ToString());
                    rmi.object_id = int.Parse(json["robots"][i]["object_id"].ToString());
                    rmi.seller_id = int.Parse(json["robots"][i]["seller_id"].ToString());
                    rmi.seller_name = json["robots"][i]["seller_name"].ToString();
                    rmi.seller_avater_id = int.Parse(json["robots"][i]["seller_avater_id"].ToString());
                    rmi.price = float.Parse(json["robots"][i]["price"].ToString());

                    rmi.head_id = int.Parse(json["robots"][i]["head_id"].ToString());
                    rmi.larm_id = int.Parse(json["robots"][i]["larm_id"].ToString());
                    rmi.rarm_id = int.Parse(json["robots"][i]["rarm_id"].ToString());
                    rmi.leg_id = int.Parse(json["robots"][i]["leg_id"].ToString());

                    rmi.name = json["robots"][i]["name"].ToString();
                    rmi.level = int.Parse(json["robots"][i]["level"].ToString());
                    rmi.imagine_time = int.Parse(json["robots"][i]["imagine_time"].ToString());
                    rmi.life = float.Parse(json["robots"][i]["life"].ToString());
                    rmi.attack = float.Parse(json["robots"][i]["attack"].ToString());
                    rmi.defence = float.Parse(json["robots"][i]["defence"].ToString());
                    rmi.velocity = float.Parse(json["robots"][i]["velocity"].ToString());

                    robotMallItems.Add(rmi);
                }

                pieceMallItems = new List<PieceMallItem>();
                for (int i = 0; i < json["pieces"].Count; i++)
                {
                    PieceMallItem pmi = new PieceMallItem();
                    pmi.item_id = int.Parse(json["pieces"][i]["item_id"].ToString());
                    pmi.object_id = int.Parse(json["pieces"][i]["object_id"].ToString());
                    pmi.piece_type = (PieceType)Enum.Parse(typeof(PieceType), json["pieces"][i]["piece_type"].ToString());
                    pmi.piece_id = int.Parse(json["pieces"][i]["piece_id"].ToString());
                    pmi.seller_id = int.Parse(json["pieces"][i]["seller_id"].ToString());
                    pmi.seller_name = json["pieces"][i]["seller_name"].ToString();
                    pmi.seller_avater_id = int.Parse(json["pieces"][i]["seller_avater_id"].ToString());
                    pmi.price = float.Parse(json["pieces"][i]["price"].ToString());

                    pmi.name = json["pieces"][i]["name"].ToString();
                    pmi.level = int.Parse(json["pieces"][i]["level"].ToString());
                    pmi.imagine_time = int.Parse(json["pieces"][i]["imagine_time"].ToString());
                    pmi.life = float.Parse(json["pieces"][i]["life"].ToString());
                    pmi.attack = float.Parse(json["pieces"][i]["attack"].ToString());
                    pmi.defence = float.Parse(json["pieces"][i]["defence"].ToString());
                    pmi.velocity = float.Parse(json["pieces"][i]["velocity"].ToString());

                    pieceMallItems.Add(pmi);
                }
            }
            else if (status == "fail")
            {
                Debug.Log("GetMallItems: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetMallItems: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(mallItemJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    robotMallItems = new List<RobotMallItem>();
                    for (int i = 0; i < json["result"]["robots"].Count; i++)
                    {
                        RobotMallItem rmi = new RobotMallItem();
                        rmi.object_id = int.Parse(json["result"]["robots"][i]["object_id"].ToString());
                        rmi.seller_id = int.Parse(json["result"]["robots"][i]["seller_id"].ToString());
                        rmi.seller_name = json["result"]["robots"][i]["seller_name"].ToString();
                        rmi.seller_avater_id = int.Parse(json["result"]["robots"][i]["seller_avater_id"].ToString());
                        rmi.price = float.Parse(json["result"]["robots"][i]["price"].ToString());

                        rmi.head_id = int.Parse(json["result"]["robots"][i]["head_id"].ToString());
                        rmi.larm_id = int.Parse(json["result"]["robots"][i]["larm_id"].ToString());
                        rmi.rarm_id = int.Parse(json["result"]["robots"][i]["rarm_id"].ToString());
                        rmi.leg_id = int.Parse(json["result"]["robots"][i]["leg_id"].ToString());

                        rmi.name = json["result"]["robots"][i]["robot_name"].ToString();
                        rmi.level = int.Parse(json["result"]["robots"][i]["level"].ToString());
                        rmi.imagine_time = int.Parse(json["result"]["robots"][i]["imagine_time"].ToString());
                        rmi.life = float.Parse(json["result"]["robots"][i]["life"].ToString());
                        rmi.attack = float.Parse(json["result"]["robots"][i]["attack"].ToString());
                        rmi.defence = float.Parse(json["result"]["robots"][i]["defence"].ToString());
                        rmi.velocity = float.Parse(json["result"]["robots"][i]["velocity"].ToString());

                        robotMallItems.Add(rmi);
                    }

                    pieceMallItems = new List<PieceMallItem>();
                    for (int i = 0; i < json["result"]["pieces"].Count; i++)
                    {
                        PieceMallItem pmi = new PieceMallItem();
                        pmi.object_id = int.Parse(json["result"]["pieces"][i]["object_id"].ToString());
                        pmi.piece_type = (PieceType)Enum.Parse(typeof(PieceType), json["result"]["pieces"][i]["piece_type"].ToString());
                        pmi.piece_id = int.Parse(json["result"]["pieces"][i]["piece_id"].ToString());
                        pmi.seller_id = int.Parse(json["result"]["pieces"][i]["seller_id"].ToString());
                        pmi.seller_name = json["result"]["pieces"][i]["seller_name"].ToString();
                        pmi.seller_avater_id = int.Parse(json["result"]["pieces"][i]["seller_avater_id"].ToString());
                        pmi.price = float.Parse(json["result"]["pieces"][i]["price"].ToString());

                        pmi.level = int.Parse(json["result"]["pieces"][i]["level"].ToString());
                        pmi.imagine_time = int.Parse(json["result"]["pieces"][i]["imagine_time"].ToString());
                        pmi.life = float.Parse(json["result"]["pieces"][i]["life"].ToString());
                        pmi.attack = float.Parse(json["result"]["pieces"][i]["attack"].ToString());
                        pmi.defence = float.Parse(json["result"]["pieces"][i]["defence"].ToString());
                        pmi.velocity = float.Parse(json["result"]["pieces"][i]["velocity"].ToString());

                        pieceMallItems.Add(pmi);
                    }
                }
            }
        }
        return 0;
    }

    #region Buy
    public void BuyRobotMall(RobotMallItem r_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("BuyRobotMall");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("item_id", r_mall.item_id);


        GameManage.Get("/" + GB.g_APIBuyRobotMall, data, callbackUserInfo);
    }

    public void BuyPieceMall(PieceMallItem p_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("BuyPieceMall");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("item_id", p_mall.item_id);


        GameManage.Get("/" + GB.g_APIBuyPieceMall, data, callbackUserInfo);
    }

    public void BuyRobotPromotion(RobotMallItem r_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("BuyRobotPromotion");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("item_id", r_mall.item_id);


        GameManage.Get("/" + GB.g_APIBuyRobotPromotion, data, callbackUserInfo);
    }

    public void BuyPiecePromotion(PieceMallItem p_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("BuyPiecePromotion");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("item_id", p_mall.item_id);


        GameManage.Get("/" + GB.g_APIBuyPiecePromotion, data, callbackUserInfo);
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
                Debug.Log("BuyRobotOrPiece: Done");
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

                MainMenuPanel.instance.Refresh();
                MenuUIManager.instance.ShowPanel(4);
            }
            else if (status == "fail")
            {
                Debug.Log("BuyRobotOrPiece: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("BuyRobotOrPiece: Error");
            Debug.Log(www.error);
            MobileNative.Alert("Error", www.error, "OK");
        }

        return 0;
    }
    #endregion

    #region Sell
    public void SellRobot(Robot robot, float price, Func<WWW, int> callbackFunction = null)
    {
        RobotMallItem rmi = new RobotMallItem();
        rmi.object_id = robot.object_id;
        rmi.seller_id = GB.g_MyID;
        rmi.seller_name = GameManage.User.username;
        rmi.seller_avater_id = GameManage.User.avatar_id;
        rmi.price = price;

        rmi.head_id = robot.Head.head_id;
        rmi.larm_id = robot.LeftArm.larm_id;
        rmi.rarm_id = robot.RightArm.rarm_id;
        rmi.leg_id = robot.Leg.leg_id;

        rmi.name = robot.name;
        rmi.level = robot.Level;
        rmi.imagine_time = robot.ImagineTime();
        rmi.life = robot.Life;
        rmi.attack = robot.Attack;
        rmi.defence = robot.Defence;
        rmi.velocity = robot.Velocity;

        SellRobotInternel(rmi, callbackFunction);
    }

    public void SellHead(Head head, float price, Func<WWW, int> callbackFunction = null)
    {
        PieceMallItem pmi = new PieceMallItem();
        pmi.object_id = head.object_id;
        pmi.piece_type = PieceType.Head;
        pmi.piece_id = head.head_id;
        pmi.seller_id = GB.g_MyID;
        pmi.seller_name = GameManage.User.username;
        pmi.seller_avater_id = GameManage.User.avatar_id;
        pmi.price = price;

        pmi.level = head.level;
        pmi.imagine_time = head.ImagineTime();
        pmi.life = head.life;
        pmi.attack = head.attack;
        pmi.defence = head.defence;
        pmi.velocity = head.velocity;

        SellPieceInternel(pmi, callbackFunction);
    }

    public void SellLeftArm(LeftArm larm, float price, Func<WWW, int> callbackFunction = null)
    {
        PieceMallItem pmi = new PieceMallItem();
        pmi.object_id = larm.object_id;
        pmi.piece_type = PieceType.LeftArm;
        pmi.piece_id = larm.larm_id;
        pmi.seller_id = GB.g_MyID;
        pmi.seller_name = GameManage.User.username;
        pmi.seller_avater_id = GameManage.User.avatar_id;
        pmi.price = price;

        pmi.level = larm.level;
        pmi.imagine_time = larm.ImagineTime();
        pmi.life = larm.life;
        pmi.attack = larm.attack;
        pmi.defence = larm.defence;
        pmi.velocity = larm.velocity;

        SellPieceInternel(pmi, callbackFunction);
    }

    public void SellRightArm(RightArm rarm, float price, Func<WWW, int> callbackFunction = null)
    {
        PieceMallItem pmi = new PieceMallItem();
        pmi.object_id = rarm.object_id;
        pmi.piece_type = PieceType.RightArm;
        pmi.piece_id = rarm.rarm_id;
        pmi.seller_id = GB.g_MyID;
        pmi.seller_name = GameManage.User.username;
        pmi.seller_avater_id = GameManage.User.avatar_id;
        pmi.price = price;

        pmi.level = rarm.level;
        pmi.imagine_time = rarm.ImagineTime();
        pmi.life = rarm.life;
        pmi.attack = rarm.attack;
        pmi.defence = rarm.defence;
        pmi.velocity = rarm.velocity;

        SellPieceInternel(pmi, callbackFunction);
    }

    public void SellLeg(Leg leg, float price, Func<WWW, int> callbackFunction = null)
    {
        PieceMallItem pmi = new PieceMallItem();
        pmi.object_id = leg.object_id;
        pmi.piece_type = PieceType.Leg;
        pmi.piece_id = leg.leg_id;
        pmi.seller_id = GB.g_MyID;
        pmi.seller_name = GameManage.User.username;
        pmi.seller_avater_id = GameManage.User.avatar_id;
        pmi.price = price;

        pmi.level = leg.level;
        pmi.imagine_time = leg.ImagineTime();
        pmi.life = leg.life;
        pmi.attack = leg.attack;
        pmi.defence = leg.defence;
        pmi.velocity = leg.velocity;

        SellPieceInternel(pmi, callbackFunction);
    }

    private void SellRobotInternel(RobotMallItem r_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("SellRobot");
        Hashtable data = new Hashtable();
        data.Add("object_id", r_mall.object_id);
        data.Add("seller_id", r_mall.seller_id);
        data.Add("seller_name", r_mall.seller_name);
        data.Add("price", r_mall.price);


        GameManage.Get("/" + GB.g_APISellRobotMall, data, callbackFunction);
        if (MB_Configs.bDummyData)
        {
            robotMallItems.Add(r_mall);
            UpdateLocalFile();
        }
    }

    private void SellPieceInternel(PieceMallItem p_mall, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("SellPiece");
        Hashtable data = new Hashtable();
        data.Add("object_id", p_mall.object_id);
        data.Add("piece_type", (int)p_mall.piece_type);
        data.Add("piece_id", p_mall.piece_id);
        data.Add("seller_id", p_mall.seller_id);
        data.Add("seller_name", p_mall.seller_name);
        data.Add("price", p_mall.price);


        GameManage.Get("/" + GB.g_APISellPieceMall, data, callbackFunction);
        if (MB_Configs.bDummyData)
        {
            pieceMallItems.Add(p_mall);
            UpdateLocalFile();
        }
    }
    #endregion
}
