using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;

public class Balance : MonoBehaviour
{
    public GameObject BackPanel;

    private float ridingPerClick = 1000;
    private float backPerClick = 3000;

    private float[] plusGoldPerClick = {
        2f, 3f, 5f, 10f, 30f,
        50, 100, 200, 400, 1200,
        3000, 5000f, 50000, 200000f,
        500000f
    };

    private float[] goldRisingPercent =
    {
        0f, 1f, 3f, 7f, 20f, 50f, 150f, 300f, 1800f, 3000f,
        10000f, 30000, 82000f, 230000, 650000f, 1800000f, 5400000f, 15000000f, 50000000f,
        200000000f
    };

    private float[] ReinforceAutoClick =
    {
        5,
        2.0f, 1.9f, 1.8f, 1.7f, 1.6f,
        1.5f, 1.4f, 1.3f, 1.2f, 1.1f,
        1.0f, 0.95f, 0.9f, 0.85f, 0.8f,
        0.75f, 0.70f, 0.65f, 0.6f, 0.55f,
        0.5f, 0.45f, 0.4f, 0.35f, 0.3f,
        0.25f, 0.2f, 0.18f, 0.16f, 0.14f,
        0.12f, 0.10f, 0.098f, 0.096f, 0.095f,
        0.094f, 0.093f, 0.092f, 0.091f, 0.09f,
        0.089f, 0.088f, 0.087f, 0.086f, 0.085f,
        0.084f, 0.083f, 0.082f, 0.081f, 0.08f
    };

    private void OnEnable()
    {
    }

    // Use this for initialization
    void Start()
    {
        DataController.Instance.reinforceAutoTime = ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
        StartCoroutine(Balanced());
    }

    private float[] ReinforceGoldPerClick =
    {
        0,
        0.2f, 0.2f, 0.2f, 0.2f, 0.2f,
        0.5f, 0.5f, 1f, 1f, 1,
        5, 6, 7, 8, 9,
        10, 20, 30, 40, 50,
        60, 70, 80, 90, 100,
        300, 500, 1000, 2500, 8000,
        10000, 15000, 20000, 25000, 30000,
        50000, 60000, 70000, 80000, 90000,
        100000, 150000, 200000, 250000, 300000,
        400000, 500000, 600000, 800000, 1000000
    };

    private float[] plusGoldPerSec = {2f, 2f, 2f, 5f, 5f, 5f, 20f, 20f, 20f, 30f, 70f, 100f, 500f, 1500f, 4000f};

    private float[] goldRising =
    {
        1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f,
        3.2f, 3.4f, 3.6f, 3.8f, 4f, 4.3f, 4.6f, 4.8f, 5.2f, 5.5f
    };

    private float[] reverseGolePerClickSec =
    {
        5, 10, 20, 30, 40, 50
    };

    private IEnumerator Balanced()
    {
        var firstOpen = PlayerPrefs.GetFloat("FirstOpen", 0);

        if (firstOpen == 0)
        {
            //처음 오픈
        }
        else
        {
            BackPanel.SetActive(true);

            DataController.Instance.collectionGoldPerClick = 1;

            for (int i = 0; i < 50; i++)
            {
                if (PlayerPrefs.GetFloat("CollectionItem_" + i, 0) == 1)
                {
                    DataController.Instance.collectionGoldPerClick += 0.2f;
                }
            }

            if (DataController.Instance.reverseLevel > 0)
            {
                DataController.Instance.reverseGolePerClick =
                    reverseGolePerClickSec[(int) DataController.Instance.reverseLevel - 1];
                DataController.Instance.reverseGolePerSec =
                    reverseGolePerClickSec[(int) DataController.Instance.reverseLevel - 1];
            }

            DataController.Instance.criticalPercent = 0;
            DataController.Instance.criticalRising = 1;
            DataController.Instance.plusCollectionDrop = 1;

            for (int i = 0; i < DataController.Instance.reverseLevel; i++)
            {
                DataController.Instance.criticalPercent += 5;
                DataController.Instance.criticalRising += 0.3f;
                DataController.Instance.plusCollectionDrop += 1;
            }

            DataController.Instance.skinGoldPerClick = 1;
            DataController.Instance.skinGoldPerSec = 1;

            for (int i = 1; i <= 5; i++)
            {
                if (i <= 4)
                {
                    if (PlayerPrefs.GetFloat("Skin_" + i, 0) == 1)
                    {
                        DataController.Instance.skinGoldPerClick += 5;
                        DataController.Instance.skinGoldPerSec += 5;
                    }   
                }
                else
                {
                    if (PlayerPrefs.GetFloat("Skin_" + i, 0) == 1)
                    {
                        DataController.Instance.criticalPercent += 20;
                        DataController.Instance.criticalRising += 2;
                    }   
                }
            }

            for (int i = 0; i < PetMenu.LENGTH; i++)
            {
                if (PlayerPrefs.GetFloat("Pet_" + i, 0) >= 1)
                {
                    DataController.Instance.feverGoldRising = goldRising[i];
                }
            }

            // 레벨당 골드
            DataController.Instance.goldPerClick = 1;

            for (int i = 2; i <= DataController.Instance.level; i++)
            {
                DataController.Instance.goldPerClick += Mathf.Pow(1.008f, i);
                print(DataController.Instance.goldPerClick);
            }

            DataController.Instance.reinforceGoldPerClick = 1;

            for (int i = 0; i <= PlayerPrefs.GetFloat("ReinforceLevel", 0); i++)
            {
                DataController.Instance.reinforceGoldPerClick += ReinforceGoldPerClick[i];
            }


            //코스튬
            DataController.Instance.plusGoldPerClick = 1;

            for (int i = 1; i < CostumeMenu.LENGTH; i++)
            {
                if (PlayerPrefs.GetFloat("Costume_" + i, 0) >= 1)
                {
                    DataController.Instance.plusGoldPerClick += goldRisingPercent[i];
                }
            }

            // 업그레이드 골드 증가량
            DataController.Instance.levelGoldPerClick = 1;

            for (int i = 0; i < (int) (DataController.Instance.level / 100); i++)
            {
                if (i < 15)
                {
                    DataController.Instance.levelGoldPerClick += plusGoldPerClick[i];
                }
            }

            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick *
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;

            DataController.Instance.goldPerSec = 0;

            for (int i = 0; i < RidingMenu.LENGTH; i++)
            {
                if (PlayerPrefs.GetFloat("Riding_" + i, 0) >= 1)
                {
                    DataController.Instance.goldPerSec += ridingPerClick * Mathf.Pow(6.2f, i);
                }
            }

            for (int i = 0; i < BackgroundMenu.LENGTH; i++)
            {
                if (PlayerPrefs.GetFloat("Background_" + i, 0) >= 1)
                {
                    DataController.Instance.goldPerSec += backPerClick * Mathf.Pow(4.5f, i);
                }
            }

            DataController.Instance.plusGoldPerSec = 1;

            for (int i = 0; i < TownMenu.LENGTH; i++)
            {
                if (PlayerPrefs.GetFloat("Town_" + i, 0) >= 1)
                {
                    DataController.Instance.plusGoldPerSec += plusGoldPerSec[i];
                }
            }

            BackPanel.SetActive(false);
        }

        yield return null;
    }
}