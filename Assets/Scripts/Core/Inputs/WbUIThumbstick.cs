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

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.Inputs {

    public enum thumbstickType {
        Left = 0,
        Middle = 1,
        Right = 2
    };

    public class WbUIThumbstick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

        //-----------------------------------------------------------------------------
        // Member
        //-----------------------------------------------------------------------------

        private Camera uiCamera;
        public thumbstickType thumbstickType;

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

        //-----------------------------------------------------------------------------
        // Properties
        //-----------------------------------------------------------------------------

        //-----------------------------------------------------------------------------
        // MonoBehaviour
        //-----------------------------------------------------------------------------

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

        public void Init() {

            if (this.indicator == null) {

                Debug.LogError("No Indicator set for Thumbstick :(  Why are you doing this?");
                Destroy(this.gameObject);
                return;
            }

            this.startPos = this.indicator.rectTransform.anchoredPosition;

            this.useX = (this.axisToUse == WbUIAxisOption.Both || this.axisToUse == WbUIAxisOption.OnlyHorizontal);
            this.useY = (this.axisToUse == WbUIAxisOption.Both || this.axisToUse == WbUIAxisOption.OnlyVertical);

            this.SetIconsActive(false);

            // Create Virtual Axis
            if (this.useX) {
                // TODO
                //WbInputManager.VirtualInput.RegisterAxis(new WbVirtualAxis(this.horizontalAxisName));
            }

            if (this.useY) {
                // TODO
                //WbInputManager.VirtualInput.RegisterAxis(new WbVirtualAxis(this.verticalAxisName));
            }
        }

        //-----------------------------------------------------------------------------
        // Interface Implementations
        //-----------------------------------------------------------------------------

        public void OnPointerUp(PointerEventData data) {

            this.indicator.rectTransform.anchoredPosition = new Vector2(0, 0);
            this.SetIconsActive(false);
            this.UpdateAxes(this.indicator.rectTransform.anchoredPosition);
        }

        //-----------------------------------------------------------------------------

        public void OnPointerDown(PointerEventData data) {

            this.SetIconsActive(true);
            this.UpdateIndicator(data);
        }

        //-----------------------------------------------------------------------------

        public void OnDrag(PointerEventData data) {

            this.UpdateIndicator(data);
        }

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

        private void UpdateIndicator(PointerEventData data) {

            Vector2 pos;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(this.wrapperRect, data.position, Wb.Camera.WbCameraManager.UICamera, out pos)
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

            if (this.useX) {
                // TODO
                //WbInputManager.VirtualInput.SetAxisValue(this.horizontalAxisName, delta.x);
            }
            if (this.useY) {
                // TODO
                //WbInputManager.VirtualInput.SetAxisValue(this.verticalAxisName, delta.y);
            }
            this.SimulateHover(delta);
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

    //-----------------------------------------------------------------------------

    public enum WbUIAxisOption {
        Both,
        OnlyHorizontal,
        OnlyVertical
    }
}
