using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Wb.Companion.Core.WbNetwork;

// ENUM TYPE ------------------------------------------------------------------
public enum ArrowType { Up, Down }
//-----------------------------------------------------------------------------

public class UIRetailerItem : MonoBehaviour {

	public WbCompRetailManager retailManager;

    public string retailItemName; 
    public int amount;

    public Text amountField;
    public int minAmount = 0;
    public int maxAmount = 10;

    //-------------------------------------------------------------------------

    public void setAmountValueUp() {

        int x = 0;
        Int32.TryParse(this.amountField.text, out x);

        x++;
        x = Mathf.Clamp(x, this.minAmount, this.maxAmount);

        this.amount = x;
        this.amountField.text = x.ToString();

		foreach(KeyValuePair<string, int> entry in this.retailManager.shoppingList){
			
			if(entry.Key.Equals(this.retailItemName)){
				
				// set new value for key
				this.retailManager.shoppingList[entry.Key] = this.amount;
				Debug.Log ("xxx: " + entry.Value);
			}
		}
    }
	
    //-------------------------------------------------------------------------

    public void setAmountValueDown() {

        int x = 0;
        Int32.TryParse(this.amountField.text, out x);

        x--;
        x = Mathf.Clamp(x, this.minAmount, this.maxAmount);

        this.amount = x;
        this.amountField.text = x.ToString();

		foreach(KeyValuePair<string, int> entry in this.retailManager.shoppingList){

			if(entry.Key.Equals(this.retailItemName)){

				// set new value for key
				this.retailManager.shoppingList[entry.Key] = this.amount;
				Debug.Log ("yyy: " + entry.Value);
			}
		}
    }
}
