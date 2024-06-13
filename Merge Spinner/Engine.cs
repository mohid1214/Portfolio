using DG.DemiLib;
using Lean.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Engine : MonoBehaviour
{

    [Header("Healths")]
    public float EnemiesHealthLeft = 0;
    public float RangersHealthLeft = 0;
    public float TotalEnemyHealth;
    public float TotalRangersHealth;
    bool DecreasingHealth;



    [Header("Environment")]
    [SerializeField] private Transform environmentContainer;
    [SerializeField] private List<GameObject> environments;



    [Header("Level")]
    public int level = 1;
    public Level currentLevel;
    public List<Level> levels = new List<Level>();

    [Header("enemy requirements")]
    public Transform EnemyContainer;
    public Transform[] EnemyGrid;


    [Header("Ranger Requirements")]
    public List<int> activeRangerPos = new List<int>();
    public List<Transform> rangerGrid = new List<Transform>();
    public GameObject rangerContainer;

    [Header("AvatarController")]
    public AvatarController AvatarSpinner;

    [Header("spinner Characters")]
    public CharacterPrefab[] characterprefabs;

    [Header("Keeping count")]
    public int availableSpinners;

    [Header("Things")]
    public Things RangersList;
    public Things Enemylist;

    [Header("Events")]
    public static Action patrolEvent;



    [System.Serializable]
    public struct Things
    {
        public List<AvatarController> _spinnerAvatar;
    }

    [Header("Action State")]
    public ActionState action;


    private void OnEnable()
    {
        Patrol1.DeadEvent += RemoveFromList;
    }

    private void Start()
    {
        LoadLevel();

        if(level == 1)
        {
            CreateRangerWithColum(12, 2);
        }
    }

    void LoadLevel()
    {
        InstantiateLevelContainer();
        MakeEnemyOnStart();
    }

    void InstantiateLevelContainer()
    {
        Instantiate(environments[(level - 1) / 10], environmentContainer.position, Quaternion.identity, environmentContainer);
    }

    void MakeEnemyOnStart()
    {
        currentLevel = levels[level - 1];
        GridItem[] tempItems = currentLevel.items;

        for (int i = 0; i < tempItems.Length; i++)
        {
            if (tempItems[i]._hasItem)
            {
                int somemath = (ValueFromPower(tempItems[i].value) + 1);
                CreateFighter(CharacterSide.Enemy, somemath, i, i, EnemyGrid[i].position, EnemyContainer);
            }

        }
    }

    int ValueFromPower(int val)
    {
        float x = val;
        int counter = 0;
        while (x >= 2)
        {
            x /= 2;
            counter++;
        }

        if (x == 1)
        {
            return counter;
        }
        else
        {
            return -1;
        }
    }


    void CreateRangerWithColum(int col, int value)
    {
        if (activeRangerPos.Count >= 15) return;

        int randomNumber = UnityEngine.Random.Range(0, 15);

        randomNumber = col;

        if(activeRangerPos.Contains(randomNumber))
        {
            CreateRanger();
            return;
        }
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        CreateFighter(CharacterSide.Ranger, (1), randomNumber, randomNumber, rangerGrid[randomNumber].position + offset, rangerContainer.transform);
        UpdateRangerPositions();
    }

    void CreateRanger()
    {
        if (activeRangerPos.Count >= 15) return;

        int randomNumber = UnityEngine.Random.Range(0, 15);

        if (activeRangerPos.Contains(randomNumber))
        {
            CreateRanger();
            return;
        }
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        CreateFighter(CharacterSide.Ranger, (1), randomNumber, randomNumber, rangerGrid[randomNumber].position + offset, rangerContainer.transform);
        UpdateRangerPositions();
    }


    #region Creating Fighter
    public void CreateFighter(CharacterSide side, int value, int row, int col, Vector3 pos, Transform container)
    {
        AvatarController GO = null;
        GameObject body = null;

        GO = InstantiateAvatarSpinner(pos, container);

        body = InstatiateBody(value, GO);

        CharacterInformation _info = GetBaseValueofGame.GetSpinnerBaseAtLevel(value);

        AssignHealthAndAttack(GO, _info);

        RangerSide(GO, side, value);

        EnemySide(GO,side);

        GO.engine = this;

        GO.grid.CurrentGridN = row;


        body.transform.localPosition = Vector3.zero;
        GO.body = body.transform.parent;
        GO.value = value;


        if (side == CharacterSide.Ranger && action == ActionState.Idle)
        {
            GO.BoughtNow = true;
        }
    }
    AvatarController InstantiateAvatarSpinner(Vector3 pos, Transform container)
    {
        return Instantiate(AvatarSpinner, pos, Quaternion.identity, container);
    }

    GameObject InstatiateBody(int value, AvatarController GO)
    {
        return Instantiate(characterprefabs[value-1].prefab, Vector3.zero, Quaternion.identity, GO.transform.GetChild(0).transform);
    }

    void AssignHealthAndAttack(AvatarController GO, CharacterInformation _info)
    {
        GO.health = _info.Health;
        GO.attack = _info.Attack;
    }

    void RangerSide(AvatarController GO, CharacterSide side,int value)
    {
        if (side == CharacterSide.Ranger)
        {

            TotalRangersHealth += GO.health;
            int mul = (int)Mathf.Pow(2, value - 1);
            availableSpinners += mul;
            GO.side = CharacterSide.Ranger;
            RangersList._spinnerAvatar.Add(GO);
        }
    }

    void EnemySide(AvatarController GO,CharacterSide side)
    {
        if (side == CharacterSide.Enemy)
        {

            TotalEnemyHealth += GO.health;
            GO.side = CharacterSide.Enemy;
            Enemylist._spinnerAvatar.Add(GO);

        }
    }

    public void UpdateRangerPositions()
    {

        activeRangerPos.Clear();
        for (int i = 0; i < rangerContainer.transform.childCount; i++)
        {
            if (rangerContainer.transform.GetChild(i).GetComponent<AvatarController>() != null)
            {
                AvatarController _char = rangerContainer.transform.GetChild(i).GetComponent<AvatarController>();
                activeRangerPos.Add(_char.grid.CurrentGridN);
            }
        }

    }
    #endregion

    void RemoveFromList(AvatarController _avatarController)
    {
        if (RangersList._spinnerAvatar.Contains(_avatarController))
        {
            RangersList._spinnerAvatar.Remove(_avatarController);
        }

        if (Enemylist._spinnerAvatar.Contains(_avatarController))
        {
            Enemylist._spinnerAvatar.Remove(_avatarController);
        }
    }

    public void startFight()
    {
        DisableIsKinematicEnemy();
        DisableIskinematicRanger();
        Engine.patrolEvent?.Invoke();
    }

    void DisableIskinematicRanger()
    {
        for (int i = 0; i < RangersList._spinnerAvatar.Count; i++)
        {
            RangersList._spinnerAvatar[i].rb.isKinematic = false;
        }
    }

    void DisableIsKinematicEnemy()
    {
        for (int i = 0; i < Enemylist._spinnerAvatar.Count; i++)
        {
            Enemylist._spinnerAvatar[i].rb.isKinematic = false;
        }
    }

    private void OnDisable()
    {
        Patrol1.DeadEvent -= RemoveFromList;
    }
}


