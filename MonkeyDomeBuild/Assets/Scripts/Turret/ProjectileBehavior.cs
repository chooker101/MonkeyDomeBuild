using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour
{
    private Rigidbody2D _rigid;
    public bool canCollideWithFloor = false;
    private bool canEffectCharacter = true;
    public float characterInc;

    private float fadeTime = 2f;
    private bool faded = false;

    private Color spriteColor = Color.white;
    

    public Sprite squishSprite;
    //private bool isBanana = true;
    private SpriteRenderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        characterInc = 0.5f;

    }

    void Update()
    {
        if (!canCollideWithFloor)
        {
            if (_rigid.velocity.y < 0f)
            {
                canCollideWithFloor = true;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (canCollideWithFloor)
        {
            if (other.CompareTag("Floor"))
            {
                canEffectCharacter = false;
				Vector2 remainLoc = transform.position;
                _rigid.isKinematic = true;
                //_rigid.useGravity = false;
                _rigid.transform.position = remainLoc;
                //_rigid.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
                myRenderer.sprite = squishSprite;
                remainLoc.y = other.transform.position.y + other.transform.localScale.y / 2;
            }
            else if (other.CompareTag("Wall"))
            {
                canEffectCharacter = false;
				Vector2 remainLoc = transform.position;
                float whichSideMultiplier = other.transform.position.x > transform.position.x ? -1f : 1f;
                _rigid.isKinematic = true;
                //_rigid.useGravity = false;
                _rigid.transform.position = remainLoc;
                _rigid.transform.localScale = new Vector3(1.5f, 0.5f, 1f);
                _rigid.transform.eulerAngles = new Vector3(0f, 0f, whichSideMultiplier * 90f);
                remainLoc.x = other.transform.position.x + whichSideMultiplier * other.transform.localScale.x / 2;
            }
            Transform trail = transform.FindChild("BananaTrail");
            if (trail != null)
            {
                trail.gameObject.SetActive(false);
            }
            StartCoroutine(StartFading());
        }
    }

    public float GetIncAmount()
    {
        return characterInc;
    }

    public void CollideWithCharacter()
    {
        canEffectCharacter = false;
    }

    public bool GetCanEffectCharacter()
    {
        return canEffectCharacter;
    }

   

    private IEnumerator StartFading()
    {
        float startTime = Time.time;
        float fade = 1f;
        if (!faded)
        {
            //myRenderer.color = new Color(1f, 1f, 1f, Mathf.PingPong(Time.deltaTime * fadeTime, 1f));
            //myRenderer.color = new Color(1f, 1f, 1f, Mathf.SmoothStep(0f, 1f, Time.deltaTime * fadeTime));
            while (fade > 0f)
            {
                fade = Mathf.Lerp(1f, 0f, (Time.time - startTime) / fadeTime);
                spriteColor.a = fade;
                myRenderer.color = spriteColor;
                faded = true;
                yield return null;
            }
            fade = 0f;
            spriteColor.a = fade;
            myRenderer.color = spriteColor;
            yield return new WaitForSeconds(2f);
        }
        //yield return new WaitForSeconds(fadeDelay);
    }

}
