using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public List<GameObject> PanelList = new List<GameObject>();
    public List<Sprite> AvatarList = new List<Sprite>();
    public List<Sprite> ComponentImageList = new List<Sprite>();

    public GameObject QuitPanel;

    public static MenuUIManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            Sprite temp = Resources.Load("Sprites/Avatar/avatar" + i.ToString(), typeof(Sprite)) as Sprite;
            AvatarList.Add(temp);
        }

        GameObject ParentObj = GameObject.Find("UI1");
        if (ParentObj != null)
        {
            for (int i = 0; i < ParentObj.transform.childCount; i++)
            {
                PanelList.Add(ParentObj.transform.GetChild(i).gameObject);
            }
        }

        QuitPanel = GameObject.Find("UI2").transform.Find("20-QuitPanel").gameObject;

        MainMenuPanel.instance.Refresh();
        if (PlayerPrefs.GetInt("NextLevel", 0) == 1)
        {
            PlayerPrefs.SetInt("NextLevel", 0);
            ShowPanel(6);
        }
        else
        {
            ShowPanel(0);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowQuitPanel();
        }
    }

    public void ShowPanel(int iIndex)
    {
        //		SoundManager.instance.ButtonPlay ();

        for (int i = 0; i < PanelList.Count; i++)
        {
            if (i == iIndex)
                PanelList[i].SetActive(true);
            else
                PanelList[i].SetActive(false);
        }
    }

    public void ShowQuitPanel()
    {
        if (QuitPanel == null)
        {
            QuitPanel = FindObjectOfType<QuitGame>().gameObject;
        }
        QuitPanel.SetActive(true);
    }
}
