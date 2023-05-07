using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentTeamSubPanel : MonoBehaviour
{
    public UserLevel CurrentLevel;
    public GameObject TeamMemberItemPrefab;
    public Transform memberItemsParent;

    public List<GameObject> items = new List<GameObject>();

    public Sprite itemSelected, itemDeselected;
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        CurrentLevel = GameManage.UserLevels.Find(x => x.battle_id == AdventureCS.instance.CurrentLevelIndex);
        //Debug.LogError("CurLev: " + AdventureCS.instance.CurrentLevelIndex);

        GameManage.instance.opponentTeam = new List<Robot>();
        Clear();
        items = new List<GameObject>();
        for (int i = 0; i < CurrentLevel.OpponentRobots.Count; i++)
        {
            GameObject go = Instantiate(TeamMemberItemPrefab, memberItemsParent);
            TeamMemberRobotItem comp = go.GetComponent<TeamMemberRobotItem>();
            Robot robot = CurrentLevel.OpponentRobots[i];
            comp.Init(i, false, robot, this, null, null);

            GameManage.instance.opponentTeam.Add(robot);
            items.Add(go);
        }
        SelectItem(0);
    }

    private void Clear()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i]);
        }
    }

    public void SelectItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (i == id)
            {
                items[i].GetComponent<TeamMemberRobotItem>().SelectItem();
                AdventureCS.instance.ShowRobot(items[i].GetComponent<TeamMemberRobotItem>().robot);
            }
            else
            {
                items[i].GetComponent<TeamMemberRobotItem>().DeselectItem();
            }
        }


    }
}
