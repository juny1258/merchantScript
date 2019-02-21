using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.Native.Cwrapper;
using UnityEngine;
using UnityEngine.UI;

public class PlayTimeManager : MonoBehaviour
{
    public GameObject NotificationPanel;
    public Text text;

    public GameObject BackPanel;

    // Use this for initialization
    void Start()
    {
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(isSuccess =>
        {
            if (!isSuccess)
            {
                NotificationPanel.SetActive(true);
                BackPanel.SetActive(true);
                text.text = "로그인에 실패하여\n플레이 시간이 기록되지 않습니다.";
            }
        });

        InvokeRepeating("RecordPlayTime", 30f, 30f);
    }

    private void RecordPlayTime()
    {
        if (Social.localUser.authenticated)
        {
            // login success
            PlayerPrefs.SetFloat("PlayTime", PlayerPrefs.GetFloat("PlayTime", 0) + 30000);
            
            float highScore = PlayerPrefs.GetFloat("PlayTime", 0);
            string leaderBoardId = "CgkI_LDZ7OkMEAIQBQ";

            Social.ReportScore((long) highScore, leaderBoardId, success =>
            {
                if (success)
                {
                    print("Success");
                }
            });
        }
        
        
    }
}