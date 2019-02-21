using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseButton : MonoBehaviour
{
    public int index;

    public GameObject Menu;
    public GameObject BackPanel;

    public GameObject NotPurchasePanel;

    private void OnEnable()
    {
        var ReverseIndex = (int) PlayerPrefs.GetFloat("Reverse_" + index, index == 0 ? 1 : 0);
        
        if (ReverseIndex == 0)
        {
            // 환생 하지 않은 마을
        }
        else if (ReverseIndex == 1)
        {
            NotPurchasePanel.SetActive(false);
            if (PlayerPrefs.GetFloat("WearingTown", 0) != index)
            {
                // 환생했지만 착용중이지 않은 마을
                GetComponentInChildren<Text>().text = "이동하기";
                GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                GetComponent<Button>().onClick.AddListener(() =>
                {
                    Menu.SetActive(false);
                    BackPanel.SetActive(false);
                    
                    PlayerPrefs.SetFloat("WearingTown", index);
                    
                    DataChangeEvent.Instance.ResetData();
                });
            }
            else
            {
                // 착용중인 마을
                GetComponentInChildren<Text>().text = "착용중";
                GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
                GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
    }
}