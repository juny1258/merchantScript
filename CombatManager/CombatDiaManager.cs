using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatDiaManager : MonoBehaviour {

	private static CombatDiaManager instance;

	public GameObject prefab;

	public GameObject prefab2;

	public RectTransform canvasTransform;

	public float speed;

	public Vector3 direction;

	public static CombatDiaManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<CombatDiaManager>();
			}

			return instance;
		}
	}

	public void CreateDia(Vector3 position)
	{
		var sct = Instantiate(prefab, position, Quaternion.identity);
		
		sct.transform.SetParent(canvasTransform);
		
		sct.GetComponent<CombatDia>().Initialize(speed, direction);
	}

	public void CreateCollectionItem(Vector3 position, int index)
	{
		var sct = Instantiate(prefab2, position, Quaternion.identity);
		
		sct.transform.SetParent(canvasTransform);
		
		sct.GetComponent<CombatCollectionItem>().Initialize(speed, direction, index, position);
	}
}
