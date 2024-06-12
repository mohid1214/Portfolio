using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCheckOutDetails : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemQty;
    [SerializeField] private TextMeshProUGUI totalPrice;

    public void SetItemName(string name)=>itemName.text = name;
    public void SetItemQty(string qty)=>itemQty.text = qty;
    public void SetTotalPrice(string price)=>totalPrice.text = price;
}
