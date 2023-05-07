using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;

public class UserNameLoad : MonoBehaviour 
{
	void OnEnable()
	{
		GetComponent<Text> ().text = GB.g_MyNickname;
	}

	public void ShowProfile()
	{
		FindObjectOfType<OptionManager>().ButtonPlay();
		GameObject Panel2 = GameObject.Find ("UI2").transform.Find("18-UserProfileSubPanel").gameObject;
		Panel2.SetActive (true);
	}


}
