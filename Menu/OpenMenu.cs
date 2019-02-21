using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{

	public GameObject MenuPanel;

	public void OnClick()
	{
		MenuPanel.SetActive(!MenuPanel.active);
	}

	public void Test()
	{
	}
}
