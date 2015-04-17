/*
 * brief	Input Management class
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Wb.Core;

//-----------------------------------------------------------------------------

namespace Wb.Core.Inputs {

    //-------------------------------------------------------------------------

    public class InputManager : MonoBehaviour {

        public TouchScript.Gestures.PanGesture panGesture;
        public TouchScript.Gestures.ScaleGesture scaleGesture;
        public TouchScript.Gestures.TapGesture tapGesture;

        //---------------------------------------------------------------------

        public void Start() {

            this.tapGesture.Tapped += touchGestureTappedEvent;
            this.panGesture.Panned += touchGesturePannedEvent;
            this.scaleGesture.Scaled += touchGestureScaledEvent;
        }

        //---------------------------------------------------------------------

        void touchGestureScaledEvent(object sender, System.EventArgs e) {
       
        }

        //---------------------------------------------------------------------

        void touchGesturePannedEvent(object sender, System.EventArgs e) {

        }

        //---------------------------------------------------------------------

        void touchGestureTappedEvent(object sender, System.EventArgs e) {

        }
    }
}
