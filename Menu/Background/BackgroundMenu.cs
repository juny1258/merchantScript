using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMenu : MonoBehaviour
{
    public static int LENGTH = 9;

    public GameObject ItemObject;

    public Transform Content;

    private string[] names =
    {
        "마을 건축", "분수대 건축", "작은 화단 설치", "재봉사 고용", "대장장이 고용", "울타리 설치", "가로등 설치", "만국기 설치", "폭죽 설치(밤)"
    };

    private string[] profile1 =
    {
        "분수대 건축", "사과 동상 설치", "우물 건축", "모루 동상 설치", "전사 동상 설치", "돛 동상 설치"
    };

    private string[] profile2 =
    {
        "작은 화단 설치", "야채 농장 설치", "마른 건초 설치", "마른 건초 설치", "작은 화단 설치", "작은 화단 설치"
    };

    private string[] profile6 =
    {
        "가로등 설치", "가로수 설치", "선인장 설치", "가로수 설치", "신전 기둥 설치", "가로등 설치"
    };

    private string[] profile7 =
    {
        "만국기 설치", "고추 말리기", "빨래 널기", "현수막 설치", "만국기 설치", "갈매기 분양"
    };
    
    private float[] reverseRisingPrice =
    {
        1, 5, 10, 20, 30, 40, 50
    };


    private List<BackgroundObject> items = new List<BackgroundObject>();

    private float plusGoldPerSec = 3000;

    public static float price = 80000;

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

    private void OnDisable()
    {
        foreach (Transform content in Content.transform)
        {
            Destroy(content.gameObject);
        }
    }

    public void Binding()
    {
        for (var i = 0; i < names.Length; i++)
        {
            var ItemTemp = Instantiate(ItemObject);
            //오브젝트가 가지고 있는 'ItemObject'를 찾는다.
            var itemObjectTemp = ItemTemp.GetComponent<BackgroundObject>();

            switch (i)
            {
                case 1:
                    itemObjectTemp.Name.text = profile1[(int) PlayerPrefs.GetFloat("WearingTown", 0)];
                    break;
                case 2:
                    itemObjectTemp.Name.text = profile2[(int) PlayerPrefs.GetFloat("WearingTown", 0)];
                    break;
                case 6:
                    itemObjectTemp.Name.text = profile6[(int) PlayerPrefs.GetFloat("WearingTown", 0)];
                    break;
                case 7:
                    itemObjectTemp.Name.text = profile7[(int) PlayerPrefs.GetFloat("WearingTown", 0)];
                    break;
                default:
                    itemObjectTemp.Name.text = names[i];
                    break;
            }

            itemObjectTemp.Info.text = "초당 골드 증가\n+ " +
                                       DataController.Instance.FormatGold(plusGoldPerSec * Mathf.Pow(4.5f, i)) + "G";
            if (i == 8)
            {
                itemObjectTemp.BackgroundImage.sprite =
                    Resources.Load("BackProfile/Profile8", typeof(Sprite)) as Sprite;
            }
            else
            {
                itemObjectTemp.BackgroundImage.sprite =
                    Resources.Load("BackProfile/Profile" + PlayerPrefs.GetFloat("WearingTown", 0) + "" + i,
                        typeof(Sprite)) as Sprite;
            }

            itemObjectTemp.purchaseButton.GetComponentInChildren<Text>().text =
                DataController.Instance.FormatGold2(price * Mathf.Pow(8.5f, i)
                                                          * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            var isPurchase = PlayerPrefs.GetFloat("Background_" + i, 0);

            if (isPurchase == 0)
            {
                var index = i;
                if (i == 0 || PlayerPrefs.GetFloat("Background_" + (i - 1), 0) > 0)
                {
                    itemObjectTemp.notPurchasePanel.SetActive(false);
                    itemObjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
                else if (PlayerPrefs.GetFloat("Background_" + i, 0) == 0)
                {
                    switch (i-1)
                    {
                        case 1:
                            itemObjectTemp.notPurchaseInfo.text = "[" + profile1[(int) PlayerPrefs.GetFloat("WearingTown", 0)] + "]" + " 구매 필요";
                            break;
                        case 2:
                            itemObjectTemp.notPurchaseInfo.text = "[" + profile2[(int) PlayerPrefs.GetFloat("WearingTown", 0)] + "]" + " 구매 필요";
                            break;
                        case 6:
                            itemObjectTemp.notPurchaseInfo.text = "[" + profile6[(int) PlayerPrefs.GetFloat("WearingTown", 0)] + "]" + " 구매 필요";
                            break;
                        case 7:
                            itemObjectTemp.notPurchaseInfo.text = "[" + profile7[(int) PlayerPrefs.GetFloat("WearingTown", 0)] + "]" + " 구매 필요";
                            break;
                        default:
                            itemObjectTemp.notPurchaseInfo.text = "[" + names[i] + "]" + " 구매 필요";
                            break;
                    }
                    itemObjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
                }
            }
            else
            {
                itemObjectTemp.notPurchasePanel.SetActive(false);
                itemObjectTemp.purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
                itemObjectTemp.purchaseButton.GetComponentInChildren<Text>().text = "구매완료";
                itemObjectTemp.purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
            }

            items.Add(itemObjectTemp);

            ItemTemp.transform.SetParent(Content);
        }
    }

    private void PurchaseItem(int index)
    {
        if (DataController.Instance.PurchaseGold(price * Mathf.Pow(8.5f, index)
                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]) < DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * Mathf.Pow(8.5f, index)
                                                                                       * reverseRisingPrice[(int)DataController.Instance.reverseLevel]);

            PlayerPrefs.SetFloat("Background_" + index, 1);
            PlayerPrefs.SetFloat("BackgroundIndex", index + 1);

            items[index].purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
            items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
            items[index].purchaseButton.GetComponentInChildren<Text>().text = "구매완료";
            items[index].purchaseButton.onClick.RemoveAllListeners();

            DataController.Instance.goldPerSec += plusGoldPerSec * Mathf.Pow(4.5f, index);

            DataChangeEvent.Instance.PurchaseBackground();

            try
            {
                items[index + 1].notPurchasePanel.SetActive(false);
            }
            catch (Exception e)
            {
            }
        }
    }
}