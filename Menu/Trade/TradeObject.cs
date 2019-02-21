using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeObject : MonoBehaviour
{
    public Image TownImage;
    public Text Name;
    public Text Info;
    public Button TradeStartButton;
    public Text TradeButtonText;
    public Slider TradeSlider;
    public Text SliderText;
    
    public GameObject DiaPanel;

    public Button NextTownButton;
    
    private string[] townName = {
        "루어럴", "엔 에이치 타운", "샌딜", "볼케이브", "콜로세움", "블루 홀", "유니티야드", "데드 존", "루인스",
        "드래곤 빌리지", "페어리어", "엠 피 에리어", "이빌 타운", "릴렌틀리스", "사이버 타운"
    };

    public GameObject BackPanel;
    public GameObject Menu;
    public GameObject OnMenu1;

    private void OnEnable()
    {
        TownImage.sprite =
            Resources.Load("Town/Profile" + PlayerPrefs.GetFloat("TradeLevel", 0), typeof(Sprite)) as Sprite;
        Name.text = townName[(int) PlayerPrefs.GetFloat("TradeLevel", 0)];
        Info.text = "예상 수익\n: " + DataController.Instance.FormatGold(
                        DataController.Instance.goldPerSec
                        * DataController.Instance.plusGoldPerSec
                        * DataController.Instance.skinGoldPerSec
                        * DataController.Instance.reverseGolePerSec
                        * DataController.Instance.tradeRevenue[(int) PlayerPrefs.GetFloat("TradeLevel", 0)]
                    ) + "G";
        
        if (DataController.Instance.isTrading)
        {
            TradeButtonText.text = "무역끝내기";
            DiaPanel.SetActive(true);
            TradeStartButton.onClick.AddListener(() => UseDiaForCompelete((int) PlayerPrefs.GetFloat("TradeLevel", 0)));
        }
        else
        {
            TradeButtonText.text = "무역하기";
            DiaPanel.SetActive(false);
            TradeStartButton.onClick.AddListener(() => StartTrade((int) PlayerPrefs.GetFloat("TradeLevel", 0)));
        }
        // TODO 무역이 끝났을 때 리스너
        DataChangeEvent.TradeCompeleteEvent += index =>
        {
            TradeButtonText.text = "무역하기";
            DiaPanel.SetActive(false);
            TradeStartButton.onClick.RemoveAllListeners();
            TradeStartButton.onClick.AddListener(() => StartTrade((int) PlayerPrefs.GetFloat("TradeLevel", 0)));
            TradeSlider.value = 0;
        };
    }
    private void Start()
    {

        NextTownButton.onClick.AddListener(NextTown);
    }

    private void Update()
    {
        // TODO 무역의 달성률을 가져와 Slider에 표시
        SliderText.text = TimeFormat(
            DataController.Instance.tradeCompleteTime -
            PlayerPrefs.GetFloat("TradeTime_" + (int) PlayerPrefs.GetFloat("TradeLevel", 0), 0) + 0.999f);

        TradeSlider.value =
            100 / DataController.Instance.tradeCompleteTime *
            PlayerPrefs.GetFloat("TradeTime_" + (int) PlayerPrefs.GetFloat("TradeLevel", 0), 0);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OnMenu1.active)
            {
                Menu.SetActive(false);
                BackPanel.SetActive(false);
            }
        }
    }

    private void StartTrade(int tradeLevel)
    {
        if (!DataController.Instance.isTrading)
        {
            DataChangeEvent.Instance.StartTrade(tradeLevel);

            DataController.Instance.isTrading = true;
            TradeStartButton.onClick.RemoveAllListeners();
            TradeButtonText.text = "무역끝내기";
            DiaPanel.SetActive(true);
            TradeStartButton.onClick.AddListener(() => UseDiaForCompelete((int) PlayerPrefs.GetFloat("TradeLevel", 0)));
        }
    }

    // 다이아 사용, 무역 완료
    private void UseDiaForCompelete(int tradeLevel)
    {
        if (DataController.Instance.isTrading && DataController.Instance.dia >= 20)
        {
            TradeStartButton.onClick.RemoveAllListeners();
            DataController.Instance.dia -= 20;
            PlayerPrefs.SetFloat("TradeTime_" + tradeLevel, DataController.Instance.tradeCompleteTime);
            TradeButtonText.text = "무역하기";
            DiaPanel.SetActive(false);
            TradeStartButton.onClick.AddListener(() => StartTrade((int)PlayerPrefs.GetFloat("TradeLevel", 0)));
        }
    }

    // 다음 마을을 구매하면 무역지를 다음 마을로 이동
    private void NextTown()
    {
        if (PlayerPrefs.GetFloat("Town_" + ((int)PlayerPrefs.GetFloat("TradeLevel", 0) + 1), 0) == 1
            && !DataController.Instance.isTrading)
        {
            PlayerPrefs.SetFloat("TradeLevel", PlayerPrefs.GetFloat("TradeLevel", 0) + 1);

            TownImage.sprite =
                Resources.Load("Town/Profile" + PlayerPrefs.GetFloat("TradeLevel", 0), typeof(Sprite)) as Sprite;
            Name.text = townName[(int) PlayerPrefs.GetFloat("TradeLevel", 0)];
            Info.text = "예상 수익\n: " + DataController.Instance.FormatGold(
                            DataController.Instance.goldPerSec
                            * DataController.Instance.plusGoldPerSec 
                            * DataController.Instance.skinGoldPerSec
                            * DataController.Instance.reverseGolePerSec
                            * DataController.Instance.tradeRevenue[(int) PlayerPrefs.GetFloat("TradeLevel", 0)]
                        ) + "G";

            DataChangeEvent.Instance.TownChange();

            TradeButtonText.text = "무역하기";
            TradeStartButton.onClick.RemoveAllListeners();
            TradeStartButton.onClick.AddListener(() => StartTrade((int) PlayerPrefs.GetFloat("TradeLevel", 0)));
        }

//        PlayerPrefs.SetFloat("TradeLevel", PlayerPrefs.GetFloat("TradeLevel", 0) + 1);
    }

    private string TimeFormat(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);

        var timeText = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        return timeText;
    }
}