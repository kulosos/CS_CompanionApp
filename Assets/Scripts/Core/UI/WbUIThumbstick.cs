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

    public enum thumbstickType {
        Left = 0,
        Middle = 1,
        Right = 2
    };

    //-------------------------------------------------------------------------

    public class WbUIThumbstick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

        //---------------------------------------------------------------------
        // Member
        //---------------------------------------------------------------------

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

        //---------------------------------------------------------------------
        // Properties
        //---------------------------------------------------------------------

        private Camera uiCamera;
        public thumbstickType thumbstickType;
        //private List<WbUIThumbstick> thumbsticks = new List<WbUIThumbstick>();

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
