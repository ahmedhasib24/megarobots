using UnityEngine;
using System.Collections;

//Script for Wind Turbine in Desert Town Poly Pixel Inc.
public class WindTurbineRotate : MonoBehaviour {

	//Rotation Speed.
	public float speed = 1f;

	void Update () 
	{
		//Rotating on the X axis only.
		transform.Rotate (speed, 0, 0);
	}
}
