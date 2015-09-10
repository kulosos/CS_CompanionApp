/**
* @brief		MapMarker Manager for controlling all map marker behaviours
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			Sep 2015
*/

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace Wb.Companion.Core.Game {

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

    //-----------------------------------------------------------------------------

    public class WbCompMapManager : MonoBehaviour {

        public Camera currentCamera;
        private WbCompMapMarker[] mapMarkers;

        public bool debugging = false;

        //-------------------------------------------------------------------------
        // MonoBehaviour
        //-------------------------------------------------------------------------

        void Start() {
            this.mapMarkers = WbCompMapMarker.FindObjectsOfType(typeof(WbCompMapMarker)) as WbCompMapMarker[];
        }

        //-------------------------------------------------------------------------

        void Update() {


            if (debugging) {
                Ray ray = this.currentCamera.ScreenPointToRay(new Vector3(200, 200, 0));
                Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            }


            //Ray ray = Camera.main.ScreenPointToRay(screenPos);
            //RaycastHit hit;
            //float distanceToGround = 0;

            //if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
            //    distanceToGround = hit.distance;


            //}


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

        //-------------------------------------------------------------------------


        //-------------------------------------------------------------------------
        // SETTER / GETTER
        //-------------------------------------------------------------------------

        public WbCompMapMarker[] getMapMarkers() {
            return this.mapMarkers;
        }

    }

}
