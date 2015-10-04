using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Wb.Companion.Core.WbNetwork;

namespace Wb.Companion.Core.UI
{
	// ENUM TYPE ------------------------------------------------------------------
	public enum ArrowType { Up, Down }
	//-----------------------------------------------------------------------------

	public class UIRetailerItem : MonoBehaviour {

		public WbCompRetailManager retailManager;

	    public string retailItemName; 
		public float price;
	    public int amount;

		public Text priceField;
	    public Text amountField;
	    public int minAmount = 0;
	    public int maxAmount = 10;

	    //-------------------------------------------------------------------------
		// MonoBehaviour
		//-------------------------------------------------------------------------

		void Start(){
			this.priceField.text = price.ToString();
		}

		//-------------------------------------------------------------------------

	    public void setAmountValue(bool positive) {

	        int x = 0;
	        Int32.TryParse(this.amountField.text, out x);

			x += (positive) ? 1 : -1;
	        x = Mathf.Clamp(x, this.minAmount, this.maxAmount);

	        this.amount = x;
	        this.amountField.text = x.ToString();

			// set amount dictionary value for retail item accordingly
			this.retailManager.shoppingList[this.retailItemName] = this.amount;

			retailManager.calcTotalPrice();
	    }
	}
}
