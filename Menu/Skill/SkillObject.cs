using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillObject : MonoBehaviour {

	public Image skillImage;
	public Text Name;
	public Text info;
	public Button purchaseButton;

	private void Start()
	{
		transform.localScale = new Vector3(1, 1, 1);
	}
}
