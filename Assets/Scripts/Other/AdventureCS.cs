using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdventureCS : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static AdventureCS s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static AdventureCS instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(AdventureCS)) as AdventureCS;
                if (s_Instance == null)
                    Debug.Log("Could not locate an AdventureCS object. \n You have to have exactly one PR_Utility in the scene.");
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
    public List<GameObject> m_AdventureRobots = new List<GameObject>();

    public List<Text> m_RobotName1 = new List<Text>();
    public List<Text> m_RobotName2 = new List<Text>();

    public List<Text> m_Level1 = new List<Text>();
    public List<Text> m_Level2 = new List<Text>();

    public List<Image> m_Abilities1 = new List<Image>();
    public List<Image> m_Abilities2 = new List<Image>();

    public List<Slider> m_AbilitySlider1 = new List<Slider>();
    public List<Slider> m_AbilitySlider2 = new List<Slider>();

    public List<Text> m_AbilityData1 = new List<Text>();
    public List<Text> m_AbilityData2 = new List<Text>();


    public Text m_MyRobotBestName;
    public Text m_MyRobotBestLevel;

    public List<Text> m_MyNameList = new List<Text>();
    public List<Image> m_MyAbilities = new List<Image>();
    public List<Slider> m_MyAbilitySlider = new List<Slider>();
    public List<Text> m_MyAbilityData = new List<Text>();

    public Text m_EnemyRobotBestName;
    public Text m_EnemyRobotBestLevel;

    public List<Text> m_EnemyNameList = new List<Text>();
    public List<Image> m_EnemyAbilities = new List<Image>();
    public List<Slider> m_EnemyAbilitySlider = new List<Slider>();
    public List<Text> m_EnemyAbilityData = new List<Text>();

    public List<GameObject> AdventureLevelList = new List<GameObject>();

    int m_Index;

    public List<GameObject> SubPanelList = new List<GameObject>();

    GameObject Robot;

    bool bFirst = false;

    List<int> m_RobotIndexList = new List<int>();
    int iLevel;

    public GameObject LoadingPanel;

    public int CurrentLevelIndex
    {
        get
        {
            iLevel = PlayerPrefs.GetInt("iLevel", 0);
            return iLevel;
        }
        set
        {
            iLevel = value;
            PlayerPrefs.SetInt("iLevel", iLevel);
        }
    }


    public Sprite userLevelUnLockedSprite, userLevelLockedSprite, userLevelCompleteSprite;
    public Color userLevelUnLockedColor, userLevelLockedColor, userLevelCompleteColor;

    public List<GameObject> headObjects = new List<GameObject>();
    public List<GameObject> larmObjects = new List<GameObject>();
    public List<GameObject> rarmObjects = new List<GameObject>();
    public List<GameObject> legObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        

        bFirst = true;

        //ShowRobot("19");
        //ShowRobot("15");
        //ShowRobot("0");

        //if(!PlayerPrefs.HasKey("iLevel"))
        //{
        //    PlayerPrefs.SetInt("iLevel", 1);           
        //}

        //iLevel = PlayerPrefs.GetInt("iLevel");

        //SetLevel(iLevel);
    }

    public void ShowRobot(Robot robot)
    {
        //Debug.LogError("Showing: " + robot.Head.head_id + robot.LeftArm.larm_id + robot.RightArm.rarm_id + robot.Leg.leg_id);
        //Robot robot = currentRobot = myRobots[id];
        ShowHead(robot.Head.head_id);
        ShowLeftArm(robot.LeftArm.larm_id);
        ShowRightArm(robot.RightArm.rarm_id);
        ShowLeg(robot.Leg.leg_id);
    }

    public void HideAllRobots()
    {
        //Debug.LogError("Hiding all");
        ShowHead(-1);
        ShowLeftArm(-1);
        ShowRightArm(-1);
        ShowLeg(-1);
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

    public void ShowRobot(string m_ID)
    {
        m_Index = int.Parse(m_ID);

        m_RobotIndexList.Add(m_Index);

        if (m_Index < 0) m_Index = 0;
        if (m_Index >= m_AdventureRobots.Count) m_Index = m_AdventureRobots.Count - 1;

        for (int i = 0; i < m_AdventureRobots.Count; i++)
        {
            if (m_Index == i) m_AdventureRobots[i].SetActive(true);
            else m_AdventureRobots[i].SetActive(false);
        }

        ShowInfo();
    }

    void ShowInfo()
    {
        int m_temp1;

        if (m_Index < 15)
        {
            m_RobotName1[0].text = m_AdventureRobots[m_Index].name;
            m_RobotName2[0].text = m_AdventureRobots[m_Index].name;

            m_Level1[0].text = "1";
            m_Level2[0].text = "1";

            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[0].text = m_temp1.ToString();
            m_AbilityData2[0].text = m_temp1.ToString();

            m_Abilities1[0].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[0].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[0].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[0].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[1].text = m_temp1.ToString();
            m_AbilityData2[1].text = m_temp1.ToString();

            m_Abilities1[1].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[1].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[1].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[2].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[2].text = m_temp1.ToString();
            m_AbilityData2[2].text = m_temp1.ToString();

            m_Abilities1[2].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[2].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[2].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[2].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[3].text = m_temp1.ToString();
            m_AbilityData2[3].text = m_temp1.ToString();

            m_Abilities1[3].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[3].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[3].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[3].value = (float)m_temp1 / 100.0f;
        }
        else if (m_Index < 19)
        {
            m_RobotName1[1].text = m_AdventureRobots[m_Index].name;
            m_RobotName2[1].text = m_AdventureRobots[m_Index].name;

            m_Level1[1].text = "1";
            m_Level2[1].text = "1";

            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[4].text = m_temp1.ToString();
            m_AbilityData2[4].text = m_temp1.ToString();

            m_Abilities1[4].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[4].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[4].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[4].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[5].text = m_temp1.ToString();
            m_AbilityData2[5].text = m_temp1.ToString();

            m_Abilities1[5].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[5].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[5].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[5].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[6].text = m_temp1.ToString();
            m_AbilityData2[6].text = m_temp1.ToString();

            m_Abilities1[6].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[6].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[6].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[6].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[7].text = m_temp1.ToString();
            m_AbilityData2[7].text = m_temp1.ToString();

            m_Abilities1[7].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[7].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[7].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[7].value = (float)m_temp1 / 100.0f;
        }
        else if(m_Index < 22)
        {
            m_RobotName1[2].text = m_AdventureRobots[m_Index].name;
            m_RobotName2[2].text = m_AdventureRobots[m_Index].name;

            m_Level1[2].text = "1";
            m_Level2[2].text = "1";

            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[8].text = m_temp1.ToString();
            m_AbilityData2[8].text = m_temp1.ToString();

            m_Abilities1[8].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[8].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[8].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[8].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[9].text = m_temp1.ToString();
            m_AbilityData2[9].text = m_temp1.ToString();

            m_Abilities1[9].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[9].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[9].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[9].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[10].text = m_temp1.ToString();
            m_AbilityData2[10].text = m_temp1.ToString();

            m_Abilities1[10].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[10].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[10].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[10].value = (float)m_temp1 / 100.0f;



            m_temp1 = Random.Range(10, 50);

            m_AbilityData1[11].text = m_temp1.ToString();
            m_AbilityData2[11].text = m_temp1.ToString();

            m_Abilities1[11].fillAmount = (float)m_temp1 / 100.0f;
            m_Abilities2[11].fillAmount = (float)m_temp1 / 100.0f;

            m_AbilitySlider1[11].value = (float)m_temp1 / 100.0f;
            m_AbilitySlider2[11].value = (float)m_temp1 / 100.0f;
        }
        else
        {
        }
    }

    public void SetLevel(int iStep)
    {
        if(iLevel < iStep)
        {
            return;
        }

        iLevel = iStep;

        for(int i = 1; i <= AdventureLevelList.Count; i++)
        {
            AdventureLevelList[i - 1].transform.Find("StarList").gameObject.SetActive(false);
            AdventureLevelList[i - 1].transform.Find("ExpTxt").gameObject.SetActive(false);

            GameObject LevelTxt = AdventureLevelList[i - 1].transform.Find("LevelTxt").gameObject;

            if(i < iLevel)
            {
                Color col = new Color(0xFE, 0x86, 0x81, 0xFF);
                LevelTxt.GetComponent<Text>().color = col;

                LevelTxt.GetComponent<Outline>().enabled = false;
            }else if(i == iLevel)
            {
                LevelTxt.GetComponent<Text>().color = Color.white;

                LevelTxt.GetComponent<Outline>().enabled = true;
            }else
            {
                Color col = new Color(0x4A, 0xC1, 0xFD, 0xFF);
                LevelTxt.GetComponent<Text>().color = col;

                LevelTxt.GetComponent<Outline>().enabled = false;
            }
        }

        switch (iLevel)
        {
            case 1:
                PlayerPrefs.SetInt("AI1", Random.Range(0, 5));
                PlayerPrefs.SetInt("AI2", Random.Range(0, 5));
                PlayerPrefs.SetInt("AI3", Random.Range(0, 5));
                break;
            case 2:
                PlayerPrefs.SetInt("AI1", Random.Range(0, 10));
                PlayerPrefs.SetInt("AI2", Random.Range(0, 10));
                PlayerPrefs.SetInt("AI3", Random.Range(0, 10));
                break;
            case 3:
                PlayerPrefs.SetInt("AI1", Random.Range(0, 15));
                PlayerPrefs.SetInt("AI2", Random.Range(0, 15));
                PlayerPrefs.SetInt("AI3", Random.Range(15, 19));
                break;
            case 4:
                PlayerPrefs.SetInt("AI1", Random.Range(0, 15));
                PlayerPrefs.SetInt("AI2", Random.Range(15, 19));
                PlayerPrefs.SetInt("AI3", Random.Range(15, 19));
                break;
            case 5:
                PlayerPrefs.SetInt("AI1", Random.Range(15, 19));
                PlayerPrefs.SetInt("AI2", Random.Range(15, 19));
                PlayerPrefs.SetInt("AI3", Random.Range(19, 22));
                break;
            case 6:
                PlayerPrefs.SetInt("AI1", Random.Range(15, 19));
                PlayerPrefs.SetInt("AI2", Random.Range(19, 22));
                PlayerPrefs.SetInt("AI3", Random.Range(19, 22));
                break;
            case 7:
                PlayerPrefs.SetInt("AI1", Random.Range(19, 22));
                PlayerPrefs.SetInt("AI2", Random.Range(19, 22));
                PlayerPrefs.SetInt("AI3", Random.Range(22, 25));
                break;
            case 8:
                PlayerPrefs.SetInt("AI1", Random.Range(19, 22));
                PlayerPrefs.SetInt("AI2", Random.Range(22, 25));
                PlayerPrefs.SetInt("AI3", Random.Range(22, 25));
                break;
        }
    }

    void OnEnable()
    {
        Robot = GameObject.Find("Models").transform.Find("Team").gameObject;

        if (SubPanelList.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                SubPanelList.Add(transform.GetChild(i).gameObject);
            }
        }

        if (PlayerPrefs.GetInt("NextLevel", 0) == 1)
        {
            PlayerPrefs.SetInt("NextLevel", 0);
            CurrentLevelIndex = PlayerPrefs.GetInt("CurLev") + 1;
            ShowSubPanel(2);
        }
        else
        {
            ShowSubPanel(0);
        }
    }

    public void CloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        ShowSubPanel(-1);

        MenuUIManager.instance.ShowPanel(0);
    }

    public void SubCloseBtn(int iIndex)
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(iIndex);
    }

    public void StopSearchBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void PlayBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(1);
    }

    public void RobotViewBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(3);
    }

    public void AddRobotBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void RemoveRobotBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void OpponentTeamNextBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(2);
    }

    public void MyTeamNextBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //ShowSubPanel(3);
        PlayerPrefs.SetInt("CurLev", CurrentLevelIndex);

        LoadingPanel.SetActive(true);
        StartCoroutine(LoadSinglePlayerScene());
    }

    IEnumerator LoadSinglePlayerScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("04-SingleGamePlay");
        SceneLoadingBar.instance.Show();

        while (!asyncOperation.isDone)
        {
            SceneLoadingBar.instance.SetProgressBar(asyncOperation.progress);
            yield return null;
        }
    }

    public void RequestBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
    }

    public void ReadyBtn()
    {
        PlayerPrefs.SetInt("My1", m_RobotIndexList[m_RobotIndexList.Count - 3]);
        PlayerPrefs.SetInt("My2", m_RobotIndexList[m_RobotIndexList.Count - 2]);
        PlayerPrefs.SetInt("My3", m_RobotIndexList[m_RobotIndexList.Count - 1]);

        m_MyRobotBestName.text = m_RobotName1[2].text;
        m_MyRobotBestLevel.text = m_Level1[2].text;

        for(int i = 0; i < 4; i++)
        {
            m_MyAbilities[i].fillAmount = m_Abilities1[i + 8].fillAmount;
            m_MyAbilitySlider[i].value = m_AbilitySlider1[i + 8].value;
            m_MyAbilityData[i].text = m_AbilityData1[i + 8].text;
        }

        m_MyNameList[0].text = m_AdventureRobots[PlayerPrefs.GetInt("My1")].name;
        m_MyNameList[1].text = m_AdventureRobots[PlayerPrefs.GetInt("My2")].name;
        m_MyNameList[2].text = m_AdventureRobots[PlayerPrefs.GetInt("My3")].name;



        m_EnemyRobotBestName.text = m_AdventureRobots[PlayerPrefs.GetInt("AI3")].name;
        m_EnemyRobotBestLevel.text = "1";

        int m_temp1;
        for (int i = 0; i < 4; i++)
        {
            m_temp1 = Random.Range(10, 50);
            m_EnemyAbilities[i].fillAmount = (float)m_temp1 / 100.0f;

            m_temp1 = Random.Range(10, 50);
            m_EnemyAbilitySlider[i].value = (float)m_temp1 / 100.0f;

            m_temp1 = Random.Range(10, 50);
            m_EnemyAbilityData[i].text = m_temp1.ToString();
        }

        m_EnemyNameList[0].text = m_AdventureRobots[PlayerPrefs.GetInt("AI1")].name;
        m_EnemyNameList[1].text = m_AdventureRobots[PlayerPrefs.GetInt("AI2")].name;
        m_EnemyNameList[2].text = m_AdventureRobots[PlayerPrefs.GetInt("AI3")].name;



        FindObjectOfType<OptionManager>().ButtonPlay();
        ShowSubPanel(2);
    }
    public void BattleBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        //SceneManager.LoadScene("04-SingleGamePlay");
        ShowSubPanel(1);
    }

    public void ShowSubPanel(int iIndex)
    {
        if (iIndex == 1 || iIndex == 2)
        {
            HideAllRobots();
            Robot.SetActive(true);
            //Robot.SetActive(true);
        }
        else
        {
            HideAllRobots();
            Robot.SetActive(false);
        }
        //Debug.LogError("Showing sub panel: " + iIndex);
        for (int i = 0; i < SubPanelList.Count; i++)
        {
            if (i == iIndex)
                SubPanelList[i].SetActive(true);
            else
                SubPanelList[i].SetActive(false);
        }
    }
}
