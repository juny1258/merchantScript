using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverHeartManager : MonoBehaviour {

	private static FeverHeartManager instance;

	public GameObject prefab;

	public RectTransform canvasTransform;

	public float fadeTime;

	public float speed;

	public static FeverHeartManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<FeverHeartManager>();
			}

			return instance;
		}
	}

	public void CreateText(Vector3 position)
	{
		var sct = Instantiate(prefab, position, Quaternion.identity);
		
		sct.transform.SetParent(canvasTransform);
		
		sct.GetComponent<FeverCombatHeart>().Initialize();
	}
}
