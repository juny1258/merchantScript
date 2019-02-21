using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OKButton : MonoBehaviour
{
	public GameObject ReinforcePanel;
	public GameObject SuccessPanel;
	public GameObject FailPanel;
	public GameObject ResultText;

	public GameObject ReinforceSelect;
	public GameObject OnMenu1;

	public void OnClick()
	{
		ReinforcePanel.SetActive(false);
		SuccessPanel.SetActive(false);
		FailPanel.SetActive(false);
		ResultText.SetActive(false);
		gameObject.SetActive(false);
		ReinforceSelect.SetActive(false);
		OnMenu1.SetActive(false);
		
//		AdMob.Instance.ShowReinforceAd();
	}

	public void LevelUpButton()
	{
		ReinforcePanel.SetActive(false);
	}
	
}
