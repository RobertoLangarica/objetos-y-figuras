using UnityEngine;
using System.Collections;
using Soomla.Store;

public class KSEconomy : IStoreAssets
{
	protected static IAPPManager _instance;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public int GetVersion() {
		return 0;
	}
	
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{};
	}
	
	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {PREMIUM_VERSION_LTVG};
	}
	
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] {};
	}
	
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{};
	} 
	
	// NOTE: Create non-consumable items using LifeTimeVG with PurchaseType of PurchaseWithMarket.
	public static VirtualGood PREMIUM_VERSION_LTVG = new LifetimeVG(
		"Premium",                             // Name
		"Get more ships!",                       // Description
		"premium_ver_ID",                          // Item ID
		new PurchaseWithMarket(               // Purchase type (with real money $)
	                       "premium_ver_prod_id",                      // Product ID
	                       0.99                                   // Price (in real money $)
	                       )
		);
}