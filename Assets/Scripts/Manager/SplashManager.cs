using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour {

	public Image LoadingBar;

	public string SceneName;

	void Start()
	{
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		StartCoroutine(StartLoader());
	}

	AsyncOperation oper;

	IEnumerator StartLoader()
	{
		oper = SceneManager.LoadSceneAsync(SceneName);
		oper.allowSceneActivation = false;

		while (!oper.isDone)
		{
			LoadingBar.fillAmount = oper.progress;
			yield return new WaitForEndOfFrame();
			if (oper.progress >= 0.9f) break;
		}

		yield return new WaitForEndOfFrame();
//		yield return new WaitForSeconds(3f);

		LoadingBar.fillAmount = 1f;
		oper.allowSceneActivation = true;
		yield return null;
	}


//	public Image SplashImg;
//
//	IEnumerator Start()
//	{
//		SplashImg.canvasRenderer.SetAlpha (0.0f);
//
//		FadeIn ();
//		yield return new WaitForSeconds (2.5f);
//
//		FadeOut ();
//		yield return new WaitForSeconds (2.5f);
//
//		SceneManager.LoadScene ("02-Main");
//	}
//
//	void FadeIn()
//	{
//		SplashImg.CrossFadeAlpha (1.0f, 1.5f, false);
//	}
//
//	void FadeOut()
//	{
//		SplashImg.CrossFadeAlpha (0.0f, 2.5f, false);
//	}
}