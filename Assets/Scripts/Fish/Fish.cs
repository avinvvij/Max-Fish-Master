using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class Fish : MonoBehaviour
{
    
    private FishType fishType;    

    private CircleCollider2D collider;
    private SpriteRenderer renderer;
    private float screenLeft;
    private float screenRight;

    private Tweener tweener;

    private void Awake()
    {
        collider = GetComponent<CircleCollider2D>();
        renderer = GetComponent<SpriteRenderer>();
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;        
    }

    void Start()
    {
        
    }

    public void ResetFish()
    {
        if(tweener!= null)
        {
            tweener.Kill(false);
        }
        collider.enabled = true;        

        float posY = UnityEngine.Random.Range(this.fishType.minLength, this.fishType.maxLength);
        float posX = screenLeft;

        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        screenRight = edgeVector.x;

        int spawnX = UnityEngine.Random.Range(0 , 2);

        transform.position = new Vector3(spawnX == 0 ? posX : screenRight, posY, 0);
        transform.localScale = new Vector2(spawnX == 0 ? transform.localScale.x : transform.localScale.x * -1, transform.localScale.y);

        float delay = UnityEngine.Random.Range(1, 10);

        if(spawnX == 0)
        {
            tweener = transform.DOMoveX(screenRight, delay, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).OnStepComplete(delegate
            {
                float scaleX = transform.localScale.x;
                scaleX = scaleX * -1;
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            });
        }
        else
        {
            tweener = transform.DOMoveX(posX, delay, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).OnStepComplete(delegate
            {
                float scaleX = transform.localScale.x;
                scaleX = scaleX * -1;
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
            });
        }



    }


    public void Hooked()
    {
        collider.enabled = false;
        if(tweener != null)
        {
            tweener.Kill(false);
        }
        
    }


    public FishType GetFishType()
    {
        return fishType;
    }

    public void setFishType(FishType value)
    {        
        fishType = value;
        collider.radius = value.colliderRadius;
        renderer.sprite = value.sprite;
    }
}

[Serializable]
public class FishType
{
    public float price;
    public float minLength;
    public float maxLength;
    public float colliderRadius;
    public Sprite sprite;
    public int fishCount;
}
