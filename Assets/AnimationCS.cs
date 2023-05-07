using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationCS : MonoBehaviour {

    public Animator FlyingAni;

    public string IdleName;
    public string WalkName;
    public string RunName;
    public string DieName;

    public string[] LeftAttackList;
    public string[] RightAttackList;

    public InputField IndexInput;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IdleButton()
    {
        FlyingAni.Play(IdleName, -1, 0);
    }
    public void WalkButton()
    {
        FlyingAni.Play(WalkName, -1, 0);
    }
    public void RunButton()
    {
        FlyingAni.Play(RunName, -1, 0);
    }
    public void DieButton()
    {
        FlyingAni.Play(DieName, -1, 0);
    }

    public void LeftButton()
    {
        FlyingAni.Play(LeftAttackList[int.Parse(IndexInput.text)], -1, 0);
    }

    public void RightButton()
    {
        FlyingAni.Play(RightAttackList[int.Parse(IndexInput.text)], -1, 0);
    }
}
