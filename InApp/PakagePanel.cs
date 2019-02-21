using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PakagePanel : MonoBehaviour
{
    public int index;

    private void OnEnable()
    {
        DataChangeEvent.PurchasePakageEvent += () =>
        {
            if ((PlayerPrefs.GetFloat("Town_" + (index - 1), 0) == 0 && index != 0) ||
                PlayerPrefs.GetFloat("Town_" + (index + 2), 0) == 1)
            {
                // 이전 마을을 획득하지 않았을 때
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        };

        if ((PlayerPrefs.GetFloat("Town_" + (index - 1), 0) == 0 && index != 0) ||
            PlayerPrefs.GetFloat("Town_" + (index + 2), 0) == 1)
        {
            // 이전 마을을 획득하지 않았을 때
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}