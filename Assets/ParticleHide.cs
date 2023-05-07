using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHide : MonoBehaviour
{
    public float fTotalTime;
    public float fTmpTime;
    public bool bStart;

    private void OnEnable()
    {
        fTmpTime = 0;
        bStart = true;    
    }

    private void OnDisable()
    {
        HideSelf();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bStart)
        {
            if(fTmpTime > fTotalTime)
            {
                HideSelf();
            }
            else
            {
                fTmpTime += Time.deltaTime;
            }
        }
    }

    void HideSelf()
    {
        fTmpTime = 0;
        bStart = false;
        gameObject.SetActive(false);
    }
}
