using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{

	public GameObject SkillRawImage;

	public GameObject Skill;

	public Texture SkillSrc;

	public GameObject SkillWindow;

	public Image CoolDownImage;

	public GameObject SkillView;

	private int[] diaArray = {10, 20, 30, 40, 50};

	public int index;

	private void OnEnable()
	{
		GetComponent<Button>().onClick.RemoveAllListeners();
		
		if (PlayerPrefs.GetFloat("Skill_" + index, 0) == 1)
		{
			// 스킬 구매 후
			GetComponent<Image>().sprite = Resources.Load("SkillImage" + (index+1), typeof(Sprite)) as Sprite;
			GetComponent<Button>().onClick.AddListener(SkillOn);
		}
		else
		{
			GetComponent<Image>().sprite = Resources.Load("LockSkill" + (index+1), typeof(Sprite)) as Sprite;
			// 스킬 구매 전
		}
	}

	private void SkillOn()
	{
		if (!DataController.Instance.isSkillOn && PlayerPrefs.GetFloat("CoolDown_" + index, 0) == 0
		    && DataController.Instance.dia >= diaArray[index])
		{
			DataController.Instance.dia -= diaArray[index];
			PlayerPrefs.SetFloat("CoolDown_" + index, 1);
			DataController.Instance.skillTime = 1;
			DataChangeEvent.Instance.SkillStarted(index);
			SkillWindow.SetActive(false);
			SkillRawImage.GetComponent<RawImage>().texture = SkillSrc;
			SkillRawImage.SetActive(true);
			Skill.SetActive(true);
			SkillView.GetComponent<Image>().sprite = Resources.Load("SkillImage" + (index + 1), typeof(Sprite)) as Sprite;
			SkillView.SetActive(true);

			DataController.Instance.skillGoldPerClick = DataController.Instance.skillRisingGold[index];
			DataController.Instance.isSkillOn = true;
			
			DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
			                                             DataController.Instance.levelGoldPerClick *
			                                             DataController.Instance.skillGoldPerClick *
			                                             DataController.Instance.plusGoldPerClick *
			                                             DataController.Instance.collectionGoldPerClick *
			                                             DataController.Instance.reinforceGoldPerClick * 
			                                             DataController.Instance.skinGoldPerClick *
			                                             DataController.Instance.reverseGolePerClick;
		
			Invoke("EndSkill", 20f);
		}
		
	}

	private void FixedUpdate()
	{
		CoolDownImage.fillAmount = PlayerPrefs.GetFloat("CoolDown_" + index, 0);
	}

	private void EndSkill()
	{
		SkillRawImage.SetActive(false);
		Skill.SetActive(false);
		SkillView.SetActive(false);
		DataController.Instance.skillGoldPerClick = 1;
		
		DataController.Instance.masterGoldPerClick = DataController.Instance.goldPerClick *
		                                             DataController.Instance.levelGoldPerClick *
		                                             DataController.Instance.skillGoldPerClick *
		                                             DataController.Instance.plusGoldPerClick *
		                                             DataController.Instance.collectionGoldPerClick *
		                                             DataController.Instance.reinforceGoldPerClick * 
		                                             DataController.Instance.skinGoldPerClick *
		                                             DataController.Instance.reverseGolePerClick;

		DataController.Instance.isSkillOn = false;
	}
}
