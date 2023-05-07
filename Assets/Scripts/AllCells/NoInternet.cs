using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NoInternet : MonoBehaviour {

	public void TryInternetAgain() {
		Destroy (gameObject);
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene ().buildIndex);
	}
}
