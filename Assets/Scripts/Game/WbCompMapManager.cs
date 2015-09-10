/**
* @brief		MapMarker Manager for controlling all map marker behaviours
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			Sep 2015
*/

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.WbCamera;

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

        private WbCompMapMarker[] mapMarkers;

        public bool debugging = false;

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

        public void OnTouchMapMarker(Vector2 screenPos){

            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3000f)) {
                hit.transform.gameObject.GetComponent<Renderer>().material.SetColor("_MainTex", Color.red);
                Debug.Log("HIT: " + hit.transform.gameObject.name);
            }

            //RaycastHit hit;
            //float hitDistance = 0;
            //float distanceToGround = 0;

            //if (CameraManager.getInstance().plane.Raycast(ray, out hitDistance)) {

            //    //distanceToGround = hit.distance;
            //    Debug.LogWarning("hitDistance: " + hitDistance);
            //}

        }
        

        //-------------------------------------------------------------------------
        // SETTER / GETTER
        //-------------------------------------------------------------------------

        public WbCompMapMarker[] getMapMarkers() {
            return this.mapMarkers;
        }

    }

}
