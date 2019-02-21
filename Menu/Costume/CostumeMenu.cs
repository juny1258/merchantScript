using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumeMenu : MonoBehaviour
{
    public static int LENGTH = 20;
    
    public GameObject itemObject;

    public Transform content;

    private string[] names =
    {
        "잡상인", "대장장이", "다이아 상인", "스크립트 상인", "우주 대마법사", 
        "코가 약한 좀비", "스파르타!!", "우주인", "귀여운 토끼 옷", "배고픈 악어", "서커스 곰",
        "천사 코스튬", "악마 코스튬", "홈런왕", "귀여운 꿀벌", "시원한 바캉스", "해적왕 호피",
        "우주 대스타", "야매 마술사", "시워언해"
    };

    private string[] imageSrc =
    {
        "Costume1", "Costume2", "Costume3", "Costume4", "Costume5", "Costume6", "Costume7", "Costume8", "Costume9",
        "Costume10", "Costume11", "Costume12", "Costume13", "Costume14", "Costume15", "Costume16", "Costume17",
        "Costume18", "Costume19", "Costume20"
    };

    private float[] goldRisingPercent = {
        0f, 1f, 3f, 7f, 20f, 50f, 150f, 300f, 1800f, 3000f,
        10000f, 30000, 82000f, 230000, 650000f, 1800000f, 5400000f, 15000000f, 50000000f, 200000000f
    };
    
    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50
    };

    public static float price = 230000f;

    private List<CostumeObject> items = new List<CostumeObject>();
    
    public GameObject BackPanel;

    public GameObject OnMenu1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OnMenu1.active)
            {
                gameObject.SetActive(false);
                BackPanel.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        items.Clear();
        Binding();
    }

    /// 아이템 리스트를 지정된 프리팹으로 변환하여 추가합니다.
    private void Binding()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        print("Length : " + names.Length);

        for (var i = 0; i < names.Length; i++)
        {
            //추가할 오브젝트를 생성한다.
            var ItemTemp = Instantiate(itemObject);
            //오브젝트가 가지고 있는 'ItemObject'를 찾는다.
            var itemobjectTemp = ItemTemp.GetComponent<CostumeObject>();

            itemobjectTemp.Name.text = names[i];
            itemobjectTemp.info.text = "클릭당 골드 + " + DataController.Instance.FormatGold(goldRisingPercent[i]) + "배";
            itemobjectTemp.characterImage.sprite = Resources.Load(imageSrc[i], typeof(Sprite)) as Sprite;
            itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text =
                DataController.Instance.FormatGold2(price * Mathf.Pow(12.5f, i-1)
                                                    * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);
//            itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text =
//                DataController.Instance.FormatGold(price[i]);
//            itemobjectTemp.purchaseButton.onClick = item.OnItemPurchasEvent;

            var isPurchase = DataController.Instance.GetCostumeInfo(i);
            print(isPurchase);
            if (isPurchase == 0)
            {
                var index = i;
                if (i != 0 && DataController.Instance.GetCostumeInfo(i - 1) > 0)
                {
                    itemobjectTemp.notPurchasePanel.SetActive(false);
                    itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
                else
                {
                    itemobjectTemp.notPurchaseInfo.text = "[" + names[i - 1] + "]" + " 구매 필요";
                    itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
            }
            else if (isPurchase == 1)
            {
                itemobjectTemp.notPurchasePanel.SetActive(false);
                itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text = "착용하기";
                itemobjectTemp.purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
                var index = i;
                itemobjectTemp.purchaseButton.onClick.AddListener(() => NotWearingItemClick(index));
            }
            else if (isPurchase == 2)
            {
                itemobjectTemp.notPurchasePanel.SetActive(false);
                itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text = "착용중";
                itemobjectTemp.purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
                itemobjectTemp.purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
                var index = i;
                DataChangeEvent.Instance.CostumeDataChanged(index);
            }

            //각 속성 입력

            items.Add(itemobjectTemp);

            ItemTemp.transform.SetParent(content);


            //화면에 추가
        }
    }

    public void PurchaseItem(int index)
    {
        if (DataController.Instance.PurchaseGold(price * Mathf.Pow(12.5f, index-1)
                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]) < DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * Mathf.Pow(12.5f, index-1)
                                                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            for (int i = 0; i < names.Length; i++)
            {
                if (DataController.Instance.GetCostumeInfo(i) == 2)
                {
                    SetNotWearingButton(i);
                    break;
                }
            }

            items[index].purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);

            DataController.Instance.plusGoldPerClick += goldRisingPercent[index];
            SetWearingButton(index);
            
            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick * 
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;
            
            DataChangeEvent.Instance.PurchaseCostume();

            try
            {
                items[index + 1].notPurchasePanel.SetActive(false);
            }
            catch (Exception e)
            {
            }
        }
    }

    public void NotWearingItemClick(int index)
    {
        print("NotWearingItemClick" + index);
        for (int i = 0; i < names.Length; i++)
        {
            if (DataController.Instance.GetCostumeInfo(i) == 2)
            {
                SetNotWearingButton(i);
                break;
            }
        }

        SetWearingButton(index);
    }

    private void SetWearingButton(int index)
    {
        PlayerPrefs.SetFloat("Costume_" + index, 2);
        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용중";
        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
        items[index].purchaseButton.onClick.RemoveAllListeners();

        DataChangeEvent.Instance.CostumeDataChanged(index);

//        DataChangeEvent.Instance.CharacterChanged();
    }

    private void SetNotWearingButton(int index)
    {
        PlayerPrefs.SetFloat("Costume_" + index, 1);
        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용하기";
        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        items[index].purchaseButton.onClick.RemoveAllListeners();
        items[index].purchaseButton.onClick.AddListener(delegate { NotWearingItemClick(index); });
    }
}