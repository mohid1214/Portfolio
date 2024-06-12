using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ProductUnlockManager : MonoBehaviour
{
    [SerializeField] private List<Item_So> freeItemsInStart = new();
    [SerializeField] private List<Item_So> allItems = new();
    [SerializeField] private ProductToBuy productToBuy;
    [SerializeField] private Transform productToBuyParent;

    [Space(2)]
    [Header("license Data")]
    public List<LicenseData> licenseDataList = new List<LicenseData>();
    [SerializeField] private ProductLicense productLicenseOriginal;
    [SerializeField] private Transform productLicecnseParent;

    public static ProductUnlockManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UnlockFreeItems();
        AddFreeItems();
        InstantiateLicenses();
    }

    private void UnlockFreeItems()
    {
        for (int i = 0; i < freeItemsInStart.Count; i++)
        {
            freeItemsInStart[i].SetProductUnLockedTrue();
            allItems.Add(freeItemsInStart[i]);
        }
    }

    private void AddFreeItems()
    {
        for(int i=0; i<allItems.Count; i++)
        {
            if (!allItems[i].GetIsUnlocked()) continue;
            ProductToBuy newProductToBuy = Instantiate(productToBuy, productToBuyParent);
            Item_So newItem = allItems[i];
            newProductToBuy.SetItemSo(newItem);
        }
    }

    //Liceses Data
    public void InstantiateLicenses()
    {
        for(int i=0; i<licenseDataList.Count; i++)
        {
            ProductLicense newProductLicense = Instantiate(productLicenseOriginal, productLicecnseParent);
            for(int z=0; z < licenseDataList[i].itemSoList.Count; z++)
            {
                newProductLicense.GetProductNameList()[z].text = licenseDataList[i].itemSoList[z].GetProductID().ToString();
                newProductLicense.SetLicenseID(licenseDataList[i].id);
            }
        }
    }

    public void AddProductsOnLicensePurchasing(int id)
    {
        List<Item_So> itemsSoList = new List<Item_So>();
        for(int i=0; i < licenseDataList.Count; i++)
        {
            if(id == licenseDataList[i].id)
            {
                itemsSoList = licenseDataList[i].itemSoList;
                break;
            }
        }

        for(int z=0; z < itemsSoList.Count; z++)
        {
            ProductToBuy newProductToBuy = Instantiate(productToBuy, productToBuyParent);
            Item_So newItem = itemsSoList[z];
            newProductToBuy.SetItemSo(newItem);
        }
        for(int i=0; i<itemsSoList.Count; i++)
        {
            freeItemsInStart.Add(itemsSoList[i]);
        }
    }

    public List<Item_So> GetListFreeItemsInStart() => freeItemsInStart;
}

[System.Serializable]
public class LicenseData
{
    public string licenseName;
    public List<Item_So> itemSoList;
    public int requiredLevel;
    public int id;
}
