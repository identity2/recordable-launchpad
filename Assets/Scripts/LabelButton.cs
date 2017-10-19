using UnityEngine;
using UnityEngine.UI;

public class LabelButton : MonoBehaviour
{
	public delegate void LabelButtonToggleAction(bool labelOn);
	public static event LabelButtonToggleAction labelButtonToggleEvent;
	private bool labelOn = false;

	public Color onColor;

	public Image innerImage;

	private Color offColor;
	private Image _image;

	void Start()
	{
		_image = GetComponent<Image>();
		offColor = _image.color;	
	}

	public void OnToggle()
	{
		labelOn = !labelOn;

		//change the color of the button
		_image.color = labelOn ? onColor : offColor;
		innerImage.color = labelOn ? onColor : offColor;

		//the actual labels would react throught this event
		if (labelButtonToggleEvent != null)
				labelButtonToggleEvent(labelOn);
	}
}
