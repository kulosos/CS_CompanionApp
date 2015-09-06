using UnityEngine;
using System.Collections;
using Wb.Companion.Core.UI;
using Wb.Companion.Core.Inputs;
using Wb.Companion.Core.WbCamera;

namespace Wb.Companion.Core.WbAdministration {

    //-------------------------------------------------------------------------

    public static class SceneList
    {
        public static string Main = "00_Main";                                  
        public static string Map = "01_Map";
        public static string RemoteControlDriving = "02_RemoteControl_Driving"; 
        public static string RemoteControlCrane = "03_RemoteControl_Crane";
		public static string BackViewCamera = "04_BackViewCamera";
    }

    //-------------------------------------------------------------------------

    public class SceneManager : MonoBehaviour {

        public static SceneManager instance;

        private bool debugging = true;
        public UIManager uiManager;
        public string currentScene;
        [SerializeField]
        private string defaultStartScene;
        //private List<WbUIThumbstick> thumbsticks = new List<WbUIThumbstick>();

        //---------------------------------------------------------------------
        // MonoBehaviour
        //---------------------------------------------------------------------

        public void Awake() {
            SceneManager.instance = this;
        }

        void Start() {
        }

        void Update() {
        }

        //--- SINGLETON -------------------------------------------------------

        public static SceneManager getInstance() {
            return SceneManager.instance;
        }

        //---------------------------------------------------------------------

        public void loadScene(string scene) {

            if (!SceneManager.getInstance().currentScene.Equals(scene)) {

                //SceneManager.getInstance().uiManager.showLoadingScreen();

                GameObject sceneData = GameObject.Find("SceneData");
                if (sceneData != null) {
                    Destroy(sceneData);
                }

				this.setCurrentScene(scene);

                StartCoroutine(levelLoaded(scene));
            } else {
                SceneManager.getInstance().uiManager.unloadMainMenu("true");
            }
        }

        //---------------------------------------------------------------------

        private IEnumerator levelLoaded(string scene) {
            
            if(debugging)Debug.Log("Level: " + scene + " was loaded");
            
            // after SceneLoading is complete
            this.uiManager.hideLoadingScreen();
            this.uiManager.loadGameUI();
            //this.setCurrentScene(scene);

            // toggle TiltInput RPC sending (only if it's RemoteControlDriving Scene)
            //InputManager.getInstance().toggleActiveTiltInput(scene);

            this.uiManager.initUIElementsPerScene(scene);

            CameraManager.getInstance().setInitialCameraOnSceneLoading(scene);

            yield return Application.LoadLevelAdditiveAsync(scene);
        }

        //---------------------------------------------------------------------
        // Setter / Getter
        //---------------------------------------------------------------------


        public string[] getSceneList() {
            string[] sceneList = new string[] { 
				SceneList.Map,                      // 0
				SceneList.RemoteControlDriving,     // 1  
				SceneList.RemoteControlCrane,       // 2
				SceneList.BackViewCamera            // 3
			};
            return sceneList;
        }

        //---------------------------------------------------------------------

        public void setDefaultStartScene(int sceneId) {
            this.defaultStartScene = this.getSceneList()[sceneId];
        }

        //---------------------------------------------------------------------

        public string getDefaultStartScene() {
            return this.defaultStartScene;
        }

        //---------------------------------------------------------------------

        public int getDefaultStartSceneInt(){

            int sceneId = 0;

			for(int i = 0; i < this.getSceneList().Length; i++){
				if(this.getDefaultStartScene().Equals(this.getSceneList()[i])){
					sceneId = i;
				}
			}
			return sceneId;
		}

        //---------------------------------------------------------------------

        public int getSceneId(string scene){
            int sceneId = 0;
            for (int i = 0; i < this.getSceneList().Length; i++) {
                if (scene.Equals(this.getSceneList()[i])) {
                    sceneId = i;
                }
            }
            return sceneId;
        }

        //---------------------------------------------------------------------

        public void setCurrentScene(string scene) {
            this.currentScene = scene;
        }

        //---------------------------------------------------------------------

        public string getCurrentScene() {
            return this.currentScene;
        }

    }

}