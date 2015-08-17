using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Wb.Companion.Core.WbNetwork;
using UnityEngine.UI;
using Wb.Companion.Core.Inputs;
using Wb.Companion.Core.UI;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.Game {
	
    public class Wb3DThumbstick : MonoBehaviour {

        private List<Wb3DThumbstick> meshThumbsticks = new List<Wb3DThumbstick>();
		private UIManager uiManager;
		public ThumbstickType thumbstickType;

        //-----------------------------------------------------------------------------
        // Mono Behaviour
        //-----------------------------------------------------------------------------

        void Start() {
            this.init3DThumbsticks();
        }

        void Update() {
        }

        //-----------------------------------------------------------------------------

        private void init3DThumbsticks() {

			this.uiManager = UIManager.FindObjectOfType(typeof(UIManager)) as UIManager;
			List<WbUIThumbstick> uiSticks = this.uiManager.getWbUIThumbsticks();
            Wb3DThumbstick[] meshSticks = Wb3DThumbstick.FindObjectsOfType(typeof(Wb3DThumbstick)) as Wb3DThumbstick[];

			// Assign MeshThumbstick to appropriate UIThumbstick
            foreach (Wb3DThumbstick meshStick in meshSticks) {
                this.meshThumbsticks.Add(meshStick);

				foreach (WbUIThumbstick uiStick in uiSticks){
					if(meshStick.thumbstickType.Equals(uiStick.thumbstickType)){
						uiStick.meshThumbstick = meshStick;
					}
				}

            }

        }
    }

}
