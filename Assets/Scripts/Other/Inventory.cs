using LitJson;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static Inventory s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static Inventory instance {
        get {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(Inventory)) as Inventory;
                if (s_Instance == null)
                    Debug.Log("Could not locate an Inventory object. \n You have to have exactly one PR_Utility in the scene.");
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

    public List<GameObject> headObjects = new List<GameObject>();
    public List<GameObject> larmObjects = new List<GameObject>();
    public List<GameObject> rarmObjects = new List<GameObject>();
    public List<GameObject> legObjects = new List<GameObject>();

    public List<GameObject> skeletonHeadObjects;
    public List<GameObject> skeletonLeftArmObjects;
    public List<GameObject> skeletonRightArmObjects;
    public List<GameObject> skeletonLegObjects;

    public Text m_name, m_warningText;
    public List<Text> m_Properties = new List<Text>();
    public List<Image> m_Abilities = new List<Image>();
    public List<Text> m_AbilityData = new List<Text>();
    public List<Text> m_Pieces = new List<Text>();
    public List<Button> pieceChangingButtonList = new List<Button>();

    int m_Index;


    public List<GameObject> SubPanelList = new List<GameObject>();

    bool bFirst = false;

    List<Robot> myRobots = new List<Robot>();
    List<Head> myHeads = new List<Head>();
    List<LeftArm> myLeftArms = new List<LeftArm>();
    List<RightArm> myRightArms = new List<RightArm>();
    List<Leg> myLegs = new List<Leg>();

    public Robot CurrentRobot { get; private set; }

    private int CurrentRobotTypeId {
        get {
            int typeId = -1;
            switch (CurrentRobot.type)
            {
                case "HM":
                    typeId = 0;
                    break;
                case "FL":
                    typeId = 1;
                    break;
                case "RL":
                    typeId = 2;
                    break;
                case "SP":
                    typeId = 3;
                    break;
            }
            return typeId;
        }
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            SubPanelList.Add(transform.GetChild(i).gameObject);
        }

        ShowSubPanel(0);

        bFirst = true;

        m_Index = 0;
        //PrevRobot();
    }

    void GetRobotDetails()
    {
        myRobots = GameManage.User.Robots;

        myHeads = GameManage.User.Heads;
        //for (int i = 0; i < GameManage.User.heads.Count; i++)
        //{
        //    Head h = GameManage.AllHeads.Find(x => (x.head_id == GameManage.User.heads[i].head_id));
        //    Head userHead = new Head();
        //    int level = GameManage.User.heads[i].level;
        //    userHead.object_id = h.object_id;
        //    userHead.head_id = h.head_id;
        //    userHead.type = h.type; ;
        //    userHead.name = h.name;
        //    userHead.level = level;
        //    userHead.price = h.price;
        //    userHead.life = (h.life / 5) * level;
        //    userHead.attack = (h.attack / 5) * level;
        //    userHead.defence = (h.defence / 5) * level;
        //    userHead.velocity = (h.velocity / 5) * level;
        //    userHead.onSale = h.onSale;
        //    userHead.salePrice = h.salePrice;
        //    myHeads.Add(userHead);
        //}

        myLeftArms = GameManage.User.LeftArms;
        //for (int i = 0; i < GameManage.User.larms.Count; i++)
        //{
        //    LeftArm la = GameManage.AllLeftArms.Find(x => (x.larm_id == GameManage.User.larms[i].larm_id));
        //    LeftArm userLeftArm = new LeftArm();
        //    int level = GameManage.User.larms[i].level;
        //    userLeftArm.object_id = la.object_id;
        //    userLeftArm.larm_id = la.larm_id;
        //    userLeftArm.type = la.type; ;
        //    userLeftArm.name = la.name;
        //    userLeftArm.level = level;
        //    userLeftArm.price = la.price;
        //    userLeftArm.life = (la.life / 5) * level;
        //    userLeftArm.attack = (la.attack / 5) * level;
        //    userLeftArm.defence = (la.defence / 5) * level;
        //    userLeftArm.velocity = (la.velocity / 5) * level;
        //    userLeftArm.onSale = la.onSale;
        //    userLeftArm.salePrice = la.salePrice;
        //    myLeftArms.Add(userLeftArm);
        //}

        myRightArms = GameManage.User.RightArms;
        //for (int i = 0; i < GameManage.User.rarms.Count; i++)
        //{
        //    RightArm ra = GameManage.AllRightArms.Find(x => (x.rarm_id == GameManage.User.rarms[i].rarm_id));
        //    RightArm userRightArm = new RightArm();
        //    int level = GameManage.User.rarms[i].level;
        //    userRightArm.object_id = ra.object_id;
        //    userRightArm.rarm_id = ra.rarm_id;
        //    userRightArm.type = ra.type; ;
        //    userRightArm.name = ra.name;
        //    userRightArm.level = level;
        //    userRightArm.price = ra.price;
        //    userRightArm.life = (ra.life / 5) * level;
        //    userRightArm.attack = (ra.attack / 5) * level;
        //    userRightArm.defence = (ra.defence / 5) * level;
        //    userRightArm.velocity = (ra.velocity / 5) * level;
        //    userRightArm.onSale = ra.onSale;
        //    userRightArm.salePrice = ra.salePrice;
        //    myRightArms.Add(userRightArm);
        //}

        myLegs = GameManage.User.Legs;
        //for (int i = 0; i < GameManage.User.legs.Count; i++)
        //{
        //    Leg l = GameManage.AllLegs.Find(x => (x.leg_id == GameManage.User.legs[i].leg_id));
        //    Leg userLeg = new Leg();
        //    int level = GameManage.User.legs[i].level;
        //    userLeg.object_id = l.object_id;
        //    userLeg.leg_id = l.leg_id;
        //    userLeg.type = l.type; ;
        //    userLeg.name = l.name;
        //    userLeg.level = level;
        //    userLeg.price = l.price;
        //    userLeg.life = (l.life / 5) * level;
        //    userLeg.attack = (l.attack / 5) * level;
        //    userLeg.defence = (l.defence / 5) * level;
        //    userLeg.velocity = (l.velocity / 5) * level;
        //    userLeg.onSale = l.onSale;
        //    userLeg.salePrice = l.salePrice;
        //    myLegs.Add(userLeg);
        //}
    }

    void OnEnable()
    {
        Init();

        if (bFirst)
        {
            ShowSubPanel(0);
        }
    }

    public void Init()
    {
        GetRobotDetails();
        ShowRobot(m_Index);
        ShowRobotInfo(m_Index);
    }

    public void ShowRobot(int id)
    {
        //currentRobot = myRobots[id];
        CurrentRobot = GameManage.User.Robots[id];
        //Debug.Log("Robot id: " + CurrentRobot.object_id + "onSale: " + CurrentRobot.onSale);
        if (CurrentRobot.Head != null && CurrentRobot.Head.is_broken == 0)
        {
            ShowSkeletonHead(-1);
            ShowHead(CurrentRobot.Head.head_id);
        }
        else
        {
            ShowSkeletonHead(CurrentRobotTypeId);
        }
        if (CurrentRobot.LeftArm != null && CurrentRobot.LeftArm.is_broken == 0)
        {
            ShowSkeletonLeftArm(-1);
            ShowLeftArm(CurrentRobot.LeftArm.larm_id);
        }
        else
        {
            ShowSkeletonLeftArm(CurrentRobotTypeId);
        }
        if (CurrentRobot.RightArm != null && CurrentRobot.RightArm.is_broken == 0)
        {
            ShowSkeletonRightArm(-1);
            ShowRightArm(CurrentRobot.RightArm.rarm_id);
        }
        else
        {
            ShowSkeletonRightArm(CurrentRobotTypeId);
        }
        if (CurrentRobot.Leg != null && CurrentRobot.Leg.is_broken == 0)
        {
            ShowSkeletonLeg(-1);
            ShowLeg(CurrentRobot.Leg.leg_id);
        }
        else
        {
            ShowSkeletonLeg(CurrentRobotTypeId);
        }

        if (CurrentRobot.Head == null || CurrentRobot.LeftArm == null || CurrentRobot.RightArm == null || CurrentRobot.Leg == null)
        {
            m_warningText.text = "One or more parts lost or sold";
        }
        else if (CurrentRobot.Head.is_broken == 1 || CurrentRobot.LeftArm.is_broken == 1 || CurrentRobot.RightArm.is_broken == 1 || CurrentRobot.Leg.is_broken == 1)
        {
            m_warningText.text = "One or more parts broken";
        }
        else if (CurrentRobot.onSale == 1)
        {
            m_warningText.text = "Robot on sale";
        }
        else if (CurrentRobot.Head.onSale == 1 || CurrentRobot.LeftArm.onSale == 1 || CurrentRobot.RightArm.onSale == 1 || CurrentRobot.Leg.onSale == 1)
        {
            m_warningText.text = "One or more parts on sale";
        }
        else
        {
            m_warningText.text = "";
        }
    }

    private void ShowHead(int id)
    {
        for (int i = 0; i < headObjects.Count; i++)
        {
            if (id == i) headObjects[i].SetActive(true);
            else headObjects[i].SetActive(false);
        }
    }

    private void ShowLeftArm(int id)
    {
        for (int i = 0; i < larmObjects.Count; i++)
        {
            if (id == i) larmObjects[i].SetActive(true);
            else larmObjects[i].SetActive(false);
        }
    }

    private void ShowRightArm(int id)
    {
        for (int i = 0; i < rarmObjects.Count; i++)
        {
            if (id == i) rarmObjects[i].SetActive(true);
            else rarmObjects[i].SetActive(false);
        }
    }

    private void ShowLeg(int id)
    {
        for (int i = 0; i < legObjects.Count; i++)
        {
            if (id == i) legObjects[i].SetActive(true);
            else legObjects[i].SetActive(false);
        }
    }

    void ShowSkeletonHead(int id)
    {
        ShowHead(-1);
        for (int i = 0; i < skeletonHeadObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonHeadObjects[i].SetActive(true);
            }
            else
            {
                skeletonHeadObjects[i].SetActive(false);
            }
        }
    }

    void ShowSkeletonLeftArm(int id)
    {
        ShowLeftArm(-1);
        for (int i = 0; i < skeletonLeftArmObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonLeftArmObjects[i].SetActive(true);
            }
            else
            {
                skeletonLeftArmObjects[i].SetActive(false);
            }
        }
    }

    void ShowSkeletonRightArm(int id)
    {
        ShowRightArm(-1);
        for (int i = 0; i < skeletonRightArmObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonRightArmObjects[i].SetActive(true);
            }
            else
            {
                skeletonRightArmObjects[i].SetActive(false);
            }
        }
    }

    void ShowSkeletonLeg(int id)
    {
        ShowLeg(-1);
        for (int i = 0; i < skeletonLegObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonLegObjects[i].SetActive(true);
            }
            else
            {
                skeletonLegObjects[i].SetActive(false);
            }
        }
    }

    public void PrevRobot()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        m_Index--;
        if (m_Index < 0) m_Index = GameManage.User.robots.Count - 1;

        ShowRobot(m_Index);

        ShowRobotInfo(m_Index);
    }

    public void NextRobot()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        m_Index++;
        if (m_Index >= GameManage.User.robots.Count) m_Index = 0;

        ShowRobot(m_Index);

        ShowRobotInfo(m_Index);
    }

    void ShowRobotInfo(int id)
    {
        Robot robot = myRobots[id];

        m_name.text = robot.Name;

        if (robot.ImagineTime() == 0 || robot.Life == 0 || robot.Attack == 0 || robot.Defence == 0 || robot.Velocity == 0 || robot.MaxLife() == 0 || robot.MaxAttack() == 0 || robot.MaxDefence() == 0 || robot.MaxVelocity() == 0)
        {
            m_Properties[0].text = 0.ToString() + "h";
            m_Properties[1].text = 0.ToString();
            m_Properties[2].text = 0.ToString();
            m_Properties[3].text = 0.ToString();
            m_Properties[4].text = 0.ToString() + "m/s";

            m_Abilities[0].fillAmount = 0;
            m_Abilities[1].fillAmount = 0;
            m_Abilities[2].fillAmount = 0;
            m_Abilities[3].fillAmount = 0;

            m_AbilityData[0].text = 0.ToString() + "%";
            m_AbilityData[1].text = 0.ToString() + "%";
            m_AbilityData[2].text = 0.ToString() + "%";
            m_AbilityData[3].text = 0.ToString() + "%";
        }
        else
        {
            m_Properties[0].text = robot.ImagineTime() + "h";
            m_Properties[1].text = robot.Life.ToString();
            m_Properties[2].text = robot.Attack.ToString();
            m_Properties[3].text = robot.Defence.ToString();
            m_Properties[4].text = robot.Velocity + "m/s";

            m_Abilities[0].fillAmount = robot.Life / robot.MaxLife();
            m_Abilities[1].fillAmount = robot.Attack / robot.MaxAttack();
            m_Abilities[2].fillAmount = robot.Defence / robot.MaxDefence();
            m_Abilities[3].fillAmount = robot.Velocity / robot.MaxVelocity();

            m_AbilityData[0].text = (int)((robot.Life / robot.MaxLife()) * 100f) + "%";
            m_AbilityData[1].text = (int)((robot.Attack / robot.MaxAttack()) * 100f) + "%";
            m_AbilityData[2].text = (int)((robot.Defence / robot.MaxDefence()) * 100f) + "%";
            m_AbilityData[3].text = (int)((robot.Velocity / robot.MaxVelocity()) * 100f) + "%";
        }

        for (int i = 0; i < pieceChangingButtonList.Count; i++)
        {
            pieceChangingButtonList[i].interactable = true;
        }

        //Head
        if (robot.Head == null)
        {
            m_Pieces[0].text = "Lost or Sold";
            m_Pieces[0].color = Color.red;
        }
        else if(robot.Head.is_broken == 1)
        {
            m_Pieces[0].text = "Level " + robot.Head.level.ToString();
            m_Pieces[0].color = Color.red;
        }
        else if (robot.Head.onSale == 1)
        {
            m_Pieces[0].text = "On Sale";
            m_Pieces[0].color = Color.yellow;
        }
        else
        {
            m_Pieces[0].text = "Level " + robot.Head.level.ToString();
            m_Pieces[0].color = Color.yellow;
        }
        //LeftArm
        if (robot.LeftArm == null)
        {
            m_Pieces[1].text = "Lost or Sold";
            m_Pieces[1].color = Color.red;
        }
        else if (robot.LeftArm.is_broken == 1)
        {
            m_Pieces[1].text = "Level " + robot.LeftArm.level.ToString();
            m_Pieces[1].color = Color.red;
        }
        else if (robot.LeftArm.onSale == 1)
        {
            m_Pieces[1].text = "On Sale";
            m_Pieces[1].color = Color.yellow;
        }
        else
        {
            m_Pieces[1].text = "Level " + robot.LeftArm.level.ToString();
            m_Pieces[1].color = Color.yellow;
        }
        //RightArm
        if (robot.RightArm == null)
        {
            m_Pieces[2].text = "Lost or Sold";
            m_Pieces[2].color = Color.red;
        }
        else if (robot.RightArm.is_broken == 1)
        {
            m_Pieces[2].text = "Level " + robot.RightArm.level.ToString();
            m_Pieces[2].color = Color.red;
        }
        else if (robot.RightArm.onSale == 1)
        {
            m_Pieces[2].text = "On Sale";
            m_Pieces[2].color = Color.yellow;
        }
        else
        {
            m_Pieces[2].text = "Level " + robot.RightArm.level.ToString();
            m_Pieces[2].color = Color.yellow;
        }
        //Leg
        if (robot.Leg == null)
        {
            m_Pieces[3].text = "Lost or Sold";
            m_Pieces[3].color = Color.red;
        }
        else if (robot.Leg.is_broken == 1)
        {
            m_Pieces[3].text = "Level " + robot.Leg.level.ToString();
            m_Pieces[3].color = Color.red;
        }
        else if (robot.Leg.onSale == 1)
        {
            m_Pieces[3].text = "On Sale";
            m_Pieces[3].color = Color.yellow;
        }
        else
        {
            m_Pieces[3].text = "Level " + robot.Leg.level.ToString();
            m_Pieces[3].color = Color.yellow;
        }

        if (robot.Head != null && robot.LeftArm != null && robot.RightArm != null && robot.Leg != null)
        {
            if (robot.onSale == 1)
            {
                for (int i = 0; i < pieceChangingButtonList.Count; i++)
                {
                    pieceChangingButtonList[i].interactable = false;
                    //m_Pieces[i].text = "On Sale";
                    //m_Pieces[i].color = Color.yellow;
                }
            }
            else
            {
                for (int i = 0; i < pieceChangingButtonList.Count; i++)
                {
                    pieceChangingButtonList[i].interactable = true;
                }
            }
        }
    }

    public void ChangePiece(PieceType type, int id, int level)
    {
        switch (type)
        {
            case PieceType.Head:
                if (MB_Configs.bDummyData)
                {
                    CurrentRobot.head_object_id = id;
                    CurrentRobot.Head = myHeads.Find(x => x.object_id == CurrentRobot.head_object_id);
                }
                //ChangeRobotHead(0, callbackChangeRobotHead);
                CurrentRobot.head_object_id = id;
                UpdateRobotConfig(CurrentRobot);
                break;
            case PieceType.LeftArm:
                if (MB_Configs.bDummyData)
                {
                    CurrentRobot.larm_object_id = id;
                    CurrentRobot.LeftArm = myLeftArms.Find(x => x.object_id == CurrentRobot.larm_object_id);
                }
                //ChangeRobotLeftArm(0, callbackChangeRobotLeftArm);
                CurrentRobot.larm_object_id = id;
                UpdateRobotConfig(CurrentRobot);
                break;
            case PieceType.RightArm:
                if (MB_Configs.bDummyData)
                {
                    CurrentRobot.rarm_object_id = id;
                    CurrentRobot.RightArm = myRightArms.Find(x => x.object_id == CurrentRobot.rarm_object_id);
                }
                //ChangeRobotRightArm(0, callbackChangeRobotRightArm);
                CurrentRobot.rarm_object_id = id;
                UpdateRobotConfig(CurrentRobot);
                break;
            case PieceType.Leg:
                if (MB_Configs.bDummyData)
                {
                    CurrentRobot.leg_object_id = id;
                    CurrentRobot.Leg = myLegs.Find(x => x.object_id == CurrentRobot.leg_object_id);
                }
                //ChangeRobotLeg(0, callbackChangeRobotLeg);
                CurrentRobot.leg_object_id = id;
                UpdateRobotConfig(CurrentRobot);
                break;
        }

    }

    public Head GetHead(int head_id, int head_level)
    {
        Head head = myHeads.Find(x => x.head_id == head_id && x.level == head_level);
        return head;
    }

    public LeftArm GetLeftArm(int larm_id, int larm_level)
    {
        LeftArm larm = myLeftArms.Find(x => x.larm_id == larm_id && x.level == larm_level);
        return larm;
    }

    public RightArm GetRightArm(int rarm_id, int rarm_level)
    {
        RightArm rarm = myRightArms.Find(x => x.rarm_id == rarm_id && x.level == rarm_level);
        return rarm;
    }

    public Leg GetLeg(int leg_id, int leg_level)
    {
        Leg leg = myLegs.Find(x => x.leg_id == leg_id && x.level == leg_level);
        return leg;
    }

    #region API
    private void AddHead(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIAddHead, data, callbackFunction);
    }

    private int callbackAddRobotHead(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                CurrentRobot.head_object_id = int.Parse(json["result"]["head_id"].ToString());
                ShowSubPanel(0);
                Init();
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                GameManage.User.UpdateLocalFile();
                ShowSubPanel(0);
                Init();
            }
        }
        return 0;
    }

    private void AddLeftArm(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIAddLeftArm, data, callbackFunction);
    }

    private void AddRightArm(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIAddRightArm, data, callbackFunction);
    }

    private void AddLeg(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIAddLeg, data, callbackFunction);
    }

    private void AddRobot(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIAddRobot, data, callbackFunction);
    }

    private void UpdateRobotConfig(Robot robot, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeRobot");
        Hashtable data = new Hashtable();
        data.Add("user_id", GB.g_MyID);
        data.Add("robot_object_id", robot.object_id);
        data.Add("head_object_id", robot.head_object_id);
        data.Add("larm_object_id", robot.larm_object_id);
        data.Add("rarm_object_id", robot.rarm_object_id);
        data.Add("leg_object_id", robot.leg_object_id);
        data.Add("onSale", robot.onSale);
        data.Add("salePrice", robot.salePrice);

        GameManage.Get("/" + GB.g_APIChangeRobot, data, callbackUserInfo);
    }

    private void ChangeRobotHead(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIChangeHead, data, callbackFunction);
    }

    private int callbackChangeRobotHead(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                CurrentRobot.head_object_id = int.Parse(json["result"]["head_id"].ToString());
                ShowSubPanel(0);
                Init();
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                GameManage.User.UpdateLocalFile();
                ShowSubPanel(0);
                Init();
            }
        }
        return 0;
    }

    private void ChangeRobotLeftArm(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeLeftArm");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIChangeLeftArm, data, callbackFunction);
    }

    private int callbackChangeRobotLeftArm(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                CurrentRobot.larm_object_id = int.Parse(json["result"]["larm_id"].ToString());
                ShowSubPanel(0);
                Init();
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                GameManage.User.UpdateLocalFile();
                ShowSubPanel(0);
                Init();
            }
        }
        return 0;
    }

    private void ChangeRobotRightArm(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIChangeRightArm, data, callbackFunction);
    }

    private int callbackChangeRobotRightArm(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                CurrentRobot.rarm_object_id = int.Parse(json["result"]["rarm_id"].ToString());
                ShowSubPanel(0);
                Init();
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                GameManage.User.UpdateLocalFile();
                ShowSubPanel(0);
                Init();
            }
        }
        return 0;
    }

    private void ChangeRobotLeg(int user_id, Func<WWW, int> callbackFunction = null)
    {
        Debug.Log("ChangeHead");
        Hashtable data = new Hashtable();
        data.Add("user_id", user_id);

        GameManage.Get("/" + GB.g_APIChangeLegs, data, callbackFunction);
    }

    private int callbackChangeRobotLeg(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                CurrentRobot.leg_object_id = int.Parse(json["result"]["leg_id"].ToString());
                ShowSubPanel(0);
                Init();
            }
            else if (status == "fail")
            {
                //Debug.LogError(status);
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            if (!MB_Configs.bDummyData)
            {
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                //LoadingPanel.instance.taskCompleted++;
                GameManage.User.UpdateLocalFile();
                ShowSubPanel(0);
                Init();
            }
        }
        return 0;
    }
    #endregion

    public void AddHead(Head head)
    {
        GameManage.User.Heads.Add(head);
        GameManage.User.UpdateLocalFile();
    }

    public void AddLeftArm(LeftArm larm)
    {
        GameManage.User.LeftArms.Add(larm);
        GameManage.User.UpdateLocalFile();
    }

    public void AddRightArm(RightArm rarm)
    {
        GameManage.User.RightArms.Add(rarm);
        GameManage.User.UpdateLocalFile();
    }

    public void AddLeg(Leg leg)
    {
        GameManage.User.Legs.Add(leg);
        GameManage.User.UpdateLocalFile();
    }

    public void AddRobot(Head head, LeftArm larm, RightArm rarm, Leg leg)
    {
        Debug.LogError("Adding robot");
        AddHead(head);
        AddLeftArm(larm);
        AddRightArm(rarm);
        AddLeg(leg);

        Robot newRobot = new Robot();
        GameManage.User.Robots.Add(newRobot);

        newRobot.object_id = GameManage.User.Robots.Count - 1;
        newRobot.head_object_id = head.object_id;
        newRobot.larm_object_id = larm.object_id;
        newRobot.rarm_object_id = rarm.object_id;
        newRobot.leg_object_id = leg.object_id;

        GameManage.User.UpdateLocalFile();
    }

    public void CloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        ShowSubPanel(-1);

        MenuUIManager.instance.ShowPanel(0);
    }

    public void SubCloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(0);
    }

    public void PartsBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void RefreshButton()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        GameManage.instance.GetUserInfo(GB.g_MyID, callbackUserInfo);
    }

    public void SellToStoreBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(1);
        SubPanelList[1].GetComponent<SellSubPanel>().ShowItemsData(CurrentRobot);
    }

    public void StoreBtn()
    {
        //FindObjectOfType<OptionManager>().ButtonPlay ();
        //ShowSubPanel (2);

        FindObjectOfType<OptionManager>().ButtonPlay();

        ShowSubPanel(-1);

        MenuUIManager.instance.ShowPanel(3);
    }

    public void LevelUpBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(3);
    }

    public void MedicamentBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(4);
    }

    public void HeadBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(5);
        SubPanelList[5].GetComponent<MyInventorySubPanel>().ShowHeads(CurrentRobot, GameManage.User.Heads);
    }

    public void LArmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(5);
        SubPanelList[5].GetComponent<MyInventorySubPanel>().ShowLeftArms(CurrentRobot, GameManage.User.LeftArms);
    }

    public void RArmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(5);
        SubPanelList[5].GetComponent<MyInventorySubPanel>().ShowRightArms(CurrentRobot, GameManage.User.RightArms);
    }

    public void LegBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(5);
        SubPanelList[5].GetComponent<MyInventorySubPanel>().ShowLegs(CurrentRobot, GameManage.User.Legs);
    }

    public void ShowSubPanel(int iIndex)
    {
        GameObject Panel2 = GameObject.Find("UI2").transform.Find("08-InventoryPanel2").gameObject;
        GameObject Robot = GameObject.Find("Models").transform.Find("Inventory").gameObject;

        if (iIndex == 0)
        {
            Panel2.SetActive(true);
            Robot.SetActive(true);
        }
        else
        {
            Panel2.SetActive(false);
            Robot.SetActive(false);
        }

        for (int i = 1; i < SubPanelList.Count; i++)
        {
            if (i == iIndex)
                SubPanelList[i].SetActive(true);
            else
                SubPanelList[i].SetActive(false);
        }
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
                Debug.Log("ChangeRobot: Done");
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
                Init();
                ShowSubPanel(0);
            }
            else if (status == "fail")
            {
                Debug.Log("ChangeRobot: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("ChangeRobot: Error");
            Debug.Log(www.text);
            MobileNative.Alert("Error", "Username or password is wrong.", "OK");
        }
        return 0;
    }
}
