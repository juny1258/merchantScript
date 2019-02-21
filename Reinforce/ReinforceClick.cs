using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceClick : MonoBehaviour
{

	public GameObject ReinforcePanel;
	public GameObject OnMenu1;

	public void OnClick()
	{
		ReinforcePanel.SetActive(true);
		OnMenu1.SetActive(true);
	}
}
