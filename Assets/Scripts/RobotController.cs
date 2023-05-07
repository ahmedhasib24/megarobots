using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared;
using UnityEngine.EventSystems;
using System;

public class RobotController : MonoBehaviour
{
    public List<GameObject> headObjects;
    public List<GameObject> leftArmObjects;
    public List<GameObject> rightArmObjects;
    public List<GameObject> legObjects;

    public List<GameObject> skeletonHeadObjects;
    public List<GameObject> skeletonLeftArmObjects;
    public List<GameObject> skeletonRightArmObjects;
    public List<GameObject> skeletonLegObjects;

    public List<GameObject> headParticles;
    public List<GameObject> leftArmParticles;
    public List<GameObject> rightArmParticles;
    public List<GameObject> legParticles;

    public ParticleSystem medaforceParticle;
    public GameObject medaforceEffect;

    public List<Animator> animators;
    public List<string> idleAnimList;
    public List<string> walkAnimList;
    public List<string> runAnimList;
    public List<string> dieAnimList;

    //public List<string> leftAnimList;
    //public List<string> rightAnimList;


    public int id = 0;
    public RobotType type;
    public ParticleSystem selectParticle;
    public GameObject selectionBase;
    public GameObject attackSelectedIndicator;

    public GameObject fireballPrefab;
    public Transform fireballPosTransform;



    //public RobotPart Head;
    //public RobotPart LeftArm;
    //public RobotPart RightArm;
    //public RobotPart Leg;

    public Robot robot;

    public string attackTargetString = "";

    private Vector3 myIdlePos = Vector3.zero;

    private bool isRun = false;
    private bool isAttacking = false;
    private bool isThrowingProjectile = false;

    private Vector3 targetPos = Vector3.zero;

    private bool isReturningBack = false;

    public float totalLife;
    public float totalAttack;
    public float totalDefence;
    public float totalVelocity;

    public float headLife, headAttack, headDefence, headVelocity;
    public float leftArmLife, leftArmAttack, leftArmDefence, leftArmVelocity;
    public float rightArmLife, rightArmAttack, rightArmDefence, rightArmVelocity;
    public float legLife, legAttack, legDefence, legVelocity;

    public bool isHeadDestroyed = false;
    public bool isLeftArmDestroyed = false;
    public bool isRightArmDestroyed = false;
    public bool isLegDestroyed = false;

    public int medaforceValue = 0;

    public int headAttackCount = 5;
    public int larmAttackCount = 5;
    public int rarmAttackCount = 5;
    public int legAttackCount = 5;

    public PhotonPlayer myPlayer;

    private int RobotTypeId
    {
        get
        {
            int typeId = -1;
            switch (robot.type)
            {
                case "HM":
                    typeId = 0;
                    break;
                case "FL":
                    typeId = 1;
                    break;
                case "RL":
                    typeId = 2;
                    break;
                case "SP":
                    typeId = 3;
                    break;
            }
            return typeId;
        }
    }

    private void Start()
    {
        if (PlayManage.instance.isMultiplayer)
        {
            return;
        }
        else
        {
            //RobotPart head = new RobotPart();
            //RobotPart leftArm = new RobotPart();
            //RobotPart rightArm = new RobotPart();
            //RobotPart leg = new RobotPart();

            //head.RobotRandom();
            //leftArm.RobotRandom();
            //rightArm.RobotRandom();
            //leg.RobotRandom();

            //Initialize(head, leftArm, rightArm, leg);
        }
    }

    public void Initialize(RobotPart head, RobotPart leftArm, RobotPart rightArm, RobotPart leg)
    {
        //Head = head;
        //LeftArm = leftArm;
        //RightArm = rightArm;
        //Leg = leg;


        //totalLife = (Head.life + LeftArm.life + RightArm.life + Leg.life) / 4;
        //totalAttack = (Head.attack + LeftArm.attack + RightArm.attack + Leg.attack) / 4;
        //totalDefence = (Head.defense + LeftArm.defense + RightArm.defense + Leg.defense) / 4;
        //totalVelocity = Leg.velocity;

        //myIdlePos = transform.position;
    }

    public void Initialize(Robot robot)
    {
        this.robot = robot;

        headLife = robot.Head.life;
        headAttack = robot.Head.attack;
        headDefence = robot.Head.defence;
        headVelocity = robot.Head.velocity;

        leftArmLife = robot.LeftArm.life;
        leftArmAttack = robot.LeftArm.attack;
        leftArmDefence = robot.LeftArm.defence;
        leftArmVelocity = robot.LeftArm.life;

        rightArmLife = robot.RightArm.life;
        rightArmAttack = robot.RightArm.attack;
        rightArmDefence = robot.RightArm.defence;
        rightArmVelocity = robot.RightArm.velocity;

        legLife = robot.Leg.life;
        legAttack = robot.Leg.attack;
        legDefence = robot.Leg.defence;
        legVelocity = robot.Leg.velocity;

        totalLife = robot.Life;
        totalAttack = robot.Attack;
        totalDefence = robot.Defence;
        totalVelocity = robot.Velocity;

        ActivateSkeletonHead(-1);
        ActivateSkeletonLeftArm(-1);
        ActivateSkeletonRightArm(-1);
        ActivateSkeletonLeg(-1);

        ActivateHead(robot.Head.head_id);
        ActivateLeftArm(robot.LeftArm.larm_id);
        ActivateRightArm(robot.RightArm.rarm_id);
        ActivateLeg(robot.Leg.leg_id);
        //headObjects[robot.Head.head_id].SetActive(true);
        //leftArmObjects[robot.LeftArm.larm_id].SetActive(true);
        //rightArmObjects[robot.RightArm.rarm_id].SetActive(true);
        //legObjects[robot.Leg.leg_id].SetActive(true);

        myIdlePos = transform.position;
    }

    void ActivateHead(int id)
    {
        for (int i = 0; i < headObjects.Count; i++)
        {
            if (id == i)
            {
                headObjects[i].SetActive(true);
            }
            else
            {
                headObjects[i].SetActive(false);
            }
        }
    }

    void ActivateLeftArm(int id)
    {
        for (int i = 0; i < leftArmObjects.Count; i++)
        {
            if (id == i)
            {
                leftArmObjects[i].SetActive(true);
            }
            else
            {
                leftArmObjects[i].SetActive(false);
            }
        }
    }

    void ActivateRightArm(int id)
    {
        for (int i = 0; i < rightArmObjects.Count; i++)
        {
            if (id == i)
            {
                rightArmObjects[i].SetActive(true);
            }
            else
            {
                rightArmObjects[i].SetActive(false);
            }
        }
    }

    void ActivateLeg(int id)
    {
        for (int i = 0; i < legObjects.Count; i++)
        {
            if (id == i)
            {
                legObjects[i].SetActive(true);
            }
            else
            {
                legObjects[i].SetActive(false);
            }
        }
    }

    void ActivateSkeletonHead(int id)
    {
        ActivateHead(-1);
        for (int i = 0; i < skeletonHeadObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonHeadObjects[i].SetActive(true);
            }
            else
            {
                skeletonHeadObjects[i].SetActive(false);
            }
        }
    }

    void ActivateSkeletonLeftArm(int id)
    {
        ActivateLeftArm(-1);
        for (int i = 0; i < skeletonLeftArmObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonLeftArmObjects[i].SetActive(true);
            }
            else
            {
                skeletonLeftArmObjects[i].SetActive(false);
            }
        }
    }

    void ActivateSkeletonRightArm(int id)
    {
        ActivateRightArm(-1);
        for (int i = 0; i < skeletonRightArmObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonRightArmObjects[i].SetActive(true);
            }
            else
            {
                skeletonRightArmObjects[i].SetActive(false);
            }
        }
    }

    void ActivateSkeletonLeg(int id)
    {
        ActivateLeg(-1);
        for (int i = 0; i < skeletonLegObjects.Count; i++)
        {
            if (id == i)
            {
                skeletonLegObjects[i].SetActive(true);
            }
            else
            {
                skeletonLegObjects[i].SetActive(false);
            }
        }
    }

    void PlayIdleAnim()
    {
        animators[RobotTypeId].Play(idleAnimList[RobotTypeId], -1, 0);
    }

    void PlayWalkAnim()
    {
        animators[RobotTypeId].Play(walkAnimList[RobotTypeId], -1, 0);
    }

    void PlayRunAnim()
    {
        animators[RobotTypeId].Play(runAnimList[RobotTypeId], -1, 0);
    }

    void PlayDieAnim()
    {
        animators[RobotTypeId].Play(dieAnimList[RobotTypeId], -1, 0);
    }

    void PlayLeftAnim()
    {
        animators[RobotTypeId].Play((robot.LeftArm.name + "_Left"), -1, 0);
    }

    void PlayRightAnim()
    {
        animators[RobotTypeId].Play((robot.RightArm.name + "_Right"), -1, 0);
    }

    private void Update()
    {
        //Initial run
        if (isRun == true)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, targetPos, (totalVelocity / 20) * Time.deltaTime);//.Translate(targetPos);

            if (isReturningBack == false)
            {
                if (transform.position == targetPos)
                {
                    isReturningBack = true;
                    targetPos = myIdlePos;
                    if (type == RobotType.MINE)
                    {
                        transform.localEulerAngles = new Vector3(0, -90f, 0);
                    }
                    else
                    {
                        transform.localEulerAngles = new Vector3(0, 90, 0);
                    }

                }
                else
                {
                    Vector3 relativePos = targetPos - transform.position;

                    // the second argument, upwards, defaults to Vector3.up
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = rotation;
                }
            }
            else
            {
                if (transform.position == targetPos)
                {
                    isReturningBack = false;
                    isRun = false;
                    PlayIdleAnim();

                    PlayManage.instance.attackRobotOrder.Add(this);

                    if (type == RobotType.MINE)
                    {
                        transform.localEulerAngles = new Vector3(0, 90f, 0);
                    }
                    else
                    {
                        transform.localEulerAngles = new Vector3(0, -90, 0);
                    }
                }
                else
                {
                    Vector3 relativePos = targetPos - transform.position;

                    // the second argument, upwards, defaults to Vector3.up
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = rotation;
                }
            }
        }
        else if (isAttacking == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, (robot.Velocity / 20) * Time.deltaTime);

            if (isReturningBack == false)
            {
                if (transform.position == targetPos)
                {
                    if (type == RobotType.MINE)
                    {
                        transform.localEulerAngles = new Vector3(0, 90f, 0);
                    }
                    else
                    {
                        transform.localEulerAngles = new Vector3(0, -90, 0);
                    }

                    if (isThrowingProjectile == false)
                    {
                        isThrowingProjectile = true;
                        StartCoroutine(StartProjectileThrowRoutine());
                    }
                }
                else
                {
                    Vector3 relativePos = targetPos - transform.position;

                    // the second argument, upwards, defaults to Vector3.up
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = rotation;
                }
            }
            else
            {
                if (transform.position == targetPos)
                {
                    isReturningBack = false;
                    isAttacking = false;
                    PlayIdleAnim();
                    if (type == RobotType.MINE)
                    {
                        transform.localEulerAngles = new Vector3(0, 90f, 0);
                    }
                    else
                    {
                        transform.localEulerAngles = new Vector3(0, -90, 0);
                    }

                    PlayManage.instance.AttackCompleted();
                }
                else
                {
                    Vector3 relativePos = targetPos - transform.position;

                    // the second argument, upwards, defaults to Vector3.up
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                    transform.rotation = rotation;
                }
            }
        }
        else
        {
            if (PlayManage.instance.isFighting)
            {
                return;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<RobotController>() != null)
                    {
                        if (hit.collider.gameObject.GetComponent<RobotController>().id == id)
                        {
                            if (type == RobotType.MINE)
                            {
                                if (!isHeadDestroyed)
                                {
                                    PlayManage.instance.ToggleMineRobotSelection(id);
                                }
                            }
                            else
                            {
                                if (!isHeadDestroyed)
                                {
                                    PlayManage.instance.ToggleEnemyRobotSelection(id);
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    PlayManage.instance.ToggleMineRobotSelection(id);
                    //    PlayManage.instance.ToggleEnemyRobotSelection(id);
                    //}
                }
            }

        }
    }

    public void RunToPos(Vector3 pos)
    {
        if (isHeadDestroyed)
        {
            PlayManage.instance.attackRobotOrder.Add(this);
            return;
        }
        else if (isLegDestroyed)
        {
            PlayManage.instance.attackRobotOrder.Add(this);
            return;
        }
        isRun = true;
        PlayRunAnim();
        isReturningBack = false;
        targetPos = pos;
    }

    public void Select()
    {
        //selectParticle.Play();
        selectionBase.SetActive(true);
        //PlayManage.instance.playcs.ShowStatusView(id);
        attackSelectedIndicator.SetActive(false);
    }

    public void Deselect()
    {
        //selectParticle.Stop();
        selectionBase.SetActive(false);
        //PlayManage.instance.playcs.ShowStatusView(-1);
        if (type != RobotType.ENEMY)
        {
            if (string.IsNullOrEmpty(attackTargetString))
            {
                attackSelectedIndicator.SetActive(false);
            }
            else
            {
                attackSelectedIndicator.SetActive(true);
            }
        }
    }

    public void DeactivateAttackSelectedIndicator()
    {
        attackSelectedIndicator.SetActive(false);
    }

    public void SetAttackTarget(string ats)
    {
        attackTargetString = ats;

        if (type != RobotType.ENEMY)
        {
            if (string.IsNullOrEmpty(attackTargetString))
            {
                attackSelectedIndicator.SetActive(false);
            }
            else
            {
                attackSelectedIndicator.SetActive(true);
            }
        }
    }

    public void Attack()
    {
        PlayManage.instance.playcs.DeactivateMFBar();
        if (PlayManage.instance.isMultiplayer && type == RobotType.ENEMY)
        {
            //int robotCount = PhotonUtility.GetRoomProperties<int>(PhotonEnums.Room.RobotCount);
            if ((id - PlayManage.instance.MineActiveRobots.Count) == 0)
            {
                attackTargetString = PhotonUtility.GetPlayerProperties<string>(myPlayer, PhotonEnums.Player.AttackTargetString1);
                //Debug.LogError("jjfjfjhjhfjh 1: " + PhotonUtility.GetPlayerProperties<string>(myPlayer, PhotonEnums.Player.AttackTargetString1));
            }
            else if ((id - PlayManage.instance.MineActiveRobots.Count) == 1)
            {
                attackTargetString = PhotonUtility.GetPlayerProperties<string>(myPlayer, PhotonEnums.Player.AttackTargetString2);
                //Debug.LogError("jjfjfjhjhfjh 2: " + PhotonUtility.GetPlayerProperties<string>(myPlayer, PhotonEnums.Player.AttackTargetString2));
            }
            else if ((id - PlayManage.instance.MineActiveRobots.Count) == 2)
            {
                attackTargetString = PhotonUtility.GetPlayerProperties<string>(myPlayer, PhotonEnums.Player.AttackTargetString3);
                //Debug.LogError("jjfjfjhjhfjh 3: " + PhotonUtility.GetPlayerProperties<string>(myPlayer, PhotonEnums.Player.AttackTargetString3));
            }
        }

        //Debug.LogError("attack target string: " + attackTargetString);

        bool canAttack = true;
        if (isHeadDestroyed)
        {
            canAttack = false;
        }
        else
        {
            int minePartId = int.Parse(attackTargetString.Substring(1, 1));
            switch (minePartId)
            {
                case 0:
                    if (isHeadDestroyed)
                    {
                        canAttack = false;
                    }
                    break;
                case 1:
                    if (isLeftArmDestroyed)
                    {
                        canAttack = false;
                    }
                    break;
                case 2:
                    if (isRightArmDestroyed)
                    {
                        canAttack = false;
                    }
                    break;
                case 3:
                    if (isLegDestroyed)
                    {
                        canAttack = false;
                    }
                    break;
            }

            int opponentId = int.Parse(attackTargetString.Substring(2, 1));
            int opponentPartId = int.Parse(attackTargetString.Substring(3, 1));

            if (PlayManage.instance.allRobotControllers[opponentId].isHeadDestroyed)
            {
                canAttack = false;
            }
            else
            {
                switch (opponentPartId)
                {
                    case 0:
                        if (PlayManage.instance.allRobotControllers[opponentId].isHeadDestroyed)
                        {
                            canAttack = false;
                        }
                        break;
                    case 1:
                        if (PlayManage.instance.allRobotControllers[opponentId].isLeftArmDestroyed)
                        {
                            canAttack = false;
                        }
                        break;
                    case 2:
                        if (PlayManage.instance.allRobotControllers[opponentId].isRightArmDestroyed)
                        {
                            canAttack = false;
                        }
                        break;
                    case 3:
                        if (PlayManage.instance.allRobotControllers[opponentId].isLegDestroyed)
                        {
                            canAttack = false;
                        }
                        break;
                }
            }
        }


        if (!canAttack)
        {
            PlayManage.instance.AttackCompleted();
            //return;
        }
        else
        {
            int opponentId = int.Parse(attackTargetString.Substring(2, 1));
            int partId = int.Parse(attackTargetString.Substring(3, 1));

            if (type == RobotType.MINE)
            {
                targetPos = PlayManage.instance.MineAttackPosList[opponentId - PlayManage.instance.MineActiveRobots.Count];
                PlayManage.instance.mineAttackCount++;
                //Debug.LogError("Attack count " + PlayManage.instance.mineAttackCount);
            }
            else
            {
                if (PlayManage.instance.isMultiplayer)
                {
                    targetPos = PlayManage.instance.EnemyAttackPosList[opponentId - PlayManage.instance.MineActiveRobots.Count];
                }
                else
                {
                    targetPos = PlayManage.instance.EnemyAttackPosList[opponentId];
                }
            }

            isAttacking = true;
            PlayRunAnim();
        }
    }

    public void GetDamage(int partId, float damage)
    {
        switch (partId)
        {
            case 0:
                headLife -= (int)(damage * 0.7f);
                headAttack -= (int)(damage * 0.3f);
                if (headLife < 0)
                {
                    headLife = 0;
                }
                break;
            case 1:
                leftArmLife -= (int)(damage * 0.7f);
                leftArmAttack -= (int)(damage * 0.3f);
                if (leftArmLife < 0)
                {
                    leftArmLife = 0;
                }
                break;
            case 2:
                rightArmLife -= (int)(damage * 0.7f);
                rightArmAttack -= (int)(damage * 0.3f);
                if (rightArmLife < 0)
                {
                    rightArmLife = 0;
                }
                break;
            case 3:
                legLife -= (int)(damage * 0.7f);
                legAttack -= (int)(damage * 0.3f);
                totalVelocity = legVelocity * (legLife / robot.Leg.life);
                if (legLife < 0)
                {
                    legLife = 0;
                }
                if (totalVelocity <= 0)
                {
                    totalVelocity = legVelocity * 0.1f;
                }
                break;
        }

        //life = (Head.life + LeftArm.life + RightArm.life + Leg.life) / 4;
        //attack = (Head.attack + LeftArm.attack + RightArm.attack + Leg.attack) / 4;
        //defense = (Head.defense + LeftArm.defense + RightArm.defense + Leg.defense) / 4;
        //velocity = Leg.velocity;

        totalLife = (headLife + leftArmLife + rightArmLife + legLife);
        totalAttack = (headAttack + leftArmAttack + rightArmAttack + legAttack);
        totalDefence = (headDefence + leftArmDefence + rightArmDefence + legDefence);


        if (headLife <= 0)
        {
            DestroyHead();
        }
        if (leftArmLife <= 0)
        {
            DestroyLeftArm();
        }
        if (rightArmLife <= 0)
        {
            DestroyRightArm();
        }
        if (legLife <= 0)
        {
            DestroyLeg();
        }

        FindObjectOfType<PlayCS>().ShowStatusView(id);
    }

    void DestroyHead()
    {
        if (!isHeadDestroyed)
        {
            if (type == RobotType.MINE)
            {
                PlayManage.instance.mineRobotDied++;
                PlayManage.instance.MineDestroyedRobots.Add(robot);
            }
            else if (type == RobotType.ENEMY)
            {
                PlayManage.instance.opponentRobotDied++;
                PlayManage.instance.EnemyDestroyedRobots.Add(robot);
            }
        }
        isHeadDestroyed = true;
        ActivateSkeletonHead(RobotTypeId);
        ActivateSkeletonLeftArm(RobotTypeId);
        ActivateSkeletonRightArm(RobotTypeId);
        ActivateSkeletonLeg(RobotTypeId);
        PlayDieAnim();
        //headObjects[robot.Head.head_id].SetActive(false);
    }

    void DestroyLeftArm()
    {
        if (!isLeftArmDestroyed)
        {
            if (type == RobotType.MINE)
            {
                PlayManage.instance.minePartsDestroyed++;
            }
            else if (type == RobotType.ENEMY)
            {
                PlayManage.instance.opponentPartsDestroyed++;
            }
        }
        isLeftArmDestroyed = true;
        ActivateSkeletonLeftArm(RobotTypeId);
        //leftArmObjects[robot.LeftArm.larm_id].SetActive(false);
    }

    void DestroyRightArm()
    {
        if (!isRightArmDestroyed)
        {
            if (type == RobotType.MINE)
            {
                PlayManage.instance.minePartsDestroyed++;
            }
            else if (type == RobotType.ENEMY)
            {
                PlayManage.instance.opponentPartsDestroyed++;
            }
        }
        isRightArmDestroyed = true;
        ActivateSkeletonRightArm(RobotTypeId);
        //rightArmObjects[robot.RightArm.rarm_id].SetActive(false);
    }

    void DestroyLeg()
    {
        if (!isLegDestroyed)
        {
            if (type == RobotType.MINE)
            {
                PlayManage.instance.minePartsDestroyed++;
            }
            else if (type == RobotType.ENEMY)
            {
                PlayManage.instance.opponentPartsDestroyed++;
            }
        }
        isLegDestroyed = true;
        ActivateSkeletonLeg(RobotTypeId);
        medaforceValue = 0;
        //legObjects[robot.Leg.leg_id].SetActive(false);
    }

    IEnumerator StartProjectileThrowRoutine()
    {
        PlayIdleAnim();
        //Debug.LogError("Trowing projectile: " + id);
        isThrowingProjectile = true;
        int minePartId = int.Parse(attackTargetString.Substring(1, 1));
        float waitTime = 0f;

        switch (minePartId)
        {
            case 0:
                waitTime = 2.5f;
                break;
            case 1:
                waitTime = 2.5f;
                PlayLeftAnim();
                break;
            case 2:
                waitTime = 3.5f;
                PlayRightAnim();
                break;
            case 3:
                waitTime = 2.5f;
                break;
        }
        //yield return new WaitForSeconds(waitTime/2);

        //Throw projectile
        Vector3 fireballPos = new Vector3(transform.position.x, 2.5f, transform.position.z);
        GameObject go = Instantiate(fireballPrefab, fireballPosTransform.position, fireballPosTransform.rotation);
        BulletController bc = go.GetComponent<BulletController>();

        bc.partId = int.Parse(attackTargetString.Substring(3, 1));
        switch (minePartId)
        {
            case 0:
                bc.attack = headAttack;
                headAttackCount--;
                headParticles[robot.Head.head_id].SetActive(true);
                if (type == RobotType.MINE)
                {
                    bc.attack = headAttack;
                    PlayManage.instance.playcs.ShowEffectName(headParticles[robot.Head.head_id].name, true);
                }
                else
                {
                    PlayManage.instance.playcs.ShowEffectName(headParticles[robot.Head.head_id].name, false);
                }
                break;
            case 1:
                bc.attack = leftArmAttack;
                larmAttackCount--;
                leftArmParticles[robot.LeftArm.larm_id].SetActive(true);
                if (type == RobotType.MINE)
                {
                    bc.attack = leftArmAttack;
                    PlayManage.instance.playcs.ShowEffectName(leftArmParticles[robot.LeftArm.larm_id].name, true);
                }
                else
                {
                    PlayManage.instance.playcs.ShowEffectName(leftArmParticles[robot.LeftArm.larm_id].name, false);
                }
                break;
            case 2:
                bc.attack = rightArmAttack;
                rarmAttackCount--;
                rightArmParticles[robot.RightArm.rarm_id].SetActive(true);
                if (type == RobotType.MINE)
                {
                    bc.attack = rightArmAttack;
                    PlayManage.instance.playcs.ShowEffectName(rightArmParticles[robot.RightArm.rarm_id].name, true);
                }
                else
                {
                    PlayManage.instance.playcs.ShowEffectName(rightArmParticles[robot.RightArm.rarm_id].name, false);
                }
                break;
            case 3:
                if (medaforceValue < 100)
                {
                    bc.attack = 0;
                    int prev = medaforceValue;
                    medaforceValue += 50;
                    if (type == RobotType.MINE)
                    {
                        PlayManage.instance.playcs.ActivateMFBar();
                        PlayManage.instance.playcs.ShowMFBarWithAnimation(prev, medaforceValue);
                    }
                    //medaforceParticle.Play();
                    medaforceEffect.SetActive(true);
                    if (type == RobotType.MINE)
                    {
                        PlayManage.instance.playcs.ShowEffectName("Charging Medaforce", true);
                    }
                    else
                    {
                        PlayManage.instance.playcs.ShowEffectName("Charging Medaforce", false);
                    }
                }
                else
                {
                    bc.attack = legAttack * 3;
                    legAttackCount--;
                    int prev = medaforceValue;
                    medaforceValue = 0;
                    if (type == RobotType.MINE)
                    {
                        PlayManage.instance.playcs.ActivateMFBar();
                        PlayManage.instance.playcs.ShowMFBarWithAnimation(prev, medaforceValue);
                    }
                    legParticles[robot.Leg.leg_id].SetActive(true);
                    if (type == RobotType.MINE)
                    {
                        PlayManage.instance.playcs.ShowEffectName(legParticles[robot.Leg.leg_id].name, true);
                    }
                    else
                    {
                        PlayManage.instance.playcs.ShowEffectName(legParticles[robot.Leg.leg_id].name, false);
                    }
                }
                
                break;
        }
        //bc.attack = totalAttack;


        yield return new WaitForSeconds(6f);
        isThrowingProjectile = false;
        isReturningBack = true;
        PlayRunAnim();
        targetPos = myIdlePos;
        PlayManage.instance.playcs.DeactivateMFBar();
    }
}
