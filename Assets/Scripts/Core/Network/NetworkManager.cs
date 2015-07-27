using UnityEngine;
using System.Collections;
using Wb.Companion.Core.UI;
using Wb.Companion.Core.WbAdministration;
using System;

namespace Wb.Companion.Core.WbNetwork {

	public class NetworkManager : MonoBehaviour {

		private bool D = true; //DEBUGGING

		public static NetworkManager instance;

		public int defaultMaxConnecitions = 4;
		public string defaultIP = "127.0.0.1";
		public int defaultPort = 25000;
		public UIManager uiManager;
		public SceneManager sceneManager;

		public Transform cubePrefab;
		public NetworkView nView;
		public bool lauchServerOnStart = false;


		//---------------------------------------------------------------------
		
		public void Awake() {
			NetworkManager.instance = this;
		}
		
		public static NetworkManager getInstance(){
			return NetworkManager.instance;
		}
		
		//---------------------------------------------------------------------
	
		// Use this for initialization
		void Start() {
			if(D)Network.logLevel = NetworkLogLevel.Informational;
			nView = GetComponent<NetworkView>();

			if(lauchServerOnStart){
				NetworkManager.launchServer("4", "25000", "pw");
			}
		}

		// Update is called once per frame
		void Update() {
		}
	
		//---------------------------------------------------------------------

		public void spawner(){
			NetworkViewID viewID = Network.AllocateViewID();
			nView.RPC("SpawnBox", RPCMode.AllBuffered, viewID, transform.position);
		}

		public void rpcBtn(string str){
			NetworkViewID viewID = Network.AllocateViewID();
			nView.RPC("rpcTest", RPCMode.AllBuffered, "this is da shit from the method which is calling the real rpc test method");
		}

		public void holdFire(string txt, bool active){
            //NetworkViewID viewID = Network.AllocateViewID();
			nView.RPC("rpcTest", RPCMode.AllBuffered, txt, active);
		}

		public void disconnectBtn(){
			NetworkManager.disconnect();
		}

        public void launchServerBtn() {
            NetworkManager.launchServer("4", "25000", "pw");
        }

		public void connectionInfoBtn(){
			NetworkManager.connectionInfo();
		}

		//---------------------------------------------------------------------
        // Remote Procedure Calls
        //---------------------------------------------------------------------

        public void sendRPCTiltInput(float value) {

            nView.RPC("tiltInput", RPCMode.AllBuffered, value);

   
        }

        //[RPC]
        //public void rpcTest(string txt) {
        //    Debug.Log(txt);
        //}

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
				//                Debug.Log ("LastPing: " + Network.GetLastPing().ToString());
				//                Debug.Log ("Connections: " + Network.;
			}
		}
 
		//---------------------------------------------------------------------

		// Call on the client when server connection successfully established
		private void OnConnectedToServer() {
			Debug.Log("Server connection successfully established.");
            //this.uiManager.switchGameUI();
           
			this.sceneManager.loadScene(this.sceneManager.getDefaultStartScene());
		}

		// Call on the client
		private void OnDisconnectedFromServer(){
			Debug.Log("Server connection disconnected.");
            this.uiManager.unloadGameUI();
		}

		// Call on the server when player has successfully connected
		private void OnPlayerConnected(){
			Debug.Log("A player was successfully connected to server.");
		}

		// Call on the server when a player is disconnected
		private void OnPlayerDisconnected(){
			Debug.Log("A player has diconnected from server.");
		}

		// Called on client
		private void OnFailedToConnect() {
			Debug.Log("Failed to establish server connection.");
			this.uiManager.setConnectionErrorMsg("Failed to establish server connection");
            this.uiManager.setConnectionLoadingBar();
            if(D){
                this.sceneManager.loadScene(this.sceneManager.getDefaultStartScene());
                //this.uiManager.loadGameUI();
            }
		}

		// -----------------------------------------------------------------




	}
}