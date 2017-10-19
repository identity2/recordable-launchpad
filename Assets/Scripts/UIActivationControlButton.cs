using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActivationControlButton : MonoBehaviour
{
	public GameObject targetUI;

	public void SetActiveCanvas(bool activate)
	{
		targetUI.SetActive (activate);
	}
}
