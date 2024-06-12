using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : Player
{
    [SerializeField] private AIHolder aiHolder;

    [Header("Player")]
    [SerializeField] private NavMeshAgent navmeshAgent;
    public bool busy { get; private set; }

    [Space(10)]

    [Header("pick Food")]
    [SerializeField] private MainMachine foodMakingMachine;
    [Header("drop Food")]
    [SerializeField] private List<TakeItemMain> takeFoodFromPlayer = new List<TakeItemMain>();

    [Space(10)]

    [Header("pick Trash")]
    [SerializeField] private TableHolder tableHolder;
    [Header("dropTrash")]
    [SerializeField] private List<TrashCan> trashCans;

    [Space(10)]

    [Header("pick Box")]
    [SerializeField] private PackFood packFood;
    [Header("drop Box")]
    [SerializeField] private TakeItemMain takeBoxFromPlayer;

   

    public void PickFood()
    {
        busy = true;
        SetNavMesgAgentDestinition(foodMakingMachine.GetAiPoint().position);
        StartCoroutine(GoToPickFood());
    }

    IEnumerator GoToPickFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(GetFoodList().Count > GetMaxLimit())
            {
                DropFood();
                StopCoroutine(GoToPickFood());
                yield break;
            }
        }
    }

    private void DropFood()
    {
        TakeItemMain takeItem = null;
        if (takeFoodFromPlayer[0].GetFoodList().Count < takeFoodFromPlayer[1].GetFoodList().Count)
        {
            takeItem = takeFoodFromPlayer[0];
        }
        else
        {
            takeItem = takeFoodFromPlayer[1];
        }

        SetNavMesgAgentDestinition(takeItem.GetAIPickPoint().position);
        StartCoroutine(TaskCompleteFood());
    }

    IEnumerator TaskCompleteFood()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(navmeshAgent.remainingDistance < 1f)
            {
                busy = false;
                break;
            }
        }
    }

    public void PickTrash()
    {
        if (tableHolder.GetTableList().Count == 0) return;
        for (int i = 0; i < tableHolder.GetTableList().Count; i++)
        {
            Table table = tableHolder.GetTableList()[i];
            if (table.GetTrashList().Count != 0)
            {
                busy = true;
                StartCoroutine(GoToPickTrash());
            }
        }
    }

    IEnumerator GoToPickTrash()
    {
        for (int i = 0; i < tableHolder.GetTableList().Count; i++)
        {
            Table table = tableHolder.GetTableList()[i];
            if (table.IsColliderOn())
            {
                if (GetFoodList().Count < GetMaxLimit() + 2)
                {
                    if (table.GetTrashList().Count != 0)
                    {
                        navmeshAgent.SetDestination(table.transform.position);
                        while (true)
                        {
                            yield return new WaitForSeconds(1f);
                            if (table.GetTrashList().Count == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        if(GetFoodList().Count == 0)
        {
            busy = false;
        }
        else
        {
            DropTrash();
        }

    }

    private void DropTrash()
    {
        Vector3 targetPos = trashCans[0].GetAIPoint().position;
        SetNavMesgAgentDestinition(targetPos);
        StartCoroutine(TaskCompleteTrash());
    }

    IEnumerator TaskCompleteTrash()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (navmeshAgent.remainingDistance < 1f)
            {
                busy = false;
                break;
            }
        }
    }
    public void PickBox()
    {
        if (packFood.GetBoxList().Count > 0)
        {
            busy = true;
            SetNavMesgAgentDestinition(packFood.GetBoxPickingPoint().position);
            StartCoroutine(GoToPickBox());
        }
    }
    IEnumerator GoToPickBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (GetFoodList().Count > GetMaxLimit())
            {
                DropBox();
                StopCoroutine(GoToPickFood());
                yield break;
            }
        }
    }
    private void DropBox()
    {
        SetNavMesgAgentDestinition(takeBoxFromPlayer.GetAIPickPoint().position);
        StartCoroutine(TaskCompleteBox());
    }
    IEnumerator TaskCompleteBox()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (navmeshAgent.remainingDistance < 1f)
            {
                busy = false;
                break;
            }
        }
    }

    private void SetNavMesgAgentDestinition(Vector3 targetPos)
    {
        navmeshAgent.SetDestination(targetPos);
    }

    public NavMeshAgent GetNavmeshAgent() => navmeshAgent;
}

