/**
* @brief		Basic implementation of WbAxisButton
*
* @author		Thomas Mueller (thomas.mueller@weltenbauer-se.com)
*
* @date			July 2015
* 
*/

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

        private bool isPressed = false;
        
		//-----------------------------------------------------------------------------
		// Properties
		//-----------------------------------------------------------------------------

		//-----------------------------------------------------------------------------
		// WBehaviour
		//-----------------------------------------------------------------------------

        public void Start() {

            //WbInputManager.VirtualInput.RegisterAxis(new WbVirtualAxis(this.axisName));
        }

        //-----------------------------------------------------------------------------

        public void Update() {

            if (this.isPressed) {
                //WbInputManager.VirtualInput.SetAxisValue(this.axisName, Mathf.MoveTowards(WbInputManager.VirtualInput.GetAxis(this.axisName), this.targetValue, this.responseSpeed * Time.deltaTime));
            }
        }

		//-----------------------------------------------------------------------------
		// Methods
		//-----------------------------------------------------------------------------

        public void OnPointerDown(PointerEventData eventData) {


            this.isPressed = true;
        }

        //-----------------------------------------------------------------------------
                
        public void OnPointerUp(PointerEventData eventData) {

            this.isPressed = false;
            //WbInputManager.VirtualInput.SetAxisValue(this.axisName, 0f);
        }
    }	
}
