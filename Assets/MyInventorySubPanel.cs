using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MyInventorySubPanel : MonoBehaviour
{
    public Text txtTitle;
    public GameObject ui_InventoryPiecePrefab;
    public Transform parent;

    //public List<Sprite> headSprites, leftArmSprites, rightArmSprites, legSprites;

    public List<GameObject> items = new List<GameObject>();

    public void ShowHeads(Robot robot, List<Head> heads)
    {
        txtTitle.text = "My Inventory (Heads)";
        Clear();
        items = new List<GameObject>();

        List<Head> headTyped = heads.Where(x => x.type == robot.type).ToList();
        for (int i = 0; i < headTyped.Count; i++)
        {
            Head head = headTyped[i];
            if (head.is_broken == 0)
            {
                GameObject go = Instantiate(ui_InventoryPiecePrefab, parent);
                InventoryPieceItem pi = go.GetComponent<InventoryPieceItem>();

                bool equip = robot.head_object_id == head.object_id ? true : false;
                Sprite headSprite = GameManage.instance.AllHeadPieceImages[head.head_id];
                pi.Init(PieceType.Head, headSprite, head.object_id, head.life, head.attack, head.defence, head.velocity, head.level, head.ImagineTime(), equip);

                items.Add(go);
            }
        }
    }

    public void ShowLeftArms(Robot robot, List<LeftArm> lArms)
    {
        txtTitle.text = "My Inventory (Left Arms)";
        Clear();
        items = new List<GameObject>();

        List<LeftArm> leftArmTyped = lArms.Where(x => x.type == robot.type).ToList();
        for (int i = 0; i < leftArmTyped.Count; i++)
        {
            LeftArm larm = leftArmTyped[i];
            if (larm.is_broken == 0)
            {
                GameObject go = Instantiate(ui_InventoryPiecePrefab, parent);
                InventoryPieceItem pi = go.GetComponent<InventoryPieceItem>();

                bool equip = robot.larm_object_id == larm.object_id ? true : false;
                Sprite larmSprite = GameManage.instance.AllLeftArmPieceImages[larm.larm_id];
                pi.Init(PieceType.LeftArm, larmSprite, larm.object_id, larm.life, larm.attack, larm.defence, larm.velocity, larm.level, (int)larm.ImagineTime(), equip);

                items.Add(go);
            }
        }
    }

    public void ShowRightArms(Robot robot, List<RightArm> rArms)
    {
        txtTitle.text = "My Inventory (Right Arms)";
        Clear();
        items = new List<GameObject>();

        List<RightArm> rightArmTyped = rArms.Where(x => x.type == robot.type).ToList();
        for (int i = 0; i < rightArmTyped.Count; i++)
        {
            RightArm rarm = rightArmTyped[i];
            if (rarm.is_broken == 0)
            {
                GameObject go = Instantiate(ui_InventoryPiecePrefab, parent);
                InventoryPieceItem pi = go.GetComponent<InventoryPieceItem>();

                bool equip = robot.rarm_object_id == rarm.object_id ? true : false;
                Sprite rarmSprite = GameManage.instance.AllRightArmPieceImages[rarm.rarm_id];
                pi.Init(PieceType.RightArm, rarmSprite, rarm.object_id, rarm.life, rarm.attack, rarm.defence, rarm.velocity, rarm.level, (int)rarm.ImagineTime(), equip);

                items.Add(go);
            }
        }
    }

    public void ShowLegs(Robot robot, List<Leg> legs)
    {
        txtTitle.text = "My Inventory (Legs)";
        Clear();
        items = new List<GameObject>();

        List<Leg> legTyped = legs.Where(x => x.type == robot.type).ToList();
        for (int i = 0; i < legTyped.Count; i++)
        {
            Leg leg = legTyped[i];
            if (leg.is_broken == 0)
            {
                GameObject go = Instantiate(ui_InventoryPiecePrefab, parent);
                InventoryPieceItem pi = go.GetComponent<InventoryPieceItem>();

                bool equip = robot.leg_object_id == leg.object_id ? true : false;
                Sprite legSprite = GameManage.instance.AllLegPieceImages[leg.leg_id];
                pi.Init(PieceType.Leg, legSprite, leg.object_id, leg.life, leg.attack, leg.defence, leg.velocity, leg.level, (int)leg.ImagineTime(), equip);

                items.Add(go);
            }
        }
    }

    private void Clear()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Destroy(items[i]);
        }
    }
}
