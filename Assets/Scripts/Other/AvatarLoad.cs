using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;

public class AvatarLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Invoke ("ShowAvatar", 0.5f);


	}

	void ShowAvatar()
	{
		GetComponent<Image> ().sprite = MenuUIManager.instance.AvatarList [GB.g_MyAvatarID];
	}

	public void ShowProfile()
	{
		FindObjectOfType<OptionManager>().ButtonPlay();
		GameObject Panel2 = GameObject.Find ("UI2").transform.Find("18-UserProfileSubPanel").gameObject;
		Panel2.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
	}
}
