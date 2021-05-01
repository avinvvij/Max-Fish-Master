using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FisherMan : MonoBehaviour
{

    Tweener fisherManTweener;
    public float tweenDelay = 2.0f;

    private void Awake()
    {
        //start the movement for fisherman
        fisherManTweener =  this.transform.DORotate(new Vector3(transform.rotation.eulerAngles.x , transform.rotation.eulerAngles.y , -1.5f), tweenDelay, RotateMode.Fast).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    
}
