
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFood : MonoBehaviour
{
    public GameObject food;
    public Vector3 spawn;
    public float foodSpawnRateTime;
    float _foodSpawnRateCurrTime;

    public void Awake()
    {
        _foodSpawnRateCurrTime = foodSpawnRateTime;
    }

    private void Update()
    {
        if (_foodSpawnRateCurrTime <= 0)
        {
            RandomPos();
        }
        if (!food.activeInHierarchy)
        {
            _foodSpawnRateCurrTime -= Time.deltaTime;
        }
    }


    void RandomPos()
    {
        spawn = new Vector3(Random.Range(-GameManager.instance.width, GameManager.instance.width), 0, Random.Range(-GameManager.instance.height, GameManager.instance.height));

        food.transform.position = spawn;
        food.SetActive(true);
        _foodSpawnRateCurrTime = foodSpawnRateTime;
    }

}
