using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OKBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
        Application.Quit();
	}

	public void NOBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
		gameObject.SetActive (false);
	}
}
