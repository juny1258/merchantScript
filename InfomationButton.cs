using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfomationButton : MonoBehaviour
{

	
	public GameObject InfomationPanel;
	public GameObject SettingPanel;
	public GameObject BackPanel;

	public void OnClick()
	{
		SettingPanel.SetActive(false);
		BackPanel.SetActive(false);
		InfomationPanel.SetActive(true);
	}
}
