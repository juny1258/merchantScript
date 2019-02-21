using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeComplete : MonoBehaviour
{
    public RectTransform CompletePanel;
    public RectTransform LevelUpPanel;
    public RectTransform BackPanel;

    public void CloseButton()
    {
        CompletePanel.gameObject.SetActive(false);
        BackPanel.gameObject.SetActive(false);
    }

    public void CloseLevelUpPanel()
    {
        LevelUpPanel.gameObject.SetActive(false);
        BackPanel.gameObject.SetActive(false);
    }

    public void CompensationReward()
    {
        LevelUpPanel.gameObject.SetActive(false);
        BackPanel.gameObject.SetActive(false);
        AdMob.Instance.ShowCompensationAd();
    }
}