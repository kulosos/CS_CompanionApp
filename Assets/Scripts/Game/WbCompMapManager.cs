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

namespace Wb.Companion.Core.Game {

    public class WbCompMapManager : MonoBehaviour {

        private WbCompMapMarker[] mapMarkers;

        public List<WbCompMapVehicle> vehicleMapMarkers = new List<WbCompMapVehicle>();

        public bool debugging = false;
        public bool isActive = false;

        //-------------------------------------------------------------------------
        // MonoBehaviour
        //-------------------------------------------------------------------------

        void Start() {
            this.mapMarkers = WbCompMapMarker.FindObjectsOfType(typeof(WbCompMapMarker)) as WbCompMapMarker[];

            // register WbCompMapManager at SceneManager
            SceneManager.getInstance().setMapManager(this);
        }
   
        //-------------------------------------------------------------------------

        void Update() {
        }

        //-------------------------------------------------------------------------

        public void OnTouchMapMarker(Vector2 screenPos){

            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3000f)) {
                Debug.Log("HIT: " + hit.transform.gameObject.name);
            }
        }

        //-------------------------------------------------------------------------

        public void setVehiclePositions(string vehicleId, Vector3 position) {

            if (isActive) {
                Debug.Log("O - vID: " + vehicleId);

                foreach (WbCompMapVehicle vehicleMarker in this.vehicleMapMarkers) {

                    Debug.Log("I - vID: " + vehicleId + " - vmarkerID: " + vehicleMarker.vehicleName);

                    if (vehicleId.Equals(vehicleMarker.vehicleName)) {
                        vehicleMarker.transform.position = position;
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

    // ENUM TYPES -----------------------------------------------------------------

    public enum WbCompMarkerLocation {
        HomeBase_Small,
        HomeBase_Big,
        BuildingMerchant_Small,
        BuildingMerchant_Big,
        VehicleDealer,
        SteelFactory,
        WallFactory,
        Sandbox,
        Sawmill_Small,
        Sawmill_Big,
        NurseryGarden,
        Harbour,
        ConstructionSite
    }
}
