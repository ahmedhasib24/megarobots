using UnityEngine;
using System.Collections;

public class destroyMe : MonoBehaviour{

    public float deathtimer = 10;

    float fTime;

    private void OnEnable()
    {
        fTime = 0;
    }

    private void Update()
    {
        if (fTime > deathtimer)
        {
            fTime = 0;
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            fTime += Time.deltaTime;
        }
    }
}
