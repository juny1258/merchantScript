using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PvpInfomation : MonoBehaviour
{
    public GameObject InfomationPanel;

    public Text InfoText;
    public Button NextButton;

    public GameObject Panel1;
    public GameObject Panel2;

    public GameObject FirstInfo;

    private Color color;

    private int index;

    private string[] infomation =
    {
        "어서와 PVP는 처음이지?\n\n지금부터 PVP에 대해서 설명해줄게.",
        "먼저 PVP는 유저간에 대결을 할 수 있는 기능이야.",
        "지금 화면에 보이는 캐릭터가 보이지?\n\n그게 대전할 때 쓰이는 캐릭터야.",
        "하지만 지금 캐릭터와는 조금 다른 모습을 하고있지?",
        "화면 밑에 동기화 버튼을 누르면\n\n현재 게임의 캐릭터와 같은 능력치로 만들어 줄 수 있어!",
        "현재 캐릭터와 동기화가 되기 때문에\n\n지금 능력치보다 낮은 캐릭터를 동기화하지 않도록 주의해.",
        "그러면 이제 캐릭터를 동기화 시키고 대전을 시작해볼까?"
    };

    private void Start()
    {
    }

    private void OnEnable()
    {
        PlayerPrefs.SetFloat("IsViewInfo", 0);
        if (PlayerPrefs.GetFloat("IsViewInfo", 0) == 0)
        {
            index = 0;
            StartCoroutine(SetText(infomation[index]));
            PlayerPrefs.SetFloat("IsViewInfo", 1);
        }
    }

    private IEnumerator SetText(string text)
    {
        NextButton.onClick.RemoveAllListeners();
        InfoText.text = "";
        foreach (var c in text.ToCharArray())
        {
            InfoText.text += c;
            yield return new WaitForSeconds(0.01f);
        }

        index++;
        NextButton.onClick.AddListener(NextText);
    }

    private void NextText()
    {
        if (index == 5)
        {
            ColorUtility.TryParseHtmlString("#" + "17171700", out color);
            InfomationPanel.GetComponent<Image>().color = color;
            Panel1.SetActive(false);
            Panel2.SetActive(false);
            FirstInfo.SetActive(true);
            InfomationPanel.GetComponent<Button>().onClick.AddListener(ActiveText);
        }
        else if (index == 7)
        {
            InfomationPanel.SetActive(false);
        }
        else
        {
            StartCoroutine(SetText(infomation[index]));
        }
    }

    private void ActiveText()
    {
        ColorUtility.TryParseHtmlString("#" + "17171790", out color);
        InfomationPanel.GetComponent<Image>().color = color;
        Panel1.SetActive(true);
        Panel2.SetActive(true);
        FirstInfo.SetActive(false);
        InfomationPanel.GetComponent<Button>().onClick.RemoveAllListeners();
        StartCoroutine(SetText(infomation[index]));
    }
}