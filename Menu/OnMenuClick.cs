using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;
using UnityEngine.UI;

public class OnMenuClick : MonoBehaviour
{
    public RectTransform menuPanel;
    public RectTransform menuBackground;

    public GameObject MessageBox;
    public Text MessageText;
    public GameObject BackPanel;

    public void OpenMenu()
    {
        if (name.Contains("Trading") && PlayerPrefs.GetFloat("Town_0", 0) == 0)
        {
            MessageBox.SetActive(true);
            BackPanel.SetActive(true);
            MessageText.text = "마을을 구매한 후에\n무역을 할 수 있습니다";
        }
        else if (name.Contains("Save"))
        {
            MessageBox.SetActive(true);
            BackPanel.SetActive(true);
            MessageText.text = "준비중입니다!!";
        }
        else if (name.Equals("StatusBtn"))
        {
            menuPanel.gameObject.SetActive(true);
            if (PlayerPrefs.GetFloat("FirstStatusInfomation", 0) == 0)
            {
                PlayerPrefs.SetFloat("FirstStatusInfomation", 1);
                GameObject.Find("Finger").SetActive(false);
            }
        }
        else if (name.Equals("PvpStatusBtn"))
        {
            menuPanel.gameObject.SetActive(true);
        }
        else
        {
            menuPanel.gameObject.SetActive(true);
            menuBackground.gameObject.SetActive(true);
        }

        DataChangeEvent.Instance.OpenMenu();
    }

    public void OKButton()
    {
        MessageBox.SetActive(false);
        BackPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        menuPanel.gameObject.SetActive(false);
        menuBackground.gameObject.SetActive(false);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}