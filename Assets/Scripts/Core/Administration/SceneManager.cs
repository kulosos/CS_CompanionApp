using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Wb.Companion;
using Wb.Companion.Core.WbNetwork;
using Wb.Companion.Core.UI;
using Wb.Companion.Core.Inputs;

namespace Wb.Companion.Core.WbAdministration {

    //-------------------------------------------------------------------------

    public static class SceneList
    {
        public static string Main = "00_Main";
        public static string Map = "01_Map";
        public static string RemoteControlDriving = "02_RemoteControl_Driving";
        public static string RemoteControlCrane = "03_RemoteControl_Crane";
    }

    //-------------------------------------------------------------------------

    public class SceneManager : MonoBehaviour {

        private bool debugging = true;
        public UIManager uiManager;
        public string currentScene;
        [SerializeField]
		private string DefaultStartScene;
        private List<WbUIThumbstick> thumbsticks = new List<WbUIThumbstick>();

        //---------------------------------------------------------------------
        // MonoBehaviour
        //---------------------------------------------------------------------

        void Start() {

            // Get all Thumbsticks
            WbUIThumbstick[] sticks = WbUIThumbstick.FindObjectsOfType(typeof(WbUIThumbstick)) as WbUIThumbstick[];
            foreach (WbUIThumbstick stick in sticks) {
                this.thumbsticks.Add(stick);
            }
            Debug.Log("ts count: " + this.thumbsticks.Count);
        }

        void Update() {
        }

        //---------------------------------------------------------------------

        public void loadScene(string scene) {

            if (!this.currentScene.Equals(scene)) {

                //this.uiManager.showLoadingScreen();

                GameObject sceneData = GameObject.Find("SceneData");
                if (sceneData != null) {
                    Destroy(sceneData);
                }
                              
                StartCoroutine(levelLoaded(scene));
            } else {
                this.uiManager.unloadMainMenu("true");
            }
        }

        //---------------------------------------------------------------------

        private IEnumerator levelLoaded(string scene) {
            yield return Application.LoadLevelAdditiveAsync(scene);
            if(debugging)Debug.Log("Level: " + scene + " was loaded");
            this.uiManager.hideLoadingScreen();
            this.uiManager.loadGameUI();
            this.setCurrentScene(scene);
            this.initLoadedLevel(scene);
        }

        //---------------------------------------------------------------------

        private void initLoadedLevel(string scene) {

            // REMOTE CONTROL DRIVING SCENE
            if (scene.Equals(SceneList.RemoteControlDriving)) {
                InputManager.getInstance().isActiveTiltInput = true;
            } else {
                InputManager.getInstance().isActiveTiltInput = false;
            }

            // REMOTE CONTROL CRANE SCENE
            if (scene.Equals(SceneList.RemoteControlCrane)) {
                foreach (WbUIThumbstick stick in this.thumbsticks) {
                    stick.gameObject.SetActive(true);
                }
            } else {
                foreach (WbUIThumbstick stick in this.thumbsticks) {
                    stick.gameObject.SetActive(false);
                }
            }

            CameraManager.getInstance().setInitialCameraOnSceneLoading(scene);
        }

        // SETTER / GETTER ----------------------------------------------------

        public string[] getSceneList() {
            string[] sceneList = new string[] { SceneList.Map, SceneList.RemoteControlDriving, SceneList.RemoteControlCrane };
            return sceneList;
        }

        public void setDefaultStartScene(int scene) {
            this.DefaultStartScene = this.getSceneList()[scene];
        }

        public string getDefaultStartScene() {
            return this.DefaultStartScene;
        }

		public int getDefaultStartSceneInt(){
			int sceneId = 0;
	
			for(int i = 0; i < this.getSceneList().Length; i++){
				if(this.getDefaultStartScene().Equals(this.getSceneList()[i])){
					sceneId = i;
				}
			}
			return sceneId;
		}

        public void setCurrentScene(string scene) {
            this.currentScene = scene;
        }

        public string getCurrentScene() {
            return this.currentScene;
        }

    }

}