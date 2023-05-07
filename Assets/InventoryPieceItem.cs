using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPieceItem : MonoBehaviour
{
    public Image imgAvater;
    public GameObject velocityItem;
    public Slider sliderLife, sliderAttack, sliderDefence, sliderVelocity;
    public Text txtLife, txtAttack, txtDefence, txtVelocity;
    public Text txtLevel;


    public Text txtImgTime;
    public Button btnChange;
    public Text txtEquip;

    PieceType type;
    int id;
    int level;

    public void Init(PieceType type, Sprite avaterSprite, int id, float life, float attack, float defence, float velocity, int level, float imgTime, bool equip)
    {
        this.type = type;
        this.id = id;
        this.level = level;

        imgAvater.sprite = avaterSprite;

        sliderLife.value = life / ((life / level) * 5);
        txtLife.text = life.ToString();
        sliderAttack.value = attack / ((attack / level) * 5);
        txtAttack.text = attack.ToString();
        sliderDefence.value = defence / ((defence / level) * 5);
        txtDefence.text = defence.ToString();

        if (type == PieceType.Leg)
        {
            sliderVelocity.value = velocity / ((velocity / level) * 5);
            txtVelocity.text = velocity.ToString();
            velocityItem.SetActive(true);
        }
        else
        {
            sliderVelocity.value = 0;
            txtVelocity.text = 0.ToString();
            velocityItem.SetActive(false);
        }

        txtLevel.text = "Level : " + level.ToString();
        txtImgTime.text = imgTime + "h";
        if (equip == true)
        {
            btnChange.interactable = false;
            txtEquip.text = "Equiped";
        }
        else
        {
            btnChange.interactable = true;
            txtEquip.text = "Change";
        }
        //btnChange.onClick.AddListener()
    }

    public void ChangePiece()
    {
        FindObjectOfType<Inventory>().ChangePiece(type, id, level);
    }
}
