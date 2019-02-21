using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    private static BackgroundManager instance;

    public static BackgroundManager Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<BackgroundManager>();

            return instance;
        }
    }

    public GameObject[] Background;

    public GameObject Tile;
    public GameObject Back;

    private float[] plusGoldPerClick = {
        2f, 3f, 5f, 10f, 30f,
        50, 100, 200, 400, 1200,
        3000, 5000f, 50000, 200000f,
        500000f
    };

    public GameObject LevelUpPanel;
    public Image medalImage;
    public Text LevelText;
    public Text PlusGoldText;

    public Image medalImage2;
    public Text NextMedal;

    public GameObject BackPanel;

    private void OnEnable()
    {
        DataChangeEvent.PurchaseBackgroundEvent += setBackground;

        DataChangeEvent.ChangeSkinEvent += setBackground;

        DataChangeEvent.ResetDataEvent += setBackground;
        DataChangeEvent.ResetDataEvent += () =>
        {
            var level = (int) (DataController.Instance.level / 100);
            if (level < 15)
            {
                medalImage2.sprite = Resources.Load("Medal" + (level+1), typeof(Sprite)) as Sprite;
                NextMedal.text = "X " + plusGoldPerClick[level];
            } 
            else if (level >= 15)
            {
                medalImage2.sprite = Resources.Load("Medal15", typeof(Sprite)) as Sprite;
                NextMedal.text = "X " + plusGoldPerClick[14];
            }
        };
        
        var index = (int) (DataController.Instance.level / 100);
        if (index < 15)
        {
            medalImage2.sprite = Resources.Load("Medal" + (index+1), typeof(Sprite)) as Sprite;
            NextMedal.text = "X " + plusGoldPerClick[index];
        } 
        else if (index >= 15)
        {
            medalImage2.sprite = Resources.Load("Medal15", typeof(Sprite)) as Sprite;
            NextMedal.text = "X " + plusGoldPerClick[14];
        }
    }

    public void LevelUp(string name)
    {
        var index = (int) (DataController.Instance.level / 100);

        if (index <= 15)
        {
            if (index < 15)
            {
                LevelText.text = "칭호 \n\'" + name + "\' 획득!!";
                PlusGoldText.text = "클릭당 골드 + " + plusGoldPerClick[(int) (DataController.Instance.level / 100) - 1] + "배";
                
                medalImage.sprite = Resources.Load("Medal" + index, typeof(Sprite)) as Sprite;
                medalImage2.sprite = Resources.Load("Medal" + (index+1), typeof(Sprite)) as Sprite;
                NextMedal.text = "X " + plusGoldPerClick[index];
                
                DataController.Instance.levelGoldPerClick += plusGoldPerClick[(int) (DataController.Instance.level / 100) - 1];
                LevelUpPanel.SetActive(true);
                BackPanel.SetActive(true);
        
                DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                             DataController.Instance.levelGoldPerClick *
                                                             DataController.Instance.skillGoldPerClick *
                                                             DataController.Instance.plusGoldPerClick *
                                                             DataController.Instance.collectionGoldPerClick *
                                                             DataController.Instance.reinforceGoldPerClick * 
                                                             DataController.Instance.skinGoldPerClick *
                                                             DataController.Instance.reverseGolePerClick;
            }
            else
            {
                LevelText.text = "칭호 \n\'" + name + "\' 획득!!";
                PlusGoldText.text = "클릭당 골드 + " + plusGoldPerClick[(int) (DataController.Instance.level / 100) - 1] + "배";
                
                medalImage.sprite = Resources.Load("Medal15", typeof(Sprite)) as Sprite;
                
                DataController.Instance.levelGoldPerClick += plusGoldPerClick[(int) (DataController.Instance.level / 100) - 1];
                LevelUpPanel.SetActive(true);
                BackPanel.SetActive(true);
        
                DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                             DataController.Instance.levelGoldPerClick *
                                                             DataController.Instance.skillGoldPerClick *
                                                             DataController.Instance.plusGoldPerClick *
                                                             DataController.Instance.collectionGoldPerClick *
                                                             DataController.Instance.reinforceGoldPerClick * 
                                                             DataController.Instance.skinGoldPerClick *
                                                             DataController.Instance.reverseGolePerClick;
            }
        }
        else
        {
            
        }
    }

    public void setBackground()
    {
        Tile.GetComponent<Image>().sprite = Resources.Load("Background/" + PlayerPrefs.GetFloat("WearingTown", 0)
                                                                         + "/Tile", typeof(Sprite)) as Sprite;
        Back.GetComponent<Image>().sprite = Resources.Load("Background/" + PlayerPrefs.GetFloat("WearingTown", 0)
                                                                         + "/Back", typeof(Sprite)) as Sprite;

        if (PlayerPrefs.GetFloat("BackgroundIndex", 0) == 0)
        {
            for (int i = 0; i <= 8; i++)
            {
                Background[i].SetActive(false);
            }
        }
        
        for (int i = 0; i < PlayerPrefs.GetFloat("BackgroundIndex", 0); i++)
        {
            // 폭죽
            if (i == 8)
            {
                if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
                {
                    Background[i].SetActive(false);
                }
                else
                {
                    Background[i].SetActive(true);
                }
            }
            // 재봉사
            else if (i == 3)
            {
                if (PlayerPrefs.GetFloat("Skin", 0) == 0)
                {
                    if (PlayerPrefs.GetFloat("WearingTown", 0) == 0)
                    {
                        // 환생 전
                        Background[i].GetComponentInChildren<Animator>().runtimeAnimatorController =
                            (RuntimeAnimatorController) Resources.Load("Animation/TailorImage", typeof(RuntimeAnimatorController));
                        Background[i].SetActive(true);
                    }
                    else
                    {
                        // 환생 레벨 1 이상
                        Background[i].GetComponentInChildren<Animator>().runtimeAnimatorController =
                            (RuntimeAnimatorController) Resources.Load("Animation/TailorImage " + 
                                                                       PlayerPrefs.GetFloat("WearingTown", 0), typeof(RuntimeAnimatorController));
                        Background[i].SetActive(true);
                    }
                }
                else
                {
                    // 스킨 착용
                    Background[i].GetComponentInChildren<Animator>().runtimeAnimatorController =
                        (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + PlayerPrefs.GetFloat("Skin", 0) + 
                            "/TailorImage", typeof(RuntimeAnimatorController));
                    Background[i].SetActive(true);
                }
            }
            // 대장장이
            else if (i == 4)
            {
                if (PlayerPrefs.GetFloat("Skin", 0) == 0)
                {
                    if (PlayerPrefs.GetFloat("WearingTown", 0) == 0)
                    {
                        // 환생 전
                        Background[i].GetComponentInChildren<Animator>().runtimeAnimatorController =
                            (RuntimeAnimatorController) Resources.Load("Animation/BlackSmithImage",
                                typeof(RuntimeAnimatorController));
                        Background[i].SetActive(true);
                    }
                    else
                    {
                        // 환생 레벨 1 이상
                        Background[i].GetComponentInChildren<Animator>().runtimeAnimatorController =
                            (RuntimeAnimatorController) Resources.Load("Animation/BlackSmithImage " + 
                                                                       PlayerPrefs.GetFloat("WearingTown", 0),
                                typeof(RuntimeAnimatorController));
                        Background[i].SetActive(true);
                    }
                }
                else
                {
                    // 스킨 착용
                    Background[i].GetComponentInChildren<Animator>().runtimeAnimatorController =
                        (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + PlayerPrefs.GetFloat("Skin", 0) + 
                                                                   "/BlackSmithImage", typeof(RuntimeAnimatorController));
                    Background[i].SetActive(true);
                }
            } else if (i == 6 && PlayerPrefs.GetFloat("WearingTown", 0) == 0)
            {
                if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 15)
                {
                    Background[i].GetComponent<Image>().sprite = Resources.Load("Lamp1", typeof(Sprite)) as Sprite;
                }
                else if (DateTime.Now.Hour >= 15 && DateTime.Now.Hour < 19)
                {
                    Background[i].GetComponent<Image>().sprite = Resources.Load("Lamp1", typeof(Sprite)) as Sprite;
                }
                else
                {
                    Background[i].GetComponent<Image>().sprite = Resources.Load("Lamp2", typeof(Sprite)) as Sprite;
                }
                Background[i].SetActive(true);
            }
            // 나머지
            else
            {
                Background[i].GetComponent<Image>().sprite = Resources.Load("Background/" + PlayerPrefs.GetFloat("WearingTown", 0)
                                                                            + "/" + i, typeof(Sprite)) as Sprite;
                Background[i].SetActive(true);
            }
        }
    }
}