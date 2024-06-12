using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    [SerializeField] private Transform cartonPoint;
    [SerializeField] private GameObject playerRoot;

    public Transform GetCartonPoint() => cartonPoint;
    public GameObject GetPlayerRoot() => playerRoot;
}
