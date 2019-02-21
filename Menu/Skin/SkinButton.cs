using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinButton : MonoBehaviour
{   
    public int index;
    public GameObject NotPurchasePanel;

    private void OnEnable()
    {
        DataChangeEvent.ChangeSkinEvent += SetSkinPanel;

        SetSkinPanel();
    }

    private void SetSkinPanel()
    {
        if (index != 0 && PlayerPrefs.GetFloat("Skin_" + index, index == 0 ? 1 : 0) == 0)
        {
            // 구매 전 스킨
            NotPurchasePanel.SetActive(true);
        }
        else
        {
            // 구매완료된 스킨
            NotPurchasePanel.SetActive(false);
            if (PlayerPrefs.GetFloat("Skin", 0) == index)
            {
                // 착용중인 스킨
                SetWearingButton();
            }
            else
            {
                // 착용하지 않은 스킨
                SetNotWearingButton();
            }
        }
    }

    private void SetWearingButton()
    {
        // 착용중인 스킨 셋팅
        GetComponentInChildren<Text>().text = "착용중";
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void SetNotWearingButton()
    {
        // 착용중이지 않은 스킨 셋팅
        GetComponentInChildren<Text>().text = "착용하기";
        GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        GetComponent<Button>().onClick.RemoveAllListeners();

        GetComponent<Button>().onClick.AddListener(() =>
        {
            PlayerPrefs.SetFloat("Skin", index);

            DataChangeEvent.Instance.ChangeSkin();
        });
    }
}