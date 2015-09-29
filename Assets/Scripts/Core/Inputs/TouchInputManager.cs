/**
* @brief		TouchInputManager (TouchInput behaviour in Companion App)
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			September 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System;
using Wb.Companion.Core;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbCamera {

    //-------------------------------------------------------------------------

    public class TouchInputManager : MonoBehaviour {

        private static TouchInputManager instance;

        public TouchScript.Gestures.TapGesture tapGesture;
        public TouchScript.Gestures.PanGesture panGesture;
        public TouchScript.Gestures.ScaleGesture scaleGesture;
        public TouchScript.Gestures.RotateGesture rotateGesture;
        public TouchScript.Gestures.ReleaseGesture releaseGesture;

        public System.Action<Vector2> OnTouchSinglePan;
        public System.Action<Vector2> OnTouchDoublePan;

        //---------------------------------------------------------------------

        public static TouchInputManager getInstance() {
            return TouchInputManager.instance;
        }

        //---------------------------------------------------------------------
        // MonoBehaviour
        //---------------------------------------------------------------------

        private void Awake() {
            TouchInputManager.instance = this;
        }

        //---------------------------------------------------------------------

        private void OnEnable() {
			if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped += tapHandler;
			if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned += panHandler;
			if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted += panStartedHandler;
			if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled += scaleHandler;
			if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated += rotateHandler;
			if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released += releaseHandler;
        }

        //---------------------------------------------------------------------

        private void Start() {
            this.tapGesture.Tapped += TouchInputManager.getInstance().tapHandler;
            this.panGesture.Panned += TouchInputManager.getInstance().panStartedHandler;
            this.panGesture.PanCompleted += TouchInputManager.getInstance().panCompletedHandler;
            this.scaleGesture.Scaled += TouchInputManager.getInstance().scaleHandler;
            this.rotateGesture.Rotated += TouchInputManager.getInstance().rotateHandler;
            this.releaseGesture.Released += TouchInputManager.getInstance().releaseHandler;
        }

        //---------------------------------------------------------------------

        private void OnDisable() {
            if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped -= tapHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned -= panHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted -= panStartedHandler;
            if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled -= scaleHandler;
            if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated -= rotateHandler;
            if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released -= releaseHandler;
        }

        //---------------------------------------------------------------------
        // EVENT HANDLER
        //---------------------------------------------------------------------

        public void tapHandler(object sender, EventArgs e) {
            //Debug.Log ("TIPPIDITAPPTAPP");
            TouchScript.Gestures.TapGesture gesture = sender as TouchScript.Gestures.TapGesture;

            if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
                return;
            }
            if (gesture.ActiveTouches.Count < 2) {
                //this.setTappedPosition(gesture.ScreenPosition);
                if (SceneManager.getInstance().currentScene.Equals(SceneList.Map)) {
                    SceneManager.getInstance().getMapManager().OnTouchMapMarker(gesture.ScreenPosition);
                }
                
            }
        }

        //---------------------------------------------------------------------

        public void panStartedHandler(object sender, EventArgs e) {
            TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

            if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
                return;
            }

            if (gesture.ActiveTouches.Count < 2) {
                //this.setPosition(gesture.ScreenPosition, false, false);
            }
        }

        //---------------------------------------------------------------------

        public void releaseHandler(object sender, EventArgs e) {
            //Debug.Log ("RELEASED");
        }

        //---------------------------------------------------------------------

        public void scaleHandler(object sender, EventArgs e) {

            TouchScript.Gestures.ScaleGesture gesture = sender as TouchScript.Gestures.ScaleGesture;

            if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
                return;
            }

            if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {

				float scaleValue = gesture.LocalDeltaScale * CameraManager.getInstance().zoomFactor;

				// localDeltaScale > 1 means zoomIn | localDetlaScale < 1 means zommOut
				if(gesture.LocalDeltaScale > 1.0f){
					scaleValue = -scaleValue;
				}

				//Debug.Log("----- Scale Gesture: " + scaleValue);
             
				Vector3 oldPos = Camera.main.transform.localPosition;
				Vector3 newPos = new Vector3(oldPos.x, oldPos.y + scaleValue, oldPos.z);

				newPos.y = Mathf.Clamp(newPos.y, CameraManager.getInstance().zoomMin, CameraManager.getInstance().zoomMax);

				CameraManager.getInstance().targetPosition = newPos;

            }
        }

        //---------------------------------------------------------------------

        public void panCompletedHandler(object sender, EventArgs e) {

            TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

            if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
                return;
            }
            //this.setPosition(gesture.ScreenPosition, true, true);
        }

        //---------------------------------------------------------------------


        public void panHandler(object sender, EventArgs e) {
			//Debug.Log ("PANNER");
            TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

            if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
                return;
            }


            if (gesture.ActiveTouches.Count < 2) {

                Vector3 oldPos = Camera.main.transform.localPosition;
                Vector3 camMovement = gesture.WorldDeltaPosition;

                camMovement.x *= 1.5f * CameraManager.getInstance().movementMultiplier;
                camMovement.y = 0;
                camMovement.z *= CameraManager.getInstance().movementMultiplier;

                oldPos -= camMovement;

                Vector3 newPos = oldPos;

                CameraManager.getInstance().panMagnitude = gesture.WorldDeltaPosition.magnitude;

                newPos.x = Mathf.Clamp(newPos.x, CameraManager.getInstance().minBounds, CameraManager.getInstance().maxBounds);
                //newPos.y = CameraManager.getInstance().cameraHeight;
                newPos.z = Mathf.Clamp(newPos.z, CameraManager.getInstance().minBounds, CameraManager.getInstance().maxBounds);

                CameraManager.getInstance().targetPosition = newPos;

                // only moving without fancy stuff
                //this.camMovement = gesture.WorldDeltaPosition;
                //this.camMovement.y = 1000;
                //this.camMovement.x *= 1.5f;
                //Camera.main.transform.localPosition += camMovement;
            }


        }

        //---------------------------------------------------------------------

        public void rotateHandler(object sender, EventArgs e) {

            TouchScript.Gestures.RotateGesture gesture = sender as TouchScript.Gestures.RotateGesture;

            if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
                return;
            }

            if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {
                Debug.Log("----- Rotation Gesture");

                //Debug.Log ("LOCAL SCALE: "  + this.rotateTarget);// this.rotateTarget.transform.rotation);//Vector3 rotationOld = this.getWorldScale(this.rotateTarget.transform);

                // FIXME Fix moving lag on rotation start with Gesture Rotation Threshold
                //float rotationAngle = /*gesture.RotationThreshold - */gesture.DeltaRotation;

                //this.rotateTarget.rotation = Quaternion.AngleAxis(rotationAngle, this.rotateTarget.up ) * this.rotateTarget.rotation;

            }
        }

    }
}

