using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    public static LoadingPanel instance;

    public int totalTask = 0;
    public int taskCompleted = 0;

    private void Awake()
    {
        LoadingPanel.instance = this;
    }

    public void Show()
    {
        Debug.Log("Show Loading");
        instance.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (taskCompleted == totalTask)
        {
            taskCompleted = 0;
            totalTask = 0;
            instance.gameObject.SetActive(false);
        }
    }
}
