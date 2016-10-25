using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {

    private TurretsManager turretManager;

    public enum Event
    {
        Banana,
        Poop,
        None
    }

    private int randomEvent;

    private bool eventActive = false;
    private float eventTimer;

    public GameObject[] turretTargetLocations;
    public int launchTotal;

    void Start () {
        turretManager = GetComponentInParent<TurretsManager>();
        NoneEvent();
    }

	void Update () {
        Debug.Log(randomEvent);
	}

    void Events()
    {

        randomEvent = Random.Range(0, 3);
        switch (randomEvent)
        {
            case (int)Event.Banana:
                BananaEvent();
                break;
            case (int)Event.Poop:
                PoopEvent();
                break;
            case (int)Event.None:
                break;

        }
    }

    void BananaEvent()
    {
        eventActive = true;
        eventTimer = Random.Range(6f, 10f);
        for (int i = 0; i < launchTotal; i++)
        {
            turretManager.AddFireQueue(turretTargetLocations[Random.Range(0, (int)turretTargetLocations.Length)], 0);
        }
        StartCoroutine(EventTime());
    }

    void PoopEvent()
    {
        eventActive = true;
        eventTimer = Random.Range(6f, 10f);
        for (int i = 0; i < launchTotal; i++)
        {
            turretManager.AddFireQueue(turretTargetLocations[Random.Range(0, (int)turretTargetLocations.Length)], 1);
        }
        StartCoroutine(EventTime());
    }

    void NoneEvent()
    {
        randomEvent = (int)Event.None;
        eventActive = true;
        eventTimer = Random.Range(3f, 4f);
        StartCoroutine(EventTime());
    }

    IEnumerator EventTime()
    {
        yield return new WaitForSeconds(eventTimer);
        eventActive = false;
        StopAllCoroutines();
        switch (randomEvent)
        {
            case (int)Event.Banana:
                NoneEvent();
                break;

            case (int)Event.Poop:
                NoneEvent();
                break;
            case (int)Event.None:
                Events();
                break;

        }
    }

}
