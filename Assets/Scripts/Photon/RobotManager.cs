using Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    #region SINGLETON
    private static RobotManager instance;

    public static RobotManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RobotManager>() as RobotManager;
            }
            return instance;
        }
    }
    #endregion

    public List<Vector3> MineIdlePosList = new List<Vector3>();
    public List<Vector3> MineAttackPosList = new List<Vector3>();
    public List<Vector3> EnemyIdlePosList = new List<Vector3>();
    public List<Vector3> EnemyAttackPosList = new List<Vector3>();

    public List<GameObject> MineRobots = new List<GameObject>();
    public List<GameObject> EnemyRobots = new List<GameObject>();

    public GameObject MineAttackObj;
    public GameObject EnemyAttackObj;

    public List<string> RobotNameList = new List<string>();

    public List<GameObject> AttackMineList = new List<GameObject>();
    public List<GameObject> PartMineList = new List<GameObject>();

    public List<GameObject> AttackEnemyList = new List<GameObject>();
    public List<GameObject> PartEnemyList = new List<GameObject>();

    public DB dbData = new DB();

    List<int> m_iRobotList = new List<int>();
    List<int> m_iHeadList = new List<int>();
    List<int> m_iLArmList = new List<int>();
    List<int> m_iRArmList = new List<int>();
    List<int> m_iLegList = new List<int>();

    List<int> e_iRobotList = new List<int>();
    List<int> e_iHeadList = new List<int>();
    List<int> e_iLArmList = new List<int>();
    List<int> e_iRArmList = new List<int>();
    List<int> e_iLegList = new List<int>();

    private void Start()
    {
        SaveInfo();
        Init();
        //MakeMineRobots();
    }

    private void Init()
    {
        for (int i = 0; i < MineRobots.Count; i++)
        {
            MineRobots[i].transform.localPosition = MineIdlePosList[i];
            MineRobots[i].SetActive(true);

            EnemyRobots[i].transform.localPosition = EnemyIdlePosList[i];
            EnemyRobots[i].SetActive(true);
        }
    }

    void SaveInfo()
    {
        RobotNameList.Clear();

        //Mine
        AttackMineList.Clear();
        PartMineList.Clear();

        for (int i = 0; i < MineRobots.Count; i++)
        {
            Shared.Robot MineRobot = new Shared.Robot();
            Transform _child1 = MineRobots[i].transform.GetChild(0);

            //Get AttackList
            Transform _child2 = _child1.GetChild(0);

            for (int j = 0; j < _child2.childCount; j++)
            {
                if (i == 0)
                {
                    RobotNameList.Add(_child2.GetChild(j).name);
                }

                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    AttackMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get HMRobotList
            _child2 = _child1.GetChild(1);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get FlyingRobotList
            _child2 = _child1.GetChild(2);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get RollRobotList
            _child2 = _child1.GetChild(3);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get SPRobotList
            _child2 = _child1.GetChild(4);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartMineList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }


            dbData.m_MineMedabot.robotList.Add((Shared.Robot) MineRobot);
        }


        //Enemy
        AttackEnemyList.Clear();
        PartEnemyList.Clear();

        for (int i = 0; i < EnemyRobots.Count; i++)
        {
            Shared.Robot EnemyRobot = new Shared.Robot();
            Transform _child1 = EnemyRobots[i].transform.GetChild(0);

            //Get AttackList
            Transform _child2 = _child1.GetChild(0);

            for (int j = 0; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    AttackEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get HMRobotList
            _child2 = _child1.GetChild(1);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get FlyingRobotList
            _child2 = _child1.GetChild(2);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get RollRobotList
            _child2 = _child1.GetChild(3);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }

            //Get SPRobotList
            _child2 = _child1.GetChild(4);

            for (int j = 1; j < _child2.childCount; j++)
            {
                for (int k = 0; k < _child2.GetChild(j).childCount; k++)
                {
                    PartEnemyList.Add(_child2.GetChild(j).transform.GetChild(k).gameObject);
                }
            }


            dbData.m_EnemyMedabot.robotList.Add(EnemyRobot);
        }
    }

    public void MakeMineRobots()
    {
        m_iRobotList.Add(0); m_iRobotList.Add(1); m_iRobotList.Add(2);
        m_iHeadList.Add(0); m_iHeadList.Add(0); m_iHeadList.Add(0);
        m_iLArmList.Add(1); m_iLArmList.Add(1); m_iLArmList.Add(1);
        m_iRArmList.Add(2); m_iRArmList.Add(2); m_iRArmList.Add(2);
        m_iLegList.Add(3); m_iLegList.Add(3); m_iLegList.Add(1);

        for (int i = 0; i < MineRobots.Count; i++)
        {
            int iIndex = 0;

            if (m_iRobotList[i] == 0) iIndex = 0;
            else if (m_iRobotList[i] == 1) iIndex = 0 + 15;
            else if (m_iRobotList[i] == 2) iIndex = 0 + 15 + 4;
            else if (m_iRobotList[i] == 3) iIndex = 0 + 15 + 4 + 3;

            PartMineList[i * 100 + iIndex * 4 + m_iHeadList[i] * 4 + 0].SetActive(true);
            PartMineList[i * 100 + iIndex * 4 + m_iLArmList[i] * 4 + 1].SetActive(true);
            PartMineList[i * 100 + iIndex * 4 + m_iRArmList[i] * 4 + 2].SetActive(true);
            PartMineList[i * 100 + iIndex * 4 + m_iLegList[i] * 4 + 3].SetActive(true);
        }
    }

    public void MakeEnemyRobots()
    {
        e_iRobotList.Add(3); e_iRobotList.Add(2); e_iRobotList.Add(1);
        e_iHeadList.Add(2); e_iHeadList.Add(0); e_iHeadList.Add(3);
        e_iLArmList.Add(1); e_iLArmList.Add(2); e_iLArmList.Add(2);
        e_iRArmList.Add(2); e_iRArmList.Add(1); e_iRArmList.Add(1);
        e_iLegList.Add(0); e_iLegList.Add(0); e_iLegList.Add(0);

        for (int i = 0; i < EnemyRobots.Count; i++)
        {
            int iIndex = 0;

            if (e_iRobotList[i] == 0) iIndex = 0;
            else if (e_iRobotList[i] == 1) iIndex = 0 + 15;
            else if (e_iRobotList[i] == 2) iIndex = 0 + 15 + 4;
            else if (e_iRobotList[i] == 3) iIndex = 0 + 15 + 4 + 3;

            PartEnemyList[i * 100 + iIndex * 4 + e_iHeadList[i] * 4 + 0].SetActive(true);
            PartEnemyList[i * 100 + iIndex * 4 + e_iLArmList[i] * 4 + 1].SetActive(true);
            PartEnemyList[i * 100 + iIndex * 4 + e_iRArmList[i] * 4 + 2].SetActive(true);
            PartEnemyList[i * 100 + iIndex * 4 + e_iLegList[i] * 4 + 3].SetActive(true);
        }
    }
}
