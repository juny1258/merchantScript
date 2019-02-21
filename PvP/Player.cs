using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Player : MonoBehaviour
{
    public Vector3 characterPosition;
    public Random random;
    public int randomInt;

//    private void OnEnable()
//    {
//        DataChangeEvent.PvpTabEvent += PvpTab;
//        random = new Random();
//        characterPosition = new Vector3(transform.position.x - Screen.width * 0.07f, transform.position.y + Screen.width * 0.1f,
//            transform.position.z);
//    }
//
//    public void PvpTab()
//    {
//        randomInt = random.Next(0, 101);
//        if (randomInt < DataController.Instance.criticalPercent)
//        {
//            // 크리티컬
//            if (!DataController.Instance.isFever)
//            {
//                CombatTextManager.Instance.CreateText(characterPosition,
//                    DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
//                                                       1.2f), randomInt);
//            }
//            else
//            {
//                CombatTextManager.Instance.CreateText(characterPosition,
//                    DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
//                                                       1.2f *
//                                                       DataController.Instance.feverGoldRising), randomInt);
//            }
//        }
//        else
//        {
//            // 노 크리티컬
//            if (!DataController.Instance.isFever)
//            {
//                CombatTextManager.Instance.CreateText(characterPosition,
//                    DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick), randomInt);
//            }
//            else
//            {
//                CombatTextManager.Instance.CreateText(characterPosition,
//                    DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
//                                                       DataController.Instance.feverGoldRising), randomInt);
//            }
//        }
//    }
}