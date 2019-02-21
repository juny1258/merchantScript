using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class InApp : MonoBehaviour
{
    public GameObject NotificationPanel;
    public Text Information;
    public GameObject BackPanel;

    private int[] addDia = {100, 350, 600, 1200, 2500, 4000};
    private int[] addGold = {1, 4, 8, 15, 30, 50};

    public void PurchaseDiaItem(int index)
    {
        DataController.Instance.dia += addDia[index];
    }

    public void PurchaseGoldItem(int index)
    {
        DataController.Instance.gold += (DataController.Instance.goldPerSec *
                                         DataController.Instance.plusGoldPerSec *
                                         DataController.Instance.skinGoldPerSec *
                                         DataController.Instance.reverseGolePerSec
                                         * 3600 * addGold[index])
                                        + (DataController.Instance.masterGoldPerClick * 5 * 3600 * addGold[index]);
    }

    public void PurchaseFail(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Information.text = purchaseFailureReason.ToString();
        NotificationPanel.SetActive(true);
        BackPanel.SetActive(true);
    }
}