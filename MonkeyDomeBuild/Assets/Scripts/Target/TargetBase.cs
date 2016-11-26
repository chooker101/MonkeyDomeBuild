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
        TierDown
    }
    Target target;
    public TargetBaseState state = TargetBaseState.Null;
    public TierStatus tierStatus = TierStatus.TierNull;
    public Transform slider;
    public Transform slider2;
    public SpriteRenderer start;
    public SpriteRenderer end;
    public SpriteRenderer hit;
    public SpriteRenderer miss;
    public SpriteRenderer pop;
    public SpriteRenderer prep;
    public SpriteRenderer warning;
    public SpriteRenderer upTierLight;
    public SpriteRenderer stayTierLight;
    public SpriteRenderer downTierLight;
    public SpriteRenderer tierUp;
    public SpriteRenderer tierStay;
    public SpriteRenderer tierDown;

    private TargetManager manager;

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
                if (tierStatus != TierStatus.TierUp)
                {
                    tierStatus = TierStatus.TierUp;
                    changeTier = true;
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
            else
            {
                if (tierStatus != TierStatus.TierDown)
                {
                    tierStatus = TierStatus.TierDown;
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
            switch (tierStatus)
            {
                case TierStatus.TierNull:
                    upTierLight.color = Color.black;
                    stayTierLight.color = Color.black;
                    downTierLight.color = Color.black;
                    break;
                case TierStatus.TierUp:
                    upTierLight.color = Color.green;
                    stayTierLight.color = Color.green;
                    downTierLight.color = Color.green;
                    break;
                case TierStatus.TierStay:
                    upTierLight.color = Color.black;
                    stayTierLight.color = Color.yellow;
                    downTierLight.color = Color.yellow;
                    break;
                case TierStatus.TierDown:
                    upTierLight.color = Color.black;
                    stayTierLight.color = Color.black;
                    downTierLight.color = Color.red;
                    break;
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
            slider2.localScale = sliderNewScale;
            timeCount = 0;
            this.state = state;
            if (activatedImg != null)
            {
                activatedImg.SetActive(false);
            }
            switch (state)
            {
                case TargetBaseState.RallyStart:
                    activatedImg = start.gameObject;
                    break;
                case TargetBaseState.Hit:
                    activatedImg = hit.gameObject;
                    break;
                case TargetBaseState.Miss:
                    activatedImg = miss.gameObject;
                    break;
                case TargetBaseState.Pop:
                    activatedImg = pop.gameObject;
                    break;
                case TargetBaseState.Prep:
                    activatedImg = prep.gameObject;
                    break;
                case TargetBaseState.Warning:
                    activatedImg = warning.gameObject;
                    break;
                case TargetBaseState.RallyEnd:
                    activatedImg = end.gameObject;
                    StartCoroutine(StartUpdateTargetTierState());
                    break;
                case TargetBaseState.TierUpdate:
                    switch (tierStatus)
                    {
                        default:
                            goto case TierStatus.TierDown;
                        case TierStatus.TierUp:
                            activatedImg = tierUp.gameObject;
                            break;
                        case TierStatus.TierStay:
                            activatedImg = tierStay.gameObject;
                            break;
                        case TierStatus.TierDown:
                            activatedImg = tierDown.gameObject;
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
                slider2.localScale = sliderNewScale;
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
        slider2.localScale = sliderNewScale;
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
