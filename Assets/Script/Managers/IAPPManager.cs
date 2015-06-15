using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

public class IAPPManager : MonoBehaviour
{
	protected static IAPPManager _instance;

	// Use this for initialization
	void Start () 
	{
		StoreEvents.OnMarketPurchaseStarted += onMarketPurchaseStarted;
		StoreEvents.OnMarketPurchase += onMarketPurchase;
		StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCancelled;

		StoreEvents.OnMarketItemsRefreshStarted += onMarketItemsRefreshStarted;
		StoreEvents.OnMarketItemsRefreshFinished += onMarketItemsRefreshFinished;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startPurchase()
	{
		Debug.Log ("Aqui");
		StoreInventory.BuyItem("premium_ver_ID");
	}

	public void onMarketPurchase(PurchasableVirtualItem pvi, string payload,
	                              Dictionary<string, string> extra) {
		// pvi - the PurchasableVirtualItem that was just purchased
		// payload - a text that you can give when you initiate the purchase operation and
		//    you want to receive back upon completion
		// extra - contains platform specific information about the market purchase
		//    Android: The "extra" dictionary will contain "orderId" and "purchaseToken"
		//    iOS: The "extra" dictionary will contain "receipt" and "token"
		
		// ... your game specific implementation here ...
		Debug.Log (pvi);
		Debug.Log ("Se compro");

		UserDataManager.instance.premiumVersion = "premiumVersion";
	}

	public void onMarketPurchaseStarted(PurchasableVirtualItem pvi) {
		// pvi - the PurchasableVirtualItem whose purchase operation has just started
		
		// ... your game specific implementation here ...
		Debug.Log ("Se Iniciara la compra");
	}

	public void onMarketPurchaseCancelled(PurchasableVirtualItem pvi) {
		// pvi - the PurchasableVirtualItem whose purchase operation was cancelled
		
		// ... your game specific implementation here ...
		Debug.Log ("Se cancelara la compra");
	}

	public void onMarketItemsRefreshStarted() 
	{
		// ... your game specific implementation here ...
	}

	public void onMarketItemsRefreshFinished(List<MarketItem> items) 
	{
		// items - the list of Market items that was fetched from the Market
		
		// ... your game specific implementation here ...
	}
}
