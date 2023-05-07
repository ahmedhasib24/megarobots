using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour
{
    public Image imgHead;
    public Image imgLArm;
    public Image imgRArm;
    public Image imgLeg;

    public Text txtName, txtHead, txtLArm, txtRarm, txtLeg;

    public void Show(string name, float head, float lArm, float rArm, float leg, float headMax, float lArmMax, float rArmMax, float legMax)
    {
        float headValue = (head / headMax) * 100f;
        float lArmValue = (lArm / lArmMax) * 100f;
        float rArmValue = (rArm / rArmMax) * 100f;
        float legValue = (leg / legMax) * 100f;

        if (txtName != null)
        {
            txtName.text = name;
        }
        //imgHead.fillAmount = headValue / 100f;
        //imgLArm.fillAmount = lArmValue / 100f;
        //imgRArm.fillAmount = rArmValue / 100f;
        //imgLeg.fillAmount = legValue / 100f;

        txtHead.text = (headValue >= 0 ? (Mathf.Ceil(headValue)).ToString() : 0.ToString()) + "%";
        txtLArm.text = (lArmValue >= 0 ? (Mathf.Ceil(lArmValue)).ToString() : 0.ToString()) + "%";
        txtRarm.text = (rArmValue >= 0 ? (Mathf.Ceil(rArmValue)).ToString() : 0.ToString()) + "%";
        txtLeg.text = (legValue >= 0 ? (Mathf.Ceil(legValue)).ToString() : 0.ToString()) + "%";

        StartCoroutine(UpdateValueOverTime(headValue, lArmValue, rArmValue, legValue));
    }

    IEnumerator UpdateValueOverTime(float headValue, float lArmValue, float rArmValue, float legValue)
    {
        float time = 3f;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            imgHead.fillAmount = Mathf.Lerp(imgHead.fillAmount, headValue / 100f, (elapsedTime / time));
            imgLArm.fillAmount = Mathf.Lerp(imgLArm.fillAmount, lArmValue / 100f, (elapsedTime / time));
            imgRArm.fillAmount = Mathf.Lerp(imgRArm.fillAmount, rArmValue / 100f, (elapsedTime / time));
            imgLeg.fillAmount = Mathf.Lerp(imgLeg.fillAmount, legValue / 100f, (elapsedTime / time));


            //txtHead.text = string.IsNullOrEmpty(txtHead.text) ? "100%" : txtHead.text;
            //txtLArm.text = string.IsNullOrEmpty(txtLArm.text) ? "100%" : txtLArm.text;
            //txtRarm.text = string.IsNullOrEmpty(txtRarm.text) ? "100%" : txtRarm.text;
            //txtLeg.text = string.IsNullOrEmpty(txtLeg.text) ? "100%" : txtLeg.text;

            //string headText = txtHead.text.Trim(new char[] { '%' });
            //string larmText = txtLArm.text.Trim(new char[] { '%' });
            //string rarmText = txtRarm.text.Trim(new char[] { '%' });
            //string legText = txtLeg.text.Trim(new char[] { '%' });
            //txtHead.text = Mathf.Lerp(int.Parse(headText), headValue >= 0 ? (int)headValue : 0, elapsedTime / time).ToString() + "%";
            //txtLArm.text = Mathf.Lerp(int.Parse(larmText), headValue >= 0 ? (int)lArmValue : 0, elapsedTime / time).ToString() + "%";
            //txtRarm.text = Mathf.Lerp(int.Parse(rarmText), headValue >= 0 ? (int)rArmValue : 0, elapsedTime / time).ToString() + "%";
            //txtLeg.text = Mathf.Lerp(int.Parse(legText), headValue >= 0 ? (int)legValue : 0, elapsedTime / time).ToString() + "%";
            
            yield return null;
        }

        //yield return new WaitForSeconds(5);
        //gameObject.SetActive(false);
    }
}
