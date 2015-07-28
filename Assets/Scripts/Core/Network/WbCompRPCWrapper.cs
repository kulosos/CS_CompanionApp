using UnityEngine;
using System.Collections;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbNetwork {

    public class WbCompRPCWrapper : MonoBehaviour {

        public static WbCompRPCWrapper instance;
        public NetworkView networkView;

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
            networkView.RPC("setThrottleInput", RPCMode.AllBuffered, input, value);
        }

        public void setNextCamera(string input) {
            networkView.RPC("setRPCNextCamera", RPCMode.AllBuffered, input);
        }

        public void setGetIntoVehicle(string input) {
            networkView.RPC("setRPCGetIntoVehicle", RPCMode.AllBuffered, input);
        }

        public void setTiltInput(float value) {
            //Debug.Log(tiltValue);
            networkView.RPC("tiltInput", RPCMode.AllBuffered, value);
        }

        //---------------------------------------------------------------------
        // Remote Procedure Calls
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
    }

}