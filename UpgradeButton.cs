using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    public Text CharacterTitle;
    public Text LevelText;
    public Text GoldPerClickText;
    public Text CurrentCostText;
    public Image MedalImage;

    public GameObject ReviewPanel;
    public GameObject OnMenu1;

    public GameObject Finger;

    public string upgradeName;

    [HideInInspector] public float level;

    // 업그레이드 후 클릭당 골드
    [HideInInspector] public float goldByUpgrade;
    public float startGoldByUpgrade = 1;

    // 현재 가격
    [HideInInspector] public float currentCost;
    public float startCurrentCost = 100;

    // 업그레이드 후 클릭당 골드 상승 비율
    private float upgradePow = 1.008f;

    // 업그레이드 후 업그레이드 가격 상승 비율
    private float costPow = 1.058f;

    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50
    };

    private string[] merchantName =
    {
        "잡상인", "떠돌이 상인", "숙련된 상인", "돈의 노예", "돈의 정복자", "행복한 상인", 
        "재벌 2세", "만수르", "루어럴의 왕", "샌딜의 왕", "콜로세움의 영웅",
        "세계 최고 부자", "세계정복자", "우주정복자", "상인의 신", "게임 마스터"
    };

    private void OnEnable()
    {
        DataChangeEvent.ResetDataEvent += UpdateUpgrade;
        DataChangeEvent.ResetDataEvent += UpdateUI;
    }

    // Use this for initialization
    private void Start()
    {
        UpdateUpgrade();
        currentCost = startCurrentCost * Mathf.Pow(costPow, DataController.Instance.level - 1) /
            Mathf.Pow(DataController.Instance.level / 100 + 1, 1.5f);

        if (PlayerPrefs.GetFloat("FirstStatusInfomation", 0) == 0)
        {
            Finger.SetActive(true);
        }

        UpdateUI();
    }

    private void FixedUpdate()
    {
        if (DataController.Instance.level >= 1500)
        {
            GetComponentInChildren<Text>().text = "구매 완료";
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
        }
        else if (currentCost * reverseRisingPrice[(int)DataController.Instance.reverseLevel] >= DataController.Instance.gold)
        {
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
        }
        else  GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void PurchaseUpgrade()
    {
        if (DataController.Instance.level >= 1500)
        {
            
        }
        else
        {
            if (!(DataController.Instance.gold >= currentCost
                  * reverseRisingPrice[(int) DataController.Instance.reverseLevel])) return;
            DataController.Instance.gold -=
                currentCost * reverseRisingPrice[(int) DataController.Instance.reverseLevel];
            DataController.Instance.level += 1;

            UpdateUpgrade();

            DataController.Instance.goldPerClick += goldByUpgrade;

            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick *
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;

            UpdateUI();

            if (DataController.Instance.level % 100 == 0)
            {
                if (DataController.Instance.level < 1500)
                {
                    BackgroundManager.Instance.LevelUp(merchantName[(int) (DataController.Instance.level / 100)]);
                }
            }

            if (DataController.Instance.level == 235 && DataController.Instance.reverseLevel == 0)
            {
                ReviewPanel.SetActive(true);
                OnMenu1.SetActive(true);
            }
        }
    }

    private void UpdateUpgrade()
    {
        if (DataController.Instance.level != 1)
        {
            goldByUpgrade = startGoldByUpgrade * Mathf.Pow(upgradePow, DataController.Instance.level);
            currentCost = startCurrentCost * Mathf.Pow(costPow, DataController.Instance.level - 1) /
                          Mathf.Pow(DataController.Instance.level / 100 + 1, 1.5f);
        }
        else if (DataController.Instance.level == 1)
        {
            goldByUpgrade = startGoldByUpgrade * Mathf.Pow(upgradePow, DataController.Instance.level);
            currentCost = startCurrentCost * Mathf.Pow(costPow, DataController.Instance.level - 1);
        }
    }


    private void UpdateUI()
    {
        if (DataController.Instance.level < 1500)
        {
            CharacterTitle.text = "칭호 : " + merchantName[(int) (DataController.Instance.level / 100)];
        }
        else
        {
            CharacterTitle.text = "칭호 : " + merchantName[15];
        }

        LevelText.text = "Lv. " + (int) DataController.Instance.level;
        GoldPerClickText.text =
            DataController.Instance.FormatGold(DataController.Instance.goldPerClick) + "G / TAB";

        CurrentCostText.text = "업그레이드( " + DataController.Instance.FormatGold(currentCost
                                                                              * reverseRisingPrice[(int)DataController.Instance.reverseLevel]) + "G )";
        if ((int) (DataController.Instance.level / 100) == 0)
        {
            MedalImage.gameObject.SetActive(false);
        }
        else if ((int) (DataController.Instance.level / 100) != 0 && (int) (DataController.Instance.level / 100) < 15)
        {
            MedalImage.gameObject.SetActive(true);
            MedalImage.sprite =
                Resources.Load("Medal" + (int) (DataController.Instance.level / 100), typeof(Sprite)) as Sprite;
        }
        else if ((DataController.Instance.level / 100) >= 15)
        {
            MedalImage.gameObject.SetActive(true);
            MedalImage.sprite =
                Resources.Load("Medal15", typeof(Sprite)) as Sprite;
        }
    }
}