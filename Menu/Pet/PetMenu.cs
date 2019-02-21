using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetMenu : MonoBehaviour
{
    public static int LENGTH = 20;

    public GameObject itemObject;

    public Transform content;

    public GameObject pet;

    private string[] names =
    {
        "멍청한 댕댕이", "꼬마 불꽃", "돈 밝히는 강아지", "배고픈 선인장", "죽음의 사신", "엉터리 백신", "미니 스파르타", "길 잃은 외계인",
        "귀찮은 거북이", "살 찐 참새", "벌집 속의 곰", "리틀 천사", "리틀 악마", "변질된 야구공", "꿀맛 벌집", "수박 소년", "해적의 보물상자",
        "미니 게임기", "모자 마술", "온천 샴푸"
    };

    private string[] imageSrc =
    {
        "PetProfile1", "PetProfile2", "PetProfile3", "PetProfile4", "PetProfile5", "PetProfile6", "PetProfile7",
        "PetProfile8", "PetProfile9", "PetProfile10", "PetProfile11", "PetProfile12", "PetProfile13", "PetProfile14",
        "PetProfile15", "PetProfile16", "PetProfile17", "PetProfile18", "PetProfile19", "PetProfile20"
    };

    private float[] goldRising =
    {
        1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.4f, 2.6f, 2.8f, 3.0f,
        3.2f, 3.4f, 3.6f, 3.8f, 4f, 4.3f, 4.6f, 4.8f, 5.2f, 5.5f
    };
    
    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50 
    };

    public static float price = 20000;

    private List<PetObject> items = new List<PetObject>();

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
            var itemobjectTemp = ItemTemp.GetComponent<PetObject>();

            itemobjectTemp.Name.text = names[i];
            // info
            itemobjectTemp.info.text = "피버타임 골드 + " + goldRising[i] + "배";
            itemobjectTemp.petImage.sprite = Resources.Load(imageSrc[i], typeof(Sprite)) as Sprite;
            itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text =
                DataController.Instance.FormatGold2(price * Mathf.Pow(12.5f, i)
                                                          * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            var isPurchase = PlayerPrefs.GetFloat("Pet_" + i, 0);
            print(isPurchase);
            if (isPurchase == 0)
            {
                var index = i;
                if (i == 0 || PlayerPrefs.GetFloat("Pet_" + (i - 1), 0) > 0)
                {
                    itemobjectTemp.notPurchasePanel.SetActive(false);
                    itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
                else if (PlayerPrefs.GetFloat("Pet_" + i, 0) == 0)
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
            }

            items.Add(itemobjectTemp);

            ItemTemp.transform.SetParent(content);
        }
    }

    public void PurchaseItem(int index)
    {
        if (DataController.Instance.PurchaseGold(price * Mathf.Pow(12.5f, index)
                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]) < DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * Mathf.Pow(12.5f, index)
                                                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            pet.SetActive(true);

            for (int i = 0; i < names.Length; i++)
            {
                if (PlayerPrefs.GetFloat("Pet_" + i, 0) == 2)
                {
                    SetNotWearingButton(i);
                    break;
                }
            }

            items[index].purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);

            PlayerPrefs.SetFloat("FeverGoldRising", goldRising[index]);
            DataController.Instance.feverGoldRising = goldRising[index];

            SetWearingButton(index);


            DataChangeEvent.Instance.PurchasePet1();

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
            if (PlayerPrefs.GetFloat("Pet_" + i, 0) == 2)
            {
                SetNotWearingButton(i);
                break;
            }
        }

        SetWearingButton(index);
    }

    private void SetWearingButton(int index)
    {
        PlayerPrefs.SetFloat("Pet_" + index, 2);
        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용중";
        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
        items[index].purchaseButton.onClick.RemoveAllListeners();

        // TODO 펫 데이터 변경

        DataChangeEvent.Instance.PetDataChanged(index);
    }

    private void SetNotWearingButton(int index)
    {
        PlayerPrefs.SetFloat("Pet_" + index, 1);
        items[index].purchaseButton.GetComponentInChildren<Text>().text = "착용하기";
        items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        items[index].purchaseButton.onClick.RemoveAllListeners();
        items[index].purchaseButton.onClick.AddListener(delegate { NotWearingItemClick(index); });
    }
}