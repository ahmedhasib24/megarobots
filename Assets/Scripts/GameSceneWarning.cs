using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneWarning : MonoBehaviour
{
    public Text txtWarning;

    public void Show(string msg)
    {
        txtWarning.text = msg.ToString();
        gameObject.SetActive(true);

        CancelInvoke();
        Invoke("DisableGameobject", 3f);
    }

    void DisableGameobject()
    {
        gameObject.SetActive(false);
    }
}
