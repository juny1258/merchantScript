using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

	public GameObject riding;
	public GameObject pet;

	private void Start()
	{
		riding.SetActive(PlayerPrefs.GetFloat("Riding_0", 0) != 0);

		pet.SetActive(PlayerPrefs.GetFloat("Pet_0", 0) != 0);
	}
}
