/*
 * brief	
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using Coherent.UI.Binding;
using System.Collections.Generic;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.UI {

    //-----------------------------------------------------------------------------

    public class UIManager : MonoBehaviour {

        public CoherentUIView coherentUiView;
        public UIWrapper uiWrapper;

        //-----------------------------------------------------------------------------
        // MonoBehaviour
        //-----------------------------------------------------------------------------

        public void Start() {
            this.coherentUiView.ReceivesInput = true;
        }

        //-----------------------------------------------------------------------------

        public void ExampleTriggerToUI(string obj) {
            //this.coherentUiView.View.TriggerEvent("dataBindingEvent", obj);
        }
    }
}