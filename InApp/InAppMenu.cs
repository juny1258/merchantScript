using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppMenu : MonoBehaviour
{

	public GameObject GoldPanel;
	public GameObject DiaPanel;

	public GameObject BackPanel;

	public void AddGoldButton()
	{
		GoldPanel.SetActive(true);
		BackPanel.SetActive(true);
	}

	public void AddDiaButton()
	{
		DiaPanel.SetActive(true);
		BackPanel.SetActive(true);
	}
}
