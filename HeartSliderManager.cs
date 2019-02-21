using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSliderManager : MonoBehaviour
{
    public Slider slider;
    public Text feverText;
    public AudioSource Audio;

    public RectTransform HeartImage;

    private void Start()
    {
        DataController.Instance.heart = 0;
        
        DataController.Instance.isFever = false;
        DataChangeEvent.TabScreen += index =>
        {
            if (!DataController.Instance.isFever)
            {
                slider.value = DataController.Instance.heart;
                if (slider.value >= 100)
                {
                    DataController.Instance.isFever = true;
                    DataController.Instance.heart = 0;
                    if (PlayerPrefs.GetFloat("Sound", 0) == 0)
                    {
                        Audio.Play();
                    }
                    
                    feverText.gameObject.SetActive(true);
                }
            }
        };

        StartCoroutine("MinusSlider");
    }

    IEnumerator MinusSlider()
    {
        while (true)
        {
            if (DataController.Instance.isFever)
            {
                slider.value -= 0.5f;
                FeverHeartManager.Instance.CreateText(HeartImage.transform.position);
                if (slider.value == 0)
                {
                    DataController.Instance.isFever = false;
                    
                    feverText.gameObject.SetActive(false);
                }
            }

            yield return new WaitForSeconds(0.08f);
        }
    }
}