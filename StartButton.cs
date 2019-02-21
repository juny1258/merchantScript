using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	public RectTransform panel;

	public void CloseInfomation()
	{
		panel.gameObject.SetActive(false);
	}
}
