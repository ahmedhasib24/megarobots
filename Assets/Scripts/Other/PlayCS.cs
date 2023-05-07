using LitJson;
using Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningMessages
{
    public const string SELECT_YOUR_AND_OPPONENT_ROBOT = "Your and opponent robot not selected";
    public const string SELECT_YOUR_ROBOT = "Your robot not selected";
    public const string SELECT_OPPONENT_ROBOT = "Opponent robot not selected";
    public const string NOT_AVAILABLE_FREE = "Not available in free version. Buy premium version";
    public const string NOT_ENOUGH_MEDAFORCE = "Not enough MEDAFORCE. Charge MEDAFORCE";
    public const string PLAYER_DISCONNECTED = "You left or disconnected";
    public const string OPPONENT_DISCONNECTED = "Opponent left or disconnected";
}

public class PlayCS : MonoBehaviour
{

    public List<GameObject> SubPanelList = new List<GameObject>();

    GameObject Panel2;
    GameObject RobotList;

    public GameObject ActionGroup1;
    public GameObject ActionGroup2;
    public GameObject ActionGroup3;
    public GameObject MFBar;

    public Button fightButton;
    public Image radialTimeSliderImage;

    public Button btnMineHeadSelection, btnMineLeftArmSelection, btnMineRightArmSelection, btnMineLegSelection;
    public Text txtMineHeadCount, txtMineLeftArmCount, txtMineRightArmCount, txtMineLegCount;

    public Button btnHeadSelection, btnLeftArmSelection, btnRightArmSelection, btnLegSelection;
    public Text txtHeadCount, txtLeftArmCount, txtRightArmCount, txtLegCount;

    public Text txtMineEffectname, txtEnemyEffectName;
    public Button btnAutoAttack, btnManualAttack, btnShowStat, btnChargeMF, btnMineLeg;
    public Button btnActionGroup2BackButton;

    public Image mfBarImage;
    public Text mfText;

    public GameSceneWarning warning;

    bool bFirst = false;

    public GameObject loadingPanel;

    int isPaid = 1; //for paid version test purpose

    // Use this for initialization
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        //Panel2 = GameObject.Find("UI2").transform.Find("11-GamePanel2").gameObject;
        ////RobotList = GameObject.Find ("Models").transform.Find("11").gameObject;

        //for (int i = 0; i < Panel2.transform.childCount; i++)
        //{
        //    SubPanelList.Add(Panel2.transform.GetChild(i).gameObject);

        //    //if (i == 0)
        //    //{
        //    //    GameObject firstObj = Panel2.transform.GetChild(i).gameObject;
        //    //    //ActionGroup1 = firstObj.transform.Find("ActionGroup1").gameObject;
        //    //    //ActionGroup2 = firstObj.transform.Find("ActionGroup2").gameObject;
        //    //    //ActionGroup3 = firstObj.transform.Find("ActionGroup3").gameObject;
        //    //}
        //}
        DeactivateRadialTimeSlider();
        //ShowSubPanel (0);

        ShowStatusView(-1);
        txtMineEffectname.text = "";
        txtEnemyEffectName.text = "";
        DeactivateMFBar();
        btnChargeMF.gameObject.SetActive(false);

        bFirst = true;
    }

    public void MatchDraw()
    {
        StartCoroutine(MatchDrawRoutine());
    }

    public void MatchWin()
    {
        StartCoroutine(MatchWinRoutine());
    }

    public void MatchLoose()
    {
        StartCoroutine(MatchLooseRoutine());
    }

    IEnumerator MatchDrawRoutine()
    {
        yield return new WaitForSeconds(2f);
        PlayManage.instance.MatchDraw();
    }

    IEnumerator MatchWinRoutine()
    {
        yield return new WaitForSeconds(2f);
        PlayManage.instance.MatchWin();
    }

    IEnumerator MatchLooseRoutine()
    {
        yield return new WaitForSeconds(2f);
        PlayManage.instance.MatchLoose();
    }

    #region HASIB
    public Text txtInitTimer;
    public Text txtMatchTimer;

    public List<GameObject> statusViews;
    public void SetInitTimerValue(int value)
    {
        txtInitTimer.text = value.ToString();
    }

    public void SetMatchTimerValue(int value)
    {
        string time = string.Format("{0:0}" + ":" + "{1:00}", (int)value / 60, (int)value % 60);
        txtMatchTimer.text = time;
    }

    public void ShowMatchTimer()
    {
        SubPanelList[4].SetActive(true);
    }

    public void ActivateFightButton()
    {
        fightButton.interactable = true;
    }

    public void DeactivateFightButton()
    {
        fightButton.interactable = false;
    }

    public void ActivateRadialTimeSlider()
    {
        radialTimeSliderImage.gameObject.SetActive(true);
    }

    public void DeactivateRadialTimeSlider()
    {
        radialTimeSliderImage.gameObject.SetActive(false);
    }

    public void UpdateRadialTimeSlider(float value, float highestValue)
    {
        radialTimeSliderImage.fillAmount = value / highestValue;
    }

    public void ShowEffectName(string name, bool isMine)
    {
        if (isMine)
        {
            txtMineEffectname.text = name;
            txtEnemyEffectName.text = "";
        }
        else
        {
            txtMineEffectname.text = "";
            txtEnemyEffectName.text = name;
        }
    }

    public void OnFightButtonClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        if (PlayManage.instance.isMultiplayer)
        {
            PhotonUtility.SetPlayerProperties(PhotonNetwork.player, PhotonEnums.Player.AttackSelected, true);
            DeactivateFightButton();
        }
        else
        {
            PlayManage.instance.StartFight();
        }
    }

    public void OnMineSelection()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //btnAutoAttack.interactable = false;
        //btnManualAttack.interactable = false;
        if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
        {
            btnShowStat.interactable = true;
        }
        else
        {
            btnShowStat.interactable = true;
        }

        ActionGroup1.SetActive(true);
        btnChargeMF.gameObject.SetActive(false);
        ActionGroup2.SetActive(false);
        ActionGroup3.SetActive(false);

        ActivateMFBar();
        ShowMFBar(PlayManage.instance.allRobotControllers[PlayManage.instance.mineSelectedId].medaforceValue);
    }

    public void OnEnemySelection()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //btnAutoAttack.interactable = true;
        //btnManualAttack.interactable = true;
        if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
        {
            btnShowStat.interactable = true;
        }
        else
        {
            btnShowStat.interactable = true;
        }
        ActionGroup1.SetActive(true);
        //ConfigureMinePartSelectionButtons();
        ActionGroup2.SetActive(false);
        ActionGroup3.SetActive(false);
    }

    public void OnAutoAttackClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //ConfigureMinePartSelectionButtons();
        //ActionGroup2.SetActive(true);
        //ActionGroup3.SetActive(false);
        if (!PlayManage.instance.isMineSelected)
        {
            ShowWarningMessage(WarningMessages.SELECT_YOUR_AND_OPPONENT_ROBOT);
        }
        else if (!PlayManage.instance.isEnemySelected)
        {
            ShowWarningMessage(WarningMessages.SELECT_OPPONENT_ROBOT);
        }
        else
        {
            PlayManage.instance.SelectAutoAttack();
        }
    }

    public void OnManualAttackClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (!PlayManage.instance.isMineSelected)
        {
            ShowWarningMessage(WarningMessages.SELECT_YOUR_AND_OPPONENT_ROBOT);
        }
        else if (!PlayManage.instance.isEnemySelected)
        {
            ShowWarningMessage(WarningMessages.SELECT_OPPONENT_ROBOT);
        }
        else
        {
            ActionGroup1.SetActive(false);
            ConfigureMinePartSelectionButtons();
            if (PlayManage.instance.allRobotControllers[PlayManage.instance.mineSelectedId].medaforceValue < 100)
            {
                if (!PlayManage.instance.allRobotControllers[PlayManage.instance.mineSelectedId].isLegDestroyed)
                {
                    btnChargeMF.interactable = true;
                }
                else
                {
                    btnChargeMF.interactable = false;
                }
                //btnMineLeg.interactable = false;
            }
            else
            {
                btnChargeMF.interactable = false;
                //btnMineLeg.interactable = true;
            }
            btnChargeMF.gameObject.SetActive(true);
            ActionGroup2.SetActive(true);
            if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
            {
                JustShowOpponentPartSelectionButton();
                ActionGroup3.SetActive(true);
                btnActionGroup2BackButton.gameObject.SetActive(false);
            }
            else
            {
                ActionGroup3.SetActive(false);
                btnActionGroup2BackButton.gameObject.SetActive(true);
            }
        }
    }

    void ConfigureMinePartSelectionButtons()
    {
        RobotController rb = PlayManage.instance.allRobotControllers[PlayManage.instance.mineSelectedId];

        if (rb.isHeadDestroyed)
        {
            btnMineHeadSelection.interactable = false;
        }
        else
        {
            btnMineHeadSelection.interactable = true;
        }

        if (rb.isLeftArmDestroyed)
        {
            btnMineLeftArmSelection.interactable = false;
        }
        else
        {
            btnMineLeftArmSelection.interactable = true;
        }

        if (rb.isRightArmDestroyed)
        {
            btnMineRightArmSelection.interactable = false;
        }
        else
        {
            btnMineRightArmSelection.interactable = true;
        }

        if (rb.isLegDestroyed)
        {
            btnMineLegSelection.interactable = false;
        }
        else
        {
            btnMineLegSelection.interactable = true;
        }

        int activeHeadCount = 0;
        int activeLeftArmCount = 0;
        int activeRightArmCount = 0;
        int activeLegCount = 0;

        for (int i = 0; i < PlayManage.instance.MineActiveRobots.Count; i++)
        {
            RobotController rc = PlayManage.instance.allRobotControllers[i];
            if (!rc.isHeadDestroyed)
            {
                activeHeadCount++;
            }
            if (!rc.isLeftArmDestroyed)
            {
                activeLeftArmCount++;
            }
            if (!rc.isRightArmDestroyed)
            {
                activeRightArmCount++;
            }
            if (!rc.isLegDestroyed)
            {
                activeLegCount++;
            }
        }

        int headAttackCount = rb.headAttackCount;
        int larmAttackCount = rb.larmAttackCount;
        int rarmAttackCount = rb.rarmAttackCount;
        int legAttackCount = rb.legAttackCount;

        txtMineHeadCount.text = headAttackCount + "/5";
        txtMineLeftArmCount.text = larmAttackCount + "/5";
        txtMineRightArmCount.text = rarmAttackCount + "/5";
        txtMineLegCount.text = legAttackCount + "/5";

        if (rb.isHeadDestroyed || headAttackCount == 0)
        {
            btnMineHeadSelection.interactable = false;
            txtMineHeadCount.text = 0 + "/5";
            txtMineHeadCount.color = Color.red;
        }
        else
        {
            btnMineHeadSelection.interactable = true;
            txtMineHeadCount.color = Color.white;
        }

        if (rb.isLeftArmDestroyed || larmAttackCount == 0)
        {
            btnMineLeftArmSelection.interactable = false;
            txtMineLeftArmCount.text = 0 + "/5";
            txtMineLeftArmCount.color = Color.red;
        }
        else
        {
            btnMineLeftArmSelection.interactable = true;
            txtMineLeftArmCount.color = Color.white;
        }

        if (rb.isRightArmDestroyed || rarmAttackCount == 0)
        {
            btnMineRightArmSelection.interactable = false;
            txtMineRightArmCount.text = 0 + "/5";
            txtMineRightArmCount.color = Color.red;
        }
        else
        {
            btnMineRightArmSelection.interactable = true;
            txtMineRightArmCount.color = Color.white;
        }

        if (rb.isLegDestroyed || legAttackCount == 0)
        {
            btnMineLegSelection.interactable = false;
            txtMineLegCount.text = 0 + "/5";
            txtMineLegCount.color = Color.red;
        }
        else
        {
            btnMineLegSelection.interactable = true;
            txtMineLegCount.color = Color.white;
        }

        //txtMineHeadCount.text = activeHeadCount + "/" + PlayManage.instance.MineActiveRobots.Count;
        //txtMineLeftArmCount.text = activeLeftArmCount + "/" + PlayManage.instance.MineActiveRobots.Count;
        //txtMineRightArmCount.text = activeRightArmCount + "/" + PlayManage.instance.MineActiveRobots.Count;
        //txtMineLegCount.text = activeLegCount + "/" + PlayManage.instance.MineActiveRobots.Count;

        
    }

    void JustShowOpponentPartSelectionButton()
    {
        RobotController rb = PlayManage.instance.allRobotControllers[PlayManage.instance.enemySelectedId];

        int activeHeadCount = 0;
        int activeLeftArmCount = 0;
        int activeRightArmCount = 0;
        int activeLegCount = 0;

        for (int i = PlayManage.instance.MineActiveRobots.Count; i < PlayManage.instance.allRobotControllers.Count; i++)
        {
            RobotController rc = PlayManage.instance.allRobotControllers[i];
            if (!rc.isHeadDestroyed)
            {
                activeHeadCount++;
            }
            if (!rc.isLeftArmDestroyed)
            {
                activeLeftArmCount++;
            }
            if (!rc.isRightArmDestroyed)
            {
                activeRightArmCount++;
            }
            if (!rc.isLegDestroyed)
            {
                activeLegCount++;
            }
        }

        int headAttackCount = rb.headAttackCount;
        int larmAttackCount = rb.larmAttackCount;
        int rarmAttackCount = rb.rarmAttackCount;
        int legAttackCount = rb.legAttackCount;

        //txtHeadCount.text = activeHeadCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;
        //txtLeftArmCount.text = activeLeftArmCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;
        //txtRightArmCount.text = activeRightArmCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;
        //txtLegCount.text = activeLegCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;

        txtHeadCount.text = headAttackCount + "/5";
        txtLeftArmCount.text = larmAttackCount + "/5";
        txtRightArmCount.text = rarmAttackCount + "/5";
        txtLegCount.text = legAttackCount + "/5";

        if (rb.isHeadDestroyed || headAttackCount == 0)
        {
            txtHeadCount.text = 0 + "/5";
            txtHeadCount.color = Color.red;
        }
        else
        {
            txtHeadCount.color = Color.white;
        }

        if (rb.isLeftArmDestroyed || larmAttackCount == 0)
        {
            txtLeftArmCount.text = 0 + "/5";
            txtLeftArmCount.color = Color.red;
        }
        else
        {
            txtLeftArmCount.color = Color.white;
        }

        if (rb.isRightArmDestroyed || rarmAttackCount == 0)
        {
            txtRightArmCount.text = 0 + "/5";
            txtRightArmCount.color = Color.red;
        }
        else
        {
            txtRightArmCount.color = Color.white;
        }

        if (rb.isLegDestroyed || legAttackCount == 0)
        {
            txtLegCount.text = 0 + "/5";
            txtLegCount.color = Color.red;
        }
        else
        {
            txtLegCount.color = Color.white;
        }

        btnHeadSelection.interactable = false;
        btnLeftArmSelection.interactable = false;
        btnRightArmSelection.interactable = false;
        btnLegSelection.interactable = false;
    }

    void ConfigureOpponentPartSelectionButtons()
    {
        RobotController rb = PlayManage.instance.allRobotControllers[PlayManage.instance.enemySelectedId];

        if (rb.isHeadDestroyed)
        {
            btnHeadSelection.interactable = false;
        }
        else
        {
            btnHeadSelection.interactable = true;
        }

        if (rb.isLeftArmDestroyed)
        {
            btnLeftArmSelection.interactable = false;
        }
        else
        {
            btnLeftArmSelection.interactable = true;
        }

        if (rb.isRightArmDestroyed)
        {
            btnRightArmSelection.interactable = false;
        }
        else
        {
            btnRightArmSelection.interactable = true;
        }

        if (rb.isLegDestroyed)
        {
            btnLegSelection.interactable = false;
        }
        else
        {
            btnLegSelection.interactable = true;
        }

        int activeHeadCount = 0;
        int activeLeftArmCount = 0;
        int activeRightArmCount = 0;
        int activeLegCount = 0;

        for (int i = PlayManage.instance.MineActiveRobots.Count; i < PlayManage.instance.allRobotControllers.Count; i++)
        {
            RobotController rc = PlayManage.instance.allRobotControllers[i];
            if (!rc.isHeadDestroyed)
            {
                activeHeadCount++;
            }
            if (!rc.isLeftArmDestroyed)
            {
                activeLeftArmCount++;
            }
            if (!rc.isRightArmDestroyed)
            {
                activeRightArmCount++;
            }
            if (!rc.isLegDestroyed)
            {
                activeLegCount++;
            }
        }

        int headAttackCount = rb.headAttackCount;
        int larmAttackCount = rb.larmAttackCount;
        int rarmAttackCount = rb.rarmAttackCount;
        int legAttackCount = rb.legAttackCount;

        //txtHeadCount.text = activeHeadCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;
        //txtLeftArmCount.text = activeLeftArmCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;
        //txtRightArmCount.text = activeRightArmCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;
        //txtLegCount.text = activeLegCount + "/" + PlayManage.instance.EnemyActiveRobots.Count;

        txtHeadCount.text = headAttackCount + "/5";
        txtLeftArmCount.text = larmAttackCount + "/5";
        txtRightArmCount.text = rarmAttackCount + "/5";
        txtLegCount.text = legAttackCount + "/5";

        if (rb.isHeadDestroyed || headAttackCount == 0)
        {
            btnHeadSelection.interactable = false;
            txtHeadCount.text = 0 + "/5";
            txtHeadCount.color = Color.red;
        }
        else
        {
            btnHeadSelection.interactable = true;
            txtHeadCount.color = Color.white;
        }

        if (rb.isLeftArmDestroyed || larmAttackCount == 0)
        {
            btnLeftArmSelection.interactable = false;
            txtLeftArmCount.text = 0 + "/5";
            txtLeftArmCount.color = Color.red;
        }
        else
        {
            btnLeftArmSelection.interactable = true;
            txtLeftArmCount.color = Color.white;
        }

        if (rb.isRightArmDestroyed || rarmAttackCount == 0)
        {
            btnRightArmSelection.interactable = false;
            txtRightArmCount.text = 0 + "/5";
            txtRightArmCount.color = Color.red;
        }
        else
        {
            btnRightArmSelection.interactable = true;
            txtRightArmCount.color = Color.white;
        }

        if (rb.isLegDestroyed || legAttackCount == 0)
        {
            btnLegSelection.interactable = false;
            txtLegCount.text = 0 + "/5";
            txtLegCount.color = Color.red;
        }
        else
        {
            btnLegSelection.interactable = true;
            txtLegCount.color = Color.white;
        }
    }

    public void OnBackButtonClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ActionGroup1.SetActive(true);
        btnChargeMF.gameObject.SetActive(false);
        ActionGroup2.SetActive(false);
        ActionGroup3.SetActive(false);
        PlayManage.instance.minePartId = -1;
        //PlayManage.instance.ToggleEnemyRobotSelection(-1);
    }

    public void HideActionGroups()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ActionGroup1.SetActive(false);
        btnChargeMF.gameObject.SetActive(false);
        ActionGroup2.SetActive(false);
    }

    public void ShowStatusView(int id)
    {
        //int index = 0;
        for (int i = 0; i < statusViews.Count; i++)
        {
            statusViews[i].SetActive(false);
        }

        if (id != -1)
        {
            RobotController rb = PlayManage.instance.allRobotControllers[id];
            //int head = rb.headLife;
            //int lArm = rb.leftArmLife;
            //int rArm = rb.RightArm.life;
            //int leg = rb.Leg.life;


            if (rb.type == RobotType.ENEMY)
            {
                id = (id - GameManage.instance.myTeam.Count) + 3;
                //statusViews[id].GetComponent<StatusView>().Show(rb.robot.Name, rb.headLife, rb.leftArmLife, rb.rightArmLife, rb.legLife, rb.robot.Head.life, rb.robot.LeftArm.life, rb.robot.RightArm.life, rb.robot.Leg.life);
            }
            else
            {
                //statusViews[id].GetComponent<StatusView>().Show(rb.robot.Name, rb.headLife, rb.leftArmLife, rb.rightArmLife, rb.legLife, rb.robot.Head.life, rb.robot.LeftArm.life, rb.robot.RightArm.life, rb.robot.Leg.life);
            }
            for (int i = 0; i < statusViews.Count; i++)
            {
                if (id == i)
                {
                    statusViews[i].SetActive(true);
                }
                else
                {
                    statusViews[i].SetActive(false);
                }
            }
            Debug.Log("Showing status: " + id);
            statusViews[id].GetComponent<StatusView>().Show(rb.robot.Name, rb.headLife, rb.leftArmLife, rb.rightArmLife, rb.legLife, rb.robot.Head.life, rb.robot.LeftArm.life, rb.robot.RightArm.life, rb.robot.Leg.life);
        }
    }

    public void ReturnToMenu()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        if (PlayManage.instance.isMultiplayer)
        {
            PhotonNetwork.LeaveRoom();
            loadingPanel.SetActive(true);
            StartCoroutine(LoadScene("03-Main"));
        }
        else
        {
            loadingPanel.SetActive(true);
            StartCoroutine(LoadScene("03-Main"));
        }
    }

    public void PlayNextLevel()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        if (PlayerPrefs.GetInt("CurLev") < (GameManage.UserLevels.Count - 1))
        {
            PlayerPrefs.SetInt("NextLevel", 1);
            //loadingPanel.SetActive(true);
            StartCoroutine(LoadScene("03-Main"));
        }
        else
        {
            //loadingPanel.SetActive(true);
            StartCoroutine(LoadScene("03-Main"));
        }
    }

    public void Replay()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        loadingPanel.SetActive(true);
        StartCoroutine(LoadScene("04-SingleGamePlay"));
    }

    IEnumerator LoadScene(string sceneName)
    {
        yield return null;

        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        SceneLoadingBar.instance.Show();

        while (!asyncOperation.isDone)
        {
            SceneLoadingBar.instance.SetProgressBar(asyncOperation.progress);
            yield return null;
        }
    }
    #endregion

    #region PREVIOUS
    void OnEnable()
    {
        if (bFirst)
        {
            if (PlayManage.instance.isMultiplayer)
            {
                ShowSubPanel(8);
            }
            else
            {
                ShowSubPanel(0);
            }
        }
    }

    public void CloseBtn(int iIndex)
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();

        ShowSubPanel(-1);

        //GameManage.instance.ShowPanel(iIndex);
    }

    public void SubCloseBtn(int iIndex)
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        ShowSubPanel(iIndex);
        ShowMatchTimer();
    }

    public void ChargeMFBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();

        PlayManage.instance.SelectMinePart(3);
        PlayManage.instance.SelectOpponentPart(3);
        OnBackButtonClick();
    }

    public void SearchStatsBtnMine()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();

        if (PlayerPrefs.GetInt("IsPaid", 0) != isPaid)
        {
            ShowWarningMessage(WarningMessages.NOT_AVAILABLE_FREE);
        }
        else
        {
            if (PlayManage.instance.isMineSelected)
            {
                RobotController rb = PlayManage.instance.allRobotControllers[PlayManage.instance.mineSelectedId];

                ShowSubPanel(2);
                SubPanelList[2].GetComponent<StatusView>().Show(rb.robot.Name, rb.headLife, rb.leftArmLife, rb.rightArmLife, rb.legLife, rb.robot.Head.life, rb.robot.LeftArm.life, rb.robot.RightArm.life, rb.robot.Leg.life);
                //}
            }
            else
            {
                ShowSubPanel(1);
            }
        }
        
    }

    public void SearchStatsBtnEnemy()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        if (PlayManage.instance.isEnemySelected)
        {
            RobotController rb = PlayManage.instance.allRobotControllers[PlayManage.instance.enemySelectedId];

            ShowSubPanel(2);
            SubPanelList[2].GetComponent<StatusView>().Show(rb.robot.Name, rb.headLife, rb.leftArmLife, rb.rightArmLife, rb.legLife, rb.robot.Head.life, rb.robot.LeftArm.life, rb.robot.RightArm.life, rb.robot.Leg.life);
            //}
        }
        else
        {
            ShowSubPanel(1);
        }
    }

    public void SelectOpponentBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();

        if (!PlayManage.instance.isMineSelected)
        {
            ShowWarningMessage(WarningMessages.SELECT_YOUR_ROBOT);
        }
        else
        {
            PlayManage.instance.ToggleEnemyRobotSelection(PlayManage.instance.MineActiveRobots.Count);
        }
    }

    public void ReturnBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();

        ActionGroup1.SetActive(true);
        btnChargeMF.gameObject.SetActive(false);
        ActionGroup2.SetActive(false);
        ActionGroup3.SetActive(false);
        PlayManage.instance.minePartId = -1;
    }

    public void AttachMethodBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        ShowSubPanel(1);
    }

    public void AutoAttackBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        ShowSubPanel(1);
    }

    public void MineHeadBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectMinePart(0);
        if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
        {
            ConfigureOpponentPartSelectionButtons();
            //ActionGroup3.SetActive(true);
        }
        else
        {
            PlayManage.instance.SelectAutoOpponentParts();
            ActionGroup1.SetActive(true);
            btnChargeMF.gameObject.SetActive(false);
            ActionGroup2.SetActive(false);
            ActionGroup3.SetActive(false);
        }
        
        //ActionGroup2.SetActive(true);
        //ActionGroup3.SetActive(true);
        //OnBackButtonClick();
    }

    public void MineRightArmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectMinePart(2);
        if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
        {
            ConfigureOpponentPartSelectionButtons();
            //ActionGroup3.SetActive(true);
        }
        else
        {
            PlayManage.instance.SelectAutoOpponentParts();
            ActionGroup1.SetActive(true);
            btnChargeMF.gameObject.SetActive(false);
            ActionGroup2.SetActive(false);
            ActionGroup3.SetActive(false);
        }
    }

    public void MineLeftArmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectMinePart(1);
        if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
        {
            ConfigureOpponentPartSelectionButtons();
            //ActionGroup3.SetActive(true);
        }
        else
        {
            PlayManage.instance.SelectAutoOpponentParts();
            ActionGroup1.SetActive(true);
            btnChargeMF.gameObject.SetActive(false);
            ActionGroup2.SetActive(false);
            ActionGroup3.SetActive(false);
        }
    }

    public void MineLegBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);

        if (PlayManage.instance.allRobotControllers[PlayManage.instance.mineSelectedId].medaforceValue < 100)
        {
            ShowWarningMessage(WarningMessages.NOT_ENOUGH_MEDAFORCE);
        }
        else
        {
            PlayManage.instance.SelectMinePart(3);
            if (PlayerPrefs.GetInt("IsPaid", 0) == isPaid)
            {
                ConfigureOpponentPartSelectionButtons();
                //ActionGroup3.SetActive(true);
            }
            else
            {
                PlayManage.instance.SelectAutoOpponentParts();
                ActionGroup1.SetActive(true);
                btnChargeMF.gameObject.SetActive(false);
                ActionGroup2.SetActive(false);
                ActionGroup3.SetActive(false);
            }
        }
    }

    public void ActivateMFBar()
    {
        MFBar.SetActive(true);
    }

    public void DeactivateMFBar()
    {
        MFBar.SetActive(false);
    }

    public void ShowMFBar(int mfValue)
    {
        mfBarImage.fillAmount = mfValue / 100f;
        mfText.text = mfValue + "/" + 100;
    }

    public void ShowMFBarWithAnimation(int prevValue, int newValue)
    {
        StartCoroutine(UpdateMFValueOverTime(prevValue, newValue));
    }

    IEnumerator UpdateMFValueOverTime(int prevValue, int newValue)
    {
        float time = 2f;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            Debug.Log(newValue);
            elapsedTime += Time.deltaTime;
            mfBarImage.fillAmount = Mathf.Lerp((prevValue / 100f), ((newValue) / 100f), (elapsedTime / time));
            mfText.text = (int)Mathf.Lerp(prevValue, newValue, (elapsedTime / time)) + "/" + 100;

            yield return null;
        }
    }

    public void HeadBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectOpponentPart(0);
        OnBackButtonClick();
    }

    public void RightArmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectOpponentPart(2);
        OnBackButtonClick();
    }

    public void LeftArmBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectOpponentPart(1);
        OnBackButtonClick();
    }

    public void LegBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay ();
        //SubCloseBtn(0);
        PlayManage.instance.SelectOpponentPart(3);
        OnBackButtonClick();
    }

    public void ShowWarningMessage(string msg)
    {
        warning.Show(msg);
    }
    public void ShowSubPanel(int iIndex)
    {
        //Debug.LogError(iIndex);
        for (int i = 0; i < SubPanelList.Count; i++)
        {
            if (i == iIndex)
                SubPanelList[i].SetActive(true);
            else
                SubPanelList[i].SetActive(false);
        }

        //if (iIndex == -1) {
        //	Panel2.SetActive (false);
        //	//RobotList.SetActive (false);
        //} else {
        //	Panel2.SetActive (true);
        //	//RobotList.SetActive (true);
        //}
    }
    #endregion
}
