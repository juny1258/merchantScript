using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewManager : MonoBehaviour
{

	public GameObject ReviewPanel;
	public GameObject OnMenu1;
	
	public void NO()
	{
		ReviewPanel.SetActive(false);
		OnMenu1.SetActive(false);
	}

	public void YES()
	{
		Application.OpenURL("market://details?id=com.juny.merchant");
		ReviewPanel.SetActive(false);
		OnMenu1.SetActive(false);
	}

	public void GoCafe()
	{
		Application.OpenURL("https://cafe.naver.com/juny0merchant");
	}
}
