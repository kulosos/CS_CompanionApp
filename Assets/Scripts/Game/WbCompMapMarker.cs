/**
* @brief		MapMarker Object DataType
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			Sep 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.UI;

namespace Wb.Companion.Core.Game {

    public class WbCompMapMarker : MonoBehaviour {

        public WbCompMapManager mapManager;
        public WbCompMarkerLocation markerLocation;
		public WbLocationName uiLocationName;

        //-------------------------------------------------------------------------
        // MonoBehaviour
        //-------------------------------------------------------------------------

        void Start() {
        }

        void Update() {
        }

        void LateUpdate() {

            // Billboarding of MapMarkers
            if (SceneManager.getInstance().currentScene.Equals(SceneList.Map)) {
                transform.LookAt(Camera.main.transform, Camera.main.transform.rotation * Vector3.up);
            }
            
        }

        //-------------------------------------------------------------------------

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
