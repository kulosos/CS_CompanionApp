/*
 * brief	
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.Game;

#if UNITY_IOS
using Coherent.UI.Mobile.Binding; // TriggerEvent binding with extra parameter on mobile 
#endif

#if UNITY_ANDROID
using Coherent.UI.Mobile.Binding; // TriggerEvent binding with extra parameter on mobile 
#endif

#if UNITY_EDITOR
using Coherent.UI.Binding; // TriggerEvent binding with extra parameter on desktop 
#endif

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.UI {

	public class UIManager : MonoBehaviour {

        //-----------------------------------------------------------------------------

		public CoherentUIView coherentUiView;
		public UIWrapper uiWrapper;
		public bool debugging = false;

        private List<WbUIThumbstick> uiThumbsticks = new List<WbUIThumbstick>();
		private List<Wb3DThumbstick> meshThumbsticks;

		//-----------------------------------------------------------------------------
		// MonoBehaviour
		//-----------------------------------------------------------------------------

		public void Start() {
			this.coherentUiView.ReceivesInput = true;
            this.initWbUIThumbstickList();
		}

        //-----------------------------------------------------------------------------
        
        // Get all Thumbsticks in scene
        private void initWbUIThumbstickList() {
            this.uiThumbsticks.Clear();
            WbUIThumbstick[] sticks = WbUIThumbstick.FindObjectsOfType(typeof(WbUIThumbstick)) as WbUIThumbstick[];
            foreach (WbUIThumbstick stick in sticks) {
                this.uiThumbsticks.Add(stick);
            }
        }

        //-----------------------------------------------------------------------------

        // toggle Thumbsticks in scene (e.g. RemoteControlCraneScene)
        public void toggleUIThumbsticks(string scene) {
            if (scene.Equals(SceneList.RemoteControlCrane)) {
                foreach (WbUIThumbstick uiStick in this.uiThumbsticks) {
                    uiStick.gameObject.SetActive(true);

					foreach(Wb3DThumbstick meshStick in this.meshThumbsticks){
						if(meshStick.thumbstickType.Equals(uiStick.thumbstickType)){
                            uiStick.transform.position = new Vector3(meshStick.UIThumbstickLocator.transform.position.x,
                                                                     meshStick.UIThumbstickLocator.transform.position.y,
                                                                     uiStick.transform.position.z);
						}
					}
                }
            } else {
                foreach (WbUIThumbstick stick in this.uiThumbsticks) {
                    stick.gameObject.SetActive(false);
                }
            }
        }

        //-----------------------------------------------------------------------------

        public List<WbUIThumbstick> getWbUIThumbsticks() {
            return this.uiThumbsticks;
        }

		//-----------------------------------------------------------------------------
		// Bind Methods to UI 
		//-----------------------------------------------------------------------------

        // UI ELEMENTS
		public void switchGameUI(){
			this.coherentUiView.View.TriggerEvent("switchGameUI");
		}

        public void loadGameUI() {
            this.coherentUiView.View.TriggerEvent("loadGameUI");
        }

        public void unloadGameUI() {
            this.coherentUiView.View.TriggerEvent("unloadGameUI");
        }

        public void setConnectionLoadingBar() {
            this.coherentUiView.View.TriggerEvent("setConnectionLoadingBar");
        }

        public void unloadMainMenu(string setMenuInactive) {
            this.coherentUiView.View.TriggerEvent("unloadMainMenu", setMenuInactive);
        }

        // CONNECTION
		public void setConnectionErrorMsg(string obj){
			this.coherentUiView.View.TriggerEvent("setConnectionErrorMsg", obj);
		}

        public void showLoadingScreen() {
            this.coherentUiView.View.TriggerEvent("showLoadingScreen");
        }

        public void hideLoadingScreen() {
            this.coherentUiView.View.TriggerEvent("hideLoadingScreen");
        }

        //-----------------------------------------------------------------------------

		public void set3DThumbstickList(List<Wb3DThumbstick> meshThumbsticks){
			this.meshThumbsticks = meshThumbsticks;
		}

	}
}