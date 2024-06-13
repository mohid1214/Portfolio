using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlades : MonoBehaviour
{
    [SerializeField] private Transform _targetObject;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private Ease spinEase = Ease.Linear;

    void Start()
    {
        StartSpinning();
    }

    private void StartSpinning()
    {
        // Infinite loop for continuous spinning
        Tween spinTween = null;
        spinTween = _targetObject.DORotate(new Vector3(0f, 360f, 0f), spinDuration, RotateMode.LocalAxisAdd)
            .SetEase(spinEase)
            .OnComplete(() => RestartSpinning(spinTween));
    }

    private void RestartSpinning(Tween spinTween)
    {
        // Reset the object's rotation before restarting the spinning
        _targetObject.rotation = Quaternion.identity;

        // Restart the spinning animation
        spinTween.Restart();
    }
}
