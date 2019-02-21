using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColor : MonoBehaviour
{
    private string[] colorString =
    {
        "C3C3BAFF", "D3AF79FF", "F7C986FF", "BF965CFF", "B6B59EFF", "C3C3BAFF"
    };

    private Color color;

    // Use this for initialization
    private void OnEnable()
    {
        ColorUtility.TryParseHtmlString("#" + colorString[(int) PlayerPrefs.GetFloat("WearingTown", 0)], out color);
        GetComponent<Camera>().backgroundColor = color;

        DataChangeEvent.SkillStartEvent += index =>
        {
            ColorUtility.TryParseHtmlString("#" + colorString[(int) PlayerPrefs.GetFloat("WearingTown", 0)], out color);
            GetComponent<Camera>().backgroundColor = color;
        };

        DataChangeEvent.ResetDataEvent += () =>
        {
            ColorUtility.TryParseHtmlString("#" + colorString[(int) PlayerPrefs.GetFloat("WearingTown", 0)], out color);
            GetComponent<Camera>().backgroundColor = color;
        };
    }

    // Update is called once per frame
    void Update()
    {
    }
}