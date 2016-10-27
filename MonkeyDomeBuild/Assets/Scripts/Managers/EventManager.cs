using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * PAN CAMERA OUT WHEN EVENT ACTIVE!
 */

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
    private int noneCounter = 0;

    public GameObject[] turretTargetLocations;
    public int launchTotal;
    public Text BananaText;
    public Text PoopText;

    void Start () {
        turretManager = GetComponentInParent<TurretsManager>();
        NoneEvent();
        BananaText.gameObject.SetActive(false);
        PoopText.gameObject.SetActive(false);
    }

	void Update () {
	}

    void Events()
    {
        if (noneCounter == 2)
        {
            randomEvent = Random.Range(0, 3);
        }
        else
        {
            randomEvent = Random.Range(0, 2);
            noneCounter = 0;
        }

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
        PoopText.gameObject.SetActive(false);
        BananaText.gameObject.SetActive(true);
        eventTimer = Random.Range(8f, 12f);
        for (int i = 0; i < launchTotal; i++)
        {
            turretManager.AddFireQueue(turretTargetLocations[Random.Range(0, (int)turretTargetLocations.Length)], 0);
        }
        StartCoroutine(EventTime());
    }

    void PoopEvent()
    {
        eventActive = true;
        BananaText.gameObject.SetActive(false);
        PoopText.gameObject.SetActive(true);
        eventTimer = Random.Range(8f, 12f);
        for (int i = 0; i < launchTotal; i++)
        {
            turretManager.AddFireQueue(turretTargetLocations[Random.Range(0, (int)turretTargetLocations.Length)], 1);
        }
        StartCoroutine(EventTime());
    }

    void NoneEvent()
    {
        noneCounter++;
        BananaText.gameObject.SetActive(false);
        PoopText.gameObject.SetActive(false);
        randomEvent = (int)Event.None;
        eventActive = true;
        eventTimer = Random.Range(4f, 12f);
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
