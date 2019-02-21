using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class FeverCombatHeart : MonoBehaviour {

	private float speed;
	private Vector3 direction;

	private Random rand;
	private float randFloat;
	private float x, y;

	private void Update()
	{
		var translation = speed * Time.deltaTime;
        
		transform.Translate(direction * translation);
	}

	public void Initialize()
	{
		rand = new Random();
		randFloat = (float)rand.NextDouble() + (float)rand.NextDouble() * 0.1f;
		
		x = (float)Math.Sin(randFloat * 3.14f) * 5f;
		y = -Math.Abs((float)Math.Cos(randFloat * 3.14f) * 5f);

		speed = rand.Next((int)(Screen.width * 0.033333f), (int)(Screen.height * 0.05f));
		direction = new Vector3(x, y, 0f);

		StartCoroutine(FadeOut());
	}
	
	private IEnumerator FadeOut()
	{   
		var rate = 1.0f;
		var progress = 0.0f;

		while (progress <= 5.6f)
		{
			progress += rate * Time.deltaTime;

			if (progress > 5.5f)
			{
				Destroy(gameObject);
			}

			yield return null;
		}
	}
}
