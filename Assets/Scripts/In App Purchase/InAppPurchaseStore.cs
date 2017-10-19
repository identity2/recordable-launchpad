using UnityEngine;

public class InAppPurchaseStore : MonoBehaviour
{

	public void BuyRemoveAds()
	{
		InAppPurchaser.Instance.BuyRemoveAds ();
	}

	public void RestorePurchase()
	{
		InAppPurchaser.Instance.RestorePurchases ();
	}
		
}
