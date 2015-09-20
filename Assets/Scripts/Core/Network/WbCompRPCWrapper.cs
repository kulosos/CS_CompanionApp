using UnityEngine;
using System.Collections;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbNetwork {

    public class WbCompRPCWrapper : MonoBehaviour {

        public static WbCompRPCWrapper instance;
        public NetworkView networkView;

		public bool debugging = false;

		//private float currentSpeed; 
		//private float currentRPM;

		private string backViewCameraFrameBase64;
		private byte[] backViewCameraFrameByteArray;

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

        //---------------------------------------------------------------------
        // Remote Procedure Calls
        //---------------------------------------------------------------------
		// OUT GOING RPCs
		//---------------------------------------------------------------------


		//---------------------------------------------------------------------
		// INCOMING RPCs
		//---------------------------------------------------------------------

		[RPC]
		public void sendRPCbroadcastCamera(string imgData){
			backViewCameraFrameBase64 = imgData;
		}


		//------ SETTER / GETTER ---------------------------------------------

		public string getBackViewCameraFrameAsB64String(){
			return backViewCameraFrameBase64;
		}
	
    }

}