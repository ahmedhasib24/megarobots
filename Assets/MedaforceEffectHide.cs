using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedaforceEffectHide : MonoBehaviour
{
    public float time = 1f;

    private void OnEnable()
    {
        Invoke("Disable", 1f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
