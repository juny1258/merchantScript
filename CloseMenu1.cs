using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu1 : MonoBehaviour
{
    public GameObject BackPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            BackPanel.SetActive(false);
        }
    }
}