using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointingArrow : MonoBehaviour
{
    public static PointingArrow Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}

