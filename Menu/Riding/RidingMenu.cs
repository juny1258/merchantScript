using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RidingMenu : MonoBehaviour
{
    public static int LENGTH = 20;
    
    public GameObject itemObject;

    public Transform content;

    public GameObject Riding;

    private string[] names =
    {
        "낡은 수레", "지저왕 김두더지", "검은 리무진", "얄미운 낙타", "살찐 레드드래곤", "병에 걸린 생쥐",
        "왕의 전차", "멋진 우주선", "당근 안에 토끼", "방랑자 악어"
        , "외발자전거", "근두운", "지옥에서 온 황소", "배트 비행기", "장수왕 김풍뎅", "새 이름 몰라", "유령의 해적선",
        "고장난 게임기", "조수 비둘이", "욕탕안에 나있다"
    };

    private string[] imageSrc =
    {
        "RidingProfile1", "RidingProfile2", "RidingProfile3", "RidingProfile4", "RidingProfile5", "RidingProfile6", "RidingProfile7", "RidingProfile8", "RidingProfile9",
        "RidingProfile10", "RidingProfile11", "RidingProfile12", "RidingProfile13", "RidingProfile14", "RidingProfile15", "RidingProfile16", "RidingProfile17",
        "RidingProfile18", "RidingProfile19", "RidingProfile20"
    };
    
    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50
    };

    private float plusGoldPerSec = 1000f;

    public static float price = 30000f;

    private List<RidingObject> items = new List<RidingObject>();
    
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

        for (var i = 0; i < names.Length; i++)
        {
            //추가할 오브젝트를 생성한다.
            var ItemTemp = Instantiate(itemObject);
            //오브젝트가 가지고 있는 'ItemObject'를 찾는다.
            var itemobjectTemp = ItemTemp.GetComponent<RidingObject>();

            itemobjectTemp.Name.text = names[i];
            // info
            itemobjectTemp.info.text = "초당 골드 증가\n+ " + DataController.Instance.FormatGold(plusGoldPerSec * Mathf.Pow(6.2f, i)) + "G";
            itemobjectTemp.ridingImage.sprite = Resources.Load(imageSrc[i], typeof(Sprite)) as Sprite;
            itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text =
                DataController.Instance.FormatGold2(price * Mathf.Pow(12.5f, i)
                                                          * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            var isPurchase = PlayerPrefs.GetFloat("Riding_" + i, 0);
            if (isPurchase == 0)
            {
                var index = i;
                if (i == 0 || PlayerPrefs.GetFloat("Riding_" + (i-1), 0) > 0)
                {
                    itemobjectTemp.notPurchasePanel.SetActive(false);
                    itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
                else if (PlayerPrefs.GetFloat("Riding_" + i, 0) == 0)
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
                DataChangeEvent.Instance.RidingDataChanged(index);
            }

            //각 속성 입력

            items.Add(itemobjectTemp);

            ItemTemp.transform.SetParent(content);


            //화면에 추가
        }
    }

    public void PurchaseItem(int index)
    {
        if (DataController.Instance.PurchaseGold(price * Mathf.Pow(12.5f, index)
                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]) < DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * Mathf.Pow(12.5f, index)
                                                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);
            
            Riding.SetActive(true);

            for (int i = 0; i < names.Length; i++)
            {
                if (PlayerPrefs.GetFloat("Riding_" + i, 0) == 2)
                {
                    SetNotWearingButton(i);
                    break;
                }
            }

            items[index].purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);

            DataController.Instance.goldPerSec += plusGoldPerSec * Mathf.Pow(6.2f, index);
            
            DataChangeEvent.Instance.PurchaseRiding();
            
            SetWearingButton(index);

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
            if (PlayerPrefs.GetFloat("Riding_" + i, 0) == 2)
            {
                SetNotWearingButton(i);
                break;
            }
        }

        SetWearingButton(index);
    }

    private void SetWearingButton(int index)
    {
        PlayerPrefs.SetFloat("Riding_" + index, 2);
        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용중";
        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
        items[index].purchaseButton.onClick.RemoveAllListeners();

        DataChangeEvent.Instance.RidingDataChanged(index);

//        DataChangeEvent.Instance.CharacterChanged();
    }

    private void SetNotWearingButton(int index)
    {
        PlayerPrefs.SetFloat("Riding_" + index, 1);
        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용하기";
        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        items[index].purchaseButton.onClick.RemoveAllListeners();
        items[index].purchaseButton.onClick.AddListener(delegate { NotWearingItemClick(index); });
    }

}