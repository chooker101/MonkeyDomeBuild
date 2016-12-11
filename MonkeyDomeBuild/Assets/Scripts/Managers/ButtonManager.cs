using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    private Sprite spriteNormal;
    private Sprite spriteHover;
    //private Button myButton;

    public AudioSource audioPress;

    // Use this for initialization
    void Awake()
    {
        //myButton = GetComponent<Button>();

        spriteNormal = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/ArtAssets/Menu/Monkey-Dome-Title-Target.png", typeof(Sprite));
        spriteHover = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/ArtAssets/Menu/Monkey-Dome-Title-Target-Selected.png", typeof(Sprite));
        audioPress = gameObject.GetComponent<AudioSource>();
        gameObject.GetComponent<Image>().sprite = spriteNormal;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartButton()
    {
        SceneManager.LoadScene("PregameRoom");
        PlayButtonSE();
    }
    public void PointerEnter()
    {
        gameObject.GetComponent<Image>().sprite = spriteHover;
    }
    public void PointerExit()
    {
        gameObject.GetComponent<Image>().sprite = spriteNormal;
    }

    public void QuitButton()
    {
        Application.Quit();
        PlayButtonSE();
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
        PlayButtonSE();
    }

    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
        PlayButtonSE();
    }

    public void PlayButtonSE()
    {
        if (audioPress != null && audioPress.clip != null)
        {
            audioPress.pitch = Random.Range(.75f, 1f);
            audioPress.PlayOneShot(audioPress.clip);
        }
    }
}
