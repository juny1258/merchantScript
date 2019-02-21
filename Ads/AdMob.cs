using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class AdMob : MonoBehaviour
{
    private static AdMob _instance;

    public static AdMob Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<AdMob>();

            return _instance;
        }
    }

    public Text TimeText;

    private IEnumerator _coroutine;

    public InterstitialAd ReinforceAd;
    public InterstitialAd TradeAd;
    public InterstitialAd PostAd;
    public RewardBasedVideoAd CompensationAd;
    public RewardBasedVideoAd PostRewardAd;

    public GameObject DiaAdsPanel;
    public GameObject BackPanel;

    private bool isAdsReady;


    private void Start()
    {
        MobileAds.Initialize("ca-app-pub-8345080599263513~6922431269");
        CompensationAd = RewardBasedVideoAd.Instance;
        PostRewardAd = RewardBasedVideoAd.Instance;

        RequestReinforceAd();
        RequestTradeAd();
        RequestPostAd();
        RequestCompensationAd();
        RequestPostRewardAd();

        _coroutine = ShowAds();
        
        InvokeRepeating("IsReady", 0, 300);

        DataChangeEvent.OpenMenuEvent += () =>
        {
            if (isAdsReady)
            {
                ShowReinforceAd();
                isAdsReady = false;
            }
        };
    }

    private void IsReady()
    {
        isAdsReady = true;
        print("이즈뤠디");
    }

    private void RequestPostRewardAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-8345080599263513/5876991912";
#elif UNITY_IOS
        adUnitId = ios_interstitialAdUnitId;
#endif

        AdRequest request = new AdRequest.Builder().Build();

        PostRewardAd.LoadAd(request, adUnitId);

        PostRewardAd.OnAdClosed += HandleOnPostRewardAdClosed;
        PostRewardAd.OnAdRewarded += HandleOnPostRewardAdReward;
    }

    private void RequestCompensationAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-8345080599263513/3757294338";
#elif UNITY_IOS
        adUnitId = ios_interstitialAdUnitId;
#endif

        AdRequest request = new AdRequest.Builder().Build();

        CompensationAd.LoadAd(request, adUnitId);

        CompensationAd.OnAdClosed += HandleOnCompensationAdAdClosed;
        CompensationAd.OnAdRewarded += HandleOnCompensationAdAdReward;
    }

    private void RequestReinforceAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-8345080599263513/7985560484";
#elif UNITY_IOS
        adUnitId = ios_interstitialAdUnitId;
#endif

        ReinforceAd = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();

        ReinforceAd.LoadAd(request);

        ReinforceAd.OnAdClosed += HandleOnReinforceAdClosed;
    }

    private void RequestTradeAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-8345080599263513/2752774471";
#elif UNITY_IOS
        adUnitId = ios_interstitialAdUnitId;
#endif

        TradeAd = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();

        TradeAd.LoadAd(request);

        TradeAd.OnAdClosed += HandleOnTradeAdClosed;
    }

    private void RequestPostAd()
    {
        string adUnitId = string.Empty;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-8345080599263513/5484539123";
#elif UNITY_IOS
        adUnitId = ios_interstitialAdUnitId;
#endif

        PostAd = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();

        PostAd.LoadAd(request);

        PostAd.OnAdClosed += HandleOnPostAdClosed;
    }

    private void HandleOnPostRewardAdReward(object sender, EventArgs args)
    {
        // 보상
        DataController.Instance.dia += 30;
    }

    private void HandleOnCompensationAdAdReward(object sender, EventArgs args)
    {
        DataController.Instance.gold += DataController.Instance.compensationGold;
    }

    private void HandleOnPostRewardAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        PostRewardAd.OnAdClosed -= HandleOnPostRewardAdClosed;
        PostRewardAd.OnAdRewarded -= HandleOnPostRewardAdReward;

        RequestPostRewardAd();
    }

    private void HandleOnCompensationAdAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        CompensationAd.OnAdClosed -= HandleOnCompensationAdAdClosed;
        CompensationAd.OnAdRewarded -= HandleOnCompensationAdAdReward;

        RequestCompensationAd();
    }

    private void HandleOnReinforceAdClosed(object sender, EventArgs args)
    {
        print("HandleOnInterstitialAdClosed event received.");

        ReinforceAd.Destroy();

        RequestReinforceAd();
    }

    private void HandleOnTradeAdClosed(object sender, EventArgs args)
    {
        print("HandleOnTradeAdClosed event received.");

        TradeAd.Destroy();

        RequestTradeAd();
    }

    private void HandleOnPostAdClosed(object sender, EventArgs args)
    {
        print("HandleOnTradeAdClosed event received.");

        PostAd.Destroy();

        RequestPostAd();
    }

    public void ShowReinforceAd()
    {
        if (PlayerPrefs.GetFloat("NoAds", 0) == 0)
        {
            if (!ReinforceAd.IsLoaded())
            {
                RequestReinforceAd();
                return;
            }

            ReinforceAd.Show();
        }
    }

    public void ShowPostRewardAd()
    {
        DiaAdsPanel.SetActive(false);
        BackPanel.SetActive(false);
        PostRewardAd.Show();
    }

    public void ShowCompensationAd()
    {
        StartCoroutine(_coroutine);
    }

    private IEnumerator ShowAds()
    {
        int i = 0;
        while (true)
        {
            if (CompensationAd.IsLoaded())
            {
                CompensationAd.Show();
                StopCoroutine(_coroutine);
            }

            if (i > 20)
            {
                StopCoroutine(_coroutine);
            }

            i++;
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ShowTradeAd()
    {
        if (DataController.Instance.bangchi == 1)
        {
            // 방치모드일 때
        }
        else if (PlayerPrefs.GetFloat("NoAds", 0) == 0)
        {
            if (!TradeAd.IsLoaded())
            {
                RequestTradeAd();
                return;
            }

            TradeAd.Show();
        }
    }

    public void ShowPostAd()
    {
        if (PlayerPrefs.GetFloat("NoAds", 0) == 0)
        {
            if (!PostAd.IsLoaded())
            {
                RequestPostAd();
                return;
            }

            PostAd.Show();
        }
    }
}