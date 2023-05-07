using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;

public class loading : MonoBehaviour {

//	public Image m_loadBG;
	public Image m_loadIcon;
	public Text m_loadText;
	// Update is called once per frame
	void FixedUpdate () {
		m_loadIcon.transform.Rotate (new Vector3 (0, 0, -5f));
	}
//	public void setLoading(Color32 bgColor, string iconName, string txt) {
//		m_loadBG.GetComponent<Image> ().color = bgColor;
//		Texture2D iconTexture = Resources.Load ("Loading/" + iconName) as Texture2D;
//		m_loadIcon.GetComponent<Image> ().sprite = Sprite.Create(iconTexture, new Rect(0, 0, iconTexture.width, iconTexture.height), new Vector2(0.5f, 0.5f));
//		m_loadText.GetComponent<Text> ().text = txt;
//	}
	public void SetText(string txt) {
		m_loadText.GetComponent<Text> ().text = txt;
	}

}
