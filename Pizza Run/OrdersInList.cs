using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersInList : MonoBehaviour
{

    public static OrdersInList Instance { get; private set; }

    [SerializeField] private List<OrderPlacingPointData> cusOrderTakingList;
    [SerializeField] private CashierCentralizationScript cashierCentralizationScript;
    private void Awake()=> Instance = this;
    
   
    /// <summary>
    /// /////Function of order taking list
    /// </summary>
    /// <returns></returns>
    public void AddToCustomerOrderTakingList(OrderPlacingPointData x) => cusOrderTakingList.Add(x);   
    public void RemoveFromCustomerOrderTakingList(OrderPlacingPointData x)=> cusOrderTakingList.Remove(x);
    public bool IsCustomerOrderTakingListEmpty()=> cusOrderTakingList.Count == 0;
    public List<OrderPlacingPointData> GetOrderTakingPointDataList()=> cusOrderTakingList;
    public OrderPlacingPointData GetFirstOrderPlacingPoint() => cusOrderTakingList[0];
    //remove this function later
    public void ClearOrderTakingList()=> cusOrderTakingList.Clear();
    
    //////Summnary
    ///2D lists function
    /// 
    
   
}


