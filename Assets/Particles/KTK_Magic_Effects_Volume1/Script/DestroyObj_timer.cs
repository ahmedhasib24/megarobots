﻿ using UnityEngine;
using System.Collections;

public class DestroyObj_timer : MonoBehaviour {
	
	public float timer = 0.5f;

	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0){
			Object.Destroy(gameObject);
		}
	}
}
