using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMenu : MonoBehaviour {
    
    public GameObject itemObject;

    public Transform content;

    private string[] names = {"빛의 정령의 가호", "물의 정령의 가호", "불의 정령의 가호", "흑마법사의 마법진", "태양신의 축복"};

    private string[] imageSrc = {"Skill1", "Skill2", "Skill3", "Skill4", "Skill5"};

    private float[] price = {100, 300, 500, 800, 1200};

    private List<SkillObject> items = new List<SkillObject>();

    private void Start()
    {
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
        for (var i = 0; i < names.Length; i++)
        {
            //추가할 오브젝트를 생성한다.
            var ItemTemp = Instantiate(itemObject);
            //오브젝트가 가지고 있는 'ItemObject'를 찾는다.
            var itemobjectTemp = ItemTemp.GetComponent<SkillObject>();

            itemobjectTemp.Name.text = names[i];
            // info
            itemobjectTemp.info.text = "20초간 클릭당 골드 " + DataController.Instance.skillRisingGold[i] + "배\n(쿨타임 3분)";
            itemobjectTemp.skillImage.sprite = Resources.Load(imageSrc[i], typeof(Sprite)) as Sprite;
            itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text = price[i].ToString();

            var isPurchase = PlayerPrefs.GetFloat("Skill_" + i, 0);
            print(isPurchase);
            if (isPurchase == 0)
            {
                // 구매 전 상태
                var index = i;
                itemobjectTemp.purchaseButton.onClick.AddListener(() => PurchaseItem(index));
            }
            else
            {
                // 구매 후 상태
                itemobjectTemp.purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
                itemobjectTemp.purchaseButton.GetComponentInChildren<Text>().text = "구매완료";
                itemobjectTemp.purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
            }
            //각 속성 입력

            items.Add(itemobjectTemp);

            ItemTemp.transform.SetParent(content);


            //화면에 추가
        }
    }

    public void PurchaseItem(int index)
    {
        if (price[index] <= DataController.Instance.dia)
        {
            DataController.Instance.dia -= price[index];

            PlayerPrefs.SetFloat("Skill_" + index, 1);

            items[index].purchaseButton.GetComponentsInChildren<Image>()[1].gameObject.SetActive(false);
            items[index].purchaseButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.7f);
            items[index].purchaseButton.GetComponentInChildren<Text>().text = "구매완료";
            items[index].purchaseButton.onClick.RemoveAllListeners();
        }
    }
}
