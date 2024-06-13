using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightCollision : MonoBehaviour
{
    private Coroutine _TimerDelay;

    public AvatarController _thisAvatarController;
    public AvatarController _otherAvatarController;
    public Slider healthSlider;

    [SerializeField] private Vector3 _collisionHitPoint;


    private void Start()
    {

        if (_thisAvatarController == null)
        {
            //_thisAvatarController = transform.GetComponentInParent<TargetAssigner>()._parentAvatar;
        }
        if(healthSlider == null)
        {
            //healthSlider = transform.GetComponentInParent<HealthManager>()._healthSlider;
        }
        
        SetHealthSliderValue();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (_TimerDelay != null) return;

       if(!other.gameObject.CompareTag("Maro")) return;


       

        if(_otherAvatarController == null)
        {
            //_otherAvatarController = other.GetComponentInParent<TargetAssigner>()._parentAvatar;
            Debug.Log(_otherAvatarController);
        }

        if (_thisAvatarController.side == _otherAvatarController.side)
            return;

        _TimerDelay = StartCoroutine(DelayInTrigegr());

        _collisionHitPoint = other.ClosestPointOnBounds(transform.position);

        
    }


    IEnumerator DelayInTrigegr()
    {
        yield return new WaitForSeconds(0.2f);
        DecreaseHealthOnCollision();
        PushSpinnersApart();
        StopTimerCoroutine();
    }

    private void StopTimerCoroutine()
    {
        if (_TimerDelay == null) return;

        StopCoroutine(_TimerDelay);
        _TimerDelay = null;
    }


    //we already checked if it is colliding with enemy else return;
    public void DecreaseHealthOnCollision()
    {

        _thisAvatarController.health -= _otherAvatarController.attack;
        healthSlider.value = _thisAvatarController.health;
        if( _thisAvatarController.health < 1)
        {
            //_thisAvatarController.gameObject.SetActive(false);
        }
    }

    public void SetHealthSliderValue()
    {
        healthSlider.maxValue = _thisAvatarController.health;
        healthSlider.value = _thisAvatarController.health;
    }


    #region Pushing spinners apart after collision
    public void PushSpinnersApart()
    {
        Vector3 _direction = _collisionHitPoint - _thisAvatarController.transform.position;

        Vector2 x = GiveVectorTwo(_direction);

        Vector3 xy = GiveVectorThree(x);

        xy.Normalize();

        _thisAvatarController.rb.AddForce(-xy * 140f,ForceMode.Impulse);
        _otherAvatarController.rb.AddForce(xy * 140f, ForceMode.Impulse);

        Debug.Log("Seperation force applied");
    }

    public Vector2 GiveVectorTwo(Vector3 pos)
    {
        return new Vector2(pos.x, pos.y);
    }

    public Vector3 GiveVectorThree(Vector2 pos)
    {
        return new Vector3(pos.x, 0f, pos.y);
    }
    #endregion
}
