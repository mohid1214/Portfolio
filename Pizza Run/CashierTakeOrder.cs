using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierTakeOrder : MonoBehaviour
{
    public  List<CashierDataScript> cashierDataScriptList;
    [SerializeField] private CashierCentralizationScript cashierCentralizationScript;

    [SerializeField] private CashierDataScript cashierToTakeOrder;
    private bool TakingOrder;
    public static CashierTakeOrder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTakingOrderFalse() => TakingOrder = false;
    public void SetTakingOrderTrue() => TakingOrder = true;
    public bool IsTakingOrder() => TakingOrder;

    public void SetCashierToTakeOrder(CashierDataScript cashierDataScript) => cashierToTakeOrder = cashierDataScript;
    public CashierDataScript GetCashierToTakeOrder() => cashierToTakeOrder;


    public bool IsFreeCashierNeeded() => cashierToTakeOrder == null;
    
    public CashierDataScript GetFreeCashierToTakeOrder()
    {
        for (int i = 0; i < cashierDataScriptList.Count; i++)
        {
            if (!cashierDataScriptList[i].IsOrderProcessing())
            {
                return cashierDataScriptList[i];
            }
        }
        return null;
    }

    public void SendCashierToOtherCustomerToTakeOrder()
    {
        if (cashierCentralizationScript.orderInListScript.IsCustomerOrderTakingListEmpty())
        {
            SetTakingOrderFalse();
            if (IsFreeCashierNeeded()) return;
            cashierToTakeOrder.SetOrderProcessingFalse();
            cashierToTakeOrder = null;
            cashierCentralizationScript.cashierPrepareOrderScript.DistributeOrderAmongFreeCashiers();
            return;
        }

        SendCashierToTakeOrder();
    }

    public void CalledWhenCustomerLands()
    {
        if (GetFreeCashierToTakeOrder() == null) return;
        if (IsTakingOrder()) return;
        SendCashierToOtherCustomerToTakeOrder();
    }

    public void SendCashierToTakeOrder()
    {
        SetTakingOrderTrue();
        if (IsFreeCashierNeeded()) cashierToTakeOrder = GetFreeCashierToTakeOrder();
        if(cashierToTakeOrder == null) return;
        cashierToTakeOrder.SetOrderProcessingTrue();
        OrderPlacingPointData newOrderPlacingPointData = cashierCentralizationScript.orderInListScript.GetFirstOrderPlacingPoint();
        cashierCentralizationScript.orderInListScript.RemoveFromCustomerOrderTakingList(newOrderPlacingPointData);
        AddToOrderPrepareList(newOrderPlacingPointData);
        //bhai masla ya ha k orderpreparing list ma dasti data aata, isko tab tak rokna jab tak orde taking canvas full na ha
        cashierToTakeOrder.GoToTakeOrder(newOrderPlacingPointData);
    }

    public void AddToOrderPrepareList(OrderPlacingPointData x) 
    {
        cashierCentralizationScript.cashierPrepareOrderScript.AddToOrdersToPrepareList(x);
    }

    public void AddInstantiatedCustomer(CashierDataScript cashier)
    {
        cashierDataScriptList.Add(cashier);
    }

}

