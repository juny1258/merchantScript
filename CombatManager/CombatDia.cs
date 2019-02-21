using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatDia : MonoBehaviour {

	private float speed;
	private Vector3 direction;
	private float fadeTime;

	private float rate;
	private float progress;
	private float translation;

	void Update()
	{
		if (progress > 0)
		{
			translation = speed * Time.deltaTime;
		}
		transform.Translate(direction * translation);
	}

	private void Start()
	{
		transform.localScale = new Vector3(1, 1, 1);
	}

	public void Initialize(float speed, Vector3 direction)
	{
		this.speed = speed;
		this.direction = direction;
		
		GetComponent<Image>().raycastTarget = false;
		DataController.Instance.dia += 1;
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		var startImageAlpha = GetComponentInChildren<Image>().color.a;
        
		rate = 1.0f;
		progress = 0.0f;

		while (progress <= 3f)
		{
			var tmpImageColor = GetComponentInChildren<Image>().color;
            
			GetComponentInChildren<Image>().color = new Color(tmpImageColor.r, tmpImageColor.g, tmpImageColor.b, Mathf.Lerp(startImageAlpha, 0, progress));

			progress += rate * Time.deltaTime;

			if (progress > 2.8f)
			{
				Destroy(gameObject);
			}

			yield return null;
		}
	}
}
