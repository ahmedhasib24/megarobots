using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PieceType
{
    Head,
    LeftArm,
    RightArm,
    Leg
}

public enum RobotStatus
{
    IDLE,
    MOVE,
    ATTACK,
    DIE
}
public enum RobotType
{
    MINE,
    AI,
    ENEMY
}
namespace Shared
{
    [System.Serializable]
    public struct RobotPart
    {
        public int life;
        public int attack;
        public int defense;
        public int velocity;

        public RobotPart(int life, int attack, int defense, int velocity)
        {
            this.life = 4 * Mathf.Clamp(life, 10, 22);
            this.attack = 4 * Mathf.Clamp(attack, 10, 22);
            this.defense = 4 * Mathf.Clamp(defense, 10, 22);
            this.velocity = 4 * Mathf.Clamp(velocity, 15, 22);
        }

        public void RobotRandom()
        {
            life = 4 * UnityEngine.Random.Range(10, 22);
            attack = 4 * UnityEngine.Random.Range(10, 22);
            defense = 4 * UnityEngine.Random.Range(10, 22);
            velocity = 4 * UnityEngine.Random.Range(15, 22);
        }
    }


    

    

    [Serializable]
    public class Medabot
    {
        public List<Robot> robotList;
        public Medabot()
        {
            robotList = new List<Robot>();
        }
    }

    [Serializable]
    public class RobotHead
    {
        public int part_id;
        public string part_name;
        public string part_attackname;
        public int part_attackforce;
        public int part_defense;
        public int part_life;

        public RobotHead()
        {
            part_id = 0;
            part_name = "";
            part_attackname = "";
            part_attackforce = 4 * UnityEngine.Random.Range(10, 22);
            part_defense = 4 * UnityEngine.Random.Range(10, 22);
            part_life = 4 * UnityEngine.Random.Range(10, 22);
        }
    }

    [Serializable]
    public class RobotLeftArm
    {
        public int part_id;
        public string part_name;
        public string part_attackname;
        public int part_attackforce;
        public int part_defense;
        public int part_life;

        public RobotLeftArm()
        {
            part_id = 0;
            part_name = "";
            part_attackname = "";
            part_attackforce = 4 * UnityEngine.Random.Range(10, 22);
            part_defense = 4 * UnityEngine.Random.Range(10, 22);
            part_life = 4 * UnityEngine.Random.Range(10, 22);
        }
    }

    [Serializable]
    public class RobotRightArm
    {
        public int part_id;
        public string part_name;
        public string part_attackname;
        public int part_attackforce;
        public int part_defense;
        public int part_life;

        public RobotRightArm()
        {
            part_id = 0;
            part_name = "";
            part_attackname = "";
            part_attackforce = 4 * UnityEngine.Random.Range(10, 22);
            part_defense = 4 * UnityEngine.Random.Range(10, 22);
            part_life = 4 * UnityEngine.Random.Range(10, 22);
        }
    }

    [Serializable]
    public class RobotLeg
    {
        public int part_id;
        public string part_name;
        public string part_attackname;
        public int part_attackforce;
        public int part_defense;
        public int part_life;
        public int part_velocity;

        public RobotLeg()
        {
            part_id = 0;
            part_name = "";
            part_attackname = "";
            part_attackforce = 4 * UnityEngine.Random.Range(10, 22);
            part_defense = 4 * UnityEngine.Random.Range(10, 22);
            part_life = 4 * UnityEngine.Random.Range(10, 22);
            part_velocity = 4 * UnityEngine.Random.Range(15, 22);
        }
    }

    [Serializable]
    public class Robot
    {
        public RobotType robot_type;
        public RobotStatus robot_status;       //Seconds

        public int robot_style;
        public int robot_id;
        public string robot_name;

        public Vector3 ori_pos;
        public Vector3 cur_pos;

        public RobotHead robot_head;
        public RobotLeftArm robot_larm;
        public RobotRightArm robot_rarm;
        public RobotLeg robot_leg;

        public int Robot_life
        {
            get
            {
                int value = (robot_head.part_life + robot_larm.part_life + robot_rarm.part_life + robot_leg.part_life) / 4;
                return value;
            }
        }
        public int Robot_attackforce
        {
            get
            {
                int value = (robot_head.part_attackforce + robot_larm.part_attackforce + robot_rarm.part_attackforce + robot_leg.part_attackforce) / 4;
                return value;
            }
        }
        public int Robot_defense
        {
            get
            {
                int value = (robot_head.part_defense + robot_larm.part_defense + robot_rarm.part_defense + robot_leg.part_defense) / 4;
                return value;
            }
        }
        public int Robot_velocity
        {
            get
            {
                int value = robot_leg.part_velocity;
                return value;
            }
        }

        public Robot()
        {
            robot_type = RobotType.MINE;
            robot_status = RobotStatus.IDLE;

            robot_style = 0;
            robot_id = 0;
            robot_name = "";

            ori_pos = Vector3.zero;
            cur_pos = Vector3.zero;

            robot_head = new RobotHead();
            robot_larm = new RobotLeftArm();
            robot_rarm = new RobotRightArm();
            robot_leg = new RobotLeg();
        }
    }

    [Serializable]
    public class DB
    {
        public Medabot m_MineMedabot;
        public Medabot m_EnemyMedabot;

        public DB()
        {
            m_MineMedabot = new Medabot();
            m_EnemyMedabot = new Medabot();
        }
    }

    public class GB : MonoBehaviour
    {

        static public string APPNAME = "MEGAROBOTS";

        static public int g_MyID;
        static public string g_MyNickname;
        static public string g_MyMail;
        static public int g_MyGems;
        static public int g_MyRank;
        static public int g_MyWin;
        static public int g_MyLoss;
        static public int g_MyAvatarID;



        static public bool g_PlayerRoomExist;
        static public int g_PlayerWebRoomID;
        static public string g_PlayerPhotonRoomName;
        static public int g_PlayerMaxCount;
        static public int g_PlayerNowCount;
        static public int g_PlayerCompleteTime;
        static public int g_PlayerOrderTime;
        static public int g_PlayerBettingMoney;
        static public int g_PlayerRoomType;

        static public int g_GraphicQuality;
        static public float g_SFXVolume;
        static public float g_MusicVolume;

        static public string g_BASE_URL = "http://megarobots.fantasticmobilesolution.com/api/";

        //APIPage Name
        static public string g_APIUserLogin = "login";
        static public string g_APIUserRegister = "register";
        static public string g_APIUserForgotPwd = "forget-password";
        static public string g_APIUserChangePwd = "change-password";
        static public string g_APIUserInfo = "get-user-info";
        static public string g_APIUserUpdate1 = "update-user1";
        static public string g_APIUserUpdate2 = "update-user2";
        static public string g_APIUserUpdate3 = "update-user3";
        static public string g_APIGetUserLevels = "get-user-levels";
        static public string g_APIGetPromotionItems = "get-promotion-items";

        static public string g_APIGetBasicRobots = "get-basic-robots";
        static public string g_APIGetAllHeads = "get-all-heads";
        static public string g_APIGetAllLeftArms = "get-all-left-arms";
        static public string g_APIGetAllRightArms = "get-all-right-arms";
        static public string g_APIGetAllLegs = "get-all-legs";



        //Left arm
        static public string g_APIAddLeftArm = "add-left-arm";
        static public string g_APIChangeLeftArm = "change-left-arm";
        static public string g_APIRemoveLeftArm = "remove-left-arm";
        //Right arm
        static public string g_APIAddRightArm = "add-right-arm";
        static public string g_APIChangeRightArm = "change-right-arm";
        static public string g_APIRemoveRightArm = "remove-right-arm";
        //Legs
        static public string g_APIAddLeg = "add-leg";
        static public string g_APIChangeLegs = "change-legs";
        static public string g_APIRemoveLeg = "remove-leg";
        //Head
        static public string g_APIAddHead = "add-head";
        static public string g_APIChangeHead = "change-head";
        static public string g_APIRemoveHead = "remove-head";
        //Robots
        static public string g_APIAddRobot = "add-robot";
        static public string g_APIChangeRobot = "change-robot";
        static public string g_APIRemoveRobot = "remove-robot";

        static public string g_APIGetMallItems = "get-mall-items";
        static public string g_APISellRobotMall = "sell-robot-mall";
        static public string g_APISellPieceMall = "sell-piece-mall";
        static public string g_APIBuyRobotMall = "buy-robot-mall";
        static public string g_APIBuyPieceMall = "buy-piece-mall";
        static public string g_APIBuyRobotPromotion = "buy-robot-promotion";
        static public string g_APIBuyPiecePromotion = "buy-piece-promotion";

        static public string g_APIUpdateUserGems = "update-user-gems";
        static public string g_APIUpdateUserLevel = "update-user-level";
        static public string g_APIRewardAdventure = "reward-adventure";

        static public string g_APICreateRoom = "creat-room";
        static public string g_APIRemoveRoom = "remove-room";

        static public string g_APICreateGameStatus = "create-gamestatus";
        static public string g_APIRemoveGameStatus = "remove-gamestatus";

        static public string g_APIGetRoom = "get-room";
        static public string g_APIGetRoomUsers = "get-room-users";

        static public string g_APIGetUserMoney = "get-user-money";
        static public string g_APIAddUserMoney = "add-user-money";

        static public string g_APIFilterRoomsByUser = "filter-rooms-by-user";
        static public string g_APIFilterRoomsByType = "filter-rooms-by-type";
        static public string g_APIFilterRoomsByRoomID = "filter-rooms-by-roomid";

        static public string g_APIConversionRate = "get-conversion-rate";


        // Alert and Messages
        static public string g_LoadingMessage = "Loading.....";

        static public string g_FWarning = "Warning!";


        static public string g_FLoginFailedEmpty = "All fields are required.";
        static public string g_FLoginFailedWrong = "Invalid email or password.";
        static public string g_FLoginSuccess = "Login successful";

        static public string g_FSignUpFailedEmpty = "All fields are required";
        static public string g_FSignUpSuccess = "Signup successful";

        static public string g_FNicknameEmpty = "Please enter your nickname.";
        static public string g_FNicknameExist = "The Nickname already exists.";

        static public string g_FEmailEmpty = "Please enter your email.";
        static public string g_FEmailInvalid = "Please enter a valid email.";
        static public string g_FEmailExist = "Email exists";

        static public string g_FPasswordEmpty = "Please enter your password.";
        static public string g_FNewPwdEmpty = "Please enter new password.";
        static public string g_FCurrentPasswordAlert = "Current password does not match.";
        static public string g_FPasswordAlert = "Password does not match.";

        static public string g_FVCodeAlert = "Invalid verification code.";
        static public string g_FVCodeSent = "Verification code sent to your email.";
        static public string g_FChangePwdSuccess = "Password has changed.";

        static public string g_FNewNickNameEmpty = "Please enter your new nickname.";
        static public string g_FChangeAvatarSuccess = "Avatar and Nickname have changed.";

        static public string g_FCreateRoomNameEmpty = "Please enter new room name.";
        static public string g_FCreateRoomSuccess = "A new room has been created";
        static public string g_FCreateRoomExist = "The room name already exists.";
        static public string g_FBettingMoneyEmpty = "Please enter the amount of your bet.";
        static public string g_FBettingMoneyZero = "Betting money can't be 0.";

        static public string g_FJoinRoomSuccess = "Join success.";
        static public string g_FJoinRoomFail = "Please create new room for you.";

        static public string g_FInternetTitleAlert = "No Internet Connection";
        static public string g_FNoResultAlert = "No results found.";

        // Prefabs String
        static public string g_pLoading = "Prefabs/pref_loading";
        static public string g_PNoInternet = "Prefabs/pref_nointernet";
        static public string g_PNotification = "Prefabs/pref_notification";
        static public string g_PVisitor = "Prefabs/pref_visitor";

        // Database json file name
        static public string g_DATABASE = "/DB.json";

        static public string CACHE_PATH = Application.persistentDataPath + "/cache/";

        public GB()
        {

        }
    }
}