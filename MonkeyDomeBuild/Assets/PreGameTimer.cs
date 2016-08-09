using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreGameTimer : MonoBehaviour
{

    Text text;
    public float timer;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(timer <= 0)
        {
            Application.LoadLevel("testingroom");
        }
        text.text = "Pre-game Room\n" + timer.ToString("F2");
    }
}
