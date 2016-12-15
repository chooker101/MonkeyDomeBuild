using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuCanvas : MonoBehaviour
{
    GameObject currentlySelected;
    public EventSystem eventSystem;

    void Start()
    {
        currentlySelected = eventSystem.firstSelectedGameObject;
    }


    void Update()
    {
        if(eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(currentlySelected);
        }
        else
        {
            if (eventSystem.currentSelectedGameObject != currentlySelected)
            {
                currentlySelected = eventSystem.currentSelectedGameObject;
            }
        }
    }
}
