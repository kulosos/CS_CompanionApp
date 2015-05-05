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

		public float dampSpeed = 1.0f;
		public float zoomMin = 1.0f;
		public float zoomMax = 10.0f;

      	// ----------------------------------------------------------------------------

		public static CameraTransform getInstance(){
			return CameraTransform.instance;
		}

        //-----------------------------------------------------------------------------
        // MonoBehaviour
        //-----------------------------------------------------------------------------

		private void Awake() {
			CameraTransform.instance = this;
		}

        private void Start() {

            if (this.rotateTarget == null) {
                this.rotateTarget = this.transform;
            }

            if (this.translateTarget == null) {
                this.translateTarget = this.transform;
            }
		
            this.pos = this.translateTarget.position;
        }

        //-----------------------------------------------------------------------------

        private void OnEnable() {
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned += panHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted += panned;
            if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated += rotateHandler;
			if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped += tapHandler;
			if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled += scaled;
			if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released += releasedHandler;
		}

        //-----------------------------------------------------------------------------

        private void OnDisable() {
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned -= panHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted -= panned;
            if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated -= rotateHandler;
			if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped -= tapHandler;
			if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled -= scaled;
			if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released -= releasedHandler;
		}

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

		public void tapHandler(object sender, EventArgs e) {

		;
			Debug.Log ("TIPPIDITAPPTAPP");
			TouchScript.Gestures.TapGesture gesture = sender as TouchScript.Gestures.TapGesture;
			//this.setTappedPosition(gesture.ScreenPosition);
		}

		//-----------------------------------------------------------------------------
		
		public void releasedHandler(object sender, EventArgs e) {
		
			//Debug.Log ("RELEASED");
		}

		//-----------------------------------------------------------------------------
		
		private void setTappedPosition(Vector3 screenPos) {
			
			this.plane.SetNormalAndPosition(this.translateTarget.up, this.pos);
			
			Ray ray = Camera.main.ScreenPointToRay(screenPos);
			
			float hitDistance;
			this.plane.Raycast(ray, out hitDistance);
			
			if (this.plane.Raycast(ray, out hitDistance)) {
				

				Vector3 delta = this.oldPos - ray.GetPoint(hitDistance);
				delta.y = 0; // fix y value
				
				Vector3 pos = this.translateTarget.position;
				Vector3 oPos = this.translateTarget.position;
				pos -= delta;
				
				this.translateTarget.position = pos; //Vector3.Lerp (oPos, pos - oPos, 0.5f);

			}
		}
		
		//-----------------------------------------------------------------------------
		
		public void scaled(object sender, EventArgs e) {
			
			TouchScript.Gestures.ScaleGesture gesture = sender as TouchScript.Gestures.ScaleGesture;

			if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {
				this.setZoom(gesture.LocalDeltaScale);
			}
		}
		
		//-----------------------------------------------------------------------------

		public void panned(object sender, EventArgs e) {

		    TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
		    if (gesture.ActiveTouches.Count < 2) {
                this.setPosition(gesture.ScreenPosition, false, false);
            }
        }

        //-----------------------------------------------------------------------------

		public void panCompleted(object sender, EventArgs e) {

			TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
			this.setPosition(gesture.ScreenPosition, true, true);
		}
		
		//-----------------------------------------------------------------------------


        public void panHandler(object sender, EventArgs e) {

	        TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
	        if (gesture.ActiveTouches.Count < 2) {
                this.setPosition(gesture.ScreenPosition, true, false);
            }
        }

        //-----------------------------------------------------------------------------

        public void rotateHandler(object sender, EventArgs e) {
		
	        TouchScript.Gestures.RotateGesture gesture = sender as TouchScript.Gestures.RotateGesture;
			if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {
				this.rotateTarget.rotation = Quaternion.AngleAxis(gesture.DeltaRotation, this.rotateTarget.up) * this.rotateTarget.rotation;
			}

        }

        //-----------------------------------------------------------------------------

        private void setPosition(Vector3 screenPos, bool changePos, bool isCompleted) {

	        this.plane.SetNormalAndPosition(this.translateTarget.up, this.pos);

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

			delta.y = 0; // fix y value
			//delta.x = 0;

			Vector3 pos = this.translateTarget.position;
			Vector3 oPos = this.translateTarget.position;
			pos -= delta;
			Vector3 toSet;
			if(isCompleted){
				Debug.Log ("PAN COMPLETE");
				toSet = Vector3.Lerp(oPos, pos, Time.deltaTime * this.dampSpeed);
			} else {
				toSet = pos - delta;
			}

			this.translateTarget.position = toSet;
			//Debug.Log ("POSITION: " + p.ToString());
		
		}

		//-----------------------------------------------------------------------------
		
		private void setZoom(float localDeltaScale) {


			Vector3 pos = this.translateTarget.position;

			float resultY = Mathf.Clamp(pos.y * localDeltaScale, this.zoomMin, this.zoomMax);
			//Debug.Log ("RESULT " + resultY.ToString());

			this.translateTarget.position = new Vector3(pos.x, resultY, pos.z);
		}

    }
}

