using UnityEngine;
using System.Collections;
using Wb.Companion.Core.Game;
using Wb.Companion.Core.WbAdministration;

public class WbCompStateSyncReceiving : MonoBehaviour, ICompNetworkOwner {

	public static WbCompStateSyncReceiving instance;
	public NetworkView networkView;

	// Tachometer Values ------------------------------------------------------
	public float vehicleSpeed = 0f;
	public float vehicleRPM = 0f;

    // VEHICLE MAP POSITIONS --------------------------------------------------
    public Vector3 wbConcreteMixer = Vector3.zero;
    public Vector3 wbConcretePump = Vector3.zero;
    public Vector3 wbDepositTipper = Vector3.zero;
    public Vector3 wbEscortSchleicher = Vector3.zero;
    public Vector3 wbExcavator = Vector3.zero;
    public Vector3 wbFlatbedTruck = Vector3.zero;
    public Vector3 wbFlatTopCrane = Vector3.zero;
    public Vector3 wbForkLift = Vector3.zero;
    public Vector3 wbFrontLoader = Vector3.zero;
    public Vector3 wbGeneratorTrailer = Vector3.zero;
    //public Vector3 HalfpipeTrailer = Vector3.zero;
    public Vector3 wbHalfpipeTruck = Vector3.zero;
    public Vector3 wbHeavyDutyTrailer = Vector3.zero;
    public Vector3 wbLittleFlabBedTruck = Vector3.zero;
    public Vector3 wbLittleHalfpipeTruck = Vector3.zero;
    public Vector3 wbLowLoaderTrailer = Vector3.zero;
    public Vector3 wbMiniExcavator = Vector3.zero;
    public Vector3 wbPlattmaker = Vector3.zero;
    public Vector3 wbRotaryDrillingRig = Vector3.zero;
    public Vector3 wbTowerCrane = Vector3.zero;
    public Vector3 wbTrailer = Vector3.zero;
    //public Vector3 wbTrailerDrawbarFlatbed = Vector3.zero;
    public Vector3 wbTrailerFlatbed = Vector3.zero;
    public Vector3 wbTrailerSmall = Vector3.zero;
    public Vector3 wbTruckCrane = Vector3.zero;

    //---------------------------------------------------------------------
    // Singleton
    //---------------------------------------------------------------------

    public static WbCompStateSyncReceiving getInstance() {
		return WbCompStateSyncReceiving.instance;
	}
	
	//---------------------------------------------------------------------
	// Mono Behaviour
	//---------------------------------------------------------------------
	
	public void Awake() {
		WbCompStateSyncReceiving.instance = this;
	}
	
	void Start () {
		networkView = GetComponent<NetworkView>();
	}

	void Update () {
	
	}

	//---------------------------------------------------------------------
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {

		// SENDING
		if (stream.isWriting) {

		} 
		
		// RECEIVING
		else{
			// Tachometer Values
			stream.Serialize(ref vehicleSpeed);
			stream.Serialize(ref vehicleRPM);

            // Vehicle Positions
            stream.Serialize(ref wbConcreteMixer);
            SceneManager.getInstance().getMapManager().setVehiclePositions(WbCompVehicleData.WB_CONCRETE_MIXER, wbConcreteMixer);

            stream.Serialize(ref wbConcretePump);
            stream.Serialize(ref wbDepositTipper);
            SceneManager.getInstance().getMapManager().setVehiclePositions(WbCompVehicleData.WB_DEPOSIT_TIPPER, wbDepositTipper);

            stream.Serialize(ref wbEscortSchleicher);
            stream.Serialize(ref wbExcavator);
            stream.Serialize(ref wbFlatbedTruck);
            stream.Serialize(ref wbFlatTopCrane);
            stream.Serialize(ref wbForkLift);
            stream.Serialize(ref wbFrontLoader);
            stream.Serialize(ref wbGeneratorTrailer);
            stream.Serialize(ref wbHalfpipeTruck);
            stream.Serialize(ref wbHeavyDutyTrailer);

            stream.Serialize(ref wbLittleFlabBedTruck);
            SceneManager.getInstance().getMapManager().setVehiclePositions(WbCompVehicleData.WB_LITTLE_FLATBED_TRUCK, wbLittleFlabBedTruck);

            stream.Serialize(ref wbLittleHalfpipeTruck);
            stream.Serialize(ref wbLowLoaderTrailer);
            stream.Serialize(ref wbMiniExcavator);
            stream.Serialize(ref wbPlattmaker);
            stream.Serialize(ref wbRotaryDrillingRig);
            stream.Serialize(ref wbTowerCrane);
            stream.Serialize(ref wbTrailer);
            stream.Serialize(ref wbTrailerFlatbed);
            stream.Serialize(ref wbTrailerSmall);

            stream.Serialize(ref wbTruckCrane);
            SceneManager.getInstance().getMapManager().setVehiclePositions(WbCompVehicleData.WB_TRUCK_CRANE, wbTruckCrane);
        }
	}

	//---------------------------------------------------------------------
	// Interface Implementations
	//---------------------------------------------------------------------
	
	public void setAsOwner(){
		Debug.Log ("Set WbCompStateSyncSending as Owner");
		NetworkViewID newViewId = Network.AllocateViewID();
		this.networkView.RPC("allocateNewNetworkViewID", RPCMode.All, newViewId);
	}
	
	//---------------------------------------------------------------------
	
	[RPC]
	public void allocateNewNetworkViewID(NetworkViewID newId){
		Debug.Log ("Change Owner for Sender");
		this.networkView.viewID = newId;
	}
}
