using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundboardSlot : MonoBehaviour 
{
	public int slotNum;

	public Image innerCircle;

	//edit state appearence
	public GameObject micIcon;
	public Color editColor = Color.red;
	public Color recordingColor;

	//when state changes, action changes
	private delegate void TouchAction();
	private TouchAction touchDownAction;
	private TouchAction touchUpAction;

	//save the original color of the slot
	private Color slotColor;
	private Color transparentColor;

	//components
	private AudioSource _audioSouce;
	private Image _image;

	private const int MaxRecordLength = 30;
	private ImageColorAnimation anim;

	public void OnTouchDown()
	{
		touchDownAction ();
	}

	public void OnTouchUp()
	{
		touchUpAction ();
	}

	void Start()
	{
		//references of the components
		_audioSouce = GetComponent<AudioSource>();
		_image = GetComponent<Image> ();

		//init original color
		slotColor = _image.color;
		transparentColor = new Color (0f, 0f, 0f, 0f);

		//init anim
		anim = new ImageColorAnimation (
			innerCircle,
			slotColor,
			transparentColor,
			1f //will reset duration every time the coroutine starts
		);

		//register to soundboard manager
		SoundboardManager.stateChangedEvent += StateChanged;

		//disable image when loading
		_image.enabled = false;
	}

	void OnDestroy()
	{
		SoundboardManager.stateChangedEvent -= StateChanged;

		//save (serialize) AudioClipq to local disk
		AudioDataSaver.SaveAudioClipToDisk(_audioSouce.clip, slotNum.ToString() + ".dat");
	}

	//set different touch down action for different states
	void StateChanged(SoundboardManager.SoundboardState state)
	{
		//prevent coroutines through state switches
		StopAllCoroutines();

		switch (state) {
		case SoundboardManager.SoundboardState.Default:

			//set touch actions
			touchDownAction = DefaultStateTouchDown;
			touchUpAction = EmptyFunc;

			//return to default color
			_image.color = slotColor;
			innerCircle.color = transparentColor;

			//enable image
			_image.enabled = true;

			//mic icon disappear
			micIcon.SetActive(false);

			break;

		case SoundboardManager.SoundboardState.Edit:

			//set touch actions
			touchDownAction = EditStateTouchDown;
			touchUpAction = EditStateTouchUp;

			//stop the AudioClip if it is playing
			_audioSouce.Stop();

			//change color
			_image.color = editColor;
			innerCircle.color = transparentColor;

			//mic icon appear
			micIcon.SetActive(true);

			break;

		case SoundboardManager.SoundboardState.Jam:
			
			break;
		}
	}

	void DefaultStateTouchDown()
	{
		//stop previous coroutine
		StopAllCoroutines ();

		//play sound
		_audioSouce.Play();

		//inner circle appears
		innerCircle.color = slotColor;

		//start coroutine
		anim.Duration = _audioSouce.clip.length;
		StartCoroutine (anim.AnimationCoroutine ());
	}

	void RecoverSlotColor()
	{
		innerCircle.color = transparentColor;
	}

	void EditStateTouchDown()
	{
		CancelInvoke();

		//if already recording other slots, return
		if (Microphone.IsRecording(null)) return;

		//start the mic
		_audioSouce.clip = Microphone.Start(null, false, MaxRecordLength, 44100);

		//force to play the music on the bottom speaker instead of the thin ear speaker
		#if UNITY_IOS
		iPhoneSpeaker.ForceToSpeaker();
		#endif

		//change color of the inner circle
		innerCircle.color = recordingColor;

		//if it exceeds maximum length, call touch up func
		Invoke("EditStateTouchUp", (float) MaxRecordLength);
	}

	void EditStateTouchUp()
	{
		//if is not recording, return
		if (!Microphone.IsRecording(null)) return; 

		//change back the color of the inner circle
		innerCircle.color = transparentColor;

		//get mic position (in samples)
		int lastTime = Microphone.GetPosition(null);

		//end mic
		Microphone.End (null);

		//if too short, return
		if (lastTime == 0) {
			return;
		}

		//get the full samples and store them in an array
		float[] samples = new float[_audioSouce.clip.samples];
		_audioSouce.clip.GetData (samples, 0);

		//store the clipped samples to a new array
		float[] clipSamples = new float[lastTime];
		Array.Copy (samples, clipSamples, clipSamples.Length);

		//create a new AudioClip and set the data from the clipped samples
		_audioSouce.clip = AudioClip.Create("new", clipSamples.Length, 1, 44100, false);
		_audioSouce.clip.SetData (clipSamples, 0);

		//change state
		SoundboardManager.ChangeState(SoundboardManager.SoundboardState.Default);
	}

	void EmptyFunc() {}
}
