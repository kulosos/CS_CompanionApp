using UnityEngine;
using System.Collections;
using Wb.Companion.Core;

namespace Wb.Companion {

	public class NetworkManager : MonoBehaviour {

		public static NetworkManager instance;

	    public int defaultMaxConnecitions = 4;
	    public string defaultip = "127.0.0.1";
	    public int defaultPort = 25000;

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
	    }

	    // Update is called once per frame
	    void Update() {
	    }

		//---------------------------------------------------------------------

	    void OnGUI() {

	        if (GUI.Button(new Rect(10, 100, 400, 80), "launch Server")){
	            NetworkManager.launchServer("4", "25000", "pw");
	        }

	        if (GUI.Button(new Rect(10, 200, 400, 80), "connect")) {
	            Debug.Log("connect");
	           // Network.Connect(this.ip, this.listenPort);
	        }

	        if (GUI.Button(new Rect(10, 300, 400, 80), "disconnect")) {
	            Debug.Log("disconnect");
				NetworkManager.disconnect();
	        }

	        if (GUI.Button(new Rect(10, 400, 400, 80), "info")) {

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

			Network.InitializeServer(maxCon, lPort);
	    }

		//---------------------------------------------------------------------
		
		void OnConnectedToServer() {
			Debug.Log("Server connection successfully established.");
		}

		//---------------------------------------------------------------------

		public static void connect(string ip, string port, string password){
			Network.Connect(ip, port);
			Debug.Log ("Connection established for " + ip +":"+ port + " - Password: " + password);
			
		}

		//---------------------------------------------------------------------
		
		public static void disconnect(){
			Debug.Log ("Connection disconnected");
			Network.Disconnect();
			MasterServer.UnregisterHost();
		}

		//---------------------------------------------------------------------

		public static void connectionInfo(){

			if(Network.connections.Length > 0){

				for(int i = 0; i <= Network.connections.Length; i++){
					Debug.Log("Connection[" + i + "]: " + Network.connections[i].ipAddress + ":" + Network.connections[0].port);
				}

			}else{
				Debug.Log ("No open network connections.");
				//                Debug.Log ("LastPing: " + Network.GetLastPing().ToString());
				//                Debug.Log ("Connections: " + Network.;
			}
		}
	}
}