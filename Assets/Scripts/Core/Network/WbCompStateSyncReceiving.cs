using UnityEngine;
using System.Collections;

public class WbCompStateSyncReceiving : MonoBehaviour, ICompNetworkOwner {

	public static WbCompStateSyncReceiving instance;
	public NetworkView networkView;

	//---------------------------------------------------------------------
	// Tachometer Values
	//---------------------------------------------------------------------

	public float vehicleSpeed = 0f;
	public float vehicleRPM = 0f;
	public char compCamB64;


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

			//Companion Camera
			stream.Serialize(ref compCamB64);
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
