using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class TabScreen : MonoBehaviour
{
    private Random rand;
    private int randInt;

    private void OnEnable()
    {
        rand = new Random();
    }

    private void Start()
    {
        DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                     DataController.Instance.levelGoldPerClick *
                                                     DataController.Instance.skillGoldPerClick *
                                                     DataController.Instance.plusGoldPerClick *
                                                     DataController.Instance.collectionGoldPerClick *
                                                     DataController.Instance.reinforceGoldPerClick *
                                                     DataController.Instance.skinGoldPerClick *
                                                     DataController.Instance.reverseGolePerClick;

        DataController.Instance.criticalPer = DataController.Instance.criticalPercent;
    }

    public void OnClick()
    {
        randInt = rand.Next(0, 100);

        if (randInt < DataController.Instance.criticalPer)
        {
            // 크리티컬
            if (!DataController.Instance.isFever)
            {
                DataController.Instance.gold += DataController.Instance.masterGoldPerClick *
                    DataController.Instance.criticalRising;
            }
            else
            {
                DataController.Instance.gold += DataController.Instance.masterGoldPerClick *
                                                DataController.Instance.feverGoldRising*
                                                DataController.Instance.criticalRising;
            }
        }
        else
        {
            // 노크리티컬
            if (!DataController.Instance.isFever)
            {
                DataController.Instance.gold += DataController.Instance.masterGoldPerClick;
            }
            else
            {
                DataController.Instance.gold += DataController.Instance.masterGoldPerClick *
                                                DataController.Instance.feverGoldRising;
            }
        }

        DataChangeEvent.Instance.Tab(randInt);
    }
}