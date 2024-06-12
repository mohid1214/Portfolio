using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PickAndDropManager : MonoBehaviour
{
    private Coroutine DetectionCoroutine;
    private Coroutine ShelfDetectionCoroutine;
    private Coroutine placeCartonCoroutine;


    [Space(2)]
    [Header("Carton Details")]
    [SerializeField] private GameObject carton;
    [SerializeField] private ArrangeProductInCarton cartonScript;

    [Space(2)]
    [Header("Shelf Details")]
    [SerializeField] private GameObject shelf;
    [SerializeField] private Shelve shelfScript;

    [SerializeField] private float raycastDistanceOfDetection;

    [Space(2)]
    [Header("Bools")]
    [SerializeField] private bool cartonPicked;
    [SerializeField] private bool cartonInView;
    [SerializeField] private bool placeOnGround;

    [Space(2)]
    [Header("Layer Masks")]

    [SerializeField] private LayerMask placeProductLayer;
    [SerializeField] private LayerMask cartonLayer;
    [SerializeField] private LayerMask computerLayer;
    [SerializeField] private LayerMask joyStickLayer;

    [Space(2)]
    [Header("Buttons")]
    [SerializeField] private Button pickButton;
    [SerializeField] private Button dropButton;
    [SerializeField] private Button placeProductButton;
    [SerializeField] private Button openCartonButton;
    [SerializeField] private Button closeCartonButton;
    [SerializeField] private Button placeCartonButton;
    [SerializeField] private Button OkPlaceButton;

    [Space(2)]
    [Header("Dependencies")]
    [SerializeField] private MyPlayer player;
    [SerializeField] private FurnitureMoveManager furnitureMoveManager;

    private void Awake()
    {
        pickButton.onClick.AddListener(OnPickButtonDown);
        dropButton.onClick.AddListener(OnDropButtonDown);
        placeProductButton.onClick.AddListener(OnPlanceProductButtonDown);
        openCartonButton.onClick.AddListener(OnCartonOpenButtonDown);
        closeCartonButton.onClick.AddListener(OnCartonCloseButtonDown);
        placeCartonButton.onClick.AddListener(OnPlaceCartonButtonDown);
        OkPlaceButton.onClick.AddListener(OnPositionOkButtonDown);
    }

    private void OnEnable()
    {
        StartDetectionCoroutine();
    }

    public void StartDetectionCoroutine()
    {
        DetectionCoroutine ??= StartCoroutine(DetectObjectsCoroutine());
    }



    public void StopDetectionCoroutine()
    {
        if (DetectionCoroutine != null)
        {
            StopCoroutine(DetectionCoroutine);
            DetectionCoroutine = null;
        }
    }


    IEnumerator DetectObjectsCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (cartonPicked)
            {
                cartonInView = false;
                continue;
            }
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

            if (Physics.Raycast(ray, out RaycastHit hittt, raycastDistanceOfDetection, joyStickLayer))
            {
                GameEvents.cashCounterInView?.Invoke();
            }
            else
            {
                GameEvents.cashCounterOutView?.Invoke();
            }

            if (Physics.Raycast(ray, out RaycastHit hitt, raycastDistanceOfDetection, computerLayer))
            {
                GameEvents.computerinViewEvent?.Invoke();
            }
            else
            {
                GameEvents.computeroutViewEvent?.Invoke();
            }
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistanceOfDetection, cartonLayer))
            {
                cartonInView = true;
                GameEvents.HideCartonOpenCloseButtonsEvent?.Invoke();
                GameEvents.CartonInViewEvent?.Invoke();
                carton = hit.transform.gameObject;
                hit.transform.GetComponent<Outline>().enabled = true;
            }
            else
            {
                if(carton != null) carton.transform.GetComponent<Outline>().enabled = false; 
                cartonInView = false;
                carton = null;
                GameEvents.CartonOutViewEvent?.Invoke();
            }
           
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P)) OnPickButtonDown();
        if (Input.GetKey(KeyCode.T)) OnDropButtonDown();
        if (Input.GetKeyUp(KeyCode.O)) OnPlanceProductButtonDown();
        if (Input.GetKeyUp(KeyCode.X)) OnCartonOpenButtonDown();
        if (Input.GetKeyUp(KeyCode.C)) OnCartonCloseButtonDown();
        if (Input.GetKeyUp(KeyCode.B)) OnPlaceCartonButtonDown();
        if (Input.GetKeyUp(KeyCode.N)) OnPositionOkButtonDown();
    }

    private void OnPickButtonDown()
    {
        if (!cartonInView) return;
        carton.transform.GetComponent<Outline>().enabled = false;
        GameEvents.dustbinDeActiveEvent?.Invoke();
        StopDetectionCoroutine();
        StartShelfDetectionCoroutine();
        cartonScript = carton.GetComponent<ArrangeProductInCarton>();
        ManageCartonOpenCloseButton();
        cartonPicked = true;
        cartonScript.GetCartonRigidBody().isKinematic = true;
        cartonScript.GetCartonRigidBody().useGravity = false;
        GameEvents.CartonPickedEvent?.Invoke();
        carton.transform.DOJump(player.GetCartonPoint().position, 1, 1, 0.4f).OnComplete(() =>
        {
            carton.transform.SetParent(player.GetPlayerRoot().transform);
            carton.transform.position = player.GetCartonPoint().position;
            carton.transform.rotation = player.GetCartonPoint().rotation;
        });

    }

    private void OnDropButtonDown()
    {
        if (!cartonPicked) return;
        GameEvents.dustbinActiveEvent?.Invoke();
        CartonPhysics newCartonPhysics = carton.GetComponent<CartonPhysics>();
        newCartonPhysics.GetCartonRigidbody().isKinematic = false;
        newCartonPhysics.GetCartonRigidbody().useGravity = true;
        newCartonPhysics.GetCartonRigidbody().AddForce(new Vector3(3f, 1f, 0f), ForceMode.Impulse);
        cartonScript = null;
        carton.transform.parent = null;
        carton = null;
        StopShelfDetectionCoroutine();
        StartDetectionCoroutine();
        cartonPicked = false;
    }



    public void StartShelfDetectionCoroutine()
    {
        ShelfDetectionCoroutine ??= StartCoroutine(DetectShelfCoroutine());
    }
    public void StopShelfDetectionCoroutine()
    {
        if (ShelfDetectionCoroutine != null)
        {
            StopCoroutine(ShelfDetectionCoroutine);
            ShelfDetectionCoroutine = null;
        }
    }

    IEnumerator DetectShelfCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (!cartonScript.IsOpen)
            {
                Debug.Log("Open carton first");
                continue;
            }
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistanceOfDetection, placeProductLayer))
            {
                GameEvents.ShelfInViewEvent?.Invoke();
                shelf = hit.transform.gameObject;
            }
            else
            {
                shelf = null;
                GameEvents.ShelfOutViewEvent?.Invoke();
            }
        }
    }

    public void OnPlanceProductButtonDown()
    {
        if (!cartonScript.IsOpen)
        {
            Debug.Log("Open carton First");
            return;
        }
        List<CartonPoints> cartonPoints = cartonScript.GetProductsInCarton();
        if (!cartonPoints[0].hasItem)
        {
            Debug.Log("Carton Empty");
            return;
        }
        shelfScript = shelf.GetComponent<Shelve>();
        Item_So newItem = cartonScript.GetItemScriptable();
        shelfScript.CreateGridInShelf(newItem);
        List<CartonPoints> shelfPoints = shelfScript.GetPointsInShelf();
        if (shelfPoints[^1].hasItem)
        {
            Debug.Log("shelf Fill");
            return;
        }
        ProductDetails onlyDetailsOfProduct = cartonPoints[0].item;
        if (shelfPoints[0].hasItem)
        {
            if (shelfPoints[0].item.GetProductName() != onlyDetailsOfProduct.GetProductName())
            {
                Debug.Log("items are not same");
                return;
            }
        }
        ProductDetails newProductDetails = null;

        for (int i = cartonPoints.Count - 1; i >= 0; i--)
        {
            if (!cartonPoints[i].hasItem) continue;
            newProductDetails = cartonPoints[i].item;
            cartonPoints[i].hasItem = false;
            cartonPoints[i].item = null;
            break;
        }

        if (shelfPoints[0].hasItem)
        {
            if (shelfPoints[0].item.GetProductName() != newProductDetails.GetProductName())
            {
                Debug.Log("items are not same");
                return;
            }
        }

        for (int i = 0; i < shelfPoints.Count; i++)
        {
            if (shelfPoints[i].hasItem) continue;
            shelfPoints[i].item = newProductDetails;
            shelfPoints[i].hasItem = true;
            newProductDetails.transform.rotation = shelf.transform.rotation;
            newProductDetails.transform.parent = shelf.transform;
            Vector3 targetPoint = new(shelfPoints[i].x, shelfPoints[i].y, shelfPoints[i].z);
            newProductDetails.transform.DOJump(targetPoint, 0.4f, 1, 0.25f).OnComplete(() => newProductDetails.transform.rotation = shelf.transform.rotation);
            break;
        }
    }

    private void ManageCartonOpenCloseButton()
    {
        if (cartonScript.IsOpen)
        {
            GameEvents.ShowCartonCloseButtonEvent?.Invoke();
        }
        else
        {
            GameEvents.ShowCartonOpenButtonEvent?.Invoke();
        }
    }

    private void OnCartonOpenButtonDown()
    {
        cartonScript.OpenCartonAnimation();
        GameEvents.ShowCartonCloseButtonEvent?.Invoke();
    }

    private void OnCartonCloseButtonDown()
    {
        cartonScript.CloseCartonAnimation();
        GameEvents.ShowCartonOpenButtonEvent?.Invoke();
    }


    private void OnPlaceCartonButtonDown()
    {
        if (placeCartonCoroutine == null)
        {
            StopShelfDetectionCoroutine();
            placeCartonCoroutine = StartCoroutine(PlacingCartonCoroutine());
            carton.transform.parent = null;
            cartonScript.GetCartonCollider().enabled = false;
            GameEvents.PlaceCartonEvent?.Invoke();
        }
    }
    private void OnPositionOkButtonDown()
    {
        if (placeCartonCoroutine != null)
        {
            GameEvents.PlaceCartionOkEvent?.Invoke();
            StopCoroutine(placeCartonCoroutine);
            placeCartonCoroutine = null;
            cartonScript.GetCartonCollider().enabled = true;
            CartonPhysics newCartonPhysics = carton.GetComponent<CartonPhysics>();
            newCartonPhysics.GetCartonRigidbody().isKinematic = false;
            newCartonPhysics.GetCartonRigidbody().useGravity = true;

            cartonScript = null;
            carton.transform.parent = null;
            carton = null;
            StopShelfDetectionCoroutine();
            StartDetectionCoroutine();
            cartonPicked = false;
        }
    }

    IEnumerator PlacingCartonCoroutine()
    {
        while (true)
        {
            yield return null;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            Debug.Log("Working");

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                int objectLayer = hit.transform.gameObject.layer;

                // Check if the layer of the hit object is included in the placeProductLayer mask
                if (placeProductLayer == (placeProductLayer | (1 << objectLayer)))
                {
                    // The hit object's layer is included in the placeProductLayer mask
                    // Do something if needed
                }
                else
                {
                    carton.transform.rotation = Quaternion.identity;
                    carton.transform.position = hit.point;
                }
            }

        }
    }

    public bool IsCartonNull()
    {
        if (carton == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
