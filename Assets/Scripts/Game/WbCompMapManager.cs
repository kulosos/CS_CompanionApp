/**
* @brief		MapMarker Manager for controlling all map marker behaviours
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			Sep 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.WbCamera;
using Wb.Companion.Core.WbNetwork;
using Wb.Companion.Core.UI;

namespace Wb.Companion.Core.Game {

    public class WbCompMapManager : MonoBehaviour {

        private WbCompMapMarker[] mapMarkers;
		public bool offlineBypass = false;
		public float dampingFactor = 1.0f;
        public List<WbCompMapVehicle> vehicleMapMarkers = new List<WbCompMapVehicle>();
		private UIElement[] uiElements;

        //-------------------------------------------------------------------------
        // MonoBehaviour
        //-------------------------------------------------------------------------

        void Start() {
            this.mapMarkers = WbCompMapMarker.FindObjectsOfType(typeof(WbCompMapMarker)) as WbCompMapMarker[];

            // register WbCompMapManager at SceneManager
            SceneManager.getInstance().setMapManager(this);

			// get all UIElements from UIManager
			this.uiElements = SceneManager.getInstance().uiManager.getUIElements();
        }
   
        //-------------------------------------------------------------------------

        void Update() {

			if((NetworkManager.getInstance().isActiveConnection && SceneManager.getInstance().currentScene.Equals(SceneList.Map)) || offlineBypass){
				setVehiclePositions();
			}

		}

        //-------------------------------------------------------------------------

        public void OnTouchMapMarker(Vector2 screenPos){

            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3000f)) {

				//Debug.Log("HIT: " + hit.transform.gameObject.name);

				if(hit.transform.GetComponents<WbCompMapMarker>().Length > 0){

					foreach(UIElement uie in this.uiElements){

						if(hit.transform.gameObject.GetComponent<WbCompMapMarker>().uiLocationName.Equals(uie.location)){

							uie.transform.gameObject.SetActive(true);

							WbCompRetailManager.getInstance().currentRetailLocationName = uie.location;
						}		
					}
				}   
            }
        }

		//-------------------------------------------------------------------------

		// get current received map position of vehicle and set the vehiclemarkes accordingly
		public void setVehiclePositions() {

			foreach(WbCompMapVehicle vehicle in vehicleMapMarkers){

				// Positions
				foreach(KeyValuePair<VehicleID, Vector3> vehiclePositions in WbCompStateSyncReceiving.getInstance().getVehicleMapPositionList()){
					
					if(vehiclePositions.Key.Equals(vehicle.vehicleId)){

						Vector3 pos = vehicle.transform.position;
						vehicle.transform.position = Vector3.Lerp (pos, vehiclePositions.Value, Time.deltaTime * dampingFactor);
					}
				}

				// Rotations
				foreach(KeyValuePair<VehicleID, Quaternion> vehicleRotations in WbCompStateSyncReceiving.getInstance().getVehicleMapRotationList()){

					if(vehicleRotations.Key.Equals(vehicle.vehicleId)){
					
						Quaternion rot = vehicle.transform.rotation;
						vehicle.transform.rotation = Quaternion.Lerp (rot, vehicleRotations.Value, Time.deltaTime * dampingFactor);
					}
				}
			}
		}
        
        //-------------------------------------------------------------------------
        // SETTER / GETTER
        //-------------------------------------------------------------------------

        public WbCompMapMarker[] getMapMarkers() {
            return this.mapMarkers;
        }
    }
}
