using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReversePanel : MonoBehaviour
{
    public GameObject BackPanel;
    public GameObject ReverseMenu;

    public GameObject LevelUpPanel;

    public Text TitleText;

    public Image TownImage;
    public Text TownText;
    public Text ReverseLevel;
    public Text Infomation;
    public Button ReverseButton;
    public GameObject LockPanel;
    public Text LockInfoText;

    public GameObject OnMenu1;
    public GameObject InfomationPanel;
    public Text InfomationText;

    private int index;

    private int[] levelArray =
    {
        20, 25, 30, 35, 40, 45, 50
    };

    private float[] reverseGolePerClickSec =
    {
        5, 10, 20, 30, 40, 50 
    };

    private int[] reversePoint =
    {
        10000, 13000, 16000, 19000, 21000, 23000
    };

    private string[] names =
    {
        "엔 에이치 타운", "샌딜", "볼케이브", "콜로세움", "블루 홀", "유니티야드", "데드 존", "루인스",
        "드래곤 빌리지", "페어리어", "엠 피 에리어", "이빌 타운", "릴렌틀리스", "사이버 타운"
    };

    private void OnEnable()
    {
        index = (int) DataController.Instance.reverseLevel;

        if (index == 0)
        {
            TitleText.text = "TAP + 0배, SEC + 0배";
        }
        else
        {
            TitleText.text = "TAP + " + reverseGolePerClickSec[index - 1] + "배, SEC + " +
                             reverseGolePerClickSec[index - 1] +
                             "배";
        }


        TownImage.sprite = Resources.Load("Town/Profile" + (index + 1), typeof(Sprite)) as Sprite;
        TownText.text = names[index];
        ReverseLevel.text = "(환생 " + (index + 1) + "단계)";
        Infomation.text = "영구 클릭당, 초당 골드 x " + reverseGolePerClickSec[index] + "배\n" +
                          "크리티컬 확률 : " + DataController.Instance.criticalPercent + "% -> " + 
                          (DataController.Instance.criticalPercent + 5) + "%\n" +
                          "크리티컬 골드 상승률 : " + (DataController.Instance.criticalRising * 100) + "% -> " +
                          + (DataController.Instance.criticalRising + 0.3f)*100 + "%\n" +
                          "수집품 획득 확률 : " + DataController.Instance.plusCollectionDrop/2 + "% -> " +
                          + (DataController.Instance.plusCollectionDrop + 1f)/2 + "%\n" +
                          "환생 포인트 + " + reversePoint[index] + "( 초과 시 1레벨당 + 2000 )\n" +
                          "레벨, 골드, 코스튬, 생산품, 마을, 수집품, 반지 초기화";
        ReverseButton.onClick.RemoveAllListeners();
        ReverseButton.onClick.AddListener(NextTown);

        if (DataController.Instance.reverseLevel <= 4)
        {
            LockInfoText.text = "반지 강화\nLv. " + levelArray[index] + " 필요";
            LockPanel.SetActive(PlayerPrefs.GetFloat("ReinforceLevel", 0) < levelArray[index]);
        }
        else
        {
            LockPanel.SetActive(true);
            LockInfoText.text = "준비중 입니다.";
        }
    }

    public void NextTown()
    {
        // 환생
        if (!DataController.Instance.isTrading)
        {
            ReverseButton.onClick.RemoveAllListeners();
            // 클릭당 골드, 초당 골드 증가
            DataController.Instance.reverseGolePerClick = reverseGolePerClickSec[index];
            DataController.Instance.reverseGolePerSec = reverseGolePerClickSec[index];

            var plusPoint = (PlayerPrefs.GetFloat("ReinforceLevel", 0) - levelArray[index]) * 2000;

            // 환생포인트 증가
            DataController.Instance.reversePoint += reversePoint[index] + plusPoint;

            // 환생 레벨 증가
            DataController.Instance.reverseLevel += 1;
            
            // 수집품 획득 확률 증가
            DataController.Instance.plusCollectionDrop += 1f;
            
            // 크리티컬 확률, 상승률 증가
            DataController.Instance.criticalPercent += 5;
            DataController.Instance.criticalRising += 0.3f;

            DataController.Instance.criticalPer = DataController.Instance.criticalPercent;

            for (var i = 0; i <= DataController.Instance.reverseLevel; i++)
            {
                PlayerPrefs.SetFloat("Reverse_" + i, 1);
            }

            PlayerPrefs.SetFloat("WearingTown", DataController.Instance.reverseLevel);

            // 데이터 초기화
            DataController.Instance.ResetData();


            // UIManager, BackgroundManager
            DataChangeEvent.Instance.ResetData();
            LevelUpPanel.SetActive(true);

            BackPanel.SetActive(false);
            ReverseMenu.SetActive(false);
        }
        else
        {
            OnMenu1.SetActive(true);
            InfomationPanel.SetActive(true);
            InfomationText.text = "무역중에는 환생을\n할 수 없습니다.";
        }
    }
}