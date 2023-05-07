using LitJson;
using NiobiumStudios;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Daily : MonoBehaviour
{

    public List<Toggle> TabList = new List<Toggle>();
    public List<GameObject> ObjList = new List<GameObject>();

    public GameObject robotItemPrefab, pieceItemPrefab, dailyRewardPrefab;
    public Transform robotParent, pieceParent, gemParent;

    private List<GameObject> robotList = new List<GameObject>(), pieceList = new List<GameObject>();
    public List<GameObject> gemObjectList;

    private DailyRewards dailyRewards;
    private List<DailyRewardUI> dailyRewardsUI = new List<DailyRewardUI>();
    private bool readyToClaim;
    public Text textReward;
    public Text textTimeDue;
    public ScrollRect scrollRect;

    public Text txtBoughtRobot, txtBoughtPiece, txtNothingRobot, txtNothingPiece;


    [Range(0, 1)] public float scroll;

    private void Awake()
    {
        dailyRewards = GetComponent<DailyRewards>();
    }

    // Use this for initialization
    void Start()
    {
        SelectTabInternal();
    }

    void OnEnable()
    {
        dailyRewards.onClaimPrize += OnClaimPrize;
        //dailyRewards.onInitialize += OnInitialize;

        Initialize();
    }

    void OnDisable()
    {
        if (dailyRewards != null)
        {
            dailyRewards.onClaimPrize -= OnClaimPrize;
            //dailyRewards.onInitialize -= OnInitialize;
        }
    }

    private void Initialize()
    {
        DisableRobotBoughtText();
        DisablePieceBoughtText();
        textReward.gameObject.SetActive(false);
        //Create robot items
        CreateRobotItems();
        //Create piece items
        CreatePieceItems();
        //Create gems items
        //CreateGemItems();

        SelectTabInternal();
    }

    void CreateRobotItems()
    {
        ClearRobotList();

        robotList = new List<GameObject>();
        if (GameManage.PromotionRobots.Count == 0)
        {
            txtNothingRobot.gameObject.SetActive(true);
        }
        else
        {
            txtNothingRobot.gameObject.SetActive(false);

            for (int i = 0; i < GameManage.PromotionRobots.Count; i++)
            {
                GameObject go = Instantiate(robotItemPrefab, robotParent);
                PromotionRobotItemUI comp = go.GetComponent<PromotionRobotItemUI>();
                comp.Init(GameManage.PromotionRobots[i]);
                robotList.Add(go);
            }
        }
    }

    void CreatePieceItems()
    {
        ClearPieceList();

        pieceList = new List<GameObject>();
        if (GameManage.PromotionPieces.Count == 0)
        {
            txtNothingPiece.gameObject.SetActive(true);
        }
        else
        {
            txtNothingPiece.gameObject.SetActive(false);

            for (int i = 0; i < GameManage.PromotionPieces.Count; i++)
            {
                GameObject go = Instantiate(pieceItemPrefab, pieceParent);
                PromotionPieceItemUI comp = go.GetComponent<PromotionPieceItemUI>();
                comp.Init(GameManage.PromotionPieces[i]);
                robotList.Add(go);
            }
        }
    }

    void CreateGemItems()
    {
        for (int i = 0; i < dailyRewards.rewards.Count; i++)
        {
            int day = i + 1;
            var reward = dailyRewards.GetReward(day);

            GameObject dailyRewardGo = GameObject.Instantiate(dailyRewardPrefab, gemParent) as GameObject;

            DailyRewardUI dailyRewardUI = dailyRewardGo.GetComponent<DailyRewardUI>();
            //dailyRewardUI.transform.SetParent(gemParent);
            //dailyRewardGo.transform.localScale = Vector2.one;

            dailyRewardUI.daily = this;
            dailyRewardUI.day = day;
            dailyRewardUI.reward = reward;
            dailyRewardUI.Initialize();

            dailyRewardsUI.Add(dailyRewardUI);
        }
    }

    private void ClearRobotList()
    {
        for (int i = 0; i < robotList.Count; i++)
        {
            Destroy(robotList[i].gameObject);
        }
    }

    private void ClearPieceList()
    {
        for (int i = 0; i < pieceList.Count; i++)
        {
            Destroy(pieceList[i].gameObject);
        }
    }

    public void OnClaim()
    {
        dailyRewards.ClaimPrize();
        readyToClaim = false;
        UpdateUI();
    }

    public void ShowRobotBoughtText(string text)
    {
        txtBoughtRobot.gameObject.SetActive(true);
        txtBoughtRobot.text = text;
        Invoke("DisableRobotBoughtText", 3f);
    }

    void DisableRobotBoughtText()
    {
        txtBoughtRobot.gameObject.SetActive(false);
        txtBoughtRobot.text = "";
    }

    public void ShowPieceBoughtText(string text)
    {
        txtBoughtPiece.gameObject.SetActive(true);
        txtBoughtPiece.text = text;
        Invoke("DisablePieceBoughtText", 3f);
    }

    void DisablePieceBoughtText()
    {
        txtBoughtPiece.gameObject.SetActive(false);
        txtBoughtPiece.text = "";
    }

    public void UpdateUI()
    {
        dailyRewards.CheckRewards();

        bool isRewardAvailableNow = false;

        var lastReward = dailyRewards.lastReward;
        var availableReward = dailyRewards.availableReward;

        foreach (var dailyRewardUI in dailyRewardsUI)
        {
            var day = dailyRewardUI.day;

            if (day == availableReward)
            {
                dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_AVAILABLE;

                isRewardAvailableNow = true;
            }
            else if (day <= lastReward)
            {
                dailyRewardUI.state = DailyRewardUI.DailyRewardState.CLAIMED;
            }
            else
            {
                dailyRewardUI.state = DailyRewardUI.DailyRewardState.UNCLAIMED_UNAVAILABLE;
            }

            dailyRewardUI.Refresh();
        }

        //buttonClaim.gameObject.SetActive(isRewardAvailableNow);
        //buttonClose.gameObject.SetActive(!isRewardAvailableNow);
        if (isRewardAvailableNow)
        {
            //SnapToReward();
            textTimeDue.text = "Claim your reward!";
        }
        readyToClaim = isRewardAvailableNow;
    }

    public void SnapToReward()
    {
        Canvas.ForceUpdateCanvases();

        var lastRewardIdx = dailyRewards.lastReward;
        //lastRewardIdx = 4;
        // Scrolls to the last reward element
        //if (dailyRewardsUI.Count - 1 > lastRewardIdx)
        //    lastRewardIdx++;

        //if (lastRewardIdx > dailyRewardsUI.Count - 1)
        //    lastRewardIdx = dailyRewardsUI.Count - 1;
        //Debug.LogError(lastRewardIdx);
        if (lastRewardIdx > 3 && lastRewardIdx < 7)
        {
            scrollRect.horizontalNormalizedPosition = 0.25f + (lastRewardIdx - 3) * 0.25f;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = 0;

            //scrollRect.horizontalNormalizedPosition = 0.25f + (lastRewardIdx - 3) * 0.25f;
        }
        //Debug.LogError(scrollRect.horizontalNormalizedPosition);

        //var target = dailyRewardsUI[lastRewardIdx].GetComponent<RectTransform>();

        //var content = scrollRect.content;

        ////content.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(content.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

        //float normalizePosition = (float)target.GetSiblingIndex() / (float)content.transform.childCount;
        //scrollRect.horizontalNormalizedPosition = normalizePosition;

        //Debug.LogError(scrollRect.horizontalNormalizedPosition);
    }

    int rewardValue = 0;
    private void OnClaimPrize(int day)
    {
        //panelReward.SetActive(true);

        var reward = dailyRewards.GetReward(day);
        var unit = reward.unit;
        var rewardQt = reward.reward;
        rewardValue = rewardQt;
        //imageReward.sprite = reward.sprite;
        //StartCoroutine(UpdateGems(rewardQt));
        GameManage.instance.UpdateUserGems(GB.g_MyID, rewardValue, callbackUserInfo);
    }

    public void OnInitialize()
    {

        CreateGemItems();
        //if (!error)
        //{
        var showWhenNotAvailable = dailyRewards.keepOpen;
        var isRewardAvailable = dailyRewards.availableReward > 0;

        UpdateUI();
        if (isRewardAvailable)
        {
            TabList[2].isOn = true;
        }
        else
        {
            TabList[0].isOn = true;
        }
        //gameObject.SetActive(showWhenNotAvailable || (!showWhenNotAvailable && isRewardAvailable));
        gameObject.SetActive(true);
        //SnapToReward();
        CheckTimeDifference();
        //}
    }

    void Update()
    {
        //scrollRect.horizontalNormalizedPosition = scroll;
        //PlayerPrefs.DeleteAll();
        if (!gameObject.activeSelf)
        {
            return;
        }
        dailyRewards.TickTime();
        // Updates the time due
        CheckTimeDifference();
    }

    private void CheckTimeDifference()
    {
        if (!readyToClaim)
        {
            TimeSpan difference = dailyRewards.GetTimeDifference();

            // If the counter below 0 it means there is a new reward to claim
            if (difference.TotalSeconds <= 0)
            {
                readyToClaim = true;
                UpdateUI();
                //SnapToReward();
                return;
            }

            string formattedTs = dailyRewards.GetFormattedTime(difference);

            textTimeDue.text = string.Format("Come back in {0} for your next reward", formattedTs);
        }
    }

    public void CloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        gameObject.SetActive(false);
        MenuUIManager.instance.ShowPanel(0);
    }

    public void SelectTab()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        SelectTabInternal();
    }

    private void SelectTabInternal()
    {
        for (int i = 0; i < TabList.Count; i++)
        {
            ObjList[i].SetActive(TabList[i].isOn);
            if (i == 2)
            {
                UpdateUI();
            }
        }
    }

    #region API Callsbacks
    private int callbackUserInfo(WWW www)
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
                if (rewardValue > 0)
                {
                    textReward.text = string.Format("You got {0} gems!", rewardValue);
                    textReward.gameObject.SetActive(true);
                }
                else
                {
                    textReward.text = string.Format("You got nothing!");
                    textReward.gameObject.SetActive(true);
                }
                StartCoroutine(DeactivateMessageText());
                //textReward.gameObject.SetActive(false);
                //StoreManager.instance.DailyPanel.SetActive(false);
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


    IEnumerator DeactivateMessageText()
    {
        yield return new WaitForSeconds(2);
        textReward.gameObject.SetActive(false);
    }
}
