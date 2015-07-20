using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbNetwork;
using Wb.Companion.Core.UI;
using Wb.Companion;
using System;

namespace Wb.Companion.Core.WbAdministration {

    //-----------------------------------------------------------------------------

    public static class SceneList
    {
        public static string Main = "00_Main";
        public static string Map = "01_Map";
    }

    //-----------------------------------------------------------------------------

    public class SceneManager : MonoBehaviour {

        public UIManager uiManager;

        //-----------------------------------------------------------------------------
        // MonoBehaviour
        //-----------------------------------------------------------------------------

        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }

        //-----------------------------------------------------------------------------

        public void loadScene(string scene) {
            // TODO
            // start loading bar (rotating circle for e.g.)
            this.uiManager.showLoadingBar();
            StartCoroutine(LevelLoaded(scene));
        }

        private IEnumerator LevelLoaded(string scene) {
            yield return Application.LoadLevelAdditiveAsync(scene); ;
            Debug.Log("Level: " + scene + " was loaded");
            // TODO
            // stop loading bar (rotating circle for e.g.)
            this.uiManager.hideLoadingBar();
        }
    }

}