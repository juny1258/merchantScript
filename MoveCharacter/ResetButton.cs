using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive(false);
	}
	
	public void OnClickResetButton()
	{
		DataChangeEvent.Instance.ResetPositions();
	}
}
