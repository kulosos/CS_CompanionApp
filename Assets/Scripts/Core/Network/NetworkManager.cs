/**
* @brief		Network Management (RPCs, NetworkView, LAN)
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			May 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using Wb.Companion.Core.UI;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.Inputs;
using System;
using UnityEngine.UI;

namespace Wb.Companion.Core.WbNetwork {

	public class NetworkManager : MonoBehaviour {

		public static NetworkManager instance;

		public int defaultMaxConnecitions = 4;
		public string defaultIP = "127.0.0.1";
		public int defaultPort = 25000;
        public string defaultPassword = "pw";
        public float globalRPCSendRate = 15;
        public bool clampSendRate = false;

		public UIManager uiManager;
		public SceneManager sceneManager;
		public NetworkView networkView;

		public bool isActiveConnection = false;
        public bool debugging = true;

        public Text labelSliderDrivingRPCSendRate;
        public Text labelSliderCraneRPCSendRate;

        //---------------------------------------------------------------------
		
		public void Awake() {
			NetworkManager.instance = this;
		}
		
		public static NetworkManager getInstance(){
			return NetworkManager.instance;
		}
		
		//---------------------------------------------------------------------

		void Start() {

            // Set Network Log Level for Information
            if (debugging) {
                Network.logLevel = NetworkLogLevel.Informational;
            } else {
                Network.logLevel = NetworkLogLevel.Off;
            }

			this.networkView = GetComponent<NetworkView>();

            // Clamps max RenterTexture Rate per second to the max RPC Send rate per second
            if (globalRPCSendRate > Network.sendRate && clampSendRate) globalRPCSendRate = Network.sendRate;
            if (debugging) Debug.Log("Global RPC Send Rate = " + this.globalRPCSendRate);
            
		}

		//---------------------------------------------------------------------

        public void changeOwner() {
            NetworkViewID newViewId = Network.AllocateViewID();
            networkView.RPC("DidAllocateNewId", RPCMode.All, newViewId);
        }

        //---------------------------------------------------------------------

        [RPC]
        void DidAllocateNewId(NetworkViewID newId) {
            networkView.viewID = newId;
        }

        //---------------------------------------------------------------------

		public static void launchServer(string maxConnections, string listenport, string password) {
			Debug.Log("Init Server");
			Network.incomingPassword = password;
			bool useNat = !Network.HavePublicAddress();

			int maxCon = int.Parse(maxConnections);
			int lPort = int.Parse(listenport);

			Network.InitializeServer(maxCon, lPort, useNat);
		}

		//---------------------------------------------------------------------

		public static void connect(string ip, string port, string password){ 
			int portNum = Convert.ToInt32(port);
			Network.Connect(ip, portNum, password);
			Debug.Log ("Connecting to " + ip +":"+ portNum + " - Password: " + password);	
		}

		//---------------------------------------------------------------------
		
		public static void disconnect(){
			Debug.Log ("Disconnecting ...");
			if(Network.connections.Length > 0){
				Network.Disconnect();
				//MasterServer.UnregisterHost();
			}else{
				Debug.Log ("There is no connection to disconnect");
			}
		}

		//---------------------------------------------------------------------

		public static void connectionInfo(){

			if(Network.isClient) {Debug.Log ("Running as a Client");}
			if(Network.isServer) {Debug.Log ("Running as a Server");}


			if (Network.peerType == NetworkPeerType.Disconnected){
				Debug.Log ("Not Connected");
			}else{
				if (Network.peerType == NetworkPeerType.Connecting){
					Debug.Log ("Connecting");
				} else{
					Debug.Log("Network started");
				}
			}

			if(Network.connections.Length > 0){

				for(int i = 0; i < Network.connections.Length; i++){
					Debug.Log("Connection[" + i + "]: " + Network.connections[i].ipAddress + ":" + Network.connections[0].port);
				}

			}else{
				Debug.Log ("No open network connections.");
				//Debug.Log ("LastPing: " + Network.GetLastPing().ToString());
			}
		}
 
		//---------------------------------------------------------------------

		// Call on the client when server connection successfully established
		private void OnConnectedToServer() {
			Debug.Log("Server connection successfully established.");
            //this.uiManager.switchGameUI();
           
			this.sceneManager.loadScene(this.sceneManager.getDefaultStartScene());
			NetworkManager.getInstance().isActiveConnection = true;

		}

		// Call on the client
		private void OnDisconnectedFromServer(){
			Debug.Log("Server connection disconnected.");
            this.uiManager.unloadGameUI();
			NetworkManager.getInstance().isActiveConnection = false;
		}

		// Call on the server when player has successfully connected
		private void OnPlayerConnected(){
			Debug.Log("A player was successfully connected to server.");
		}

		// Call on the server when a player is disconnected
		private void OnPlayerDisconnected(){
			Debug.Log("A player has diconnected from server.");
			NetworkManager.getInstance().isActiveConnection = false;
		}

		// Called on client
		private void OnFailedToConnect() {
			Debug.Log("Failed to establish server connection.");
			this.uiManager.setConnectionErrorMsg("Failed to establish server connection");
            this.uiManager.setConnectionLoadingBar();

            // HACK this is only for debugging purposes
            if(debugging){
                this.sceneManager.loadScene(this.sceneManager.getDefaultStartScene());
                //this.uiManager.loadGameUI();
            }
		}

        //---------------------------------------------------------------------
        // SETTER / GETTER
        //---------------------------------------------------------------------

        public void setGlobalRPCSendRate(float value) {
            globalRPCSendRate = value;
            labelSliderDrivingRPCSendRate.text = value.ToString();
            labelSliderCraneRPCSendRate.text = value.ToString();
        }

        public float getGlobalRPCSendRate() {
            return globalRPCSendRate;
        }

	}
}