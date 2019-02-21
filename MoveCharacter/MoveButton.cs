using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveButton : MonoBehaviour
{
    private GameObject middlePanel;
    private GameObject setPositionButton;

    public GameObject SkillPanel;

    public Button ResetButton;
    public Button CompleteButton;

    public GameObject SettingPanel;
    public GameObject BackPanel;

    public Image imageView;

    private void Start()
    {
        middlePanel = GameObject.Find("MovePanel");
    }

    public void OnClickMoveButton()
    {
        SettingPanel.SetActive(false);
        BackPanel.SetActive(false);
        
        DataController.Instance.isCharacterMove = true;
        middlePanel.SetActive(false);

        foreach (Transform child in middlePanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (SkillPanel.active)
        {
            SkillPanel.SetActive(false);
        }
        ResetButton.gameObject.SetActive(true);
        CompleteButton.gameObject.SetActive(true);
    }

    public void OnClickToReverse()
    {
        imageView.transform.rotation = imageView.transform.rotation.y == 0
            ? new Quaternion(0, 180, 0, 0)
            : new Quaternion(0, 0, 0, 0);
    }
}