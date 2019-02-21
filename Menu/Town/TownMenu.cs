using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownMenu : MonoBehaviour
{
    public static int LENGTH = 15;
    
    public GameObject itemObject;

    public Transform content;

    private string[] names =
    {
        "루어럴", "엔 에이치 타운", "샌딜", "볼케이브", "콜로세움", "블루 홀", "유니티야드", "데드 존", "루인스",
        "드래곤 빌리지", "페어리어", "엠 피 에리어", "이빌 타운", "릴렌틀리스", "사이버 타운"
    };

    private float[] plusGoldPerSec = {2f, 2f, 2f, 5f, 5f, 5f, 20f, 20f, 20f, 30f, 70f, 100f, 500f, 1500f, 4000f};

    private float price = 5000900000000;
    
    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50
    };

    private List<TownObject> items = new List<TownObject>();

    private void OnEnable()
    {
        items.Clear();
        Binding();
    }

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

    /// 아이템 리스트를 지정된 프리팹으로 변환하여 추가합니다.
    private void Binding()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        
        for (var i = 0; i < names.Length; i++)
        {
            //추가할 오브젝트를 생성한다.
            var ItemTemp = Instantiate(itemObject);
            //오브젝트가 가지고 있는 'ItemObject'를 찾는다.
            var itemobjectTemp = ItemTemp.GetComponent<TownObject>();

            itemobjectTemp.Name.text = names[i];
            // info
            itemobjectTemp.info.text = "초당 골드 + " + plusGoldPerSec[i] + "배";
            itemobjectTemp.townImage.sprite = Resources.Load("Town/Profile" + i, typeof(Sprite)) as Sprite;
            itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text =
                DataController.Instance.FormatGold2(price * Mathf.Pow(6.1f, i)
                                                          * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            var isPurchase = PlayerPrefs.GetFloat("Town_" + i, 0);
            print(isPurchase);
            if (isPurchase == 0)
            {
                var index = i;
                if (i == 0 || PlayerPrefs.GetFloat("Town_" + (i - 1), 0) > 0)
                {
                    itemobjectTemp.notPurchasePanel.SetActive(false);
                    itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
                else if (PlayerPrefs.GetFloat("Town_" + i, 0) == 0)
                {
                    itemobjectTemp.notPurchaseInfo.text = "[" + names[i - 1] + "]" + " 구매 필요";
                    itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
            }
            else
            {
                itemobjectTemp.notPurchasePanel.SetActive(false);
                itemobjectTemp.purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
                itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text = "구매완료";
                itemobjectTemp.purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
                
                itemobjectTemp.purchaseButton.onClick.RemoveAllListeners();
            }
            //각 속성 입력

            items.Add(itemobjectTemp);

            ItemTemp.transform.SetParent(content);


            //화면에 추가
        }
    }

    public void PurchaseItem(int index)
    {
        if (DataController.Instance.PurchaseGold(price * Mathf.Pow(6.1f, index)
                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]) < DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * Mathf.Pow(6.1f, index)
                                                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            PlayerPrefs.SetFloat("Town_" + index, 1);

            items[index].purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
            items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
            items[index].purchaseButton.GetComponentInChildren<Text>().text = "구매완료";
            items[index].purchaseButton.onClick.RemoveAllListeners();

            DataController.Instance.plusGoldPerSec += plusGoldPerSec[index];

//            PlayerPrefs.SetFloat("TownIndex", index);
            
            DataChangeEvent.Instance.PurchaseTown();

            try
            {
                items[index + 1].notPurchasePanel.SetActive(false);
            }
            catch (Exception e)
            {
            }
        }
    }

//    private void SetNotWearingButton(int index)
//    {
//        PlayerPrefs.SetFloat("Town_" + index, 1);
//        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용하기";
//        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
//        items[index].purchaseButton.onClick.RemoveAllListeners();
//    }
//	
}