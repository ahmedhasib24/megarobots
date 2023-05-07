using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiPlayerCS : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static MultiPlayerCS s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static MultiPlayerCS instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(MultiPlayerCS)) as MultiPlayerCS;
                if (s_Instance == null)
                    Debug.Log("Could not locate an MultiPlayerCS object. \n You have to have exactly one MultiPlayerCS in the scene.");
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

    public List<GameObject> SubPanelList = new List<GameObject> ();

    public List<GameObject> headObjects;
    public List<GameObject> larmObjects;
    public List<GameObject> rarmObjects;
    public List<GameObject> legObjects;

    public Image imgLifeProgress, imgAttackProgress, imgDefenceProgress, imgVelocityProgress;
    public Slider sliderLifeProgress, sliderAttackProgress, sliderDefenceProgress, sliderVelocityProgress;
    public Text txtLifeProgress, txtAttackProgress, txtDefenceProgress, txtVelocityProgress;

    GameObject Panel2;
	GameObject Robot;

    bool bFirst = false;

	// Use this for initialization
	void Start () {

		Panel2 = GameObject.Find ("UI2").transform.Find("09-MultiplayerPanel2").gameObject;
        Robot = GameObject.Find ("Models").transform.Find("Multiplayer").gameObject;

		for (int i = 0; i < transform.childCount; i++) {
			SubPanelList.Add (transform.GetChild (i).gameObject);
		}

		ShowSubPanel (0);
        //PhotonRoomInfoManager.instance.GetAllAvailableRooms();
		bFirst = true;
	}

	void OnEnable()
	{
		if (bFirst) {
			ShowSubPanel (0);
		}
	}

	public void CloseBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();

		ShowSubPanel (-1);

        SubPanelList[0].GetComponent<MultiplayerTeamSubPanel>().bFirst = true;
        MenuUIManager.instance.ShowPanel (0);
	}

	public void SubCloseBtn(int iIndex)
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		ShowSubPanel (iIndex);
	}

	public void StopSearchBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
	}

	public void PlayBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		ShowSubPanel (1);
	}

	public void RobotViewBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		ShowSubPanel (3);
	}

	public void AddRobotBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
	}

	public void RemoveRobotBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
	}

	public void BattleBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		ShowSubPanel (1);
	}

	public void RequestBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
	}

	public void AcceptBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
        PhotonRoomInfoManager.instance.JoinOrCreateRoom();

        LoadingPanel.instance.totalTask++;
        LoadingPanel.instance.Show();
        //GameManage.instance.ShowPanel (7);
    }

	public void DeclineBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		ShowSubPanel (0);
	}

    public void MyTeamNextBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(1);
    }


	public void ShowSubPanel(int iIndex)
	{
        if (iIndex == 1) {
			Panel2.SetActive (true);
			Robot.SetActive (true);

            Robot robot = GameManage.instance.myTeam[0];
            ShowRobot(robot);
            ShowRobotInfo(robot);
		} else {
			Panel2.SetActive (false);
			Robot.SetActive (false);
		}

		for (int i = 0; i < SubPanelList.Count; i++) {
			if (i == iIndex)
				SubPanelList [i].SetActive (true);
			else
				SubPanelList [i].SetActive (false);
		}
	}

    public void ShowRobotInfo(Robot robot)
    {
        imgLifeProgress.fillAmount = (robot.MaxLife() / 5f) / robot.MaxLife();
        imgAttackProgress.fillAmount = (robot.MaxAttack() / 5f) / robot.MaxAttack();
        imgDefenceProgress.fillAmount = (robot.MaxDefence() / 5f) / robot.MaxDefence();
        imgVelocityProgress.fillAmount = (robot.MaxVelocity() / 5f) / robot.MaxVelocity();

        //imgLifeProgress.fillAmount = (this.robot.Life) / this.robot.MaxLife();
        //imgAttackProgress.fillAmount = (this.robot.Attack) / this.robot.MaxAttack();
        //imgDefenceProgress.fillAmount = (this.robot.Defence) / this.robot.MaxDefence();
        //imgVelocityProgress.fillAmount = (this.robot.Velocity) / this.robot.MaxVelocity();

        sliderLifeProgress.value = (robot.MaxLife() / 5f) / robot.MaxLife();
        sliderAttackProgress.value = (robot.MaxAttack() / 5f) / robot.MaxAttack();
        sliderDefenceProgress.value = (robot.MaxDefence() / 5f) / robot.MaxDefence();
        sliderVelocityProgress.value = (robot.MaxVelocity() / 5f) / robot.MaxVelocity();

        txtLifeProgress.text = (robot.MaxLife() / 5f).ToString();
        txtAttackProgress.text = (robot.MaxAttack() / 5f).ToString();
        txtDefenceProgress.text = (robot.MaxDefence() / 5f).ToString();
        txtVelocityProgress.text = (robot.MaxVelocity() / 5f).ToString();
    }

    public void ShowRobot(Robot robot)
    {
        //currentRobot = myRobots[id];
        ShowHead(robot.Head.head_id);
        ShowLeftArm(robot.LeftArm.larm_id);
        ShowRightArm(robot.RightArm.rarm_id);
        ShowLeg(robot.Leg.leg_id);
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
}
