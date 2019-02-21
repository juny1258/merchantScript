using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsButton : MonoBehaviour
{
    public GameObject DiaAdsPanel;
    public GameObject BackPanel;
    public GameObject NotificationPanel;


    public void ShowRewardedAd()
    {
        if (PlayerPrefs.GetFloat("NoAds", 0) == 0)
        {
            // 광고 제거 전
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions {resultCallback = HandleShowResult};
                Advertisement.Show("rewardedVideo", options);   
            }

            PostManager.Instance.StartTimer();
        }
        else
        {
            // 광고 제거 후
            DataController.Instance.dia += 30;

            DiaAdsPanel.SetActive(false);
            BackPanel.SetActive(false);

            PostManager.Instance.StartTimer();
        }
    }

    public void FreeDiaButton()
    {
        DataController.Instance.dia += 3;

        DiaAdsPanel.SetActive(false);
        BackPanel.SetActive(false);

        PostManager.Instance.StartTimer();
    }

    public void NotificationButtonClick()
    {
        NotificationPanel.SetActive(false);
        DiaAdsPanel.SetActive(true);
        BackPanel.SetActive(true);

        PostManager.Instance.RemoveNotification();
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                AdMob.Instance.ShowPostAd();
                DataController.Instance.dia += 30;

                DiaAdsPanel.SetActive(false);
                BackPanel.SetActive(false);

                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}