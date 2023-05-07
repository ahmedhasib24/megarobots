using UnityEngine;
using System.Collections;

namespace Shared
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;
		
		public static T GetInstance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType(typeof(T)) as T;
				}
				
				if (_instance == null)
				{
					GameObject obj = new GameObject("_instance_" + typeof(T).ToString());
					_instance = obj.AddComponent(typeof(T)) as T;
				}
				
				return _instance;
			}
		}

		public static bool IsInstance() 
		{ 
			return _instance != null; 
		}

		private void OnApplicationQuit()
		{
			_instance = null;
		}
		
		void OnDestroy() 
		{ 
			if (_instance == this) _instance = null; 
		}
	}
}
