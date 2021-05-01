using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField]
    private Fish fishPrefab;


    [SerializeField]
    private FishType[] fishTypes;

    private void Awake()
    {
        //let us spawn the fishes now
        for (int i = 0; i < fishTypes.Length; i++)
        {   
            FishType fishType = fishTypes[i];            
            int count = 0;
            while (count < fishType.fishCount)
            {                            
                Fish _spawnFish = UnityEngine.GameObject.Instantiate<Fish>(fishPrefab);
                _spawnFish.setFishType(fishType);
                _spawnFish.ResetFish();
                count++;
            }
        }
    }

    void Start()
    {
        
    }
}
