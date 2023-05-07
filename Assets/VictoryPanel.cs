using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardBluePrint
{
    public string name;
    public string rewardType;
    public int rewardLevel;
    public int itemId;
}

public class VictoryPanel : MonoBehaviour
{
    public Text txtTime, txtAttackCount, txtDefenceCount, txtMessage, txtVideoAd;

    public Button watchVideoButton;

    public List<GameObject> rewardUiItemList;

    public void Init(int time, int attackCount, int defenceCount, List<RewardBluePrint> rewardsBluePrint)
    {
        //Advertisement.Instance.HideBannerAd();
        txtTime.text = string.Format("{0:0}" + ":" + "{1:00}", (int)time / 60, (int)time % 60);
        txtAttackCount.text = attackCount.ToString();
        txtDefenceCount.text = defenceCount.ToString();

        //Init reward pieces here
        for (int i = 0; i < rewardUiItemList.Count; i++)
        {
            if (i < rewardsBluePrint.Count)
            {
                rewardUiItemList[i].SetActive(true);
                rewardUiItemList[i].GetComponent<VictoryPanelRewardUIItem>().Init(rewardsBluePrint[i].name, rewardsBluePrint[i].rewardType, rewardsBluePrint[i].rewardLevel, rewardsBluePrint[i].itemId);
            }
            else
            {
                rewardUiItemList[i].SetActive(false);
            }
        }

        if (PlayManage.instance.minePartsDestroyed > 0)
        {
            // if (Advertisement.Instance.IsRewardedAdReady())
            // {
            //     watchVideoButton.gameObject.SetActive(true);
            //     txtVideoAd.text = "Watch " + PlayManage.instance.videoAdCount + " Videos";
            // }
        }
        else
        {
            watchVideoButton.gameObject.SetActive(false);
        }
        txtMessage.gameObject.SetActive(false);
    }

    public void OnVideoAdButton()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();
        //Advertisement.Instance.ShowVideoAd();
    }

    public void WatchedVideoAd()
    {
        if (PlayManage.instance.videoAdCount == 0)
        {
            watchVideoButton.gameObject.SetActive(false);
        }
        else
        {
            txtVideoAd.text = "Watch " + PlayManage.instance.videoAdCount + " Videos";
        }
    }

    public void ShowMedicamentMessage()
    {
        StartCoroutine(ShowMessageRoutine());
    }

    IEnumerator ShowMessageRoutine()
    {
        txtMessage.text = "You got 1 medicament!";
        txtMessage.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        txtMessage.gameObject.SetActive(false);
    }
}
