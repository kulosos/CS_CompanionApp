/**
* @brief		Basic implementation of WbUIThumbstick
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			June 2015
*/

//-----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Wb.Companion.Core;
using Wb.Companion.Core.WbNetwork;
using UnityEngine.UI;
using Wb.Companion.Core.Inputs;
using Wb.Companion.Core.Game;
using System.Collections.Generic;
using Wb.Companion.Core.WbAdministration;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.UI {

    // ENUM TYPES -------------------------------------------------------------

    public enum WbUIAxisOption { Both, OnlyHorizontal, OnlyVertical }

    public enum ThumbstickType { Left = 0, Middle = 1, Right = 2 };

	public enum ThumbstickAxis { UP = 0, DOWN = 1, LEFT = 2, RIGHT = 3 };

    //-------------------------------------------------------------------------

    public class WbUIThumbstick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

		public Wb3DThumbstick meshThumbstick;
		private Camera uiCamera;
		public ThumbstickType thumbstickType;
		public float resetDamping = 5f;
		public float inputThreshold = 0.01f;
		public bool isActive = false;

        [SerializeField]
        private int movementRange = 100;
        [SerializeField]
        private WbUIAxisOption axisToUse = WbUIAxisOption.Both;
        [SerializeField]
        private string horizontalAxisName = "Stick1Horizontal";
        [SerializeField]
        private string verticalAxisName = "Stick1Vertical";
        [SerializeField]
        private UnityEngine.UI.Image indicator;
        [SerializeField]
        private RectTransform wrapperRect;
        [SerializeField]
        private UnityEngine.UI.Image iconLeft;
        [SerializeField]
        private UnityEngine.UI.Image iconRight;
        [SerializeField]
        private UnityEngine.UI.Image iconTop;
        [SerializeField]
        private UnityEngine.UI.Image iconBottom;

        [SerializeField]
        private Color iconStartColor = new Color(1f, 1f, 1f, 0.25f);
        [SerializeField]
        private Color iconHoverColor = new Color(1f, 1f, 1f, 0.785f);
        [SerializeField]
        private float iconHoverValue = 0.3f;

        // private
        private Vector2 startPos;
        private Vector2 thumbstickDelta;
        private bool useX;
        private bool useY;
		public bool isReleased = false;
        private float timeSinceLastStart = 0;

		// debugging
		public bool debugging = false;
		private DebugUI[] debugUIElements;

		public Text DebugLabelThumbstickUp;
		public Text DebugLabelThumbstickDown;
		public Text DebugLabelThumbstickLeft;
		public Text DebugLabelThumbstickRight;


        //---------------------------------------------------------------------
        // MonoBehaviour
        //---------------------------------------------------------------------

        void Start() {

            // Get UI Camera
            Camera[] cameras = Camera.FindObjectsOfType(typeof(Camera)) as Camera[];
            foreach (Camera cam in cameras)
            {
                if (cam.gameObject.layer == 5) {  // Layer 5 == UI
                    this.uiCamera = cam;
                }
            }

            // Toggle and init Debug Labels for Thumbsticks
            this.DebugLabelThumbstickUp.gameObject.SetActive(this.debugging);
            this.DebugLabelThumbstickDown.gameObject.SetActive(this.debugging);
            this.DebugLabelThumbstickLeft.gameObject.SetActive(this.debugging);
            this.DebugLabelThumbstickRight.gameObject.SetActive(this.debugging);

            this.debugUIElements = DebugUI.FindObjectsOfType(typeof(DebugUI)) as DebugUI[];

        }

		void Update() {

            // send thumbstick values frame rate independent
            if (SceneManager.getInstance().currentScene.Equals(SceneList.RemoteControlCrane) && NetworkManager.getInstance().isActiveConnection) {
                // send Thumbstick Values every 1/rate second (e.g 1/15 = 15 times per second)
                // should send data when touch input position don't change and touch is only OnPointerDown
                if (timeSinceLastStart >= 1f / NetworkManager.getInstance().globalRPCSendRate) {
                    this.updateInputValues(this.thumbstickDelta);
                    timeSinceLastStart = 0;
                }
                timeSinceLastStart += Time.deltaTime;

                //Snap 3DThumbstick back to middle postion, when touch ends
                if (isReleased) {
                    this.snap3DThumbstickBack();
                }
            }
            
		}

        //-----------------------------------------------------------------------------
        // Interface Implementations
        //-----------------------------------------------------------------------------

        public void OnPointerUp(PointerEventData data) {
            if(debugging)Debug.Log("OnPointerUp");
            this.indicator.rectTransform.anchoredPosition = new Vector2(0, 0);
            this.SetIconsActive(false);
            this.UpdateAxes(this.indicator.rectTransform.anchoredPosition);
            this.isReleased = true;
        }

        //-----------------------------------------------------------------------------

        public void OnPointerDown(PointerEventData data) {
            if(debugging)Debug.Log("OnPointerDown");
            this.SetIconsActive(true);
            this.UpdateIndicator(data);
        }

        //-----------------------------------------------------------------------------

        public void OnDrag(PointerEventData data) {
            if(debugging)Debug.Log("OnDrag");
            this.UpdateIndicator(data);
            this.isReleased = false;
        }

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

        private void UpdateIndicator(PointerEventData data) {

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.wrapperRect, data.position, this.uiCamera, out pos);

            if (this.useX) {
                pos.x = Mathf.Clamp(pos.x, -this.movementRange, this.movementRange);
            }
            if (this.useY) {
                pos.y = Mathf.Clamp(pos.y, -this.movementRange, this.movementRange);
            }

            this.UpdateAxes(pos);

            // Clamp visuals to Circle
            if (this.axisToUse == WbUIAxisOption.Both) {
                pos = Vector2.ClampMagnitude(pos, this.movementRange);
            }
            this.indicator.rectTransform.anchoredPosition = pos;
        }

        //-----------------------------------------------------------------------------

        private void UpdateAxes(Vector2 value) {

            Vector2 delta = this.startPos - value;
            delta.x = -delta.x;
            delta.y = -delta.y;
            delta /= this.movementRange;
            
            //this.SimulateHover(delta);

            //Debug.Log ("Delta (x/y): " + delta);

            this.thumbstickDelta = delta;

            //this.updateInputValues(delta);
        }

		//-----------------------------------------------------------------------------

		private void updateInputValues(Vector2 delta){

			// ------------------------------------------------------- //
			// Blendshape Order: 0 = UP, 1 = DOWN, 2 = LEFT, 3 = RIGHT //
			// ------------------------------------------------------- //
			
			// Thumbstick UP / Y-AXIS
			if(delta.y > 0 && delta.y > this.inputThreshold){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.y)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 0);
				
				// send input key for vehicle interaction
				if(this.thumbstickType.Equals(ThumbstickType.Left)){
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_01_UP, Mathf.Clamp01(delta.y));
				}
				if(this.thumbstickType.Equals(ThumbstickType.Right)){
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_ROPE_UP, Mathf.Clamp01(delta.y));
				}
				// debugging
				this.updateThumbstickDebugUI(ThumbstickAxis.UP, Mathf.Clamp01(delta.y));
			}

			// Thumbstick DOWN / Y-AXIS
			if(delta.y < 0 && Mathf.Abs(delta.y) > this.inputThreshold){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.y)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
				
				// send input key for vehicle interaction
				if (this.thumbstickType.Equals(ThumbstickType.Left)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_01_DOWN, Mathf.Clamp01(Mathf.Abs(delta.y)));
				}
				if (this.thumbstickType.Equals(ThumbstickType.Right)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_ROPE_DOWN, Mathf.Clamp01(Mathf.Abs(delta.y)));
				}
				// debugging
				this.updateThumbstickDebugUI(ThumbstickAxis.DOWN, Mathf.Clamp01(Mathf.Abs(delta.y)));
			}

			// Thumbstick RESET Y-AXIS
			if( (Mathf.Abs(delta.y) > 0 || delta.y == 0) && Mathf.Abs(delta.y) < this.inputThreshold){
				
				// send input key for vehicle interaction
				if (this.thumbstickType.Equals(ThumbstickType.Left)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_01_UP, 0f);
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_01_DOWN, 0f);
				}
				if (this.thumbstickType.Equals(ThumbstickType.Right)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_ROPE_UP, 0f);
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_ROPE_DOWN, 0f);
				}
				// debugging
				this.updateThumbstickDebugUI(ThumbstickAxis.UP, delta.y);
				this.updateThumbstickDebugUI(ThumbstickAxis.DOWN, delta.y);
			}

			// ----------------------------------------

			// Thumbstick LEFT / X-AXIS
			if(delta.x < 0 && Mathf.Abs(delta.x) > this.inputThreshold){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.x)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(3, 0);
				
				// send input key for vehicle interaction
				if (this.thumbstickType.Equals(ThumbstickType.Left)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_MAIN_LEFT,  Mathf.Clamp01(Mathf.Abs(delta.x)));
				}
				if (this.thumbstickType.Equals(ThumbstickType.Right)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_FORWARD,  Mathf.Clamp01(Mathf.Abs(delta.x)));
				}
				// debugging
				this.updateThumbstickDebugUI(ThumbstickAxis.LEFT, Mathf.Clamp01(Mathf.Abs(delta.x)));
			}

			// Thumbstick RIGHT / X-AXIS
			if(delta.x > 0 && delta.x > this.inputThreshold){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.x)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(3, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, 0);
				
				// send input key for vehicle interaction
				if (this.thumbstickType.Equals(ThumbstickType.Left)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_MAIN_RIGHT, Mathf.Clamp01(delta.x));
				}
				if (this.thumbstickType.Equals(ThumbstickType.Right)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_BACKWARD, Mathf.Clamp01(delta.x));
				}
				// debugging
				this.updateThumbstickDebugUI(ThumbstickAxis.RIGHT, Mathf.Clamp01(delta.x));
			}

			// Thumbstick RESET X-AXIS
			if( (Mathf.Abs(delta.x) > 0 || delta.x == 0) && Mathf.Abs(delta.x) < this.inputThreshold){

				// send input key for vehicle interaction
				if (this.thumbstickType.Equals(ThumbstickType.Left)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_MAIN_LEFT, 0f);
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_MAIN_RIGHT, 0f);
				}
				if (this.thumbstickType.Equals(ThumbstickType.Right)) {
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_FORWARD, 0f);
					WbCompStateSyncSending.getInstance().setVehicleInput(InputKeys.TRUCKCRANE_BOOM_BACKWARD, 0f);
				}
				// debugging
				this.updateThumbstickDebugUI(ThumbstickAxis.LEFT, delta.x);
				this.updateThumbstickDebugUI(ThumbstickAxis.RIGHT, delta.x);
			}
		}

        //-----------------------------------------------------------------------------

		// snap back to default thumbstick position
		private void snap3DThumbstickBack(){
			
			for (int i = 0; i <= 3; i++) {
				if (this.meshThumbstick != null) {
					
					float value = this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);
					
					if(value > this.inputThreshold){
						this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, Mathf.Lerp(value, 0f, Time.deltaTime * this.resetDamping));
						/*
						if(debugging){
							if(this.meshThumbstick.thumbstickType.Equals(ThumbstickType.Left)){
								Debug.Log ("ThumbstickLeft BlendShapeWeight "+i+": " + this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i));
							}
							if(this.meshThumbstick.thumbstickType.Equals(ThumbstickType.Right)){
								Debug.Log ("ThumbstickRight BlendShapeWeight "+i+": " + this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i));
							}
						}*/
					}
				}
			}
		}
		
		//-----------------------------------------------------------------------------

        private void SetIconsActive(bool value) {

            this.indicator.enabled = value;

            if (this.iconLeft) {
                this.iconLeft.enabled = value;
            }
            if (this.iconRight) {
                this.iconRight.enabled = value;
            }
            if (this.iconTop) {
                this.iconTop.enabled = value;
            }
            if (this.iconBottom) {
                this.iconBottom.enabled = value;
            }
        }

        //-----------------------------------------------------------------------------

        private void SimulateHover(Vector2 delta) {

            if (this.iconLeft) {
                this.iconLeft.color = delta.x < -this.iconHoverValue ? this.iconHoverColor : this.iconStartColor;
            }
            if (this.iconRight) {
                this.iconRight.color = delta.x > this.iconHoverValue ? this.iconHoverColor : this.iconStartColor;
            }
            if (this.iconTop) {
                this.iconTop.color = delta.y > this.iconHoverValue ? this.iconHoverColor : this.iconStartColor;
            }
            if (this.iconBottom) {
                this.iconBottom.color = delta.y < -this.iconHoverValue ? this.iconHoverColor : this.iconStartColor;
            }
        }

		// ----------------------------------------------------------------------------

		private void updateThumbstickDebugUI(ThumbstickAxis stickaxis, float value){
		
			foreach(DebugUI element in this.debugUIElements){

				if(this.thumbstickType.Equals(element.thumbstickType) && stickaxis.Equals(ThumbstickAxis.UP)){
					this.DebugLabelThumbstickUp.text = value.ToString();
				}

				if(this.thumbstickType.Equals(element.thumbstickType) && stickaxis.Equals(ThumbstickAxis.DOWN)){
					this.DebugLabelThumbstickDown.text = value.ToString();
				}

				if(this.thumbstickType.Equals(element.thumbstickType) && stickaxis.Equals(ThumbstickAxis.LEFT)){
					this.DebugLabelThumbstickLeft.text = value.ToString();
				}

				if(this.thumbstickType.Equals(element.thumbstickType) && stickaxis.Equals(ThumbstickAxis.RIGHT)){
					this.DebugLabelThumbstickRight.text = value.ToString();
				}
			}
		}

    }



}
