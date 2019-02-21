using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelImage : MonoBehaviour
{
	private float animTime = 1.5f;

	private Image FadeImage;

	private float start = 0f;
	private float end = 1f;
	private float time;

	private bool isPlaying;

	public GameObject OKButton;

	private void Awake()
	{
		FadeImage = GetComponent<Image>();
	}

	private void OnEnable()
	{
		FadeImage.sprite = Resources.Load("LevelUp/Level" + DataController.Instance.reverseLevel,
			typeof(Sprite)) as Sprite;
		StartFadeAnim();
		
		Invoke("SetButton", 2f);
	}

	private void SetButton()
	{
		OKButton.SetActive(true);
	}

	private void StartFadeAnim()
	{
		if (isPlaying)
		{
			return;
		}

		StartCoroutine("PlalyFadeOut");


	}

	private IEnumerator PlalyFadeOut()
	{
		isPlaying = true;

		var color = FadeImage.color;
		time = 0f;
		color.a = Mathf.Lerp(start, end, time);

		while (color.a < 1)
		{
			time += Time.deltaTime / animTime;

			color.a = Mathf.Lerp(start, end, time);

			FadeImage.color = color;

			yield return null;
		}

		isPlaying = false;
	}

	private void OnDisable()
	{
		OKButton.SetActive(false);
	}
}
