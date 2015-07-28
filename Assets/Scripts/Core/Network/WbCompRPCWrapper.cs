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

            // Tilt Input
            if (InputManager.getInstance().activeConnection && InputManager.getInstance().activeTiltInput) {
                float tiltValue = InputManager.getInstance().CalcAxisValue(InputManager.TiltAxis.sideways);
                Debug.Log(tiltValue);
                sendRPCTiltInput(tiltValue);
            }

        }

        // ------------------------------------------------------------------------

        //public void rpcBtn(string str){
        //    NetworkViewID viewID = Network.AllocateViewID();
        //    nView.RPC("rpcTest", RPCMode.AllBuffered, str, 1234.56f);
        //}

        public void holdFire(string input, float value) {
            //NetworkViewID viewID = Network.AllocateViewID();
            networkView.RPC("throttleInput", RPCMode.AllBuffered, input, value);
        }

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
        // Remote Procedure Calls
        //---------------------------------------------------------------------

        public void sendRPCTiltInput(float value) {
            networkView.RPC("tiltInput", RPCMode.AllBuffered, value);
        }

        [RPC]
        public void throttleInput(string txt, float value) {
            //Debug.Log(txt);
        }

        [RPC]
        public void tiltInput(float value) {
            //Debug.Log ("TiltInput: " + value);
        }
    }

}