using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoItem : MonoBehaviour {

	public GameObject isPurchasePanel;
    	
	private void OnEnable()
	{
		if (PlayerPrefs.GetFloat("AutoItem", 0) == 1)
		{
			isPurchasePanel.SetActive(true);
		}
		else
		{
			isPurchasePanel.SetActive(false);
		}
	}

	public void PurchaseAutoItem()
	{
		PlayerPrefs.SetFloat("AutoItem", 1);
		
		isPurchasePanel.SetActive(true);
	}
}
