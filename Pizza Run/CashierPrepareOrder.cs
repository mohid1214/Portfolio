using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierPrepareOrder : MonoBehaviour
{
    [SerializeField] private List<CashierDataScript> cashierDataScriptList;
    [SerializeField] private CashierCentralizationScript cashierCentralizationScript;


    [SerializeField] private List<OrderPlacingPointData> OrdersToPreparelist;

    [SerializeField] private List<ExtractedOrder> extractedOrderList;

    public static CashierPrepareOrder Instance { get; private set; }


    private void OnEnable()
    {
        UIManager.DeletePref -= DeletePrefsNow;
    }
    private void Awake() => Instance = this;

    private void Start()
    {
        if(LoadCashierPref() != 0)
        {
            MakeCashierFromPref();
        }
        else
        {
            SaveCashierPref();
        }
       
    }

    public void AddToOrdersToPrepareList(OrderPlacingPointData x) 
    {
        OrdersToPreparelist.Add(x);
        
    } 

    public void ExtractAllOrderFromList()
    {
        for(int i=0; i < OrdersToPreparelist.Count; i++)
        {
            ExtractOrderFromPlacingPoint(OrdersToPreparelist[i]);
        }
        OrdersToPreparelist.Clear();
    }

    public void ExtractOrderFromPlacingPoint(OrderPlacingPointData x)
    {
        CustomerDataScript newCustomerDataScript = x.GetCustomerDataScript();
        OrderSO newOrderSo = newCustomerDataScript.GetOrderOfCustoemr();
        OrderDeliveryPointData newOrderDeliveryPoint = x.GetOrderDeliveryPointdata();
        List<ItemData> listOfItems = newOrderSo.GetItemsDataList();

        for (int i = 0; i < listOfItems.Count; i++)
        {
            for(int z=0; z<listOfItems[i].qty; z++)
            {
                ExtractedOrder newExtractedOrder = new();
                newExtractedOrder.itemName = listOfItems[i].item.GetNameOfItem().ToString();
                newExtractedOrder.orderDeliveryPoint = newOrderDeliveryPoint;
                extractedOrderList.Add(newExtractedOrder);
            }
        }
    }

    public void AddFromQty(int count)
    {
       
    }

    public void DistributeOrderAmongFreeCashiers()
    {
        if(IsExtractedOrderNull()) return;
        for (int i=0; i < cashierDataScriptList.Count; i++)
        {
            if (!cashierDataScriptList[i].IsOrderProcessing())
            {
                if (GetFirstExtractedOrder() == null) continue;
                string itemName = GetFirstExtractedOrder().itemName;
                if (!IsPointAvailable(itemName)) continue;
                OrderDeliveryPointData orderDeliveryPoint = GetFirstExtractedOrder().orderDeliveryPoint;
                RemoveFirstExtractedItem(GetFirstExtractedOrder());
                cashierDataScriptList[i].GoToPrepareOrder(itemName,orderDeliveryPoint);
                cashierDataScriptList[i].SetOrderProcessingTrue();
            }
        }
    }
    public bool IsPointAvailable(string nameItem)
    {
        MainMachine newMainMachine = GameManager.Instance.GetMainMachine(nameItem);
        if(newMainMachine.GetFreeMachinePoint() != null)
        {
            return true;
        }
        return false;
    }

    public void RemoveFirstExtractedItem(ExtractedOrder x) => extractedOrderList.Remove(x);

    public bool IsExtractedOrderNull() => extractedOrderList.Count == 0;
    public ExtractedOrder GetFirstExtractedOrder()
    {
        if (IsExtractedOrderNull()) return null;
        return extractedOrderList[0];
    }
    public void AddInstantiatedCustomer(CashierDataScript cashier)
    {
        cashierDataScriptList.Add(cashier);
        SaveCashierPref();
    }

    public void SaveCashierPref()
    {
        PlayerPrefs.SetInt(prefs.numberOfCashier, cashierDataScriptList.Count);
    }

    public int LoadCashierPref()
    {
        return PlayerPrefs.GetInt(prefs.numberOfCashier);
    }

    public void MakeCashierFromPref()
    {
        int x = LoadCashierPref();
        cashierDataScriptList.Clear();
        CashierTakeOrder.Instance.cashierDataScriptList.Clear();

        for(int i=0; i<x; i++)
        {
            GameObject newCashier = Instantiate(CashierInstantiate.Instance.cashier);
            newCashier.transform.position =  CashierInstantiate.Instance.InstantiationPoint.position;
            CashierDataScript newCashierDataScript = newCashier.GetComponent<CashierDataScript>();
            RemoveCashierBox newCashierBox = newCashier.GetComponentInChildren<RemoveCashierBox>();
            newCashierBox.cashierDataScript.enabled = true;
            newCashierBox.doTweenAnim.enabled = true;
            newCashierBox.box.SetActive(false);
            gameObject.SetActive(false);
            CashierPrepareOrder.Instance.AddInstantiatedCustomer(newCashierDataScript);
            CashierTakeOrder.Instance.AddInstantiatedCustomer(newCashierDataScript);


        }
    }

    public void DeletePrefsNow()
    {
        PlayerPrefs.DeleteKey(prefs.numberOfCashier);

    }

    private void OnDisable()
    {
        UIManager.DeletePref -= DeletePrefsNow;
    }

}

[System.Serializable]
public class ExtractedOrder
{
    public string itemName;
    public OrderDeliveryPointData orderDeliveryPoint;
}
