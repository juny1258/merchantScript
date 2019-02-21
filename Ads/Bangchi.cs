using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bangchi : MonoBehaviour
{

	public GameObject BangchiPanel;
	
	private void OnEnable()
	{
		if (DataController.Instance.bangchi != 0)
		{
			GetComponent<Text>().color = new Color(0.09f, 0.1647f, 0.9255f);
			GetComponent<Text>().text = "ON";
		}
		else
		{
			GetComponent<Text>().color = new Color(0.9255f, 0.09f, 0.09f);
			GetComponent<Text>().text = "OFF";
		}
	}

	public void BangchiMode()
	{
		if (DataController.Instance.bangchi == 0)
		{
			
			DataController.Instance.bangchi = 1;
			GetComponent<Text>().color = new Color(0.09f, 0.1647f, 0.9255f);
			GetComponent<Text>().text = "ON";
			BangchiPanel.SetActive(true);
		}
		else
		{
			DataController.Instance.bangchi = 0;
			GetComponent<Text>().color = new Color(0.9255f, 0.09f, 0.09f);
			GetComponent<Text>().text = "OFF";
			BangchiPanel.SetActive(false);
		}
	}
}
