using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMenu : MonoBehaviour {

    public GameObject BackPanel;

    public GameObject OnMenu1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OnMenu1.active)
            {
                gameObject.SetActive(false);
                BackPanel.SetActive(false);
            }
        }
    }
}
