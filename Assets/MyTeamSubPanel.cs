using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTeamSubPanel : MonoBehaviour
{
    public UserLevel CurrentLevel;
    public GameObject TeamMemberItemPrefab /*, TeamInventoryitemPrefab*/;
    public Transform memberItemsParent, inventoryItemsParent;

    public List<GameObject> teamMemberItems = new List<GameObject>();
    public List<GameObject> inventoryItems = new List<GameObject>();

    public Sprite memberItemSelectedSprite, memberItemDeselectedSprite;

    private List<Robot> team;
    private Robot[] inventoryArray;
    private List<Robot> inventory;

    private int currentMemberId;
    private int currentInventoryId;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        CurrentLevel = GameManage.UserLevels.Find(x => x.battle_id == AdventureCS.instance.CurrentLevelIndex);
        //Debug.LogError("CurLev: " + AdventureCS.instance.CurrentLevelIndex);
        //Debug.LogError("CurLev: " + CurrentLevel.OpponentRobots.Count);

        team = new List<Robot>();
        inventory = new List<Robot>(GameManage.User.Robots);


        //GameManage.User.Robots.CopyTo(inventoryArray);

        //inventory = inventoryArray

        //Init team
        Clear(teamMemberItems);
        teamMemberItems = new List<GameObject>();

        int teamItemCount = 0;
        for (int i = 0; i < CurrentLevel.OpponentRobots.Count; i++)
        {
            Robot robot = GameManage.User.Robots[i];
            if ((robot.Head != null && robot.Head.is_broken == 0) && (robot.LeftArm != null && robot.LeftArm.is_broken == 0) && (robot.RightArm != null && robot.RightArm.is_broken == 0) && (robot.Leg != null && robot.Leg.is_broken == 0))
            {
                if (robot.onSale == 0 && robot.Head.onSale == 0 && robot.LeftArm.onSale == 0 && robot.RightArm.onSale == 0 && robot.Leg.onSale == 0)
                {
                    GameObject go = Instantiate(TeamMemberItemPrefab, memberItemsParent);
                    TeamMemberRobotItem comp = go.GetComponent<TeamMemberRobotItem>();
                    comp.Init(teamItemCount, false, robot, null, this, null);

                    team.Add(robot);
                    inventory.Remove(robot);
                    teamItemCount++;
                    //Debug.LogError(GameManage.User.Robots.Count);

                    teamMemberItems.Add(go);
                }
            }
        }

        
        GameManage.instance.myTeam = team;
        SelectItem(0, false);

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
                    comp.Init(inventoryItemCount, true, robot, null, this, null);

                    inventoryItems.Add(go);
                    inventoryItemCount++;
                }
            }
        }
        SelectItem(0, true);
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
                    AdventureCS.instance.ShowRobot(teamMemberItems[i].GetComponent<TeamMemberRobotItem>().robot);
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
                    AdventureCS.instance.ShowRobot(inventoryItems[i].GetComponent<TeamMemberRobotItem>().robot);
                }
                else
                {
                    inventoryItems[i].GetComponent<TeamMemberRobotItem>().DeselectItem();
                }
            }
        }
    }

    public void SwapBtnClicked()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        Robot tempRobot = team[currentMemberId];

        team[currentMemberId] = inventory[currentInventoryId];
        teamMemberItems[currentMemberId].GetComponent<TeamMemberRobotItem>().Init(currentMemberId, false, team[currentMemberId], null, this, null);
        GameManage.instance.myTeam = team;
        SelectItem(currentMemberId, false);

        inventory[currentInventoryId] = tempRobot;
        inventoryItems[currentInventoryId].GetComponent<TeamMemberRobotItem>().Init(currentInventoryId, true, inventory[currentInventoryId], null, this, null);
        SelectItem(currentInventoryId, true);
    }
}
