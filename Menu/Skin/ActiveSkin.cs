using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkin : MonoBehaviour
{

	public GameObject[] SkinPanels;

	private void OnEnable()
	{
		for (var i = 1; i < SkinPanels.Length + 1; i++)
		{
			SkinPanels[i - 1].SetActive(PlayerPrefs.GetFloat("Skin_" + i, 0) != 0);
		}
	}
}
