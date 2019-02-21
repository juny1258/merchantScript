using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHeartManager : MonoBehaviour {

	private static CombatHeartManager instance;

	public GameObject prefab;

	public RectTransform canvasTransform;

	public float fadeTime;

	public float speed;

	public static CombatHeartManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<CombatHeartManager>();
			}

			return instance;
		}
	}

	public void CreateText(Vector3 position)
	{
		var sct = Instantiate(prefab, position, Quaternion.identity);
		
		sct.transform.SetParent(canvasTransform);
		
		sct.GetComponent<CombatHeart>().Initialize(speed, fadeTime);
	}
}
