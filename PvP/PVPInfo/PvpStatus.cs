using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

public class PvpStatus : MonoBehaviour
{
    public GameObject InfomationPanel;
    
    public Animator CostumeAnimator;
    public Animator RidingAnimator;
    public Animator PetAnimator;

    public Image CostumeImage;
    public Image RidingImage;
    public Image PetImage;

    public Text Level;

    public Text InfomationText;

    private DatabaseReference reference;

    private float costumeIndex;
    private float ridingIndex;
    private float petIndex;

    private void OnEnable()
    {
        DataChangeEvent.SyncPvpDataEvent += SetPanel;
        
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://api-project-73960775.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        SetPanel();
        
    }

    private void SetPanel()
    {
        if (DataController.Instance.playerID != "")
        {
            reference.Child("user").Child(DataController.Instance.playerID).GetValueAsync().ContinueWith(task =>
            {
                DataController.Instance.player = JsonUtility.FromJson<User>(task.Result.GetRawJsonValue());
                
                Level.text = "Lv. " + DataController.Instance.player.level;

                InfomationText.text = "클릭당 골드 : " +
                                      DataController.Instance.FormatGold(DataController.Instance.player.goldPerClick) +
                                      "\n자동 클릭 속도 : " + 1 / DataController.Instance.player.autoClick + " tap/s" +
                                      "\n크리티컬 확률 " + DataController.Instance.player.criticalPercent + "%" +
                                      "\n크리티컬 골드 상승률 : " + DataController.Instance.player.criticalRising + "배" +
                                      "\n피버타임 골드 상승률 : " + DataController.Instance.player.feverRising + "배";

                costumeIndex = DataController.Instance.player.costumeIndex;
                ridingIndex = DataController.Instance.player.ridingIndex;
                petIndex = DataController.Instance.player.petIndex;
        
                SetImage();
            });
        }
        else
        {
            InfomationPanel.SetActive(true);
        }
    }
    
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

    private void SetImage()
    {
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
}