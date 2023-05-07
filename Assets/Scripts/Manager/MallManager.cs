using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MallManager : MonoBehaviour {

    public List<GameObject> m_InventoryRobots = new List<GameObject>();

    public List<Text> m_Properties = new List<Text>();

    int m_Index;

    // Use this for initialization
    void Start () {

        ShowRobot("0");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowRobot(string m_ID)
    {
        m_Index = int.Parse(m_ID);

        if (m_Index < 0) m_Index = 0;
        if (m_Index >= m_InventoryRobots.Count) m_Index = m_InventoryRobots.Count - 1;

        for (int i = 0; i < m_InventoryRobots.Count; i++)
        {
            if (m_Index == i) m_InventoryRobots[i].SetActive(true);
            else m_InventoryRobots[i].SetActive(false);
        }

        ShowInfo();
    }

    void ShowInfo()
    {
        int m_temp1, m_temp2;

        //m_temp1 = Random.Range(0, 5); m_temp2 = Random.Range(0, 59);
        //m_Properties[0].text = m_temp1.ToString("D2") + " : " + m_temp2.ToString("D2");

        m_temp1 = Random.Range(10, 50);
        m_Properties[1].text = m_temp1.ToString();

        m_temp1 = Random.Range(10, 50);
        m_Properties[2].text = m_temp1.ToString();

        m_temp1 = Random.Range(10, 50);
        m_Properties[3].text = m_temp1.ToString();

        m_temp1 = Random.Range(10, 50);
        m_Properties[4].text = m_temp1.ToString() + "(m/s)";
    }

    public void CloseBtn()
	{
		MenuUIManager.instance.ShowPanel (0);
	}
}
