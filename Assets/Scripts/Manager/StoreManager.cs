using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Shared;
using LitJson;
using System;
using System.Globalization;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    private void Awake()
    {
        instance = this;
    }


    public GameObject m_loading;
	public List<Text> MyGems = new List<Text>();

	[SerializeField] public GameObject DailyPanel;
	[SerializeField] private List<GameObject> ShineList = new List<GameObject> ();

	private const string FMT = "O";

	// Use this for initialization
	void Start ()
    {	
		DailyPanel.SetActive (false);
	}

    public void CheckAndOpenDailyPanel()
    {
        //if (!PlayerPrefs.HasKey("Daily"))
        //{

        //    if (PlayerPrefs.GetInt("Daily") < 2)
        //    {

        //        ShineList[0].GetComponent<Button>().interactable = true;

        //    }
        //    else
        //    {

        //        ShineList[PlayerPrefs.GetInt("Daily") - 1].GetComponent<Button>().interactable = true;
        //    }

        //    PlayerPrefs.SetInt("GetBonus", 1);

        //}
        //else
        //{

        //    if (PlayerPrefs.HasKey("Time"))
        //    {
        //        CheckDaily();
        //    }
        //}
        //DailyPanel.SetActive(true);
        DailyPanel.GetComponent<Daily>().OnInitialize();
    }

	public void CheckDaily()
	{
		DateTime CurTime = DateTime.Now;
		DateTime lastRewardTime = DateTime.ParseExact(PlayerPrefs.GetString("Time"), FMT, CultureInfo.InvariantCulture);

		TimeSpan diff = CurTime - lastRewardTime;

		//Debug.Log("Last claim was " + (long)diff.TotalHours + " hours ago.");

		int days = (int)(Math.Abs(diff.TotalHours) / 24);

		if (days == 0)
		{
			// No claim for you. Try tomorrow
			return;
		}

		// The player can only claim if he logs between the following day and the next.
		if (days >= 1 && days < 2)
		{
			if (PlayerPrefs.GetInt ("GetBonus" + PlayerPrefs.GetInt("Daily")) == 0) {
				// If reached the last reward, resets to the first restarting the cicle
				if (PlayerPrefs.GetInt ("Daily") == 7) {
					PlayerPrefs.SetInt ("Daily", 0);
				} else {
					PlayerPrefs.SetInt ("Daily", PlayerPrefs.GetInt ("Daily") + 1);
				}

				PlayerPrefs.SetInt ("GetBonus" + PlayerPrefs.GetInt("Daily"), 1);
			}

			if (PlayerPrefs.GetInt ("Daily") < 2) {

				ShineList [0].GetComponent<Button> ().interactable = true;

			} else {

				ShineList [PlayerPrefs.GetInt ("Daily") - 1].GetComponent<Button> ().interactable = true;
			}

			DailyPanel.SetActive (true);

			return;
		}

		if (days >= 2)
		{
			// The player loses the following day reward and resets the prize
			PlayerPrefs.SetInt ("Daily", 1);

			ShineList [0].GetComponent<Button> ().interactable = true;

			DailyPanel.SetActive (true);

			return;
		}
	}

    

	public void BuyGems(int gem)
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		if (CoreApp.GetInstance.IsInternetConnection (true)) {
			m_loading.SetActive (true);
			StartCoroutine (UpdateGems (gem));
		}
	}

	public void GetBonus(int gem)
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		if (CoreApp.GetInstance.IsInternetConnection (true)) {
			m_loading.SetActive (true);
			StartCoroutine (UpdateGems (gem));
		}
	}

	public void CloseBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
        MenuUIManager.instance.ShowPanel (0);
	}

	public void ShowMall()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		GameObject Panel2 = GameObject.Find ("UI2").transform.Find("19-DailyPanel").gameObject;
		Panel2.SetActive (false);
        MenuUIManager.instance.ShowPanel (3);
	}

	IEnumerator UpdateGems(int gem) 
	{
		string strUrl = GB.g_BASE_URL + GB.g_APIUserUpdate1;
		Dictionary<string, string> param = new Dictionary<string, string> ();

		param.Add ("user_id", GB.g_MyID.ToString());
		param.Add ("gems", (GB.g_MyGems + gem).ToString());
		param.Add ("rank", GB.g_MyRank.ToString());
		param.Add ("avatar_id", GB.g_MyAvatarID.ToString());

		WWWForm form = new WWWForm(); 
		foreach (KeyValuePair<string, string> post_arg in param) 
		{ 
			form.AddField(post_arg.Key, post_arg.Value); 
		}
		WWW www = new WWW (strUrl, form);
		yield return www;

		if (www.error == null) {

			m_loading.SetActive (false);
			JsonData json = JsonMapper.ToObject (www.text);

			string status = json ["status"].ToString ();
			if (status == "success") {
				GB.g_MyGems = GB.g_MyGems + gem;

				for (int i = 0; i < MyGems.Count; i++) {
					MyGems[i].text = GB.g_MyGems.ToString ();
				}
			} else {
				MobileNative.Alert ("Error", json ["status"].ToString(), "OK");
			}
		}
        else
        {
            if (!MB_Configs.bDummyData)
            {
                MobileNative.Alert(GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
            }
            else
            {
                GameManage.User.gems += gem;
                PlayerPrefs.SetInt("GetBonus" + PlayerPrefs.GetInt("Daily"), 1);
                StoreManager.instance.DailyPanel.SetActive(false);
            }
		}
	}


}
