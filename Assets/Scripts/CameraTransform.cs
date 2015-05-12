using UnityEngine;
using System.Collections;
using System;
using Wb.Companion.Core;

namespace Wb.Companion.Core.Inputs {

//-----------------------------------------------------------------------------

    public class CameraTransform : MonoBehaviour {

		private static CameraTransform instance;

        public bool isActive = true;

        public Transform rotateTarget;
        public Transform translateTarget;

        public Plane plane = new Plane();
        private Vector3 oldPos;
        private Vector3 pos;

		public float dampSpeed = 50.0f;
		public float minBounds = -1500f;
		public float maxBounds = 1500f;
		public float zoomMin = 0.0f;
		public float zoomMax = 1000f;

		private Vector3 targetPosition = Vector3.zero;

      	// ----------------------------------------------------------------------------

		public static CameraTransform getInstance(){
			return CameraTransform.instance;
		}

        //-----------------------------------------------------------------------------
        // MonoBehaviour
        //-----------------------------------------------------------------------------

		private void Awake() {
			CameraTransform.instance = this;
			this.targetPosition = this.translateTarget.position;
		}

		//-----------------------------------------------------------------------------

        private void Start() {

            if (this.rotateTarget == null) {
                this.rotateTarget = this.transform;
            }

            if (this.translateTarget == null) {
                this.translateTarget = this.transform;
            }
		
            this.pos = this.translateTarget.position;
        }

		private void LateUpdate() {

			this.translateTarget.position = Vector3.Lerp(this.translateTarget.position, this.targetPosition, Time.deltaTime*10f);
		}

        //-----------------------------------------------------------------------------

        private void OnEnable() {
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned += panHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted += panStartedHandler;
            if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated += rotateHandler;
			if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped += tapHandler;
			if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled += scaleHandler;
			if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released += releaseHandler;
		}

        //-----------------------------------------------------------------------------

        private void OnDisable() {
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned -= panHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted -= panStartedHandler;
            if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated -= rotateHandler;
			if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped -= tapHandler;
			if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled -= scaleHandler;
			if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released -= releaseHandler;
		}

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

		public void tapHandler(object sender, EventArgs e) {
			//Debug.Log ("TIPPIDITAPPTAPP");
			TouchScript.Gestures.TapGesture gesture = sender as TouchScript.Gestures.TapGesture;
	
			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}
			//this.setTappedPosition(gesture.ScreenPosition);
		}

		//-----------------------------------------------------------------------------
		
		public void releaseHandler(object sender, EventArgs e) {
			//Debug.Log ("RELEASED");
		}

		//-----------------------------------------------------------------------------
		
		public void scaleHandler(object sender, EventArgs e) {
			
			TouchScript.Gestures.ScaleGesture gesture = sender as TouchScript.Gestures.ScaleGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}

			if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {
				this.setZoom(gesture.LocalDeltaScale);
			}
		}
		
		//-----------------------------------------------------------------------------

		public void panStartedHandler(object sender, EventArgs e) {

		    TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}

		    if (gesture.ActiveTouches.Count < 2) {
                this.setPosition(gesture.ScreenPosition, false, false);
            }
        }

        //-----------------------------------------------------------------------------

		public void panCompletedHandler(object sender, EventArgs e) {

			TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}
			this.setPosition(gesture.ScreenPosition, true, true);
		}
		
		//-----------------------------------------------------------------------------


        public void panHandler(object sender, EventArgs e) {

	        TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}

	        if (gesture.ActiveTouches.Count < 2) {
                this.setPosition(gesture.ScreenPosition, true, false);
            }
        }

        //-----------------------------------------------------------------------------

        public void rotateHandler(object sender, EventArgs e) {
		
	        TouchScript.Gestures.RotateGesture gesture = sender as TouchScript.Gestures.RotateGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}

			if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {
				// FIXME Fix moving lag on rotation start with Gesture Rotation Threshold
				float rotationAngle = /*gesture.RotationThreshold - */gesture.DeltaRotation;
				this.rotateTarget.rotation = Quaternion.AngleAxis(rotationAngle, this.rotateTarget.up) * this.rotateTarget.rotation;
			}
        }

        //-----------------------------------------------------------------------------

        private void setPosition(Vector3 screenPos, bool changePos, bool isCompleted) {

	        this.plane.SetNormalAndPosition(this.translateTarget.up, this.targetPosition);

			Ray ray = Camera.main.ScreenPointToRay(screenPos);

			float hitDistance;
			this.plane.Raycast(ray, out hitDistance);
	
	        if (this.plane.Raycast(ray, out hitDistance)) {

	            if (changePos) {
					this.changePosition(this.oldPos - ray.GetPoint(hitDistance), isCompleted);
	            }
	            this.oldPos = ray.GetPoint(hitDistance);
	        }
        }

        //-----------------------------------------------------------------------------

        private void changePosition(Vector3 delta, bool isCompleted) {

			delta.y = 0; // fix value

			Vector3 pos = this.targetPosition;
			Vector3 oPos = this.targetPosition;
			pos -= delta;
			Vector3 toSet = pos;

			Debug.Log("delta Magnitude: " + delta.magnitude.ToString());
			Debug.Log ("delta: " + delta.ToString());

			Vector3 speedMultiplier = -delta * Time.deltaTime * this.dampSpeed;
			speedMultiplier.y = 0f;
			//toSet = pos - delta;

			toSet.x = Mathf.Clamp(toSet.x, this.minBounds, this.maxBounds);
			toSet.z = Mathf.Clamp(toSet.z, this.minBounds, this.maxBounds);

			this.targetPosition = toSet + speedMultiplier;
		}

		//-----------------------------------------------------------------------------
		
		private void setZoom(float localDeltaScale) {

			Vector3 pos = this.translateTarget.position;
			float zoomY = Mathf.Clamp(pos.y * localDeltaScale, this.zoomMin, this.zoomMax);
			//Debug.Log ("RESULT " + resultY.ToString());
			this.translateTarget.position = new Vector3(pos.x, zoomY, pos.z);
		}

    }
}

