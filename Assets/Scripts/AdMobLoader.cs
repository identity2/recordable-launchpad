using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobLoader : MonoBehaviour 
{
	private static BannerView bannerView = null;

	void Start()
	{
		InAppPurchaser.completeLoadingEvent += InAppPurchaseLoaded;
	}

	void OnDestroy()
	{
		InAppPurchaser.completeLoadingEvent -= InAppPurchaseLoaded;
	}

	//determine if the user bought "Remove Ads"
	void InAppPurchaseLoaded()
	{
		if (!InAppPurchaser.Instance.localData.noAds)
		{
			RequestBanner();
		}
	}

	void RequestBanner()
	{
		string adUnitId;

		#if UNITY_EDITOR
		adUnitId = "unused";

		#elif UNITY_ANDROID
		Debug.Log("ANDROID");
		adUnitId = "ca-app-pub-3679599074148025/3230231995";

		#elif UNITY_IOS
		adUnitId = "ca-app-pub-3679599074148025/9091856394";

		#else
		adUnitId = "unexpected_platform";

		#endif

		//create a banner
		bannerView = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);

		//create an empty ad request
		//AdRequest request = new AdRequest.Builder ().Build ();

		//test device
		Debug.Log("HAVE ADS");
		AdRequest request = new AdRequest.Builder ().Build ();

		//load the banner with the request
		bannerView.LoadAd (request);
	}

	public static void DestroyBannerView()
	{
		if (bannerView != null)
			bannerView.Destroy();
	}
}
