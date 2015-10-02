using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// ENUM TYPE ------------------------------------------------------------------
public enum ArrowType { Up, Down }
//-----------------------------------------------------------------------------

public class UIArrowBtn : MonoBehaviour {

    public ArrowType arrowType;
    public Text textField;

    //-------------------------------------------------------------------------
    // MonoBehaviour
    //-------------------------------------------------------------------------

	void Start () {
	
	}

    //-------------------------------------------------------------------------
	
	void Update () {
	
	}

    //-------------------------------------------------------------------------

    public void setAmountValue() {

        int x = 0;
        Int32.TryParse(this.textField.text, out x);

        if(arrowType.Equals(ArrowType.Up)){
            x++;
        }

        if (arrowType.Equals(ArrowType.Down)) {
            x--;

            //Mathf.Clamp(x, 0, 50);
        }
        
    }

}
