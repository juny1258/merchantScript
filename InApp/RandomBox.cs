using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class RandomBox : MonoBehaviour
{
    public GameObject ResultPanel;
    public Image ResultImage;
    public Text ResultText;

    public GameObject BackPanel;

    public GameObject NotificationPanel;
    public Text InfoText;

    private Random rand;

    public Text UseButtonText;

    private string[] ItemName =
    {
        "허름한 상의", "꿀벌 머리띠", "꿀벌", "명품 페도라", "빛의 다이아몬드", "대장장이의 망치", "대장장이의 고글", "현자의 지팡이",
        "현자의 모자", "명품 선글라스", "훔쳐온 튜브", "휴지로 만든 모자", "보물지도", "전설의 투구", "전사의 창", "악마의 꼬리",
        "악마의 뿔", "악마의 삼지창", "악어 꼬리", "악어 모자", "야구공", "야구 헬멧", "야구 방망이", "신형 우주복",
        "산소", "좀비의 뼈", "좀비의 뇌", "천사의 날개", "천사의 링", "토끼 모자", "토끼 얼굴장식", "당근당근",
        "곰돌이 모자", "꿀 발라놓은 입", "선장의 모자", "선장의 칼", "최고급 망원경", "스타의 썬글라스", "스타의 게임기", "목욕탕 비누",
        "목욕 세트", "온천 간판"
    };

    private void OnEnable()
    {
        UpdateUI();

        rand = new Random();
    }

    private void UpdateUI()
    {
        UseButtonText.text = "랜덤박스\n사용\n(" + PlayerPrefs.GetFloat("RandomBoxCount", 0) + "개 보유)";
    }

    public void PurchaseBox()
    {
        // 랜덤박스 5개를 살 때

        PlayerPrefs.SetFloat("RandomBoxCount", PlayerPrefs.GetFloat("RandomBoxCount", 0) + 5);

        ResultPanel.SetActive(true);
        BackPanel.SetActive(true);
        ResultImage.sprite = Resources.Load("RandomBox4", typeof(Sprite)) as Sprite;
        ResultText.text = "랜덤박스 5개 획득!!";

        UpdateUI();
    }

    public void UseBox()
    {
        // 랜덤박스를 사용했을 때

        if (PlayerPrefs.GetFloat("RandomBoxCount", 0) != 0)
        {
            PlayerPrefs.SetFloat("RandomBoxCount", PlayerPrefs.GetFloat("RandomBoxCount", 0) - 1);

            var randInt = rand.Next(1, 43);

            if (PlayerPrefs.GetFloat("CollectionItem_" + randInt, 0) == 1)
            {
                randInt = rand.Next(1, 3);
                if (randInt == 1)
                {
                    randInt = rand.Next(50, 101);

                    ResultPanel.SetActive(true);
                    BackPanel.SetActive(true);
                    ResultImage.sprite = Resources.Load("Dia", typeof(Sprite)) as Sprite;
                    ResultText.text = "다이아 " + randInt + "개 획득!!";
                    DataController.Instance.dia += randInt;
                }
                else if (randInt == 2)
                {
                    randInt = rand.Next(1000, 1201);

                    ResultPanel.SetActive(true);
                    BackPanel.SetActive(true);
                    ResultImage.sprite = Resources.Load("Gold", typeof(Sprite)) as Sprite;
                    DataController.Instance.gold +=
                        DataController.Instance.goldPerSec * DataController.Instance.plusGoldPerSec * randInt;
                    ResultText.text = DataController.Instance.FormatGold(DataController.Instance.goldPerSec * 
                                                                         DataController.Instance.plusGoldPerSec * 
                                                                         DataController.Instance.skinGoldPerSec *
                                                                         DataController.Instance.reverseGolePerSec *
                                                                         randInt) +
                                      "G 획득!!";
                }

                // 중복됐을 때 다이아 50개 획득
            }
            else
            {
                // 중복이 아닐 때 수집품 획득
                PlayerPrefs.SetFloat("CollectionItem_" + randInt, 1);
                DataController.Instance.collectionGoldPerClick += 0.2f;
                ResultPanel.SetActive(true);
                BackPanel.SetActive(true);
                ResultImage.sprite = Resources.Load("CollectionItem/Profile" + randInt, typeof(Sprite)) as Sprite;
                ResultText.text = "수집품\n\'" + ItemName[randInt - 1] + "\' 획득!!";

                DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                             DataController.Instance.levelGoldPerClick *
                                                             DataController.Instance.skillGoldPerClick *
                                                             DataController.Instance.plusGoldPerClick *
                                                             DataController.Instance.collectionGoldPerClick *
                                                             DataController.Instance.reinforceGoldPerClick * 
                                                             DataController.Instance.skinGoldPerClick *
                                                             DataController.Instance.reverseGolePerClick;
            }
        }


        UpdateUI();
    }

    public void PurchaseFail()
    {
        InfoText.text = "알 수 없는 오류로\n결제에 실패하였습니다";
        NotificationPanel.SetActive(true);
        BackPanel.SetActive(true);
    }
}