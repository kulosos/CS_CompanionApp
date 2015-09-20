using UnityEngine;
using System.Collections;
using System;
using Wb.Companion.Core;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbCamera {

//-----------------------------------------------------------------------------

	public class CameraManager : MonoBehaviour {

		private static CameraManager instance;

		public Camera uiCamera;
		public bool isMapCameraActive = true;

		public Plane plane = new Plane();
		public float cameraHeight = 1000f;

		public float dampingFactor = 10.0f;
		public float movementMultiplier = 2f;
		public float minBounds = -1500f;
		public float maxBounds = 1500f;
		public float zoomMin = 0.0f;
		public float zoomMax = 1000f;

		public Vector3 targetPosition = Vector3.zero;
		public float panMagnitude;


		// ----------------------------------------------------------------------------

		public static CameraManager getInstance(){
			return CameraManager.instance;
		}

		//-----------------------------------------------------------------------------
		// MonoBehaviour
		//-----------------------------------------------------------------------------

		private void Awake() {
			CameraManager.instance = this;
			/*
			Camera[] cameras = Camera.FindObjectsOfType(typeof(Camera)) as Camera[];
			foreach (Camera cam in cameras)
			{
				if (cam.gameObject.layer == 5) {  // Layer 5 == UI
					this.uiCamera = cam;
				}
			}*/

		}

		//-----------------------------------------------------------------------------

		private void Start() {
		}

        //-----------------------------------------------------------------------------

		private void LateUpdate() {

			if(isMapCameraActive){
				float damping = this.dampingFactor * (1 + this.panMagnitude / 100);
				Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, this.targetPosition, Time.deltaTime * damping);
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

			// REMOTE CONTROL DRIVING SCENE
			if(scene.Equals(SceneList.RemoteControlDriving)){
				Camera.main.transform.localPosition = new Vector3(0f, 2.6f, 5f);

				Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
				Camera.main.transform.rotation = rotation;

				Camera.main.orthographicSize = 2.6f;

				toggleOrthogonalCamera(true);
				this.isMapCameraActive = false;
			}
        
            // REMOTE CONTROL CRANE SCENE
            if (scene.Equals(SceneList.RemoteControlCrane))
            {
                //Camera.main.transform.localPosition = new Vector3(0f, 0f, 0f);
				uiCamera.transform.localPosition = new Vector3(0f, 0f, 0f);

                Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
                Camera.main.transform.rotation = rotation;

                //uiCamera.orthographicSize = 1.28f;

                toggleOrthogonalCamera(true);
                this.isMapCameraActive = false;
                //this.minBounds = -1f;
                //this.maxBounds = 0.5f;
            }
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

