using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager sharedInstance = null;
    private string _url = "http://leatonm.net/wp-content/uploads/2017/candlepin/getdate.php";
    private string _timeData;
    private string _currentTime;
    private string _currentDate;
    private int serverTime;

    public GameObject OnMenu2;
    public GameObject AttendanceCheckPanel;
    public GameObject OnMenu3;
    public GameObject OfflineCompensationPanel;
    public Text Title;
    public Text Infomation;

    private int[] addDia =
    {
        50, 100, 150, 200, 250, 300, 500
    };


    //make sure there is only one instance of this always.
    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else if (sharedInstance != this)
        {
            Destroy(gameObject);
        }


        DontDestroyOnLoad(gameObject);
    }

    public void RecordTime()
    {
        DataController.Instance.lastPlayTime += 5;
        if (DataController.Instance.lastPlayTime > 86400)
        {
            DataController.Instance.lastPlayTime = 0;
        }

        print("시간 -> sec : " + DataController.Instance.lastPlayTime);
    }

    public IEnumerator OfflineCompensation()
    {
        WebRequest request = WebRequest.Create(_url);
        WebResponse response = request.GetResponse();
        StreamReader stream = new StreamReader(response.GetResponseStream());
        try
        {
            var timeData1 = "";
            timeData1 += stream.ReadLine();
            string[] words = timeData1.Split('/');
            //timerTestLabel.text = www.text;
            Debug.Log("The date is : " + words[0]);
            Debug.Log("The time is : " + words[1]);
            var time = Convert.ToDateTime(words[0].Replace('-', '/') + " " + words[1]);

            var nowTime = float.Parse(TimeSpan.Parse(time.Add(TimeSpan.FromHours(14)).ToString().Split(' ')[1])
                .TotalSeconds
                .ToString());

            var compensationTime = nowTime - DataController.Instance.lastPlayTime;

            if (DataController.Instance.lastPlayTime != 0)
            {
                if (compensationTime < 0)
                {
                    // 다음날로 지났을 때
                    var rewordTime = 86400 - DataController.Instance.lastPlayTime;

                    if (rewordTime >= 3600)
                    {
                        // 2시간 이상 경과
                        OnMenu3.SetActive(true);
                        OfflineCompensationPanel.SetActive(true);
                        OfflineCompensationPanel.GetComponentInChildren<Text>().text =
                            DataController.Instance.FormatGold(((DataController.Instance.goldPerSec *
                                                                 DataController.Instance.plusGoldPerSec *
                                                                 DataController.Instance.skinGoldPerSec *
                                                                 DataController.Instance.reverseGolePerSec
                                                                 * 3600)
                                                                + (DataController.Instance.masterGoldPerClick * 3 *
                                                                   3600)) / 3) + "G 획득!!";

                        DataController.Instance.gold += ((DataController.Instance.goldPerSec *
                                                          DataController.Instance.plusGoldPerSec *
                                                          DataController.Instance.skinGoldPerSec *
                                                          DataController.Instance.reverseGolePerSec
                                                          * 3600)
                                                         + (DataController.Instance.masterGoldPerClick * 3 *
                                                            3600)) / 3;

                        DataController.Instance.compensationGold = ((DataController.Instance.goldPerSec *
                                                                     DataController.Instance.plusGoldPerSec *
                                                                     DataController.Instance.skinGoldPerSec *
                                                                     DataController.Instance.reverseGolePerSec
                                                                     * 3600)
                                                                    + (DataController.Instance.masterGoldPerClick * 3 *
                                                                       3600)) / 3;
                    }
                    else
                    {
                        // 2시간 미만
                        OnMenu3.SetActive(true);
                        OfflineCompensationPanel.SetActive(true);
                        OfflineCompensationPanel.GetComponentInChildren<Text>().text =
                            DataController.Instance.FormatGold(((DataController.Instance.goldPerSec *
                                                                 DataController.Instance.plusGoldPerSec *
                                                                 DataController.Instance.skinGoldPerSec *
                                                                 DataController.Instance.reverseGolePerSec
                                                                 * rewordTime)
                                                                + (DataController.Instance.masterGoldPerClick * 3 *
                                                                   rewordTime)) / 3) + "G 획득!!";

                        DataController.Instance.gold += ((DataController.Instance.goldPerSec *
                                                          DataController.Instance.plusGoldPerSec *
                                                          DataController.Instance.skinGoldPerSec *
                                                          DataController.Instance.reverseGolePerSec
                                                          * rewordTime)
                                                         + (DataController.Instance.masterGoldPerClick * 3 *
                                                            rewordTime)) / 3;

                        DataController.Instance.compensationGold = ((DataController.Instance.goldPerSec *
                                                                     DataController.Instance.plusGoldPerSec *
                                                                     DataController.Instance.skinGoldPerSec *
                                                                     DataController.Instance.reverseGolePerSec
                                                                     * rewordTime)
                                                                    + (DataController.Instance.masterGoldPerClick * 3 *
                                                                       rewordTime)) / 3;
                    }
                }
                else
                {
                    if (compensationTime >= 3600)
                    {
                        // 2시간 이상 경과
                        OnMenu3.SetActive(true);
                        OfflineCompensationPanel.SetActive(true);
                        OfflineCompensationPanel.GetComponentInChildren<Text>().text =
                            DataController.Instance.FormatGold(((DataController.Instance.goldPerSec *
                                                                 DataController.Instance.plusGoldPerSec *
                                                                 DataController.Instance.skinGoldPerSec *
                                                                 DataController.Instance.reverseGolePerSec
                                                                 * 3600)
                                                                + (DataController.Instance.masterGoldPerClick * 3 * 3600
                                                                )) / 3) + "G 획득!!";

                        DataController.Instance.gold += ((DataController.Instance.goldPerSec *
                                                          DataController.Instance.plusGoldPerSec *
                                                          DataController.Instance.skinGoldPerSec *
                                                          DataController.Instance.reverseGolePerSec
                                                          * 3600)
                                                         + (DataController.Instance.masterGoldPerClick * 3 * 3600
                                                         )) / 3;

                        DataController.Instance.compensationGold = ((DataController.Instance.goldPerSec *
                                                                     DataController.Instance.plusGoldPerSec *
                                                                     DataController.Instance.skinGoldPerSec *
                                                                     DataController.Instance.reverseGolePerSec
                                                                     * 3600)
                                                                    + (DataController.Instance.masterGoldPerClick * 3 *
                                                                       3600
                                                                    )) / 3;
                    }
                    else
                    {
                        OnMenu3.SetActive(true);
                        OfflineCompensationPanel.SetActive(true);
                        OfflineCompensationPanel.GetComponentInChildren<Text>().text =
                            DataController.Instance.FormatGold(((DataController.Instance.goldPerSec *
                                                                 DataController.Instance.plusGoldPerSec *
                                                                 DataController.Instance.skinGoldPerSec *
                                                                 DataController.Instance.reverseGolePerSec
                                                                 * compensationTime)
                                                                + (DataController.Instance.masterGoldPerClick * 3 *
                                                                   compensationTime)) / 3) + "G 획득!!";

                        DataController.Instance.gold += ((DataController.Instance.goldPerSec *
                                                          DataController.Instance.plusGoldPerSec *
                                                          DataController.Instance.skinGoldPerSec *
                                                          DataController.Instance.reverseGolePerSec
                                                          * compensationTime)
                                                         + (DataController.Instance.masterGoldPerClick * 3 *
                                                            compensationTime)) / 3;

                        DataController.Instance.compensationGold = ((DataController.Instance.goldPerSec *
                                                                     DataController.Instance.plusGoldPerSec *
                                                                     DataController.Instance.skinGoldPerSec *
                                                                     DataController.Instance.reverseGolePerSec
                                                                     * compensationTime)
                                                                    + (DataController.Instance.masterGoldPerClick * 3 *
                                                                       compensationTime)) / 3;
                    }
                }
            }

            DataController.Instance.lastPlayTime = float.Parse(TimeSpan
                .Parse(time.Add(TimeSpan.FromHours(14)).ToString().Split(' ')[1]).TotalSeconds.ToString());

            print("시간 -> sec : " + DataController.Instance.lastPlayTime);
            InvokeRepeating("RecordTime", 5, 5);
        }
        catch (Exception e)
        {
        }


        yield return null;
    }

    //time fether coroutine
    public IEnumerator getTime()
    {
        WebRequest request = WebRequest.Create(_url);
        WebResponse response = request.GetResponse();
        StreamReader stream = new StreamReader(response.GetResponseStream());
        Debug.Log("==> step 1. Getting info from internet now!");
        try
        {
            Debug.Log("==> step 2. Got the info from internet!");
            _timeData = "";
            _timeData += stream.ReadLine();

            string[] words = _timeData.Split('/');
            //timerTestLabel.text = www.text;
            Debug.Log("The date is : " + words[0]);
            Debug.Log("The time is : " + words[1]);

            var time = Convert.ToDateTime(words[0].Replace('-', '/') + " " + words[1]);

            Debug.Log("The time is : " + time.Add(TimeSpan.FromHours(14)));

            serverTime = int.Parse(time.Add(TimeSpan.FromHours(14)).ToString().Split('/')[1]);

            print("현재 시간 : " + serverTime);

            // 출석체크
            if (DataController.Instance.lastAttendance == 0)
            {
                // 게임을 키고 처음 출석을 할 때
                DataController.Instance.lastAttendance = serverTime;
                DataController.Instance.attendanceIndex++;

                Title.text = "출석 " + DataController.Instance.attendanceIndex + "일차";
                Infomation.text = "다이아 " + addDia[(int) DataController.Instance.attendanceIndex - 1] + "개 획득!!";

                DataController.Instance.dia += addDia[(int) DataController.Instance.attendanceIndex - 1];

                AttendanceCheckPanel.SetActive(true);
                OnMenu2.SetActive(true);
                print("1");
            }
            else
            {
                // 출석체크
                if (serverTime - DataController.Instance.lastAttendance == 1)
                {
                    // 연속 출석체크
                    if (DataController.Instance.attendanceIndex < 7)
                    {
                        // 7일차 이전
                        DataController.Instance.lastAttendance = serverTime;
                        DataController.Instance.attendanceIndex++;

                        Title.text = "출석 " + DataController.Instance.attendanceIndex + "일차";
                        Infomation.text = "다이아 " + addDia[(int) DataController.Instance.attendanceIndex - 1] + "개 획득!!";

                        DataController.Instance.dia += addDia[(int) DataController.Instance.attendanceIndex - 1];
                        print("2");
                    }
                    else
                    {
                        // 7일차 출석까지 한 단계에서는 1일차로 돌아간다
                        DataController.Instance.lastAttendance = serverTime;
                        DataController.Instance.attendanceIndex = 1;

                        Title.text = "출석 " + DataController.Instance.attendanceIndex + "일차";
                        Infomation.text = "다이아 " + addDia[(int) DataController.Instance.attendanceIndex - 1] + "개 획득!!";

                        DataController.Instance.dia += addDia[(int) DataController.Instance.attendanceIndex - 1];
                        print("3");
                    }

                    AttendanceCheckPanel.SetActive(true);
                    OnMenu2.SetActive(true);
                }
                else if (serverTime - DataController.Instance.lastAttendance == 0)
                {
                    // 출석한 당일 연속 접속 (출석체크x)
                    print("4");
                }
                else
                {
                    // 출석한 다음날 연속 접속 안 함
                    DataController.Instance.lastAttendance = serverTime;
                    DataController.Instance.attendanceIndex = 1;

                    Title.text = "출석 " + DataController.Instance.attendanceIndex + "일차";
                    Infomation.text = "다이아 " + addDia[(int) DataController.Instance.attendanceIndex - 1] + "개 획득!!";

                    DataController.Instance.dia += addDia[(int) DataController.Instance.attendanceIndex - 1];

                    AttendanceCheckPanel.SetActive(true);
                    OnMenu2.SetActive(true);
                    print("5");
                }
            }

            // 접속하지 않은 동안 벌린 돈


            //setting current time
            _currentDate = words[0];
            _currentTime = words[1];
        }
        catch (Exception e)
        {
        }

        yield return null;
    }


    //get the current time at startup
    void Start()
    {
        Debug.Log("==> TimeManager script is Ready.");
        StartCoroutine("getTime");
        StartCoroutine("OfflineCompensation");
    }

    //get the current date
    public string getCurrentDateNow()
    {
        return _currentDate;
    }


    //get the current Time
    public string getCurrentTimeNow()
    {
        return _currentTime;
    }
}