using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Shared;
using LitJson;
using System;
using System.Globalization;

public class DailyManager : MonoBehaviour {

	public GameObject m_loading;

	private const string FMT = "O";

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void CollectBtn(int gems)
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();

		if (CoreApp.GetInstance.IsInternetConnection (true)) {
			m_loading.SetActive (true);
			StartCoroutine (UpdateDaily (gems));
		}
	}

	/*
		int gems = 0;

		if (!PlayerPrefs.HasKey ("Daily")) {
			gems = 10;
		} else {
			switch (PlayerPrefs.GetInt ("Daily")) {
			case 1:
				gems = 10;
				break;
			case 2:
				gems = 20;
				break;
			case 3:
				gems = 30;
				break;
			}
		}

	*/


	IEnumerator UpdateDaily(int gems) 
	{
		string strUrl = GB.g_BASE_URL + GB.g_APIUserUpdate1;
		Dictionary<string, string> param = new Dictionary<string, string> ();

		param.Add ("user_id", GB.g_MyID.ToString());
		param.Add ("gems", (GB.g_MyGems + gems).ToString());
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
				GB.g_MyGems = GB.g_MyGems + gems;

				for (int i = 0; i < FindObjectOfType<StoreManager>().MyGems.Count; i++) {
					FindObjectOfType<StoreManager>().MyGems[i].text = GB.g_MyGems.ToString ();
				}
			} else {
				MobileNative.Alert ("Error", json ["status"].ToString(), "OK");
			}
		} else {
			MobileNative.Alert (GB.g_FInternetTitleAlert, GB.g_FInternetTitleAlert, "OK");
		}
	}
}
