using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {

    private void OnEnable()
    {
        if (GetComponent<ParticleSystem>() != null)
        {
            if (GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Stop();
            }

            GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnDisable()
    {
        if (GetComponent<ParticleSystem>() != null)
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }
}
