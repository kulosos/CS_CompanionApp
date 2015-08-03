/*
 * brief	Input Management class
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Wb.Companion.Core;
using Wb.Companion.Core.WbNetwork;

namespace Wb.Companion.Core.Inputs {

    //-------------------------------------------------------------------------

    public class InputManager : MonoBehaviour {

        public enum TiltAxis {
            forward = 0,
            sideways = 1
        }

		public static InputManager instance;

        public TouchScript.Gestures.PanGesture panGesture;
        public TouchScript.Gestures.ScaleGesture scaleGesture;
        public TouchScript.Gestures.TapGesture tapGesture;
		public TouchScript.Gestures.RotateGesture rotateGesture;
		public TouchScript.Gestures.ReleaseGesture releaseGesture;

		public System.Action<Vector2> OnTouchSinglePan;
		public System.Action<Vector2> OnTouchDoublePan;

        // TiltInput
        public float forwardCenterAngleOffset = 0f;
        public float sidewaysCenterAngleOffset = 0f;
        public float forwardFullTiltAngle = 42f;
        public float sidewaysFullTiltAngle = 42f;

		public bool activeConnection = false;
		public bool activeTiltInput = false;

		//---------------------------------------------------------------------

		public void Awake() {
			InputManager.instance = this;
		}

		public static InputManager getInstance(){
			return InputManager.instance;
		}

        public void Start() {

            this.tapGesture.Tapped += CameraManager.getInstance().tapHandler;
            this.scaleGesture.Scaled += CameraManager.getInstance().scaleHandler;
			this.panGesture.Panned += CameraManager.getInstance().panStartedHandler; //this.wbTransformer2d.panStarted; //touchGesturePanEvent;
			//this.panGesture.PanStarted += touchGesturePanStartet;
			this.panGesture.PanCompleted += CameraManager.getInstance().panCompletedHandler;
			this.rotateGesture.Rotated += CameraManager.getInstance().rotateHandler;
			this.releaseGesture.Released += CameraManager.getInstance().releaseHandler;

        }

        void Update() {

            // Tilt Input
            if (InputManager.getInstance().activeConnection && InputManager.getInstance().activeTiltInput) {
                float tiltValue = InputManager.getInstance().CalcAxisValue(InputManager.TiltAxis.sideways);
                WbCompRPCWrapper.getInstance().setTiltInput(tiltValue);
            }

        }

        //---------------------------------------------------------------------

        private void touchGestureScaleEvent(object sender, System.EventArgs e) {
			//Debug.Log ("touch gesture SCALE recognized");
        }

        //---------------------------------------------------------------------

        private void touchGestureTapEvent(object sender, System.EventArgs e) {
			//Debug.Log ("touch gesture TAP recognized");

        }	

		//---------------------------------------------------------------------
		
		private void touchGesturePanEvent(object sender, System.EventArgs e) {
			//Debug.Log ("touch gesture PAN recognized");

			TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
			Vector2 delta = (gesture.ScreenPosition - gesture.PreviousScreenPosition);

			//Debug.Log ("Delta: " + delta.ToString());


			//GameObject go = GameObject.FindGameObjectsWithTag("Cube");


			//Camera.main.transform.Translate(new Vector3(delta.x, 0.0f, delta.y));

			if(gesture.ActiveTouches.Count == 1){
				if(this.OnTouchSinglePan != null){
					this.OnTouchSinglePan(delta);

				}
			}
			else if (gesture.ActiveTouches.Count > 0){
				if(this.OnTouchDoublePan != null){
					this.OnTouchDoublePan(delta);

				}
			}
			
		}

		//---------------------------------------------------------------------
		
		private void touchGesturePanStartet(object sender, System.EventArgs e) {
			//Debug.Log ("touch gesture PAN STARTED recognized");
			
		}

		//---------------------------------------------------------------------
		
		private void touchGesturePanCompleted(object sender, System.EventArgs e) {
			//Debug.Log ("touch gesture PAN COMPLETED recognized");
			
		}

        // --------------------------------------------------------------------
        // Tilt Input
        // --------------------------------------------------------------------

        private float ForwardTiltAngle() {
            return Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * Mathf.Rad2Deg + this.forwardCenterAngleOffset;
        }

        //-----------------------------------------------------------------------------
        /// <summary>Tilt in angle around y-Axis</summary>
        private float SideWaysTiltAngle() {
            return Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg + this.sidewaysCenterAngleOffset;
        }

        //-----------------------------------------------------------------------------

        public float CalcAxisValue(TiltAxis axis) {

        #if UNITY_EDITOR
            // Axes return strange default values in Editor
            if (Input.acceleration == Vector3.zero) { return 0f; }
        #endif

            float angle = axis == TiltAxis.forward ? this.ForwardTiltAngle() : this.SideWaysTiltAngle();
            float fullTiltAngle = axis == TiltAxis.forward ? this.forwardFullTiltAngle : this.sidewaysFullTiltAngle;
            return Mathf.InverseLerp(-fullTiltAngle, fullTiltAngle, angle) * 2 - 1;
        }

		// ----------------------------------------------------------------------------

		public void toggleTiltInput(){
			if(InputManager.getInstance().activeTiltInput){
				InputManager.getInstance().activeTiltInput = false;
			}else{
				InputManager.getInstance().activeTiltInput = true;
			}
			 
		}

	}
}
