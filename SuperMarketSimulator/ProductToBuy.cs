using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductToBuy : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button increaseQtyButton;
    [SerializeField] private Button decreaseQtybutton;
    [SerializeField] private Button addToCartButton;

    [Space(4)]
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI displayTypeText;
    [SerializeField] private TextMeshProUGUI unitPriceText;
    [SerializeField] private TextMeshProUGUI countInCartonText;
    [SerializeField] private TextMeshProUGUI totalAmountText;
    [SerializeField] private TextMeshProUGUI qtyOrderedText;

    [Space(4)]
    [Header("UI Image")]
    [SerializeField] private Image productImage;

    [Space(4)]
    [Header("values")]
    private int orderQty;

    [SerializeField] private Item_So item;

   
    private void OnEnable()
    {
        increaseQtyButton.onClick.AddListener(OnIncreaseQtyButtonDown);
        decreaseQtybutton.onClick.AddListener(OnDecreaseQtyButtonDown);
        addToCartButton.onClick.AddListener(OnAddToCartButtonDown);
    }

    public void SetItemSo(Item_So newItem)
    {
        item = newItem;
        Sprite sprite = item.GetItemSprite();
        productImage.sprite = sprite;
        productImage.SetNativeSize();
        SetProductName();
        SetDisplayTypeText();
        SetUnitPriceText();
        SetCountInCartonText();
        SetInitialQty();
       
    }
    private void SetProductName() => productNameText.text = item.GetProductID().ToString();
    private void SetDisplayTypeText() => displayTypeText.text = item.GetDisplayEnum().ToString();
    private void SetUnitPriceText() => unitPriceText.text = item.GetWholeSalePrice().ToString();
    private void SetCountInCartonText() => countInCartonText.text = item.GetCountInCarton().ToString();

    private void SetInitialQty()
    {
        orderQty = 1;
        qtyOrderedText.text = orderQty.ToString();
        CalculateTotalCost();
    }

    private void OnIncreaseQtyButtonDown()
    {
        orderQty++;
        qtyOrderedText.text = orderQty.ToString();
        addToCartButton.interactable = true;
        CalculateTotalCost();
    }

    private void OnDecreaseQtyButtonDown()
    {
        if(orderQty > 1)
        {
            addToCartButton.interactable = true;
            orderQty--;
            qtyOrderedText.text = orderQty.ToString();
        }
        CalculateTotalCost();
    }

    private void OnAddToCartButtonDown()
    {
        WholeSaleCheckOut.Instance.AddToCheckOutList(orderQty, item.GetProductID().ToString(), item.GetCountInCarton(),item.GetWholeSalePrice(),item);
        addToCartButton.interactable = false;
    }

    private void CalculateTotalCost()
    {
        float totalAmount = item.GetWholeSalePrice() * orderQty * item.GetCountInCarton();
        totalAmountText.text = totalAmount.ToString();
    }
    

    
}
