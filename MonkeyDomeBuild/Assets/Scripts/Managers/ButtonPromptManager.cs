using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonPromptManager : MonoBehaviour
{
    public List<Transform> climbPrompts = new List<Transform>();
    public List<Transform> catchPrompts = new List<Transform>();
    public List<bool> climbConditions = new List<bool>();
    public List<bool> catchConditions = new List<bool>();

    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            climbConditions.Add(false);
            catchConditions.Add(false);
        }
        for(int i = 0; i < 4; i++)
        {
            GameObject temp = (GameObject)Instantiate(climbPrompts[0].gameObject, transform.position, Quaternion.identity);
            temp.transform.SetParent(transform);
            climbPrompts.Add(temp.transform);
            temp.SetActive(false);

            GameObject temp2 = (GameObject)Instantiate(catchPrompts[0].gameObject, transform.position, Quaternion.identity);
            temp2.transform.SetParent(transform);
            catchPrompts.Add(temp2.transform);
            temp2.SetActive(false);
        }
    }

    void Update()
    {
        for(int i = 0; i < GameManager.Instance.TotalNumberofPlayers; i++)
        {
            if (GameManager.Instance.gmPlayerScripts[i].canClimb && !GameManager.Instance.gmPlayerScripts[i].isClimbing && GameManager.Instance.gmPlayerScripts[i].IsInAir)
            {
                if (!climbConditions[i])
                {
                    climbConditions[i] = true;
                }
            }
            else
            {
                if (climbConditions[i])
                {
                    climbConditions[i] = false;
                }
            }
            if(GameManager.Instance.gmPlayerScripts[i].ballInRange && !GameManager.Instance.gmPlayerScripts[i].IsHoldingBall)
            {
                if (!catchConditions[i])
                {
                    catchConditions[i] = true;
                }
            }
            else
            {
                if (catchConditions[i])
                {
                    catchConditions[i] = false;
                }
            }
        }
    }
    void LateUpdate()
    {
        UpdateConditon(climbPrompts, climbConditions, new Vector3(4f,4f));
        UpdateConditon(catchPrompts, catchConditions, new Vector3(4f, 2.5f));
    }
    void UpdateConditon(List<Transform> prompts, List<bool> conditions, Vector3 offset)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (conditions[i])
            {
                if (!prompts[i].gameObject.activeInHierarchy)
                {
                    Color c = prompts[i].GetComponentInChildren<Text>().color;
                    c.a = 0;
                    prompts[i].GetComponentInChildren<Text>().color = c;
                    prompts[i].gameObject.SetActive(true);
                }
                else
                {
                    Color c = prompts[i].GetComponentInChildren<Text>().color;
                    Color cf = c;
                    cf.a = 1f;
                    c = Color.Lerp(c, cf, Time.deltaTime * 10f);
                    prompts[i].GetComponentInChildren<Text>().color = c;
                }
                Vector3 newPos = GameManager.Instance.gmPlayerScripts[i].transform.position + offset;
                newPos.z = prompts[i].position.z;
                prompts[i].position = newPos;
            }
            else
            {
                if (prompts[i].GetComponentInChildren<Text>().color.a > 0.05f)
                {
                    Color c = prompts[i].GetComponentInChildren<Text>().color;
                    Color cf = c;
                    cf.a = 0;
                    c = Color.Lerp(c, cf, Time.deltaTime * 20f);
                    prompts[i].GetComponentInChildren<Text>().color = c;
                    Vector3 newPos = GameManager.Instance.gmPlayerScripts[i].transform.position + offset;
                    newPos.z = prompts[i].position.z;
                    prompts[i].position = newPos;
                }
                else
                {
                    if (prompts[i].gameObject.activeInHierarchy)
                    {
                        prompts[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }

}
