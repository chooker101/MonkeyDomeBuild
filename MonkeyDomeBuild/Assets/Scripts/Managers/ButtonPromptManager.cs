﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonPromptManager : MonoBehaviour
{
    public Canvas canvas;
    public List<Transform> climbPrompts = new List<Transform>();
    public List<Transform> catchPrompts = new List<Transform>();
    public List<Transform> throwPrompts = new List<Transform>();
    public List<bool> climbConditions = new List<bool>();
    public List<bool> catchConditions = new List<bool>();
    public List<bool> throwConditions = new List<bool>();

    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            climbConditions.Add(false);
            catchConditions.Add(false);
            throwConditions.Add(false);
        }
        for(int i = 0; i < 4; i++)
        {
            GameObject temp = (GameObject)Instantiate(climbPrompts[0].gameObject, transform.position, Quaternion.identity);
            temp.transform.SetParent(canvas.transform);
            temp.transform.localScale = new Vector3(1f, 1f, 1f);
            climbPrompts.Add(temp.transform);
            temp.SetActive(false);

            temp = (GameObject)Instantiate(catchPrompts[0].gameObject, transform.position, Quaternion.identity);
            temp.transform.SetParent(canvas.transform);
            temp.transform.localScale = new Vector3(1f, 1f, 1f);
            catchPrompts.Add(temp.transform);
            temp.SetActive(false);

            temp = (GameObject)Instantiate(throwPrompts[0].gameObject, transform.position, Quaternion.identity);
            temp.transform.SetParent(canvas.transform);
            temp.transform.localScale = new Vector3(1f, 1f, 1f);
            throwPrompts.Add(temp.transform);
            temp.SetActive(false);
        }
    }

    void Update()
    {
        for(int i = 0; i < GameManager.Instance.TotalNumberofActors; i++)
        {
            ClimbCheck(i);
            CatchCheck(i);
            ThrowCheck(i);
        }
    }
    void LateUpdate()
    {
        UpdateConditon(climbPrompts, climbConditions, new Vector3(4f,3f));
        UpdateConditon(catchPrompts, catchConditions, new Vector3(-4f, 3f));
        UpdateConditon(throwPrompts, throwConditions, new Vector3(-4f, 3f));
    }
    void UpdateConditon(List<Transform> prompts, List<bool> conditions, Vector3 offset)
    {
        for (int i = 0; i < conditions.Count; i++)
        {
            if (i < GameManager.Instance.TotalNumberofActors)
            {
                if (conditions[i])
                {
                    if (!prompts[i].gameObject.activeInHierarchy)
                    {
                        Color c = prompts[i].GetComponentInChildren<Text>().color;
                        c.a = 0;
                        prompts[i].GetComponentInChildren<Text>().color = c;
                        if(prompts[i].GetComponentInChildren<Image>()!= null)
                        {
                            Color c2 = prompts[i].GetComponentInChildren<Image>().color;
                            c2.a = 0;
                            prompts[i].GetComponentInChildren<Image>().color = c2;
                        }
                        prompts[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        Color c = prompts[i].GetComponentInChildren<Text>().color;
                        Color cf = c;
                        cf.a = 1f;
                        c = Color.Lerp(c, cf, Time.deltaTime * 15f);
                        prompts[i].GetComponentInChildren<Text>().color = c;
                        if (prompts[i].GetComponentInChildren<Image>() != null)
                        {
                            Color c2 = prompts[i].GetComponentInChildren<Image>().color;
                            Color c2f = c2;
                            c2f.a = 1f;
                            c2 = Color.Lerp(c2, c2f, Time.deltaTime * 15f);
                            prompts[i].GetComponentInChildren<Image>().color = c2;
                        }
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
                        c = Color.Lerp(c, cf, Time.deltaTime * 10f);
                        prompts[i].GetComponentInChildren<Text>().color = c;
                        if (prompts[i].GetComponentInChildren<Image>() != null)
                        {
                            Color c2 = prompts[i].GetComponentInChildren<Image>().color;
                            Color c2f = c2;
                            c2f.a = 0;
                            c2 = Color.Lerp(c2, c2f, Time.deltaTime * 10f);
                            prompts[i].GetComponentInChildren<Image>().color = c2;
                        }

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
    void ClimbCheck(int index)
    {
        if (GameManager.Instance.gmPlayerScripts[index].canClimb && !GameManager.Instance.gmPlayerScripts[index].isClimbing && GameManager.Instance.gmPlayerScripts[index].IsInAir)
        {
            if (!climbConditions[index])
            {
                climbConditions[index] = true;
            }
        }
        else
        {
            if (climbConditions[index])
            {
                climbConditions[index] = false;
            }
        }
    }
    void CatchCheck(int index)
    {
        if (GameManager.Instance.gmPlayerScripts[index].ballInRange && !GameManager.Instance.gmPlayerScripts[index].IsHoldingBall && GameManager.Instance.gmPlayerScripts[index].CanCatch)
        {
            if (!catchConditions[index])
            {
                catchConditions[index] = true;
            }
        }
        else
        {
            if (catchConditions[index])
            {
                catchConditions[index] = false;
            }
        }
    }
    void ThrowCheck(int index)
    {
        if (GameManager.Instance.gmPlayerScripts[index].IsHoldingBall)
        {
            if (!throwConditions[index])
            {
                throwConditions[index] = true;
            }
        }
        else
        {
            if (throwConditions[index])
            {
                throwConditions[index] = false;
            }
        }
    }
}
