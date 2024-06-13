using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Lean.Touch;
using JetBrains.Annotations;

public abstract class AvatarController : MonoBehaviour
{
    public float TotalHealth;
    public float health;
    public float attack;

    [Header("Current Target")]
    public GameObject CurrentTarget;


    public GridRepositioning grid;

    public static bool IsAnyPlayerSelected;

    [Header("External")]
    public Engine engine;

    [Header("Internal")]
    public Transform body;



    //is it on ranger side or enemy side
    public CharacterSide side;

    //public CharacterType type; we only have spinners so we do not need a character type;



    [SerializeField] protected Animator animator;

    [SerializeField] Animator bodyAnimator;



    [Header("FX")]
    public GameObject mergeFx;
    public GameObject placeFx;

    public List<AvatarController> _enemies;

    protected bool Dead;

    public bool canmove;

    public List<SkinnedMeshRenderer> _skinned;
    protected List<Material> _mats = new();
    protected List<Color> _defaultColor = new();

    Coroutine _cor;

    public int value;

    protected AvatarController _otherAvatar;

    public Camera cam;

    protected bool once = false;

    public LeanSelectableByFinger _finger;

    public bool BoughtNow = false;

    public abstract bool IsDead();


    [SerializeField] public Rigidbody rb;

   
    public virtual void Start()
    {
        TotalHealth = health;

        if (side == CharacterSide.Enemy)
        {
            _finger.enabled = false;

            //healthBar.transform.localRotation = Quaternion.Euler(30, -180, 0);
            //healthBarFill_img.color = new Color32(243, 58, 24, 255);
        }

        if (side == CharacterSide.Ranger)
        {
            //  healthBarFill_img.color = new Color32(0, 171, 255, 255);
            Dead = false;
        }


    }


    /*private void FixedUpdate()
    {
        if (canmove)
        {
            
            engine.action = ActionState.Attacking;
            transform.position = Vector3.MoveTowards(transform.position, CurrentTarget.transform.position, Time.deltaTime);

        }
    }*/

    public void ThisObjectSelecetd(bool _t)
    {
        Debug.Log("I was called on clicks up or down");
        if (engine.action != ActionState.BeforeIdle)
            return;

        Debug.Log("I was able to go through the return statement");

        IsAnyPlayerSelected = _t;

        if (!_t)
        {
            Debug.Log("I was able to go through the !t");
            body.transform.localPosition = Vector3.zero;
        }
    }


    public virtual void Merge(AvatarController character)
    {
        if (engine.action != ActionState.BeforeIdle)
            return;

        if (character == null)
            return;

        Destroy(character.gameObject);
        Destroy(this.body.GetChild(0).gameObject);


        if (side == CharacterSide.Enemy)
        {
            //engine.EnemiesList._SpinnerAvatar.Remove(this);
            //engine.EnemiesList._SpinnerAvatar.Remove(character);
        }
        else
        {
            //engine.RangersList._SpinnerAvatar.Remove(this);
            //engine.RangersList._SpinnerAvatar.Remove(character);
        }

        //Invoke(nameof(boolBack), 0.05f);
    }



    void boolBack()
    {
        once = false;
    }

    public bool IsMergePossible(AvatarController _a, AvatarController _b)
    {

        if (_a.value != _b.value)
            return false;

        if (_a.side == CharacterSide.Enemy)
            return false;

        if (_b.side == CharacterSide.Enemy)
            return false;

        return true;

    }

    public void SearchForTarget()
    {
        _enemies.Clear();

       var _characters = FindObjectsOfType<AvatarController>();

        for(int i=0; i<_characters.Length; i++)
        {
            if(side == CharacterSide.Ranger && _characters[i].side == CharacterSide.Enemy)
            {
                _enemies.Add(_characters[i]);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (engine.action != ActionState.BeforeIdle)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Death"))
            return;


        if (!other.gameObject.CompareTag("Fighter"))
            return;                                                       commnedted this iteration

        if (IsAnyPlayerSelected)
            return;

        if (engine.action != ActionState.BeforeIdle)
            return;

        AvatarController _otherAvatar = other.GetComponent<AvatarController>();

        if (!IsMergePossible(this, _otherAvatar))
            return;


        Merge(_otherAvatar);*/

        /*if (engine.action == ActionState.BeforeIdle)  //it was idle, I changed it to before idle  //commented by muhammad sumyal
    {*/

        /*if (!IsAnyPlayerSelected )  //commented by muhammad sumyal
        {*/
        /*}*/

        /*}*/

        /* if (engine.action == ActionState.Start)
        {

            _otherAvatar = other.transform.gameObject.GetComponent<AvatarController>();



            if (_otherAvatar == null)
            {


                _otherAvatar = other.transform.gameObject.transform.parent.parent.GetComponent<AvatarController>();

            }


            if (side != _otherAvatar.side && !_otherAvatar.IsDead())
            {
                //isTouchingOpponent = true;
            }

        }*/


    }


}
