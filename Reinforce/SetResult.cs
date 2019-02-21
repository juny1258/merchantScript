using System;
using UnityEngine;
using UnityEngine.UI;

public class SetResult : MonoBehaviour
{

	public string result;

	public Image ResultText;

	private void OnEnable()
	{
		ResultText.sprite = Resources.Load(result, typeof(Sprite)) as Sprite;
		Invoke("ShowPanel", 2.1f);
	}

	private void ShowPanel()
	{
		ResultText.gameObject.SetActive(true);
	}
}
