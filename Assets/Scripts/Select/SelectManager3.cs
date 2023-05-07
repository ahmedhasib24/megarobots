using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager3 : MonoBehaviour {

	public string stButtonName;

	int CurIndex = 0;

	// Use this for initialization
	void Start () {
		
		transform.GetChild(CurIndex).Find("BackImg").gameObject.GetComponent<Image> ().color = Color.green;
		transform.GetChild (CurIndex).Find (stButtonName).gameObject.SetActive (true);
	}

	public void SelectedObj(int iIndex)
	{
		FindObjectOfType<OptionManager>().ButtonPlay();

		transform.GetChild(CurIndex).Find("BackImg").gameObject.GetComponent<Image> ().color = Color.white;
		transform.GetChild (CurIndex).Find (stButtonName).gameObject.SetActive (false);

		CurIndex = iIndex;

		transform.GetChild(CurIndex).Find("BackImg").gameObject.GetComponent<Image> ().color = Color.green;
		transform.GetChild (CurIndex).Find (stButtonName).gameObject.SetActive (true);
	}
}
