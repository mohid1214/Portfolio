using JetBrains.Annotations;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHolders : MonoBehaviour
{
    public List<PathFollower> carList;


    private void OnEnable()
    {
        staticevents.GameEvents.RemoveCarFromList += RemoveCarFromList;
        staticevents.GameEvents._getList += GiveList;
    }

    public bool AnyCarMooving()
    {
        for(int i=0; i<carList.Count; i++)
        {
            if (carList[i].enabled== true)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveCarFromList(PathFollower x)
    {
        if (carList.Contains(x))
        {
            carList.Remove(x);
        }
    }

    public void GiveList()
    {
        staticevents.GameEvents._pathFollowersList?.Invoke(carList);
        Debug.Log("sending list");
    }

    private void OnDisable()
    {
        staticevents.GameEvents.RemoveCarFromList -= RemoveCarFromList;
        staticevents.GameEvents._getList -= GiveList;
    }
}
