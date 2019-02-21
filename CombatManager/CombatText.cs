using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private float fadeTime;

    public AudioSource Audio;

    private void OnEnable()
    {
        if (PlayerPrefs.GetFloat("Sound", 0) == 0)
        {
            Audio.Play();
        }
    }

    void Update()
    {
        var translation = speed * Time.deltaTime;
        
        transform.Translate(direction * translation);
    }

    public void Initialize(float speed, Vector3 direction, float fadeTime)
    {
        this.speed = speed;
        this.direction = direction;

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        var startTextAlpha = GetComponentInChildren<Text>().color.a;
        var startImageAlpha = GetComponentInChildren<Image>().color.a;
        
        var rate = 1.0f;
        var progress = 0.0f;

        while (progress <= 1.5f)
        {
            var tmpTextColor = GetComponentInChildren<Text>().color;
            var tmpImageColor = GetComponentInChildren<Image>().color;
            
            GetComponentInChildren<Text>().color = new Color(tmpTextColor.r, tmpTextColor.g, tmpTextColor.b, Mathf.Lerp(startTextAlpha, 0, progress));
            GetComponentInChildren<Image>().color = new Color(tmpImageColor.r, tmpImageColor.g, tmpImageColor.b, Mathf.Lerp(startImageAlpha, 0, progress));

            progress += rate * Time.deltaTime;

            if (progress > 1.2f)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }
}