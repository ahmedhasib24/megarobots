using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserLevelItem : MonoBehaviour
{
    public Image imgLevel;
    public GameObject LevelSelectionObject;
    public Text txtLevel;

    public UserLevel levelConfig;
    private LevelSelectionSubPanel selectionSubPanel;

    public void Init(UserLevel config, LevelSelectionSubPanel subPanel)
    {
        levelConfig = config;
        selectionSubPanel = subPanel;

        if (levelConfig.locked == 1)
        {
            imgLevel.sprite = AdventureCS.instance.userLevelLockedSprite;
            txtLevel.color = AdventureCS.instance.userLevelLockedColor;
        }
        else if (levelConfig.passed == 1 )
        {
            imgLevel.sprite = AdventureCS.instance.userLevelCompleteSprite;
            txtLevel.color = AdventureCS.instance.userLevelCompleteColor;
        }
        else
        {
            imgLevel.sprite = AdventureCS.instance.userLevelUnLockedSprite;
            txtLevel.color = AdventureCS.instance.userLevelUnLockedColor;
        }

        txtLevel.text = "Level " + (config.battle_id + 1);


        DeselectLevel();
    }

    public void SelectLevel()
    {
        
        LevelSelectionObject.SetActive(true);
        imgLevel.sprite = AdventureCS.instance.userLevelUnLockedSprite;
        txtLevel.color = AdventureCS.instance.userLevelUnLockedColor;
    }

    public void DeselectLevel()
    {
        LevelSelectionObject.SetActive(false);
        if (levelConfig.locked == 1)
        {
            imgLevel.sprite = AdventureCS.instance.userLevelLockedSprite;
            txtLevel.color = AdventureCS.instance.userLevelLockedColor;
        }
        else if (levelConfig.passed == 1)
        {
            imgLevel.sprite = AdventureCS.instance.userLevelCompleteSprite;
            txtLevel.color = AdventureCS.instance.userLevelCompleteColor;
        }
        else
        {
            imgLevel.sprite = AdventureCS.instance.userLevelUnLockedSprite;
            txtLevel.color = AdventureCS.instance.userLevelUnLockedColor;
        }
    }

    public void OnClick()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        if (levelConfig.locked == 1)
        {
            return;
        }
        selectionSubPanel.SelectLevel(levelConfig.battle_id);
    }
}
