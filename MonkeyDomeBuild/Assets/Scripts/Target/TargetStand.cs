using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetStand : MonoBehaviour
{
    Rigidbody2D rigid;

    Vector3 startLoc;
    Vector3 endLoc;
    public List<Actor> playersOnStand = new List<Actor>();
    bool pressed = false;
    bool activated = false;

    float timeNeededToActivate = 2f;
    float timeCount;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        startLoc = transform.localPosition;
        endLoc = startLoc + Vector3.down;
    }

    void Update()
    {
        if (playersOnStand.Count > 0)
        {
            if (!pressed)
            {
                pressed = true;
            }
        }
        else
        {
            if (pressed)
            {
                pressed = false;
            }
        }
        if (pressed)
        {
            if (timeCount >= timeNeededToActivate)
            {
                if (!activated)
                {
                    activated = true;
                }
            }
            else
            {
                timeCount += Time.deltaTime;
            }
            float tempTime = timeCount > timeNeededToActivate ? timeNeededToActivate : timeCount;
            Vector3 newLoc = Vector3.Lerp(startLoc, endLoc, tempTime / timeNeededToActivate);
            transform.localPosition = newLoc;
        }
        else
        {
            if (activated)
            {
                activated = false;
            }
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
            }
            else
            {
                if (timeCount != 0)
                {
                    timeCount = 0;
                }
            }
            float tempTime = timeCount < 0 ? 0 : timeCount;
            Vector3 newLoc = Vector3.Lerp(startLoc, endLoc, tempTime / timeNeededToActivate);
            transform.localPosition = newLoc;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (playersOnStand.Count > 0)
            {
                bool repeated = false;
                for (int i = 0; i < playersOnStand.Count; i++)
                {
                    if (other.GetComponent<Actor>().playerIndex == playersOnStand[i].playerIndex)
                    {
                        repeated = true;
                        break;
                    }
                }
                if (!repeated)
                {
                    playersOnStand.Add(other.GetComponent<Actor>());
                }
            }
            else
            {
                playersOnStand.Add(other.GetComponent<Actor>());
            }

        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnTriggerEnter2D(other);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < playersOnStand.Count; i++)
            {
                if (other.GetComponent<Actor>().playerIndex == playersOnStand[i].playerIndex)
                {
                    playersOnStand.RemoveAt(i);
                    break;
                }
            }
        }
    }
    public bool IsActivated
    {
        get
        {
            return activated;
        }
    }
}
