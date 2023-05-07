using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCashCS : MonoBehaviour {

	public GameObject MaskObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable()
	{
		MaskObj.SetActive (true);
	}

	void OnDisable()
	{
		MaskObj.SetActive (false);
	}
}
