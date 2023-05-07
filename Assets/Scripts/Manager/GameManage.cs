using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class PromotionRobotItem
{
    public int item_id;
    public int object_id;

    public int head_object_id;
    public int larm_object_id;
    public int rarm_object_id;
    public int leg_object_id;

    public string robot_name;
    public int level;
    public int is_new;
    public int is_discounted;
    public float old_price;
    public float new_price;
}

[Serializable]
public class PromotionPieceItem
{
    public int item_id;
    public int object_id;
    public PieceType piece_type;
    public int piece_id;

    public int level;
    public int is_new;
    public int is_discounted;
    public float old_price;
    public float new_price;
}

[Serializable]
public class RobotBluePrint
{
    public int object_id;
    public string type;
    public string name;
    public float price;
    public int head_object_id;
    public int larm_object_id;
    public int rarm_object_id;
    public int leg_object_id;
    public int onSale;
    public float salePrice;


    public Head head;
    public LeftArm larm;
    public RightArm rarm;
    public Leg leg;

    public string toJson()
    {
        string strJSON = JsonUtility.ToJson(this, true);
        Debug.Log(strJSON);
        //strJSON = strJSON;
        return strJSON;
    }
}
[Serializable]
public class Robot
{
    public int object_id;
    public string type;
    public string name;
    public float price;
    public int head_object_id;
    public int larm_object_id;
    public int rarm_object_id;
    public int leg_object_id;
    public int onSale;
    public float salePrice;


    private Head head;
    public Head Head {
        get {
            if (head == null)
            {
                head = GameManage.User.Heads.Find(x => (x.object_id == head_object_id));
            }

            return head;
        }
        set { head = value; }
    }

    private LeftArm larm;
    public LeftArm LeftArm {
        get {
            if (larm == null)
            {
                larm = GameManage.User.LeftArms.Find(x => (x.object_id == larm_object_id));
            }

            return larm;
        }
        set { larm = value; }
    }

    private RightArm rarm;
    public RightArm RightArm {
        get {
            if (rarm == null)
            {
                rarm = GameManage.User.RightArms.Find(x => (x.object_id == rarm_object_id));
            }

            return rarm;
        }
        set { rarm = value; }
    }

    private Leg leg;
    public Leg Leg {
        get {
            if (leg == null)
            {
                leg = GameManage.User.Legs.Find(x => (x.object_id == leg_object_id));
            }

            return leg;
        }
        set { leg = value; }
    }

    public string Name {
        get {
            Head h = Head;
            return h.name;
        }
    }

    public int Level {
        get {
            if (Head != null && LeftArm != null && RightArm != null && Leg != null)
            {
                int total = Head.level + LeftArm.level + RightArm.level + Leg.level;
                int average = total / 4;
                return average;
            }
            else
            {
                return 0;
            }
        }
    }

    public float ImagineTime()
    {
        float[] partsTime = new float[4];
        float maxImagineTime = 0;

        float headtTime = (Head == null)? 0 : GameManage.CalculateImagineTime(Level, head.medicament);
        float leftArmTime = (LeftArm == null)? 0 : GameManage.CalculateImagineTime(Level, LeftArm.medicament);
        float rightArmTime = (RightArm == null)? 0 : GameManage.CalculateImagineTime(Level, RightArm.medicament);
        float legTime = (Leg == null)? 0 : GameManage.CalculateImagineTime(Level, Leg.medicament);

        partsTime[0] = headtTime;
        partsTime[1] = leftArmTime;
        partsTime[2] = rightArmTime;
        partsTime[3] = legTime;

        for (int i = 0; i < partsTime.Length; i++)
        {
            if (partsTime[i] > maxImagineTime)
            {
                maxImagineTime = partsTime[i];
            }
        }

        return maxImagineTime;
    }

    public float Life {
        get {
            if (Head != null && LeftArm != null && RightArm != null && Leg != null)
            {
                float total = Head.life + LeftArm.life + RightArm.life + Leg.life;
                float average = total;
                //average = 
                return average;
            }
            else
            {
                return 0;
            }

        }
    }

    public float Attack {
        get {
            if (Head != null && LeftArm != null && RightArm != null && Leg != null)
            {
                float total = Head.attack + LeftArm.attack + RightArm.attack + Leg.attack;
                float average = total;
                return average;
            }
            else
            {
                return 0;
            }
        }
    }

    public float Defence {
        get {
            if (Head != null && LeftArm != null && RightArm != null && Leg != null)
            {
                float total = Head.defence + LeftArm.defence + RightArm.defence + Leg.defence;
                float average = total;
                return average;
            }
            else
            {
                return 0;
            }
        }
    }

    public float Velocity {
        get {
            if (Leg != null)
            {
                float velocity = Leg.velocity;
                return velocity;
            }
            else
            {
                return 0;
            }
        }
    }

    public float MaxLife()
    {
        if (Head != null && LeftArm != null && RightArm != null && Leg != null)
        {
            Head head = GameManage.AllHeads.Find(x => x.head_id == Head.head_id);
            LeftArm leftArm = GameManage.AllLeftArms.Find(x => x.larm_id == LeftArm.larm_id);
            RightArm rightArm = GameManage.AllRightArms.Find(x => x.rarm_id == RightArm.rarm_id);
            Leg leg = GameManage.AllLegs.Find(x => x.leg_id == Leg.leg_id);

            float total = head.life + leftArm.life + rightArm.life + leg.life;
            return total;
        }
        else
        {
            return 0;
        }
    }

    public float MaxAttack()
    {

        if (Head != null && LeftArm != null && RightArm != null && Leg != null)
        {
            Head head = GameManage.AllHeads.Find(x => x.head_id == Head.head_id);
            LeftArm leftArm = GameManage.AllLeftArms.Find(x => x.larm_id == LeftArm.larm_id);
            RightArm rightArm = GameManage.AllRightArms.Find(x => x.rarm_id == RightArm.rarm_id);
            Leg leg = GameManage.AllLegs.Find(x => x.leg_id == Leg.leg_id);

            float total = head.attack + leftArm.attack + rightArm.attack + leg.attack;
            return total;
        }
        else
        {
            return 0;
        }
    }

    public float MaxDefence()
    {
        if (Head != null && LeftArm != null && RightArm != null && Leg != null)
        {
            Head head = GameManage.AllHeads.Find(x => x.head_id == Head.head_id);
            LeftArm leftArm = GameManage.AllLeftArms.Find(x => x.larm_id == LeftArm.larm_id);
            RightArm rightArm = GameManage.AllRightArms.Find(x => x.rarm_id == RightArm.rarm_id);
            Leg leg = GameManage.AllLegs.Find(x => x.leg_id == Leg.leg_id);

            float total = head.defence + leftArm.defence + rightArm.defence + leg.defence;
            return total;
        }
        else
        {
            return 0;
        }

    }

    public float MaxVelocity()
    {
        if (Leg != null)
        {
            Leg leg = GameManage.AllLegs.Find(x => x.leg_id == Leg.leg_id);

            float total = leg.velocity;
            return total;
        }
        else
        {
            return 0;
        }

    }

    public string toJson()
    {
        string strJSON = JsonUtility.ToJson(this, true);
        Debug.Log(strJSON);
        //strJSON = strJSON;
        return strJSON;
    }
}

[Serializable]
public class Head
{
    public int object_id;
    public int head_id;
    public string type;
    public string name;
    public int level;
    public float price;
    public int medicament;
    public float life;
    public float attack;
    public float defence;
    public float velocity;
    public int onSale;
    public float salePrice;
    public int is_broken;
    public Int64 break_timestamp;

    public string Name {
        get { return name; }
    }

    public float ActualImagineTime()
    {
        float time = GameManage.ActualImagineTime(level);
        return time;
    }

    public float ImagineTime()
    {
        float time = GameManage.CalculateImagineTime(level, medicament);
        return time;
    }
}

[Serializable]
public class LeftArm
{
    public int object_id;
    public int larm_id;
    public string type;
    public string name;
    public int level;
    public float price;
    public int medicament;
    public float life;
    public float attack;
    public float defence;
    public float velocity;
    public int onSale;
    public float salePrice;
    public int is_broken;
    public Int64 break_timestamp;

    public float ActualImagineTime()
    {
        float time = GameManage.ActualImagineTime(level);
        return time;
    }

    public float ImagineTime()
    {
        float time = GameManage.CalculateImagineTime(level, medicament);
        return time;
    }
}

[Serializable]
public class RightArm
{
    public int object_id;
    public int rarm_id;
    public string type;
    public string name;
    public int level;
    public float price;
    public int medicament;
    public float life;
    public float attack;
    public float defence;
    public float velocity;
    public int onSale;
    public float salePrice;
    public int is_broken;
    public Int64 break_timestamp;

    public float ActualImagineTime()
    {
        float time = GameManage.ActualImagineTime(level);
        return time;
    }

    public float ImagineTime()
    {
        float time = GameManage.CalculateImagineTime(level, medicament);
        return time;
    }
}

[Serializable]
public class Leg
{
    public int object_id;
    public int leg_id;
    public string type;
    public string name;
    public int level;
    public float price;
    public int medicament;
    public float life;
    public float attack;
    public float defence;
    public float velocity;
    public int onSale;
    public float salePrice;
    public int is_broken;
    public Int64 break_timestamp;

    public float ActualImagineTime()
    {
        float time = GameManage.ActualImagineTime(level);
        return time;
    }

    public float ImagineTime()
    {
        float time = GameManage.CalculateImagineTime(level, medicament);
        return time;
    }
}

[Serializable]
public class MB_User
{
    public string username;
    public int gems;
    public int rank;
    public int win;
    public int loss;
    public int avatar_id;
    public List<Robot> robots = new List<Robot>();
    public List<Head> heads = new List<Head>();
    public List<LeftArm> larms = new List<LeftArm>();
    public List<RightArm> rarms = new List<RightArm>();
    public List<Leg> legs = new List<Leg>();

    public List<Robot> Robots {
        get { return robots; }
    }

    public List<Head> Heads {
        get { return heads; }
    }

    public List<LeftArm> LeftArms {
        get { return larms; }
    }

    public List<RightArm> RightArms {
        get { return rarms; }
    }

    public List<Leg> Legs {
        get { return legs; }
    }

    public Robot GetRobot(int object_id)
    {
        Robot robot = Robots.Find(x => x.object_id == object_id);
        return robot;
    }
    public void AddRobot(Robot robot)
    {
        Robots.Add(robot);
    }

    public void ChangeRobot(int object_id, Robot robot)
    {
        Robot r = Robots.Find(x => x.object_id == object_id);
        r = robot;
    }

    public void RemoveRobot(int object_id)
    {
        Robot r = Robots.Find(x => x.object_id == object_id);
        Robots.Remove(r);
    }

    public Head GetHead(int object_id)
    {
        Head head = Heads.Find(x => x.object_id == object_id);
        return head;
    }
    public void AddHead(Head head)
    {
        Heads.Add(head);
    }

    public void ChangeHead(int object_id, Head head)
    {
        Head h = Heads.Find(x => x.object_id == object_id);
        h = head;
    }

    public void RemoveHead(int object_id)
    {
        Head h = Heads.Find(x => x.object_id == object_id);
        Heads.Remove(h);
    }

    public LeftArm GetLeftArm(int object_id)
    {
        LeftArm leftArm = LeftArms.Find(x => x.object_id == object_id);
        return leftArm;
    }
    public void AddLeftArm(LeftArm leftArm)
    {
        LeftArms.Add(leftArm);
    }

    public void ChangeLeftArm(int object_id, LeftArm leftArm)
    {
        LeftArm la = LeftArms.Find(x => x.object_id == object_id);
        la = leftArm;
    }

    public void RemoveLeftArm(int object_id)
    {
        LeftArm la = LeftArms.Find(x => x.object_id == object_id);
        LeftArms.Remove(la);
    }

    public RightArm GetRightArm(int object_id)
    {
        RightArm rightArm = RightArms.Find(x => x.object_id == object_id);
        return rightArm;
    }
    public void AddRightArm(RightArm rightArm)
    {
        RightArms.Add(rightArm);
    }

    public void ChangeRightArm(int object_id, RightArm rightArm)
    {
        RightArm ra = RightArms.Find(x => x.object_id == object_id);
        ra = rightArm;
    }

    public void RemoveRightArm(int object_id)
    {
        RightArm ra = RightArms.Find(x => x.object_id == object_id);
        RightArms.Remove(ra);
    }

    public Leg GetLeg(int object_id)
    {
        Leg leg = Legs.Find(x => x.object_id == object_id);
        return leg;
    }
    public void AddLeg(Leg leg)
    {
        Legs.Add(leg);
    }

    public void ChangeLeg(int object_id, Leg leg)
    {
        Leg l = Legs.Find(x => x.object_id == object_id);
        l = leg;
    }

    public void RemoveLeg(int object_id)
    {
        Leg l = Legs.Find(x => x.object_id == object_id);
        Legs.Remove(l);
    }

    //public 

    public string toJson()
    {
        string strJSON = JsonUtility.ToJson(this, true);
        //Debug.Log(strJSON);
        strJSON = @"{""status"":""success"", ""result"":" + strJSON + "}";
        return strJSON;
    }

    public void UpdateLocalFile()
    {
        string filepath = GameManage.DataPath("TextAssets/user_info.json");
        string result = string.Empty;
        using (StreamReader r = new StreamReader(filepath))
        {
            result = toJson();
            //Debug.Log(result);
        }
        File.WriteAllText(filepath, result);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "03-Main")
        {
            MenuUIManager.instance.PanelList[0].GetComponent<MainMenuPanel>().Refresh();
        }

    }
}

[Serializable]
public class HeadConfig
{
    public int head_id;
    public int level;
}

[Serializable]
public class LeftArmConfig
{
    public int larm_id;
    public int level;
}

[Serializable]
public class RightArmConfig
{
    public int rarm_id;
    public int level;
}

[Serializable]
public class LegConfig
{
    public int leg_id;
    public int level;
}

[Serializable]
public class UserLevel
{
    public int battle_id;
    public int level_id;
    public int env_id;
    public int passed;
    public int locked;
    public int win_count;
    public List<int> opponent_robots;
    public LevelReward reward1;
    public LevelReward reward2;

    private List<Robot> opponentRobots = new List<Robot>();
    public List<Robot> OpponentRobots {
        get {
            if (opponentRobots.Count == 0)
            {
                for (int i = 0; i < opponent_robots.Count; i++)
                {
                    opponentRobots.Add(GameManage.BasicRobots.Find(x => x.object_id == opponent_robots[i]));
                }
            }
            return opponentRobots;
        }
    }

    private List<Robot> reward1Robots = new List<Robot>();
    public List<Robot> Reward1Robots {
        get {
            if (reward1Robots.Count == 0)
            {
                for (int i = 0; i < reward1.robot_ids.Count; i++)
                {
                    reward1Robots.Add(GameManage.BasicRobots.Find(x => x.object_id == reward1.robot_ids[i]));
                }
            }
            return reward1Robots;
        }
    }

    private List<Robot> reward2Robots = new List<Robot>();
    public List<Robot> Reward2Robots {
        get {
            if (reward2Robots.Count == 0)
            {
                for (int i = 0; i < reward2.robot_ids.Count; i++)
                {
                    reward2Robots.Add(GameManage.BasicRobots.Find(x => x.object_id == reward2.robot_ids[i]));
                }
            }
            return reward2Robots;
        }
    }
}

[Serializable]
public class LevelReward
{
    public RewardType reward_type;
    public int reward_count;
    public List<int> robot_ids;
    public int medicament;
    public int gems;
}

public enum RewardType
{
    Robot = 0,
    Piece = 1
}


public class UserLevelData
{
    public List<UserLevel> levels;
}

public enum GameMode
{
    Adventure = 0,
    Multiplayer = 1
}

public class GameManage : MonoBehaviour
{
    public GameMode gameMode;

    public TextAsset basicRobotsJson, allHeadsJson, allLeftArmsJson, allRightArmsJson, allLegsJson, userInfoJson, allLevelsJson, allPromotionsJson;

    private static MB_User user = new MB_User();
    public static MB_User User {
        get { return user; }
        set { user = value; }
    }

    private static List<PromotionRobotItem> promotionRobots = new List<PromotionRobotItem>();
    public static List<PromotionRobotItem> PromotionRobots {
        get { return promotionRobots; }
        set { promotionRobots = value; }
    }

    private static List<PromotionPieceItem> promotionPieces = new List<PromotionPieceItem>();
    public static List<PromotionPieceItem> PromotionPieces {
        get { return promotionPieces; }
        set { promotionPieces = value; }
    }


    private static List<Robot> basicRobots = new List<Robot>();
    public static List<Robot> BasicRobots {
        get { return basicRobots; }
        set { basicRobots = value; }
    }

    private static List<Head> allHeads = new List<Head>();
    public static List<Head> AllHeads {
        get { return allHeads; }
        set { allHeads = value; }
    }

    private static List<LeftArm> allLeftArms = new List<LeftArm>();
    public static List<LeftArm> AllLeftArms {
        get { return allLeftArms; }
        set { allLeftArms = value; }
    }

    private static List<RightArm> allRightArms = new List<RightArm>();
    public static List<RightArm> AllRightArms {
        get { return allRightArms; }
        set { allRightArms = value; }
    }

    private static List<Leg> allLegs = new List<Leg>();
    public static List<Leg> AllLegs {
        get { return allLegs; }
        set { allLegs = value; }
    }

    private static List<Robot> myRobots = new List<Robot>();
    public static List<Robot> MyRobots {
        get { return myRobots; }
        set { myRobots = value; }
    }

    private static List<Head> myHeads = new List<Head>();
    public static List<Head> MyHeads {
        get { return myHeads; }
        set { myHeads = value; }
    }

    private static List<LeftArm> myLeftArms = new List<LeftArm>();
    public static List<LeftArm> MyLeftArms {
        get { return myLeftArms; }
        set { myLeftArms = value; }
    }

    private static List<RightArm> myRightArms = new List<RightArm>();
    public static List<RightArm> MyRightArms {
        get { return myRightArms; }
        set { myRightArms = value; }
    }

    private static List<Leg> myLegs = new List<Leg>();
    public static List<Leg> MyLegs {
        get { return myLegs; }
        set { myLegs = value; }
    }

    private static List<UserLevel> userLevels = new List<UserLevel>();
    public static List<UserLevel> UserLevels {
        get { return userLevels; }
        set { userLevels = value; }
    }

    public List<Robot> myTeam = new List<Robot>();
    public List<Robot> opponentTeam = new List<Robot>();

    //public List<GameObject> PanelList = new List<GameObject>();
    //public List<Sprite> AvatarList = new List<Sprite>();
    //public List<Sprite> ComponentImageList = new List<Sprite>();

    public List<Sprite> AllBasicRobotImages = new List<Sprite>();
    public List<Sprite> AllHeadPieceImages = new List<Sprite>();
    public List<Sprite> AllLeftArmPieceImages = new List<Sprite>();
    public List<Sprite> AllRightArmPieceImages = new List<Sprite>();
    public List<Sprite> AllLegPieceImages = new List<Sprite>();
    //public GameObject QuitPanel;



    /// Shared instance to access values
    public static GameManage instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        //		DontDestroyOnLoad(gameObject);
    }

    #region level update
    public string toJson()
    {
        UserLevelData data = new UserLevelData();
        data.levels = UserLevels;

        //string strJSON = JsonUtility.ToJson(UserLevels, true);
        //string strPiecesJSON = JsonUtility.ToJson(pieceMallItems, true);
        string strJSON = JsonUtility.ToJson(data, true);
        //string strJSON = @"{""status"":""success"", ""result"":{""robots"":" + strRobotsJSON + "," + @"""pieces"":" + strPiecesJSON + "}}";
        strJSON = @"{""status"":""success"", ""result"":" + strJSON + "}";
        //Debug.LogError(strJSON);
        return strJSON;
    }

    public void UpdateLevelLocalFile()
    {
        string filepath = GameManage.DataPath("TextAssets/all_levels.json");
        string result = string.Empty;
        using (StreamReader r = new StreamReader(filepath))
        {
            result = toJson();
            //Debug.Log(result);
        }
        File.WriteAllText(filepath, result);
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        UpdateConfigs();
    }

    private void UpdateConfigs()
    {
        GetUserInfo(GB.g_MyID, callbackUserInfoFirstTime);
        GetBasicRobots(GB.g_MyID, callbackBasicRobots);
        GetAllHeads(GB.g_MyID, callbackAllHeads);
        GetAllLeftArms(GB.g_MyID, callbackAllLeftArms);
        GetAllRightArms(GB.g_MyID, callbackAllRightArms);
        GetAllLegs(GB.g_MyID, callbackAllLegs);
        GetLevels(GB.g_MyID, callbackLevels);
    }

    public static void Get(string relativeUrl, Hashtable data, Func<WWW, int> callbackFunction = null)
    {
        LoadingPanel.instance.totalTask++;
        LoadingPanel.instance.Show();
        NetworkConnector.instance.HttpPostDirect(MB_Configs.instance.Host + relativeUrl, data, callbackFunction);
    }

    public static void Post(string relativeUrl, Hashtable data, Func<WWW, int> callbackFunction = null)
    {
        LoadingPanel.instance.totalTask++;
        LoadingPanel.instance.Show();
        NetworkConnector.instance.HttpPostDirect(MB_Configs.instance.Host + relativeUrl, data, callbackFunction);
    }

    private void GetPromotionItems(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetPromotionItems");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetPromotionItems, data, callbackFunction);
    }

    private int callbackPromotionItems(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        //Debug.Log("GetPromotionItems");
        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.LogError(www.text);
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetPromotionItems: Done");
                Debug.Log(www.text);
                PromotionRobots = new List<PromotionRobotItem>();
                for (int i = 0; i < json["result"]["robots"].Count; i++)
                {
                    PromotionRobotItem pr = new PromotionRobotItem();
                    pr.item_id = int.Parse(json["result"]["robots"][i]["item_id"].ToString());
                    pr.object_id = int.Parse(json["result"]["robots"][i]["object_id"].ToString());
                    pr.head_object_id = int.Parse(json["result"]["robots"][i]["head_object_id"].ToString());
                    pr.larm_object_id = int.Parse(json["result"]["robots"][i]["larm_object_id"].ToString());
                    pr.rarm_object_id = int.Parse(json["result"]["robots"][i]["rarm_object_id"].ToString());
                    pr.leg_object_id = int.Parse(json["result"]["robots"][i]["leg_object_id"].ToString());
                    pr.robot_name = json["result"]["robots"][i]["robot_name"].ToString();
                    pr.level = int.Parse(json["result"]["robots"][i]["level"].ToString());
                    pr.is_new = int.Parse(json["result"]["robots"][i]["is_new"].ToString());
                    pr.is_discounted = int.Parse(json["result"]["robots"][i]["is_discounted"].ToString());
                    pr.old_price = int.Parse(json["result"]["robots"][i]["old_price"].ToString());
                    pr.new_price = int.Parse(json["result"]["robots"][i]["new_price"].ToString());

                    PromotionRobots.Add(pr);
                }

                PromotionPieces = new List<PromotionPieceItem>();
                for (int i = 0; i < json["result"]["pieces"].Count; i++)
                {
                    PromotionPieceItem pp = new PromotionPieceItem();
                    pp.item_id = int.Parse(json["result"]["pieces"][i]["item_id"].ToString());
                    pp.object_id = int.Parse(json["result"]["pieces"][i]["object_id"].ToString());
                    pp.piece_type = (PieceType)int.Parse(json["result"]["pieces"][i]["piece_type"].ToString());
                    pp.piece_id = int.Parse(json["result"]["pieces"][i]["piece_id"].ToString());
                    pp.level = int.Parse(json["result"]["pieces"][i]["level"].ToString());
                    pp.is_new = int.Parse(json["result"]["pieces"][i]["is_new"].ToString());
                    pp.is_discounted = int.Parse(json["result"]["pieces"][i]["is_discounted"].ToString());
                    pp.old_price = int.Parse(json["result"]["pieces"][i]["old_price"].ToString());
                    pp.new_price = int.Parse(json["result"]["pieces"][i]["new_price"].ToString());

                    PromotionPieces.Add(pp);
                }
                StoreManager.instance.CheckAndOpenDailyPanel();
                //Debug.LogError("BasicRobots Count: " + BasicRobots.Count);

            }
            else if (status == "fail")
            {
                Debug.LogError("GetPromotionItems: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetPromotionItems: Error"); 
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allPromotionsJson.text);
                //Debug.Log(allPromotionsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;

                    PromotionRobots = new List<PromotionRobotItem>();
                    for (int i = 0; i < json["result"]["robots"].Count; i++)
                    {
                        PromotionRobotItem pr = new PromotionRobotItem();
                        pr.object_id = int.Parse(json["result"]["robots"][i]["object_id"].ToString());
                        pr.head_object_id = int.Parse(json["result"]["robots"][i]["head_object_id"].ToString());
                        pr.larm_object_id = int.Parse(json["result"]["robots"][i]["larm_object_id"].ToString());
                        pr.rarm_object_id = int.Parse(json["result"]["robots"][i]["rarm_object_id"].ToString());
                        pr.leg_object_id = int.Parse(json["result"]["robots"][i]["leg_object_id"].ToString());
                        pr.robot_name = json["result"]["robots"][i]["robot_name"].ToString();
                        pr.level = int.Parse(json["result"]["robots"][i]["level"].ToString());
                        pr.is_new = int.Parse(json["result"]["robots"][i]["is_new"].ToString());
                        pr.is_discounted = int.Parse(json["result"]["robots"][i]["is_discounted"].ToString());
                        pr.old_price = int.Parse(json["result"]["robots"][i]["old_price"].ToString());
                        pr.new_price = int.Parse(json["result"]["robots"][i]["new_price"].ToString());

                        PromotionRobots.Add(pr);
                    }

                    PromotionPieces = new List<PromotionPieceItem>();
                    for (int i = 0; i < json["result"]["pieces"].Count; i++)
                    {
                        PromotionPieceItem pp = new PromotionPieceItem();
                        pp.object_id = int.Parse(json["result"]["pieces"][i]["object_id"].ToString());
                        pp.piece_type = (PieceType)int.Parse(json["result"]["pieces"][i]["piece_type"].ToString());
                        pp.piece_id = int.Parse(json["result"]["pieces"][i]["piece_id"].ToString());
                        pp.level = int.Parse(json["result"]["pieces"][i]["level"].ToString());
                        pp.is_new = int.Parse(json["result"]["pieces"][i]["is_new"].ToString());
                        pp.is_discounted = int.Parse(json["result"]["pieces"][i]["is_discounted"].ToString());
                        pp.old_price = int.Parse(json["result"]["pieces"][i]["old_price"].ToString());
                        pp.new_price = int.Parse(json["result"]["pieces"][i]["new_price"].ToString());

                        PromotionPieces.Add(pp);
                    }
                    StoreManager.instance.CheckAndOpenDailyPanel();

                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    private void GetBasicRobots(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetBasicRobots");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetBasicRobots, data, callbackFunction);
    }

    private int callbackBasicRobots(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.LogError(www.text);
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetBasicRobots: Done");
                Debug.Log(www.text);
                BasicRobots = new List<Robot>();
                for (int i = 0; i < json["result"].Count; i++)
                {
                    Robot r = new Robot();
                    r.object_id = int.Parse(json["result"][i]["id"].ToString());
                    r.type = json["result"][i]["type"].ToString();
                    r.name = json["result"][i]["name"].ToString();
                    r.price = float.Parse(json["result"][i]["price"].ToString());
                    r.head_object_id = int.Parse(json["result"][i]["head_id"].ToString());
                    r.larm_object_id = int.Parse(json["result"][i]["larm_id"].ToString());
                    r.rarm_object_id = int.Parse(json["result"][i]["rarm_id"].ToString());
                    r.leg_object_id = int.Parse(json["result"][i]["leg_id"].ToString());

                    BasicRobots.Add(r);
                }
            }
            else if (status == "fail")
            {
                Debug.Log("GetAllHeads: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetAllHeads: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(basicRobotsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    BasicRobots = new List<Robot>();
                    for (int i = 0; i < json["result"].Count; i++)
                    {
                        Robot r = new Robot();
                        r.object_id = int.Parse(json["result"][i]["id"].ToString());
                        r.type = json["result"][i]["type"].ToString();
                        r.name = json["result"][i]["name"].ToString();
                        r.price = float.Parse(json["result"][i]["price"].ToString());
                        r.head_object_id = int.Parse(json["result"][i]["head_id"].ToString());
                        r.larm_object_id = int.Parse(json["result"][i]["larm_id"].ToString());
                        r.rarm_object_id = int.Parse(json["result"][i]["rarm_id"].ToString());
                        r.leg_object_id = int.Parse(json["result"][i]["leg_id"].ToString());

                        BasicRobots.Add(r);
                    }

                    //Debug.LogError("BasicRobots Count: " + BasicRobots.Count);

                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    private void GetAllHeads(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetAllHeads");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetAllHeads, data, callbackFunction);
    }

    private int callbackAllHeads(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetAllHeads: Done");
                Debug.Log(www.text);
                AllHeads = new List<Head>();
                for (int i = 0; i < json["result"].Count; i++)
                {
                    Head h = new Head();
                    h.object_id = int.Parse(json["result"][i]["id"].ToString());
                    h.head_id = int.Parse(json["result"][i]["head_id"].ToString());
                    h.type = json["result"][i]["type"].ToString();
                    h.name = json["result"][i]["name"].ToString();
                    h.level = int.Parse(json["result"][i]["level"].ToString());
                    h.price = float.Parse(json["result"][i]["price"].ToString());
                    //h.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                    h.life = int.Parse(json["result"][i]["life"].ToString());
                    h.attack = int.Parse(json["result"][i]["attack"].ToString());
                    h.defence = int.Parse(json["result"][i]["defence"].ToString());
                    h.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                    AllHeads.Add(h);
                }

                //Debug.LogError("AllHeads Count: " + AllHeads.Count);

            }
            else if (status == "fail")
            {
                Debug.Log("GetAllHeads: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetAllHeads: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allHeadsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    AllHeads = new List<Head>();
                    for (int i = 0; i < json["result"].Count; i++)
                    {
                        Head h = new Head();
                        h.object_id = int.Parse(json["result"][i]["id"].ToString());
                        h.head_id = int.Parse(json["result"][i]["head_id"].ToString());
                        h.type = json["result"][i]["type"].ToString();
                        h.name = json["result"][i]["name"].ToString();
                        h.level = int.Parse(json["result"][i]["level"].ToString());
                        h.price = float.Parse(json["result"][i]["price"].ToString());
                        h.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                        h.life = int.Parse(json["result"][i]["life"].ToString());
                        h.attack = int.Parse(json["result"][i]["attack"].ToString());
                        h.defence = int.Parse(json["result"][i]["defence"].ToString());
                        h.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                        AllHeads.Add(h);
                    }

                    //Debug.LogError("AllHeads Count: " + AllHeads.Count);

                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    private void GetAllLeftArms(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetAllLeftArms");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetAllLeftArms, data, callbackFunction);
    }

    private int callbackAllLeftArms(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetAllLeftArms: Done");
                Debug.Log(www.text);
                AllLeftArms = new List<LeftArm>();
                for (int i = 0; i < json["result"].Count; i++)
                {
                    LeftArm la = new LeftArm();
                    la.object_id = int.Parse(json["result"][i]["id"].ToString());
                    la.larm_id = int.Parse(json["result"][i]["larm_id"].ToString());
                    la.type = json["result"][i]["type"].ToString();
                    la.name = json["result"][i]["name"].ToString();
                    la.level = int.Parse(json["result"][i]["level"].ToString());
                    la.price = float.Parse(json["result"][i]["price"].ToString());
                    //la.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                    la.life = int.Parse(json["result"][i]["life"].ToString());
                    la.attack = int.Parse(json["result"][i]["attack"].ToString());
                    la.defence = int.Parse(json["result"][i]["defence"].ToString());
                    la.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                    AllLeftArms.Add(la);
                }

                //Debug.LogError("AllLeftArms Count: " + AllLeftArms.Count);

            }
            else if (status == "fail")
            {
                Debug.Log("GetAllLeftArms: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetAllLeftArms: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allLeftArmsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    AllLeftArms = new List<LeftArm>();
                    for (int i = 0; i < json["result"].Count; i++)
                    {
                        LeftArm la = new LeftArm();
                        la.object_id = int.Parse(json["result"][i]["id"].ToString());
                        la.larm_id = int.Parse(json["result"][i]["larm_id"].ToString());
                        la.type = json["result"][i]["type"].ToString();
                        la.name = json["result"][i]["name"].ToString();
                        la.level = int.Parse(json["result"][i]["level"].ToString());
                        la.price = float.Parse(json["result"][i]["price"].ToString());
                        la.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                        la.life = int.Parse(json["result"][i]["life"].ToString());
                        la.attack = int.Parse(json["result"][i]["attack"].ToString());
                        la.defence = int.Parse(json["result"][i]["defence"].ToString());
                        la.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                        AllLeftArms.Add(la);
                    }

                    //Debug.LogError("AllLeftArms Count: " + AllLeftArms.Count);

                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    private void GetAllRightArms(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetAllRightArms");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetAllRightArms, data, callbackFunction);
    }

    private int callbackAllRightArms(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetAllRightArms: Done");
                Debug.Log(www.text);
                AllRightArms = new List<RightArm>();
                for (int i = 0; i < json["result"].Count; i++)
                {
                    RightArm ra = new RightArm();
                    ra.object_id = int.Parse(json["result"][i]["id"].ToString());
                    ra.rarm_id = int.Parse(json["result"][i]["rarm_id"].ToString());
                    ra.type = json["result"][i]["type"].ToString();
                    ra.name = json["result"][i]["name"].ToString();
                    ra.level = int.Parse(json["result"][i]["level"].ToString());
                    ra.price = float.Parse(json["result"][i]["price"].ToString());
                    //ra.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                    ra.life = int.Parse(json["result"][i]["life"].ToString());
                    ra.attack = int.Parse(json["result"][i]["attack"].ToString());
                    ra.defence = int.Parse(json["result"][i]["defence"].ToString());
                    ra.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                    AllRightArms.Add(ra);
                }

                //Debug.LogError("AllRightArms Count: " + AllRightArms.Count);

            }
            else if (status == "fail")
            {
                Debug.Log("GetAllRightArms: Failed");
                Debug.Log(www.text);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetAllRightArms: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allRightArmsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    AllRightArms = new List<RightArm>();
                    for (int i = 0; i < json["result"].Count; i++)
                    {
                        RightArm ra = new RightArm();
                        ra.object_id = int.Parse(json["result"][i]["id"].ToString());
                        ra.rarm_id = int.Parse(json["result"][i]["rarm_id"].ToString());
                        ra.type = json["result"][i]["type"].ToString();
                        ra.name = json["result"][i]["name"].ToString();
                        ra.level = int.Parse(json["result"][i]["level"].ToString());
                        ra.price = float.Parse(json["result"][i]["price"].ToString());
                        ra.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                        ra.life = int.Parse(json["result"][i]["life"].ToString());
                        ra.attack = int.Parse(json["result"][i]["attack"].ToString());
                        ra.defence = int.Parse(json["result"][i]["defence"].ToString());
                        ra.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                        AllRightArms.Add(ra);
                    }

                    //Debug.LogError("AllRightArms Count: " + AllRightArms.Count);

                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    private void GetAllLegs(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetAllLegs");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetAllLegs, data, callbackFunction);
    }

    private int callbackAllLegs(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetAllLegs: Done");
                Debug.Log(www.text);
                AllLegs = new List<Leg>();
                for (int i = 0; i < json["result"].Count; i++)
                {
                    Leg l = new Leg();
                    l.object_id = int.Parse(json["result"][i]["id"].ToString());
                    l.leg_id = int.Parse(json["result"][i]["leg_id"].ToString());
                    l.type = json["result"][i]["type"].ToString();
                    l.name = json["result"][i]["name"].ToString();
                    l.level = int.Parse(json["result"][i]["level"].ToString());
                    l.price = float.Parse(json["result"][i]["price"].ToString());
                    //l.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                    l.life = int.Parse(json["result"][i]["life"].ToString());
                    l.attack = int.Parse(json["result"][i]["attack"].ToString());
                    l.defence = int.Parse(json["result"][i]["defence"].ToString());
                    l.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                    AllLegs.Add(l);
                }

                //Debug.LogError("AllLegs Count: " + AllLegs.Count);

            }
            else if (status == "fail")
            {
                Debug.Log("GetAllLegs: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetAllLegs: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allLegsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    AllLegs = new List<Leg>();
                    for (int i = 0; i < json["result"].Count; i++)
                    {
                        Leg l = new Leg();
                        l.object_id = int.Parse(json["result"][i]["id"].ToString());
                        l.leg_id = int.Parse(json["result"][i]["leg_id"].ToString());
                        l.type = json["result"][i]["type"].ToString();
                        l.name = json["result"][i]["name"].ToString();
                        l.level = int.Parse(json["result"][i]["level"].ToString());
                        l.price = float.Parse(json["result"][i]["price"].ToString());
                        l.medicament = int.Parse(json["result"][i]["medicament"].ToString());
                        l.life = int.Parse(json["result"][i]["life"].ToString());
                        l.attack = int.Parse(json["result"][i]["attack"].ToString());
                        l.defence = int.Parse(json["result"][i]["defence"].ToString());
                        l.velocity = int.Parse(json["result"][i]["velocity"].ToString());

                        AllLegs.Add(l);
                    }

                    //Debug.LogError("AllLegs Count: " + AllLegs.Count);

                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    public void GetUserInfo(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetUserInfo");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);
        //Debug.LogError(user_id);
        Get("/" + GB.g_APIUserInfo, data, callbackFunction);
    }

    private int callbackUserInfoFirstTime(WWW www)
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
                User.username = json["result"]["username"].ToString();
                User.gems = int.Parse(json["result"]["gems"].ToString());
                User.rank = int.Parse(json["result"]["rank"].ToString());
                User.win = int.Parse(json["result"]["win"].ToString());
                User.loss = int.Parse(json["result"]["loss"].ToString());
                User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                User.Robots.Clear();
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

                    User.AddRobot(r);
                }
                User.Heads.Clear();
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

                    User.AddHead(hc);
                }
                User.LeftArms.Clear();
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

                    User.AddLeftArm(lac);
                }
                User.RightArms.Clear();
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

                    User.AddRightArm(rac);
                }
                User.Legs.Clear();
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

                    User.AddLeg(lc);
                }

                MainMenuPanel.instance.Refresh();

                GetPromotionItems(GB.g_MyID, callbackPromotionItems);
            }
            else if (status == "fail")
            {
                Debug.Log("GetUserInfo: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetUserInfo: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(userInfoJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    User.username = json["result"]["username"].ToString();
                    User.gems = int.Parse(json["result"]["gems"].ToString());
                    User.rank = int.Parse(json["result"]["rank"].ToString());
                    User.win = int.Parse(json["result"]["win"].ToString());
                    User.loss = int.Parse(json["result"]["loss"].ToString());
                    User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                    User.Robots.Clear();
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

                        User.AddRobot(r);
                    }
                    User.Heads.Clear();
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

                        User.AddHead(hc);
                    }
                    User.LeftArms.Clear();
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

                        User.AddLeftArm(lac);
                    }
                    User.RightArms.Clear();
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

                        User.AddRightArm(rac);
                    }
                    User.Legs.Clear();
                    for (int i = 0; i < json["result"]["legs"].Count; i++)
                    {
                        Leg lc = new Leg();
                        lc.object_id = int.Parse(json["result"]["legs"][i]["object_id"].ToString());
                        lc.leg_id = int.Parse(json["result"]["legs"][i]["leg_id"].ToString());
                        lc.type = json["result"]["legs"][i]["type"].ToString();
                        lc.name = json["result"]["legs"][i]["name"].ToString();
                        lc.level = int.Parse(json["result"]["legs"][i]["level"].ToString());
                        lc.price = float.Parse(json["result"]["legs"][i]["price"].ToString());
                        lc.medicament = int.Parse(json["result"]["legs"][i]["medicament"].ToString());
                        lc.life = float.Parse(json["result"]["legs"][i]["life"].ToString());
                        lc.attack = float.Parse(json["result"]["legs"][i]["attack"].ToString());
                        lc.defence = float.Parse(json["result"]["legs"][i]["defence"].ToString());
                        lc.velocity = float.Parse(json["result"]["legs"][i]["velocity"].ToString());
                        lc.onSale = int.Parse(json["result"]["legs"][i]["onSale"].ToString());
                        lc.salePrice = int.Parse(json["result"]["legs"][i]["salePrice"].ToString());

                        User.AddLeg(lc);
                    }

                    MainMenuPanel.instance.Refresh();
                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    public int callbackUserInfo(WWW www)
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
                User.username = json["result"]["username"].ToString();
                User.gems = int.Parse(json["result"]["gems"].ToString());
                User.rank = int.Parse(json["result"]["rank"].ToString());
                User.win = int.Parse(json["result"]["win"].ToString());
                User.loss = int.Parse(json["result"]["loss"].ToString());
                User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                User.Robots.Clear();
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

                    User.AddRobot(r);
                }
                User.Heads.Clear();
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

                    User.AddHead(hc);
                }
                User.LeftArms.Clear();
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

                    User.AddLeftArm(lac);
                }
                User.RightArms.Clear();
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

                    User.AddRightArm(rac);
                }
                User.Legs.Clear();
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

                    User.AddLeg(lc);
                }

                MainMenuPanel.instance.Refresh();
            }
            else if (status == "fail")
            {
                Debug.Log("GetUserInfo: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetUserInfo: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(userInfoJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    User.username = json["result"]["username"].ToString();
                    User.gems = int.Parse(json["result"]["gems"].ToString());
                    User.rank = int.Parse(json["result"]["rank"].ToString());
                    User.win = int.Parse(json["result"]["win"].ToString());
                    User.loss = int.Parse(json["result"]["loss"].ToString());
                    User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                    User.Robots.Clear();
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

                        User.AddRobot(r);
                    }
                    User.Heads.Clear();
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

                        User.AddHead(hc);
                    }
                    User.LeftArms.Clear();
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

                        User.AddLeftArm(lac);
                    }
                    User.RightArms.Clear();
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

                        User.AddRightArm(rac);
                    }
                    User.Legs.Clear();
                    for (int i = 0; i < json["result"]["legs"].Count; i++)
                    {
                        Leg lc = new Leg();
                        lc.object_id = int.Parse(json["result"]["legs"][i]["object_id"].ToString());
                        lc.leg_id = int.Parse(json["result"]["legs"][i]["leg_id"].ToString());
                        lc.type = json["result"]["legs"][i]["type"].ToString();
                        lc.name = json["result"]["legs"][i]["name"].ToString();
                        lc.level = int.Parse(json["result"]["legs"][i]["level"].ToString());
                        lc.price = float.Parse(json["result"]["legs"][i]["price"].ToString());
                        lc.medicament = int.Parse(json["result"]["legs"][i]["medicament"].ToString());
                        lc.life = float.Parse(json["result"]["legs"][i]["life"].ToString());
                        lc.attack = float.Parse(json["result"]["legs"][i]["attack"].ToString());
                        lc.defence = float.Parse(json["result"]["legs"][i]["defence"].ToString());
                        lc.velocity = float.Parse(json["result"]["legs"][i]["velocity"].ToString());
                        lc.onSale = int.Parse(json["result"]["legs"][i]["onSale"].ToString());
                        lc.salePrice = int.Parse(json["result"]["legs"][i]["salePrice"].ToString());

                        User.AddLeg(lc);
                    }

                    MainMenuPanel.instance.Refresh();
                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    private void GetLevels(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("GetLevels");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        Get("/" + GB.g_APIGetUserLevels, data, callbackFunction);
    }

    private int callbackLevels(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("GetLevels: Done");
                Debug.Log(www.text);
                UserLevels = new List<UserLevel>();
                for (int i = 0; i < json["result"]["levels"].Count; i++)
                {
                    UserLevel ul = new UserLevel();
                    ul.battle_id = int.Parse(json["result"]["levels"][i]["battle_id"].ToString());
                    ul.level_id = int.Parse(json["result"]["levels"][i]["level_id"].ToString());
                    ul.env_id = int.Parse(json["result"]["levels"][i]["env_id"].ToString());
                    ul.passed = int.Parse(json["result"]["levels"][i]["passed"].ToString());
                    ul.locked = int.Parse(json["result"]["levels"][i]["locked"].ToString());
                    ul.win_count = int.Parse(json["result"]["levels"][i]["win_count"].ToString());

                    ul.opponent_robots = new List<int>();
                    for (int j = 0; j < json["result"]["levels"][i]["opponent_robots"].Count; j++)
                    {
                        //Debug.LogError(json["result"][i]["opponent_robots"].Count);
                        ul.opponent_robots.Add(int.Parse(json["result"]["levels"][i]["opponent_robots"][j].ToString()));
                    }

                    ul.reward1 = new LevelReward();
                    ul.reward1.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward1"]["reward_type"].ToString());
                    ul.reward1.reward_count = int.Parse(json["result"]["levels"][i]["reward1"]["reward_count"].ToString());

                    ul.reward1.robot_ids = new List<int>();
                    for (int j = 0; j < json["result"]["levels"][i]["reward1"]["robot_ids"].Count; j++)
                    {
                        ul.reward1.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward1"]["robot_ids"][j].ToString()));
                    }
                    ul.reward1.medicament = int.Parse(json["result"]["levels"][i]["reward1"]["medicament"].ToString());
                    ul.reward1.gems = int.Parse(json["result"]["levels"][i]["reward1"]["gems"].ToString());



                    ul.reward2 = new LevelReward();
                    ul.reward2.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward2"]["reward_type"].ToString());
                    ul.reward2.reward_count = int.Parse(json["result"]["levels"][i]["reward2"]["reward_count"].ToString());

                    ul.reward2.robot_ids = new List<int>();
                    for (int j = 0; j < json["result"]["levels"][i]["reward2"]["robot_ids"].Count; j++)
                    {
                        ul.reward2.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward2"]["robot_ids"][j].ToString()));
                    }
                    ul.reward2.medicament = int.Parse(json["result"]["levels"][i]["reward2"]["medicament"].ToString());
                    ul.reward2.gems = int.Parse(json["result"]["levels"][i]["reward2"]["gems"].ToString());

                    UserLevels.Add(ul);
                }
                //Debug.LogError("Levels: " + UserLevels.Count);

                //Debug.LogError("BasicRobots Count: " + BasicRobots.Count);
                //toJson();
            }
            else if (status == "fail")
            {
                Debug.Log("GetLevels: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("GetLevels: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allLevelsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    UserLevels = new List<UserLevel>();
                    for (int i = 0; i < json["result"]["levels"].Count; i++)
                    {
                        UserLevel ul = new UserLevel();
                        ul.battle_id = int.Parse(json["result"]["levels"][i]["battle_id"].ToString());
                        ul.level_id = int.Parse(json["result"]["levels"][i]["level_id"].ToString());
                        ul.env_id = int.Parse(json["result"]["levels"][i]["env_id"].ToString());
                        ul.passed = int.Parse(json["result"]["levels"][i]["passed"].ToString());
                        ul.locked = int.Parse(json["result"]["levels"][i]["locked"].ToString());
                        ul.win_count = int.Parse(json["result"]["levels"][i]["win_count"].ToString());

                        ul.opponent_robots = new List<int>();
                        for (int j = 0; j < json["result"]["levels"][i]["opponent_robots"].Count; j++)
                        {
                            //Debug.LogError(json["result"][i]["opponent_robots"].Count);
                            ul.opponent_robots.Add(int.Parse(json["result"]["levels"][i]["opponent_robots"][j].ToString()));
                        }

                        ul.reward1 = new LevelReward();
                        ul.reward1.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward1"]["reward_type"].ToString());
                        ul.reward1.reward_count = int.Parse(json["result"]["levels"][i]["reward1"]["reward_count"].ToString());

                        ul.reward1.robot_ids = new List<int>();
                        for (int j = 0; j < json["result"]["levels"][i]["reward1"]["robot_ids"].Count; j++)
                        {
                            ul.reward1.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward1"]["robot_ids"][j].ToString()));
                        }
                        ul.reward1.medicament = int.Parse(json["result"]["levels"][i]["reward1"]["medicament"].ToString());
                        ul.reward1.gems = int.Parse(json["result"]["levels"][i]["reward1"]["gems"].ToString());



                        ul.reward2 = new LevelReward();
                        ul.reward2.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward2"]["reward_type"].ToString());
                        ul.reward2.reward_count = int.Parse(json["result"]["levels"][i]["reward2"]["reward_count"].ToString());

                        ul.reward2.robot_ids = new List<int>();
                        for (int j = 0; j < json["result"]["levels"][i]["reward2"]["robot_ids"].Count; j++)
                        {
                            ul.reward2.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward2"]["robot_ids"][j].ToString()));
                        }
                        ul.reward2.medicament = int.Parse(json["result"]["levels"][i]["reward2"]["medicament"].ToString());
                        ul.reward2.gems = int.Parse(json["result"]["levels"][i]["reward2"]["gems"].ToString());

                        UserLevels.Add(ul);
                    }
                    //Debug.LogError("Levels: " + UserLevels.Count);

                    //Debug.LogError("BasicRobots Count: " + BasicRobots.Count);
                    //toJson();
                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    public void UpdateUserGems(int user_id, int gems, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("UpdateUserGems");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);
        data.Add("gems_to_add", gems);

        Get("/" + GB.g_APIUpdateUserGems, data, callbackFunction);
    }

    public void ChangeHead(int user_id, Head head, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);
        data.Add("head_object_id", head.object_id);
        data.Add("level", head.level);
        data.Add("medicament", head.medicament);
        data.Add("life", head.life);
        data.Add("attack", head.attack);
        data.Add("defence", head.defence);
        data.Add("onSale", head.onSale);
        data.Add("salePrice", head.salePrice);
        data.Add("is_broken", head.is_broken);
        data.Add("break_timestamp", head.break_timestamp);

        Get("/" + GB.g_APIChangeHead, data, callbackFunction);
    }

    public void ChangeLeftArm(int user_id, LeftArm larm, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeLeftArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);
        data.Add("left_arm_object_id", larm.object_id);
        data.Add("level", larm.level);
        data.Add("medicament", larm.medicament);
        data.Add("life", larm.life);
        data.Add("attack", larm.attack);
        data.Add("defence", larm.defence);
        data.Add("onSale", larm.onSale);
        data.Add("salePrice", larm.salePrice);
        data.Add("is_broken", larm.is_broken);
        data.Add("break_timestamp", larm.break_timestamp);

        Get("/" + GB.g_APIChangeLeftArm, data, callbackFunction);
    }

    public void ChangeRightArm(int user_id, RightArm rarm, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeRightArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);
        data.Add("right_arm_object_id", rarm.object_id);
        data.Add("level", rarm.level);
        data.Add("medicament", rarm.medicament);
        data.Add("life", rarm.life);
        data.Add("attack", rarm.attack);
        data.Add("defence", rarm.defence);
        data.Add("onSale", rarm.onSale);
        data.Add("salePrice", rarm.salePrice);
        data.Add("is_broken", rarm.is_broken);
        data.Add("break_timestamp", rarm.break_timestamp);

        Get("/" + GB.g_APIChangeRightArm, data, callbackFunction);
    }

    public void ChangeLegs(int user_id, Leg leg, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeLegs");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);
        data.Add("legs_object_id", leg.object_id);
        data.Add("level", leg.level);
        data.Add("medicament", leg.medicament);
        data.Add("life", leg.life);
        data.Add("attack", leg.attack);
        data.Add("defence", leg.defence);
        data.Add("velocity", leg.velocity);
        data.Add("onSale", leg.onSale);
        data.Add("salePrice", leg.salePrice);
        data.Add("is_broken", leg.is_broken);
        data.Add("break_timestamp", leg.break_timestamp);

        Get("/" + GB.g_APIChangeLegs, data, callbackFunction);
    }

    public void RemoveHead(Head head, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("RemoveHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("head_object_id", head.object_id);

        Get("/" + GB.g_APIRemoveHead, data, callbackFunction);
    }

    public void RemoveLeftArm(LeftArm larm, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("RemoveLeftArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("left_arm_object_id", larm.object_id);

        Get("/" + GB.g_APIRemoveLeftArm, data, callbackFunction);
    }

    public void RemoveRightArm(RightArm rarm, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("RemoveRightArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("right_arm_object_id", rarm.object_id);

        Get("/" + GB.g_APIRemoveRightArm, data, callbackFunction);
    }

    public void RemoveLeg(Leg leg, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("RemoveLeg");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("leg_object_id", leg.object_id);

        Get("/" + GB.g_APIRemoveLeg, data, callbackFunction);
    }

    public void RemoveRobot(Robot robot, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("RemoveRobot");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("robot_object_id", robot.object_id);

        Get("/" + GB.g_APIRemoveRobot, data, callbackFunction);
    }

    public void AddHead(Head head, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("AddHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("head_id", head.head_id);
        data.Add("type", head.type);
        data.Add("name", head.name);
        data.Add("level", head.level);
        data.Add("price", head.price);
        data.Add("medicament", head.medicament);
        data.Add("life", head.life);
        data.Add("attack", head.attack);
        data.Add("defence", head.defence);
        data.Add("velocity", head.velocity);
        data.Add("onSale", head.onSale);
        data.Add("salePrice", head.salePrice);

        Get("/" + GB.g_APIAddHead, data, callbackFunction);
    }

    public void AddLeftArm(LeftArm larm, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("AddLeftArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("larm_id", larm.larm_id);
        data.Add("type", larm.type);
        data.Add("name", larm.name);
        data.Add("level", larm.level);
        data.Add("price", larm.price);
        data.Add("medicament", larm.medicament);
        data.Add("life", larm.life);
        data.Add("attack", larm.attack);
        data.Add("defence", larm.defence);
        data.Add("velocity", larm.velocity);
        data.Add("onSale", larm.onSale);
        data.Add("salePrice", larm.salePrice);

        Get("/" + GB.g_APIAddLeftArm, data, callbackFunction);
    }

    public void AddRightArm(RightArm rarm, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("AddRightArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("rarm_id", rarm.rarm_id);
        data.Add("type", rarm.type);
        data.Add("name", rarm.name);
        data.Add("level", rarm.level);
        data.Add("price", rarm.price);
        data.Add("medicament", rarm.medicament);
        data.Add("life", rarm.life);
        data.Add("attack", rarm.attack);
        data.Add("defence", rarm.defence);
        data.Add("velocity", rarm.velocity);
        data.Add("onSale", rarm.onSale);
        data.Add("salePrice", rarm.salePrice);

        Get("/" + GB.g_APIAddRightArm, data, callbackFunction);
    }

    public void AddLeg(Leg leg, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("AddLeftArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("leg_id", leg.leg_id);
        data.Add("type", leg.type);
        data.Add("name", leg.name);
        data.Add("level", leg.level);
        data.Add("price", leg.price);
        data.Add("medicament", leg.medicament);
        data.Add("life", leg.life);
        data.Add("attack", leg.attack);
        data.Add("defence", leg.defence);
        data.Add("velocity", leg.velocity);
        data.Add("onSale", leg.onSale);
        data.Add("salePrice", leg.salePrice);

        Get("/" + GB.g_APIAddLeg, data, callbackFunction);
    }

    public void AddRobot(Robot robot, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("AddRobot");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);

        data.Add("head_id", robot.Head.head_id);
        data.Add("head_type", robot.Head.type);
        data.Add("head_name", robot.Head.name);
        data.Add("head_level", robot.Head.level);
        data.Add("head_price", robot.Head.price);
        data.Add("head_medicament", robot.Head.medicament);
        data.Add("head_life", robot.Head.life);
        data.Add("head_attack", robot.Head.attack);
        data.Add("head_defence", robot.Head.defence);
        data.Add("head_velocity", robot.Head.velocity);
        data.Add("head_onSale", robot.Head.onSale);
        data.Add("head_salePrice", robot.Head.salePrice);

        data.Add("larm_id", robot.LeftArm.larm_id);
        data.Add("larm_type", robot.LeftArm.type);
        data.Add("larm_name", robot.LeftArm.name);
        data.Add("larm_level", robot.LeftArm.level);
        data.Add("larm_price", robot.LeftArm.price);
        data.Add("larm_medicament", robot.LeftArm.medicament);
        data.Add("larm_life", robot.LeftArm.life);
        data.Add("larm_attack", robot.LeftArm.attack);
        data.Add("larm_defence", robot.LeftArm.defence);
        data.Add("larm_velocity", robot.LeftArm.velocity);
        data.Add("larm_onSale", robot.LeftArm.onSale);
        data.Add("larm_salePrice", robot.LeftArm.salePrice);

        data.Add("rarm_id", robot.RightArm.rarm_id);
        data.Add("rarm_type", robot.RightArm.type);
        data.Add("rarm_name", robot.RightArm.name);
        data.Add("rarm_level", robot.RightArm.level);
        data.Add("rarm_price", robot.RightArm.price);
        data.Add("rarm_medicament", robot.RightArm.medicament);
        data.Add("rarm_life", robot.RightArm.life);
        data.Add("rarm_attack", robot.RightArm.attack);
        data.Add("rarm_defence", robot.RightArm.defence);
        data.Add("rarm_velocity", robot.RightArm.velocity);
        data.Add("rarm_onSale", robot.RightArm.onSale);
        data.Add("rarm_salePrice", robot.RightArm.salePrice);

        data.Add("leg_id", robot.Leg.leg_id);
        data.Add("leg_type", robot.Leg.type);
        data.Add("leg_name", robot.Leg.name);
        data.Add("leg_level", robot.Leg.level);
        data.Add("leg_price", robot.Leg.price);
        data.Add("leg_medicament", robot.Leg.medicament);
        data.Add("leg_life", robot.Leg.life);
        data.Add("leg_attack", robot.Leg.attack);
        data.Add("leg_defence", robot.Leg.defence);
        data.Add("leg_velocity", robot.Leg.velocity);
        data.Add("leg_onSale", robot.Leg.onSale);
        data.Add("leg_salePrice", robot.Leg.salePrice);

        Get("/" + GB.g_APIAddRobot, data, callbackFunction);
    }

    public void UpdateUserLevel(int battleId, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("UpdateUserLevel");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("battle_id", battleId);

        Get("/" + GB.g_APIUpdateUserLevel, data, callbackUpdateLevels);
    }

    private int callbackUpdateLevels(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            //Debug.LogError(www.text);
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("UpdateUserLevel: Done");
                Debug.Log(www.text);
                UserLevels = new List<UserLevel>();
                for (int i = 0; i < json["result"]["levels"].Count; i++)
                {
                    UserLevel ul = new UserLevel();
                    ul.battle_id = int.Parse(json["result"]["levels"][i]["battle_id"].ToString());
                    ul.level_id = int.Parse(json["result"]["levels"][i]["level_id"].ToString());
                    ul.env_id = int.Parse(json["result"]["levels"][i]["env_id"].ToString());
                    ul.passed = int.Parse(json["result"]["levels"][i]["passed"].ToString());
                    ul.locked = int.Parse(json["result"]["levels"][i]["locked"].ToString());
                    ul.win_count = int.Parse(json["result"]["levels"][i]["win_count"].ToString());

                    ul.opponent_robots = new List<int>();
                    for (int j = 0; j < json["result"]["levels"][i]["opponent_robots"].Count; j++)
                    {
                        //Debug.LogError(json["result"][i]["opponent_robots"].Count);
                        ul.opponent_robots.Add(int.Parse(json["result"]["levels"][i]["opponent_robots"][j].ToString()));
                    }

                    ul.reward1 = new LevelReward();
                    ul.reward1.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward1"]["reward_type"].ToString());
                    ul.reward1.reward_count = int.Parse(json["result"]["levels"][i]["reward1"]["reward_count"].ToString());

                    ul.reward1.robot_ids = new List<int>();
                    for (int j = 0; j < json["result"]["levels"][i]["reward1"]["robot_ids"].Count; j++)
                    {
                        ul.reward1.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward1"]["robot_ids"][j].ToString()));
                    }
                    ul.reward1.medicament = int.Parse(json["result"]["levels"][i]["reward1"]["medicament"].ToString());
                    ul.reward1.gems = int.Parse(json["result"]["levels"][i]["reward1"]["gems"].ToString());



                    ul.reward2 = new LevelReward();
                    ul.reward2.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward2"]["reward_type"].ToString());
                    ul.reward2.reward_count = int.Parse(json["result"]["levels"][i]["reward2"]["reward_count"].ToString());

                    ul.reward2.robot_ids = new List<int>();
                    for (int j = 0; j < json["result"]["levels"][i]["reward2"]["robot_ids"].Count; j++)
                    {
                        ul.reward2.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward2"]["robot_ids"][j].ToString()));
                    }
                    ul.reward2.medicament = int.Parse(json["result"]["levels"][i]["reward2"]["medicament"].ToString());
                    ul.reward2.gems = int.Parse(json["result"]["levels"][i]["reward2"]["gems"].ToString());

                    UserLevels.Add(ul);
                }
                //Debug.LogError("Levels: " + UserLevels.Count);

                //Debug.LogError("BasicRobots Count: " + BasicRobots.Count);
                //toJson();
            }
            else if (status == "fail")
            {
                Debug.LogError("UpdateUserLevel: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                Debug.Log("UpdateUserLevel: Error");
                Debug.Log(www.error);
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                JsonData json = JsonMapper.ToObject(allLevelsJson.text);
                string status = json["status"].ToString();
                if (status == "success")
                {
                    //LoadingPanel.instance.taskCompleted++;
                    UserLevels = new List<UserLevel>();
                    for (int i = 0; i < json["result"]["levels"].Count; i++)
                    {
                        UserLevel ul = new UserLevel();
                        ul.battle_id = int.Parse(json["result"]["levels"][i]["battle_id"].ToString());
                        //ul.level_id = int.Parse(json["result"]["levels"][i]["level_id"].ToString());
                        //ul.env_id = int.Parse(json["result"]["levels"][i]["env_id"].ToString());
                        ul.passed = int.Parse(json["result"]["levels"][i]["passed"].ToString());
                        ul.locked = int.Parse(json["result"]["levels"][i]["locked"].ToString());
                        ul.win_count = int.Parse(json["result"]["levels"][i]["win_count"].ToString());

                        ul.opponent_robots = new List<int>();
                        for (int j = 0; j < json["result"]["levels"][i]["opponent_robots"].Count; j++)
                        {
                            //Debug.LogError(json["result"][i]["opponent_robots"].Count);
                            ul.opponent_robots.Add(int.Parse(json["result"]["levels"][i]["opponent_robots"][j].ToString()));
                        }

                        ul.reward1 = new LevelReward();
                        ul.reward1.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward1"]["reward_type"].ToString());
                        ul.reward1.reward_count = int.Parse(json["result"]["levels"][i]["reward1"]["reward_count"].ToString());

                        ul.reward1.robot_ids = new List<int>();
                        for (int j = 0; j < json["result"]["levels"][i]["reward1"]["robot_ids"].Count; j++)
                        {
                            ul.reward1.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward1"]["robot_ids"][j].ToString()));
                        }
                        ul.reward1.medicament = int.Parse(json["result"]["levels"][i]["reward1"]["medicament"].ToString());
                        ul.reward1.gems = int.Parse(json["result"]["levels"][i]["reward1"]["gems"].ToString());



                        ul.reward2 = new LevelReward();
                        ul.reward2.reward_type = (RewardType)int.Parse(json["result"]["levels"][i]["reward2"]["reward_type"].ToString());
                        ul.reward2.reward_count = int.Parse(json["result"]["levels"][i]["reward2"]["reward_count"].ToString());

                        ul.reward2.robot_ids = new List<int>();
                        for (int j = 0; j < json["result"]["levels"][i]["reward2"]["robot_ids"].Count; j++)
                        {
                            ul.reward2.robot_ids.Add(int.Parse(json["result"]["levels"][i]["reward2"]["robot_ids"][j].ToString()));
                        }
                        ul.reward2.medicament = int.Parse(json["result"]["levels"][i]["reward2"]["medicament"].ToString());
                        ul.reward2.gems = int.Parse(json["result"]["levels"][i]["reward2"]["gems"].ToString());

                        UserLevels.Add(ul);
                    }
                    //Debug.LogError("Levels: " + UserLevels.Count);

                    //Debug.LogError("BasicRobots Count: " + BasicRobots.Count);
                    //toJson();
                }
                else if (status == "fail")
                {
                    //Debug.LogError(status);
                    MobileNative.Alert("Error", "Username or password is wrong.", "OK");
                }
            }
        }
        return 0;
    }

    public void RewardAdventure(int battleId, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("RewardAdventure");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("battle_id", battleId);

        Get("/" + GB.g_APIRewardAdventure, data, callbackFunction);
    }
    public static float CalculateImagineTime(int level, int med)
    {
        float temp = 0;
        float time = 0;
        switch (level)
        {
            case 1:
                temp = (1 - (0.5f * med));
                time = temp <= 0 ? 0 : temp;
                break;
            case 2:
                temp = (3 - (0.5f * med));
                time = temp <= 0 ? 0 : temp;
                break;
            case 3:
                temp = (5 - (0.5f * med));
                time = temp <= 0 ? 0 : temp;
                break;
            case 4:
                temp = (12 - (0.5f * med));
                time = temp <= 0 ? 0 : temp;
                break;
            case 5:
                temp = (24 - (0.5f * med));
                time = temp <= 0 ? 0 : temp;
                break;
        }
        return time;
    }

    public static float ActualImagineTime(int level)
    {
        float time = 0;
        switch (level)
        {
            case 1:
                time = 1;
            break;
            case 2:
                time = 3;
            break;
            case 3:
                time = 5;
            break;
            case 4:
                time = 12;
            break;
            case 5:
                time = 24;
            break;
        }
        return time;
    }

    public static string DataPath(string path2)
    {
        string filePath = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //filePath = Path.Combine(Application.dataPath, "Resources/" + path2);
            filePath = Path.Combine(Application.dataPath, path2);
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            filePath = Path.Combine(Application.dataPath, path2);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            filePath = Path.Combine(Application.persistentDataPath, path2);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            filePath = Path.Combine(Application.persistentDataPath, path2);
        }
        else
        {
            filePath = Path.Combine(Application.dataPath, path2);
        }
        return filePath;
    }
}
