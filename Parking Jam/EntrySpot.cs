using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntrySpot : MonoBehaviour
{
    public bool isLocked = true,hasCar=false;
    public int price = 100;
    public float parkingTime = 8f;
    public Transform finishPoint;
    public GameObject lockedObject,UnlockedObject,blinkObject;
    public CarLotMove currentCar;
    GameObject tempCar;

    public int numberofcarsSpawned;

    // Start is called before the first frame update
    private void Start()
    {
        GameManagerLot.Instance.UpdateScoreText();
        lockedObject.SetActive(false);
        UnlockedObject.SetActive(false);
        blinkObject.SetActive(false);
        this.OnEnable();
    }
    void OnEnable()
    {
        if (this.isLocked && PlayerPrefs.GetInt(gameObject.name) == 0)
            lockedObject.SetActive(true);
        else
        {
            UnlockedObject.SetActive(true);
            isLocked = false;
            transform.GetComponent<showpriceentryspot>()._PriceIndicator.SetActive(false);
            InvokeRepeating("SpawnCar",2f,3f);
        }
    }
    public void SpawnCar() 
    {
        if (!hasCar)
        {
            tempCar=PoolSystem.instance.GetCar();
            currentCar = tempCar.GetComponent<CarLotMove>();
            currentCar.destination = this.gameObject;
            this.hasCar = true;
            currentCar.MoveToStart();

            numberofcarsSpawned++;
            if(numberofcarsSpawned > 5)
            {
                InterstatialInstance.instance.LoadInterstatialAdNow();
                InterstatialInstance.instance.ShowInterstatialAdNow();
                numberofcarsSpawned = 0;
            }

        }
        if (!GameManagerLot.Instance.entryPoints.Contains(this))
            GameManagerLot.Instance.entryPoints.Add(this);
    }
    private void OnMouseDown()
    {
        if (GameConstants.Instance.totalScore >= price && this.isLocked)
            UnlockObject();
        else if (this.hasCar && !this.isLocked)
        {
            GameManagerLot.Instance.currentSelectedCar = currentCar;
            SoundManagerLot.instance.PlayAudio(SoundManagerLot.instance._carclick);
            ObjectBlink();
        }

    }
    void ObjectBlink() 
    {
    blinkObject.SetActive(true);
        Invoke("DeActiveBlink",0.3f);
    }
    void DeActiveBlink() { blinkObject.SetActive(false); }
    void UnlockObject() 
    {
        SoundManagerLot.instance.PlayAudio(SoundManagerLot.instance._carclick);
        GameConstants.Instance.totalScore -= price;
        GameManagerLot.Instance.UpdateScoreText();
        transform.GetComponent<showpriceentryspot>()._PriceIndicator.SetActive(false);
        PlayerPrefs.SetInt(gameObject.name, 1);
        this.isLocked = false;
        lockedObject.SetActive(false);
        UnlockedObject.SetActive(true);
        GameManagerLot.Instance.entryPoints.Add(this);
        InvokeRepeating("SpawnCar", 2f, 3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Debug.Log("trigger detected");
            SoundManagerLot.instance.PlayAudio(SoundManagerLot.instance._CarHorn);
            StartCoroutine(ParkingTime());
        }
    }
    IEnumerator ParkingTime()
    {
        float time = 0;
        while (time <= parkingTime && this.hasCar)
        {
            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        if (this.hasCar)
        {
            currentCar.MoveBack(finishPoint.position);
            currentCar = null;
            this.hasCar = false;
            
        }
            yield return null;
    }
}
