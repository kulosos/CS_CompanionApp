using UnityEngine;
using System.Collections;
using Wb.Companion.Core.UI;
using System;

namespace Wb.Companion {

	public class NetworkManager : MonoBehaviour {

		private bool D = true; //DEBUGGING

		public static NetworkManager instance;

	    public int defaultMaxConnecitions = 4;
	    public string defaultip = "127.0.0.1";
	    public int defaultPort = 25000;
		public UIManager uiManager;

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
		}

	    // Update is called once per frame
	    void Update() {
	    }

		//---------------------------------------------------------------------

	    void OnGUI() {

			if (GUI.Button(new Rect(0, 5, 150, 50), "launch Server")){
	            NetworkManager.launchServer("4", "25000", "pw");
	        }

			if (GUI.Button(new Rect(170, 5, 150, 50), "connect")) {
	           Network.Connect(this.defaultip, this.defaultPort);
	        }

			if (GUI.Button(new Rect(330, 5, 150, 50), "disconnect")) {
				NetworkManager.disconnect();
	        }

			if (GUI.Button(new Rect(490, 5, 150, 50), "info")) {
				NetworkManager.connectionInfo();
	        }
	        
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
			Debug.Log ("Disconnecting ....");
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
			this.uiManager.switchGameUI();
		}

		// Call on the client
		private void OnDisconnectedFromServer(){
			Debug.Log("Server connection disconnected.");
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

		}

	}
}