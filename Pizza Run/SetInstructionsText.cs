using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInstructionsText : MonoBehaviour
{
    public static SetInstructionsText Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI instructionsText;

    private void Awake()
    {
        Instance = this;
    }

    public void NewInstrutionText(string x)
    {
        instructionsText.text = x;
    }
}
