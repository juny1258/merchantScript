using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAds : MonoBehaviour
{

	public GameObject isPurchasePanel;
    	
	private void OnEnable()
	{
//		if (Debug.isDebugBuild)
//		{
//			PlayerPrefs.SetFloat("NoAds", 0);
//		}
		if (PlayerPrefs.GetFloat("NoAds", 0) == 1)
		{
			isPurchasePanel.SetActive(true);
		}
		else
		{
			isPurchasePanel.SetActive(false);
		}
	}

	public void PurchaseStartPakage()
	{
		DataController.Instance.dia += 3000;
		PlayerPrefs.SetFloat("NoAds", 1);
		
		isPurchasePanel.SetActive(true);
	}
	
}
