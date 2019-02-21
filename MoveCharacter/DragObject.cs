using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class DragObject : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Button reverseButton;

    public float heightBalance;

    public float x, y, z;

    public CanvasScaler canvasScaler;

    public Random random;
    public int randomInt;

    private float positionX, positionY;

    public Vector3 characterPosition;

    private void Awake()
    {
        characterPosition = new Vector3();

        DataController.Instance.LoadObjectPosition(this);

        random = new Random();
    }

    private void OnEnable()
    {
        heightBalance = Screen.height / 3.38266f + Screen.height / 1.60965f / 2 - Screen.height / 2;

        DataChangeEvent.TabScreen += Tab;

        DataChangeEvent.ResetPosition += ResetPosition;

        DataChangeEvent.ChangeSkinEvent += SetActivePanel;

        DataChangeEvent.ResetDataEvent += SetActivePanel;

        if (name == "Character")
        {
            DataChangeEvent.CostumeChange += CostumeChanged;
        }

        if (name == "Riding")
        {
            DataChangeEvent.RidingChange += RidingChanged;
        }

        if (name == "Pet")
        {
            DataChangeEvent.PetChange += PetChanged;
        }

        if (name == "Tailor")
        {
            StartCoroutine(StartDia1());
        }

        if (name == "BlackSmith")
        {
            StartCoroutine(StartDia2());
        }

        positionX = (Screen.width >> 1) + Screen.width / (900f / x);
        positionY = (Screen.height >> 1) + heightBalance + Screen.height / (1600f / y);
    }

    private IEnumerator StartDia1()
    {
        while (true)
        {
            if (PlayerPrefs.GetFloat("Background_3", 0) == 1)
            {
                characterPosition.x = transform.position.x;
                characterPosition.y = transform.position.y + Screen.height * 0.095f;
                characterPosition.z = transform.position.z;

                var randInt = random.Next(1, 200);
                if (randInt > 195)
                {
                    CombatDiaManager.Instance.CreateDia(characterPosition);
                }
                else if (randInt <= DataController.Instance.plusCollectionDrop)
                {
                    randInt = random.Next(1, 43);
                    CombatDiaManager.Instance.CreateCollectionItem(characterPosition, randInt);
                }
            }

            yield return new WaitForSeconds(8f);
        }
    }

    private IEnumerator StartDia2()
    {
        while (true)
        {
            if (PlayerPrefs.GetFloat("Background_4", 0) == 1)
            {
                characterPosition.x = transform.position.x;
                characterPosition.y = transform.position.y + Screen.height * 0.095f;
                characterPosition.z = transform.position.z;
                var randInt = random.Next(1, 200);
                if (randInt > 195)
                {
                    CombatDiaManager.Instance.CreateDia(characterPosition);
                }
                else if (randInt <= DataController.Instance.plusCollectionDrop)
                {
                    randInt = random.Next(1, 43);
                    CombatDiaManager.Instance.CreateCollectionItem(characterPosition, randInt);
                }
            }

            yield return new WaitForSeconds(8f);
        }
    }

    private void Start()
    {
        SetActivePanel();

        transform.position = new Vector3(x, y, z);
    }

    public void SetActivePanel()
    {
        int i = 0;
        while (true)
        {
            if (DataController.Instance.GetCostumeInfo(i) == 2)
            {
                DataChangeEvent.Instance.CostumeDataChanged(i);
                break;
            }

            i++;
        }

        int j = 0;
        while (true)
        {
            if (PlayerPrefs.GetFloat("Riding_" + j, 0) == 0)
            {
                if (name == "Riding")
                {
                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                break;
            }

            if (PlayerPrefs.GetFloat("Riding_" + j, 0) == 2)
            {
                if (name == "Riding")
                {
                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }

                DataChangeEvent.Instance.RidingDataChanged(j);
                break;
            }

            j++;
        }

        j = 0;

        while (true)
        {
            if (PlayerPrefs.GetFloat("Pet_" + j, 0) == 0)
            {
                if (name == "Pet")
                {
                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(false);
                    }
                }

                break;
            }

            if (PlayerPrefs.GetFloat("Pet_" + j, 0) == 2)
            {
                if (name == "Pet")
                {
                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                }

                DataChangeEvent.Instance.PetDataChanged(j);
                break;
            }

            j++;
        }
    }

    private void CostumeChanged(int i)
    {
        if (PlayerPrefs.GetFloat("Skin", 0) == 0)
        {
            if (i == 0)
            {
                GameObject.Find("CharacterImage").GetComponent<Animator>().runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/CharacterImage",
                        typeof(RuntimeAnimatorController));
            }
            else
            {
                GameObject.Find("CharacterImage").GetComponent<Animator>().runtimeAnimatorController =
                    (RuntimeAnimatorController) Resources.Load("Animation/CharacterImage " + i,
                        typeof(RuntimeAnimatorController));
            }
        }
        else
        {
            GameObject.Find("CharacterImage").GetComponent<Animator>().runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0)
                                                                             + "/CharacterImage",
                    typeof(RuntimeAnimatorController));
        }
    }

    private void RidingChanged(int i)
    {
        if (name == "Riding")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (PlayerPrefs.GetFloat("Skin", 0) == 0)
        {
            GameObject.Find("RidingImage").GetComponent<Animator>().runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animation/Riding/RidingImage " + i,
                    typeof(RuntimeAnimatorController));
        }
        else
        {
            GameObject.Find("RidingImage").GetComponent<Animator>().runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0)
                                                                             + "/RidingImage",
                    typeof(RuntimeAnimatorController));
        }

//        GameObject.Find("RidingImage").GetComponent<Image>().sprite = Resources.Load("Riding" + (i+1) , typeof(Sprite)) as Sprite;
    }

    private void PetChanged(int i)
    {
        if (name == "Pet")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (PlayerPrefs.GetFloat("Skin", 0) == 0)
        {
            GameObject.Find("PetImage1").GetComponent<Animator>().runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animation/Pet/PetImage " + i,
                    typeof(RuntimeAnimatorController));
        }
        else
        {
            GameObject.Find("PetImage1").GetComponent<Animator>().runtimeAnimatorController =
                (RuntimeAnimatorController) Resources.Load("Animation/Skin/" + (int) PlayerPrefs.GetFloat("Skin", 0)
                                                                             + "/PetImage1",
                    typeof(RuntimeAnimatorController));
        }
    }

    private void Tab(int r)
    {
        if (name == "Character")
        {
            characterPosition.x = transform.position.x;
            characterPosition.y = transform.position.y + Screen.height * 0.1f;
            characterPosition.z = transform.position.z;

            if (r < DataController.Instance.criticalPer)
            {
                // 크리티컬
                if (!DataController.Instance.isFever)
                {
                    CombatTextManager.Instance.CreateText(characterPosition,
                        DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
                                                           DataController.Instance.criticalRising), r);
                }
                else
                {
                    CombatTextManager.Instance.CreateText(characterPosition,
                        DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
                                                           DataController.Instance.feverGoldRising*
                                                           DataController.Instance.criticalRising), r);
                }
            }
            else
            {
                // 노 크리티컬
                if (!DataController.Instance.isFever)
                {
                    CombatTextManager.Instance.CreateText(characterPosition,
                        DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick), r);
                }
                else
                {
                    CombatTextManager.Instance.CreateText(characterPosition,
                        DataController.Instance.FormatGold(DataController.Instance.masterGoldPerClick *
                                                           DataController.Instance.feverGoldRising), r);
                }
            }
        }

        randomInt = random.Next(0, 100);

        if (randomInt < 20)
        {
            if (name.Contains("Pet") && PlayerPrefs.GetFloat("Pet_0", 0) > 0)
            {
                characterPosition.x = transform.position.x;
                characterPosition.y = transform.position.y + Screen.height * 0.05f;
                characterPosition.z = transform.position.z;

                if (!DataController.Instance.isFever)
                {
                    DataController.Instance.heart += 1;
                }

                if (!DataController.Instance.isCharacterMove)
                {
                    CombatHeartManager.Instance.CreateText(characterPosition);
                }
            }
        }
    }

    private void Update()
    {
        reverseButton.gameObject.SetActive(DataController.Instance.isCharacterMove);
    }

    private void FixedUpdate()
    {
        if (!DataController.Instance.isCharacterMove)
        {
            transform.position = new Vector3(positionX, positionY, z);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (DataController.Instance.isCharacterMove && !name.Contains("Character"))
        {
            transform.position = Input.mousePosition;
            print(transform.position);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (!name.Contains("Character"))
        {
            float canvasX = canvasScaler.referenceResolution.x;
            float canvasY = canvasScaler.referenceResolution.y;

            x = Input.mousePosition.x * (canvasX / Screen.width) - canvasX / 2;
            y = Input.mousePosition.y * (canvasY / Screen.height) - canvasY / 2 - 170;
            z = Input.mousePosition.z;

            DataController.Instance.SaveObjectPosition(this);
            DataController.Instance.LoadObjectPosition(this);
            positionX = (Screen.width >> 1) + Screen.width / (900f / x);
            positionY = (Screen.height >> 1) + heightBalance + Screen.height / (1600f / y);
        }
    }

    private void ResetPosition()
    {
        if (name.Contains("Character"))
        {
            x = 0f;
            y = -154f;
        }
        else if (name.Contains("Riding"))
        {
            x = -160f;
            y = -50f;
        }

        else if (name.Contains("BlackSmith"))
        {
            x = -240f;
            y = -275f;
        }
        else if (name.Contains("Tailor"))
        {
            x = 230f;
            y = -220f;
        }
        else if (name.Contains("Pet"))
        {
            x = 60f;
            y = -250f;
        }

        if (gameObject.active)
        {
            transform.position = new Vector3(Screen.width / 2 + Screen.width / (900f / x),
                Screen.height / 2 + heightBalance + Screen.height / (1600f / y), z);
        }

        z = 0f;

        DataController.Instance.SaveObjectPosition(this);

        DataController.Instance.LoadObjectPosition(this);
        positionX = (Screen.width >> 1) + Screen.width / (900f / x);
        positionY = (Screen.height >> 1) + heightBalance + Screen.height / (1600f / y);
    }
}