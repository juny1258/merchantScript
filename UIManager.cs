using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class UIManager : MonoBehaviour
{
    public RectTransform panel;

    public Image BackgroundImage;
    public Image StreetLamp;

    private float firstOpen;

    // 상단 UI
    public Text goldDisplayer;
    public Text diaDisplayer;

    // 하단 UI
    public Text tabDisplayer;
    public Text secDisplayer;
    public Image PurchaseImage;

    public Image ProfileImage;

    // 메뉴 UI
    public Text costumeText;
    public Text ridingText;
    public Text petText;
    public Text tradeText;
    public Text productText;
    public Text townText;
    public Text collectionText;
    public Text skinText;
    public Text reverseText;

    // 무역 성공 UI
    public Text CompleteText;
    public RectTransform CompletePanel;
    public RectTransform BackPanel;
    public GameObject OnMenu;

    public Slider TradeSlider;

    private string gold;

    public RectTransform Chat;
    public Text ChatText;

    private int costumeIndex;
    private float costumePrice;
    public RectTransform newCostume;

    private int ridingIndex;
    private float ridingPrice;
    public RectTransform newRiding;

    private int petIndex;
    private float petPrice;
    public RectTransform newPet;

    private int townIndex;
    private float townPrice;
    public RectTransform newTown;

    private int backgroundIndex;
    private float backgroundPrice;
    public RectTransform newbackground;

    public GameObject FireworkPanel;

    public Image SkillView;

    public GameObject BangchiPanel;

    public GameObject PakagePanel;

    private void Awake()
    {
    }

    private string[] chatString =
    {
        "오늘 점심은\n뭘 먹지?", "엄마 보고싶다...", "+9강 부러진 \n나무 검 팔아요!", "오늘도 화이팅!", "난 부자가\n될거야!",
        "여기 맛집이\n어디죠?", "노력한다면 뭐든지\n해낼 수 있어!", "난 너무 멋있어.", "지금 이 순간이\n가장 소중한거야.", "오늘도 손님이\n별로 없네...",
        "멋진 코스튬을\n입혀줘!!", "펫을 사면 하트를\n얻을 수 있어!", "여기 좀 봐주세요!!", "나랑 이야기할래?", "수집품을 모아\n능력을 올려봐!",
        "생산품을 구매해서\n마을을 꾸며봐!", "좀 더 열심히\n클릭해봐!!", "마을을 인수해서\n무역을 시작해봐!!", "마을을 인수해서\n무역을 시작해봐!!",
        "힘들 땐 주위를\n한 번 둘러봐", "오늘 하루는\n어땠어?", "좋은 하루\n보내고 있지?", "인생은 즐거워!", "난 사기는 안 쳐!",
        "난 돈이 좋아ㅎㅎ", "+3강 철갑옷\n급처해요!", "마감세일중!!", "도레미파솔라시도~", "블루 홀에는\n거북이가 산대", "루어럴은 살기\n좋은 마을이야!",
        "우주는 어떤 곳일까?", "난 귀신은 안 믿어", "1더하기 1은?\n귀요미ㅎㅎ", "시간 남으면 리뷰\n한 개 쓰고가!!", "넌 그 모습 그대로\n멋있는 사람이야",
        "요즘 다이어트\n하는중이야", "배고프다...", "고양이 키우고싶어", "폭죽을 사면 예쁜\n야경을 볼 수 있어!", "우리 집에서...\n라면 먹고갈래..?",
        "공부하기 싫다..."
    };

    private string[] townNames =
    {
        "루어럴", "엔 에이치 타운", "샌딜", "볼케이브", "콜로세움", "블루 홀", "유니티야드", "데드 존", "루인스",
        "드래곤 빌리지", "페어리어", "엠 피 에리어", "이빌 타운", "릴렌틀리스", "사이버 타운"
    };

    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50
    };

    public GameObject ReviewPanel;

    public GameObject Camera;

    private void OnEnable()
    {
        // 방치 패널
        if (DataController.Instance.bangchi == 0)
        {
            BangchiPanel.SetActive(false);
        }
        else
        {
            BangchiPanel.SetActive(true);
        }

        firstOpen = PlayerPrefs.GetFloat("FirstOpen", 0);
        if (firstOpen == 0)
        {
            panel.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("FirstOpen", 1);
            PlayerPrefs.SetFloat("FirstReview", 1);
        }
        else
        {
            panel.gameObject.SetActive(false);
            if (DataController.Instance.level < 235)
            {
                PlayerPrefs.SetFloat("FirstReview", 1);
            }
            else if (PlayerPrefs.GetFloat("FirstReview", 0) == 0)
            {
                ReviewPanel.SetActive(true);
                BackPanel.gameObject.SetActive(true);
                PlayerPrefs.SetFloat("FirstReview", 1);
            }
        }

        if (PlayerPrefs.GetFloat("SoundSetting", 0) == 0)
        {
            Camera.GetComponent<AudioSource>().volume = 1;
        }
        else if (PlayerPrefs.GetFloat("SoundSetting", 0) == 1)
        {
            Camera.GetComponent<AudioSource>().volume = 0;
        }

        DataChangeEvent.OpenMenuEvent += SetTitle;

        DataChangeEvent.CostumeChange += CostumeChanged;

        SetNotification();

        DataChangeEvent.TradeCompeleteEvent += completeGold =>
        {
            CompletePanel.gameObject.SetActive(true);
            BackPanel.gameObject.SetActive(true);
            CompleteText.text = DataController.Instance.FormatGold(completeGold) + "G 획득!!";
            AdMob.Instance.ShowTradeAd();
        };

        DataChangeEvent.PurchaseCostumeEvent += () =>
        {
            costumeIndex++;
            if (costumeIndex < CostumeMenu.LENGTH)
            {
                costumePrice =
                    DataController.Instance.PurchaseGold(CostumeMenu.price * Mathf.Pow(12.5f, costumeIndex - 1)
                                                                           * reverseRisingPrice[
                                                                               (int) DataController.Instance
                                                                                   .reverseLevel]);
            }

            SetTitle();
        };

        DataChangeEvent.PurchaseRidingEvent += () =>
        {
            ridingIndex++;
            if (ridingIndex < RidingMenu.LENGTH)
            {
                ridingPrice = DataController.Instance.PurchaseGold(RidingMenu.price * Mathf.Pow(12.5f, ridingIndex)
                                                                                    * reverseRisingPrice[
                                                                                        (int) DataController.Instance
                                                                                            .reverseLevel]);
            }

            SetTitle();
        };

        DataChangeEvent.PurchasePetEvent1 += () =>
        {
            petIndex++;
            if (petIndex < PetMenu.LENGTH)
            {
                petPrice = DataController.Instance.PurchaseGold(PetMenu.price * Mathf.Pow(12.5f, petIndex)
                                                                              * reverseRisingPrice[
                                                                                  (int) DataController.Instance
                                                                                      .reverseLevel]);
            }

            SetTitle();
        };

        DataChangeEvent.PurchaseTownEvent += () =>
        {
            townIndex++;
            if (townIndex < TownMenu.LENGTH)
            {
                townPrice = DataController.Instance.PurchaseGold(5000900000000 * Mathf.Pow(6.1f, townIndex)
                                                                               * reverseRisingPrice[
                                                                                   (int) DataController.Instance
                                                                                       .reverseLevel]);
            }

            SetTitle();
        };

        DataChangeEvent.PurchaseBackgroundEvent += () =>
        {
            backgroundIndex++;
            if (backgroundIndex < BackgroundMenu.LENGTH)
            {
                backgroundPrice =
                    DataController.Instance.PurchaseGold(BackgroundMenu.price * Mathf.Pow(8.5f, backgroundIndex)
                                                                              * reverseRisingPrice[
                                                                                  (int) DataController.Instance
                                                                                      .reverseLevel]);
            }

            SetTitle();
        };

        DataChangeEvent.ResetDataEvent += SetNotification;

        StartCoroutine(StartChat());

        StartCoroutine(TimeCheck());
    }

    private void SetNotification()
    {
        for (costumeIndex = 1; costumeIndex < CostumeMenu.LENGTH; costumeIndex++)
        {
            if (PlayerPrefs.GetFloat("Costume_" + costumeIndex, 0) == 0)
            {
                costumePrice =
                    DataController.Instance.PurchaseGold(CostumeMenu.price * Mathf.Pow(12.5f, costumeIndex - 1)
                                                                           * reverseRisingPrice[
                                                                               (int) DataController.Instance
                                                                                   .reverseLevel]);
                break;
            }
        }

        for (ridingIndex = 0; ridingIndex < RidingMenu.LENGTH; ridingIndex++)
        {
            if (PlayerPrefs.GetFloat("Riding_" + ridingIndex, 0) == 0)
            {
                ridingPrice = DataController.Instance.PurchaseGold(RidingMenu.price * Mathf.Pow(12.5f, ridingIndex)
                                                                                    * reverseRisingPrice[
                                                                                        (int) DataController.Instance
                                                                                            .reverseLevel]);
                break;
            }
        }

        for (petIndex = 0; petIndex < PetMenu.LENGTH; petIndex++)
        {
            if (PlayerPrefs.GetFloat("Pet_" + petIndex, 0) == 0)
            {
                petPrice = DataController.Instance.PurchaseGold(PetMenu.price * Mathf.Pow(12.5f, petIndex)
                                                                              * reverseRisingPrice[
                                                                                  (int) DataController.Instance
                                                                                      .reverseLevel]);
                break;
            }
        }

        for (townIndex = 0; townIndex < TownMenu.LENGTH; townIndex++)
        {
            if (PlayerPrefs.GetFloat("Town_" + townIndex, 0) == 0)
            {
                townPrice = DataController.Instance.PurchaseGold(5000900000000 * Mathf.Pow(6.1f, townIndex)
                                                                               * reverseRisingPrice[
                                                                                   (int) DataController.Instance
                                                                                       .reverseLevel]);
                break;
            }
        }

        for (backgroundIndex = 0; backgroundIndex < BackgroundMenu.LENGTH; backgroundIndex++)
        {
            if (PlayerPrefs.GetFloat("Background_" + backgroundIndex, 0) == 0)
            {
                backgroundPrice =
                    DataController.Instance.PurchaseGold(BackgroundMenu.price * Mathf.Pow(8.5f, backgroundIndex)
                                                                              * reverseRisingPrice[
                                                                                  (int) DataController.Instance
                                                                                      .reverseLevel]);
                break;
            }
        }
    }

    private IEnumerator TimeCheck()
    {
        while (true)
        {
            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
            {
                BackgroundImage.sprite = Resources.Load("Background1", typeof(Sprite)) as Sprite;
                FireworkPanel.SetActive(false);
            }
            else if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 19)
            {
                BackgroundImage.sprite = Resources.Load("Background2", typeof(Sprite)) as Sprite;
                FireworkPanel.SetActive(false);
            }
            else
            {
                BackgroundImage.sprite = Resources.Load("Background3", typeof(Sprite)) as Sprite;
            }

            BackgroundManager.Instance.setBackground();

            yield return new WaitForSeconds(180);
        }
    }

    private IEnumerator StartChat()
    {
        while (true)
        {
            Random rand = new Random();

            var index = rand.Next(0, chatString.Length);

            ChatText.text = chatString[index];
            Chat.gameObject.SetActive(true);

            Invoke("WaitChatTime", 3f);

            yield return new WaitForSeconds(10f);
        }
    }

    private void WaitChatTime()
    {
        Chat.gameObject.SetActive(false);
    }

    private void CostumeChanged(int i)
    {
        if (PlayerPrefs.GetFloat("Skin", 0) == 0)
        {
            ProfileImage.sprite = Resources.Load("Costume" + (i + 1), typeof(Sprite)) as Sprite;
        }
        else
        {
            ProfileImage.sprite =
                Resources.Load("Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0) + "/Profile", typeof(Sprite)) as Sprite;
        }
    }

    private void Update()
    {
        try
        {
            goldDisplayer.text = "GOLD : " + DataController.Instance.FormatGold(DataController.Instance.gold);
            SkillView.fillAmount = DataController.Instance.skillTime;
        }
        catch (Exception e)
        {
            DataController.Instance.gold = 300000000000000000000000000000000000000f;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!OnMenu.gameObject.active && !BackPanel.gameObject.active)
                {
                    Application.Quit();
                }
            }
        }
    }

    public void SetTitle()
    {
        costumeText.text = "클릭당 골드 + " +
                           DataController.Instance.FormatGold(DataController.Instance.plusGoldPerClick - 1) + "배";
        ridingText.text = "초당 골드 + " + DataController.Instance.FormatGold(DataController.Instance.goldPerSec) + "G";
        petText.text = "피버타임 골드 + " + DataController.Instance.feverGoldRising + "배";
        if ((int) PlayerPrefs.GetFloat("TradeLevel", 0) < 14)
        {
            tradeText.text = "다음 무역지 : 초당 골드 x " +
                             DataController.Instance.tradeRevenue[(int) PlayerPrefs.GetFloat("TradeLevel", 0) + 1];
        }
        else
        {
            tradeText.text = "다음 무역지 : 준비중입니다!";
        }

        productText.text = "초당 골드 + " + DataController.Instance.FormatGold(DataController.Instance.goldPerSec) + "G";
        townText.text = "초당 골드 + " + DataController.Instance.plusGoldPerSec + "배";
        collectionText.text =
            "클릭당 골드 + " + string.Format("{0:#.#}", DataController.Instance.collectionGoldPerClick) + "배";
        skinText.text = "TAP + " + (DataController.Instance.skinGoldPerClick - 1) + "배, SEC + " +
                        (DataController.Instance.skinGoldPerSec - 1) + "배";
        reverseText.text = "환생포인트 : " + DataController.Instance.reversePoint;
    }

    private void FixedUpdate()
    {
        if (!DataController.Instance.isFever)
        {
            tabDisplayer.text =
                DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick) + "G";
        }
        else
        {
            tabDisplayer.text =
                DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
                                                   DataController.Instance.feverGoldRising) + "G";
        }

        diaDisplayer.text = DataController.Instance.dia.ToString();

        secDisplayer.text = DataController.Instance.FormatGold(DataController.Instance.goldPerSec *
                                                               DataController.Instance.plusGoldPerSec *
                                                               DataController.Instance.skinGoldPerSec *
                                                               DataController.Instance.reverseGolePerSec) + "G";

        TradeSlider.value =
            100 / DataController.Instance.tradeCompleteTime *
            PlayerPrefs.GetFloat("TradeTime_" + PlayerPrefs.GetFloat("TradeLevel", 0), 0);


        if (costumePrice < DataController.Instance.gold && costumeIndex < CostumeMenu.LENGTH)
        {
            if (!newCostume.gameObject.active)
            {
                newCostume.gameObject.SetActive(true);
            }
        }
        else
        {
            if (newCostume.gameObject.active)
            {
                newCostume.gameObject.SetActive(false);
            }
        }

        if (ridingPrice < DataController.Instance.gold && ridingIndex < RidingMenu.LENGTH)
        {
            if (!newRiding.gameObject.active)
            {
                newRiding.gameObject.SetActive(true);
            }
        }
        else
        {
            if (newRiding.gameObject.active)
            {
                newRiding.gameObject.SetActive(false);
            }
        }

        if (petPrice < DataController.Instance.gold && petIndex < PetMenu.LENGTH)
        {
            if (!newPet.gameObject.active)
            {
                newPet.gameObject.SetActive(true);
            }
        }
        else
        {
            if (newPet.gameObject.active)
            {
                newPet.gameObject.SetActive(false);
            }
        }

        if (townPrice < DataController.Instance.gold && townIndex < TownMenu.LENGTH)
        {
            if (!newTown.gameObject.active)
            {
                newTown.gameObject.SetActive(true);
            }
        }
        else
        {
            if (newTown.gameObject.active)
            {
                newTown.gameObject.SetActive(false);
            }
        }

        if (backgroundPrice < DataController.Instance.gold && backgroundIndex < BackgroundMenu.LENGTH)
        {
            if (!newbackground.gameObject.active)
            {
                newbackground.gameObject.SetActive(true);
            }
        }
        else
        {
            if (newbackground.gameObject.active)
            {
                newbackground.gameObject.SetActive(false);
            }
        }
    }
}