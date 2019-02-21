using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteButton : MonoBehaviour
{
    public GameObject resetButton;

    public GameObject movePanel;
    
    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SettingComplete()
    {
        movePanel.SetActive(true);

        resetButton.SetActive(false);
        gameObject.SetActive(false);
        
        DataController.Instance.isCharacterMove = false;
    }
}