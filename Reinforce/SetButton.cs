using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetButton : MonoBehaviour
{

	public GameObject Button;
	
	private void OnEnable()
	{
		Invoke("SetButtonSec", 1f);
	}

	private void SetButtonSec()
	{
		Button.SetActive(true);
	}
	
	
}
