/**
* @brief		MapMarker Object DataType
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			Sep 2015
*/

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbAdministration;

namespace Wb.Companion.Core.Game {

    public class WbCompMapMarker : MonoBehaviour {

        public WbCompMapManager mapManager;
        public WbCompMarkerLocation markerLocation;

        //-------------------------------------------------------------------------
        // MonoBehaviour
        //-------------------------------------------------------------------------

        void Start() {
        }

        void Update() {
        }

        void LateUpdate() {
            // Billboarding of MapMarkers
            transform.LookAt(Camera.main.transform, Camera.main.transform.rotation * Vector3.up);
        }

        //-------------------------------------------------------------------------

    }

}
