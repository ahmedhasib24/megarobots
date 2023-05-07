using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryCS : MonoBehaviour
{

	GameObject Robot;

	// Use this for initialization
	void Start () {

		Robot = GameObject.Find ("Models").transform.Find("12").gameObject;
		Robot.SetActive (true);

	}

	void OnEnable()
	{
		if (Robot) {
			Robot.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ReturnBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();

		Robot.SetActive (false);

        MenuUIManager.instance.ShowPanel (5);
	}

	public void NextLevelBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();
	}
}
