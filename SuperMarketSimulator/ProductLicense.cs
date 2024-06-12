using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ProductLicense : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> productNameList = new List<TextMeshProUGUI>();
    [SerializeField] private TextMeshProUGUI requiredLevelText;
    [SerializeField] private Button buyLicenseButton;
    [SerializeField] private int licenseID;
    private void Awake()
    {
        buyLicenseButton.onClick.AddListener(OnLicenseBuyButtonDown);
    }

    public List<TextMeshProUGUI> GetProductNameList() => productNameList;
    public void SetRequiredLevelText(int requiredLevel)=>requiredLevelText.text = requiredLevel.ToString();
    public void OnLicenseBuyButtonDown()
    {
        ProductUnlockManager.Instance.AddProductsOnLicensePurchasing(licenseID);
        Destroy(gameObject);
    }
    public void SetLicenseID(int id)=>licenseID = id;
}
