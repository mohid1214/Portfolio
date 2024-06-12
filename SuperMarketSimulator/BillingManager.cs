using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BillingManager : MonoBehaviour
{
    public List<LinePoint> pointList = new();
    public List<Transform> billingPointPositions = new();
    public List<GameObject> productsOnCounter = new();
    private Coroutine billingCoroutine;
    [SerializeField] private LayerMask productsLayer;
    [SerializeField] private Transform productJumpPosition;
    [SerializeField] private Transform storeExitPoint1, storeExitPoint2;
    [SerializeField] private TextMeshProUGUI totalAmountText;
    [SerializeField] private float totalAmount;
    public static BillingManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public LinePoint GetLastLinePoint()
    {
        if (!pointList[^1].isOccupied)
        {
            return pointList[^1];
        }
        return null;
    }

    public LinePointAndIndex GetEmptyPoint()
    {
        for(int i=0; i<pointList.Count; i++)
        {
            if (pointList[i].isOccupied) continue;
            LinePointAndIndex newLinePointAndIndex = new LinePointAndIndex();
            newLinePointAndIndex.linePoint = pointList[i];
            newLinePointAndIndex.index = i;
            return newLinePointAndIndex;
        }
        return null;
    }

    public class LinePointAndIndex
    {
        public LinePoint linePoint;
        public int index;
    }
    public void PlaceOrderProducts()
    {
        if (!pointList[0].isOccupied) return;
        productsOnCounter.Clear();
        CustomerData newCustomerData = pointList[0].customerData;
        CustomerMovement newcustomerMovement = newCustomerData.GetCustomerMovementScript();

        for(int i=0; i<newcustomerMovement.GetItemsFound().Count; i++)
        {
            GameObject newProduct = Instantiate( newcustomerMovement.GetItemsFound()[i].GetProductDetailsScript().gameObject);
            newProduct.transform.position = billingPointPositions[i].transform.position;
            productsOnCounter.Add(newProduct);
        }
    }

    public void StartBillingCoroutine()
    {
        if(billingCoroutine == null)
        {
            billingCoroutine = StartCoroutine(BillProductsOnCounter());
            totalAmountText.text = 0.ToString();
        }
    }

    IEnumerator BillProductsOnCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            totalAmountText.text = GetTotalAmount().ToString();
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, productsLayer))
            {
                hit.transform.DOJump(productJumpPosition.position, 0.5f, 1, 0.3f).OnComplete(() => hit.transform.gameObject.SetActive(false));
                productsOnCounter.RemoveAt(productsOnCounter.Count - 1);
                if (productsOnCounter.Count == 0)
                {
                    pointList[0].customerData.EnableNavmesh();
                    pointList[0].customerData.MoveCustomerToCurrentTarget(storeExitPoint2);
                    pointList[0].customerData = null;
                    pointList[0].isOccupied = false;
                    totalAmount = 0;
                    totalAmountText.text = totalAmount.ToString();
                    yield return new WaitForSeconds(3f);
                    moveRemainiingCustomersForward();
                }

            }
        }

    }

    public void moveRemainiingCustomersForward()
    {
        bool moved = true;

        while (moved)
        {
            moved = false;

            for (int i = 1; i < pointList.Count; i++)
            {
                if (!pointList[i - 1].isOccupied && pointList[i].isOccupied)
                {
                    pointList[i].customerData.DisableNavmesh();
                    pointList[i - 1].customerData = pointList[i].customerData;
                    pointList[i].customerData = null;

                    pointList[i - 1].isOccupied = true;
                    pointList[i].isOccupied = false;
                    pointList[i - 1].customerData.transform.DOMove(pointList[i - 1].pointTransform.position, 3f);
                    pointList[i - 1].customerData.transform.DORotate(pointList[i - 1].pointTransform.eulerAngles, 1f);

                    moved = true; // Set moved to true if a customer has been moved forward
                }
            }
        }

        if (productsOnCounter.Count == 0)
        {
            PlaceOrderProducts();
        }
    }
    public void StopBillingCoroutine()
    {
        if(billingCoroutine != null)
        {
            StopCoroutine(billingCoroutine);
            billingCoroutine = null;
        }
    }

    public float GetTotalAmount()
    {
        totalAmount = 0;
        if (pointList[0].customerData == null) return 0;
        CustomerData newCustomerData = pointList[0].customerData;
        List<Item_So> itemFound = newCustomerData.GetCustomerMovementScript().GetItemsFound();
        Debug.Log(itemFound.Count.ToString());
        foreach (Item_So item in itemFound)
        {
            totalAmount += item.GetSellingPirce();
        }
        /*for(int i=0; i<itemFound.Count; i++)
        {
            totalAmount += itemFound[i].GetSellingPirce();
            Debug.Log(totalAmount.ToString());
        }*/
        return totalAmount;

    }

}


[System.Serializable]
public class LinePoint
{
    public CustomerData customerData;
    public Transform pointTransform;
    public bool isOccupied;
}
