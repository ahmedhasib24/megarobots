using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    #region Singleton
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    private static MainMenuPanel s_Instance = null;

    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static MainMenuPanel instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first PR_Utility object in the scene.
                s_Instance = FindObjectOfType(typeof(MainMenuPanel)) as MainMenuPanel;
                if (s_Instance == null)
                    Debug.Log("Could not locate an MainMenuPanel object. \n You have to have exactly one MainMenuPanel in the scene.");
            }
            return s_Instance;
        }
    }

    // Ensure that the instance is destroyed when the game is stopped in the editor.
    void OnApplicationQuit()
    {
        s_Instance = null;
    }
    #endregion

    public Text txtUserName, txtUserGems;
    public Image imgUserAvatar;
    

    public void Refresh()
    {
        txtUserName.text = GameManage.User.username;
        txtUserGems.text = GameManage.User.gems.ToString();
        imgUserAvatar.sprite = MenuUIManager.instance.AvatarList[GameManage.User.avatar_id];
    }
}
