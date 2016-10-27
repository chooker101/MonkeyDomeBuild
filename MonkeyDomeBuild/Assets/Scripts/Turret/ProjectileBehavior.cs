using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour
{
    private Rigidbody2D _rigid;
    public bool canCollideWithFloor = false;
    private bool canEffectCharacter = true;
    public float characterInc;


    public Sprite squishSprite;
    private bool isBanana = true;
    private SpriteRenderer myRenderer;

    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        characterInc = 0.5f;

        //if (gameObject.CompareTag("Banana"))
        //{
        //    squishSprite = bananaSprite;
        //} else if (gameObject.CompareTag("Poop"))
        //{
        //    squishSprite = poopSprite;
        //}
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
}
