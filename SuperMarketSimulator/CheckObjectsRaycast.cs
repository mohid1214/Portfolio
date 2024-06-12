using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CheckObjectsRaycast : MonoBehaviour
{
    [SerializeField] private MyPlayer player;
    [SerializeField] private GameObject carton;

    private bool isCarton;
    private bool cartonPicked;

    private Coroutine RaycastCoroutine;

    

   

    public bool HasCarton() => isCarton;

    public void SetCartonPickedTrue() => cartonPicked=true;
    public void SetCartonPickedFalse()=>cartonPicked=false;

    public GameObject GetCarton() => carton;
    public MyPlayer GetPlayer() => player;
}
