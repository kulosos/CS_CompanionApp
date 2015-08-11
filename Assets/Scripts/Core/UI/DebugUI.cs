using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Wb.Companion.Core.WbNetwork;
using Wb.Companion.Core.Inputs;

public class DebugUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  {

	public bool accelerateBtn = false;
	public bool brakeBtn = false;
	private bool pressed = false;

	
	// --------------------------------------------------------------------

	void Update() {

		if(accelerateBtn){
			if (pressed && NetworkManager.getInstance().isActiveConnection) {
				WbCompRPCWrapper.getInstance().setThrottle("accelerate", 1.0f);
			} else{
                WbCompRPCWrapper.getInstance().setThrottle("accerlerate", 0.0f);
			}
		}

		if(brakeBtn){
			if (pressed && NetworkManager.getInstance().isActiveConnection) {
                WbCompRPCWrapper.getInstance().setThrottle("brake", 1.0f);
			} else{
                WbCompRPCWrapper.getInstance().setThrottle("brake", 0.0f);
			}
		}
	}

	// --------------------------------------------------------------------

    public void OnPointerDown(PointerEventData eventData){
		//Debug.Log ("HoldFire - OnPointerDown");
		pressed = true;
        //NetworkManager.getInstance().holdFire("accerlerate", 1.0f);
    }

    public void OnPointerUp(PointerEventData eventData) {
		//Debug.Log ("HoldFire - OnPointerUp");
		pressed = false;
        //NetworkManager.getInstance().holdFire("accerlerate", 0.0f);
    }

	
}
