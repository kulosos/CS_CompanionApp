/**
* @brief		Basic implementation of WbUIThumbstick
*
* @author		Thomas Mueller (thomas.mueller@weltenbauer-se.com)
*
* @date			MONTH 2015
* 
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

    public enum WbUIAxisOption {
        Both,
        OnlyHorizontal,
        OnlyVertical
    }

    public enum ThumbstickType {
        Left = 0,
        Middle = 1,
        Right = 2
    };

	public enum ThumbstickAxis {
		UP = 0,
		DOWN = 1,
		LEFT = 2,
		RIGHT = 3
	};

    //-------------------------------------------------------------------------

    public class WbUIThumbstick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

        //---------------------------------------------------------------------
        // Member
        //---------------------------------------------------------------------

		public Wb3DThumbstick meshThumbstick;
		private Camera uiCamera;
		public ThumbstickType thumbstickType;
		public float resetDamping = 5f;
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
        private bool useX;
        private bool useY;
		public bool isReleased = false;


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
        }

		void Update() {

			// snap back to default thumbstick position
			if(isReleased){
				for(int i = 0; i <= 3; i++){
					float value = this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(i);
					this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(i, Mathf.Lerp(value, 0f, Time.deltaTime * this.resetDamping));
				}
				//this.isReleased = false;
			}
		}

        //-----------------------------------------------------------------------------
        // Interface Implementations
        //-----------------------------------------------------------------------------

        public void OnPointerUp(PointerEventData data) {

            this.indicator.rectTransform.anchoredPosition = new Vector2(0, 0);
            this.SetIconsActive(false);
            this.UpdateAxes(this.indicator.rectTransform.anchoredPosition);
			this.isReleased = true;
        }

        //-----------------------------------------------------------------------------

        public void OnPointerDown(PointerEventData data) {

            this.SetIconsActive(true);
            this.UpdateIndicator(data);
        }

        //-----------------------------------------------------------------------------

        public void OnDrag(PointerEventData data) {

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
            
            this.SimulateHover(delta);

            //Debug.Log ("Delta: " + delta);

			// Blendshape Order: 0 = UP, 1 = DOWN, 2 = LEFT, 3 = RIGHT

            // Thumbstick UP
			if(delta.y > 0){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.y)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 0);
                
                // send input key for vehicle interaction
                if(this.thumbstickType.Equals(ThumbstickType.Left)){
                    WbCompRPCWrapper.getInstance().setVehicleInput("boom01-up", Mathf.Clamp01(delta.y));
                }
                if(this.thumbstickType.Equals(ThumbstickType.Right)){
                    WbCompRPCWrapper.getInstance().setVehicleInput("wbtruckCrane-rope-up", Mathf.Clamp01(delta.y));
                }
			}
            // Thumbstick DOWN
			if(delta.y < 0){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.y)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, blendshapeValue);
                this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
                
                // send input key for vehicle interaction
                if (this.thumbstickType.Equals(ThumbstickType.Left)) {
                    WbCompRPCWrapper.getInstance().setVehicleInput("boom01-down", Mathf.Clamp01(Mathf.Abs(delta.y)));
                }
                if (this.thumbstickType.Equals(ThumbstickType.Right)) {
                    WbCompRPCWrapper.getInstance().setVehicleInput("wbtruckCrane-rope-down", Mathf.Clamp01(Mathf.Abs(delta.y)));
                }
			}

            // Thumbstick LEFT
			if(delta.x < 0){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.x)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(3, 0);

                // send input key for vehicle interaction
                if (this.thumbstickType.Equals(ThumbstickType.Left)) {
                    WbCompRPCWrapper.getInstance().setVehicleInput("boomMainLeft", Mathf.Clamp01(delta.y));
                }
                if (this.thumbstickType.Equals(ThumbstickType.Right)) {
                    WbCompRPCWrapper.getInstance().setVehicleInput("boom02-03-04-05-06-forward", Mathf.Clamp01(delta.y));
                }
			}

            // Thumbstick RIGHT
			if(delta.x > 0){
				float blendshapeValue = Mathf.Lerp(0, 100f, Mathf.InverseLerp(0f, 1f, Mathf.Abs(delta.x)));
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(3, blendshapeValue);
				this.meshThumbstick.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, 0);

                // send input key for vehicle interaction
                if (this.thumbstickType.Equals(ThumbstickType.Left)) {
                    WbCompRPCWrapper.getInstance().setVehicleInput("boomMainRight", Mathf.Clamp01(delta.y));
                }
                if (this.thumbstickType.Equals(ThumbstickType.Right)) {
                    WbCompRPCWrapper.getInstance().setVehicleInput("boom02-03-04-05-06-backward", Mathf.Clamp01(delta.y));
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

    }



}
