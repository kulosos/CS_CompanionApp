using UnityEngine;
using System.Collections.Generic;
using Wb.Companion.Core.WbNetwork;

public class WbCompRetailManager : MonoBehaviour {

	public List<UIRetailerItem> retailItems = new List<UIRetailerItem>();
	public Dictionary<string, int> shoppingList = new Dictionary<string, int>();

	public WbLocationName currentLocationName;

    //-------------------------------------------------------------------------
    // MonoBehaviour
    //-------------------------------------------------------------------------

	void Awake() {
		foreach(UIRetailerItem item in this.retailItems){
			this.shoppingList.Add(item.retailItemName, 0);
		}
	}

	void Start () {
	
	}
    //-------------------------------------------------------------------------
	
	void Update () {
	
	}

	//-------------------------------------------------------------------------
	
	public void executePurchase(){
		
		foreach(KeyValuePair<string, int> entry in this.shoppingList){
			WbCompRPCWrapper.getInstance().networkView.RPC("addItemToShoppingListExternal", RPCMode.Server, this.currentLocationName.ToString(), entry.Key, entry.Value);
		}
		
		WbCompRPCWrapper.getInstance().networkView.RPC("executeExternalPurchase", RPCMode.Server);
	}
}

public enum WbLocationName {

    HomebaseVillage = 0,
    HomebaseCity = 1,
    HarbourCity = 2,
    TrainStationVillage = 3,
    TrainStationCity = 4,
    MaterialsTraderVillage = 5,
    MaterialsTraderCity = 6,
    VehicleTraderCity = 7,
    WallFactoryCity = 8,
    SteelFactoryCity = 9,
    NurseryGardenVillage = 10,
    Sandbox = 11,
    SawmillVillage = 12,
    SawmillCity = 13,
    //Dam = 14,
    Mission = 98,
    Undefined = 99,
    Custom = 100
}
