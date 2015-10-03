using UnityEngine;
using UnityEngine.UI;
using System;

// ENUM TYPE ------------------------------------------------------------------
public enum ArrowType { Up, Down }
//-----------------------------------------------------------------------------

public class UIRetailerItem : MonoBehaviour {

    public string retailItemName; 
    public int amount;

    public Text amountField;
    public int minAmount = 0;
    public int maxAmount = 10;

    //-------------------------------------------------------------------------
    // MonoBehaviour
    //-------------------------------------------------------------------------

    void Start() {
    }

    //-------------------------------------------------------------------------

    void Update() {
    }

    //-------------------------------------------------------------------------

    public void setAmountValueUp() {

        int x = 0;
        Int32.TryParse(this.amountField.text, out x);

        x++;
        x = Mathf.Clamp(x, this.minAmount, this.maxAmount);

        this.amount = x;
        this.amountField.text = x.ToString();
    }


    //-------------------------------------------------------------------------

    public void setAmountValueDown() {

        int x = 0;
        Int32.TryParse(this.amountField.text, out x);

        x--;
        x = Mathf.Clamp(x, this.minAmount, this.maxAmount);

        this.amount = x;
        this.amountField.text = x.ToString();
    }

}
