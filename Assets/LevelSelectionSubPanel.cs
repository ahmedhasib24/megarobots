using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionSubPanel : MonoBehaviour
{
    public List<UserLevel> userLevels = new List<UserLevel>();

    public Button btnBattle;
    public List<GameObject> levelParentObjects;

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        userLevels = GameManage.UserLevels;
        for (int i = 0; i < levelParentObjects.Count; i++)
        {
            levelParentObjects[i].SetActive(false);
        }

        for (int i = 0; i < userLevels.Count; i++)
        {
            levelParentObjects[i].SetActive(true);
            levelParentObjects[i].GetComponent<UserLevelItem>().Init(userLevels[i], this);

            if (userLevels[i].passed == 0 && userLevels[i].locked == 0)
            {
                AdventureCS.instance.CurrentLevelIndex = i;
                levelParentObjects[i].GetComponent<UserLevelItem>().SelectLevel();
            }
        }
    }

    public void SelectLevel(int battleId)
    {
        for (int i = 0; i < levelParentObjects.Count; i++)
        {
            if (i == battleId)
            {
                AdventureCS.instance.CurrentLevelIndex = battleId;
                levelParentObjects[i].GetComponent<UserLevelItem>().SelectLevel();
                if (GameManage.User.Robots.Count < GameManage.UserLevels.Find(x => x.battle_id == battleId).OpponentRobots.Count)
                {
                    btnBattle.interactable = false;
                }
                else
                {
                    btnBattle.interactable = true;
                }
            }
            else
            {
                levelParentObjects[i].GetComponent<UserLevelItem>().DeselectLevel();
            }
        }
    }
}
