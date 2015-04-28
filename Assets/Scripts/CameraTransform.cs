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
        }

        //-----------------------------------------------------------------------------

        private void OnDisable() {
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned -= panHandler;
            if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted -= panned;
            if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated -= rotateHandler;
        }

        //-----------------------------------------------------------------------------
        // Methods
        //-----------------------------------------------------------------------------

        public void panned(object sender, EventArgs e) {

		    TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
		    //if (gesture.ActiveTouches.Count > 1) {
                this.setPosition(gesture.ScreenPosition, false);
            //}
        }

        //-----------------------------------------------------------------------------

		public void panCompleted(object sender, EventArgs e) {
			
			TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
			this.setPosition(gesture.ScreenPosition, false);
		}
		
		//-----------------------------------------------------------------------------


        public void panHandler(object sender, EventArgs e) {

	        TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;
	        //if (gesture.ActiveTouches.Count > 1) {
                this.setPosition(gesture.ScreenPosition, true);
            //}
        }

        //-----------------------------------------------------------------------------

        public void rotateHandler(object sender, EventArgs e) {
		
	        TouchScript.Gestures.RotateGesture gesture = sender as TouchScript.Gestures.RotateGesture;
	        this.rotateTarget.rotation = Quaternion.AngleAxis(gesture.DeltaRotation, this.rotateTarget.up) * this.rotateTarget.rotation;
        }

        //-----------------------------------------------------------------------------

        private void setPosition(Vector3 screenPos, bool changePos) {

	        this.plane.SetNormalAndPosition(this.translateTarget.up, this.pos);

			Ray ray = Camera.main.ScreenPointToRay(screenPos);

			float hitDistance;
			this.plane.Raycast(ray, out hitDistance);
	
	        if (this.plane.Raycast(ray, out hitDistance)) {

	            if (changePos) {
					this.changePosition(this.oldPos - ray.GetPoint(hitDistance));
	            }
	            this.oldPos = ray.GetPoint(hitDistance);
	        }
        }

        //-----------------------------------------------------------------------------

        private void changePosition(Vector3 delta) {
		


			delta.y = 0; // fix y value
			//delta.x = 0;
			Vector3 p = this.translateTarget.position;
			p -= delta;
			this.translateTarget.position = p;
			//Debug.Log ("POSITION: " + p.ToString());
		
		}

    }
}

