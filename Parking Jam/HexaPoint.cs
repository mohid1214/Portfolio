using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexaPoint : MonoBehaviour
{

    

    public GameObject carPos, car;
    public HexaPoint[] targetPoints;
    public bool canMove = false;
    HexaPoint endPoint = null;
    public ColorType pointColor;


    public void Start()
    {
        // InputWrapper.simulateMouseWithTouches = true;
     //   Debug.Log(GameConstants.isGameStarted + "   " + GameConstants.isGameFinished);
        UpdateCarDirection();
        UpdateCarMovement();    
    }

  



    private void OnMouseDown()
    {
        if (this.car != null && canMove && endPoint!=null && !GameConstants.anyHexaCarMoving && GameConstants.isGameStarted && !GameConstants.isGameFinished)
        {
            soundmanagerHexa.instance.PlayMySound(soundmanagerHexa.instance._CarClick);
            GameConstants.anyHexaCarMoving = true;
            this.car.GetComponent<CarHexaMove>().destination = endPoint.gameObject;
            endPoint.car = this.car;
            this.car = null;
            GameManagerHexa.Instance.UpdateAllPointMovement();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            this.car = null;
          //  UpdateCarMovement();
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
         //   Debug.Log("Here 1");
            this.car = other.gameObject;
            GameManagerHexa.Instance.UpdateAllPointMovement();
            GameManagerHexa.Instance.UpdateAllPointDirection();
            GameConstants.anyHexaCarMoving = false;
            GameManagerHexa.Instance.CheckLevelCompletion();
            // this.car.transform.position = carPos.transform.position;
            //   UpdateCarDirection();
            //   UpdateCarMovement();
            // this.car.GetComponent<CarHexaMove>().destination = null;
        }
       
    }
    public void UpdateCarMovement()
    {
    //    Debug.Log("Here 2");
        this.canMove = false;
       
            for (int i = 0; i < targetPoints.Length; i++)
            {
                if (targetPoints[i].car == null)
                {
                 //   Debug.Log("Here 3 " + targetPoints[i].name);
                   
                    // this.car.transform.LookAt(targetPoints[i].transform);
                    this.canMove = true;
                   
                }
            }
        
    }
    public void UpdateCarDirection() 
    {
  
        endPoint = null;
        if (this.car != null)
        {
            for (int i = 0; i < targetPoints.Length; i++)
            {
                if (targetPoints[i].car == null)
                {
                 //   Debug.Log("Here 3 " + targetPoints[i].name);
                    this.car.GetComponent<CarHexaMove>().lookPoint = targetPoints[i].gameObject;
                  
                    endPoint = targetPoints[i];
                }
            }
        }
    }

}
