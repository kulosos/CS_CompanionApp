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
		public int imgPart;
		public int imgWidth;
		public int imgHeight;
		public int imgSlices;

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
		public void sendRPCbroadcastCamera(string imgData, int part, int width, int height, int slices){
			backViewCameraFrameBase64 = imgData;
			imgPart = part;
			imgWidth = width;
			imgHeight = height;
			imgSlices = slices;
		}

		//------ SETTER / GETTER ---------------------------------------------

		public string getBackViewCameraFrameAsB64String(){
			return backViewCameraFrameBase64;
		}
	
    }

}