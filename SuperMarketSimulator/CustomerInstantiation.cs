using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomerInstantiation : MonoBehaviour
{
    [SerializeField] private CustomerData customer;
    [SerializeField] private Transform customerInstantionPoint;
    [SerializeField] private Transform storePointOutside;
    [SerializeField] private Transform storePointInside;
    [SerializeField] private Transform endPointTransform;

    private void Start()
    {
        InvokeRepeating("InstantiateNewCustomer", 2, 20);
        
    }

    public void InstantiateNewCustomer()
    {
        CustomerData newCustomer = Instantiate(customer);
        newCustomer.DisableNavmesh();
        newCustomer.GetCharacterVisualScript().GetCharacterVisual().Walk();
        newCustomer.transform.position = customerInstantionPoint.position;
        newCustomer.transform.DOLookAt(storePointOutside.position, 1f);
        newCustomer.transform.DOMove(storePointOutside.position, 15f).SetEase(Ease.Linear).OnComplete(() =>
        {
            RackCustomerPoint newRackPoint = null;
            for (int i = 0; i < newCustomer.GetCustomerMovementScript().GetOrderList().Count; i++)
            {
                if (ShelvesHolder.Instance.FindItemInRack(newCustomer.GetCustomerMovementScript().GetOrderList()[i].GetProductID().ToString()) == null) continue;
                newRackPoint = ShelvesHolder.Instance.FindItemInRack(newCustomer.GetCustomerMovementScript().GetOrderList()[i].GetProductID().ToString());
            }

            if (newRackPoint == null)
            {
                newCustomer.transform.DOLookAt(endPointTransform.position, 1f);
                newCustomer.transform.DOMove(endPointTransform.position, 15f).SetEase(Ease.Linear).OnComplete(() => 
                {
                    newCustomer.gameObject.SetActive(false);
                    Destroy(newCustomer.gameObject, 2f);
                });
            }
            else
            {
                newCustomer.transform.DOLookAt(storePointInside.position, 1f);
                newCustomer.transform.DOMove(storePointInside.position, 3f).OnComplete(() =>
                {
                    newCustomer.EnableNavmesh();
                    newCustomer.GetCustomerMovementScript().MoveCustomerToBuyStuff();
                });
            }
           
        });
    }

}
