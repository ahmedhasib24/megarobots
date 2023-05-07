using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParticle : MonoBehaviour {

	public AudioClip clip;
	public float delay_time;
	public float end_time;
	public bool isLoop;
	public bool isDestroy;

    private float fTime;

	private AudioSource audioSource;
	// Use this for initialization


	void OnEnable(){
		audioSource = gameObject.AddComponent<AudioSource> ();
		audioSource.playOnAwake = false;
		audioSource.loop = isLoop;
        fTime = 0;
        isDestroy = true;
        StartCoroutine(PlayAudio());
	}

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(delay_time);
        audioSource.PlayOneShot(clip);
    }

    private void Update()
    {
        if(isDestroy)
        {
            if(fTime > end_time)
            {
                Stop();
            }
        }
    }

    void Stop(){
        isDestroy = false;
        fTime = 0;
		if (audioSource != null) {
			audioSource.Stop ();
            Destroy(audioSource);
		}
	}

    private void OnDisable()
    {
        Stop();
    }
}
