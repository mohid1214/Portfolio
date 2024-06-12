using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderDeliveryPointData : MonoBehaviour
{
    [SerializeField] private OrderPlacingPointData orderPlacingPoint;

    [SerializeField] private Transform comeToDeliverPoint;

    public OrderPlacingPointData GetOrderPlacingPoint ()=> orderPlacingPoint;
    public Transform GetComeToDeliverPoint()
    {
        return comeToDeliverPoint;
    }
}
