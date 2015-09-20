/**
* @brief		Basic implementation of WbAxisButton
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			September 2015
**/

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Wb.Companion.Core.WbNetwork;

//-----------------------------------------------------------------------------
namespace Wb.Companion.Core.Inputs {

    public class WbAxisButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

		//-----------------------------------------------------------------------------
		// Member
		//-----------------------------------------------------------------------------

        [SerializeField]
        private string axisName = "Stick1Horizontal";
        [SerializeField]
        private float targetValue = 1f;
        [SerializeField]
        private float responseSpeed = 3;

        [SerializeField]
        private bool isPressed = false;
        [SerializeField]
        private float value = 0;
        
		//-----------------------------------------------------------------------------
		// Monoehaviour
		//-----------------------------------------------------------------------------

        public void Start() {
        }

        //-----------------------------------------------------------------------------

        public void Update() {

            if (this.isPressed) {
                value = Mathf.MoveTowards(value, this.targetValue, this.responseSpeed * Time.deltaTime);

                WbCompStateSyncSending.getInstance().setVehicleInput(this.axisName, value);
            }
        }

		//-----------------------------------------------------------------------------
		// Interface implementations
		//-----------------------------------------------------------------------------

        public void OnPointerDown(PointerEventData eventData) {

            this.isPressed = true;
        }

        //-----------------------------------------------------------------------------
                
        public void OnPointerUp(PointerEventData eventData) {

            this.isPressed = false;
            this.value = 0f;

            WbCompStateSyncSending.getInstance().setVehicleInput(this.axisName, value);
        }
    }	
}
