using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Level")]
public class LevelScriptable : ScriptableObject
{
    public int LevelNumber;
    public int NumberOfCars;
    public GameObject LevelPrefab;
}
