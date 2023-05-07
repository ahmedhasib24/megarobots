using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleCS : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class EffectClass
{
    public int m_Part; //0 : Head, 1: Left Arm, 2: Right Arm
    public int m_Type; //0 : Particle, 1 : Model
    public GameObject m_Attack;

    public EffectClass()
    {
        m_Part = 0;
        m_Type = 0;
        m_Attack = new GameObject();
    }
}
