using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager7 : MonoBehaviour {

	public Sprite SelectedSprite;
	public Sprite DeSelectedSprite;

	int CurIndex = 0;

	// Use this for initialization
	void Start () {
		
		transform.GetChild (CurIndex).Find ("BackImg").gameObject.GetComponent<Image> ().sprite = SelectedSprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectedObj(int iIndex)
	{
		transform.GetChild (CurIndex).Find ("BackImg").gameObject.GetComponent<Image> ().sprite = DeSelectedSprite;

		CurIndex = iIndex;

		transform.GetChild (CurIndex).Find ("BackImg").gameObject.GetComponent<Image> ().sprite = SelectedSprite;
	}
}
