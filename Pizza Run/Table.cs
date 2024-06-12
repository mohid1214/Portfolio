using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Chairs> chairsList;
    [SerializeField] private Transform stackingPoint;
    [SerializeField] private List<GameObject> foodList;
    [SerializeField] private GameObject trash;
    [SerializeField] private List<GameObject> trashList;
    [SerializeField] private List<GameObject> trashStackPoints;
    [SerializeField] private BoxCollider collders;
    [SerializeField] private ChairCashStack chairStack;
    [SerializeField] private UnLockTable unlockTable;

    private Player player;
    private Coroutine PickTrashCoroutine;   

    public List<Chairs> GetChairList() => chairsList;
    public Transform GetStackingPoint() => stackingPoint;

    public void AddToFoodList(GameObject food) => foodList.Add(food);
    public void RemoveFoodFromList(GameObject food) => foodList.Remove(food);
    public List<GameObject> GetFoodList() => foodList;

    public List<GameObject> GetTrashStackPoints() => trashStackPoints;
    public GameObject GetTrashGameObject() => trash;
    public void AddToTrashList(GameObject newTrash) => trashList.Add(newTrash);
    public void RemoveTrashFromList(GameObject newTrash) => trashList.Remove(newTrash);

    

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        player = other.gameObject.GetComponent<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (player == null) return;
        if (!other.gameObject.CompareTag("Player")) return;
        StartCoroutineNow();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        StopCoroutineNow();
    }

    private void StartCoroutineNow()
    {
        if (player.GetItemName() != "null" && player.GetItemName() != "trash") return;
        if (PickTrashCoroutine == null)
        {
            PickTrashCoroutine = StartCoroutine(PlayerPickTrash());
        }
        else
        {
            return;
        }
    }

    IEnumerator PlayerPickTrash()
    {
        if (trashList.Count == 0)
        {
            StopCoroutineNow();
            yield break;
        }
        Tutorial.Instance.ShowDropTrash();

        for (int i = trashList.Count - 1; i >= 0; i--)
        {
            GameObject newTrash = trashList[^1];
            newTrash.transform.SetParent(player.transform);
            player.ArrangeFoodInStacks(newTrash,0.05f);
            trashList.Remove(newTrash);
            player.SetItemName("trash");
            yield return new WaitForSeconds(0.05f);
        }

        if(trashList.Count == 0)
        {
            SetColliderOff();
            CleanTablesAchievemet.Instance.IncreaseNumberOfTablesCleaned();
            SetChairsBack();
        }
        StopCoroutineNow();
        player.CheckFoodListZero();
    }
    private void StopCoroutineNow()
    {
        if(PickTrashCoroutine != null)
        {
            StopCoroutine(PickTrashCoroutine);
            PickTrashCoroutine = null;
        }
    }

    public void SetColliderOn()
    {
        collders.enabled = true;
    }

    private void SetColliderOff()
    {
        collders.enabled = false;
    }

    private void SetChairsBack()
    {
        
        for(int i=0; i<chairsList.Count; i++)
        {
            Chairs newChair = chairsList[i];
            newChair.transform.DOLocalMove(newChair.GetChairStartingPoint().localPosition, 0.5f);
            Vector3 targetRotation = newChair.GetChairStartingPoint().transform.eulerAngles;
            newChair.transform.DORotate(targetRotation, 0.5f);
            newChair.SetHasCustomerFalse();
        }
    }

    public List<GameObject> GetTrashList() => trashList;

    public bool IsColliderOn() => collders.enabled;

    public ChairCashStack GetChairStack() => chairStack;

}
