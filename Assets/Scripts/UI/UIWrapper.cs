/*
 * brief	
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Wb.Core.Inputs;
using Coherent.UI.Binding;

//-----------------------------------------------------------------------------

namespace Wb.Core.UI {

    public class UIWrapper : MonoBehaviour {

        private CoherentUIView coherentUiView;
		public UIManager uiManager;
        
        //-----------------------------------------------------------------------------

        public void Start() {

            this.coherentUiView = this.GetComponent<CoherentUIView>();
            this.coherentUiView.Listener.ReadyForBindings += this.BindMethods;
        }

        //-----------------------------------------------------------------------------

        public void BindMethods(int frameId, string path, bool isMainFrame) {

            // Example of binding JavaScript to UNITY with RETURN
            this.coherentUiView.View.BindCall("functionName", new Func<string, string>(this.LogWithReturn));

            // Example of binding JavaScript to UNITY with Return Parameter
            this.coherentUiView.View.RegisterForEvent("functionName", new Action<string>(this.Log));

			//this.uiManager.ExampleTriggerToUI("huhu");
        }

		//-----------------------------------------------------------------------------

		//[Coherent.UI.CoherentMethod("functionName")]
		public void functionName(){
		
			Debug.Log (" t e s t - t e s t - t e s t ");
		}
	


        //-------------------------------------------------------------------------
        // Protected Methods
        //-------------------------------------------------------------------------

        private void Log(string s) {
            Debug.Log("From Coherent : " + s);
        }

        private string LogWithReturn(string s) {
			Debug.Log("debug log with return: " + s);
            return s;
        }
    }
}
