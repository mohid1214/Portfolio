using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVisualSelection : MonoBehaviour
{
    [SerializeField] private List<CharacterVisual> charactersList = new List<CharacterVisual>();
    [SerializeField] private Transform visualParentTransform;
    [SerializeField] private CharacterVisual characterVisual;

    private void Awake()
    {
        int randomCharacter = Random.Range(0, charactersList.Count);
        characterVisual = Instantiate(charactersList[randomCharacter], visualParentTransform);
    }

    public CharacterVisual GetCharacterVisual() => characterVisual;
}
