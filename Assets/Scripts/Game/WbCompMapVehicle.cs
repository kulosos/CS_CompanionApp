using UnityEngine;
using System.Collections;


namespace Wb.Companion.Core.Game {

    public class WbCompMapVehicle : MonoBehaviour {

        public VehicleID vehicleId;
        public VehicleFunction vehicleFunction;
        public GameObject vehicleMarkerBillboard;

        //-------------------------------------------------------------------------

        void Start() {

        }

        void Update() {

            if (this.vehicleMarkerBillboard != null) {
                this.vehicleMarkerBillboard.transform.LookAt(Camera.main.transform, Camera.main.transform.rotation * Vector3.up);
            }

        }

        //-------------------------------------------------------------------------
    }
}

//---------------------------------------------------------------------------------

public enum VehicleID {
        wbConcreteMixer,
        wbConcretePump,
        wbDepositTipper,
        wbEscortSchleicher,
        wbExcavator,
        wbFlatbedTruck,
        wbFlatTopCrane,
        wbForklift,
        wbFrontLoader,
        wbGeneratorTrailer,
        wbHalfpipeTruck,
		wbHalfpipeTrailer,
        wbHeavyDutyTrailer,
        wbLittleFlatbedTruck,
        wbLittleHalfpipeTruck,
        wbLowLoaderTrailer,
        wbLowLoaderTruck,
        wbMiniExcavator,
        wbPlattmaker,
        wbRotaryDrillingRig,
        wbTowerCrane,
        wbTrailer,
        wbTrailerFlatbed,
        wbTrailerSmall,
        wbTruckCrane
}

public enum VehicleFunction {
    Concrete_Pump,
    Concrete_Mixer,
    Transport_Bulk,
    Transport_Cargo,
    Crane,
    Excavator,
    Roller
}
