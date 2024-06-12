using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RemoveCashierBox : MonoBehaviour
{
    public GameObject box;
    public CashierDataScript cashierDataScript;
    public DOTweenAnimation doTweenAnim;
    public ParticleSystem unlockCashierParticle;

   

    private void Update()
    {
        if (box.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == gameObject)
                    {
                        cashierDataScript.enabled = true;
                        SoundManager.Instance.OnCrateOpenSound();
                        SoundManager.Instance.PlayStrongvibration();
                        doTweenAnim.enabled = true;
                        unlockCashierParticle.Play();
                        box.SetActive(false);
                        gameObject.SetActive(false);
                        CashierPrepareOrder.Instance.AddInstantiatedCustomer(cashierDataScript);
                        CashierTakeOrder.Instance.AddInstantiatedCustomer(cashierDataScript);
                    }
                }

            }
        }
    }
}
