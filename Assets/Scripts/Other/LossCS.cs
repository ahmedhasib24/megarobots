using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossCS : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NotBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();

        MenuUIManager.instance.ShowPanel (0);
	}

	public void BuyBtn()
	{
		FindObjectOfType<OptionManager>().ButtonPlay ();

        MenuUIManager.instance.ShowPanel (3);
	}
}
