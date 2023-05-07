using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager8 : MonoBehaviour {

	public Sprite SelectedSprite;
	public Sprite DeSelectedSprite;

	public List<GameObject> BackImgList = new List<GameObject>();

	int CurIndex = 0;

	// Use this for initialization
	void Start () {

		BackImgList[CurIndex].GetComponent<Image> ().sprite = SelectedSprite;
	}

	// Update is called once per frame
	void Update () {

	}

	public void SelectedObj(int iIndex)
	{
		BackImgList[CurIndex].GetComponent<Image> ().sprite = DeSelectedSprite;

		CurIndex = iIndex;

		BackImgList[CurIndex].GetComponent<Image> ().sprite = SelectedSprite;
	}
}
