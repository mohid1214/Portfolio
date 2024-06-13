using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem instance;
    public int poolCount = 15;
    public List<GameObject> carsToPool;
    public List<GameObject> PooledCars;
    public GameObject tempCar;
    // Start is called before the first frame update
    void Start()
    {
        BannerAd.instance.CreateBannerView();
        
            instance = this;
        PoolCars();
    }
    void PoolCars() 
    {
        for (int i = 0; i < carsToPool.Count; i++)
        {
            tempCar = Instantiate(carsToPool[i%carsToPool.Count]);
            tempCar.SetActive(false);
            PooledCars.Add(tempCar);
        }
        
    }

    public GameObject GetCar()
    {
        for (int i = 0; i < PooledCars.Count; i++)
        {
            if (!PooledCars[i].activeInHierarchy)
            {
                PooledCars[i].SetActive(true);
                return PooledCars[i];
            }
        }
        tempCar = Instantiate(carsToPool[Random.Range(0,carsToPool.Count)]);
        tempCar.SetActive(false);
        PooledCars.Add(tempCar);
        tempCar.SetActive(true);
        return tempCar;
    }
   

}
