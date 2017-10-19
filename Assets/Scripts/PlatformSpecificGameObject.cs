using UnityEngine;

public class PlatformSpecificGameObject : MonoBehaviour 
{
	public bool android;
	public bool ios;

	void Start()
	{
		#if UNITY_ANDROID
		if (!android)
		{
			Destroy(gameObject);
		}
		#endif

		#if UNITY_IOS
		if (!ios)
		{
			Destroy(gameObject);
		}
		#endif
	}

}
