using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatus : MonoBehaviour
{
    public Animator CostumeAnimator;
    public Animator RidingAnimator;
    public Animator PetAnimator;

    public Image CostumeImage;
    public Image RidingImage;
    public Image PetImage;

    public Text Level;

    public Text InfomationText;

    private string title;

    private int costumeIndex;
    private int ridingIndex;
    private int petIndex;

    private string[] townNames =
    {
        "루어럴", "엔 에이치 타운", "샌딜", "볼케이브", "콜로세움", "블루 홀", "유니티야드", "데드 존", "루인스",
        "드래곤 빌리지", "페어리어", "엠 피 에리어", "이빌 타운", "릴렌틀리스", "사이버 타운"
    };

    private string[] merchantName =
    {
        "잡상인", "떠돌이 상인", "숙련된 상인", "돈의 노예", "돈의 정복자", "행복한 상인",
        "재벌 2세", "만수르", "루어럴의 왕", "샌딜의 왕", "콜로세움의 영웅",
        "세계 최고 부자", "세계정복자", "우주정복자", "상인의 신", "게임 마스터"
    };
    
    public GameObject OnMenu1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OnMenu1.active)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        Level.text = "Lv. " + DataController.Instance.level;

        if (DataController.Instance.level < 1600)
        {
            title = "칭호 : " + merchantName[(int) (DataController.Instance.level / 100)];
        }
        else
        {
            title = "칭호 : " + merchantName[15];
        }

        SetIndex();

        InfomationText.text = title +
                              "\n환생 레벨 : " + DataController.Instance.reverseLevel +
                              " (" + townNames[(int) DataController.Instance.reverseLevel] + ")" +
                              "\n클릭당 골드 : " +
                              DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick) +
                              "\n자동 클릭 속도 : " + 1 / DataController.Instance.reinforceAutoTime + " tap/s" +
                              "\n크리티컬 확률 " + DataController.Instance.criticalPercent + "%" +
                              "\n크리티컬 골드 상승률 : " + DataController.Instance.criticalRising + "배" +
                              "\n피버타임 골드 상승률 : " + DataController.Instance.feverGoldRising + "배";
        
        if (PlayerPrefs.GetFloat("Skin", 0) == 0)
        {
            CostumeImage.sprite = Resources.Load("Costume" + (costumeIndex + 1), typeof(Sprite)) as Sprite;

            if (ridingIndex != -1)
            {
                RidingImage.sprite = Resources.Load("RidingProfile" + (ridingIndex + 1), typeof(Sprite)) as Sprite;
            }
            else
            {
                RidingImage.sprite = Resources.Load("Blank", typeof(Sprite)) as Sprite;
            }

            if (petIndex != -1)
            {
                PetImage.sprite = Resources.Load("PetProfile" + (petIndex + 1), typeof(Sprite)) as Sprite;
            }
            else
            {
                PetImage.sprite = Resources.Load("Blank", typeof(Sprite)) as Sprite;
            }
        }
        else
        {
            CostumeImage.sprite = Resources.Load("Skin/" + PlayerPrefs.GetFloat("Skin", 0) +
                                                 "/Profile", typeof(Sprite)) as Sprite;

            if (ridingIndex != -1)
            {
                RidingImage.sprite = Resources.Load("Skin/" + PlayerPrefs.GetFloat("Skin", 0) +
                                                    "/Riding", typeof(Sprite)) as Sprite;
            }
            else
            {
                RidingImage.sprite = Resources.Load("Blank", typeof(Sprite)) as Sprite;
            }

            if (petIndex != -1)
            {
                PetImage.sprite = Resources.Load("Skin/" + PlayerPrefs.GetFloat("Skin", 0) +
                                                 "/Pet", typeof(Sprite)) as Sprite;
            }
            else
            {
                PetImage.sprite = Resources.Load("Blank", typeof(Sprite)) as Sprite;
            }
        }
        
        if (PlayerPrefs.GetFloat("Skin", 0) == 0)
        {
            if (costumeIndex == 0)
            {
                CostumeAnimator.runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/CharacterImage",
                        typeof(RuntimeAnimatorController));
            }
            else
            {
                CostumeAnimator.runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/CharacterImage " + costumeIndex,
                        typeof(RuntimeAnimatorController));
            }

            if (ridingIndex != -1)
            {
                RidingAnimator.gameObject.SetActive(true);
                RidingAnimator.runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/Riding/RidingImage " + ridingIndex,
                        typeof(RuntimeAnimatorController));
            }
            else
            {
                RidingAnimator.gameObject.SetActive(false);
            }

            if (petIndex != -1)
            {
                PetAnimator.gameObject.SetActive(true);
                PetAnimator.runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/Pet/PetImage " + petIndex,
                        typeof(RuntimeAnimatorController));
            }
            else
            {
                PetAnimator.gameObject.SetActive(false);
            }
        }
        else
        {
            CostumeAnimator.runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0)
                                                                             + "/CharacterImage",
                    typeof(RuntimeAnimatorController));

            if (ridingIndex != -1)
            {
                RidingAnimator.runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0)
                                                                                 + "/RidingImage",
                        typeof(RuntimeAnimatorController));
            }
            else
            {
                RidingAnimator.gameObject.SetActive(false);
            }

            if (petIndex != -1)
            {
                PetAnimator.runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0)
                                                                                 + "/PetImage1",
                        typeof(RuntimeAnimatorController));
            }
            else
            {
                PetAnimator.gameObject.SetActive(false);
            }
        }
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