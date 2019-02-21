using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetObject : MonoBehaviour {
	
	public Image petImage;
	public Text Name;
	public Text info;
	public Button purchaseButton;
    
	public Text notPurchaseInfo;
	public GameObject notPurchasePanel;

	private void Start()
	{
		transform.localScale = new Vector3(1, 1, 1);
	}
}
