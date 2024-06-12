using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackCustomerPoint : MonoBehaviour
{
    [SerializeField] private Rack rack;

    public Rack GetRack() => rack;
}
