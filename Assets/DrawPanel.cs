using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPanel : MonoBehaviour
{
    public Text txtMessage, txtVideoAd;

    public Button watchVideoButton;

    public void Init()
    {
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
