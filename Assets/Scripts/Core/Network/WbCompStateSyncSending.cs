using UnityEngine;
using System.Collections;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbNetwork {

    public class WbCompStateSyncSending : MonoBehaviour, ICompNetworkOwner {

        public static WbCompStateSyncSending instance;
        public NetworkView networkView;

        //---------------------------------------------------------------------
        // INPUT VALUES
        //---------------------------------------------------------------------

        // DRIVING
        public float driving_accelerate = 0f;
        public float driving_brake = 0f;
        public float driving_steerLeft = 0f;
        public float driving_steerRight = 0f;

        // TRUCK CRANE
        public float truckcrane_boom01Up = 0f;
        public float truckcrane_boom01Down = 0f;
        public float truckcrane_boomMainLeft = 0f;
        public float truckcrane_boomMainRight = 0f;
        public float truckcrane_boomForward = 0f;
        public float truckcrane_boomBackward = 0f;
        public float truckcrane_cabinUp = 0f;
        public float truckcrane_cabinDown = 0f;
        public float truckcrane_ropeUp = 0f;
        public float truckcrane_ropeDown = 0f;
        public float truckcrane_supportLegsIn = 0f;
        public float truckcrane_supportLegsOut = 0f;
	
        //---------------------------------------------------------------------
        // Singleton
        //---------------------------------------------------------------------

        public static WbCompStateSyncSending getInstance() {
            return WbCompStateSyncSending.instance;
        }

        //---------------------------------------------------------------------
        // Mono Behaviour
        //---------------------------------------------------------------------

        public void Awake() {
            WbCompStateSyncSending.instance = this;
        }

        //---------------------------------------------------------------------

        void Start() {
            networkView = GetComponent<NetworkView>();
        }

        //---------------------------------------------------------------------

        void Update() {

        }

        //---------------------------------------------------------------------

        void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {

            // SENDING
            if (stream.isWriting) {

                // DRIVING
				stream.Serialize(ref driving_accelerate);
                stream.Serialize(ref driving_brake);
                stream.Serialize(ref driving_steerLeft);
                stream.Serialize(ref driving_steerRight);

                // TRUCK CRANE
				stream.Serialize(ref truckcrane_boom01Up);
				stream.Serialize(ref truckcrane_boom01Down);
				stream.Serialize(ref truckcrane_boomMainLeft);
				stream.Serialize(ref truckcrane_boomMainRight);
				stream.Serialize(ref truckcrane_boomForward);
				stream.Serialize(ref truckcrane_boomBackward);
				stream.Serialize(ref truckcrane_cabinUp);
				stream.Serialize(ref truckcrane_cabinDown);
				stream.Serialize(ref truckcrane_ropeUp);
				stream.Serialize(ref truckcrane_ropeDown);
				stream.Serialize(ref truckcrane_supportLegsIn);
				stream.Serialize(ref truckcrane_supportLegsOut);
			
            } 

            // RECEIVING
            else{
	

            }
         }

        //---------------------------------------------------------------------
        public void setVehicleInput(string inputkey, float value) {

            //Debug.Log("setVehicleInput - " + inputkey + " / " + value );

            // DRIVING---------------------------------------------------------
            if(InputKeys.DRIVING_ACCELERATE.Equals(inputkey)){
                driving_accelerate = value;
                return;
            }

            if (InputKeys.DRIVING_BRAKE.Equals(inputkey)) {
                driving_brake = value;
                return;
            }

            if (InputKeys.DRIVING_STEER_LEFT.Equals(inputkey)) {
                driving_steerLeft = value;
                return;
            }

            if (InputKeys.DRIVING_STEER_RIGHT.Equals(inputkey)) {
                driving_steerRight = value;
                return;
            }

            // TRUCK CRANE-----------------------------------------------------
            if (InputKeys.TRUCKCRANE_BOOM_01_UP.Equals(inputkey)) {
                truckcrane_boom01Up = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_BOOM_01_DOWN.Equals(inputkey)) {
                truckcrane_boom01Down = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_BOOM_MAIN_LEFT.Equals(inputkey)) {
                truckcrane_boomMainLeft = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_BOOM_MAIN_RIGHT.Equals(inputkey)) {
                truckcrane_boomMainRight = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_BOOM_FORWARD.Equals(inputkey)) {
                truckcrane_boomForward = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_BOOM_BACKWARD.Equals(inputkey)) {
                truckcrane_boomBackward = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_CABIN_UP.Equals(inputkey)) {
                truckcrane_cabinUp = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_CABIN_DOWN.Equals(inputkey)) {
                truckcrane_cabinDown = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_ROPE_UP.Equals(inputkey)) {
                truckcrane_ropeUp = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_ROPE_DOWN.Equals(inputkey)) {
                truckcrane_ropeDown = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_SUPPORTLEGS_IN.Equals(inputkey)) {
                truckcrane_supportLegsIn = value;
                return;
            }

            if (InputKeys.TRUCKCRANE_SUPPORTLEGS_OUT.Equals(inputkey)) {
                truckcrane_supportLegsOut = value;
                return;
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
}
