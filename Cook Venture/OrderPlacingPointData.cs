using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OrderPlacingPointData : MonoBehaviour
{
    [SerializeField] bool HasCustomer;
    [SerializeField] private Transform pointToCome;
    [SerializeField] private CustomerDataScript cusDataScript;
    [SerializeField] private OrderDeliveryPointData orderDeliveryPoint;
    [SerializeField] private List<Transform> exitPoints;

    [SerializeField] private bool orderTaken;

    public void SetOrderTakenFalse() { orderTaken = false; }
    public void SetOrderTakenTrue() { orderTaken = true; }
    public bool IsOrderTaken() { return orderTaken; }


    public bool IsHasCustomer() { return HasCustomer; }
    public void SetHasCustomerFalse() { HasCustomer = false; }
    public void SetHasCustomerTrue() { HasCustomer = true; }


    public Transform GetPointToCome() { return pointToCome; }


    public void SetCustomerDataScript(CustomerDataScript cusDataScript) { this.cusDataScript = cusDataScript; }
    public OrderDeliveryPointData GetOrderDeliveryPointdata() { return orderDeliveryPoint; }

    
    public CustomerDataScript GetCustomerDataScript() { return cusDataScript; }

    public void MakeCustomerExit()
    {
        CustomerDataScript newcustomerdatascript = cusDataScript;
        cusDataScript = null;
        newcustomerdatascript.transform.DOMove(pointToCome.position, 1f).OnComplete(() =>
        {
            StartCoroutine(CustomerMoveToExit(newcustomerdatascript));
           /* newcustomerdatascript.transform.DOMove(GameManager.Instance.GetCustomerExitpoint().position, 1f).OnComplete(() => 
            {
                
               
            });*/

        });
    }

    IEnumerator CustomerMoveToExit(CustomerDataScript customerDataScript)
    {
        for(int i=0; i<exitPoints.Count; i++)
        {
            customerDataScript.transform.DORotate(exitPoints[i].eulerAngles, 0.2f);
            yield return customerDataScript.transform.DOMove(exitPoints[i].position, 1.5f).SetEase(Ease.Linear).WaitForCompletion();
        }
        Destroy(customerDataScript.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("customer")) return;

        SetCustomerDataScript(other.gameObject.transform.GetComponent<CustomerDataScript>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("customer")) return;

        EventsInProject.GameEvents.RemoveOrderPlacingPointDataEvent?.Invoke(this);
    }

}
