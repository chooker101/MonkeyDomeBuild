using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public enum TargetBaseState
{
    Null,
    RallyStart,
    Hit,
    Miss,
    Pop,
    Prep,
    Warning,
    RallyEnd,
    TierUpdate
}
public class TargetBase : MonoBehaviour
{
    public enum TierStatus
    {
        TierNull,
        TierUp,
        TierStay,
        TierDown,
        TierEmpty,
        TierMax
    }
    Target target;
    public TargetBaseState state = TargetBaseState.Null;
    public TierStatus tierStatus = TierStatus.TierNull;
    public Transform slider;
    //public Transform sliderBackground;
    public SpriteRenderer sliderTop; // New image slider
    public SpriteRenderer sliderTopBackground;
    public SpriteRenderer sliderBottom; // New image slider
    public SpriteRenderer sliderBottomBackground;
    public SpriteRenderer start;
    public SpriteRenderer end;
    public SpriteRenderer hit;
    public SpriteRenderer hit_white;
    public SpriteRenderer miss;
    public SpriteRenderer active;
    public SpriteRenderer prep;
    public SpriteRenderer warning;
    public SpriteRenderer tierUp;
    public SpriteRenderer tierUp_white;
    public SpriteRenderer tierStay;
    public SpriteRenderer tierStay_white;
    public SpriteRenderer tierDown;
    public SpriteRenderer tierDown_white;
    public SpriteRenderer tierMax;

    public SpriteRenderer upTierLight;
    public SpriteRenderer stayTierLight;
    public SpriteRenderer downTierLight;
    public SpriteRenderer targetCounter_black;
    public SpriteRenderer targetCounter_white;
    public SpriteRenderer targetCounter_red;
    public SpriteRenderer targetCounterDowngrade;
    public SpriteRenderer targetCounterSamegrade;
    public SpriteRenderer targetCounterUpgrade;

    private TargetManager manager;
    private Color color;

    float timeCount = 0;
    float defaultTime = 2f;
    private bool changeTier = false;

    private GameObject activatedImg = null;
    void Start()
    {
        manager = FindObjectOfType<TargetManager>();
        changeTier = true;
        //ChangeTargetState(TargetBaseState.RallyStart);
    }

    void Update()
    {
        PanelUpdate();
        TierUpdate();
    }
    void PanelUpdate()
    {
        switch (state)
        {
            case TargetBaseState.RallyStart:
                StartState(2f, false);
                break;
            case TargetBaseState.Hit:
                HitState();
                break;
            case TargetBaseState.Miss:
                MissState();
                break;
            case TargetBaseState.Pop:
                PopState();
                break;
            case TargetBaseState.Prep:
                PrepState();
                break;
            case TargetBaseState.Warning:
                WarningState();
                break;
            case TargetBaseState.RallyEnd:
                EndState();
                break;
            case TargetBaseState.TierUpdate:
                TierUpdateState();
                break;
        }
    }
    void TierUpdate()
    {
        if (GameManager.Instance.gmTargetManager.RallyOn)
        {
            if (GameManager.Instance.gmTargetManager.HitSum >= 3)
            {
                if (GameManager.Instance.gmTargetManager.IsAtMaxTier)
                {
                    if (tierStatus != TierStatus.TierMax)
                    {
                        tierStatus = TierStatus.TierMax;
                        changeTier = true;
                    }
                }
                else
                {
                    if (tierStatus != TierStatus.TierUp)
                    {
                        tierStatus = TierStatus.TierUp;
                        changeTier = true;
                    }
                }
            }
            else if (GameManager.Instance.gmTargetManager.HitSum >= 2)
            {
                if (tierStatus != TierStatus.TierStay)
                {
                    tierStatus = TierStatus.TierStay;
                    changeTier = true;
                }
            }
            else if (GameManager.Instance.gmTargetManager.HitSum >= 1)
            {
                if (tierStatus != TierStatus.TierDown)
                {
                    tierStatus = TierStatus.TierDown;
                    changeTier = true;
                }
            }
            else
            {
                if (tierStatus != TierStatus.TierEmpty)
                {
                    tierStatus = TierStatus.TierEmpty;
                    changeTier = true;
                }
            }
        }
        else
        {
            if (tierStatus != TierStatus.TierNull)
            {
                //tierStatus = TierStatus.TierNull;
                //changeTier = true;
            }
        }
        if (changeTier)
        {
            changeTier = false;
            if(state != TargetBaseState.RallyStart && state != TargetBaseState.TierUpdate)
            {
                switch (tierStatus)
                {
                    case TierStatus.TierNull:
                        targetCounter_black.gameObject.SetActive(true);
                        targetCounter_white.gameObject.SetActive(false);
                        targetCounter_red.gameObject.SetActive(false);
                        targetCounterDowngrade.gameObject.SetActive(false);
                        targetCounterSamegrade.gameObject.SetActive(false);
                        targetCounterUpgrade.gameObject.SetActive(false);
                        //upTierLight.color = Color.black;
                        //stayTierLight.color = Color.black;
                        //downTierLight.color = Color.black;
                        break;
                    case TierStatus.TierUp:
                        targetCounter_black.gameObject.SetActive(false);
                        targetCounter_white.gameObject.SetActive(false);
                        targetCounter_red.gameObject.SetActive(false);
                        targetCounterDowngrade.gameObject.SetActive(false);
                        targetCounterSamegrade.gameObject.SetActive(false);
                        targetCounterUpgrade.gameObject.SetActive(true);
                        //upTierLight.color = Color.green;
                        //stayTierLight.color = Color.green;
                        //downTierLight.color = Color.green;
                        break;
                    case TierStatus.TierStay:
                        targetCounter_black.gameObject.SetActive(false);
                        targetCounter_white.gameObject.SetActive(false);
                        targetCounter_red.gameObject.SetActive(false);
                        targetCounterDowngrade.gameObject.SetActive(false);
                        targetCounterSamegrade.gameObject.SetActive(true);
                        targetCounterUpgrade.gameObject.SetActive(false);
                        //upTierLight.color = Color.black;
                        //stayTierLight.color = Color.yellow;
                        //downTierLight.color = Color.yellow;
                        break;
                    case TierStatus.TierDown:
                        targetCounter_black.gameObject.SetActive(false);
                        targetCounter_white.gameObject.SetActive(false);
                        targetCounter_red.gameObject.SetActive(false);
                        targetCounterDowngrade.gameObject.SetActive(true);
                        targetCounterSamegrade.gameObject.SetActive(false);
                        targetCounterUpgrade.gameObject.SetActive(false);
                        //upTierLight.color = Color.black;
                        //stayTierLight.color = Color.black;
                        //downTierLight.color = Color.red;
                        break;
                    case TierStatus.TierEmpty:
                        targetCounter_black.gameObject.SetActive(false);
                        targetCounter_white.gameObject.SetActive(false);
                        targetCounter_red.gameObject.SetActive(true);
                        targetCounterDowngrade.gameObject.SetActive(false);
                        targetCounterSamegrade.gameObject.SetActive(false);
                        targetCounterUpgrade.gameObject.SetActive(false);
                        //upTierLight.color = Color.black;
                        //stayTierLight.color = Color.black;
                        //downTierLight.color = Color.red;
                        break;
                }
            }
            else
            {
                targetCounter_black.gameObject.SetActive(false);
                targetCounter_white.gameObject.SetActive(true);
                targetCounter_red.gameObject.SetActive(false);
                targetCounterDowngrade.gameObject.SetActive(false);
                targetCounterSamegrade.gameObject.SetActive(false);
                targetCounterUpgrade.gameObject.SetActive(false);
            }
        }
    }
    public void ChangeTargetState(TargetBaseState state)
    {
        if(this.state != state)
        {
            Vector3 sliderNewScale = slider.localScale;
            sliderNewScale.x = 0;
            slider.localScale = sliderNewScale;
            //sliderBackground.localScale = sliderNewScale;
            timeCount = 0;

            this.state = state;
            if (activatedImg != null)
            {
                activatedImg.SetActive(false);

                // Set images of screen
                ColorUtility.TryParseHtmlString("#1C1D1E", out color); // "black" color
                sliderTopBackground.color = color;
                sliderBottomBackground.color = color;
                if(state != TargetBaseState.TierUpdate && state != TargetBaseState.RallyStart && state != TargetBaseState.Warning)
                {
                    switch (tierStatus)
                    {
                        default:
                            goto case TierStatus.TierDown;
                        case TierStatus.TierUp:
                            // Set images of screen
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(true);
                            tierMax.gameObject.SetActive(false);
                            break;
                        case TierStatus.TierStay:
                            // Set images of screen
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(true);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            tierMax.gameObject.SetActive(false);
                            break;
                        case TierStatus.TierDown:
                            // Set images of screen
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(true);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            tierMax.gameObject.SetActive(false);
                            break;
                        case TierStatus.TierMax:
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            tierMax.gameObject.SetActive(true);
                            break;
                        case TierStatus.TierNull:
                            targetCounter_black.gameObject.SetActive(true);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            break;
                    }
                }
            }
            switch (state)
            {
                case TargetBaseState.Null:
                    // Set images of screen
                    targetCounter_black.gameObject.SetActive(true);
                    targetCounter_white.gameObject.SetActive(false);
                    targetCounter_red.gameObject.SetActive(false);
                    targetCounterDowngrade.gameObject.SetActive(false);
                    targetCounterSamegrade.gameObject.SetActive(false);
                    targetCounterUpgrade.gameObject.SetActive(false);

                    break;
                case TargetBaseState.RallyStart:
                    activatedImg = start.gameObject;

                    // Set images of screen
                    sliderTopBackground.color = Color.white;
                    sliderBottomBackground.color = Color.white;
                    targetCounter_black.gameObject.SetActive(false);
                    targetCounter_white.gameObject.SetActive(true);
                    targetCounter_red.gameObject.SetActive(false);
                    targetCounterDowngrade.gameObject.SetActive(false);
                    targetCounterSamegrade.gameObject.SetActive(false);
                    targetCounterUpgrade.gameObject.SetActive(false);

                    break;
                case TargetBaseState.Hit:
                    activatedImg = hit.gameObject;
                    break;
                case TargetBaseState.Miss:
                    activatedImg = miss.gameObject;
                    break;
                case TargetBaseState.Pop:
                    activatedImg = active.gameObject;

                    // Set images of screen
                    color = Color.white;
                    sliderTopBackground.color = color;
                    sliderBottomBackground.color = color;
                    ColorUtility.TryParseHtmlString("#58585bFF", out color); // "Grey" colour
                    sliderTop.color = color;
                    sliderBottom.color = color;
                    targetCounter_black.gameObject.SetActive(false);
                    targetCounter_white.gameObject.SetActive(false);
                    targetCounter_red.gameObject.SetActive(true);
                    targetCounterDowngrade.gameObject.SetActive(false);
                    targetCounterSamegrade.gameObject.SetActive(false);
                    targetCounterUpgrade.gameObject.SetActive(false);
                    break;
                case TargetBaseState.Prep:
                    activatedImg = prep.gameObject;

                    // Set images of screen
                    ColorUtility.TryParseHtmlString("#1C1D1E", out color); // "Black" color
                    sliderTopBackground.color = color;
                    sliderBottomBackground.color = color;
                    targetCounter_black.gameObject.SetActive(true);
                    targetCounter_white.gameObject.SetActive(false);
                    targetCounter_red.gameObject.SetActive(false);
                    targetCounterDowngrade.gameObject.SetActive(false);
                    targetCounterSamegrade.gameObject.SetActive(false);
                    targetCounterUpgrade.gameObject.SetActive(false);
                    break;
                case TargetBaseState.Warning:
                    activatedImg = warning.gameObject;

                    // Set images of screen
                    color = Color.white;
                    sliderTopBackground.color = color;
                    sliderBottomBackground.color = color;
                    ColorUtility.TryParseHtmlString("#58585bFF", out color); // "Grey" colour
                    sliderTop.color = color;
                    sliderBottom.color = color;
                    break;
                case TargetBaseState.RallyEnd:
                    activatedImg = end.gameObject;
                    StartCoroutine(StartUpdateTargetTierState());

                    // Set images of screen
                    sliderTopBackground.color = Color.white;
                    sliderBottomBackground.color = Color.white;
                    targetCounter_black.gameObject.SetActive(false);
                    targetCounter_white.gameObject.SetActive(true);
                    targetCounter_red.gameObject.SetActive(false);
                    targetCounterDowngrade.gameObject.SetActive(false);
                    targetCounterSamegrade.gameObject.SetActive(false);
                    targetCounterUpgrade.gameObject.SetActive(false);
                    break;
                case TargetBaseState.TierUpdate:
                    switch (tierStatus)
                    {
                        default:
                            goto case TierStatus.TierDown;
                        case TierStatus.TierUp:
                            activatedImg = tierUp.gameObject;

                            // Set images of screen
                            ColorUtility.TryParseHtmlString("#8cc63eFF", out color); // "Green" colour
                            sliderTopBackground.color = color;
                            sliderBottomBackground.color = color;
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(true);
                            break;
                        case TierStatus.TierStay:
                            activatedImg = tierStay.gameObject;

                            // Set images of screen
                            ColorUtility.TryParseHtmlString("#f8ed31FF", out color); // "Yellow" colour
                            sliderTopBackground.color = color;
                            sliderBottomBackground.color = color;
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(true);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            break;
                        case TierStatus.TierDown:
                            activatedImg = tierDown.gameObject;

                            // Set images of screen
                            ColorUtility.TryParseHtmlString("#ed1c24FF", out color); // "Red" colour
                            sliderTopBackground.color = color;
                            sliderBottomBackground.color = color;
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(false);
                            targetCounterDowngrade.gameObject.SetActive(true);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            break;
                        case TierStatus.TierEmpty:
                            activatedImg = tierDown.gameObject;

                            // Set images of screen
                            ColorUtility.TryParseHtmlString("#ed1c24FF", out color); // "Red" colour
                            sliderTopBackground.color = color;
                            sliderBottomBackground.color = color;
                            targetCounter_black.gameObject.SetActive(false);
                            targetCounter_white.gameObject.SetActive(false);
                            targetCounter_red.gameObject.SetActive(true);
                            targetCounterDowngrade.gameObject.SetActive(false);
                            targetCounterSamegrade.gameObject.SetActive(false);
                            targetCounterUpgrade.gameObject.SetActive(false);
                            break;
                    }
                    break;
            }
        }
    }
    void StartState(float time, bool useSlider)
    {
        if (!activatedImg.activeInHierarchy && timeCount == 0)
        {
            activatedImg.SetActive(true);
            timeCount = time;
        }
        if (timeCount <= 0)
        {
            switch (state)
            {
                default:
                    ChangeTargetState(TargetBaseState.Null);
                    break;
                case TargetBaseState.RallyStart:
                    ChangeTargetState(TargetBaseState.Prep);
                    break;
                case TargetBaseState.Warning:
                    ChangeTargetState(TargetBaseState.RallyEnd);
                    break;
                case TargetBaseState.Prep:
                    ChangeTargetState(TargetBaseState.Pop);
                    break;
                case TargetBaseState.Pop:
                    ChangeTargetState(TargetBaseState.Warning);
                    break;
            }
        }
        else
        {
            if (useSlider)
            {
                Vector3 sliderNewScale = slider.localScale;
                sliderNewScale.x = (time - timeCount) / time;
                slider.localScale = sliderNewScale;
                //sliderBackground.localScale = sliderNewScale;
            }
            /*if (GetComponentInParent<TargetNode>().stand.IsActivated)
            {
                if(state != TargetBaseState.Warning)
                {
                    timeCount -= Time.deltaTime;
                }
            }
            else
            {
                timeCount -= Time.deltaTime;
            }*/
            timeCount -= Time.deltaTime;
        }
    }
    void HitState()
    {
        StartState(defaultTime, false);
    }
    void MissState()
    {
        StartState(defaultTime, false);
    }
    void PopState()
    {
        if (!activatedImg.activeInHierarchy)
        {
            AudioEffectManager.Instance.PlayTargetUpgradeSE();
            activatedImg.SetActive(true);
            timeCount = manager.SetLifeTime() - manager.warningTime;
        }
        Vector3 sliderNewScale = slider.localScale;
        sliderNewScale.x = (manager.SetLifeTime() - manager.warningTime - timeCount) / (manager.SetLifeTime() - manager.warningTime);
        /*if (!GetComponentInParent<TargetNode>().stand.IsActivated)
        {
            timeCount -= Time.deltaTime;
        }*/
        if (timeCount <= 0)
        {
            ChangeTargetState(TargetBaseState.Warning);
        }
        else
        {
            timeCount -= Time.deltaTime;
        }
        slider.localScale = sliderNewScale;
        //sliderBackground.localScale = sliderNewScale;
    }
    void PrepState()
    {
        StartState(manager.prepTime, false);
        //Vector3 sliderNewScale = slider.localScale;
        //sliderNewScale.x = (target.PrepTime - timeCount) / target.PrepTime;
        //timeCount -= Time.deltaTime;
        //slider.localScale = sliderNewScale;
    }
    void WarningState()
    {
        StartState(manager.warningTime, true);
    }
    void EndState()
    {
        StartState(3f, false);
    }
    void TierUpdateState()
    {
        StartState(2f, false);
    }
    public Target SetTarget
    {
        set
        {
            target = value;
        }
    }
    IEnumerator StartUpdateTargetTierState()
    {
        yield return new WaitForSeconds(3f);
        ChangeTargetState(TargetBaseState.TierUpdate);
    }

}
