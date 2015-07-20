﻿using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbNetwork;
using Wb.Companion.Core.UI;
using Wb.Companion;
using System;

namespace Wb.Companion.Core.WbAdministration {

    //-------------------------------------------------------------------------

    public static class SceneList
    {
        public static string Main = "00_Main";
        public static string Map = "01_Map";
        public static string RemoteControl = "02_RemoteControl";
    }

    //-------------------------------------------------------------------------

    public class SceneManager : MonoBehaviour {

        private bool D = true; //DEBUGGING
        public UIManager uiManager;
        public string currentScene;
		private string DefaultStartScene;// = SceneList.RemoteControl;

        //---------------------------------------------------------------------
        // MonoBehaviour
        //---------------------------------------------------------------------

		void Awake() {

		}

        void Start() {
        }

        void Update() {
        }


        //---------------------------------------------------------------------

        public void loadScene(string scene) {
            //Debug.Log("sakldgöklsadgösdajgökjsdagökjhsdagök");
            this.uiManager.showLoadingBar();
            StartCoroutine(levelLoaded(scene));
        }

        private IEnumerator levelLoaded(string scene) {
            this.uiManager.showLoadingBar();
            yield return Application.LoadLevelAdditiveAsync(scene); ;
            if(D)Debug.Log("Level: " + scene + " was loaded");
            this.uiManager.hideLoadingBar();
            this.uiManager.loadGameUI();
            this.setCurrentScene(scene);
        }

        // SETTER / GETTER ----------------------------------------------------

        public string[] getSceneList() {
            string [] sceneList = new string[] { SceneList.Map, SceneList.RemoteControl };
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