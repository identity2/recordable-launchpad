using System.Collections;
using UnityEngine;

//load the audio clips from the disk and save the current state of the sound board
public class SoundboardManager : MonoBehaviour
{
	public SoundboardSlot[] slots;

	public enum SoundboardState {Default, Edit, Jam};

	public GameObject loadingCanvas;

	public delegate void StateChangeAction(SoundboardState state);
	public static event StateChangeAction stateChangedEvent;

	//so that other game objects could change the state
	public static void ChangeState(SoundboardState state)
	{
		stateChangedEvent(state);
	}

	void Update()
	{
		#if UNITY_IOS
		iPhoneSpeaker.ForceToSpeaker();
		#endif
	}

	void Start()
	{
		//load audio from files
		StartCoroutine(LoadAudioClips());

		//dunno y i have to do this to make the mic work...
		Microphone.Start(null, false, 1, 44100);

		//make audio come out of iphone speaker instead of ear speaker
		#if UNITY_IOS
		iPhoneSpeaker.ForceToSpeaker();
		#endif

		Microphone.End(null);

	}


	IEnumerator LoadAudioClips()
	{
		for (int i = 0; i < slots.Length; i++) {
			AudioDataSaver.LoadAudioClipFromDisk(slots[i].GetComponent<AudioSource>(), i.ToString() + ".dat");
		}

		//wait for end of frame so that all the Start() has been called
		yield return new WaitForEndOfFrame();

		//finish loading
		stateChangedEvent(SoundboardState.Default);

		//deactivate loading sceen
		yield return new WaitForEndOfFrame();
		loadingCanvas.SetActive(false);
	}
}
