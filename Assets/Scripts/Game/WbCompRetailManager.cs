using UnityEngine;
using System.Collections.Generic;
using Wb.Companion.Core.WbNetwork;
using UnityEngine.UI;

namespace Wb.Companion.Core.UI
{
	public class WbCompRetailManager : MonoBehaviour {

		public static WbCompRetailManager instance;

		public List<UIMenuItem> menuItems = new List<UIMenuItem>();
		public List<UIRetailSubMenu> retailSubMenus = new List<UIRetailSubMenu>();
		public List<UIRetailerItem> retailItems = new List<UIRetailerItem>();
		public Dictionary<string, int> shoppingList = new Dictionary<string, int>();

		public UIMenuItem initialMenuItem;

		public float totalPrice = 0f;
		public Text totalPriceField;

		public WbLocationName currentRetailLocationName = WbLocationName.Undefined;

	    //-------------------------------------------------------------------------
	    // MonoBehaviour
	    //-------------------------------------------------------------------------

		void Awake() {

			WbCompRetailManager.instance = this;

			foreach(UIRetailerItem item in this.retailItems){
				this.shoppingList.Add(item.retailItemName, 0);
			}
		}

		void Start(){
			switchUISubMenu(initialMenuItem);
		}
	
		//-------------------------------------------------------------------------

		public static WbCompRetailManager getInstance(){
			return WbCompRetailManager.instance;
		}

		//-------------------------------------------------------------------------

		public void calcTotalPrice(){

			float sum = 0f;
			foreach(UIRetailerItem item in this.retailItems){
				sum += item.price * item.amount;
			}

			totalPrice = sum;
			totalPriceField.text = sum.ToString();
		}

		//-------------------------------------------------------------------------

		public void switchUISubMenu(UIMenuItem menuItem){

			WbLocationName location = menuItem.location;
			WbCompRetailSubMenu retailSubMenu = menuItem.retailSubMenu;

			// toggle sub menu contents
			foreach(UIRetailSubMenu subMenu in retailSubMenus){
				if(subMenu.location.Equals(location) && subMenu.retailSubMenu.Equals(retailSubMenu)){
					subMenu.gameObject.SetActive(true);
				}else{
					subMenu.gameObject.SetActive(false);
				}
			}

			// set color of menu buttons
			foreach(UIMenuItem item in menuItems){
				if(item.location.Equals(location) && item.retailSubMenu.Equals(retailSubMenu)){
					menuItem.img.color = new Color(0f, 0f, 0f, 0.5f);
				}else{
					item.img.color =  new Color(0f, 0f, 0f, 0.25f);
				}
			}
		}

		//-------------------------------------------------------------------------

		public void executePurchase(){
			
			foreach(KeyValuePair<string, int> entry in this.shoppingList){
		
				WbCompRPCWrapper.getInstance().networkView.RPC("addItemToShoppingListExternal", RPCMode.Server, this.currentRetailLocationName.ToString(), entry.Key, entry.Value);
			}
			
			WbCompRPCWrapper.getInstance().networkView.RPC("executeExternalPurchase", RPCMode.Server);
		}

		//-------------------------------------------------------------------------

		public void closeWindow(GameObject go){
			go.transform.gameObject.SetActive(false);
		}

		//-------------------------------------------------------------------------

		public void openSubMenu(GameObject go){
			go.transform.gameObject.SetActive(false);
		}
	}

	//-----------------------------------------------------------------------------

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
}
