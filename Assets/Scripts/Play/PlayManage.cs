using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Shared;
using LitJson;
using System;
using Random = UnityEngine.Random;

public enum GameState
{
    WaitingForPlayers,
    InitialCountDown,
    Racing,
    SelectingAttackOrder,
    Attacking
}

public class PlayManage : MonoBehaviour
{
    public List<Vector3> MineIdlePosList = new List<Vector3>();
    public List<Vector3> MineAttackPosList = new List<Vector3>();
    public List<Vector3> EnemyIdlePosList = new List<Vector3>();
    public List<Vector3> EnemyAttackPosList = new List<Vector3>();

    public List<GameObject> MineRobots = new List<GameObject>();
    public List<GameObject> EnemyRobots = new List<GameObject>();

    public List<GameObject> MineActiveRobots = new List<GameObject>();
    public List<GameObject> EnemyActiveRobots = new List<GameObject>();

    public GameObject MineAttackObj;
    public GameObject EnemyAttackObj;

    public List<string> RobotNameList = new List<string>();

    public List<GameObject> AttackMineList = new List<GameObject>();
    public List<GameObject> PartMineList = new List<GameObject>();

    public List<GameObject> AttackEnemyList = new List<GameObject>();
    public List<GameObject> PartEnemyList = new List<GameObject>();


    public DB dbData = new DB();

    bool bMineAttack;
    bool bMineReturn;

    float fDistance;

    NavMeshAgent _agentMine;

    int iType;

    int iSMineIndex;
    int iEMineIndex;


    List<int> m_iRobotList = new List<int>();
    List<int> m_iHeadList = new List<int>();
    List<int> m_iLArmList = new List<int>();
    List<int> m_iRArmList = new List<int>();
    List<int> m_iLegList = new List<int>();

    List<int> e_iRobotList = new List<int>();
    List<int> e_iHeadList = new List<int>();
    List<int> e_iLArmList = new List<int>();
    List<int> e_iRArmList = new List<int>();
    List<int> e_iLegList = new List<int>();

    public PlayCS playcs;

    public bool isMultiplayer = false;

    // Use this for initialization
    void Start()
    {
        //Advertisement.Instance.HideBannerAd();
        playcs = FindObjectOfType<PlayCS>();
        //SaveInfo();

        Init();
        //Init robot controllers
        InitRobotControllers();


        if (isMultiplayer)
        {
            return;
        }

        MakeMineRobots();
        MakeEnemyRobots();
        //Start match round
        StartMatchRound();
    }

    #region HASIB
    public static PlayManage instance;
    private void Awake()
    {
        instance = this;
    }

    public int matchTimeInSec = 60 * 5;

    public int currentMatchTime = 0;
    public List<RobotController> allRobotControllers = new List<RobotController>();
    public List<RobotController> attackRobotOrder = new List<RobotController>();

    public bool isMineSelected = false;
    public bool isEnemySelected = false;
    public int mineSelectedId = -1;
    public int enemySelectedId = -1;

    public int mineAttackCount = 0;
    public int mineDefenceCount = 0;

    public bool isAllAttackSelected = false;
    public bool isFighting = false;
    public bool isGameOver = false;

    int currentAttackRobotid = 0;
    List<RewardBluePrint> rewardsBluePrint = new List<RewardBluePrint>();

    public GameState currentGameState = GameState.WaitingForPlayers;
    public int attackSelectionCount = 0;

    public void InitRobotControllers()
    {
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            RobotController robot = MineActiveRobots[i].GetComponent<RobotController>();
            allRobotControllers.Add(robot);
            allRobotControllers[i].type = RobotType.MINE;
            allRobotControllers[i].id = i;
            allRobotControllers[i].Deselect();
        }

        for (int i = 0; i < EnemyActiveRobots.Count; i++)
        {
            RobotController robot = EnemyRobots[i].GetComponent<RobotController>();
            allRobotControllers.Add(robot);
            allRobotControllers[i + MineActiveRobots.Count].type = RobotType.ENEMY;
            allRobotControllers[i + MineActiveRobots.Count].id = i + MineActiveRobots.Count;
            allRobotControllers[i].Deselect();
        }
    }

    public void StartMatchRound()
    {
        Debug.Log("Starting match round");
        currentGameState = GameState.InitialCountDown;
        //Show Start Timer
        StartTimer();
        if (isMultiplayer)
        {
            OptionManager.instance.PlayMultiPlayerBG();
        }
        else
        {
            OptionManager.instance.PlaySinglePlayerBG();
        }
    }

    private void StartTimer()
    {
        StartCoroutine(StartInitialTimerRoutine());
    }

    IEnumerator StartInitialTimerRoutine()
    {
        Debug.Log("Counting initial timer");
        //Show start time ui
        playcs.ShowSubPanel(3);
        playcs.SetInitTimerValue(3);
        yield return new WaitForSeconds(1);
        playcs.SetInitTimerValue(2);
        yield return new WaitForSeconds(1);
        playcs.SetInitTimerValue(1);
        yield return new WaitForSeconds(1);
        playcs.SetInitTimerValue(0);
        yield return new WaitForSeconds(1);
        StartMatch();
    }

    private void StartMatch()
    {
        Debug.Log("Starting match");
        //Start match timer
        StartMatchTimer();
        //Determine attack order
        DetermineAttackOrder();
    }

    private void StartMatchTimer()
    {
        StartCoroutine(MatchTimerRoutine());
    }

    IEnumerator MatchTimerRoutine()
    {
        Debug.Log("Starting match timer");
        playcs.ShowSubPanel(-1);
        playcs.ShowMatchTimer();
        playcs.SetMatchTimerValue(matchTimeInSec);

        currentMatchTime = matchTimeInSec;

        while (currentMatchTime > 0 && isGameOver == false)
        {
            yield return new WaitForSeconds(1);
            currentMatchTime -= 1;
            playcs.SetMatchTimerValue(currentMatchTime);
        }

        if (isGameOver != true)
        {
            //Stop round
            isGameOver = true;
            //GameManage.instance.UpdateUserLevel(1);
            if (IsPlayerWinner())
            {
                ShowVictoryPanel(matchTimeInSec, mineAttackCount, mineDefenceCount);
            }
            else
            {
                ShowLoosePanel();
            }

            StopAllCoroutines();
        }
    }

    void ShowVictoryPanel(int time, int attackCount, int defenceCount)
    {
        //Advertisement.Instance.ShowInterstitialAd();
        //Reward player
        //if (!isMultiplayer)
        //{
        RewardPlayer();
        //}

        if (isMultiplayer)
        {
            playcs.ShowSubPanel(5);
            playcs.SubPanelList[5].GetComponent<VictoryPanel>().Init(time, attackCount, defenceCount, rewardsBluePrint);
        }
    }

    public List<Robot> MineDestroyedRobots = new List<Robot>();
    public List<Robot> EnemyDestroyedRobots = new List<Robot>();
    void RewardPlayer()
    {
        if (!isMultiplayer)
        {
            rewardsBluePrint = new List<RewardBluePrint>();


            int curLev = PlayerPrefs.GetInt("CurLev", 0);
            UserLevel currentLevel = GameManage.UserLevels.Find(x => x.battle_id == curLev);

            GameManage.instance.UpdateUserLevel(currentLevel.battle_id);
            GameManage.instance.RewardAdventure(currentLevel.battle_id, callbackRewardAdventure);


            if (curLev < GameManage.UserLevels.Count - 1)
            {
                int nextLev = curLev + 1;
                UserLevel nextLevel = GameManage.UserLevels.Find(x => x.battle_id == nextLev);
                nextLevel.locked = 0;
            }
            #region Previous RewardSystem
            //if player wins a lot this level
            //if (currentLevel.win_count > 4)
            //{
            //    LevelReward rewardData = currentLevel.reward2;

            //    GameManage.User.gems += rewardData.gems;

            //    if (rewardData.reward_type == 0) // Check and reward if reward type is robot n.b: 0 = Robot, 1 = piece
            //    {
            //        Robot opponentRobot = allRobotControllers[MineActiveRobots.Count].robot;

            //        Head h = new Head();
            //        h.object_id = GameManage.User.Heads.Count;
            //        h.head_id = opponentRobot.Head.head_id;
            //        h.type = opponentRobot.Head.type;
            //        h.name = opponentRobot.Head.name;
            //        h.level = opponentRobot.Head.level;
            //        h.price = opponentRobot.Head.price;
            //        h.life = opponentRobot.Head.life / 5;
            //        h.attack = opponentRobot.Head.attack / 5;
            //        h.defence = opponentRobot.Head.defence / 5;
            //        h.velocity = opponentRobot.Head.velocity / 5;
            //        h.onSale = opponentRobot.Head.onSale;
            //        h.salePrice = opponentRobot.Head.salePrice;

            //        GameManage.User.Heads.Add(h);

            //        LeftArm la = new LeftArm();
            //        la.object_id = GameManage.User.LeftArms.Count;
            //        la.larm_id = opponentRobot.LeftArm.larm_id;
            //        la.type = opponentRobot.LeftArm.type;
            //        la.name = opponentRobot.LeftArm.name;
            //        la.level = opponentRobot.LeftArm.level;
            //        la.price = opponentRobot.LeftArm.price;
            //        la.life = opponentRobot.LeftArm.life / 5;
            //        la.attack = opponentRobot.LeftArm.attack / 5;
            //        la.defence = opponentRobot.LeftArm.defence / 5;
            //        la.velocity = opponentRobot.LeftArm.velocity / 5;
            //        la.onSale = opponentRobot.LeftArm.onSale;
            //        la.salePrice = opponentRobot.LeftArm.salePrice;

            //        GameManage.User.LeftArms.Add(la);

            //        RightArm ra = new RightArm();
            //        ra.object_id = GameManage.User.RightArms.Count;
            //        ra.rarm_id = opponentRobot.RightArm.rarm_id;
            //        ra.type = opponentRobot.RightArm.type;
            //        ra.name = opponentRobot.RightArm.name;
            //        ra.level = opponentRobot.RightArm.level;
            //        ra.price = opponentRobot.RightArm.price;
            //        ra.life = opponentRobot.RightArm.life / 5;
            //        ra.attack = opponentRobot.RightArm.attack / 5;
            //        ra.defence = opponentRobot.RightArm.defence / 5;
            //        ra.velocity = opponentRobot.RightArm.velocity / 5;
            //        ra.onSale = opponentRobot.RightArm.onSale;
            //        ra.salePrice = opponentRobot.RightArm.salePrice;

            //        GameManage.User.RightArms.Add(ra);

            //        Leg l = new Leg();
            //        l.object_id = GameManage.User.Legs.Count;
            //        l.leg_id = opponentRobot.Leg.leg_id;
            //        l.type = opponentRobot.Leg.type;
            //        l.name = opponentRobot.Leg.name;
            //        l.level = opponentRobot.Leg.level;
            //        l.price = opponentRobot.Leg.price;
            //        l.life = opponentRobot.Leg.life / 5;
            //        l.attack = opponentRobot.Leg.attack / 5;
            //        l.defence = opponentRobot.Leg.defence / 5;
            //        l.velocity = opponentRobot.Leg.velocity / 5;
            //        l.onSale = opponentRobot.Leg.onSale;
            //        l.salePrice = opponentRobot.Leg.salePrice;

            //        GameManage.User.Legs.Add(l);

            //        Robot newRobot = new Robot();
            //        newRobot.object_id = GameManage.User.Robots.Count;
            //        newRobot.type = opponentRobot.type;
            //        newRobot.name = opponentRobot.name;
            //        newRobot.price = opponentRobot.price;
            //        newRobot.head_object_id = h.object_id;
            //        newRobot.larm_object_id = la.object_id;
            //        newRobot.rarm_object_id = ra.object_id;
            //        newRobot.leg_object_id = l.object_id;
            //        newRobot.onSale = opponentRobot.onSale;
            //        newRobot.salePrice = opponentRobot.salePrice;

            //        GameManage.User.Robots.Add(newRobot);

            //        RewardBluePrint rbp4 = new RewardBluePrint();
            //        rbp4.name = newRobot.name;
            //        rbp4.rewardType = "Robot";
            //        rbp4.rewardLevel = newRobot.Level;

            //        rewardsBluePrint.Add(rbp4);

            //        //GameManage.User.gems += rewardData.gems;
            //        //GameManage.User.med
            //    }
            //    else
            //    {
            //        //
            //        Robot robot = allRobotControllers[Random.Range(MineActiveRobots.Count, allRobotControllers.Count)].robot;

            //        for (int i = 0; i < rewardData.reward_count; i++)
            //        {
            //            int pieceId = Random.Range(0, 4);
            //            switch (pieceId)
            //            {
            //                case 0:
            //                    Head h = new Head();
            //                    h.object_id = GameManage.User.Heads.Count;
            //                    h.head_id = robot.Head.head_id;
            //                    h.type = robot.Head.type;
            //                    h.name = robot.Head.name;
            //                    h.level = robot.Head.level;
            //                    h.price = robot.Head.price;
            //                    h.life = robot.Head.life / 5;
            //                    h.attack = robot.Head.attack / 5;
            //                    h.defence = robot.Head.defence / 5;
            //                    h.velocity = robot.Head.velocity / 5;
            //                    h.onSale = robot.Head.onSale;
            //                    h.salePrice = robot.Head.salePrice;

            //                    GameManage.User.Heads.Add(h);

            //                    RewardBluePrint rbp1 = new RewardBluePrint();
            //                    rbp1.name = h.name;
            //                    rbp1.rewardType = "Head";
            //                    rbp1.rewardLevel = h.level;

            //                    rewardsBluePrint.Add(rbp1);
            //                    break;
            //                case 1:
            //                    LeftArm la = new LeftArm();
            //                    la.object_id = GameManage.User.LeftArms.Count;
            //                    la.larm_id = robot.LeftArm.larm_id;
            //                    la.type = robot.LeftArm.type;
            //                    la.name = robot.LeftArm.name;
            //                    la.level = robot.LeftArm.level;
            //                    la.price = robot.LeftArm.price;
            //                    la.life = robot.LeftArm.life / 5;
            //                    la.attack = robot.LeftArm.attack / 5;
            //                    la.defence = robot.LeftArm.defence / 5;
            //                    la.velocity = robot.LeftArm.velocity / 5;
            //                    la.onSale = robot.LeftArm.onSale;
            //                    la.salePrice = robot.LeftArm.salePrice;

            //                    GameManage.User.LeftArms.Add(la);

            //                    RewardBluePrint rbp2 = new RewardBluePrint();
            //                    rbp2.name = la.name;
            //                    rbp2.rewardType = "Left Arm";
            //                    rbp2.rewardLevel = la.level;

            //                    rewardsBluePrint.Add(rbp2);
            //                    break;
            //                case 2:
            //                    RightArm ra = new RightArm();
            //                    ra.object_id = GameManage.User.RightArms.Count;
            //                    ra.rarm_id = robot.RightArm.rarm_id;
            //                    ra.type = robot.RightArm.type;
            //                    ra.name = robot.RightArm.name;
            //                    ra.level = robot.RightArm.level;
            //                    ra.price = robot.RightArm.price;
            //                    ra.life = robot.RightArm.life / 5;
            //                    ra.attack = robot.RightArm.attack / 5;
            //                    ra.defence = robot.RightArm.defence / 5;
            //                    ra.velocity = robot.RightArm.velocity / 5;
            //                    ra.onSale = robot.RightArm.onSale;
            //                    ra.salePrice = robot.RightArm.salePrice;

            //                    GameManage.User.RightArms.Add(ra);

            //                    RewardBluePrint rbp3 = new RewardBluePrint();
            //                    rbp3.name = ra.name;
            //                    rbp3.rewardType = "Right Arm";
            //                    rbp3.rewardLevel = ra.level;

            //                    rewardsBluePrint.Add(rbp3);

            //                    break;
            //                case 3:
            //                    Leg l = new Leg();
            //                    l.object_id = GameManage.User.Legs.Count;
            //                    l.leg_id = robot.Leg.leg_id;
            //                    l.type = robot.Leg.type;
            //                    l.name = robot.Leg.name;
            //                    l.level = robot.Leg.level;
            //                    l.price = robot.Leg.price;
            //                    l.life = robot.Leg.life / 5;
            //                    l.attack = robot.Leg.attack / 5;
            //                    l.defence = robot.Leg.defence / 5;
            //                    l.velocity = robot.Leg.velocity / 5;
            //                    l.onSale = robot.Leg.onSale;
            //                    l.salePrice = robot.Leg.salePrice;

            //                    GameManage.User.Legs.Add(l);

            //                    RewardBluePrint rbp4 = new RewardBluePrint();
            //                    rbp4.name = l.name;
            //                    rbp4.rewardType = "Leg";
            //                    rbp4.rewardLevel = l.level;

            //                    rewardsBluePrint.Add(rbp4);

            //                    break;
            //            }
            //        }
            //        //GameManage.User.UpdateLocalFile();
            //    }
            //}
            //else // if player win it few times or for first time
            //{
            //    LevelReward rewardData = currentLevel.reward1;

            //    GameManage.User.gems += rewardData.gems;

            //    if (rewardData.reward_type == 0) // Check and reward if reward type is robot n.b: 0 = Robot, 1 = piece
            //    {
            //        Robot opponentRobot = allRobotControllers[MineActiveRobots.Count].robot;

            //        Head h = new Head();
            //        h.object_id = GameManage.User.Heads.Count;
            //        h.head_id = opponentRobot.Head.head_id;
            //        h.type = opponentRobot.Head.type;
            //        h.name = opponentRobot.Head.name;
            //        h.level = opponentRobot.Head.level;
            //        h.price = opponentRobot.Head.price;
            //        h.life = opponentRobot.Head.life / 5;
            //        h.attack = opponentRobot.Head.attack / 5;
            //        h.defence = opponentRobot.Head.defence / 5;
            //        h.velocity = opponentRobot.Head.velocity / 5;
            //        h.onSale = opponentRobot.Head.onSale;
            //        h.salePrice = opponentRobot.Head.salePrice;

            //        GameManage.User.Heads.Add(h);

            //        LeftArm la = new LeftArm();
            //        la.object_id = GameManage.User.LeftArms.Count;
            //        la.larm_id = opponentRobot.LeftArm.larm_id;
            //        la.type = opponentRobot.LeftArm.type;
            //        la.name = opponentRobot.LeftArm.name;
            //        la.level = opponentRobot.LeftArm.level;
            //        la.price = opponentRobot.LeftArm.price;
            //        la.life = opponentRobot.LeftArm.life / 5;
            //        la.attack = opponentRobot.LeftArm.attack / 5;
            //        la.defence = opponentRobot.LeftArm.defence / 5;
            //        la.velocity = opponentRobot.LeftArm.velocity / 5;
            //        la.onSale = opponentRobot.LeftArm.onSale;
            //        la.salePrice = opponentRobot.LeftArm.salePrice;

            //        GameManage.User.LeftArms.Add(la);

            //        RightArm ra = new RightArm();
            //        ra.object_id = GameManage.User.RightArms.Count;
            //        ra.rarm_id = opponentRobot.RightArm.rarm_id;
            //        ra.type = opponentRobot.RightArm.type;
            //        ra.name = opponentRobot.RightArm.name;
            //        ra.level = opponentRobot.RightArm.level;
            //        ra.price = opponentRobot.RightArm.price;
            //        ra.life = opponentRobot.RightArm.life / 5;
            //        ra.attack = opponentRobot.RightArm.attack / 5;
            //        ra.defence = opponentRobot.RightArm.defence / 5;
            //        ra.velocity = opponentRobot.RightArm.velocity / 5;
            //        ra.onSale = opponentRobot.RightArm.onSale;
            //        ra.salePrice = opponentRobot.RightArm.salePrice;

            //        GameManage.User.RightArms.Add(ra);

            //        Leg l = new Leg();
            //        l.object_id = GameManage.User.Legs.Count;
            //        l.leg_id = opponentRobot.Leg.leg_id;
            //        l.type = opponentRobot.Leg.type;
            //        l.name = opponentRobot.Leg.name;
            //        l.level = opponentRobot.Leg.level;
            //        l.price = opponentRobot.Leg.price;
            //        l.life = opponentRobot.Leg.life / 5;
            //        l.attack = opponentRobot.Leg.attack / 5;
            //        l.defence = opponentRobot.Leg.defence / 5;
            //        l.velocity = opponentRobot.Leg.velocity / 5;
            //        l.onSale = opponentRobot.Leg.onSale;
            //        l.salePrice = opponentRobot.Leg.salePrice;

            //        GameManage.User.Legs.Add(l);

            //        Robot newRobot = new Robot();
            //        newRobot.object_id = GameManage.User.Robots.Count;
            //        newRobot.type = opponentRobot.type;
            //        newRobot.name = opponentRobot.name;
            //        newRobot.price = opponentRobot.price;
            //        newRobot.head_object_id = h.object_id;
            //        newRobot.larm_object_id = la.object_id;
            //        newRobot.rarm_object_id = ra.object_id;
            //        newRobot.leg_object_id = l.object_id;
            //        newRobot.onSale = opponentRobot.onSale;
            //        newRobot.salePrice = opponentRobot.salePrice;

            //        GameManage.User.Robots.Add(newRobot);

            //        RewardBluePrint rbp4 = new RewardBluePrint();
            //        rbp4.name = newRobot.name;
            //        rbp4.rewardType = "Robot";
            //        rbp4.rewardLevel = newRobot.Level;

            //        rewardsBluePrint.Add(rbp4);
            //    }
            //    else
            //    {
            //        //
            //        Robot robot = allRobotControllers[Random.Range(MineActiveRobots.Count, allRobotControllers.Count)].robot;

            //        for (int i = 0; i < rewardData.reward_count; i++)
            //        {
            //            int pieceId = Random.Range(0, 4);
            //            switch (pieceId)
            //            {
            //                case 0:
            //                    Head h = new Head();
            //                    h.object_id = GameManage.User.Heads.Count;
            //                    h.head_id = robot.Head.head_id;
            //                    h.type = robot.Head.type;
            //                    h.name = robot.Head.name;
            //                    h.level = robot.Head.level;
            //                    h.price = robot.Head.price;
            //                    h.life = robot.Head.life / 5;
            //                    h.attack = robot.Head.attack / 5;
            //                    h.defence = robot.Head.defence / 5;
            //                    h.velocity = robot.Head.velocity / 5;
            //                    h.onSale = robot.Head.onSale;
            //                    h.salePrice = robot.Head.salePrice;

            //                    GameManage.User.Heads.Add(h);

            //                    RewardBluePrint rbp1 = new RewardBluePrint();
            //                    rbp1.name = h.name;
            //                    rbp1.rewardType = "Head";
            //                    rbp1.rewardLevel = h.level;

            //                    rewardsBluePrint.Add(rbp1);
            //                    break;
            //                case 1:
            //                    LeftArm la = new LeftArm();
            //                    la.object_id = GameManage.User.LeftArms.Count;
            //                    la.larm_id = robot.LeftArm.larm_id;
            //                    la.type = robot.LeftArm.type;
            //                    la.name = robot.LeftArm.name;
            //                    la.level = robot.LeftArm.level;
            //                    la.price = robot.LeftArm.price;
            //                    la.life = robot.LeftArm.life / 5;
            //                    la.attack = robot.LeftArm.attack / 5;
            //                    la.defence = robot.LeftArm.defence / 5;
            //                    la.velocity = robot.LeftArm.velocity / 5;
            //                    la.onSale = robot.LeftArm.onSale;
            //                    la.salePrice = robot.LeftArm.salePrice;

            //                    GameManage.User.LeftArms.Add(la);

            //                    RewardBluePrint rbp2 = new RewardBluePrint();
            //                    rbp2.name = la.name;
            //                    rbp2.rewardType = "Left Arm";
            //                    rbp2.rewardLevel = la.level;

            //                    rewardsBluePrint.Add(rbp2);
            //                    break;
            //                case 2:
            //                    RightArm ra = new RightArm();
            //                    ra.object_id = GameManage.User.RightArms.Count;
            //                    ra.rarm_id = robot.RightArm.rarm_id;
            //                    ra.type = robot.RightArm.type;
            //                    ra.name = robot.RightArm.name;
            //                    ra.level = robot.RightArm.level;
            //                    ra.price = robot.RightArm.price;
            //                    ra.life = robot.RightArm.life / 5;
            //                    ra.attack = robot.RightArm.attack / 5;
            //                    ra.defence = robot.RightArm.defence / 5;
            //                    ra.velocity = robot.RightArm.velocity / 5;
            //                    ra.onSale = robot.RightArm.onSale;
            //                    ra.salePrice = robot.RightArm.salePrice;

            //                    GameManage.User.RightArms.Add(ra);

            //                    RewardBluePrint rbp3 = new RewardBluePrint();
            //                    rbp3.name = ra.name;
            //                    rbp3.rewardType = "Right Arm";
            //                    rbp3.rewardLevel = ra.level;

            //                    rewardsBluePrint.Add(rbp3);

            //                    break;
            //                case 3:
            //                    Leg l = new Leg();
            //                    l.object_id = GameManage.User.Legs.Count;
            //                    l.leg_id = robot.Leg.leg_id;
            //                    l.type = robot.Leg.type;
            //                    l.name = robot.Leg.name;
            //                    l.level = robot.Leg.level;
            //                    l.price = robot.Leg.price;
            //                    l.life = robot.Leg.life / 5;
            //                    l.attack = robot.Leg.attack / 5;
            //                    l.defence = robot.Leg.defence / 5;
            //                    l.velocity = robot.Leg.velocity / 5;
            //                    l.onSale = robot.Leg.onSale;
            //                    l.salePrice = robot.Leg.salePrice;

            //                    GameManage.User.Legs.Add(l);

            //                    RewardBluePrint rbp4 = new RewardBluePrint();
            //                    rbp4.name = l.name;
            //                    rbp4.rewardType = "Leg";
            //                    rbp4.rewardLevel = l.level;

            //                    rewardsBluePrint.Add(rbp4);

            //                    break;
            //            }
            //        }
            //        //GameManage.User.UpdateLocalFile();
            //    }
            //}
            //GameManage.instance.UpdateLevelLocalFile();
            #endregion
        }
        else
        {
            rewardsBluePrint = new List<RewardBluePrint>();

            if (EnemyDestroyedRobots.Count > 0)
            {
                int index = Random.Range(0, EnemyDestroyedRobots.Count);

                Robot robot = EnemyDestroyedRobots[0];

                GameManage.instance.AddRobot(robot, callbackUserInfo);

                RewardBluePrint rbp1 = new RewardBluePrint();
                rbp1.name = robot.Name;
                rbp1.rewardType = "Robot";
                rbp1.rewardLevel = robot.Level;
                rbp1.itemId = robot.Head.head_id;

                //GameManage.User.Robots.Add(robot);

                rewardsBluePrint.Add(rbp1);
            }
            else
            {
                for (int i = 0; i < EnemyActiveRobots.Count; i++)
                {
                    RobotController robotCon = EnemyActiveRobots[i].GetComponent<RobotController>();
                    if (!EnemyDestroyedRobots.Contains(robotCon.robot))
                    {
                        if (robotCon.isLeftArmDestroyed)
                        {
                            GameManage.instance.AddLeftArm(robotCon.robot.LeftArm, callbackUserInfo);

                            RewardBluePrint rbp1 = new RewardBluePrint();
                            rbp1.name = robotCon.robot.LeftArm.name;
                            rbp1.rewardType = "Left Arm";
                            rbp1.rewardLevel = robotCon.robot.LeftArm.level;
                            rbp1.itemId = robotCon.robot.LeftArm.larm_id;

                            //GameManage.User.LeftArms.Add(robotCon.robot.LeftArm);

                            rewardsBluePrint.Add(rbp1);
                            break;
                        }
                        else if (robotCon.isRightArmDestroyed)
                        {
                            GameManage.instance.AddRightArm(robotCon.robot.RightArm, callbackUserInfo);

                            RewardBluePrint rbp1 = new RewardBluePrint();
                            rbp1.name = robotCon.robot.RightArm.name;
                            rbp1.rewardType = "Right Arm";
                            rbp1.rewardLevel = robotCon.robot.RightArm.level;
                            rbp1.itemId = robotCon.robot.RightArm.rarm_id;

                            //GameManage.User.RightArms.Add(robotCon.robot.RightArm);

                            rewardsBluePrint.Add(rbp1);
                            break;
                        }
                        else if (robotCon.isLegDestroyed)
                        {
                            GameManage.instance.AddLeg(robotCon.robot.Leg, callbackUserInfo);

                            RewardBluePrint rbp1 = new RewardBluePrint();
                            rbp1.name = robotCon.robot.RightArm.name;
                            rbp1.rewardType = "Leg";
                            rbp1.rewardLevel = robotCon.robot.Leg.level;
                            rbp1.itemId = robotCon.robot.Leg.leg_id;
                            //GameManage.User.Legs.Add(robotCon.robot.Leg);

                            rewardsBluePrint.Add(rbp1);
                            break;
                        }
                    }
                }
            }

            //GameManage.User.UpdateLocalFile();
        }

        //Upgrade current robots
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            RobotController robotCon = allRobotControllers[i];

            if (!robotCon.isHeadDestroyed)
            {
                Head basicHead = GameManage.AllHeads.Find(x => x.head_id == allRobotControllers[i].robot.Head.head_id);

                if (robotCon.robot.Head.level < 5)
                {
                    robotCon.robot.Head.life += (basicHead.life / 5) / 5;
                    robotCon.robot.Head.attack += (basicHead.attack / 5) / 5;
                    robotCon.robot.Head.defence += (basicHead.defence / 5) / 5;

                    robotCon.robot.Head.level = (int)((basicHead.life + basicHead.attack + basicHead.defence + basicHead.velocity) / (allRobotControllers[i].robot.Head.life + allRobotControllers[i].robot.Head.attack + allRobotControllers[i].robot.Head.defence + allRobotControllers[i].robot.Head.velocity));
                    GameManage.instance.ChangeHead(GB.g_MyID, robotCon.robot.Head, callbackUserInfo);
                }
            }
            else
            {
                robotCon.robot.Head.is_broken = 1;
                robotCon.robot.Head.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeHead(GB.g_MyID, robotCon.robot.Head, callbackUserInfo);
            }

            if (!robotCon.isLeftArmDestroyed)
            {
                LeftArm basicLeftArm = GameManage.AllLeftArms.Find(x => x.larm_id == allRobotControllers[i].robot.LeftArm.larm_id);

                if (robotCon.robot.LeftArm.level < 5)
                {
                    robotCon.robot.LeftArm.life += (basicLeftArm.life / 5) / 5;
                    robotCon.robot.LeftArm.attack += (basicLeftArm.attack / 5) / 5;
                    robotCon.robot.LeftArm.defence += (basicLeftArm.defence / 5) / 5;

                    robotCon.robot.LeftArm.level = (int)((basicLeftArm.life + basicLeftArm.attack + basicLeftArm.defence + basicLeftArm.velocity) / (allRobotControllers[i].robot.LeftArm.life + allRobotControllers[i].robot.LeftArm.attack + allRobotControllers[i].robot.LeftArm.defence + allRobotControllers[i].robot.LeftArm.velocity));
                    GameManage.instance.ChangeLeftArm(GB.g_MyID, robotCon.robot.LeftArm, callbackUserInfo);
                }
            }
            else
            {
                robotCon.robot.LeftArm.is_broken = 1;
                robotCon.robot.LeftArm.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeLeftArm(GB.g_MyID, robotCon.robot.LeftArm, callbackUserInfo);
            }

            if (!robotCon.isRightArmDestroyed)
            {
                RightArm basicRightArm = GameManage.AllRightArms.Find(x => x.rarm_id == allRobotControllers[i].robot.RightArm.rarm_id);

                if (robotCon.robot.RightArm.level < 5)
                {
                    robotCon.robot.RightArm.life += (basicRightArm.life / 5) / 5;
                    robotCon.robot.RightArm.attack += (basicRightArm.attack / 5) / 5;
                    robotCon.robot.RightArm.defence += (basicRightArm.defence / 5) / 5;

                    robotCon.robot.RightArm.level = (int)((basicRightArm.life + basicRightArm.attack + basicRightArm.defence + basicRightArm.velocity) / (allRobotControllers[i].robot.RightArm.life + allRobotControllers[i].robot.RightArm.attack + allRobotControllers[i].robot.RightArm.defence + allRobotControllers[i].robot.RightArm.velocity));
                    GameManage.instance.ChangeRightArm(GB.g_MyID, robotCon.robot.RightArm, callbackUserInfo);
                }
            }
            else
            {
                robotCon.robot.RightArm.is_broken = 1;
                robotCon.robot.RightArm.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeRightArm(GB.g_MyID, robotCon.robot.RightArm, callbackUserInfo);
            }

            if (!robotCon.isLegDestroyed)
            {
                Leg basicLeg = GameManage.AllLegs.Find(x => x.leg_id == allRobotControllers[i].robot.Leg.leg_id);

                if (robotCon.robot.Leg.level < 5)
                {
                    robotCon.robot.Leg.life += (basicLeg.life / 5) / 5;
                    robotCon.robot.Leg.attack += (basicLeg.attack / 5) / 5;
                    robotCon.robot.Leg.defence += (basicLeg.defence / 5) / 5;
                    robotCon.robot.Leg.velocity += (basicLeg.velocity / 5) / 5;

                    robotCon.robot.Leg.level = (int)((basicLeg.life + basicLeg.attack + basicLeg.defence + basicLeg.velocity) / (allRobotControllers[i].robot.Leg.life + allRobotControllers[i].robot.Leg.attack + allRobotControllers[i].robot.Leg.defence + allRobotControllers[i].robot.Leg.velocity));
                    GameManage.instance.ChangeLegs(GB.g_MyID, robotCon.robot.Leg, callbackUserInfo);
                }
            }
            else
            {
                robotCon.robot.Leg.is_broken = 1;
                robotCon.robot.Leg.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeLegs(GB.g_MyID, robotCon.robot.Leg, callbackUserInfo);
            }
        }

        //Update local json file
        //GameManage.User.UpdateLocalFile();
        //GameManage.instance.UpdateLevelLocalFile();
    }

    void ShowLoosePanel()
    {
        //Advertisement.Instance.ShowInterstitialAd();
        LooseRobotOrPiece();

        playcs.ShowSubPanel(6);
        if (looseBluePrintList.Count > 0)
        {
            playcs.SubPanelList[6].GetComponent<LoosePanel>().Init(looseBluePrintList[0]);
        }
        else
        {
            playcs.SubPanelList[6].GetComponent<LoosePanel>().Init(null);
        }
    }

    List<LooseBluePrint> looseBluePrintList = new List<LooseBluePrint>();
    void LooseRobotOrPiece()
    {
        looseBluePrintList = new List<LooseBluePrint>();

        if (GameManage.User.Robots.Count > 1)
        {
            if (MineDestroyedRobots.Count > 0)
            {
                int index = Random.Range(0, MineDestroyedRobots.Count);

                Robot robot = MineDestroyedRobots[0];
                LooseBluePrint rbp1 = new LooseBluePrint();
                rbp1.name = robot.Name;
                rbp1.type = "Robot";
                rbp1.price = (int)robot.price;
                rbp1.itemId = robot.Head.head_id;

                //GameManage.User.Robots.Add(robot);

                looseBluePrintList.Add(rbp1);
            }
            else
            {
                for (int i = 0; i < MineActiveRobots.Count; i++)
                {
                    RobotController robotCon = MineActiveRobots[i].GetComponent<RobotController>();

                    if (!MineDestroyedRobots.Contains(robotCon.robot))
                    {
                        if (robotCon.isLeftArmDestroyed)
                        {
                            LooseBluePrint rbp1 = new LooseBluePrint();
                            rbp1.name = robotCon.robot.LeftArm.name;
                            rbp1.type = "Left Arm";
                            rbp1.price = (int)robotCon.robot.LeftArm.price;
                            rbp1.itemId = robotCon.robot.LeftArm.larm_id;

                            //GameManage.User.LeftArms.Add(robotCon.robot.LeftArm);

                            looseBluePrintList.Add(rbp1);
                            break;
                        }
                        else if (robotCon.isRightArmDestroyed)
                        {
                            LooseBluePrint rbp1 = new LooseBluePrint();
                            rbp1.name = robotCon.robot.RightArm.name;
                            rbp1.type = "Right Arm";
                            rbp1.price = (int)robotCon.robot.RightArm.price;
                            rbp1.itemId = robotCon.robot.RightArm.rarm_id;

                            //GameManage.User.RightArms.Add(robotCon.robot.RightArm);

                            looseBluePrintList.Add(rbp1);
                            break;
                        }
                        else if (robotCon.isLegDestroyed)
                        {
                            LooseBluePrint rbp1 = new LooseBluePrint();
                            rbp1.name = robotCon.robot.Leg.name;
                            rbp1.type = "Leg";
                            rbp1.price = (int)robotCon.robot.Leg.price;
                            rbp1.itemId = robotCon.robot.Leg.leg_id;

                            //jjjjjjjGameManage.User.Legs.Add(robotCon.robot.Leg);

                            looseBluePrintList.Add(rbp1);
                            break;
                        }
                    }
                }
            }
        }
    }

    public void Loose()
    {
        if (MineDestroyedRobots.Count > 0)
        {
            Robot robot = MineDestroyedRobots[0];

            GameManage.instance.RemoveRobot(robot, callbackUserInfo);
            //GameManage.User.Robots.Remove(robot);
        }
        else
        {
            for (int i = 0; i < MineActiveRobots.Count; i++)
            {
                RobotController robotCon = MineActiveRobots[i].GetComponent<RobotController>();
                if (!MineDestroyedRobots.Contains(robotCon.robot))
                {
                    if (robotCon.isLeftArmDestroyed)
                    {
                        GameManage.instance.RemoveLeftArm(robotCon.robot.LeftArm, callbackUserInfo);
                        //GameManage.User.LeftArms.Remove(robotCon.robot.LeftArm);
                        break;
                    }
                    else if (robotCon.isRightArmDestroyed)
                    {
                        GameManage.instance.RemoveRightArm(robotCon.robot.RightArm, callbackUserInfo);
                        //GameManage.User.RightArms.Remove(robotCon.robot.RightArm);
                        break;
                    }
                    else if (robotCon.isLegDestroyed)
                    {
                        GameManage.instance.RemoveLeg(robotCon.robot.Leg, callbackUserInfo);
                        //GameManage.User.Legs.Remove(robotCon.robot.Leg);
                        break;
                    }
                }
            }
        }
    }

    public void BreakPieces()
    {
        //Upgrade current robots
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            RobotController robotCon = allRobotControllers[i];

            if (robotCon.isHeadDestroyed)
            {
                robotCon.robot.Head.is_broken = 1;
                robotCon.robot.Head.break_timestamp = (Int64) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeHead(GB.g_MyID, robotCon.robot.Head, callbackUserInfo);

                robotCon.robot.LeftArm.is_broken = 1;
                robotCon.robot.LeftArm.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeLeftArm(GB.g_MyID, robotCon.robot.LeftArm, callbackUserInfo);

                robotCon.robot.RightArm.is_broken = 1;
                robotCon.robot.RightArm.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeRightArm(GB.g_MyID, robotCon.robot.RightArm, callbackUserInfo);

                robotCon.robot.Leg.is_broken = 1;
                robotCon.robot.Leg.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                GameManage.instance.ChangeLegs(GB.g_MyID, robotCon.robot.Leg, callbackUserInfo);
            }
            else
            {
                if (robotCon.isLeftArmDestroyed)
                {
                    robotCon.robot.LeftArm.is_broken = 1;
                    robotCon.robot.LeftArm.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    GameManage.instance.ChangeLeftArm(GB.g_MyID, robotCon.robot.LeftArm, callbackUserInfo);
                }

                if (robotCon.isRightArmDestroyed)
                {
                    robotCon.robot.RightArm.is_broken = 1;
                    robotCon.robot.RightArm.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    GameManage.instance.ChangeRightArm(GB.g_MyID, robotCon.robot.RightArm, callbackUserInfo);
                }

                if (robotCon.isLegDestroyed)
                {
                    robotCon.robot.Leg.is_broken = 1;
                    robotCon.robot.Leg.break_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    GameManage.instance.ChangeLegs(GB.g_MyID, robotCon.robot.Leg, callbackUserInfo);
                }
            }
        }
    }

    public void ShowDrawPanel()
    {
        if (isMultiplayer)
        {
            playcs.ShowSubPanel(9);
            playcs.SubPanelList[9].GetComponent<DrawPanel>().Init();
        }
    }

    public int videoAdCount = 2;
    public void VideoAdWatched()
    {
        videoAdCount--;
        if (videoAdCount == 0)
        {
            //Update victory and loose panel
            playcs.SubPanelList[5].GetComponent<VictoryPanel>().WatchedVideoAd();
            playcs.SubPanelList[6].GetComponent<LoosePanel>().WatchedVideoAd();
            //Reward medicament
            List<int> pieceTypeAvailable = new List<int>();
            if (GameManage.User.Heads.Count > 0)
            {
                pieceTypeAvailable.Add(0);
            }
            if (GameManage.User.LeftArms.Count > 0)
            {
                pieceTypeAvailable.Add(1);
            }
            if (GameManage.User.RightArms.Count > 0)
            {
                pieceTypeAvailable.Add(2);
            }
            if (GameManage.User.Legs.Count > 0)
            {
                pieceTypeAvailable.Add(3);
            }

            int randPieceType = Random.Range(0, pieceTypeAvailable.Count);
            switch (pieceTypeAvailable[randPieceType])
            {
                case 0:
                    Head head = new Head();
                    head = GameManage.User.Heads[Random.Range(0, GameManage.User.Heads.Count)];
                    GameManage.instance.ChangeHead(GB.g_MyID, head, callbackRewardMedicament);
                    break;
                case 1:
                    LeftArm larm = new LeftArm();
                    larm = GameManage.User.LeftArms[Random.Range(0, GameManage.User.LeftArms.Count)];
                    GameManage.instance.ChangeLeftArm(GB.g_MyID, larm, callbackRewardMedicament);
                    break;
                case 2:
                    RightArm rarm = new RightArm();
                    rarm = GameManage.User.RightArms[Random.Range(0, GameManage.User.RightArms.Count)];
                    GameManage.instance.ChangeRightArm(GB.g_MyID, rarm, callbackRewardMedicament);
                    break;
                case 3:
                    Leg leg = new Leg();
                    leg = GameManage.User.Legs[Random.Range(0, GameManage.User.Legs.Count)];
                    GameManage.instance.ChangeLegs(GB.g_MyID, leg, callbackRewardMedicament);
                    break;
            }
        }
        else
        {
            playcs.SubPanelList[5].GetComponent<VictoryPanel>().WatchedVideoAd();
            playcs.SubPanelList[6].GetComponent<LoosePanel>().WatchedVideoAd();
        }
    }

    int deadRobotCount = 0;
    int runnableRobotCount = 0;
    List<RobotController> destroyedLegRobots;
    public void DetermineAttackOrder()
    {
        Debug.Log("Determining attack Order");
        currentGameState = GameState.Racing;

        deadRobotCount = 0;
        runnableRobotCount = 0;
        destroyedLegRobots = new List<RobotController>();
        for (int i = 0; i < allRobotControllers.Count; i++)
        {
            if (i < MineActiveRobots.Count)
            {
                allRobotControllers[i].RunToPos(MineAttackPosList[i]);
            }
            else
            {
                allRobotControllers[i].RunToPos(EnemyAttackPosList[i - MineActiveRobots.Count]);
            }
        }
        Debug.Log("Runnable : " + runnableRobotCount);
        Debug.Log("Destroyed Leg : " + destroyedLegRobots.Count);
        StartCoroutine(WaitForRaceEndRoutine());
    }

    IEnumerator WaitForRaceEndRoutine()
    {
        while (attackRobotOrder.Count < allRobotControllers.Count)
        {
            yield return new WaitForEndOfFrame();
        }

        //adding runnable robots
        List<RobotController> rList = new List<RobotController>(attackRobotOrder);
        attackRobotOrder.Clear();

        for (int i = 0; i < rList.Count; i++)
        {
            if (!rList[i].isHeadDestroyed && !rList[i].isLegDestroyed)
            {
                attackRobotOrder.Add(rList[i]);
            }
        }

        //adding robots with destroyed legs
        List<RobotController> lList = new List<RobotController>();
        for (int i = 0; i < rList.Count; i++)
        {
            if (rList[i].isLegDestroyed)
            {
                lList.Add(rList[i]);
            }
        }

        RobotController[] a = lList.ToArray();
        var result = from robotController in lList
                     orderby robotController.robot.Leg.life descending
                     select robotController;

        for (int i = 0; i < result.ToList().Count; i++)
        {
            attackRobotOrder.Add(result.ToList()[i]);
        }

        //adding robots with destroyed heads
        for (int i = 0; i < rList.Count; i++)
        {
            if (rList[i].isHeadDestroyed)
            {
                attackRobotOrder.Add(rList[i]);
            }
        }

        if (isMultiplayer)
        {
            PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.RaceComplete, true);
            if (PhotonNetwork.isMasterClient)
            {
                string attackRobotOrderString = "";
                for (int i = 0; i < attackRobotOrder.Count; i++)
                {
                    attackRobotOrderString = attackRobotOrderString + attackRobotOrder[i].id;
                }
                PhotonUtility.SetRoomProperties(PhotonEnums.Room.AttackRobotOrder, attackRobotOrderString);
            }

            PhotonMultiplayerManager.Instance.WaitForEndRace();
        }
        else
        {
            //Start selecting attack target and method
            StartSelectingAttackTarget();
        }
        yield return 0;
    }

    public void StartSelectingAttackTarget()
    {
        if (isGameOver)
        {
            return;
        }

        currentGameState = GameState.SelectingAttackOrder;
        attackSelectionCount++;

        playcs.ShowSubPanel(0);
        playcs.DeactivateFightButton();
        playcs.ShowMatchTimer();
        //Select default player
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!allRobotControllers[i].isHeadDestroyed)
            {
                ToggleMineRobotSelection(i);
                break;
            }
        }

        if (isMultiplayer)
        {
            playcs.ActivateRadialTimeSlider();
            PhotonMultiplayerManager.Instance.StartAttackSelectionTimer();
            //Start waiting for fight signal
            StartCoroutine(StartFightWaitRoutine());
            return;
        }
        else
        {
            playcs.DeactivateRadialTimeSlider();
        }
        //select AI robot's attack target
        SelectTargetForAIRobots();
        //Start waiting for fight signal
        StartCoroutine(StartFightWaitRoutine());
    }

    IEnumerator StartFightWaitRoutine()
    {
        while (isAllAttackSelected == false)
        {
            yield return new WaitForEndOfFrame();
        }
        playcs.ActivateFightButton();
    }

    public void ToggleMineRobotSelection(int id)
    {
        Debug.Log("Toggling mine robot: " + id);
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (id == i)
            {
                allRobotControllers[i].Select();
                isMineSelected = true;
                mineSelectedId = id;
                if (isEnemySelected == false)
                {
                    playcs.OnMineSelection();
                }
            }
            else
            {
                //playcs.DeactivateMFBar();
                allRobotControllers[i].Deselect();
                if (id == -1)
                {
                    isMineSelected = false;
                    mineSelectedId = -1;
                }
            }
        }
    }

    public void ToggleEnemyRobotSelection(int id)
    {
        Debug.Log("Toggling enemy robot: " + id);
        if (isMineSelected == true)
        {
            for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
            {
                if (id == i)
                {
                    allRobotControllers[i].Select();
                    isEnemySelected = true;
                    enemySelectedId = id;
                    playcs.OnEnemySelection();
                }
                else
                {
                    allRobotControllers[i].Deselect();
                    if (id == -1)
                    {
                        isEnemySelected = false;
                        enemySelectedId = -1;
                    }
                }
            }
        }
        else
        {
            playcs.ShowWarningMessage(WarningMessages.SELECT_YOUR_ROBOT);
        }
    }

    public int mineRobotDied = 0;
    public int opponentRobotDied = 0;

    public int minePartsDestroyed = 0;
    public int opponentPartsDestroyed = 0;

    public int minePartId = -1;

    public void SelectMinePart(int partId)
    {
        minePartId = partId;
    }

    public void SelectOpponentPart(int id)
    {
        string s = mineSelectedId.ToString() + minePartId.ToString() + enemySelectedId.ToString() + id.ToString();
        allRobotControllers[mineSelectedId].SetAttackTarget(s);

        if (isMultiplayer)
        {
            if (mineSelectedId == 0)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1, s);
                //Debug.LogError("s 1: " + s);
                //Debug.LogError("hgfghf 1: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1));
            }
            else if (mineSelectedId == 1)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2, s);
                //Debug.LogError("s 2: " + s);
                //Debug.LogError("hgfghf 2: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2));
            }
            else if (mineSelectedId == 2)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3, s);
                //Debug.LogError("s 3: " + s);
                //Debug.LogError("hgfghf 3: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3));
            }
        }

        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!allRobotControllers[i].isHeadDestroyed && string.IsNullOrEmpty(allRobotControllers[i].attackTargetString))
            {
                ToggleMineRobotSelection(i);
                break;
            }
        }
        //ToggleEnemyRobotSelection(-1);

        int attackSelected = 0;
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!string.IsNullOrEmpty(allRobotControllers[i].attackTargetString))
            {
                attackSelected++;
            }
        }
        if (!isMultiplayer)
        {
            if (attackSelected == GameManage.instance.myTeam.Count - mineRobotDied)
            {
                isAllAttackSelected = true;
            }
        }
        else
        {
            int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            if (attackSelected == robotCount - mineRobotDied)
            {
                isAllAttackSelected = true;
            }
        }

        if (isAllAttackSelected)
        {
            playcs.DeactivateMFBar();
            for (int i = 0; i < MineActiveRobots.Count; i++)
            {
                playcs.DeactivateMFBar();
                allRobotControllers[i].Deselect();
                isMineSelected = false;
                mineSelectedId = -1;
            }

            for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
            {
                allRobotControllers[i].Deselect();
                isEnemySelected = false;
                enemySelectedId = -1;
            }
        }
    }

    public void StartFight()
    {
        Debug.Log("Start Fighting");
        isFighting = true;
        currentGameState = GameState.Attacking;
        playcs.DeactivateRadialTimeSlider();
        playcs.HideActionGroups();
        playcs.ShowMatchTimer();
        //Deselect all robots
        for (int i = 0; i < allRobotControllers.Count; i++)
        {
            allRobotControllers[i].Deselect();
            allRobotControllers[i].DeactivateAttackSelectedIndicator();
        }
        //Starting attack with first robot in list
        attackRobotOrder[currentAttackRobotid].Attack();
    }

    public void AttackCompleted()
    {
        if (isGameOver)
        {
            return;
        }
        currentAttackRobotid++;
        if (currentAttackRobotid < attackRobotOrder.Count)
        {
            playcs.DeactivateMFBar();
            playcs.ShowStatusView(-1);
            playcs.ShowEffectName("", true);
            playcs.ShowEffectName("", false);
            attackRobotOrder[currentAttackRobotid].Attack();
        }
        else
        {
            Debug.Log("Round completed");
            playcs.DeactivateMFBar();
            playcs.ShowStatusView(-1);
            playcs.ShowEffectName("", true);
            playcs.ShowEffectName("", false);
            ResetRound();
        }
    }

    private void ResetRound()
    {
        currentGameState = GameState.InitialCountDown;

        isMineSelected = false;
        isEnemySelected = false;
        mineSelectedId = -1;
        enemySelectedId = -1;

        isAllAttackSelected = false;
        isFighting = false;

        currentAttackRobotid = 0;

        //attackRobotOrder.Clear();


        for (int i = 0; i < PlayManage.instance.allRobotControllers.Count; i++)
        {
            PlayManage.instance.allRobotControllers[i].SetAttackTarget("");
        }
        //StartSelectingAttackTarget();
        if (!isGameOver)
        {
            if (IsGameOver())
            {
                if (IsPlayerWinner())
                {
                    ShowVictoryPanel((matchTimeInSec - currentMatchTime), mineAttackCount, mineDefenceCount);
                }
                else
                {
                    ShowLoosePanel();
                }
            }
            else
            {
                if (isMultiplayer)
                {
                    PhotonMultiplayerManager.Instance.ResetRound();
                }
                else
                {
                    StartCoroutine(DetermineAttackOrderAgain());
                    //StartSelectingAttackTarget();
                }
            }
        }
        //mineAttackCount = 0;
    }

    private IEnumerator DetermineAttackOrderAgain()
    {
        yield return new WaitForSeconds(2f);
        //DetermineAttackOrder();
        StartSelectingAttackTarget();
    }

    bool IsGameOver()
    {
        float mineLife = 0;
        float enemyLife = 0;
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            mineLife += allRobotControllers[i].totalLife;
        }

        for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
        {
            enemyLife += allRobotControllers[i].totalLife;
        }

        if (mineRobotDied == MineActiveRobots.Count || opponentRobotDied == EnemyActiveRobots.Count)
        {
            isGameOver = true;
            return true;
        }
        else
        {
            if (mineLife <= 0 || enemyLife <= 0)
            {
                isGameOver = true;
                return true;
            }
            else
            {
                isGameOver = false;
                return false;
            }
        }
    }

    bool IsPlayerWinner()
    {
        float mineLife = 0;
        float enemyLife = 0;
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            mineLife += allRobotControllers[i].totalLife;
        }

        for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
        {
            enemyLife += allRobotControllers[i].totalLife;
        }

        if (mineRobotDied < opponentRobotDied)
        {
            return true;
        }
        else if (mineRobotDied > opponentRobotDied)
        {
            return false;
        }
        else
        {
            if (minePartsDestroyed < opponentPartsDestroyed)
            {
                return true;
            }
            else if (minePartsDestroyed > opponentPartsDestroyed)
            {
                return false;
            }
            else
            {
                if (mineLife > enemyLife)
                {
                    return true;
                }
                else if (mineLife < enemyLife)
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }


    }

    public void SelectAutoAttack()
    {
        bool mPartSelected = false;
        bool oPartSelected = false;

        int mPartId = -1;
        int oPartId = -1;

        while (mPartSelected == false)
        {
            float randValue = Random.value;
            if (randValue >= 0 && randValue < 0.4f)
            {
                mPartId = 0;
            }
            else if (randValue >= 0.4f && randValue < 0.6f)
            {
                mPartId = 1;
            }
            else if (randValue >= 0.6f && randValue < 0.8f)
            {
                mPartId = 2;
            }
            else
            {
                mPartId = 3;
            }

            switch (mPartId)
            {
                case 0:
                    if (allRobotControllers[mineSelectedId].isHeadDestroyed == true || allRobotControllers[mineSelectedId].headAttackCount == 0)
                    {
                        mPartSelected = false;
                    }
                    else
                    {
                        mPartSelected = true;
                    }
                    break;
                case 1:
                    if (allRobotControllers[mineSelectedId].isLeftArmDestroyed == true || allRobotControllers[mineSelectedId].larmAttackCount == 0)
                    {
                        mPartSelected = false;
                    }
                    else
                    {
                        mPartSelected = true;
                    }
                    break;
                case 2:
                    if (allRobotControllers[mineSelectedId].isRightArmDestroyed == true || allRobotControllers[mineSelectedId].rarmAttackCount == 0)
                    {
                        mPartSelected = false;
                    }
                    else
                    {
                        mPartSelected = true;
                    }
                    break;
                case 3:
                    if (allRobotControllers[mineSelectedId].isLegDestroyed == true || allRobotControllers[mineSelectedId].legAttackCount == 0)
                    {
                        mPartSelected = false;
                    }
                    else
                    {
                        mPartSelected = true;
                    }
                    break;
            }
        }

        while (oPartSelected == false)
        {
            oPartId = Random.Range(0, 4);
            //oPartId = 3;
            switch (oPartId)
            {
                case 0:
                    if (allRobotControllers[enemySelectedId].isHeadDestroyed == true)
                    {
                        oPartSelected = false;
                    }
                    else
                    {
                        oPartSelected = true;
                    }
                    break;
                case 1:
                    if (allRobotControllers[enemySelectedId].isLeftArmDestroyed == true)
                    {
                        oPartSelected = false;
                    }
                    else
                    {
                        oPartSelected = true;
                    }
                    break;
                case 2:
                    if (allRobotControllers[enemySelectedId].isRightArmDestroyed == true)
                    {
                        oPartSelected = false;
                    }
                    else
                    {
                        oPartSelected = true;
                    }
                    break;
                case 3:
                    if (allRobotControllers[enemySelectedId].isLegDestroyed == true)
                    {
                        oPartSelected = false;
                    }
                    else
                    {
                        oPartSelected = true;
                    }
                    break;
            }

        }


        string s = mineSelectedId.ToString() + mPartId.ToString() + enemySelectedId.ToString() + oPartId.ToString();
        allRobotControllers[mineSelectedId].SetAttackTarget(s);

        if (isMultiplayer)
        {
            if (mineSelectedId == 0)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1, s);
                //Debug.LogError("s 1: " + s);
                //Debug.LogError("hgfghf 1: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1));
            }
            else if (mineSelectedId == 1)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2, s);
                //Debug.LogError("s 2: " + s);
                //Debug.LogError("hgfghf 2: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2));
            }
            else if (mineSelectedId == 2)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3, s);
                //Debug.LogError("s 3: " + s);
                //Debug.LogError("hgfghf 3: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3));
            }
        }

        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!allRobotControllers[i].isHeadDestroyed && string.IsNullOrEmpty(allRobotControllers[i].attackTargetString))
            {
                ToggleMineRobotSelection(i);
                break;
            }
        }
        //ToggleEnemyRobotSelection(-1);

        int attackSelected = 0;
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!string.IsNullOrEmpty(allRobotControllers[i].attackTargetString))
            {
                attackSelected++;
            }
        }
        if (!isMultiplayer)
        {
            if (attackSelected == GameManage.instance.myTeam.Count - mineRobotDied)
            {
                isAllAttackSelected = true;
            }
        }
        else
        {
            int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            if (attackSelected == robotCount - mineRobotDied)
            {
                isAllAttackSelected = true;
            }
        }

        if (isAllAttackSelected)
        {
            playcs.DeactivateMFBar();
            for (int i = 0; i < MineActiveRobots.Count; i++)
            {
                playcs.DeactivateMFBar();
                allRobotControllers[i].Deselect();
                isMineSelected = false;
                mineSelectedId = -1;
            }

            for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
            {
                allRobotControllers[i].Deselect();
                isEnemySelected = false;
                enemySelectedId = -1;
            }
        }
    }

    public void SelectAutoOpponentParts()
    {
        bool oPartSelected = false;

        int oPartId = -1;

        while (oPartSelected == false)
        {
            float randValue = Random.value;
            //oPartId = 3;
            if (randValue <= 0.6f)
            {
                if (allRobotControllers[enemySelectedId].isHeadDestroyed == true || allRobotControllers[enemySelectedId].headAttackCount == 0)
                {
                    oPartSelected = false;
                }
                else
                {
                    oPartId = 0;
                    oPartSelected = true;
                }
            }
            else if (randValue > 0.6f && randValue <= 0.7f)
            {
                if (allRobotControllers[enemySelectedId].isLeftArmDestroyed == true || allRobotControllers[enemySelectedId].larmAttackCount == 0)
                {
                    oPartSelected = false;
                }
                else
                {
                    oPartId = 1;
                    oPartSelected = true;
                }
            }
            else if (randValue > 0.7f && randValue <= 0.8f)
            {
                if (allRobotControllers[enemySelectedId].isRightArmDestroyed == true || allRobotControllers[enemySelectedId].rarmAttackCount == 0)
                {
                    oPartSelected = false;
                }
                else
                {
                    oPartId = 2;
                    oPartSelected = true;
                }
            }
            else if (randValue > 0.8f && randValue <= 1f)
            {
                if (allRobotControllers[enemySelectedId].isLegDestroyed == true || allRobotControllers[enemySelectedId].legAttackCount == 0)
                {
                    oPartSelected = false;
                }
                else
                {
                    oPartId = 3;
                    oPartSelected = true;
                }
            }
        }


        string s = mineSelectedId.ToString() + minePartId.ToString() + enemySelectedId.ToString() + oPartId.ToString();
        allRobotControllers[mineSelectedId].SetAttackTarget(s);

        if (isMultiplayer)
        {
            if (mineSelectedId == 0)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1, s);
                //Debug.LogError("s 1: " + s);
                //Debug.LogError("hgfghf 1: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString1));
            }
            else if (mineSelectedId == 1)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2, s);
                //Debug.LogError("s 2: " + s);
                //Debug.LogError("hgfghf 2: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString2));
            }
            else if (mineSelectedId == 2)
            {
                PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3, s);
                //Debug.LogError("s 3: " + s);
                //Debug.LogError("hgfghf 3: " + PhotonUtility.GetPlayerProperties<string>(PhotonNetwork.player, PhotonEnums.Player.AttackTargetString3));
            }
        }

        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!allRobotControllers[i].isHeadDestroyed && string.IsNullOrEmpty(allRobotControllers[i].attackTargetString))
            {
                ToggleMineRobotSelection(i);
                break;
            }
        }
        //ToggleEnemyRobotSelection(-1);

        int attackSelected = 0;
        for (int i = 0; i < MineActiveRobots.Count; i++)
        {
            if (!string.IsNullOrEmpty(allRobotControllers[i].attackTargetString))
            {
                attackSelected++;
            }
        }
        if (!isMultiplayer)
        {
            if (attackSelected == GameManage.instance.myTeam.Count - mineRobotDied)
            {
                isAllAttackSelected = true;
            }
        }
        else
        {
            int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            if (attackSelected == robotCount - mineRobotDied)
            {
                isAllAttackSelected = true;
            }
        }

        if (isAllAttackSelected)
        {
            playcs.DeactivateMFBar();
            for (int i = 0; i < MineActiveRobots.Count; i++)
            {
                playcs.DeactivateMFBar();
                allRobotControllers[i].Deselect();
                isMineSelected = false;
                mineSelectedId = -1;
            }

            for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
            {
                allRobotControllers[i].Deselect();
                isEnemySelected = false;
                enemySelectedId = -1;
            }
        }
    }

    void SelectTargetForAIRobots()
    {
        for (int i = MineActiveRobots.Count; i < allRobotControllers.Count; i++)
        {
            bool mPartSelected = false;
            bool oSelected = false;
            bool oPartSelected = false;

            int mPartId = -1;
            int oId = -1;
            int oPartId = -1;

            while (mPartSelected == false)
            {
                mPartId = Random.Range(0, 4);
                switch (mPartId)
                {
                    case 0:
                        if (allRobotControllers[i].isHeadDestroyed == true)
                        {
                            mPartSelected = false;
                        }
                        else
                        {
                            mPartSelected = true;
                        }
                        break;
                    case 1:
                        if (allRobotControllers[i].isLeftArmDestroyed == true)
                        {
                            mPartSelected = false;
                        }
                        else
                        {
                            mPartSelected = true;
                        }
                        break;
                    case 2:
                        if (allRobotControllers[i].isRightArmDestroyed == true)
                        {
                            mPartSelected = false;
                        }
                        else
                        {
                            mPartSelected = true;
                        }
                        break;
                    case 3:
                        if (allRobotControllers[i].isLegDestroyed == true)
                        {
                            mPartSelected = false;
                        }
                        else
                        {
                            mPartSelected = true;
                        }
                        break;
                }

            }

            while (oSelected == false)
            {
                oId = Random.Range(0, MineActiveRobots.Count);
                if (allRobotControllers[oId].totalLife <= 0)
                {
                    oSelected = false;
                }
                else
                {
                    oSelected = true;
                }
            }

            while (oPartSelected == false)
            {
                oPartId = Random.Range(0, 4);
                //oPartId = 3;
                switch (oPartId)
                {
                    case 0:
                        if (allRobotControllers[oId].isHeadDestroyed == true)
                        {
                            oPartSelected = false;
                        }
                        else
                        {
                            oPartSelected = true;
                        }
                        break;
                    case 1:
                        if (allRobotControllers[oId].isLeftArmDestroyed == true)
                        {
                            oPartSelected = false;
                        }
                        else
                        {
                            oPartSelected = true;
                        }
                        break;
                    case 2:
                        if (allRobotControllers[oId].isRightArmDestroyed == true)
                        {
                            oPartSelected = false;
                        }
                        else
                        {
                            oPartSelected = true;
                        }
                        break;
                    case 3:
                        if (allRobotControllers[oId].isLegDestroyed == true)
                        {
                            oPartSelected = false;
                        }
                        else
                        {
                            oPartSelected = true;
                        }
                        break;
                }

            }
            //int opponentId = Random.Range(0, MineActiveRobots.Count);
            //int opponentPartId = Random.Range(0, 4);

            string s = i.ToString() + mPartId.ToString() + oId.ToString() + oPartId.ToString();
            Debug.Log("AI Target: " + s);
            allRobotControllers[i].SetAttackTarget(s);
        }
    }

    public void LocalPlayerDisconnected()
    {
        Debug.Log("Local player disconnected");
        switch (currentGameState)
        {
            case GameState.WaitingForPlayers:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.PLAYER_DISCONNECTED);
                playcs.MatchLoose();
                break;
            case GameState.InitialCountDown:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.PLAYER_DISCONNECTED);
                playcs.MatchLoose();
                break;
            case GameState.Racing:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.PLAYER_DISCONNECTED);
                playcs.MatchLoose();
                break;
            case GameState.SelectingAttackOrder:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.PLAYER_DISCONNECTED);
                playcs.MatchLoose();
                break;
            case GameState.Attacking:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.PLAYER_DISCONNECTED);
                playcs.MatchLoose();
                break;
            default:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.PLAYER_DISCONNECTED);
                playcs.MatchLoose();
                break;
        }
    }

    public void OpponentDisconnected()
    {
        Debug.Log("Remote player disconnected");
        switch (currentGameState)
        {
            case GameState.WaitingForPlayers:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.OPPONENT_DISCONNECTED);
                playcs.MatchWin();
                break;
            case GameState.InitialCountDown:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.OPPONENT_DISCONNECTED);
                playcs.MatchWin();
                break;
            case GameState.Racing:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.OPPONENT_DISCONNECTED);
                playcs.MatchWin();
                break;
            case GameState.SelectingAttackOrder:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.OPPONENT_DISCONNECTED);
                playcs.MatchWin();
                break;
            case GameState.Attacking:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.OPPONENT_DISCONNECTED);
                playcs.MatchWin();
                break;
            default:
                isGameOver = true;
                StopAllCoroutines();
                PhotonMultiplayerManager.Instance.StopAllRoutines();
                playcs.ShowSubPanel(-1);
                playcs.ShowWarningMessage(WarningMessages.OPPONENT_DISCONNECTED);
                playcs.MatchWin();
                break;
        }
    }

    public void MatchDraw()
    {
        ShowDrawPanel();
    }

    public void MatchWin()
    {
        ShowVictoryPanel(matchTimeInSec - currentMatchTime, mineAttackCount, mineDefenceCount);
    }

    public void MatchLoose()
    {
        ShowLoosePanel();
    }

    #endregion

    #region PREVIOUS
    void SaveInfo()
    {
        RobotNameList.Clear();

        //Mine
        AttackMineList.Clear();
        PartMineList.Clear();

        for (int i = 0; i < MineRobots.Count; i++)
        {
            Shared.Robot MineRobot = new Shared.Robot();
            Transform _child1 = MineRobots[i].transform.GetChild(0);

            //Get AttackList
            Transform _child2 = _child1.GetChild(0);

            for (int j = 0; j < _child2.childCount; j++)
            {
                if (i == 0)
                {
                    RobotNameList.Add(_child2.GetChild(j).name);
                }

                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    AttackMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get HMRobotList
            _child2 = _child1.GetChild(1);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get FlyingRobotList
            _child2 = _child1.GetChild(2);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get RollRobotList
            _child2 = _child1.GetChild(3);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get SPRobotList
            _child2 = _child1.GetChild(4);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }


            dbData.m_MineMedabot.robotList.Add(MineRobot);
        }


        //Enemy
        AttackEnemyList.Clear();
        PartEnemyList.Clear();

        for (int i = 0; i < EnemyRobots.Count; i++)
        {
            Shared.Robot EnemyRobot = new Shared.Robot();
            Transform _child1 = EnemyRobots[i].transform.GetChild(0);

            //Get AttackList
            Transform _child2 = _child1.GetChild(0);

            for (int j = 0; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    AttackEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get HMRobotList
            _child2 = _child1.GetChild(1);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get FlyingRobotList
            _child2 = _child1.GetChild(2);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get RollRobotList
            _child2 = _child1.GetChild(3);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get SPRobotList
            _child2 = _child1.GetChild(4);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }


            dbData.m_EnemyMedabot.robotList.Add(EnemyRobot);
        }
    }

    public void MakeMineRobots()
    {
        if (!isMultiplayer)
        {
            for (int i = 0; i < GameManage.instance.myTeam.Count; i++)
            {
                Robot robot = GameManage.instance.myTeam[i];

                RobotController robotCon = MineActiveRobots[i].GetComponent<RobotController>();

                robotCon.Initialize(robot);
            }
        }
        else
        {
            PhotonPlayer player = PhotonNetwork.player;

            int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            if (robotCount > 0)
            {
                string robotString1 = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Robot1);

                RobotBluePrint rbp = JsonUtility.FromJson<RobotBluePrint>(robotString1);
                Robot robot1 = new Robot();
                robot1.object_id = rbp.object_id;
                robot1.type = rbp.type;
                robot1.name = rbp.name;
                robot1.price = rbp.price;
                robot1.head_object_id = rbp.head_object_id;
                robot1.larm_object_id = rbp.larm_object_id;
                robot1.rarm_object_id = rbp.rarm_object_id;
                robot1.leg_object_id = rbp.leg_object_id;
                robot1.onSale = rbp.onSale;
                robot1.salePrice = rbp.salePrice;

                robot1.Head = rbp.head;
                robot1.LeftArm = rbp.larm;
                robot1.RightArm = rbp.rarm;
                robot1.Leg = rbp.leg;

                RobotController robotCon1 = MineActiveRobots[0].GetComponent<RobotController>();
                robotCon1.myPlayer = player;
                robotCon1.Initialize(robot1);
            }


            if (robotCount > 1)
            {
                string robotString2 = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Robot2);

                RobotBluePrint rbp = JsonUtility.FromJson<RobotBluePrint>(robotString2);
                Robot robot2 = new Robot();
                robot2.object_id = rbp.object_id;
                robot2.type = rbp.type;
                robot2.name = rbp.name;
                robot2.price = rbp.price;
                robot2.head_object_id = rbp.head_object_id;
                robot2.larm_object_id = rbp.larm_object_id;
                robot2.rarm_object_id = rbp.rarm_object_id;
                robot2.leg_object_id = rbp.leg_object_id;
                robot2.onSale = rbp.onSale;
                robot2.salePrice = rbp.salePrice;

                robot2.Head = rbp.head;
                robot2.LeftArm = rbp.larm;
                robot2.RightArm = rbp.rarm;
                robot2.Leg = rbp.leg;

                RobotController robotCon2 = MineActiveRobots[1].GetComponent<RobotController>();
                robotCon2.myPlayer = player;
                robotCon2.Initialize(robot2);
            }

            if (robotCount > 2)
            {
                string robotString3 = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Robot3);

                RobotBluePrint rbp = JsonUtility.FromJson<RobotBluePrint>(robotString3);
                Robot robot3 = new Robot();
                robot3.object_id = rbp.object_id;
                robot3.type = rbp.type;
                robot3.name = rbp.name;
                robot3.price = rbp.price;
                robot3.head_object_id = rbp.head_object_id;
                robot3.larm_object_id = rbp.larm_object_id;
                robot3.rarm_object_id = rbp.rarm_object_id;
                robot3.leg_object_id = rbp.leg_object_id;
                robot3.onSale = rbp.onSale;
                robot3.salePrice = rbp.salePrice;

                robot3.Head = rbp.head;
                robot3.LeftArm = rbp.larm;
                robot3.RightArm = rbp.rarm;
                robot3.Leg = rbp.leg;

                RobotController robotCon3 = MineActiveRobots[2].GetComponent<RobotController>();
                robotCon3.myPlayer = player;
                robotCon3.Initialize(robot3);
            }

            //int head1 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.Head1);
            //int head2 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.Head2);
            //int head3 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.Head3);

            //int leftArm1 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.LeftArm1);
            //int leftArm2 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.LeftArm2);
            //int leftArm3 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.LeftArm3);

            //int rightArm1 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.RightArm1);
            //int rightArm2 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.RightArm2);
            //int rightArm3 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.RightArm3);

            //int leg1 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.Leg1);
            //int leg2 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.Leg2);
            //int leg3 = PhotonUtility.GetPlayerProperties<int>(player, PhotonEnums.Player.Leg3);

            //m_iRobotList.Add(robot1); m_iRobotList.Add(robot2); m_iRobotList.Add(robot3);
            //m_iHeadList.Add(head1); m_iHeadList.Add(head2); m_iHeadList.Add(head3);
            //m_iLArmList.Add(leftArm1); m_iLArmList.Add(leftArm2); m_iLArmList.Add(leftArm3);
            //m_iRArmList.Add(rightArm1); m_iRArmList.Add(rightArm2); m_iRArmList.Add(rightArm3);
            //m_iLegList.Add(leg1); m_iLegList.Add(leg2); m_iLegList.Add(leg3);

            //for (int i = 0; i < MineRobots.Count; i++)
            //{
            //    int iIndex = 0;

            //    if (m_iRobotList[i] == 0) iIndex = 0;
            //    else if (m_iRobotList[i] == 1) iIndex = 0 + 15;
            //    else if (m_iRobotList[i] == 2) iIndex = 0 + 15 + 4;
            //    else if (m_iRobotList[i] == 3) iIndex = 0 + 15 + 4 + 3;

            //    PartMineList[i * 100 + iIndex * 4 + m_iHeadList[i] * 4 + 0].SetActive(true);
            //    PartMineList[i * 100 + iIndex * 4 + m_iLArmList[i] * 4 + 1].SetActive(true);
            //    PartMineList[i * 100 + iIndex * 4 + m_iRArmList[i] * 4 + 2].SetActive(true);
            //    PartMineList[i * 100 + iIndex * 4 + m_iLegList[i] * 4 + 3].SetActive(true);



            //    RobotPart head = new RobotPart(10 + i, 11 + i, 12 + i, 15 + i);
            //    RobotPart leftArm = new RobotPart(11 + i, 12 + i, 13 + i, 16 + i);
            //    RobotPart rightArm = new RobotPart(12 + i, 13 + i, 14 + i, 17 + i);
            //    RobotPart leg = new RobotPart(13 + i, 14 + i, 15 + i, 18 + i);


            //}
        }
    }

    public void MakeEnemyRobots()
    {
        if (!isMultiplayer)
        {
            for (int i = 0; i < GameManage.instance.opponentTeam.Count; i++)
            {
                Robot robot = GameManage.instance.opponentTeam[i];

                //e_iRobotList.Add(robot.object_id);
                //e_iHeadList.Add(robot.Head.head_id);
                //e_iLArmList.Add(robot.LeftArm.larm_id);
                //e_iRArmList.Add(robot.RightArm.rarm_id);
                //e_iLegList.Add(robot.Leg.leg_id);

                //int iIndex = 0;

                //if (e_iRobotList[i] == 0) iIndex = 0;
                //else if (e_iRobotList[i] == 1) iIndex = 0 + 15;
                //else if (e_iRobotList[i] == 2) iIndex = 0 + 15 + 4;
                //else if (e_iRobotList[i] == 3) iIndex = 0 + 15 + 4 + 3;

                //PartEnemyList[i * 100 + iIndex * 4 + e_iHeadList[i] * 4 + 0].SetActive(true);
                //PartEnemyList[i * 100 + iIndex * 4 + e_iLArmList[i] * 4 + 1].SetActive(true);
                //PartEnemyList[i * 100 + iIndex * 4 + e_iRArmList[i] * 4 + 2].SetActive(true);
                //PartEnemyList[i * 100 + iIndex * 4 + e_iLegList[i] * 4 + 3].SetActive(true);

                //EnemyActiveRobots.Add(EnemyRobots[i]);
                RobotController robotCon = EnemyActiveRobots[i].GetComponent<RobotController>();

                //RobotPart head = new RobotPart(10 + i, 11 + i, 12 + i, 15 + i);
                //RobotPart leftArm = new RobotPart(11 + i, 12 + i, 13 + i, 16 + i);
                //RobotPart rightArm = new RobotPart(12 + i, 13 + i, 14 + i, 17 + i);
                //RobotPart leg = new RobotPart(13 + i, 14 + i, 15 + i, 18 + i);

                robotCon.Initialize(robot);
            }
        }
        else
        {
            PhotonPlayer player = null;

            player = PhotonNetwork.player.GetNext();

            int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            if (robotCount > 0)
            {
                string robotString1 = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Robot1);

                RobotBluePrint rbp = JsonUtility.FromJson<RobotBluePrint>(robotString1);
                Robot robot1 = new Robot();
                robot1.object_id = rbp.object_id;
                robot1.type = rbp.type;
                robot1.name = rbp.name;
                robot1.price = rbp.price;
                robot1.head_object_id = rbp.head_object_id;
                robot1.larm_object_id = rbp.larm_object_id;
                robot1.rarm_object_id = rbp.rarm_object_id;
                robot1.leg_object_id = rbp.leg_object_id;
                robot1.onSale = rbp.onSale;
                robot1.salePrice = rbp.salePrice;

                robot1.Head = rbp.head;
                robot1.LeftArm = rbp.larm;
                robot1.RightArm = rbp.rarm;
                robot1.Leg = rbp.leg;

                RobotController robotCon1 = EnemyActiveRobots[0].GetComponent<RobotController>();
                robotCon1.myPlayer = player;
                robotCon1.Initialize(robot1);
            }


            if (robotCount > 1)
            {
                string robotString2 = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Robot2);

                RobotBluePrint rbp = JsonUtility.FromJson<RobotBluePrint>(robotString2);
                Robot robot2 = new Robot();
                robot2.object_id = rbp.object_id;
                robot2.type = rbp.type;
                robot2.name = rbp.name;
                robot2.price = rbp.price;
                robot2.head_object_id = rbp.head_object_id;
                robot2.larm_object_id = rbp.larm_object_id;
                robot2.rarm_object_id = rbp.rarm_object_id;
                robot2.leg_object_id = rbp.leg_object_id;
                robot2.onSale = rbp.onSale;
                robot2.salePrice = rbp.salePrice;

                robot2.Head = rbp.head;
                robot2.LeftArm = rbp.larm;
                robot2.RightArm = rbp.rarm;
                robot2.Leg = rbp.leg;

                RobotController robotCon2 = EnemyActiveRobots[1].GetComponent<RobotController>();
                robotCon2.myPlayer = player;
                robotCon2.Initialize(robot2);
            }

            if (robotCount > 2)
            {
                string robotString3 = PhotonUtility.GetPlayerProperties<string>(player, PhotonEnums.Player.Robot3);

                RobotBluePrint rbp = JsonUtility.FromJson<RobotBluePrint>(robotString3);
                Robot robot3 = new Robot();
                robot3.object_id = rbp.object_id;
                robot3.type = rbp.type;
                robot3.name = rbp.name;
                robot3.price = rbp.price;
                robot3.head_object_id = rbp.head_object_id;
                robot3.larm_object_id = rbp.larm_object_id;
                robot3.rarm_object_id = rbp.rarm_object_id;
                robot3.leg_object_id = rbp.leg_object_id;
                robot3.onSale = rbp.onSale;
                robot3.salePrice = rbp.salePrice;

                robot3.Head = rbp.head;
                robot3.LeftArm = rbp.larm;
                robot3.RightArm = rbp.rarm;
                robot3.Leg = rbp.leg;

                RobotController robotCon3 = EnemyActiveRobots[2].GetComponent<RobotController>();
                robotCon3.myPlayer = player;
                robotCon3.Initialize(robot3);
            }
        }
    }

    private void Init()
    {
        if (!isMultiplayer)
        {
            for (int i = 0; i < MineRobots.Count; i++)
            {
                if (i < GameManage.instance.myTeam.Count)
                {
                    MineActiveRobots.Add(MineRobots[i]);
                    MineActiveRobots[i].SetActive(true);
                }
                else
                {
                    MineRobots[i].SetActive(false);
                }
            }

            for (int i = 0; i < EnemyRobots.Count; i++)
            {
                if (i < GameManage.instance.opponentTeam.Count)
                {
                    EnemyActiveRobots.Add(EnemyRobots[i]);
                    EnemyActiveRobots[i].SetActive(true);
                }
                else
                {
                    EnemyRobots[i].SetActive(false);
                }
            }
        }
        else
        {
            int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            Debug.Log("Robot count: " + robotCount);

            for (int i = 0; i < MineRobots.Count; i++)
            {
                if (i < robotCount)
                {
                    MineActiveRobots.Add(MineRobots[i]);
                    MineActiveRobots[i].SetActive(true);
                }
                else
                {
                    MineRobots[i].SetActive(false);
                }
            }

            for (int i = 0; i < EnemyRobots.Count; i++)
            {
                if (i < robotCount)
                {
                    EnemyActiveRobots.Add(EnemyRobots[i]);
                    EnemyActiveRobots[i].SetActive(true);
                }
                else
                {
                    EnemyRobots[i].SetActive(false);
                }
            }
        }



        bMineAttack = false;
        bMineReturn = false;

        iSMineIndex = 0;
        iEMineIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (bMineAttack)
        {
            if (iType == 0)
            {
                _agentMine.destination = MineAttackPosList[iEMineIndex];

                fDistance = Vector3.Distance(_agentMine.gameObject.transform.localPosition, MineAttackPosList[iEMineIndex]);
            }
            else if (iType == 1)
            {
                _agentMine.destination = EnemyAttackPosList[iEMineIndex];

                fDistance = Vector3.Distance(_agentMine.gameObject.transform.localPosition, EnemyAttackPosList[iEMineIndex]);
            }

            if (fDistance < 0.09f)
            {
                if (iType == 0)
                {
                    _agentMine.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);
                }
                else if (iType == 1)
                {
                    _agentMine.gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
                }

                bMineAttack = false;
                MineAttackObj.SetActive(true);
                Invoke("ReturnMineRobot", 5.0f);
            }
        }
        else if (bMineReturn)
        {
            if (iType == 0)
            {
                _agentMine.destination = MineIdlePosList[iSMineIndex];

                fDistance = Vector3.Distance(_agentMine.gameObject.transform.localPosition, MineIdlePosList[iSMineIndex]);
            }
            else if (iType == 1)
            {
                _agentMine.destination = EnemyIdlePosList[iSMineIndex];

                fDistance = Vector3.Distance(_agentMine.gameObject.transform.localPosition, EnemyIdlePosList[iSMineIndex]);
            }

            if (fDistance < 0.09f)
            {
                if (iType == 0)
                {
                    _agentMine.gameObject.transform.localEulerAngles = new Vector3(0, 90, 0);
                }
                else if (iType == 1)
                {
                    _agentMine.gameObject.transform.localEulerAngles = new Vector3(0, -90, 0);
                }

                bMineReturn = false;
            }
        }
    }

    void ReturnMineRobot()
    {
        MineAttackObj.SetActive(false);
        bMineReturn = true;
    }

    public void MineAttack(string stIndex)
    {
        iType = int.Parse(stIndex.Substring(0, 1));
        iSMineIndex = int.Parse(stIndex.Substring(1, 1));
        iEMineIndex = int.Parse(stIndex.Substring(2, 1));

        if (iType == 0)
        {
            _agentMine = MineRobots[iSMineIndex].GetComponent<NavMeshAgent>();

            int iIndex = 0;

            if (m_iRobotList[iSMineIndex] == 0) iIndex = 0;
            else if (m_iRobotList[iSMineIndex] == 1) iIndex = 0 + 15;
            else if (m_iRobotList[iSMineIndex] == 2) iIndex = 0 + 15 + 4;
            else if (m_iRobotList[iSMineIndex] == 3) iIndex = 0 + 15 + 4 + 3;

            MineAttackObj = AttackMineList[iSMineIndex * 100 + iIndex * 4 + m_iHeadList[iSMineIndex] * 4 + 0];
        }
        else if (iType == 1)
        {
            _agentMine = EnemyRobots[iSMineIndex].GetComponent<NavMeshAgent>();

            int iIndex = 0;

            if (e_iRobotList[iSMineIndex] == 0) iIndex = 0;
            else if (e_iRobotList[iSMineIndex] == 1) iIndex = 0 + 15;
            else if (e_iRobotList[iSMineIndex] == 2) iIndex = 0 + 15 + 4;
            else if (e_iRobotList[iSMineIndex] == 3) iIndex = 0 + 15 + 4 + 3;

            MineAttackObj = AttackEnemyList[iSMineIndex * 100 + iIndex * 4 + e_iLArmList[iSMineIndex] * 4 + 0];
        }

        bMineAttack = true;
    }
    #endregion

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

    private int callbackRewardAdventure(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("RewardAdventure: Done");
                Debug.Log(www.text);
                GameManage.User.username = json["result"]["username"].ToString();
                GameManage.User.gems = int.Parse(json["result"]["gems"].ToString());
                GameManage.User.rank = int.Parse(json["result"]["rank"].ToString());
                GameManage.User.win = int.Parse(json["result"]["win"].ToString());
                GameManage.User.loss = int.Parse(json["result"]["loss"].ToString());
                GameManage.User.avatar_id = int.Parse(json["result"]["avatar_id"].ToString());

                for (int i = 0; i < json["result"]["heads"].Count; i++)
                {
                    Head hc = new Head();
                    hc.object_id = int.Parse(json["result"]["heads"][i]["id"].ToString());
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

                    RewardBluePrint rbp1 = new RewardBluePrint();
                    rbp1.name = hc.name;
                    rbp1.rewardType = "Head";
                    rbp1.rewardLevel = hc.level;
                    rbp1.itemId = hc.head_id;

                    rewardsBluePrint.Add(rbp1);
                }

                for (int i = 0; i < json["result"]["larms"].Count; i++)
                {
                    LeftArm lac = new LeftArm();
                    lac.object_id = int.Parse(json["result"]["larms"][i]["id"].ToString());
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

                    RewardBluePrint rbp2 = new RewardBluePrint();
                    rbp2.name = lac.name;
                    rbp2.rewardType = "Left Arm";
                    rbp2.rewardLevel = lac.level;
                    rbp2.itemId = lac.larm_id;

                    rewardsBluePrint.Add(rbp2);
                }

                for (int i = 0; i < json["result"]["rarms"].Count; i++)
                {
                    RightArm rac = new RightArm();
                    rac.object_id = int.Parse(json["result"]["rarms"][i]["id"].ToString());
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

                    RewardBluePrint rbp3 = new RewardBluePrint();
                    rbp3.name = rac.name;
                    rbp3.rewardType = "Right Arm";
                    rbp3.rewardLevel = rac.level;
                    rbp3.itemId = rac.rarm_id;

                    rewardsBluePrint.Add(rbp3);
                }

                for (int i = 0; i < json["result"]["legs"].Count; i++)
                {
                    Leg lc = new Leg();
                    lc.object_id = int.Parse(json["result"]["legs"][i]["id"].ToString());
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

                    RewardBluePrint rbp4 = new RewardBluePrint();
                    rbp4.name = lc.name;
                    rbp4.rewardType = "Leg";
                    rbp4.rewardLevel = lc.level;
                    rbp4.itemId = lc.leg_id;

                    rewardsBluePrint.Add(rbp4);
                }

                for (int i = 0; i < json["result"]["robots"].Count; i++)
                {
                    Robot r = new Robot();
                    r.object_id = int.Parse(json["result"]["robots"][i]["id"].ToString());
                    r.type = json["result"]["robots"][i]["type"].ToString();
                    r.name = json["result"]["robots"][i]["name"].ToString();
                    r.price = float.Parse(json["result"]["robots"][i]["price"].ToString());
                    r.head_object_id = int.Parse(json["result"]["robots"][i]["head_id"].ToString());
                    r.larm_object_id = int.Parse(json["result"]["robots"][i]["larm_id"].ToString());
                    r.rarm_object_id = int.Parse(json["result"]["robots"][i]["rarm_id"].ToString());
                    r.leg_object_id = int.Parse(json["result"]["robots"][i]["leg_id"].ToString());

                    GameManage.User.AddRobot(r);

                    RewardBluePrint rbp4 = new RewardBluePrint();
                    rbp4.name = r.name;
                    rbp4.rewardType = "Robot";
                    rbp4.rewardLevel = r.Level;
                    rbp4.itemId = r.Head.head_id;

                    rewardsBluePrint.Add(rbp4);
                }

                playcs.ShowSubPanel(5);
                playcs.SubPanelList[5].GetComponent<VictoryPanel>().Init(matchTimeInSec, mineAttackCount, mineDefenceCount, rewardsBluePrint);
            }
            else if (status == "fail")
            {
                Debug.LogError("RewardAdventure: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("RewardAdventure: Error");
            Debug.Log(www.error);
        }
        return 0;
    }

    private int callbackRewardMedicament(WWW www)
    {
        LoadingPanel.instance.taskCompleted++;
        if (string.IsNullOrEmpty(www.error))
        {
            JsonData json = JsonMapper.ToObject(www.text);
            string status = json["status"].ToString();
            if (status == "success")
            {
                Debug.Log("RewardMedicament: Done");
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

                playcs.SubPanelList[5].GetComponent<VictoryPanel>().ShowMedicamentMessage();
                playcs.SubPanelList[6].GetComponent<LoosePanel>().ShowMedicamentMessage();
            }
            else if (status == "fail")
            {
                Debug.LogError("RewardMedicament: Failed");
                MobileNative.Alert("Error", "Username or password is wrong.", "OK");
            }
        }
        else
        {
            Debug.Log("RewardMedicament: Error");
            Debug.Log(www.error);
        }
        return 0;
    }
    #endregion
}
