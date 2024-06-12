using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureMoveManager : MonoBehaviour
{
    private Coroutine _detectFurniture;
    private Coroutine _moveFurniture;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask computerLayer;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private LayerMask collisinLayer;

    [SerializeField] private Button moveFurnitureButton;
    [SerializeField] private Button placeFurnitureButton;
    [SerializeField] private Button rotateFurnitureButton;

    [SerializeField] private GameObject furniture;
    Outline outLine;
    [Header("Dependencies")]
    [SerializeField] private PickAndDropManager pickAndDropManager;

    private Collider furnitureCollider;

    private void Awake()
    {
        moveFurnitureButton.onClick.AddListener(OnMoveFurnitureButtonDown);
        placeFurnitureButton.onClick.AddListener(OnPlaceFurnitureButtonDown);
        rotateFurnitureButton.onClick.AddListener (OnRotateFurnitureButtonDown);
    }

    private void Start()
    {
        StartDetectFurnitureCoroutine();
        moveFurnitureButton.gameObject.SetActive(false);
        placeFurnitureButton.gameObject.SetActive(false);
        rotateFurnitureButton.gameObject .SetActive(false);
    }

    //Detect Furniture
    public void StartDetectFurnitureCoroutine()
    {
        _detectFurniture ??= StartCoroutine(DetectFurnitureCoroutine());
    }

    IEnumerator DetectFurnitureCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            if (!pickAndDropManager.IsCartonNull()) continue;
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            if (Physics.Raycast(ray, out RaycastHit hit, 2f, computerLayer))
            {
                moveFurnitureButton.gameObject.SetActive(true);
                if (furniture != null && outLine!=null) outLine.enabled = false;
                furniture = hit.transform.gameObject;
                outLine = furniture.transform.GetComponent<Outline>();
                if (outLine != null) 
                {
                    outLine.enabled = true;
                    outLine.OutlineColor = Color.green;
                }
            }
            else
            {
                moveFurnitureButton.gameObject.SetActive(false);
                if(outLine!= null) outLine.enabled = false;
                furniture = null;
            }
        }
    }
    public void StopDetectFurnitureCoroutine()
    {
        if (_detectFurniture != null)
        {
            StopCoroutine(_detectFurniture);
            _detectFurniture = null;
        }
    }

    private void OnMoveFurnitureButtonDown()
    {
        moveFurnitureButton.gameObject.SetActive(false);
        StopDetectFurnitureCoroutine();
        StartMoveFurnitureCoroutine();
    }



    //Move Furniture


    public void StartMoveFurnitureCoroutine()
    {
        _moveFurniture ??= StartCoroutine(MoveFurnitureCoroutine());
    }

    IEnumerator MoveFurnitureCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, 10f, floorLayer))
            {
                Vector3 potentialPosition = hit.point;

                if(furnitureCollider == null)
                {
                    furnitureCollider = furniture.GetComponent<BoxCollider>();
                }

                Bounds bounds = furnitureCollider.bounds;
                Vector3 halfExtents = new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);

                Vector3 offsetPosition = potentialPosition + bounds.center - furniture.transform.position;
                Collider[] colliders = Physics.OverlapBox(offsetPosition, halfExtents, Quaternion.identity, collisinLayer);
               
                if (colliders.Length == 1)
                {
                    furniture.transform.position = potentialPosition;
                    placeFurnitureButton.gameObject.SetActive(true);
                    rotateFurnitureButton.gameObject.SetActive(true);
                    outLine.OutlineColor = Color.green;
                }
                else
                {
                    furniture.transform.position = potentialPosition;
                    placeFurnitureButton.gameObject.SetActive(false);
                    rotateFurnitureButton.gameObject.SetActive(true);
                    if(outLine != null) outLine.OutlineColor = Color.red;
                }
            }
            else
            {
                placeFurnitureButton.gameObject.SetActive(false);
                rotateFurnitureButton.gameObject.SetActive(false);
            }

        }
    }
    public void StopMoveFurnitureCoroutine()
    {
        if(_moveFurniture!= null)
        {
            StopCoroutine(_moveFurniture);
            _moveFurniture = null;
        }
    }

    private void OnPlaceFurnitureButtonDown()
    {
        placeFurnitureButton.gameObject.SetActive(false);
        rotateFurnitureButton.gameObject.SetActive(false);
        StopMoveFurnitureCoroutine();
        StartDetectFurnitureCoroutine();
        furnitureCollider = null;
    }

    private void OnRotateFurnitureButtonDown()
    {
        Quaternion currentRotation = furniture.transform.rotation;
        Quaternion newRotation = currentRotation * Quaternion.Euler(0, 90, 0);
        furniture.transform.rotation = newRotation;
    }

    public bool IsFurnitureNull()
    {
        if(furniture == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
