using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVisualSelector : MonoBehaviour
{
    public List<GameObject> randomCharacterList;

    private void OnEnable()
    {
        int randomNumber = Random.Range(0, randomCharacterList.Count - 1);
        for(int i=0; i<randomCharacterList.Count; i++)
        {
            randomCharacterList[i].SetActive(false);
        }

        randomCharacterList[randomNumber].SetActive(true);
    }
}
