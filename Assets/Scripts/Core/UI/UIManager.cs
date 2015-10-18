/*
 * brief	Central control unit of UI
 * author	Oliver Kulas (oli@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * version 	1.2
 * date		Oct 2015
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

		// Coherent UI
		public CoherentUIView coherentUiView;
		public UIWrapper uiWrapper;
		public bool useCoherentUI = false;

		// Global UI Motion Values
		public GameObject canvas;
		public float uiMotionSpeedFactor = 10f;

		// UI Editor Values
		private float editorUIHeight = 1536f;
		private float editorUIWidth = 2048f;

		// StartScreen UI
		public GameObject startscreenUI;
        private RectTransform startScreenRect;
		private bool showStartScreen;
		private bool disposeStartScreen;
		private Vector2 startScreenOriginalPos = Vector2.zero;
		private Vector2 startScreenTargetPos = Vector2.zero;

		// MainHeader UI
		public GameObject mainHeader;
		private RectTransform mainHeaderRect; 
		private float mainHeaderHeight = 0f;
		private Vector2 mainHeaderDisposedPos = Vector2.zero;
		private Vector2 mainHeaderShowPos = Vector2.zero;

		// MainMenu UI
		public GameObject mainMenu;
		private RectTransform mainMenuRect;
		private float mainMenuWidth = 0f;
		private bool isMainMenuActive = true;
		private bool isMainMenuInMotion = false;
		private Vector2 mainMenuActivePos = Vector2.zero;
		private Vector2 mainMenuInactivePos = Vector2.zero; 

		// Debug
		public bool debugging = false;

		// Crane Thumbsticks
        private List<WbUIThumbstick> uiThumbsticks = new List<WbUIThumbstick>();
		private List<Wb3DThumbstick> meshThumbsticks;
		public WbCompCraneRCController craneRCController;
		
		// UI Elements / UI Members per Scene
        private UIElement[] uiElements;


		//-----------------------------------------------------------------------------
		// MonoBehaviour
		//-----------------------------------------------------------------------------

		void Awake(){
			if (!this.useCoherentUI){
				this.coherentUiView.gameObject.SetActive(false);
			}
		}

		//-----------------------------------------------------------------------------

		void Start() {

            if(this.useCoherentUI) this.coherentUiView.ReceivesInput = true;

            this.initWbUIThumbstickList();

            // get all uiElements for toggeling on and off
            this.uiElements = UIElement.FindObjectsOfType(typeof(UIElement)) as UIElement[];

			// set ui element positons paramenters for disposing/showing
            this.startScreenRect = this.startscreenUI.GetComponent<RectTransform>();
            this.startScreenOriginalPos = this.startScreenRect.anchoredPosition;
			this.startScreenTargetPos = new Vector2(this.startScreenOriginalPos.x, this.startScreenRect.anchoredPosition.y - (Screen.height * 2) );
                
                //new Vector3(this.startscreenUI.transform.localPosition.x, 
                //                                    this.startscreenUI.transform.localPosition.y - (Screen.height * 2),
                //                                    this.startscreenUI.transform.localPosition.z);

			// get rectTransforms and size of header & mainMenu elements
			this.mainHeaderRect = this.mainHeader.GetComponent<RectTransform>();
			this.mainHeaderHeight = this.mainHeaderRect.rect.height;

			this.mainMenuRect = this.mainMenu.GetComponent<RectTransform>();
			this.mainMenuWidth = this.mainMenuRect.rect.width;

			// set origin and target positions if MainHeader & MainMenu
            this.mainHeaderDisposedPos = new Vector2(this.mainHeaderRect.anchoredPosition.x, this.mainHeaderHeight);
            this.mainHeaderShowPos = new Vector2(this.mainHeaderRect.anchoredPosition.x, 0f);

			this.mainMenuActivePos = new Vector2(this.mainMenuRect.anchoredPosition.x - this.mainMenuWidth, this.mainMenuRect.anchoredPosition.y);
			this.mainMenuInactivePos = new Vector2(this.mainMenuRect.anchoredPosition.x , this.mainMenuRect.anchoredPosition.y);

            if (debugging) {
                Debug.Log("DEBUG Screen h/w " + Screen.height + "/" + Screen.width);
				Debug.Log("DEBUG start screen ui org pos: " + this.startScreenOriginalPos.ToString());
				Debug.Log("DEBUG start screen ui target pos: " + this.startScreenTargetPos.ToString());
                Debug.Log("DEBUG menu header show pos: " + this.mainHeaderShowPos.ToString());
                Debug.Log("DEBUG menu header dispose pos: " + this.mainHeaderDisposedPos.ToString());
                Debug.Log("DEBUG active menu pos: " + this.mainMenuActivePos.ToString());
                Debug.Log("DEBUG inactive menu pos: " + this.mainMenuInactivePos.ToString());
				Debug.Log ("DEBUG main menu WIDTH: " + this.mainMenuWidth);
            }
        }

		//-----------------------------------------------------------------------------

		void Update(){

			if(disposeStartScreen && !showStartScreen){
				this.toggleStartScreenUIPosition(this.startScreenTargetPos);
			}

			if(showStartScreen && !disposeStartScreen){
				this.toggleStartScreenUIPosition(this.startScreenOriginalPos);
			}

			if(this.isMainMenuInMotion){
				this.animateMenuMenu();
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

		public void toggleMainMenu(){

			if(!this.isMainMenuInMotion){
				this.isMainMenuInMotion = true;
			}/*else{
				this.isMainMenuInMotion = false;
			}*/
		}

		//-----------------------------------------------------------------------------

		public void toggleStartScreenUIPosition(Vector2 targetPos){

			float height = Screen.height;

			// DISPOSE
			if(disposeStartScreen && !showStartScreen){
				// StartScreen UI
				if(this.startScreenRect.anchoredPosition.y > -height){
                    Vector2 oldPos = this.startScreenRect.anchoredPosition;
					this.startScreenRect.anchoredPosition = Vector2.Lerp(oldPos, targetPos, Time.deltaTime * this.uiMotionSpeedFactor);
				}
				else{
					// MainHeader UI
					if(this.mainHeaderRect.anchoredPosition.y > this.mainHeaderShowPos.y){
						Vector2 oldPos = this.mainHeaderRect.anchoredPosition;
						this.mainHeaderRect.anchoredPosition = Vector2.Lerp (oldPos, this.mainHeaderShowPos, Time.deltaTime * this.uiMotionSpeedFactor);
					}else{
						this.disposeStartScreen = false;
						this.startscreenUI.gameObject.SetActive(false);
					}
				}
			}
			// SHOW
			else if (!disposeStartScreen && showStartScreen){
				// StartScreen UI
				this.startscreenUI.gameObject.SetActive(true);

                if (this.startScreenRect.anchoredPosition.y < 0f) {
                    Vector2 oldPos = this.startScreenRect.anchoredPosition;
                    this.startScreenRect.anchoredPosition = Vector2.Lerp(oldPos, targetPos, Time.deltaTime * this.uiMotionSpeedFactor);
				}
				else{
					this.showStartScreen = false;
				}

				// MainHeader UI
				if(this.mainHeaderRect.anchoredPosition.y < this.mainHeaderDisposedPos.y){
					Vector2 oldPos = this.mainHeaderRect.anchoredPosition;
					this.mainHeaderRect.anchoredPosition = Vector2.Lerp (oldPos, this.mainHeaderDisposedPos, Time.deltaTime * this.uiMotionSpeedFactor);
				}
			}
		}

		//-----------------------------------------------------------------------------

		public void animateMenuMenu(){

			if(this.isMainMenuActive){

				// Move in
				if(this.mainMenuRect.anchoredPosition.x < this.mainMenuInactivePos.x - 0.1f){
					Vector2 oldPos = this.mainMenuRect.anchoredPosition;
					this.mainMenuRect.anchoredPosition = Vector2.Lerp(oldPos, this.mainMenuInactivePos, Time.deltaTime * this.uiMotionSpeedFactor);
				}else{
					this.isMainMenuActive = false;
					this.isMainMenuInMotion = false;
					this.mainMenu.gameObject.SetActive(false);
				}

			}else{
				// Move out
				this.mainMenu.gameObject.SetActive(true);

				if(this.mainMenuRect.anchoredPosition.x > this.mainMenuActivePos.x +0.1f){
					Vector2 oldPos = this.mainMenuRect.anchoredPosition;
					this.mainMenuRect.anchoredPosition = Vector2.Lerp(oldPos, this.mainMenuActivePos, Time.deltaTime * this.uiMotionSpeedFactor);
				}else{
					this.isMainMenuActive = true;
					this.isMainMenuInMotion = false;
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
			if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("switchGameUI");
		}

        public void loadGameUI() {
			if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("loadGameUI");
        }

        public void unloadGameUI() {
			if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("unloadGameUI");
        }

        public void setConnectionLoadingBar() {
			if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("setConnectionLoadingBar");
        }

        public void unloadMainMenu(string setMenuInactive) {
            //if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("unloadMainMenu", setMenuInactive);
        }

        // CONNECTION
		public void setConnectionErrorMsg(string obj){
            //if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("setConnectionErrorMsg", obj);
		}

        public void showLoadingScreen() {
			if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("showLoadingScreen");
        }

        public void hideLoadingScreen() {
            if(this.useCoherentUI)this.coherentUiView.View.TriggerEvent("hideLoadingScreen");
        }

        //-----------------------------------------------------------------------------

		public void set3DThumbstickList(List<Wb3DThumbstick> meshThumbsticks){
			this.meshThumbsticks = meshThumbsticks;
		}

	}
}