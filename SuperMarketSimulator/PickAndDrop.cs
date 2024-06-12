using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndDrop : MonoBehaviour
{
    [SerializeField] private LayerMask pickable;
    [SerializeField] private GameObject grabbedItem;

    private void Update()
    {

        if (!Input.GetMouseButton(0)) return;

        if(grabbedItem != null)
        {
            Vector3 screenPosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 target = hit.point + new Vector3(0f, grabbedItem.transform.localScale.y / 2, 0f);
                grabbedItem.transform.position = target;
            }

        }
        else
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 5f, pickable))
            {
                if (hit.transform == null) return;
                grabbedItem = hit.transform.gameObject;
                grabbedItem.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
