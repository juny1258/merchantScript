using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;

public class SkinPakage : MonoBehaviour
{
    public int index;
    public GameObject PurchasePanel;

    private void OnEnable()
    {
        PurchasePanel.SetActive(PlayerPrefs.GetFloat("Skin_" + index, 0) == 1);
    }

    public void PurchaseSkinPakage()
    {
        PlayerPrefs.SetFloat("Skin_" + index, 1);
        DataController.Instance.skinGoldPerClick += 5;
        DataController.Instance.skinGoldPerSec += 5;

        PlayerPrefs.SetFloat("RandomBoxCount", PlayerPrefs.GetFloat("RandomBoxCount", 0) + 10);
        DataController.Instance.dia += 3000;

        DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                     DataController.Instance.levelGoldPerClick *
                                                     DataController.Instance.skillGoldPerClick *
                                                     DataController.Instance.plusGoldPerClick *
                                                     DataController.Instance.collectionGoldPerClick *
                                                     DataController.Instance.reinforceGoldPerClick *
                                                     DataController.Instance.skinGoldPerClick *
                                                     DataController.Instance.reverseGolePerClick;
        PurchasePanel.SetActive(true);
    }

    public void PurchaseChristmasPakage()
    {
        PlayerPrefs.SetFloat("Skin_" + index, 1);
        DataController.Instance.criticalPercent += 20;
        DataController.Instance.criticalRising += 2;

        DataController.Instance.criticalPer = DataController.Instance.criticalPercent;
        
        PlayerPrefs.SetFloat("RandomBoxCount", PlayerPrefs.GetFloat("RandomBoxCount", 0) + 10);
        DataController.Instance.dia += 3000;
        
        PurchasePanel.SetActive(true);
    }

    public static string xmlData;

    private void SetFloat(string key, float value)
    {
        if (!xmlData.Contains(key + ":f:"))
        {
            xmlData += key + ":f:" + value + ",";
        }
        else
        {
            xmlData.Replace(key + ":i:" + xmlData.Replace(key + ":i:", "|").Split('|')[1].Split(',')[0], key + ":i:" + value);
        }
    }

    private float GetFloat(string key, float first_value)
    {
        if (!xmlData.Contains(key + ":f:"))
        {
            xmlData += key + ":f:" + first_value;
        }

        return PlayerPrefs.GetFloat(key, first_value);
    }
}