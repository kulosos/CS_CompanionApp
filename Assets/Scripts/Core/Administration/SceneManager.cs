using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbNetwork;
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
            Application.LoadLevelAdditive(scene);
        }
    }

}