using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mall : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static Mall s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static Mall instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(Mall)) as Mall;
                if (s_Instance == null)
                    Debug.Log("Could not locate an Mall object. \n You have to have exactly one PR_Utility in the scene.");
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

    public Text txtName, txtLife, txtAttack, txtDefence, txtVelocity, txtNothingRobot, txtNothingPiece;
    public Transform robotListParent, pieceListParent;
    public GameObject mallRobotItemPrefab, mallPieceItemPrefab;

    public SelectManager1 selectManager1;
    public SelectManager2 selectManager2;

    public List<Toggle> TabList = new List<Toggle>();
    public List<GameObject> ObjList1 = new List<GameObject>();
    public List<GameObject> ObjList2 = new List<GameObject>();

    public GameObject Panel2;
    public GameObject Robot;
    public Button btnBuy;

    public List<GameObject> headObjects = new List<GameObject>();
    public List<GameObject> leftArmObjects = new List<GameObject>();
    public List<GameObject> rightArmObjects = new List<GameObject>();
    public List<GameObject> legObjects = new List<GameObject>();

    private List<GameObject> robotItems = new List<GameObject>();
    private List<GameObject> pieceItems = new List<GameObject>();

    private RobotMallItem selectedRobotItem;
    private PieceMallItem selectedPieceItem;

    // Use this for initialization
    void Start()
    {
        //SelectTab ();
    }

    public void SelectTab(int id)
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        SelectTabInternal(id);
    }

    private void SelectTabInternal(int id)
    {
        for (int i = 0; i < TabList.Count; i++)
        {
            ObjList1[i].SetActive(TabList[i].isOn);
            ObjList2[i].SetActive(TabList[i].isOn);
        }

        if (TabList[0].isOn)
        {
            InitRobotList();
        }
        else if (TabList[1].isOn)
        {
            InitPieceList();
        }
    }

    void HighlightRobotItem(int serialNumber)
    {
        for (int i = 0; i < robotItems.Count; i++)
        {
            if (serialNumber == i)
            {
                robotItems[i].GetComponent<MallRobotItem>().Highlight();

            }
            else
            {
                robotItems[i].GetComponent<MallRobotItem>().Downplay();
            }
        }
    }

    void HighlightPieceItem(int serialNumber)
    {
        for (int i = 0; i < pieceItems.Count; i++)
        {
            if (serialNumber == i)
            {
                pieceItems[i].GetComponent<MallPieceItem>().Highlight();
            }
            else
            {
                pieceItems[i].GetComponent<MallPieceItem>().Downplay();
            }
        }
    }

    void OnEnable()
    {
        txtName.text = "";
        //Configure mall list
        CLearList(robotItems);
        CLearList(pieceItems);
        Robot.SetActive(true);
        ShowHead(-1);
        ShowLeftArm(-1);
        ShowRightArm(-1);
        ShowLeg(-1);

        RobotMallManager.instance.RefreshMallItems();
    }

    void OnDisable()
    {
        if (Panel2)
        {
            Panel2.SetActive(false);
        }

        ShowHead(-1);
        ShowLeftArm(-1);
        ShowRightArm(-1);
        ShowLeg(-1);
        Robot.SetActive(false);
    }

    public void ConfigureMallItems()
    {
        SelectTabInternal(0);
    }

    void InitRobotList()
    {
        CLearList(robotItems);
        robotItems = new List<GameObject>();
        for (int i = 0; i < RobotMallManager.instance.robotMallItems.Count; i++)
        {
            GameObject go = Instantiate(mallRobotItemPrefab, robotListParent);
            MallRobotItem item = go.GetComponent<MallRobotItem>();
            RobotMallItem rmi = RobotMallManager.instance.robotMallItems[i];
            item.Init(i, rmi);
            robotItems.Add(go);
        }
        if (robotItems.Count > 0)
        {
            robotItems[0].GetComponent<MallRobotItem>().ShowItemInternal();
            Panel2.SetActive(true);
            txtNothingRobot.gameObject.SetActive(false);
            btnBuy.interactable = true;
        }
        else
        {
            Panel2.SetActive(false);
            txtNothingRobot.gameObject.SetActive(true);
            btnBuy.interactable = false;
            ShowHead(-1);
            ShowLeftArm(-1);
            ShowRightArm(-1);
            ShowLeg(-1);
        }
    }

    void InitPieceList()
    {
        CLearList(pieceItems);
        pieceItems = new List<GameObject>();
        for (int i = 0; i < RobotMallManager.instance.pieceMallItems.Count; i++)
        {
            GameObject go = Instantiate(mallPieceItemPrefab, pieceListParent);
            MallPieceItem item = go.GetComponent<MallPieceItem>();
            PieceMallItem pmi = RobotMallManager.instance.pieceMallItems[i];
            item.Init(i, pmi);
            pieceItems.Add(go);
        }
        if (pieceItems.Count > 0)
        {
            pieceItems[0].GetComponent<MallPieceItem>().ShowItemInternal();
            Panel2.SetActive(true);
            txtNothingPiece.gameObject.SetActive(false);
            btnBuy.interactable = true;
        }
        else
        {
            Panel2.SetActive(false);
            txtNothingPiece.gameObject.SetActive(true);
            btnBuy.interactable = false;
            ShowHead(-1);
            ShowLeftArm(-1);
            ShowRightArm(-1);
            ShowLeg(-1);
        }
    }

    void CLearList(List<GameObject> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
    }

    public void ShowRobotDetails(int serialNum, RobotMallItem rmi)
    {
        ShowHead(rmi.head_id);
        ShowLeftArm(rmi.larm_id);
        ShowRightArm(rmi.rarm_id);
        ShowLeg(rmi.leg_id);

        txtName.text = rmi.name.ToString();
        txtLife.text = rmi.life.ToString();
        txtAttack.text = rmi.attack.ToString();
        txtDefence.text = rmi.defence.ToString();
        txtVelocity.text = rmi.velocity + ("m/s");

        selectedRobotItem = rmi;
        HighlightRobotItem(serialNum);
    }

    public void ShowPieceDetails(int serialNum, PieceMallItem pmi)
    {
        switch (pmi.piece_type)
        {
            case PieceType.Head:
                ShowHead(pmi.piece_id);
                ShowLeftArm(-1);
                ShowRightArm(-1);
                ShowLeg(-1);
                break;
            case PieceType.LeftArm:
                ShowHead(-1);
                ShowLeftArm(pmi.piece_id);
                ShowRightArm(-1);
                ShowLeg(-1);
                break;
            case PieceType.RightArm:
                ShowHead(-1);
                ShowLeftArm(-1);
                ShowRightArm(pmi.piece_id);
                ShowLeg(-1);
                break;
            case PieceType.Leg:
                ShowHead(-1);
                ShowLeftArm(-1);
                ShowRightArm(-1);
                ShowLeg(pmi.piece_id);
                break;
        }
        txtName.text = pmi.name.ToString();
        txtLife.text = pmi.life.ToString();
        txtAttack.text = pmi.attack.ToString();
        txtDefence.text = pmi.defence.ToString();
        txtVelocity.text = pmi.velocity + ("m/s");

        selectedPieceItem = pmi;
        HighlightPieceItem(serialNum);
    }

    private void ShowHead(int id)
    {
        for (int i = 0; i < headObjects.Count; i++)
        {
            if (i == id)
            {
                headObjects[i].SetActive(true);
            }
            else
            {
                headObjects[i].SetActive(false);
            }
        }
    }

    private void ShowLeftArm(int id)
    {
        for (int i = 0; i < leftArmObjects.Count; i++)
        {
            if (i == id)
            {
                leftArmObjects[i].SetActive(true);
            }
            else
            {
                leftArmObjects[i].SetActive(false);
            }
        }
    }

    private void ShowRightArm(int id)
    {
        for (int i = 0; i < rightArmObjects.Count; i++)
        {
            if (i == id)
            {
                rightArmObjects[i].SetActive(true);
            }
            else
            {
                rightArmObjects[i].SetActive(false);
            }
        }
    }

    private void ShowLeg(int id)
    {
        for (int i = 0; i < legObjects.Count; i++)
        {
            if (i == id)
            {
                legObjects[i].SetActive(true);
            }
            else
            {
                legObjects[i].SetActive(false);
            }
        }
    }

    public void OnBuyButtonClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        if (TabList[0].isOn)
        {
            BuyRobot();
        }
        else if (TabList[1].isOn)
        {
            BuyPiece();
        }
    }

    public void CloseBtn()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        MenuUIManager.instance.ShowPanel(4);
    }

    private void BuyRobot()
    {
        if (selectedRobotItem.seller_id == 0)
        {
            BuyRobotPromotion();
        }
        else
        {
            BuyRobotMall();
        }
    }

    private void BuyPiece()
    {
        if (selectedPieceItem.seller_id == 0)
        {
            BuyPiecePromotion();
        }
        else
        {
            BuyPieceMall();
        }
    }

    private void BuyRobotMall()
    {
        RobotMallManager.instance.BuyRobotMall(selectedRobotItem);
    }

    private void BuyPieceMall()
    {
        RobotMallManager.instance.BuyPieceMall(selectedPieceItem);
    }

    private void BuyRobotPromotion()
    {
        RobotMallManager.instance.BuyRobotPromotion(selectedRobotItem);
    }

    private void BuyPiecePromotion()
    {
        RobotMallManager.instance.BuyPiecePromotion(selectedPieceItem);
    }
}
