using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation.Examples;

public class Helicopter : MonoBehaviour
{
    public GameObject _helicopter;
    public Transform _targetCarTransform;
    public GameObject _instantiatedHelicopter;

    public List<GameObject> Helicopters;


    private void OnEnable()
    {
        staticevents.GameEvents.ObjectToPick += AssignTarget;
    }

    public void BringHelicopter()
    {
       
        EnableHelicopter();
        InstantiateHelicopter();
        StartCoroutine(MoveToCar());
    }

    public void EnableHelicopter()
    {
        _helicopter.SetActive(true);
    }

    public void InstantiateHelicopter()
    {
        _instantiatedHelicopter = Instantiate(_helicopter);
        _instantiatedHelicopter.transform.position = new Vector3(-60f, 60f, 0f);
        Helicopters.Add(_instantiatedHelicopter);
    }

    IEnumerator MoveToCar()
    {
        Transform x = _targetCarTransform.GetComponent<BaseCarMove>()._helicopterPoint;
        
        yield return _instantiatedHelicopter.transform.DOMoveX(x.position.x , 3f);
        Quaternion targetRotation = Quaternion.Euler(0f, x.rotation.eulerAngles.y-180f, 0f);
        yield return _instantiatedHelicopter.transform.DOLocalRotateQuaternion(targetRotation, 3f);
        yield return _instantiatedHelicopter.transform.DOMoveZ(x.position.z, 3f);
        yield return _instantiatedHelicopter.transform.DOMoveY(x.position.y + 0.2f, 3f).WaitForCompletion();


        yield return _targetCarTransform.DOShakeRotation(duration: 2f,strength: 2f,vibrato: 5,randomness: 0f,fadeOut: false).WaitForCompletion();



        _targetCarTransform.parent = null;

        _targetCarTransform.GetComponent<BoxCollider>().enabled = false;
        _targetCarTransform.GetComponent<Rigidbody>().isKinematic = true;

        _targetCarTransform.parent = _instantiatedHelicopter.transform;

        PathFollower pathfollow = _targetCarTransform.transform.GetComponent<BaseCarMove>()._pathfollower;
        staticevents.GameEvents.RemoveCarFromList?.Invoke(pathfollow);
        staticevents.GameEvents.OnCarReached?.Invoke();
        GameManager.Instance._isHelicopter = false;

        yield return _instantiatedHelicopter.transform.DOMoveY(60f, 3f);
        yield return _instantiatedHelicopter.transform.DOMoveX(60f, 3f).WaitForCompletion();

        Helicopters.Remove(_instantiatedHelicopter);
        Destroy(_instantiatedHelicopter);

        _instantiatedHelicopter = null;
        _targetCarTransform = null;

    }

    public void AssignTarget(Transform x)
    {
        if (Helicopters.Count != 0) return;


        _targetCarTransform = x;
        BringHelicopter();
    }

    public void MakeBoolTrue()
    {
        GameManager.Instance.MakeBoolTrue();
        UIManager.instance.ShowSelectCarUI();
    }

    private void OnDisable()
    {
        staticevents.GameEvents.ObjectToPick -= AssignTarget;
    }
}
