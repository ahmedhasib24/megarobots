using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoadingBar : MonoBehaviour
{
    public static SceneLoadingBar instance;

    private void Awake()
    {
        SceneLoadingBar.instance = this;
    }

    public Image imgProgress;

    private void Start()
    {
        instance.gameObject.SetActive(false);
    }

    public void Show()
    {
        instance.gameObject.SetActive(true);
        imgProgress.fillAmount = 0;
    }

    public void SetProgressBar(float value)
    {
        imgProgress.fillAmount = value;
    }
}
