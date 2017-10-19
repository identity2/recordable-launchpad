using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopUpPanel : MonoBehaviour 
{
	public static NotificationPopUpPanel Instance {get; set;}

	public Text messageText;

	void Awake()
	{
		Instance = this;
		gameObject.SetActive(false);
	}

	public void Show(string message)
	{
		messageText.text = message;
		gameObject.SetActive(true);
	}

	public void ClosePopUp()
	{
		gameObject.SetActive(false);
	}
}
