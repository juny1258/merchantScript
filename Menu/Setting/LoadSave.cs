using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSave : MonoBehaviour
{
    public GameObject NotificationPanel;
    public GameObject OnMenu1;
    
    public void SaveButton()
    {
        OnMenu1.SetActive(true);
        NotificationPanel.SetActive(true);
        NotificationPanel.GetComponentInChildren<Text>().text = "클라우드에 저장하는 중입니다\n잠시만 기다려주세요.";
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
        
        // Critical
        SaveDataManager.Instance.GetFloat("CriticalPercent", 0);
        SaveDataManager.Instance.GetFloat("CriticalRising", 1);

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
        
        SaveDataManager.Instance.GetFloat("PlusCollectionDrop", 1);

        // InApp
        SaveDataManager.Instance.GetFloat("NoAds", 0);
        SaveDataManager.Instance.GetFloat("RandomBoxCount", 0);
        SaveDataManager.Instance.GetFloat("BackgroundIndex", 0);
        SaveDataManager.Instance.GetFloat("AutoClick", 0);

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

    public void LoadButton()
    {
        PlayCloudDataManager.Instance.LoadFromCloud(data => { SaveDataManager.Instance.GetData(data); });
    }
}