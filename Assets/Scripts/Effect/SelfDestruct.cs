using UnityEngine;
using System.Collections;


public class SelfDestruct : MonoBehaviour {

	public float selfdestruct_in = 4; // Setting this to 0 means no selfdestruct.

    float fTime;

    private void OnEnable()
    {
        fTime = 0;
    }

    private void Update()
    {
        if(fTime > selfdestruct_in)
        {
            fTime = 0;
            transform.parent.gameObject.SetActive(false);
        }else
        {
            fTime += Time.deltaTime;
        }
    }
}
