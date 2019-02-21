using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceMenu : MonoBehaviour
{
    public Image RingImage;
    public Text RingLevel;
    public Text BeforeText;
    public Text BeforeInfo;
    public Text AfterText;
    public Button ReinforceButton;
    public Text ReinforcePrice;

    public GameObject ReinforcePanel;
    public GameObject OnMenu1;

    private float[] ReinforceAutoClick =
    {
        5,
        2.0f, 1.9f, 1.8f, 1.7f, 1.6f,
        1.5f, 1.4f, 1.3f, 1.2f, 1.1f,
        1.0f, 0.95f, 0.9f, 0.85f, 0.8f,
        0.75f, 0.70f, 0.65f, 0.6f, 0.55f,
        0.5f, 0.45f, 0.4f, 0.35f, 0.3f,
        0.25f, 0.2f, 0.18f, 0.16f, 0.14f,
        0.12f, 0.10f, 0.098f, 0.096f, 0.095f,
        0.094f, 0.093f, 0.092f, 0.091f, 0.09f,
        0.089f, 0.088f, 0.087f, 0.086f, 0.085f,
        0.084f, 0.083f, 0.082f, 0.081f, 0.08f
    };

    private float[] ReinforceGoldPerClick =
    {
        0,
        0.2f, 0.2f, 0.2f, 0.2f, 0.2f,
        0.5f, 0.5f, 1f, 1f, 1,
        5, 6, 7, 8, 9,
        10, 20, 30, 40, 50,
        60, 70, 80, 90, 100,
        300, 500, 1000, 2500, 8000,
        10000, 15000, 20000, 25000, 30000,
        50000, 60000, 70000, 80000, 90000,
        100000, 150000, 200000, 250000, 300000,
        400000, 500000, 600000, 800000, 1000000
    };

    private float price = 100000f;

    private void OnEnable()
    {
        SetPanel();

        DataChangeEvent.ReinforceEvent += SetPanel;
    }

    public void OnClick()
    {
        ReinforcePanel.SetActive(true);
        OnMenu1.SetActive(true);
    }

    private void SetPanel()
    {
        if (PlayerPrefs.GetFloat("ReinforceLevel", 0) == 0)
        {
            RingImage.sprite =
                Resources.Load("Ring/" + (PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1), typeof(Sprite)) as Sprite;
            RingLevel.text = "Lv. " + PlayerPrefs.GetFloat("ReinforceLevel", 0);
            BeforeText.text = ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)] +
                              "초마다\n자동 클릭/\n클릭당 골드 X " + DataController.Instance.reinforceGoldPerClick;
            BeforeInfo.text = "";
            AfterText.text = ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1]
                             + "초마다\n자동 클릭/\n클릭당 골드 X " + (DataController.Instance.reinforceGoldPerClick
                                                           + ReinforceGoldPerClick[
                                                               (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1]);
            ReinforcePrice.text = "강화하기(" + DataController.Instance.FormatGold(price) + "G)";
            ReinforceButton.onClick.AddListener(OnClick);
        }
        else if (PlayerPrefs.GetFloat("ReinforceLevel", 0) < 50)
        {
            RingImage.sprite =
                Resources.Load("Ring/" + (PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1), typeof(Sprite)) as Sprite;
            RingLevel.text = "Lv. " + PlayerPrefs.GetFloat("ReinforceLevel", 0);
            BeforeText.text = ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)] +
                              "초마다\n자동 클릭/\n클릭당 골드 X " + DataController.Instance.reinforceGoldPerClick;
            BeforeInfo.text = "실패 시 1단계 하락";
            AfterText.text = ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1]
                             + "초마다\n자동 클릭/\n클릭당 골드 X " + (DataController.Instance.reinforceGoldPerClick
                                                           + ReinforceGoldPerClick[
                                                               (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1]);
            ReinforcePrice.text = "강화하기(" + DataController.Instance.FormatGold(price * Mathf.Pow(4.6f,
                                                                                   (int) PlayerPrefs.GetFloat(
                                                                                       "ReinforceLevel", 0))) + "G)";
            ReinforceButton.onClick.AddListener(OnClick);
        }
        else if (PlayerPrefs.GetFloat("ReinforceLevel", 0) >= 50)
        {
            RingImage.sprite =
                Resources.Load("Ring/" + (PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1), typeof(Sprite)) as Sprite;
            RingLevel.text = "Lv. " + PlayerPrefs.GetFloat("ReinforceLevel", 0);
            BeforeText.text = ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)] +
                              "초마다\n자동 클릭/\n클릭당 골드 X " + DataController.Instance.reinforceGoldPerClick;
            BeforeInfo.text = "";
            AfterText.text = "";
            ReinforcePrice.text = "강화하기";
            ReinforceButton.onClick.RemoveAllListeners();
            ReinforceButton.GetComponent<Image>().color = new Color(0.9176f, 0.8392f, 0.5803f, 0.6f);
        }
    }
}