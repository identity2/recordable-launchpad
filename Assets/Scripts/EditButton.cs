using UnityEngine;
using UnityEngine.UI;

//Default: Edit, Edit: Cancel, Jam: Inactive
public class EditButton : MonoBehaviour 
{

	public Color editColor;
	public Image micImage;
	public Image circleImage;

	private delegate void ButtonClickedAction ();
	private ButtonClickedAction buttonClicked;

	private Color defaultColor;

	private Image _image;

	private Button button { get { return GetComponent<Button> (); } }

	void Start()
	{
		button.interactable = false;

		_image = GetComponent<Image> ();

		defaultColor = _image.color;

		button.onClick.AddListener (() => {
			buttonClicked ();
		});

		SoundboardManager.stateChangedEvent += StateChanged;
	}

	void OnDestroy()
	{
		SoundboardManager.stateChangedEvent -= StateChanged;
	}

	void DefaultStateClicked()
	{
		//change to edit state
		SoundboardManager.ChangeState (SoundboardManager.SoundboardState.Edit);
	}

	void EditStateClicked()
	{
		//if mic is on, cannot cancel
		if (Microphone.IsRecording(null)) return;

		//cancel edit, switch back to default state
		SoundboardManager.ChangeState(SoundboardManager.SoundboardState.Default);
	}

	void StateChanged(SoundboardManager.SoundboardState state)
	{
		button.interactable = true;

		switch (state) {
		case SoundboardManager.SoundboardState.Default:

			//set action
			buttonClicked = DefaultStateClicked;

			//set image
			micImage.gameObject.SetActive(true);
			circleImage.gameObject.SetActive(false);

			//set color
			_image.color = defaultColor;
			break;
		case SoundboardManager.SoundboardState.Edit:

			//set action
			buttonClicked = EditStateClicked;

			//set text
			micImage.gameObject.SetActive(false);
			circleImage.gameObject.SetActive(true);

			//set color
			_image.color = editColor;
			break;
		case SoundboardManager.SoundboardState.Jam:

			break;
		}
	}
}
