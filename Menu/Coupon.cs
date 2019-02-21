using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

public class Coupon : MonoBehaviour
{
    public Text InputText;

    public GameObject OnMenu1;
    public GameObject NotificationPanel;
    public Text NotificationText;

    public GameObject OnMenu2;
    public GameObject UseCouponPnael;
    public Text CouponText;

    private DatabaseReference couponReference;

    public static TimeManager sharedInstance = null;
    private string _url = "http://leatonm.net/wp-content/uploads/2017/candlepin/getdate.php";
    private string _timeData;
    private string _currentTime;
    private string _currentDate;
    private int serverTime;

    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://api-project-73960775.firebaseio.com/");
        couponReference = FirebaseDatabase.DefaultInstance.RootReference.Child("coupon");
    }

    public void OKButton()
    {
        if (InputText.text.Contains("MerchantCoupon"))
        {
            if (PlayerPrefs.GetFloat("OverlapCoupon", 0) == 0)
            {
                DataController.Instance.dia += 500;
                OnMenu2.SetActive(true);
                UseCouponPnael.SetActive(true);
                CouponText.text = "다이아 500개 획득!!";
                PlayerPrefs.SetFloat("OverlapCoupon", 1);
            }
            else
            {
                OnMenu1.SetActive(true);
                NotificationPanel.SetActive(true);
                NotificationText.text = "이미 사용한 쿠폰입니다.";
            }
        }
        else if (InputText.text.Contains("startpakage_"))
        {
            couponReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // fail
                    OnMenu1.SetActive(true);
                    NotificationPanel.SetActive(true);
                    NotificationText.text = task.Exception.Message;
                }
                else if (task.IsCompleted)
                {
                    // success
                    DataSnapshot dataSnapshot = task.Result;

                    foreach (var coupon in dataSnapshot.Children)
                    {
                        if (coupon.Key.Contains(InputText.text))
                        {
                            if (int.Parse(coupon.Value.ToString()) == 1)
                            {
                                if (PlayerPrefs.GetFloat("NoAds", 0) == 0)
                                {
                                    coupon.Reference.SetValueAsync(0);
                                    PlayerPrefs.SetFloat("NoAds", 1);
                                    DataController.Instance.dia += 3000;
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "아이템이 지급되었습니다.";

                                    DataController.Instance.dia += 300;
                                }
                                else
                                {
                                    coupon.Reference.SetValueAsync(2);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "이미 아이템을 소유하고 계십니다.";
                                }
                            }
                            else
                            {
                                OnMenu1.SetActive(true);
                                NotificationPanel.SetActive(true);
                                NotificationText.text = "이미 사용한 쿠폰입니다.";
                            }
                        }
                    }
                }
            });
        }
        else if (InputText.text.Contains("NoAds"))
        {
            couponReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // fail
                    OnMenu1.SetActive(true);
                    NotificationPanel.SetActive(true);
                    NotificationText.text = task.Exception.Message;
                }
                else if (task.IsCompleted)
                {
                    // success
                    DataSnapshot dataSnapshot = task.Result;

                    foreach (var coupon in dataSnapshot.Children)
                    {
                        if (coupon.Key.Contains(InputText.text))
                        {
                            if (int.Parse(coupon.Value.ToString()) == 1)
                            {
                                if (PlayerPrefs.GetFloat("NoAds", 0) == 0)
                                {
                                    coupon.Reference.SetValueAsync(0);
                                    PlayerPrefs.SetFloat("NoAds", 1);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "아이템이 지급되었습니다.";

                                    DataController.Instance.dia += 300;
                                }
                                else
                                {
                                    coupon.Reference.SetValueAsync(2);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "이미 아이템을 소유하고 계십니다.";
                                }
                            }
                            else
                            {
                                OnMenu1.SetActive(true);
                                NotificationPanel.SetActive(true);
                                NotificationText.text = "이미 사용한 쿠폰입니다.";
                            }
                        }
                    }
                }
            });
        }
        else if (InputText.text.Contains("AutoItem"))
        {
            couponReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // fail
                    OnMenu1.SetActive(true);
                    NotificationPanel.SetActive(true);
                    NotificationText.text = task.Exception.Message;
                }
                else if (task.IsCompleted)
                {
                    // success
                    DataSnapshot dataSnapshot = task.Result;

                    foreach (var coupon in dataSnapshot.Children)
                    {
                        if (coupon.Key.Contains(InputText.text))
                        {
                            if (int.Parse(coupon.Value.ToString()) == 1)
                            {
                                if (PlayerPrefs.GetFloat("AutoItem", 0) == 0)
                                {
                                    coupon.Reference.SetValueAsync(0);
                                    PlayerPrefs.SetFloat("AutoItem", 1);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "아이템이 지급되었습니다.";

                                    DataController.Instance.dia += 300;
                                }
                                else
                                {
                                    coupon.Reference.SetValueAsync(2);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "이미 아이템을 소유하고 계십니다.";
                                }
                            }
                            else
                            {
                                OnMenu1.SetActive(true);
                                NotificationPanel.SetActive(true);
                                NotificationText.text = "이미 사용한 쿠폰입니다.";
                            }
                        }
                    }
                }
            });
        }
        else if (InputText.text.Contains("skinpakage_"))
        {
            couponReference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // fail
                    OnMenu1.SetActive(true);
                    NotificationPanel.SetActive(true);
                    NotificationText.text = task.Exception.Message;
                }
                else if (task.IsCompleted)
                {
                    // success
                    DataSnapshot dataSnapshot = task.Result;

                    foreach (var coupon in dataSnapshot.Children)
                    {
                        if (coupon.Key.Contains(InputText.text))
                        {
                            if (int.Parse(coupon.Value.ToString()) >= 3 && int.Parse(coupon.Value.ToString()) <= 6)
                            {
                                var index = int.Parse(coupon.Value.ToString()) - 2;
                                if (PlayerPrefs.GetFloat("Skin_" + index, 0) == 0)
                                {
                                    coupon.Reference.SetValueAsync(0);
                                    PlayerPrefs.SetFloat("Skin_" + index, 1);
                                    DataController.Instance.skinGoldPerClick += 5;
                                    DataController.Instance.skinGoldPerSec += 5;

                                    DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                                                 DataController.Instance
                                                                                     .levelGoldPerClick *
                                                                                 DataController.Instance
                                                                                     .skillGoldPerClick *
                                                                                 DataController.Instance
                                                                                     .plusGoldPerClick *
                                                                                 DataController.Instance
                                                                                     .collectionGoldPerClick *
                                                                                 DataController.Instance
                                                                                     .reinforceGoldPerClick *
                                                                                 DataController.Instance
                                                                                     .skinGoldPerClick *
                                                                                 DataController.Instance
                                                                                     .reverseGolePerClick;

                                    PlayerPrefs.SetFloat("RandomBoxCount",
                                        PlayerPrefs.GetFloat("RandomBoxCount", 0) + 10);
                                    DataController.Instance.dia += 3000;


                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "아이템이 지급되었습니다.";
                                }
                                else
                                {
                                    coupon.Reference.SetValueAsync(2);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "이미 아이템을 소유하고 계십니다.";
                                }
                            }
                            else if (int.Parse(coupon.Value.ToString()) == 7)
                            {
                                var index = int.Parse(coupon.Value.ToString()) - 2;
                                if (PlayerPrefs.GetFloat("Skin_" + index, 0) == 0)
                                {
                                    coupon.Reference.SetValueAsync(0);
                                    PlayerPrefs.SetFloat("Skin_" + index, 1);
                                    
                                    DataController.Instance.criticalPercent += 20;
                                    DataController.Instance.criticalRising += 2;

                                    DataController.Instance.criticalPer = DataController.Instance.criticalPercent;

                                    PlayerPrefs.SetFloat("RandomBoxCount",
                                        PlayerPrefs.GetFloat("RandomBoxCount", 0) + 10);
                                    DataController.Instance.dia += 3000;


                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "아이템이 지급되었습니다.";
                                }
                                else
                                {
                                    coupon.Reference.SetValueAsync(2);
                                    OnMenu1.SetActive(true);
                                    NotificationPanel.SetActive(true);
                                    NotificationText.text = "이미 아이템을 소유하고 계십니다.";
                                }
                            }
                            else
                            {
                                OnMenu1.SetActive(true);
                                NotificationPanel.SetActive(true);
                                NotificationText.text = "이미 사용한 쿠폰입니다.";
                            }
                        }
                    }
                }
            });
        }
        else
        {
            OnMenu1.SetActive(true);
            NotificationPanel.SetActive(true);
            NotificationText.text = "존재하지 않는 쿠폰입니다.";
        }
    }
}