using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Road : MonoBehaviour
{
    [SerializeField] GameObject destination;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Contains("Car"))
        {
            /*     other.gameObject.GetComponent<CarMovement>().enabled = false;
                 other.GetComponent<NavMeshAgent>().enabled = true;
                 if(other.GetComponent<NavMeshAgent>().isActiveAndEnabled)
                 other.GetComponent<NavMeshAgent>().SetDestination(destination.transform.position);*/
            other.tag = "MovingCar";
            other.gameObject.GetComponent<CarMovement>().destination = destination;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Car")) 
        {
            other.gameObject.GetComponent<CarMovement>().ActivateTyreTrail();
        }
    }
}
