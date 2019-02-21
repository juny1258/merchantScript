using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StatusButton : MonoBehaviour
{
    public GameObject IdPanel;
    public GameObject BackPanel;
    public Text IdText;

    public GameObject NotificationPanel;
    public GameObject OnMenu1;
    public Text NotificationText;

    public Text PVPStatusInfo;

    private DatabaseReference reference;

    private int costumeIndex;
    private int ridingIndex;
    private int petIndex;

    private void Start()
    {
    }

    private void OnEnable()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://api-project-73960775.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SyncButton()
    {
        if (DataController.Instance.playerID == "")
        {
            // 처음 PVP를 시작할 때 아이디 입력 받아야함
            
            IdPanel.SetActive(true);
            BackPanel.SetActive(true);
            
//            var key = reference.Child("user").Push().Key;
//
//            if (Debug.isDebugBuild)
//            {
//                DataController.Instance.player = new User
//                {
//                    name = "test",
//                    score = 1000,
//                    level = DataController.Instance.level,
//                    costumeIndex = costumeIndex,
//                    ridingIndex = ridingIndex,
//                    petIndex = petIndex,
//                    goldPerClick = DataController.Instance.masterGoldPerClick,
//                    autoClick = DataController.Instance.reinforceAutoTime,
//                    criticalPercent = DataController.Instance.criticalPercent,
//                    criticalRising = DataController.Instance.criticalRising
//                };
//            }
//            else
//            {
//                DataController.Instance.player = new User
//                {
//                    name = PlayGamesPlatform.Instance.GetUserDisplayName(),
//                    score = 1000,
//                    level = DataController.Instance.level,
//                    costumeIndex = costumeIndex,
//                    ridingIndex = ridingIndex,
//                    petIndex = petIndex,
//                    goldPerClick = DataController.Instance.masterGoldPerClick,
//                    autoClick = DataController.Instance.reinforceAutoTime,
//                    criticalPercent = DataController.Instance.criticalPercent,
//                    criticalRising = DataController.Instance.criticalRising
//                };
//            }
//
//            var json = JsonUtility.ToJson(DataController.Instance.player);
//
//            print(json);
//
//            reference.Child("user").Child(key).SetRawJsonValueAsync(json).ContinueWith(task =>
//            {
//                if (task.IsCompleted)
//                {
//                    PVPStatusInfo.text = "클릭당 골드 : " +
//                                         DataController.Instance.FormatGold(DataController.Instance
//                                             .masterGoldPerClick) +
//                                         "\n자동 클릭 속도 : " + 1 / DataController.Instance.reinforceAutoTime + " tap/s" +
//                                         "\n크리티컬 확률 " + DataController.Instance.criticalPercent + "%" +
//                                         "\n크리티컬 골드 상승률 : " + DataController.Instance.criticalRising + "배" +
//                                         "\n피버타임 골드 상승률 : " + DataController.Instance.feverGoldRising + "배";
//
//                    NotificationPanel.SetActive(true);
//                    NotificationText.text = "동기화 완료";
//                }
//                else if (task.IsFaulted)
//                {
//                    NotificationPanel.SetActive(true);
//                    NotificationText.text = task.Exception.ToString();
//                }
//            });
        }
        else
        {
            // 데이터가 있는 상태에서 데이터 동기화
            reference.Child("user").Child(DataController.Instance.playerID).Child("score").GetValueAsync().ContinueWith(
                task =>
                {
                    if (task.IsCompleted)
                    {
                        SetIndex();
                        
                        DataController.Instance.player = new User
                        {
                            name = DataController.Instance.player.name,
                            score = int.Parse(task.Result.Value.ToString()),
                            level = DataController.Instance.level,
                            costumeIndex = costumeIndex,
                            ridingIndex = ridingIndex,
                            petIndex = petIndex,
                            goldPerClick = DataController.Instance.masterGoldPerClick,
                            autoClick = DataController.Instance.reinforceAutoTime,
                            criticalPercent = DataController.Instance.criticalPercent,
                            criticalRising = DataController.Instance.criticalRising,
                            feverRising = DataController.Instance.feverGoldRising
                        };

                        var json = JsonUtility.ToJson(DataController.Instance.player);

                        print(json);

                        reference.Child("user").Child(DataController.Instance.playerID).SetRawJsonValueAsync(json)
                            .ContinueWith(task1 =>
                            {
                                if (task1.IsCompleted)
                                {
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "동기화 완료";
                                    DataChangeEvent.Instance.SyncPvpData();
                                }
                            });
                    }
                    else if (task.IsFaulted)
                    {
                        OnMenu1.SetActive(true);
                        NotificationPanel.SetActive(true);
                        NotificationText.text = task.Exception.ToString();
                    }
                });
        }
    }

    public void SetNameButton()
    {
        if (IdText.text != "")
        {
            // 닉네임을 입력하고 최초 데이터 세팅
            var key = reference.Child("user").Push().Key;
            
            SetIndex();

            DataController.Instance.player = new User
            {
                name = IdText.text,
                score = 1000,
                level = DataController.Instance.level,
                costumeIndex = costumeIndex,
                ridingIndex = ridingIndex,
                petIndex = petIndex,
                goldPerClick = DataController.Instance.masterGoldPerClick,
                autoClick = DataController.Instance.reinforceAutoTime,
                criticalPercent = DataController.Instance.criticalPercent,
                criticalRising = DataController.Instance.criticalRising,
                feverRising = DataController.Instance.feverGoldRising
            };

            var json = JsonUtility.ToJson(DataController.Instance.player);

            print(json);

            reference.Child("user").Child(key).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    PVPStatusInfo.text = "클릭당 골드 : " +
                                         DataController.Instance.FormatGold(DataController.Instance
                                             .masterGoldPerClick) +
                                         "\n자동 클릭 속도 : " + 1 / DataController.Instance.reinforceAutoTime + " tap/s" +
                                         "\n크리티컬 확률 " + DataController.Instance.criticalPercent + "%" +
                                         "\n크리티컬 골드 상승률 : " + DataController.Instance.criticalRising + "배" +
                                         "\n피버타임 골드 상승률 : " + DataController.Instance.feverGoldRising + "배";

                    DataController.Instance.playerID = key;
                    
                    DataChangeEvent.Instance.SyncPvpData();

                    BackPanel.SetActive(false);
                    OnMenu1.SetActive(true);
                    NotificationPanel.SetActive(true);
                    NotificationText.text = "동기화 완료";
                    IdPanel.SetActive(false);
                }
                else if (task.IsFaulted)
                {
                    IdPanel.SetActive(false);
                    BackPanel.SetActive(false);
                    OnMenu1.SetActive(true);
                    NotificationPanel.SetActive(true);
                    NotificationText.text = task.Exception.ToString();
                }
            });
        }
    }

    public void StartPvp()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }

    private void SetIndex()
    {
        var i = 0;
        while (true)
        {
            if (DataController.Instance.GetCostumeInfo(i) == 2)
            {
                costumeIndex = i;
                break;
            }

            i++;
        }


        int j = 0;
        while (true)
        {
            if (PlayerPrefs.GetFloat("Riding_" + j, 0) == 0)
            {
                ridingIndex = -1;
                break;
            }

            if (PlayerPrefs.GetFloat("Riding_" + j, 0) == 2)
            {
                ridingIndex = j;
                break;
            }

            j++;
        }

        j = 0;

        while (true)
        {
            if (PlayerPrefs.GetFloat("Pet_" + j, 0) == 0)
            {
                petIndex = -1;
                break;
            }

            if (PlayerPrefs.GetFloat("Pet_" + j, 0) == 2)
            {
                petIndex = j;
                break;
            }

            j++;
        }
    }
}