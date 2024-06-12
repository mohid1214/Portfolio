using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField] private Animator characterAnimator;

   
    public void Walk()
    {
        characterAnimator.SetBool("walk", true);
    }

    public void Idle()
    {
        characterAnimator.SetBool("walk", false);
        characterAnimator.SetBool("pick", false);
    }

    public void Pick()
    {
        characterAnimator.SetBool("pick",true);
    }
}
