using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private List<Item_So> orderitems = new();
    [SerializeField] private List<Item_So> itemsFound = new();
    [SerializeField] private CustomerData customerData;

    private void Start()
    {
        SetOrderItemsList();
    }

    public void SetOrderItemsList()
    {
        int randomValue = Random.Range(0, 100);
        int randomitemCount;
        if (randomValue < 50) // 50% chance
        {
            randomitemCount = Random.Range(1, 4); // 1-3 inclusive
        }
        else if (randomValue < 90) // 40% chance
        {
            randomitemCount = Random.Range(1, 6); // 1-5 inclusive
        }
        else // 10% chance
        {
            randomitemCount = Random.Range(1, 11); // 1-10 inclusive
        }
        List<Item_So> itemsList = ProductUnlockManager.Instance.GetListFreeItemsInStart();

        for (int i = 0; i < randomitemCount; i++)
        {
            int randomItem = Random.Range(0, itemsList.Count);
            orderitems.Add(itemsList[randomItem]);
        }
        
    }

    public void MoveCustomerToBuyStuff()
    {
        StartCoroutine(MoveCustomerToEveryShelf());   
    }

    IEnumerator MoveCustomerToEveryShelf()
    {
        for (int i = 0; i < orderitems.Count; i++)
        {
            // Set the destination to the current target point
            RackCustomerPoint currentTarget = ShelvesHolder.Instance.FindItemInRack(orderitems[i].GetProductID().ToString());
            if (currentTarget == null) continue;
            customerData.MoveCustomerToCurrentTarget(currentTarget.transform);
            customerData.GetCharacterVisualScript().GetCharacterVisual().Walk();
            itemsFound.Add(orderitems[i]);
            Vector3 targetRotation = currentTarget.transform.rotation.eulerAngles;
            targetRotation.y += 180;
            customerData.transform.DORotate(targetRotation, 1f);
            // Wait until the agent reaches the destination
            while (!customerData.HasReachedDestination())
            {
                yield return new WaitForSeconds(0.5f);
            }
            customerData.transform.DORotate(targetRotation, 1f);
            customerData.GetCharacterVisualScript().GetCharacterVisual().Idle();
            RemoveItemInRack(currentTarget, orderitems[i].GetProductID().ToString());
            customerData.GetCharacterVisualScript().GetCharacterVisual().Pick();
            yield return new WaitForSeconds(2f);
            customerData.GetCharacterVisualScript().GetCharacterVisual().Idle();
        }
        yield return StartCoroutine(MoveCustomersToLine());
    }

    private void RemoveItemInRack(RackCustomerPoint rackCustomerPoint, string itemName)
    {
        Rack newRack = rackCustomerPoint.GetRack();


        for (int j = 0; j < newRack.GetShelvesList().Count; j++)
        {
            if (!newRack.GetShelvesList()[j].gridCreated) continue;
            if (newRack.GetShelvesList()[j].GetProductNameInShelf() == null) continue;
            if (newRack.GetShelvesList()[j].GetProductNameInShelf() != itemName) continue;
            for (int i = newRack.GetShelvesList()[j].GetPointsInShelf().Count-1; i >= 0; i--)
            {
                if (newRack.GetShelvesList()[j].GetPointsInShelf()[i].item == null) continue;
                newRack.GetShelvesList()[j].GetPointsInShelf()[i].item.gameObject.SetActive(false);
                newRack.GetShelvesList()[j].GetPointsInShelf()[i].hasItem = false;
                newRack.GetShelvesList()[j].GetPointsInShelf()[i].item = null;
                newRack.GetShelvesList()[j].CheckShelfNull();
                break;
            }

        }

    }

    IEnumerator MoveCustomersToLine()
    {
        customerData.DisableNavmesh();
        while (BillingManager.Instance.GetLastLinePoint() == null)
        {
            float newTimer = Random.Range(1f, 2f);
            float timer = Random.Range(0.1f, 2f);
            float finalRandom = Random.Range(newTimer, timer);
            yield return new WaitForSeconds(finalRandom);
        }
        customerData.GetCharacterVisualScript().GetCharacterVisual().Walk();
        customerData.EnableNavmesh();
        LinePoint newLinePoint = BillingManager.Instance.GetLastLinePoint();
        newLinePoint.customerData = customerData;
        newLinePoint.isOccupied = true;
        customerData.MoveCustomerToCurrentTarget(newLinePoint.pointTransform);
        while (!customerData.HasReachedDestination())
        {
            yield return new WaitForSeconds(0.5f);
        }
        customerData.transform.DORotate(newLinePoint.pointTransform.rotation.eulerAngles, 1f);
        customerData.GetCharacterVisualScript().GetCharacterVisual().Idle();
        BillingManager.Instance.moveRemainiingCustomersForward();
    }

    public List<Item_So> GetOrderList() => orderitems;
    public List<Item_So> GetItemsFound() => itemsFound;
}
