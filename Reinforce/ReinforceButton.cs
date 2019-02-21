using GooglePlayGames.Native.Cwrapper;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class ReinforceButton : MonoBehaviour
{
    public Text Button3Text;

    public Text Button1Price;
    public Text Button2Price;
    public Text UseReversePoint;

    public Text DiaText;

    public GameObject ReinforcePanel;
    public GameObject Success;
    public GameObject Fail;

    private Random rand;

    private float price = 100000f;

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


    private void OnEnable()
    {
        DataChangeEvent.Instance.OpenMenu();
        DataController.Instance.reinforceIsPurchase = false;
        if (name.Contains("Reinforce1"))
        {
            Button1Price.text = DataController.Instance.FormatGold(price * Mathf.Pow(4.6f,
                                                                       (int) PlayerPrefs.GetFloat("ReinforceLevel", 0)))
                                + "G";
        }

        if (name.Contains("Reinforce2"))
        {
            Button2Price.text = DataController.Instance.FormatGold(price * 5 * Mathf.Pow(4.6f,
                                                                       (int) PlayerPrefs.GetFloat("ReinforceLevel", 0)))
                                + "G";
        }

        if (name.Contains("UseDia"))
        {
            DiaText.text = "" + (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) * 10;
            if (DataController.Instance.reinforceIsPurchase)
            {
                // 아이템 구매
                Button3Text.text = "ON";
                Button3Text.color = new Color(0.09f, 0.1647f, 0.9255f);
            }
            else
            {
                // 아이템 구매 취소
                Button3Text.text = "OFF";
                Button3Text.color = new Color(0.9255f, 0.09f, 0.09f);
            }
        }

        if (name.Contains("UseReversePoint"))
        {
            if ((int)PlayerPrefs.GetFloat("ReinforceLevel", 0) == 0)
            {
                UseReversePoint.text = "1레벨 이상 사용 가능";
            }
            else
            {
                UseReversePoint.text = "강화하기 ( 환생포인트 " + PlayerPrefs.GetFloat("ReinforceLevel", 0) * 100 + "필요 )";
            }
        }
    }

    private void Start()
    {
        rand = new Random();
    }

    public void ReversePointReinforce()
    {
        if ((int)PlayerPrefs.GetFloat("ReinforceLevel", 0) != 0 && PlayerPrefs.GetFloat("ReinforceLevel", 0) * 100 <= DataController.Instance.reversePoint)
        {
            DataController.Instance.reversePoint -= PlayerPrefs.GetFloat("ReinforceLevel", 0) * 100;
            
            ReinforcePanel.SetActive(true);
            Success.SetActive(true);
            
            PlayerPrefs.SetFloat("ReinforceLevel", PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1);
            DataController.Instance.reinforceGoldPerClick +=
                ReinforceGoldPerClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
            DataController.Instance.reinforceAutoTime =
                ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
            
            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick * 
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;

            DataChangeEvent.Instance.Reinforce();
        }
        
        DataController.Instance.SaveData();
    }

    public void Button1()
    {
        // 강화 30퍼센트
        if (DataController.Instance.PurchaseGold(price * Mathf.Pow(4.6f,
                                                     (int) PlayerPrefs.GetFloat("ReinforceLevel", 0)))
            <= DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * Mathf.Pow(4.6f,
                                                                                     (int) PlayerPrefs.GetFloat("ReinforceLevel", 0)));
            // 랜덤으로 1~100까지의 수 선정
            var index = rand.Next(1, 101);
            // 뽑힌 값 중 30 이하일 때 성공
            if (index <= 30)
            {
                // 강화 성공 패널 액티브
                ReinforcePanel.SetActive(true);
                Success.SetActive(true);
                // 강화 성공
                PlayerPrefs.SetFloat("ReinforceLevel", PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1);
                DataController.Instance.reinforceGoldPerClick +=
                    ReinforceGoldPerClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
                DataController.Instance.reinforceAutoTime =
                    ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
            }
            else
            {
                // 강화 실패 패널 액티브
                ReinforcePanel.SetActive(true);
                Fail.SetActive(true);
                if (DataController.Instance.reinforceIsPurchase)
                {
                    // 강화 터짐 방지
                }
                else
                {
                    // 강화 터짐 방지 x
                    if (PlayerPrefs.GetFloat("ReinforceLevel", 0) >= 1)
                    {
                        DataController.Instance.reinforceGoldPerClick -=
                            ReinforceGoldPerClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
                        PlayerPrefs.SetFloat("ReinforceLevel", PlayerPrefs.GetFloat("ReinforceLevel", 0) - 1);
                        DataController.Instance.reinforceAutoTime =
                            ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
                    }
                }
            }

            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick * 
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;

            DataChangeEvent.Instance.Reinforce();
            DataController.Instance.reinforceIsPurchase = false;

            DataController.Instance.SaveData();
        }
    }

    public void Button2()
    {
        // 고급 강화 확률 70퍼센트
        if (DataController.Instance.PurchaseGold(price * 5 * Mathf.Pow(4.6f,
                                                     (int) PlayerPrefs.GetFloat("ReinforceLevel", 0)))
            <= DataController.Instance.gold)
        {
            DataController.Instance.gold -= DataController.Instance.PurchaseGold(price * 5 * Mathf.Pow(4.6f,
                                                                                     (int) PlayerPrefs.GetFloat(
                                                                                         "ReinforceLevel", 0)));
            // 랜덤으로 1~100까지의 수 선정
            var index = rand.Next(1, 101);
            // 뽑힌 값 중 70 이하일 때 성공
            if (index <= 70)
            {
                // 강화 성공 패널 액티브
                ReinforcePanel.SetActive(true);
                Success.SetActive(true);
                // 강화 성공
                PlayerPrefs.SetFloat("ReinforceLevel", PlayerPrefs.GetFloat("ReinforceLevel", 0) + 1);
                DataController.Instance.reinforceGoldPerClick +=
                    ReinforceGoldPerClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
                DataController.Instance.reinforceAutoTime =
                    ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
            }
            else
            {
                // 강화 실패 패널 액티브
                ReinforcePanel.SetActive(true);
                Fail.SetActive(true);

                if (DataController.Instance.reinforceIsPurchase)
                {
                    // 강화 터짐 방지
                }
                else
                {
                    // 강화 터짐 방지 x
                    if (PlayerPrefs.GetFloat("ReinforceLevel", 0) >= 1)
                    {
                        DataController.Instance.reinforceGoldPerClick -=
                            ReinforceGoldPerClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
                        PlayerPrefs.SetFloat("ReinforceLevel", PlayerPrefs.GetFloat("ReinforceLevel", 0) - 1);
                        DataController.Instance.reinforceAutoTime =
                            ReinforceAutoClick[(int) PlayerPrefs.GetFloat("ReinforceLevel", 0)];
                    }
                }
            }

            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick * 
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;

            DataChangeEvent.Instance.Reinforce();
            DataController.Instance.reinforceIsPurchase = false;
        }
        
        DataController.Instance.SaveData();
    }

    public void Button3()
    {
        if (!DataController.Instance.reinforceIsPurchase)
        {
            // 아이템 구매
            if (DataController.Instance.dia >= (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) * 10)
            {
                DataController.Instance.dia -= (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) * 10;
                Button3Text.text = "ON";
                Button3Text.color = new Color(0.09f, 0.1647f, 0.9255f);
                DataController.Instance.reinforceIsPurchase = true;
            }
        }
        else
        {
            // 아이템 구매 취소
            DataController.Instance.dia += (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) * 10;
            Button3Text.text = "OFF";
            Button3Text.color = new Color(0.9255f, 0.09f, 0.09f);
            DataController.Instance.reinforceIsPurchase = false;
        }
    }

    private void OnDisable()
    {
        if (DataController.Instance.reinforceIsPurchase)
        {
            DataController.Instance.dia += (int) PlayerPrefs.GetFloat("ReinforceLevel", 0) * 10;
            DataController.Instance.reinforceIsPurchase = false;
        }
    }
}