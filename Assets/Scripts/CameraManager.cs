using UnityEngine;
using System.Collections;
using System;
using Wb.Companion.Core;
using Wb.Companion.Core.WbAdministration;

namespace Wb.Companion.Core.Inputs {

//-----------------------------------------------------------------------------

	public class CameraManager : MonoBehaviour {

		private static CameraManager instance;

		public bool isMapCameraActive = true;

		public Plane plane = new Plane();
		public float cameraHeight = 1000f;

		public float dampingFactor = 10.0f;
		public float movementMultiplier = 2f;
		public float minBounds = -1500f;
		public float maxBounds = 1500f;
		public float zoomMin = 0.0f;
		public float zoomMax = 1000f;

		private Vector3 targetPosition = Vector3.zero;
		private float panMagnitude;

		// ----------------------------------------------------------------------------

		public static CameraManager getInstance(){
			return CameraManager.instance;
		}

		//-----------------------------------------------------------------------------
		// MonoBehaviour
		//-----------------------------------------------------------------------------

		private void Awake() {
			CameraManager.instance = this;

		}

		//-----------------------------------------------------------------------------

		private void Start() {
		}

		private void LateUpdate() {

			if(isMapCameraActive){
				float damping = this.dampingFactor * (1 + this.panMagnitude / 100);
				Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, this.targetPosition, Time.deltaTime * damping);
			}
		}

		//-----------------------------------------------------------------------------

		private void OnEnable() {

			if(isMapCameraActive){
				if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned += panHandler;
				if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted += panStartedHandler;
				if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated += rotateHandler;
				if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped += tapHandler;
				if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled += scaleHandler;
				if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released += releaseHandler;
			}
		}

		//-----------------------------------------------------------------------------

		private void OnDisable() {
			if(isMapCameraActive){
				if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().Panned -= panHandler;
				if (GetComponent<TouchScript.Gestures.PanGesture>() != null) GetComponent<TouchScript.Gestures.PanGesture>().PanStarted -= panStartedHandler;
				if (GetComponent<TouchScript.Gestures.RotateGesture>() != null) GetComponent<TouchScript.Gestures.RotateGesture>().Rotated -= rotateHandler;
				if (GetComponent<TouchScript.Gestures.TapGesture>() != null) GetComponent<TouchScript.Gestures.TapGesture>().Tapped -= tapHandler;
				if (GetComponent<TouchScript.Gestures.ScaleGesture>() != null) GetComponent<TouchScript.Gestures.ScaleGesture>().Scaled -= scaleHandler;
				if (GetComponent<TouchScript.Gestures.ReleaseGesture>() != null) GetComponent<TouchScript.Gestures.ReleaseGesture>().Released -= releaseHandler;
			}
		}

		//-----------------------------------------------------------------------------
		// Methods
		//-----------------------------------------------------------------------------

		public void setInitialCameraOnSceneLoading(string scene){

			// MAP SCENE
			if(scene.Equals(SceneList.Map)){
				this.targetPosition = new Vector3(0f, this.cameraHeight, 0f);

				Quaternion rotation = Quaternion.Euler(60f, 90f, 0f);
				Camera.main.transform.rotation = rotation;

				toggleOrthogonalCamera(false);
				this.isMapCameraActive = true;
                this.minBounds = -1500f;
                this.maxBounds = 1500f;
			}

			// REMOTE CONTROL SCENE
			if(scene.Equals(SceneList.RemoteControl)){
				Camera.main.transform.localPosition = new Vector3(0f, 2.6f, 0.15f);

				Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
				Camera.main.transform.rotation = rotation;

				Camera.main.orthographicSize = 2.6f;

				toggleOrthogonalCamera(true);
				this.isMapCameraActive = false;
			}

            // REMOTE CONTROL CRANE SCENE
            if (scene.Equals(SceneList.RemoteControlCrane))
            {
                Camera.main.transform.localPosition = new Vector3(0f, 200f, -0.2f);

                Quaternion rotation = Quaternion.Euler(90f, 180f, 0f);
                Camera.main.transform.rotation = rotation;

                Camera.main.orthographicSize = 1.25f;

                toggleOrthogonalCamera(true);
                this.isMapCameraActive = true;
                this.minBounds = -1f;
                this.maxBounds = 0.5f;
            }
        }


		public void tapHandler(object sender, EventArgs e) {
			//Debug.Log ("TIPPIDITAPPTAPP");
			TouchScript.Gestures.TapGesture gesture = sender as TouchScript.Gestures.TapGesture;
	
			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}
			if (gesture.ActiveTouches.Count < 2) {
				//Debug.Log ("TAP NAME : " + this.rotateTarget.transform.name);
				//Debug.Log ("TAP SCALE: " + this.rotateTarget.transform.rotation);
				//this.setTappedPosition(gesture.ScreenPosition);
			}

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
				Debug.Log ("----- Scale Gesture");
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
				//this.setPosition(gesture.ScreenPosition, false, false);
			}
		}

		//-----------------------------------------------------------------------------

		public void panCompletedHandler(object sender, EventArgs e) {

			TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}
			//this.setPosition(gesture.ScreenPosition, true, true);
		}
		
		//-----------------------------------------------------------------------------


		public void panHandler(object sender, EventArgs e) {

			TouchScript.Gestures.PanGesture gesture = sender as TouchScript.Gestures.PanGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}


			if (gesture.ActiveTouches.Count < 2) {

				Vector3 oldPos = Camera.main.transform.localPosition;
				Vector3 camMovement = gesture.WorldDeltaPosition;
				
				camMovement.x *= 1.5f * this.movementMultiplier;
				camMovement.y = 0;
				camMovement.z *= this.movementMultiplier;

				oldPos -= camMovement;
				
				Vector3 newPos = oldPos;

				this.panMagnitude = gesture.WorldDeltaPosition.magnitude;

				newPos.x = Mathf.Clamp(newPos.x, this.minBounds, this.maxBounds);
				newPos.y = this.cameraHeight;
				newPos.z = Mathf.Clamp(newPos.z, this.minBounds, this.maxBounds);

				this.targetPosition = newPos;

				// only moving without fancy stuff
				//this.camMovement = gesture.WorldDeltaPosition;
				//this.camMovement.y = 1000;
				//this.camMovement.x *= 1.5f;
				//Camera.main.transform.localPosition += camMovement;
			}

		   
		}

		//-----------------------------------------------------------------------------

		public void rotateHandler(object sender, EventArgs e) {
		
			TouchScript.Gestures.RotateGesture gesture = sender as TouchScript.Gestures.RotateGesture;

			if (float.IsNaN(gesture.ScreenPosition.x) || float.IsNaN(gesture.ScreenPosition.y)) {
				return;
			}

			if (gesture.ActiveTouches.Count > 1 && gesture.ActiveTouches.Count < 3) {
				Debug.Log ("----- Rotation Gesture");

				//Debug.Log ("LOCAL SCALE: "  + this.rotateTarget);// this.rotateTarget.transform.rotation);//Vector3 rotationOld = this.getWorldScale(this.rotateTarget.transform);

				// FIXME Fix moving lag on rotation start with Gesture Rotation Threshold
                //float rotationAngle = /*gesture.RotationThreshold - */gesture.DeltaRotation;

				//this.rotateTarget.rotation = Quaternion.AngleAxis(rotationAngle, this.rotateTarget.up ) * this.rotateTarget.rotation;

			}
		}

		//-----------------------------------------------------------------------------

		private void setPosition(Vector3 screenPos, bool changePos, bool isCompleted) {

			//this.plane.SetNormalAndPosition(this.translateTarget.up, this.targetPosition);

			//Ray ray = Camera.main.ScreenPointToRay(screenPos);

			//float hitDistance;
			//this.plane.Raycast(ray, out hitDistance);
	
			//if (this.plane.Raycast(ray, out hitDistance)) {

			//    if (changePos) {
			//        this.changePosition(this.oldPos - ray.GetPoint(hitDistance), isCompleted);
			//    }
			//    this.oldPos = ray.GetPoint(hitDistance);
			//}
		}

		//-----------------------------------------------------------------------------

		private void changePosition(Vector3 delta, bool isCompleted) {

			delta.y = 0; // fix value

			Vector3 pos = this.targetPosition;
			//Vector3 oPos = this.targetPosition;
			pos -= delta;
			Vector3 newPos = pos;
		
			this.panMagnitude = delta.magnitude;
			//toSet = pos - delta;

			newPos.x = Mathf.Clamp(newPos.x, this.minBounds, this.maxBounds);
			newPos.z = Mathf.Clamp(newPos.z, this.minBounds, this.maxBounds);

			this.targetPosition = newPos;
		}

		//-----------------------------------------------------------------------------
		
		private void setZoom(float localDeltaScale) {

			////Vector3 pos = this.translateTarget.position;
			//float zoomY = Mathf.Clamp(newPos.y * localDeltaScale, this.zoomMin, this.zoomMax);
			////Debug.Log ("RESULT " + resultY.ToString());
			//this.targetPosition = new Vector3(newPos.x, zoomY, newPos.z);
		}

		// ----------------------------------------------------------------------------

		public void toggleOrthogonalCamera(bool orthographic){

			if(orthographic){
				Camera.main.orthographic = true;
			}
			else{
				Camera.main.orthographic = false;
			}
		}
		
	}
}

