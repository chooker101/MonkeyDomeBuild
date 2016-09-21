using UnityEngine;
using System.Collections;

public class Freezer : MonoBehaviour {

    public float freezeTime;
    float timer = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Freeze();
        }
	}

    void Freeze()
    {
        Time.timeScale = 0;
        StartCoroutine(FreezeGame(freezeTime));
    }


    private IEnumerator FreezeGame(float freezeTime)
    {
        Debug.Log("Freezer Called");
        yield return new WaitForEndOfFrame();
        timer += Time.unscaledDeltaTime;
        if (timer < freezeTime)
        {
            StartCoroutine(FreezeGame(freezeTime));
        }
        else
        {
            Time.timeScale = 1;
        }
        timer = 0;
    }
}
