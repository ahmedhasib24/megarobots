using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour {

	GameObject ProfilePanel;

	// Use this for initialization
	void Start () {

		ProfilePanel = GameObject.Find ("UI2").transform.Find("18-UserProfileSubPanel").gameObject;
	}

	public void ShowProfile()
	{
		ProfilePanel.SetActive (true);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
