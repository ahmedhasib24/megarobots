using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectManager1 : MonoBehaviour {

	int CurIndex = 0;

	// Use this for initialization
	void Start () {
		
		//transform.GetChild(CurIndex).Find("BackImg").gameObject.GetComponent<Image> ().color = Color.green;
	}

    private void OnEnable()
    {
        //SelectedObj(0);
    }

    public void SelectedObj(int iIndex)
	{
		transform.GetChild(CurIndex).Find("BackImg").gameObject.GetComponent<Image> ().color = Color.white;

		CurIndex = iIndex;

		transform.GetChild(CurIndex).Find("BackImg").gameObject.GetComponent<Image> ().color = Color.green;
        Debug.LogError("Select1 : " + CurIndex);
        //FindObjectOfType<MallManager>().ShowRobot(iIndex.ToString());
	}
}
