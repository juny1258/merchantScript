using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatCollectionItem : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private float fadeTime;
    private Vector3 position;

    private float rate;
    private float progress;
    private float translation;

    private float index;

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

    public void Initialize(float speed, Vector3 direction, int index, Vector3 position)
    {
        this.speed = speed;
        this.direction = direction;
        this.index = index;
        this.position = position;

        GetComponent<Image>().sprite = Resources.Load("CollectionItem/profile" + index, typeof(Sprite)) as Sprite;

        GetComponent<Image>().raycastTarget = false;
        if (PlayerPrefs.GetFloat("CollectionItem_" + index, 0) == 0)
        {
            // 처음 아이템을 획득하였을 경우
            PlayerPrefs.SetFloat("CollectionItem_" + index, 1);
            DataController.Instance.collectionGoldPerClick += 0.2f;
            StartCoroutine(FadeOut());

            DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
                                                         DataController.Instance.levelGoldPerClick *
                                                         DataController.Instance.skillGoldPerClick *
                                                         DataController.Instance.plusGoldPerClick *
                                                         DataController.Instance.collectionGoldPerClick *
                                                         DataController.Instance.reinforceGoldPerClick *
                                                         DataController.Instance.skinGoldPerClick *
                                                         DataController.Instance.reverseGolePerClick;
        }
        else
        {
            // 중복으로 아이템을 획득하였을 경우
            Destroy(gameObject);
            DataController.Instance.gold += DataController.Instance.goldPerSec * 
                                            DataController.Instance.plusGoldPerSec * 
                                            DataController.Instance.skinGoldPerSec *
                                            DataController.Instance.reverseGolePerSec * 60;
            CombatTextManager.Instance.CreateText(position,
                DataController.Instance.FormatGold(DataController.Instance.goldPerSec * 
                                                   DataController.Instance.plusGoldPerSec * 
                                                   DataController.Instance.skinGoldPerSec *
                                                   DataController.Instance.reverseGolePerSec * 60), 100);
        }
    }

    private IEnumerator FadeOut()
    {
        var startImageAlpha = GetComponentInChildren<Image>().color.a;

        rate = 1.0f;
        progress = 0.0f;

        while (progress <= 3f)
        {
            var tmpImageColor = GetComponentInChildren<Image>().color;

            GetComponentInChildren<Image>().color = new Color(tmpImageColor.r, tmpImageColor.g, tmpImageColor.b,
                Mathf.Lerp(startImageAlpha, 0, progress));

            progress += rate * Time.deltaTime;

            if (progress > 2.8f)
            {
                Destroy(gameObject);
            }

            yield return null;
        }
    }
}