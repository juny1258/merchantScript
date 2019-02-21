// code reference: http://answers.unity3d.com/questions/894995/how-to-saveload-with-google-play-services.html		
// you need to import https://github.com/playgameservices/play-games-plugin-for-unity
using UnityEngine;
using System;
using System.Collections;
//gpg
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
//for encoding
using System.Text;
//for extra save ui
using UnityEngine.SocialPlatforms;
//for text, remove
using UnityEngine.UI;
public class PlayCloudDataManager : MonoBehaviour
{

    public GameObject OnMenu1;
    public GameObject NotificationPanel;

    public GameObject LoadSuccessPanel;

    private static PlayCloudDataManager instance;

    public static PlayCloudDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayCloudDataManager>();

                if (instance == null)
                {
                    instance = new GameObject("PlayGameCloudData").AddComponent<PlayCloudDataManager>();
                }
            }

            return instance;
        }
    }

    public bool isProcessing
    {
        get;
        private set;
    }
    public string loadedData
    {
        get;
        private set;
    }
    private const string m_saveFileName = "game_save_data";
    public bool isAuthenticated
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }

    private void InitiatePlayGames()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        // enables saving game progress.
        .EnableSavedGames()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = false;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
    }

	private void Awake()
	{
		InitiatePlayGames();
	}


    public void Login()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                NotificationPanel.GetComponentInChildren<Text>().text = "Fail Login";
                Debug.Log("Fail Login");
            }
        });
    }


    private void ProcessCloudData(byte[] cloudData)
    {
        if (cloudData == null)
        {
            OnMenu1.SetActive(true);
            NotificationPanel.SetActive(true);
            NotificationPanel.GetComponentInChildren<Text>().text = "No Data saved to the cloud";
            Debug.Log("No Data saved to the cloud");
            return;
        }

        string progress = BytesToString(cloudData);
        loadedData = progress;
    }


    public void LoadFromCloud(Action<string> afterLoadAction)
    {
        if (isAuthenticated && !isProcessing)
        {
			StartCoroutine(LoadFromCloudRoutin(afterLoadAction));
        }
		else
		{
			Login();
		}
    }

	private IEnumerator LoadFromCloudRoutin(Action<string> loadAction)
	{
		isProcessing = true;
	    OnMenu1.SetActive(true);
	    NotificationPanel.SetActive(true);
	    NotificationPanel.GetComponentInChildren<Text>().text = "데이터를 불러오는 중입니다\n잠시만 기다려주세요.";
		Debug.Log("Loading game progress from the cloud.");

		((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
			m_saveFileName, //name of file.
			DataSource.ReadCacheOrNetwork,
			ConflictResolutionStrategy.UseLongestPlaytime,
			OnFileOpenToLoad);

		while(isProcessing)
		{
			yield return null;
		}

		loadAction.Invoke(loadedData);
	}

    public void SaveToCloud(string dataToSave)
    {

        if (isAuthenticated)
        {
            loadedData = dataToSave;
            isProcessing = true;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_saveFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnFileOpenToSave);
        }
        else
        {
            Login();
        }
    }

    private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status == SavedGameRequestStatus.Success)
        {

            byte[] data = StringToBytes(loadedData);

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            SavedGameMetadataUpdate updatedMetadata = builder.WithUpdatedDescription("Saved at " + DateTime.Now).Build();

            ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, data, OnGameSave);
            
            NotificationPanel.GetComponentInChildren<Text>().text = "클라우드 저장 완료!";
        }
        else
        {
            OnMenu1.SetActive(true);
            NotificationPanel.SetActive(true);
            NotificationPanel.GetComponentInChildren<Text>().text = "Error opening Saved Game" + status;
            Debug.LogWarning("Error opening Saved Game" + status);
        }
    }


    private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
            
            OnMenu1.SetActive(true);
            LoadSuccessPanel.SetActive(true);
            LoadSuccessPanel.GetComponentInChildren<Text>().text = "데이터를 불러오는 중입니다\n잠시만 기다려주세요.";
        }
        else
        {
            OnMenu1.SetActive(true);
            NotificationPanel.SetActive(true);
            NotificationPanel.GetComponentInChildren<Text>().text = "Error opening Saved Game" + status;
            Debug.LogWarning("Error opening Saved Game" + status);
        }
    }


    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            OnMenu1.SetActive(true);
            NotificationPanel.SetActive(true);
            NotificationPanel.GetComponentInChildren<Text>().text = "Error Saving" + status;
            Debug.LogWarning("Error Saving" + status);
        }
        else
        {
            ProcessCloudData(bytes);
        }

        isProcessing = false;
    }

    private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            OnMenu1.SetActive(true);
            NotificationPanel.SetActive(true);
            NotificationPanel.GetComponentInChildren<Text>().text = "Error Saving" + status;
            Debug.LogWarning("Error Saving" + status);
        }

        isProcessing = false;
    }

    private byte[] StringToBytes(string stringToConvert)
    {
        return Encoding.UTF8.GetBytes(stringToConvert);
    }

    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}
