using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using LitJson;

namespace Shared {
	public class CoreApp : Singleton<CoreApp> {
		
//		//In Library
//		public AA13 visitorApp = new AA13();
//
//		public List<AA13_Room> roomList = new List<AA13_Room> ();
//
//		public AA13_Room newAA13Room = new AA13_Room();
//
//		// Database
//		public DB g_DB = new DB();

		public bool isDownload	= false;

		// Tabs
		public bool g_isLocal = false;
		public int g_selPoiIndex = 0;

		// show alert
		public void ShowAlert(string txt) {
			Debug.Log(txt);
		}

		public List<string> loadedScene = new List<string>();

		// isInternet Connection?
		public bool IsInternetConnection(bool isShowAlert = false) {
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				this.ShowAlert (GB.g_FInternetTitleAlert);
				if(isShowAlert) {
					GameObject tmpNoInternet = Instantiate (Resources.Load<GameObject> (GB.g_PNoInternet));
					tmpNoInternet.transform.SetParent (GameObject.Find("Canvas").transform, false);
				}
				return false;
			}
			return true;
		}

//		public void GoScene( string nextScene) {
//			string curSceneName = GetCurSceneName ();
//			loadedScene.Add(curSceneName);
//			SceneManager.LoadScene (nextScene);
//		}
//
//		public void BackScene() {
//			int nCount = loadedScene.Count;
//			if (nCount == 0)
//				return;
//			string curSceneName = loadedScene [nCount - 1];
//			loadedScene.Remove (curSceneName);
//			SceneManager.LoadScene (curSceneName);
//		}
//
//		public string GetCurSceneName() {
//			return SceneManager.GetActiveScene ().name;
//		}

		// Valid Email
		public bool isValidEmail(string strEmail) {
			var regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
			return regex.IsMatch(strEmail);
		}

		public string LastPathComponent(string strLinkUrl) {
			Uri uri = new Uri(strLinkUrl);
			string filename = System.IO.Path.GetFileName(uri.LocalPath);
			return filename;
		}

		public void CreateDir(string path) {
			if(!Directory.Exists(path))
			{    
				//if it doesn't, create it
				Directory.CreateDirectory(path);
			}
		}

		public void AddLoadinUI( GameObject parent, string lablel = "Loading...") {
			GameObject loadingObj = Instantiate (Resources.Load<GameObject> (GB.g_pLoading));
			loadingObj.GetComponent<loading> ().SetText (lablel);
			loadingObj.transform.SetParent (parent.transform, false);
		}

		void GetDatabase() {
			string path = Application.persistentDataPath + GB.g_DATABASE;
//			Debug.Log (path);
			try {
				if(File.Exists(path)) {
					string jsonTxt = System.IO.File.ReadAllText (path);
					if(jsonTxt != "")
					{
//						g_DB = JsonUtility.FromJson<DB> (jsonTxt);	
					}
				}
			}
			catch(Exception e) {
				ShowAlert (e.ToString ());
			}
		}

		void CreateCacheDir() {
			string dirName = Application.persistentDataPath + "/cache";
			CreateDir (dirName);
		}

		void Start() {
			DontDestroyOnLoad (gameObject);
			CreateCacheDir ();
			GetDatabase ();

		}

		public string GetCachedPath(string url) {
			string filePath = GB.CACHE_PATH + LastPathComponent(url);
			bool useCached = false;
			string pathforwww = url;;
			useCached = System.IO.File.Exists(filePath);
			if (useCached) {
				pathforwww = "file:///" + filePath;
			}
			return pathforwww;
		}

		public void StoreToCache(WWW www, string strPath) {
			if (!File.Exists (strPath)) {
				string fullPath = GB.CACHE_PATH + LastPathComponent(strPath);
				System.IO.File.WriteAllBytes (fullPath, www.bytes);	
			}
		}
		public void ClearToCahce() {
			Directory.Delete (Application.persistentDataPath + "/cache", true);
			Directory.CreateDirectory (Application.persistentDataPath + "/cache");
		}

		public void ShowNotification(string strMsg) {
			GameObject tmpNotification = Instantiate (Resources.Load<GameObject> (GB.g_PNotification));
			tmpNotification.GetComponent<NotificationUI> ().ShowNotification (strMsg);
			tmpNotification.transform.SetParent (GameObject.Find("Canvas").transform, false);
		}

		void Update()
		{
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
		}
	}
}
