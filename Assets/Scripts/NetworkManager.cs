using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    public int maxConnecitions = 8;
    public string ip = "127.0.0.1";
    public int listenPort = 25001;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnGUI() {

        if (GUI.Button(new Rect(10, 100, 400, 80), "launch Server")){
            this.launchServer();

        }


        if (GUI.Button(new Rect(10, 200, 400, 80), "connect")) {
            Debug.Log("connect");
            Network.Connect(this.ip, this.listenPort);

        }

        if (GUI.Button(new Rect(10, 300, 400, 80), "disconnect")) {
            Debug.Log("disconnect");
            Network.Disconnect();
            MasterServer.UnregisterHost();
        }

        if (GUI.Button(new Rect(10, 400, 400, 80), "info")) {



            if(Network.connections.Length > 0){
                Debug.Log("Connection0: " + Network.connections[0].ipAddress + ":" + Network.connections[0].port);
        
            }else{
                Debug.Log ("Connections: " + Network.connections.Length.ToString());
//                Debug.Log ("LastPing: " + Network.GetLastPing().ToString());
//                Debug.Log ("Connections: " + Network.;
            }
        }
        
    }

    void OnConnectedToServer() {
        Debug.Log("Connected to server");
    }

    /* ---------------------------------------------------------------------------*/

    void launchServer() {
        Debug.Log("Init Server");
        //        Network.incomingPassword = "test";
        bool useNat = !Network.HavePublicAddress();
        Network.InitializeServer(maxConnecitions, listenPort);
    }
}