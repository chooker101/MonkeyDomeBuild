﻿using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GorillaCharge : MonoBehaviour
{
    public Image circularSilder;
    public Text chargeCountText;
    public float chargeCount;
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
        if (GetComponentInParent<Actor>().characterType is Gorilla)
        {
            if (chargeCount > 0.5f && chargeCount < maxChargeTime)
            {
                chargeCountText.text = Math.Round(chargeCount, 2).ToString();
                circularSilder.fillAmount = chargeCount / maxChargeTime;
            }
            else
            {
                chargeCountText.text = "";
                circularSilder.fillAmount = 0;
            }
        }
        else
        {
            if (chargeCountText.text != "")
            {
                chargeCountText.text = "";
            }
            if (circularSilder.fillAmount != 0)
                circularSilder.fillAmount = 0;

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
