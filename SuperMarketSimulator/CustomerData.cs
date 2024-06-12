using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerData : MonoBehaviour
{
    [SerializeField] private NavMeshAgent customerAgent;
    [SerializeField] private CustomerMovement customerMovement;
    [SerializeField] private CharacterVisualSelection characterVisual;
    public CustomerMovement GetCustomerMovementScript() => customerMovement;
    public void DisableNavmesh()=>customerAgent.enabled =false;
    public void EnableNavmesh()=>customerAgent.enabled =true;
    public void MoveCustomerToCurrentTarget(Transform target)
    {
        customerAgent.SetDestination(target.position);
    }

    public bool HasReachedDestination()
    {
        if (!customerAgent.pathPending)
        {
            if (customerAgent.remainingDistance <= customerAgent.stoppingDistance)
            {
                if (!customerAgent.hasPath || customerAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public CharacterVisualSelection GetCharacterVisualScript() => characterVisual;
}
