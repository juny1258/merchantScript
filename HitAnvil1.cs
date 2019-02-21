using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnvil1 : MonoBehaviour
{

	public void HitAnvil()
	{
		if (PlayerPrefs.GetFloat("Sound", 0) == 0)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
	}
}
