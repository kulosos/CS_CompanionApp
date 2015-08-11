using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Wb.Companion.Core;
using Wb.Companion.Core.WbNetwork;
using UnityEngine.UI;
using Wb.Companion.Core.Inputs;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.Game {

    public enum meshThumbstickType {
        Left = 0,
        Middle = 1,
        Right = 2
    };

    public class Wb3DThumbstick : MonoBehaviour {

        private List<Wb3DThumbstick> meshThumbsticks = new List<Wb3DThumbstick>();

        //-----------------------------------------------------------------------------
        // Mono Behaviour
        //-----------------------------------------------------------------------------

        // Use this for initialization
        void Start() {
            this.init3DThumbsticks();

        }

        // Update is called once per frame
        void Update() {

        }

        //-----------------------------------------------------------------------------

        private void init3DThumbsticks() {

            // get mesh thumbsticks in scene
            Wb3DThumbstick[] meshSticks = Wb3DThumbstick.FindObjectsOfType(typeof(Wb3DThumbstick)) as Wb3DThumbstick[];
            foreach (Wb3DThumbstick meshStick in meshSticks) {
                this.meshThumbsticks.Add(meshStick);
            }

            // assign mesh thumbstick to script
            //foreach (Wb3DThumbstick meshStick in this.meshThumbsticks) {
            //    foreach()
            //}

        }
    }

}
