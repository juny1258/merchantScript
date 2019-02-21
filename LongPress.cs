using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

	[SerializeField] private float holdTime = 2f;

	public RectTransform SkillPanel;
	private Vector3 position;
	private float heightBalance = Screen.height / 3.38266f + Screen.height / 1.60965f / 2 - Screen.height / 2;

	private void FixedUpdate()
	{
		position.x = Screen.width / 2 + Screen.width / (900f / PlayerPrefs.GetFloat("CharacterX", 0));
		position.y = Screen.height / 2 + heightBalance + Screen.height / (1600f / PlayerPrefs.GetFloat("CharacterY", -154f));
		SkillPanel.transform.position = position;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Invoke("OnLongPress", holdTime);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		CancelInvoke("OnLongPress");
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		CancelInvoke("OnLongPress");
	}

	private void OnLongPress()
	{
		if (!DataController.Instance.isSkillOn)
		{
			SkillPanel.gameObject.SetActive(true);
			Invoke("SetPanelFalse", 2f);
		}
	}

	private void SetPanelFalse()
	{
		SkillPanel.gameObject.SetActive(false);
	}
}
