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
    Warning
}
public class TargetBase : MonoBehaviour
{
    public TargetBaseState state = TargetBaseState.Null;
    public Image start;
    public Image hit;
    public Image miss;
    public Image pop;
    public Image prep;
    public Image warning;

    bool canFlash = false;

    float timeCount = 0;

    private GameObject activatedImg = null;

    void Start()
    {
        ChangeTargetState(TargetBaseState.RallyStart);
    }


    void Update()
    {
        switch (state)
        {
            case TargetBaseState.RallyStart:
                StartState();
                break;
            case TargetBaseState.Hit:
                StartState();
                break;
            case TargetBaseState.Miss:
                StartState();
                break;
            case TargetBaseState.Pop:
                StartState();
                break;
            case TargetBaseState.Prep:
                StartState();
                break;
            case TargetBaseState.Warning:
                StartState();
                break;
        }
    }
    public void ChangeTargetState(TargetBaseState state)
    {
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
        }

    }
    void StartState()
    {
        if (!activatedImg.activeInHierarchy && timeCount == 0)
        {
            activatedImg.SetActive(true);
            timeCount = 2f;
        }
        if (timeCount <= 0)
        {
            ChangeTargetState(TargetBaseState.Null);
        }
        else
        {
            timeCount -= Time.deltaTime;
        }
    }
    void HitState()
    {

    }
    void MissState()
    {

    }
    void PopState()
    {

    }
    void PrepState()
    {

    }
    void WarningState()
    {

    }
}
