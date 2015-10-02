using UnityEngine;
using System.Collections;

public class WbCompRetailManager : MonoBehaviour {

    //-------------------------------------------------------------------------
    // MonoBehaviour
    //-------------------------------------------------------------------------

	void Start () {
	
	}
    //-------------------------------------------------------------------------
	
	void Update () {
	
	}

    //-------------------------------------------------------------------------
}

public enum WbLocationName {

    HomebaseVillage = 0,
    HomebaseCity = 1,
    HarbourCity = 2,
    TrainStationVillage = 3,
    TrainStationCity = 4,
    MaterialsTraderVillage = 5,
    MaterialsTraderCity = 6,
    VehicleTraderCity = 7,
    WallFactoryCity = 8,
    SteelFactoryCity = 9,
    NurseryGardenVillage = 10,
    Sandbox = 11,
    SawmillVillage = 12,
    SawmillCity = 13,
    //Dam = 14,
    Mission = 98,
    Undefined = 99,
    Custom = 100
}
