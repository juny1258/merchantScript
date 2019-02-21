using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CombatHeart : MonoBehaviour {

	private float speed;
	private Vector3 direction;
	private float fadeTime;

	private Random rand;
	private float randFloat;
	private float y;

	void Update()
	{
		var translation = speed * Time.deltaTime;
        
		transform.Translate(direction * translation);
	}

	public void Initialize(float speed, float fadeTime)
	{
		rand = new Random();
		randFloat = rand.Next(-5, 5);
		
		this.speed = speed;
		this.fadeTime = fadeTime;
		direction = new Vector3(randFloat, 5, 0f);

		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		var startImageAlpha = GetComponent<Image>().color.a;
        
		var rate = 1.0f * fadeTime;
		var progress = 0.0f;

		while (progress <= 1.5f)
		{
			var tmpImageColor = GetComponent<Image>().color;
            
			GetComponent<Image>().color = new Color(tmpImageColor.r, tmpImageColor.g, tmpImageColor.b, Mathf.Lerp(startImageAlpha, 0, progress));

			progress += rate * Time.deltaTime;

			if (progress > 1.2f)
			{
				Destroy(gameObject);
			}

			yield return null;
		}
	}
}
