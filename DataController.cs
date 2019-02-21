using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DataController : MonoBehaviour
{
    private static DataController _instance;

    public float masterGoldPerClick; 

    public static DataController Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<DataController>();
            if (_instance != null) return _instance;
            var container = new GameObject("DataController");

            _instance = container.AddComponent<DataController>();

            return _instance;
        }
    }

    private void Awake()
    {
        if (Debug.isDebugBuild)
        {
//            PlayerPrefs.DeleteAll();
        }
        feverGoldRising = PlayerPrefs.GetFloat("FeverGoldRising", 1);
    }

    private IEnumerator AutoClick()
    {
        while (true)
        {
            if (!isFever)
            {
                gold += masterGoldPerClick;
            }
            else
            {
                gold += masterGoldPerClick * feverGoldRising;
            }

            DataChangeEvent.Instance.Tab(100);

            yield return new WaitForSeconds(reinforceAutoTime);
        }
    }
    
    private IEnumerator AutoClick2()
    {
        while (true)
        {
            if (PlayerPrefs.GetFloat("AutoItem", 0) == 1)
            {
                if (!isFever)
                {
                    gold += masterGoldPerClick;
                }
                else
                {
                    gold += masterGoldPerClick * feverGoldRising;
                }

                DataChangeEvent.Instance.Tab(100);

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private void OnEnable()
    {

        
        print(SaveDataManager.Instance.xmlData);
        // 생산품 코루틴 함수 입력
        DataChangeEvent.StartTradeEvent += index => { StartCoroutine(StartTrade(index)); };

        DataChangeEvent.SkillStartEvent += index => { StartCoroutine(CoolDown(index)); };

        StartCoroutine("GoldPerSec");

        StartCoroutine("AutoClick");
        StartCoroutine("AutoClick2");
    }

    private IEnumerator CoolDown(int i)
    {
        float coolDownValue;
        coolDownValue = 1f / 180f;
        while (true)
        {
            PlayerPrefs.SetFloat("CoolDown_" + i,
                PlayerPrefs.GetFloat("CoolDown_" + i, 0) - coolDownValue * Time.deltaTime);
            if (PlayerPrefs.GetFloat("CoolDown_" + i, 0) <= 0)
            {
                PlayerPrefs.SetFloat("CoolDown_" + i, 0);
                yield break;
            }

            yield return null;
        }
    }

    public float skillTime;

    private void Update()
    {
        if (skillTime > 0)
        {
            skillTime -= 0.05f * Time.deltaTime;
        }
    }

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (PlayerPrefs.GetFloat("TradeTime_" + (int) PlayerPrefs.GetFloat("TradeLevel", 0), 0) > 0)
        {
            StartCoroutine(StartTrade((int) PlayerPrefs.GetFloat("TradeLevel", 0)));
            isTrading = true;
        }
        else
        {
            isTrading = false;
        }

        var i = 0;

        for (int j = 0; j < 5; j++)
        {
            if (PlayerPrefs.GetFloat("CoolDown_" + j, 0) != 0)
            {
                StartCoroutine(CoolDown(j));
            }
        }
    }

    public bool isTrading;

    public float tradeCompleteTime = 180f;

    public float[] tradeRevenue =
    {
        600, 700, 800, 900, 1000, 1100, 1200, 1300, 1400,
        1500, 1600, 1700, 1800, 1900, 2000
    };

    private IEnumerator StartTrade(int index)
    {
        while (true)
        {
            var value = PlayerPrefs.GetFloat("TradeTime_" + index, 0) + Time.deltaTime;
            PlayerPrefs.SetFloat("TradeTime_" + index, value);

            print(PlayerPrefs.GetFloat("TradeTime_" + index, 0));

            if (PlayerPrefs.GetFloat("TradeTime_" + index, 0) >= tradeCompleteTime)
            {
                float completeGold;
                PlayerPrefs.SetFloat("TradeTime_" + index, 0);

                completeGold = goldPerSec
                               * plusGoldPerSec
                               * skinGoldPerSec
                               * reverseGolePerSec
                               * tradeRevenue[(int) PlayerPrefs.GetFloat("TradeLevel", 0)];

                gold += completeGold;

                isTrading = false;


                DataChangeEvent.Instance.TradeComplete(completeGold);

                yield break;
            }

            yield return null;
        }
    }

    public float plusGoldPerSec
    {
        get { return PlayerPrefs.GetFloat("PlusGoldPerSec", 1); }
        set { PlayerPrefs.SetFloat("PlusGoldPerSec", value); }
    }

    private IEnumerator GoldPerSec()
    {
        while (true)
        {
            gold += goldPerSec * plusGoldPerSec * skinGoldPerSec * reverseGolePerSec * 0.5f;

            yield return new WaitForSeconds(0.5f);
        }
    }

    private readonly string[] arrDecimal = {"", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극"};

    // Boolean
    public bool isCharacterMove;
    public bool isFever;
    public bool isSkillOn;

    public float feverGoldRising;

    // Data
    public float level
    {
        get { return PlayerPrefs.GetFloat("Level", 1); }
        set { PlayerPrefs.SetFloat("Level", value); }
    }

    public float gold
    {
        get { return PlayerPrefs.GetFloat("Gold", 0f); }
        set { PlayerPrefs.SetFloat("Gold", value); }
    }

    public float dia
    {
        get { return PlayerPrefs.GetFloat("Dia", 0); }
        set { PlayerPrefs.SetFloat("Dia", value); }
    }

    public float goldPerClick
    {
        get { return PlayerPrefs.GetFloat("GoldPerClick", 1); }
        set { PlayerPrefs.SetFloat("GoldPerClick", value); }
    }

    public float plusGoldPerClick
    {
        get { return PlayerPrefs.GetFloat("PlusGoldPerClick", 1); }
        set { PlayerPrefs.SetFloat("PlusGoldPerClick", value); }
    }

    public float levelGoldPerClick
    {
        get { return PlayerPrefs.GetFloat("LevelGoldPerClick", 1); }
        set { PlayerPrefs.SetFloat("LevelGoldPerClick", value); }
    }

    public float collectionGoldPerClick
    {
        get { return PlayerPrefs.GetFloat("CollectionGoldPerClick", 1); }
        set { PlayerPrefs.SetFloat("CollectionGoldPerClick", value); }
    }

    public float reinforceGoldPerClick
    {
        get { return PlayerPrefs.GetFloat("ReinforceGoldPerClick", 1); }
        set { PlayerPrefs.SetFloat("ReinforceGoldPerClick", value); }
    }

    public float skinGoldPerClick
    {
        get { return PlayerPrefs.GetFloat("SkinGoldPerClick", 1); }
        set { PlayerPrefs.SetFloat("SkinGoldPerClick", value); }
    }

    public float skinGoldPerSec
    {
        get { return PlayerPrefs.GetFloat("SkinGoldPerSec", 1); }
        set { PlayerPrefs.SetFloat("SkinGoldPerSec", value); }
    }

    public float reinforceAutoTime
    {
        get { return PlayerPrefs.GetFloat("ReinforceAutoTime", 5); }
        set { PlayerPrefs.SetFloat("ReinforceAutoTime", value); }
    }

    // 초당 골드
    public float goldPerSec
    {
        get { return PlayerPrefs.GetFloat("GoldPerSec", 0); }
        set { PlayerPrefs.SetFloat("GoldPerSec", value); }
    }

    public float skillGoldPerClick = 1;

    public bool reinforceIsPurchase;

    // 하트
    public float heart;

    public float[] skillRisingGold = {2, 5, 10, 20, 30};

    public float reverseLevel
    {
        get { return PlayerPrefs.GetFloat("ReverseLevel", 0); }
        set { PlayerPrefs.SetFloat("ReverseLevel", value); }
    }

    public float reversePoint
    {
        get { return PlayerPrefs.GetFloat("ReversePoint", 0); }
        set { PlayerPrefs.SetFloat("ReversePoint", value); }
    }

    public float reverseGolePerClick
    {
        get { return PlayerPrefs.GetFloat("ReverseGolePerClick", 1); }
        set { PlayerPrefs.SetFloat("ReverseGolePerClick", value); }
    }

    public float reverseGolePerSec
    {
        get { return PlayerPrefs.GetFloat("ReverseGolePerSec", 1); }
        set { PlayerPrefs.SetFloat("ReverseGolePerSec", value); }
    }

    public float plusCollectionDrop
    {
        get { return PlayerPrefs.GetFloat("PlusCollectionDrop", 1); }
        set { PlayerPrefs.SetFloat("PlusCollectionDrop", value); }
    }

    public float GetCostumeInfo(int index)
    {
        return PlayerPrefs.GetFloat("Costume_" + index, index == 0 ? 2 : 0);
    }

    public float attendanceIndex
    {
        get { return PlayerPrefs.GetFloat("AttendanceIndex", 0); }
        set { PlayerPrefs.SetFloat("AttendanceIndex", value); }
    }
    
    public float lastAttendance
    {
        get { return PlayerPrefs.GetFloat("LastAttendance", 0); }
        set { PlayerPrefs.SetFloat("LastAttendance", value); }
    }

    public float lastPlayTime
    {
        get { return PlayerPrefs.GetFloat("LastPlayTime", 0); }
        set { PlayerPrefs.SetFloat("LastPlayTime", value); }
    }

    public int bangchi;

    public float criticalPer;

    public float criticalPercent
    {
        get { return PlayerPrefs.GetFloat("CriticalPercent", 0); }
        set { PlayerPrefs.SetFloat("CriticalPercent", value); }
    }

    public float criticalRising
    {
        get { return PlayerPrefs.GetFloat("CriticalRising", 1); }
        set { PlayerPrefs.SetFloat("CriticalRising", value); }
    }

    public string playerID
    {
        get { return PlayerPrefs.GetString("PlayerID", ""); }
        set { PlayerPrefs.SetString("PlayerID", value); }
    }

    public float compensationGold;
    
//    public float pvpCostumeIndex;
//    public float pvpRidingIndex;
//    public float pvpPetIndex;
//    public float pvpGoldPerClick;
//    public float pvpAutoClick;
//    public float pvpCriticalPercnet;
//    public float pvpCriticalRising;
//    public float pvpFeverRising;
//
//    public float playerScore;
//    public float AIScore;

    public User player;
    
    public void LoadObjectPosition(DragObject dragObject)
    {
        var key = dragObject.name;
        if (key.Contains("Character"))
        {
            dragObject.x = PlayerPrefs.GetFloat(key + "X", 0);
            dragObject.y = PlayerPrefs.GetFloat(key + "Y", -154);
            dragObject.z = PlayerPrefs.GetFloat(key + "Z", 0);
        }
        else if (key.Contains("Riding"))
        {
            dragObject.x = PlayerPrefs.GetFloat(key + "X", -160);
            dragObject.y = PlayerPrefs.GetFloat(key + "Y", -50);
            dragObject.z = PlayerPrefs.GetFloat(key + "Z", 0);
        }
        else if (key.Contains("BlackSmith"))
        {
            dragObject.x = PlayerPrefs.GetFloat(key + "X", -240);
            dragObject.y = PlayerPrefs.GetFloat(key + "Y", -275);
            dragObject.z = PlayerPrefs.GetFloat(key + "Z", 0);
        }
        else if (key.Contains("Tailor"))
        {
            dragObject.x = PlayerPrefs.GetFloat(key + "X", 230);
            dragObject.y = PlayerPrefs.GetFloat(key + "Y", -220);
            dragObject.z = PlayerPrefs.GetFloat(key + "Z", 0);
        }
        else if (key.Contains("Pet"))
        {
            dragObject.x = PlayerPrefs.GetFloat(key + "X", 60);
            dragObject.y = PlayerPrefs.GetFloat(key + "Y", -250);
            dragObject.z = PlayerPrefs.GetFloat(key + "Z", 0);
        }
    }

    public void SaveObjectPosition(DragObject dragObject)
    {
        var key = dragObject.name;

        if (key.Contains("Character"))
        {
            PlayerPrefs.SetFloat(key + "X", dragObject.x);
            PlayerPrefs.SetFloat(key + "Y", dragObject.y);
            PlayerPrefs.SetFloat(key + "Z", dragObject.z);
        }
        else if (key.Contains("Riding"))
        {
            PlayerPrefs.SetFloat(key + "X", dragObject.x);
            PlayerPrefs.SetFloat(key + "Y", dragObject.y);
            PlayerPrefs.SetFloat(key + "Z", dragObject.z);
        }
        else if (key.Contains("BlackSmith"))
        {
            PlayerPrefs.SetFloat(key + "X", dragObject.x);
            PlayerPrefs.SetFloat(key + "Y", dragObject.y);
            PlayerPrefs.SetFloat(key + "Z", dragObject.z);
        }
        else if (key.Contains("Tailor"))
        {
            PlayerPrefs.SetFloat(key + "X", dragObject.x);
            PlayerPrefs.SetFloat(key + "Y", dragObject.y);
            PlayerPrefs.SetFloat(key + "Z", dragObject.z);
        }
        else if (key.Contains("Pet"))
        {
            PlayerPrefs.SetFloat(key + "X", dragObject.x);
            PlayerPrefs.SetFloat(key + "Y", dragObject.y);
            PlayerPrefs.SetFloat(key + "Z", dragObject.z);
        }
    }
    
    public void LoadUpgradeButton(UpgradeButton upgradeButton)
    {
        var key = upgradeButton.upgradeName;

        upgradeButton.currentCost = PlayerPrefs.GetFloat(key + "_cost", upgradeButton.startCurrentCost);
    }

    public void SaveUpgradeButton(UpgradeButton upgradeButton)
    {
        var key = upgradeButton.upgradeName;
        
        PlayerPrefs.SetFloat(key + "_cost", upgradeButton.currentCost);
    }

    // 환생 시 데이터 리셋
    public void ResetData()
    {
        // 레벨 리셋
        level = 1;

        // 골드 리셋
        gold = 0;
        
        // 반지 초기화 
        PlayerPrefs.SetFloat("ReinforceLevel", 0);
        reinforceGoldPerClick = 1;
        reinforceAutoTime = 5;
        
        for (int i = 0; i < 20; i++)
        {
            PlayerPrefs.SetFloat("Background_" + i, 0);
            if (i == 0)
            {
                PlayerPrefs.SetFloat("Costume_" + i, 2);
            }
            else
            {
                PlayerPrefs.SetFloat("Costume_" + i, 0);
            }
            PlayerPrefs.SetFloat("Pet_" + i, 0);
            PlayerPrefs.SetFloat("Riding_" + i, 0);
            PlayerPrefs.SetFloat("Town_" + i, 0);
        }
        
        // 수집품 초기화
        for (int i = 0; i < 50; i++)
        {
            PlayerPrefs.SetFloat("CollectionItem_" + i, 0);
        }

        collectionGoldPerClick = 1;

        // 탭당 수익 리셋
        goldPerClick = 1;
        plusGoldPerClick = 1;
        levelGoldPerClick = 1;

        masterGoldPerClick = goldPerClick *
                             levelGoldPerClick *
                             skillGoldPerClick *
                             plusGoldPerClick *
                             collectionGoldPerClick *
                             reinforceGoldPerClick *
                             skinGoldPerClick *
                             reverseGolePerClick;

        // 펫 능력 리셋
        feverGoldRising = 1;

        // 초당 수익 리셋
        goldPerSec = 0;
        plusGoldPerSec = 1;

        // 생산품 인덱스 초기화
        PlayerPrefs.SetFloat("BackgroundIndex", 0);

        // 무역 레벨 리셋
        PlayerPrefs.SetFloat("TradeLevel", 0);

        // 업그레이드 비용 초기화
        PlayerPrefs.SetFloat("CharcterUpgrade_cost", 100);
    }

    public string FormatGold(float gold)
    {
        if (gold == 0)
        {
            return "0";
        }

        var goldDouble = (double) gold;
        int stringLength;

        if (goldDouble.ToString("#").Length % 4 != 0)
        {
            stringLength = goldDouble.ToString("#").Length + 4 - goldDouble.ToString("#").Length % 4;
        }
        else
        {
            stringLength = goldDouble.ToString("#").Length;
        }

        var sNum = goldDouble.ToString("#").PadLeft(stringLength);
        var displayNum = string.Empty;
        var j = 0;
        for (var i = 0; i < sNum.Length >> 2 && j < 2; i++)
        {
            j++;
            var part = sNum.Substring(i << 2, 4);
            var stringFormat = int.Parse(part);
            if (stringFormat == 0)
            {
                continue;
            }

            displayNum += stringFormat + arrDecimal[(sNum.Length >> 2) - i - 1];
            if (sNum.Length >> 2 - 1 != i) displayNum += " ";
        }

        return displayNum.TrimEnd();
    }

    public float PurchaseGold(float gold)
    {
        if (gold == 0)
        {
            return 0f;
        }

        var goldDouble = (double) gold;
        int stringLength;

        if (goldDouble.ToString("#").Length % 4 != 0)
        {
            stringLength = goldDouble.ToString("#").Length + 4 - goldDouble.ToString("#").Length % 4;
        }
        else
        {
            stringLength = goldDouble.ToString("#").Length;
        }

        var sNum = goldDouble.ToString("#").PadLeft(stringLength);
        var displayNum = string.Empty;
        var j = 0;


        for (var i = 0; i < sNum.Length >> 2; i++)
        {
            if (i == 0)
            {
                var part = sNum.Substring(i >> 2, 4);
                var stringFormat = int.Parse(part);
                displayNum += stringFormat;
            }
            else
            {
                displayNum += "0000";
            }
        }

        return float.Parse(displayNum);
    }

    public string FormatGold2(float gold)
    {
        if (gold == 0)
        {
            return "0";
        }

        var goldDouble = (double) gold;
        int stringLength;

        if (goldDouble.ToString("#").Length % 4 != 0)
        {
            stringLength = goldDouble.ToString("#").Length + 4 - goldDouble.ToString("#").Length % 4;
        }
        else
        {
            stringLength = goldDouble.ToString("#").Length;
        }

        var sNum = goldDouble.ToString("#").PadLeft(stringLength);
        var displayNum = string.Empty;
        var j = 0;

        for (var i = 0; i < sNum.Length >> 2 && j < 1; i++)
        {
            var part = sNum.Substring(i << 2, 4);
            var stringFormat = int.Parse(part);
            if (stringFormat == 0) continue;
            displayNum += stringFormat + arrDecimal[(sNum.Length >> 2) - i - 1];
            j++;
        }

        return displayNum;
    }
    
    public void SaveData()
    {
        SaveDataManager.Instance.xmlData = "";
        
        SaveDataManager.Instance.GetFloat("Level", 1);
        SaveDataManager.Instance.GetFloat("Gold", 0);
        SaveDataManager.Instance.GetFloat("FeverGoldRising", 1);
        SaveDataManager.Instance.GetFloat("GoldPerClick", 1);
        SaveDataManager.Instance.GetFloat("PlusGoldPerClick", 1);
        SaveDataManager.Instance.GetFloat("LevelGoldPerClick", 1);
        SaveDataManager.Instance.GetFloat("CollectionGoldPerClick", 1);
        SaveDataManager.Instance.GetFloat("ReverseGolePerClick", 1);
        SaveDataManager.Instance.GetFloat("ReinforceGoldPerClick", 1);
        SaveDataManager.Instance.GetFloat("SkinGoldPerClick", 1);
        SaveDataManager.Instance.GetFloat("GoldPerSec", 0);
        SaveDataManager.Instance.GetFloat("PlusGoldPerSec", 1);
        SaveDataManager.Instance.GetFloat("SkinGoldPerSec", 1);
        SaveDataManager.Instance.GetFloat("ReverseGolePerSec", 1);
        SaveDataManager.Instance.GetFloat("ReinforceLevel", 0);
        SaveDataManager.Instance.GetFloat("ReinforceAutoTime", 5);
        SaveDataManager.Instance.GetFloat("WearingTown", 0);

        // Reverse
        SaveDataManager.Instance.GetFloat("ReverseLevel", 0);
        SaveDataManager.Instance.GetFloat("ReversePoint", 0);

        for (int i = 0; i <= 5; i++)
        {
            SaveDataManager.Instance.GetFloat("Reverse_" + i, i == 0 ? 1 : 0);
        }

        // Collection
        for (int i = 1; i <= 40; i++)
        {
            SaveDataManager.Instance.GetFloat("CollectionItem_" + i, 0);
        }

        // Menu
        for (int i = 0; i <= 4; i++)
        {
            SaveDataManager.Instance.GetFloat("Skill_" + i, 0);
        }

        for (int i = 0; i < CostumeMenu.LENGTH; i++)
        {
            SaveDataManager.Instance.GetFloat("Costume_" + i, i == 0 ? 2 : 0);
        }

        for (int i = 0; i < RidingMenu.LENGTH; i++)
        {
            SaveDataManager.Instance.GetFloat("Riding_" + i, 0);
        }

        for (int i = 0; i < BackgroundMenu.LENGTH; i++)
        {
            SaveDataManager.Instance.GetFloat("Background_" + i, 0);
        }

        for (int i = 0; i < PetMenu.LENGTH; i++)
        {
            SaveDataManager.Instance.GetFloat("Pet_" + i, 0);
        }

        for (int i = 0; i < TownMenu.LENGTH; i++)
        {
            SaveDataManager.Instance.GetFloat("Town_" + i, 0);
        }

        // InApp
        SaveDataManager.Instance.GetFloat("NoAds", 0);
        SaveDataManager.Instance.GetFloat("RandomBoxCount", 0);
        SaveDataManager.Instance.GetFloat("BackgroundIndex", 0);

        // WaerSkin
        SaveDataManager.Instance.GetFloat("Skin", 0);

        // Trade
        SaveDataManager.Instance.GetFloat("TradeLevel", 0);

        for (int i = 1; i <= 5; i++)
        {
            SaveDataManager.Instance.GetFloat("Skin_" + i, 0);
        }

        SaveDataManager.Instance.GetFloat("CharcterUpgrade_cost", 100);

        SaveDataManager.Instance.GetFloat("Dia", 0);
        
        // 출석체크 데이터
        SaveDataManager.Instance.GetFloat("AttendanceIndex", 0);
        SaveDataManager.Instance.GetFloat("LastAttendance", 0);

        SaveDataManager.Instance.GetFloat("OverlapCoupon", 0);

        SaveDataManager.Instance.GetString("PlayerID", "");

        SaveDataManager.Instance.GetFloat("PlayTime", 0);
        
        print(SaveDataManager.Instance.xmlData);

        PlayCloudDataManager.Instance.SaveToCloud(SaveDataManager.Instance.xmlData);
    }
}