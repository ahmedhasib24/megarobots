using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MallPieceItem : MonoBehaviour
{
    public Image imgType;
    public Text txtLevel;
    public Image imgSellerAvater;
    public Text txtSellerName;
    public Text txtPrice;

    public Image imgBackground;
    public int serialNumber = 0;
    private PieceMallItem config;

    public void Init(int serialNumber, PieceMallItem config)
    {
        this.serialNumber = serialNumber;
        this.config = config;
        imgType.sprite = MenuUIManager.instance.ComponentImageList[(int) config.piece_type];
        txtLevel.text = config.level.ToString();
        imgSellerAvater.sprite = MenuUIManager.instance.AvatarList[config.seller_avater_id];
        txtSellerName.text = config.seller_name;
        txtPrice.text = config.price.ToString();

        //if (serialNumber == 0)
        //{
        //    ShowItem();
        //}
    }

    public void ShowItem()
    {
        FindObjectOfType<OptionManager>().ButtonPlay();

        ShowItemInternal();
    }

    public void ShowItemInternal()
    {
        Mall.instance.ShowPieceDetails(serialNumber, config);
    }

    public void Highlight()
    {
        imgBackground.color = Color.green;
    }

    public void Downplay()
    {
        imgBackground.color = Color.white;
    }
}
