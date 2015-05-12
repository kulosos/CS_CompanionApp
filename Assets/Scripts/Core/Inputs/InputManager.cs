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

namespace Wb.Companion.Core.Inputs {

    //-------------------------------------------------------------------------

    public class InputManager : MonoBehaviour {

		public static InputManager instance;

        public TouchScript.Gestures.PanGesture panGesture;
        public TouchScript.Gestures.ScaleGesture scaleGesture;
        public TouchScript.Gestures.TapGesture tapGesture;
		public TouchScript.Gestures.RotateGesture rotateGesture;
		public TouchScript.Gestures.ReleaseGesture releaseGesture;

		public System.Action<Vector2> OnTouchSinglePan;
		public System.Action<Vector2> OnTouchDoublePan;

		//---------------------------------------------------------------------

		public void Awake() {
			InputManager.instance = this;
		}

		public static InputManager getInstance(){
			return InputManager.instance;
		}

        //---------------------------------------------------------------------

        public void Start() {

            this.tapGesture.Tapped += CameraTransform.getInstance().tapHandler;
            this.scaleGesture.Scaled += CameraTransform.getInstance().scaleHandler;
			this.panGesture.Panned += CameraTransform.getInstance().panStartedHandler; //this.wbTransformer2d.panStarted; //touchGesturePanEvent;
			//this.panGesture.PanStarted += touchGesturePanStartet;
			this.panGesture.PanCompleted += CameraTransform.getInstance().panCompletedHandler;
			this.rotateGesture.Rotated += CameraTransform.getInstance().rotateHandler;
			this.releaseGesture.Released += CameraTransform.getInstance().releaseHandler;

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

	


	}
}
