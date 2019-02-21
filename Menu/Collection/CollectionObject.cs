using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionObject : MonoBehaviour
{

	public int index;

	private void OnEnable()
	{
		if (PlayerPrefs.GetFloat("CollectionItem_" + index, 0) == 0)
		{
			// TODO 잠금 이미지 로드
			GetComponent<Image>().sprite = Resources.Load("CollectionItem/ItemLock" + index, typeof(Sprite)) as Sprite;
			
		}
		else
		{
			// TODO 수집품 이미지 로드
			GetComponent<Image>().sprite = Resources.Load("CollectionItem/item_" + index, typeof(Sprite)) as Sprite;
		}
	}
}
