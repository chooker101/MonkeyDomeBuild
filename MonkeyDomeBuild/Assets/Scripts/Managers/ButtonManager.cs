using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    //private Button myButton;

	// Use this for initialization
	void Awake () {
        //myButton = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void StartButton()
    {
        SceneManager.LoadScene("test01");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
