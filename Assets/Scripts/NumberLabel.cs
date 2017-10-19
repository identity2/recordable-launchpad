using UnityEngine;
using UnityEngine.UI;

public class NumberLabel : MonoBehaviour 
{
	private Text _text;

	private Color originalColor;

	//label button -> set active
	//record -> set alpha to 0
	void Start()
	{
		LabelButton.labelButtonToggleEvent += LabelToggled;
		SoundboardManager.stateChangedEvent += StateChanged;
		
		_text = GetComponent<Text>();
		originalColor = _text.color;

		gameObject.SetActive(false);
	}

	void OnDestroy()
	{
		LabelButton.labelButtonToggleEvent -= LabelToggled;
		SoundboardManager.stateChangedEvent -= StateChanged;
	}

	void StateChanged(SoundboardManager.SoundboardState state)
	{
		if (state == SoundboardManager.SoundboardState.Default)
		{
			_text.color = originalColor;	
		}
		else
		{
			_text.color = new Color(0f,0f,0f,0f);
		}
	}

	void LabelToggled(bool labelOn)
	{
		gameObject.SetActive(labelOn);
	}
}
