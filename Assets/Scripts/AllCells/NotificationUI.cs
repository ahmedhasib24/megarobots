using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;
public class NotificationUI : MonoBehaviour {

	public GameObject m_Message;
	public Text m_appNameTxt;
	public void OnClickNotification() {
		Destroy (gameObject);
	}
	public void ShowNotification(string strMsgText) {
		m_Message.transform.Find ("Msg").GetComponent<Text> ().text = strMsgText;
		m_appNameTxt.text = GB.APPNAME;
		m_Message.SetActive (true);

	}
}
