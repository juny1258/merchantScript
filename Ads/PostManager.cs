using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostManager : MonoBehaviour
{
    
    private static PostManager instance;

    public static PostManager Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType<PostManager>();

            return instance;
        }
    }
    
    public GameObject DiaAdsPanel;
    public GameObject BackPanel;
    public GameObject NotificationPanel;
	
    public void NotificationButtonClick()
    {
        NotificationPanel.SetActive(false);
        DiaAdsPanel.SetActive(true);
        BackPanel.SetActive(true);
        
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Image>().raycastTarget = false;
    }

    public GameObject PostNotification;
    
    private void Start()
    {
        StartTimer();
    }
    
    public void StartTimer()
    {
        Invoke("SetNoticiationPanel", 180f);
    }

    public void SetNoticiationPanel()
    {
        GetComponent<Button>().onClick.AddListener(NotificationButtonClick);
        GetComponent<Image>().raycastTarget = true;
        PostNotification.SetActive(true);
        Invoke("RemoveNotification",170f);
    }

    public void RemoveNotification()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Image>().raycastTarget = false;
        if (PostNotification.active)
        {
            PostNotification.SetActive(false);
            StartTimer();
        }
    }
}
