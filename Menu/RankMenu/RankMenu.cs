using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;

public class RankMenu : MonoBehaviour
{
	public GameObject NotificationPanel;
	public Text text;

	public GameObject BackPanel;

	public void ShowBoard()
	{
		PlayGamesPlatform.Activate();
		Social.localUser.Authenticate(ShowLeaderBoard);
	}
	
	
	void ShowLeaderBoard(bool isSuccess, string ErrorMessage)
	{
		if (isSuccess)
		{
			// login success
			var levelScore = DataController.Instance.level;
			const string levelBoardId = "CgkI_LDZ7OkMEAIQBA";

			Social.ReportScore((long)levelScore, levelBoardId, success =>
			{
				if (success)
				{

				}
			});
			
			var reinforceScore = PlayerPrefs.GetFloat("ReinforceLevel", 0);
			const string reinforceBoardId = "CgkI_LDZ7OkMEAIQBg";

			Social.ReportScore((long)reinforceScore, reinforceBoardId, success =>
			{
				if (success)
				{

					
				}
			});
			
			var reverseLevelScore = DataController.Instance.reverseLevel;
			const string reverseLevelBoardId = "CgkI_LDZ7OkMEAIQBw";
			
			Social.ReportScore((long)reverseLevelScore, reverseLevelBoardId, success =>
			{
				if (success)
				{

					
				}
			});
			
			
			PlayGamesPlatform.Instance.ShowLeaderboardUI();
			
			
		}
		else
		{

			NotificationPanel.SetActive(true);
			BackPanel.SetActive(true);
			text.text = ErrorMessage;
		}

	}
}
