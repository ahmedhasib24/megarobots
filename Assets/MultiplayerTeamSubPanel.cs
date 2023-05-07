using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerTeamSubPanel : MonoBehaviour
{
    public GameObject TeamMemberItemPrefab, TeamInventoryitemPrefab;
    public Transform memberItemsParent, inventoryItemsParent;

    public List<GameObject> teamMemberItems = new List<GameObject>();
    public List<GameObject> inventoryItems = new List<GameObject>();

    public Sprite memberItemSelectedSprite, memberItemDeselectedSprite;

    private List<Robot> team;
    //private Robot[] inventoryArray;
    private List<Robot> inventory;

    private int currentMemberId;
    private int currentInventoryId;

    GameObject Robot;

    public Button btnAddTeamMember, btnRemoveTeamMember; 

    public List<GameObject> headObjects;
    public List<GameObject> larmObjects;
    public List<GameObject> rarmObjects;
    public List<GameObject> legObjects;

    public bool bFirst = true;

    private void Awake()
    {
        Robot = GameObject.Find("Models").transform.Find("Team").gameObject;
    }

    private void OnEnable()
    {
        Robot.SetActive(true);
        if (bFirst)
        {
            Init();
        }
    }

    private void OnDisable()
    {
        Robot.SetActive(false);
    }

    private void Init()
    {
        bFirst = false;

        team = new List<Robot>();
        inventory = new List<Robot>(GameManage.User.Robots);

        for (int i = 0; i < GameManage.User.Robots.Count; i++)
        {
            Robot robot = GameManage.User.Robots[i];
            if ((robot.Head != null && robot.Head.is_broken == 0) && (robot.LeftArm != null && robot.LeftArm.is_broken == 0) && (robot.RightArm != null && robot.RightArm.is_broken == 0) && (robot.Leg != null && robot.Leg.is_broken == 0))
            {
                if (robot.onSale == 0 && robot.Head.onSale == 0 && robot.LeftArm.onSale == 0 && robot.RightArm.onSale == 0 && robot.Leg.onSale == 0)
                {
                    team.Add(robot);
                    inventory.Remove(robot);
                    break;
                }
            }
        }

        InitTeam();
        InitInventory();
    }

    void InitTeam()
    {
        //Init team
        Clear(teamMemberItems);
        teamMemberItems = new List<GameObject>();

        int teamItemCount = 0;
        for (int i = 0; i < team.Count; i++)
        {
            GameObject go = Instantiate(TeamMemberItemPrefab, memberItemsParent);
            TeamMemberRobotItem comp = go.GetComponent<TeamMemberRobotItem>();
            Robot robot = team[i];
            comp.Init(teamItemCount, false, robot, null, null, this);

            teamMemberItems.Add(go);
            teamItemCount++;
        }
        GameManage.instance.myTeam = team;

        //GameManage.instance.myTeam[0].toJson();

        SelectItem(0, false);

        if (team.Count == 0)
        {
            btnRemoveTeamMember.interactable = false;
        }
        else
        {
            btnRemoveTeamMember.interactable = true;
        }
    }

    void InitInventory()
    {
        //Init inventory
        Clear(inventoryItems);
        inventoryItems = new List<GameObject>();

        int inventoryItemCount = 0;
        for (int i = 0; i < inventory.Count; i++)
        {
            Robot robot = inventory[i];
            if ((robot.Head != null && robot.Head.is_broken == 0) && (robot.LeftArm != null && robot.LeftArm.is_broken == 0) && (robot.RightArm != null && robot.RightArm.is_broken == 0) && (robot.Leg != null && robot.Leg.is_broken == 0))
            {
                if (robot.onSale == 0 && robot.Head.onSale == 0 && robot.LeftArm.onSale == 0 && robot.RightArm.onSale == 0 && robot.Leg.onSale == 0)
                {
                    GameObject go = Instantiate(TeamMemberItemPrefab, inventoryItemsParent);
                    TeamMemberRobotItem comp = go.GetComponent<TeamMemberRobotItem>();
                    comp.Init(inventoryItemCount, true, robot, null, null, this);

                    inventoryItems.Add(go);
                    inventoryItemCount++;
                }
            }
        }
        SelectItem(0, true);

        if (inventory.Count == 0 || team.Count == 3)
        {
            btnAddTeamMember.interactable = false;
        }
        else
        {
            btnAddTeamMember.interactable = true;
        }
    }

    private void Clear(List<GameObject> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
        //itemList.Clear();
    }

    public void SelectItem(int id, bool isInventory)
    {
        if (!isInventory)
        {
            for (int i = 0; i < teamMemberItems.Count; i++)
            {
                if (i == id)
                {
                    currentMemberId = i;
                    teamMemberItems[i].GetComponent<TeamMemberRobotItem>().SelectItem();
                    ShowRobot(teamMemberItems[i].GetComponent<TeamMemberRobotItem>().robot);
                }
                else
                {
                    teamMemberItems[i].GetComponent<TeamMemberRobotItem>().DeselectItem();
                }
            }
        }
        else
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (i == id)
                {
                    currentInventoryId = i;
                    inventoryItems[i].GetComponent<TeamMemberRobotItem>().SelectItem();
                    ShowRobot(inventoryItems[i].GetComponent<TeamMemberRobotItem>().robot);
                }
                else
                {
                    inventoryItems[i].GetComponent<TeamMemberRobotItem>().DeselectItem();
                }
            }
        }
    }

    public void OnAddButtonClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        Robot tempRobot = inventory[currentInventoryId];

        team.Add(tempRobot);
        inventory.Remove(tempRobot);

        InitTeam();
        InitInventory();
    }

    public void OnRemoveButtonClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        Robot tempRobot = team[currentMemberId];

        team.Remove(tempRobot);
        inventory.Add(tempRobot);

        InitTeam();
        InitInventory();
    }

    public void SwapBtnClicked()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        Robot tempRobot = team[currentMemberId];

        team[currentMemberId] = inventory[currentInventoryId];
        teamMemberItems[currentMemberId].GetComponent<TeamMemberRobotItem>().Init(currentMemberId, false, team[currentMemberId], null, null, this);
        GameManage.instance.myTeam = team;
        SelectItem(currentMemberId, false);

        inventory[currentInventoryId] = tempRobot;
        inventoryItems[currentInventoryId].GetComponent<TeamMemberRobotItem>().Init(currentMemberId, false, inventory[currentInventoryId], null, null, this);
        SelectItem(currentInventoryId, true);
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
