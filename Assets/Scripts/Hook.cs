using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hook : MonoBehaviour
{

    public Transform transform;

    public Transform hookedTransform;

    private Camera mainCamera;

    private int length;
    private int strength;
    private int fishCount;

    private Collider2D collider;

    private bool canMove = false;
    private Tweener cameraTwinner;

    private bool canStartGame = false;

    public GameObject startGameText;

    List<Fish> hookedFishes;

    private void Awake()
    {
        mainCamera = Camera.main;
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        canStartGame = true;
        hookedFishes = new List<Fish>();        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove && canStartGame == true && Input.GetMouseButtonDown(0))
        {
            startFishing();
        }
        
        if(canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }


    public void startFishing()
    {
        startGameText.SetActive(false);
        canStartGame = false;        
        length = -50;
        strength = 3;
        fishCount = 0;
        float time = (-length) * 0.1f;

        cameraTwinner = mainCamera.transform.DOMoveY(length , 1 + time * 0.25f, false).OnUpdate(delegate
        {
            if(mainCamera.transform.position.y <= -11)
            {
                canMove = true;
                transform.SetParent(mainCamera.transform);
                transform.localPosition = new Vector3(transform.position.x , 2.6f , 10);
            }
        }).OnComplete(delegate {
            collider.enabled = true;
            cameraTwinner = mainCamera.transform.DOMoveY(0,  time * 3f, false).OnUpdate(delegate {
                   if(mainCamera.transform.position.y >= -11f)
                {
                    //the camera has reached the end position, we will stop the fishing
                    stopFishing();
                }
            });
        });
    }

    void stopFishing()
    {
        canMove = false;
        cameraTwinner.Kill(false);
        collider.enabled = false;        
        //transform.DOMove(new Vector3(-0.04f, -7.78f, 0), 1, false);
        Vector3 cameraOriginalPostiton = new Vector3(0, 0, -10);        
        cameraTwinner = mainCamera.transform.DOMove(cameraOriginalPostiton, 2, false).OnUpdate(delegate {
            
	    }).OnComplete(delegate {
            transform.SetParent(null);
            transform.position = new Vector3(-0.04f, -7.78f, 0);
            startGameText.SetActive(true);
            canStartGame = true;               
	    });

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if(collision.CompareTag("Fish") && fishCount<= strength)
        {
            Handheld.Vibrate();
            fishCount++;
            Fish _targetFish = collision.GetComponent<Fish>();
            hookedFishes.Add(_targetFish);
            _targetFish.Hooked();
            collision.transform.SetParent(hookedTransform);
            collision.transform.position = hookedTransform.position;
            collision.transform.eulerAngles = new Vector3(collision.transform.eulerAngles.x, collision.transform.eulerAngles.y, -90);
            //collision.transform.localScale = Vector3.one;
            collision.transform.DOShakeRotation(5, Vector3.forward * 60, 20, 90).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                collision.transform.rotation = Quaternion.identity;
            });
            if(fishCount == strength)
            {                
                stopFishing();                         
            }
        }
    }    

}
