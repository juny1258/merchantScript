using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;

    public GameObject prefab;

    public GameObject critical;

    public RectTransform canvasTransform;

    public float fadeTime;

    public float speed;

    public Vector3 direction;

    public static CombatTextManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }

            return instance;
        }
    }

    public void CreateText(Vector3 position, string text, int randInt)
    {
        if (randInt < DataController.Instance.criticalPer)
        {
            var sct = Instantiate(critical, position, Quaternion.identity);

            sct.transform.SetParent(canvasTransform);

            sct.GetComponent<CombatText>().Initialize(speed, direction, fadeTime);
            sct.GetComponentInChildren<Text>().text = " + " + text;
        }
        else
        {
            var sct = Instantiate(prefab, position, Quaternion.identity);

            sct.transform.SetParent(canvasTransform);

            sct.GetComponent<CombatText>().Initialize(speed, direction, fadeTime);
            sct.GetComponentInChildren<Text>().text = " + " + text;
        }
    }
}