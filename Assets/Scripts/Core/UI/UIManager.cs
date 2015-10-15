/*
 * brief	Central control unit of UI
 * author	Oliver Kulas (oli@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		June 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
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

		public float uiMotionDampingFactor = 5f;

		// StartScreen UI
		public GameObject startscreenUI;
		private bool showStartScreen;
		private bool disposeStartScreen;
		private Vector3 startScreenOriginalPos = Vector3.zero;
		private Vector3 startScreenTargetPos = Vector3.zero;

		// MainHeader & MainMenu UI
		public GameObject mainHeader;
		public GameObject mainMenu;


		public bool debugging = false;

        private List<WbUIThumbstick> uiThumbsticks = new List<WbUIThumbstick>();
		private List<Wb3DThumbstick> meshThumbsticks;

        private UIElement[] uiElements;

		public WbCompCraneRCController craneRCController;

		//-----------------------------------------------------------------------------
		// MonoBehaviour
		//-----------------------------------------------------------------------------

		void Start() {

			this.coherentUiView.ReceivesInput = true;
            this.initWbUIThumbstickList();

            // get all uiElements for toggeling on and off
            this.uiElements = UIElement.FindObjectsOfType(typeof(UIElement)) as UIElement[];

			Debug.Log ("Screen h/w " + Screen.height + "/" + Screen.width);

			// set start screen ui paramenter for disposing
			this.startScreenOriginalPos = this.startscreenUI.transform.localPosition;
			this.startScreenTargetPos = new Vector3(this.startscreenUI.transform.localPosition.x, 
			                                        this.startscreenUI.transform.localPosition.y - (Screen.height * 2),
			                                        this.startscreenUI.transform.localPosition.z);

			//HACK for debug in Unity Editor, because weired results of given screen size
			#if UNITY_EDITOR
			this.startScreenTargetPos = new Vector3(this.startscreenUI.transform.localPosition.x, 
			                                        this.startscreenUI.transform.localPosition.y - (1600f),
			                                        this.startscreenUI.transform.localPosition.z);
			#endif
        }

		//-----------------------------------------------------------------------------

		void Update(){

			if(disposeStartScreen && !showStartScreen){
				this.toggleStartScreenUIPosition(this.startScreenTargetPos, true);
			}

			if(showStartScreen && !disposeStartScreen){
				this.toggleStartScreenUIPosition(this.startScreenOriginalPos, false);
			}
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

        public void initUIElementsPerScene(string scene){

            // toggeling UIElements on and off accordingly to scene
            foreach (UIElement uie in this.uiElements) {

                if (SceneManager.getInstance().getSceneId(scene) == (int)uie.uiElementMember && !uie.hideOnInitialStart) {

                    uie.gameObject.SetActive(true);

					// HACK - Companion App
					// Because the Crane RC Funciton Buttons are loading to late / call order is wrong
					if(SceneManager.getInstance().getCurrentScene().Equals(SceneList.RemoteControlCrane)){
						craneRCController.setFunctionBtnPositions();
					}

                } else {
                    uie.gameObject.SetActive(false);
                } 
            }
        }

        //-----------------------------------------------------------------------------

        // setPosition and toggle Thumbsticks in scene (e.g. RemoteControlCraneScene)
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

		public void showStartScreenUI(){
			this.showStartScreen = true;
			this.disposeStartScreen = false;
		}

		//-----------------------------------------------------------------------------

		public void diposeStartScreen(){
			this.disposeStartScreen = true;
			this.showStartScreen = false;
		}

		//-----------------------------------------------------------------------------

		public void toggleStartScreenUIPosition(Vector3 targetPos, bool setInactive){

			float height;

			//HACK for debug in Unity Editor, because weired results of given screen size
			#if UNITY_IOS
			height = Screen.height *2;
			#endif
			
			#if UNITY_ANDROID
			height = Screen.height *2;
			#endif
			
			#if UNITY_EDITOR
			height = 1598f;
			#endif

			// DISPOSE
			if(disposeStartScreen && !showStartScreen){

				if(this.startscreenUI.transform.localPosition.y > -height){
					Vector3 oldPos = startscreenUI.transform.localPosition;
					startscreenUI.transform.localPosition = Vector3.Lerp(oldPos, targetPos, Time.deltaTime * this.uiMotionDampingFactor);
				}else{
					this.disposeStartScreen = false;
					this.startscreenUI.gameObject.SetActive(false);
				}
			}
			// SHOW
			else if (!disposeStartScreen && showStartScreen){

				this.startscreenUI.gameObject.SetActive(true);

				if(this.startscreenUI.transform.localPosition.y < 0.05f){
					Vector3 oldPos = startscreenUI.transform.localPosition;
					startscreenUI.transform.localPosition = Vector3.Lerp(oldPos, targetPos, Time.deltaTime * this.uiMotionDampingFactor);
				}else{
					this.showStartScreen = false;
				}
			}

		}

        //-----------------------------------------------------------------------------

        public List<WbUIThumbstick> getWbUIThumbsticks() {
            return this.uiThumbsticks;
        }

		//-----------------------------------------------------------------------------

		public UIElement[] getUIElements(){
			return this.uiElements;
		}

		//-----------------------------------------------------------------------------
		// Bind Methods to Coherent UI 
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