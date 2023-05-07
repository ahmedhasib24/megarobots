using UnityEngine;
using System.Collections;

//Script for Wind Turbine in Desert Town Poly Pixel Inc.
public class AirConFanRotate : MonoBehaviour {

	//Rotation Speed.
	public float speed = 1f;

	void Update () 
	{
		//Rotating on the Z axis only.
		transform.Rotate (0, 0, speed);
	}
}
