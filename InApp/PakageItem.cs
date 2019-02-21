using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class PakageItem : MonoBehaviour
{
    public GameObject ResultPanel;
    public Image ResultImage;
    public Text ResultText;
    
    public GameObject BackPanel;
    
    public GameObject NotificationPanel;
    public Text InfoText;

    public Text UseButtonText;
    
    private float[] plusGoldPerSec = {2f, 2f, 2f, 5f, 5f, 5f, 20f, 20f, 20f, 30f, 70f, 100f, 500f, 1500f, 4000f};

    public void PurchasePakageItem(int index)
    {
        if (PlayerPrefs.GetFloat("Town_" + (index + 1), 0) == 1)
        {
            // 타운을 2개 소유하고 있을 때, 타운 1개만 구매

            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index + 2];

            PlayerPrefs.SetFloat("Town_" + (index + 2), 1);

//            PlayerPrefs.SetFloat("TownIndex", index + 2);

            DataChangeEvent.Instance.PurchaseTown();
        }
        else if (PlayerPrefs.GetFloat("Town_" + index, 0) == 1)
        {
            // 타운을 1개 소유하고 있을 때, 타운 2개만 구매

            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index + 1];
            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index + 2];

            PlayerPrefs.SetFloat("Town_" + (index + 1), 1);
            PlayerPrefs.SetFloat("Town_" + (index + 2), 1);

//            PlayerPrefs.SetFloat("TownIndex", index + 2);

            DataChangeEvent.Instance.PurchaseTown();
            DataChangeEvent.Instance.PurchaseTown();
        }
        else
        {
            // 타운 3개 전부 구매
            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index];
            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index + 1];
            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index + 2];

            PlayerPrefs.SetFloat("Town_" + index, 1);
            PlayerPrefs.SetFloat("Town_" + (index + 1), 1);
            PlayerPrefs.SetFloat("Town_" + (index + 2), 1);

//            PlayerPrefs.SetFloat("TownIndex", index + 2);

            DataChangeEvent.Instance.PurchaseTown();
            DataChangeEvent.Instance.PurchaseTown();
            DataChangeEvent.Instance.PurchaseTown();

        }
        
        // TODO 랜덤박스 지급
        PlayerPrefs.SetFloat("RandomBoxCount", PlayerPrefs.GetFloat("RandomBoxCount", 0) + 5);

        ResultPanel.SetActive(true);
        BackPanel.SetActive(true);
        ResultImage.sprite = Resources.Load("RandomBox4", typeof(Sprite)) as Sprite;
        ResultText.text = "랜덤박스 5개 획득!!";
        
        UseButtonText.text = "랜덤박스\n사용\n(" + PlayerPrefs.GetFloat("RandomBoxCount", 0) + "개 보유)";
        
        DataChangeEvent.Instance.PurchasePakage();
    }
    
    public void PurchaseFail()
    {
        InfoText.text = "알 수 없는 오류로\n결제에 실패하였습니다";
        NotificationPanel.SetActive(true);
        BackPanel.SetActive(true);
    }
}