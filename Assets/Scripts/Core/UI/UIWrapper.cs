/**
* @brief		Central UI Wrapper for Coherent UI Inputs
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			April 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Wb.Companion.Core.WbNetwork;
using Wb.Companion.Core.WbAdministration;
using Coherent.UI.Binding;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.UI {
	
	public enum InputBehavior 
	{
		CoherentUI_Only = 1,
		Unity3D_Only = 2,
		CoherentUI_Unity3D = 3,
	}

	public class UIWrapper : MonoBehaviour {

		private CoherentUIView coherentUiView;
		public UIManager uiManager;
        public SceneManager sceneManager;

		//private InputBehavior inputBehavior = InputBehavior.CoherentUI_Unity3D;
		
		//-----------------------------------------------------------------------------

		public void Start() {

			this.coherentUiView = this.GetComponent<CoherentUIView>();
			this.coherentUiView.Listener.ReadyForBindings += this.BindMethods;

			this.coherentUiView.IsTransparent = true;

		}

		//-----------------------------------------------------------------------------

		public void BindMethods(int frameId, string path, bool isMainFrame) {

			Debug.Log ("init bind methods");

            /*
			//Example of binding JavaScript to UNITY with RETURN
			//From Unity to JavaScript
			this.coherentUiView.View.BindCall("switchGameUI", new Action<string>(this.uiManager.ExampleTriggerToUI));

			this.coherentUiView.View.BindCall("functionName", new Func<string, string>(this.LogWithReturn));

			//Example of binding JavaScript to UNITY with RETURN
			this.coherentUiView.View.BindCall("getConnectionParameter", new Func<string, string>(this.LogWithReturn));

			//Example of binding JavaScript to UNITY with Return Parameter
					
			this.coherentUiView.View.RegisterForEvent("debugInfo", new Action<string>( this.printDebugInfo ));
            */

            //From JavaScript to Unity
            this.coherentUiView.View.RegisterForEvent("connect", new Action<string, string, string>(NetworkManager.connect));
            this.coherentUiView.View.RegisterForEvent("disconnect", new Action(NetworkManager.disconnect));
            this.coherentUiView.View.RegisterForEvent("loadScene", new Action<string>(this.sceneManager.loadScene));
		}

		//-----------------------------------------------------------------------------
	
		//[Coherent.UI.CoherentMethod("functionName")]
		private void printDebugInfo(string di){
			Debug.Log (di);
		}

		private void Log(string s) {
			Debug.Log("From Coherent : " + s);
		}

		private string LogWithReturn(string s) {
			Debug.Log("debug log with return: " + s);
			return s;
		}

	}
}
