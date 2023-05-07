using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotRotate : MonoBehaviour {

	public float fSpeed = -50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.localEulerAngles += new Vector3 (0, fSpeed * Time.deltaTime, 0);
	}
}
