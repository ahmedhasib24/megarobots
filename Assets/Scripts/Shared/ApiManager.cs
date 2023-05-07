using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared {
	public class ApiManager : Singleton<ApiManager> {
		public WWW GET(string url) 
		{ 
			WWW www = new WWW(url); 
			StartCoroutine(WaitForRequest(www)); 
			return www; 
		} 

		public WWW POST(string url, Dictionary<string, string> post) 
		{ 
			WWWForm form = new WWWForm(); 
			foreach (KeyValuePair<string, string> post_arg in post) 
			{ 
				form.AddField(post_arg.Key, post_arg.Value); 
			} 

			WWW www = new WWW(url, form); 
			StartCoroutine(WaitForRequest(www)); 
			return www; 
		} 
			
		private IEnumerator WaitForRequest(WWW www) 
		{ 
			yield return www; 
			// check for errors 
			if (www.error == null) 
			{ 
//				Debug.Log("ApI Manager Ok!: " + www.text); 
			} 
			else 
			{ 
				Debug.Log("Api Manager Error: " + www.error); 
			} 
		} 
	}
}