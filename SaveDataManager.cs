using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;
using UnityEngine.UI;

public class SaveDataManager : MonoBehaviour
{
    public GameObject OnMenu1;
    public GameObject LoadDataPanel;

    public string xmlData = "";

    private static SaveDataManager instance;

    public static SaveDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveDataManager>();

                if (instance == null)
                {
                    instance = new GameObject("SaveDataManager").AddComponent<SaveDataManager>();
                }
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        
    }

    public void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);

        if (!xmlData.Contains(key + ":f:"))
        {
            xmlData += key + ":f:" + value + ",";
        }
        else
        {
            xmlData = xmlData.Replace(key + ":f:" + xmlData.Replace(key + ":f:", "|").Split('|')[1].Split(',')[0],
                key + ":f:" + value);
        }
    }

    public void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);

        if (!xmlData.Contains(key + ":s:"))
        {
            xmlData += key + ":s:" + value + ",";
        }
        else
        {
            xmlData = xmlData.Replace(key + ":s:" + xmlData.Replace(key + ":s:", "|").Split('|')[1].Split(',')[0],
                key + ":s:" + value);
        }
    }

    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);

        if (!xmlData.Contains(key + ":i:"))
        {
            xmlData += key + ":i:" + value + ",";
        }
        else
        {
            xmlData = xmlData.Replace(key + ":i:" + xmlData.Replace(key + ":i:", "|").Split('|')[1].Split(',')[0],
                key + ":i:" + value);
        }
    }


    public float GetFloat(string key, float first_value)
    {
        var result = PlayerPrefs.GetFloat(key, first_value);

        if (!xmlData.Contains(key + ":f:"))
        {
            xmlData += key + ":f:" + result + ",";
        }

        return result;
    }

    public string GetString(string key, string first_value)
    {
        var result = PlayerPrefs.GetString(key, first_value);

        if (!xmlData.Contains(key + ":s:"))
        {
            xmlData += key + ":s:" + result + ",";
        }

        return result;
    }

    public int GetInt(string key, int first_value)
    {
        var result = PlayerPrefs.GetInt(key, first_value);

        if (!xmlData.Contains(key + ":i:"))
        {
            xmlData += key + ":i:" + result + ",";
        }

        return result;
    }

    public void GetData(string data)
    {
        string[] array = data.Split(',');

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].Contains(":f:"))
            {
                SetFloat(array[i].Replace(":f:", "|").Split('|')[0],
                    float.Parse(array[i].Replace(":f:", "|").Split('|')[1]));
            }
            else if (array[i].Contains(":s:"))
            {
                SetString(array[i].Replace(":s:", "|").Split('|')[0],
                    (array[i].Replace(":s:", "|").Split('|')[1]));
            }
            else if (array[i].Contains(":i:"))
            {
                SetInt(array[i].Replace(":i:", "|").Split('|')[0],
                    int.Parse(array[i].Replace(":i:", "|").Split('|')[1]));
            }
        }

        OnMenu1.SetActive(true);
        LoadDataPanel.SetActive(true);
        LoadDataPanel.GetComponentInChildren<Text>().text = "데이터를 불러왔습니다.\n어플을 재시작 해주세요!";
        LoadDataPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            Application.Quit();
            LoadDataPanel.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        });
    }
}