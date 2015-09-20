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
		
		float speed = 0f;
		float rpm = 0f;
		
		// SENDING
		if (stream.isWriting) {

		} 
		
		// RECEIVING
		else{

			// Tachometer Values
			stream.Serialize(ref speed);
			vehicleSpeed = speed;

			stream.Serialize(ref rpm);
			vehicleRPM = rpm;
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
