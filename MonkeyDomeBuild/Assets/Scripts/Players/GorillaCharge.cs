using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GorillaCharge : MonoBehaviour
{

    public Text chargeCountText;
    private float chargeCount;
    private float maxChargeTime;
    void Start()
    {
        //chargeCountText.gameObject.SetActive(false);
    }
    void OnEnable()
    {

    }
    void Update()
    {
        if (chargeCount > 0.5f && chargeCount < maxChargeTime)
        {
            chargeCountText.text = Math.Round(chargeCount, 2).ToString();
        }
        else
        {
            chargeCountText.text = "";
        }
    }
    public float ChargeCount
    {
        get { return chargeCount; }
        set { chargeCount = value; }
    }
    public float MaxChargeTime
    {
        set { maxChargeTime = value; }
    }
}
