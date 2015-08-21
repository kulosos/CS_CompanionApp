using UnityEngine;
using System.Collections;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbNetwork {

    public class WbCompRPCWrapper : MonoBehaviour {

        public static WbCompRPCWrapper instance;
        public NetworkView networkView;

		public bool debugging = false;

		private float currentSpeed; 
		private float currentRPM;

        // ------------------------------------------------------------------------

        public static WbCompRPCWrapper getInstance() {
            return WbCompRPCWrapper.instance;
        }

        // ------------------------------------------------------------------------
        // MonoBehaviour
        // ------------------------------------------------------------------------

        public void Awake() {
            WbCompRPCWrapper.instance = this;
        }

        void Start() {
            this.networkView = GetComponent<NetworkView>();
        }

        void Update() {

        }

        // ------------------------------------------------------------------------

        public void disconnectBtn() {
            NetworkManager.disconnect();
        }

        public void launchServerBtn() {
            NetworkManager.launchServer("4", "25000", "pw");
        }

        public void connectionInfoBtn() {
            NetworkManager.connectionInfo();
        }

        //---------------------------------------------------------------------
        // Set Methods
        //---------------------------------------------------------------------

        public void setThrottle(string input, float value) {
            //NetworkViewID viewID = Network.AllocateViewID();
			networkView.RPC("setRPCThrottleInput", RPCMode.Server, input, value);
        }

        public void setNextCamera(string input) {
			networkView.RPC("setRPCNextCamera", RPCMode.Server, input);
        }

        public void setVehicleInput(string inputkey, float value) {
            networkView.RPC("setRPCVehicleInput", RPCMode.Server, inputkey, value);
        }

        public void setTiltInput(float value) {
            //if(debugging)Debug.Log(value);
			networkView.RPC("setRPCTiltInput", RPCMode.Server, value);
        }

        //---------------------------------------------------------------------
        // Remote Procedure Calls
        //---------------------------------------------------------------------
		// OUT GOING RPCs
		//---------------------------------------------------------------------

        [RPC]
        public void setRPCThrottleInput(string txt, float value) {
            //Debug.Log(txt);
        }

        [RPC]
        public void setRPCNextCamera(string txt) {
            //Debug.Log(txt);
        }

        [RPC]
        public void setRPCGetIntoVehicle(string txt) {
            //Debug.Log(txt);
        }

        [RPC]
        public void setRPCTiltInput(float value) {
            //Debug.Log ("TiltInput: " + value);
        }

		//---------------------------------------------------------------------
		// INCOMING RPCs
		//---------------------------------------------------------------------

		[RPC]
		public void setRPCVehicleRPM(float value){
			//if(debugging)Debug.Log ("CurrentVehicleRPM Value: " + value);
			currentRPM = value;
		}

		[RPC]
		public void setRPCVehicleSpeed(float value){
			//if(debugging)Debug.Log ("CurrentVehicleSpeed Value: " + value);
			currentSpeed = value;
		}


		//------ SETTER / GETTER ---------------------------------------------

		public float getCurrentRPM(){
			return currentRPM;
		}

		public float getCurrentSpeed(){
			return currentSpeed;
		}
	
    }

}