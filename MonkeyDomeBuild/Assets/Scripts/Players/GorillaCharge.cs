using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GorillaCharge : MonoBehaviour
{
    public Image circularSlider;
    private Image sliderBackground;
    public Text chargeCountText;
    public float chargeCount;
    private float maxChargeTime;
    void Start()
    {
        //chargeCountText.gameObject.SetActive(false);
        sliderBackground = transform.FindChild("ChargeBgImage").GetComponent<Image>();
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
                sliderBackground.gameObject.SetActive(true);

                chargeCountText.text = Math.Round((maxChargeTime - chargeCount)+1).ToString();
                circularSlider.fillAmount = chargeCount / maxChargeTime;
            }
            else
            {
                chargeCountText.text = "";
                circularSlider.fillAmount = 0;
                sliderBackground.gameObject.SetActive(false);
            }
        }
        else
        {
            if (chargeCountText.text != "")
            {
                chargeCountText.text = "";
            }
            if (circularSlider.fillAmount != 0)
                circularSlider.fillAmount = 0;
            sliderBackground.gameObject.SetActive(false);

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
