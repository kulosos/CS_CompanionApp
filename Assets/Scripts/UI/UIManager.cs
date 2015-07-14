/*
 * brief	
 * author	Benedikt Niesen (benedikt@weltenbauer-se.com)
 * company	weltenbauer. Software Entwicklung GmbH
 * date		March 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

#if UNITY_IOS
using Coherent.UI.Mobile.Binding; // TriggerEvent binding with extra parameter on mobile 
#endif

#if UNITY_ANDROID
using Coherent.UI.Mobile.Binding; // TriggerEvent binding with extra parameter on mobile 
#endif

#if UNITY_EDITOR
using Coherent.UI.Binding; // TriggerEvent binding with extra parameter on desktop 
#endif

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.UI {

	public class UIManager : MonoBehaviour {

		public CoherentUIView coherentUiView;
		public UIWrapper uiWrapper;
		public bool debugging = false;

		//-----------------------------------------------------------------------------
		// MonoBehaviour
		//-----------------------------------------------------------------------------

		public void Start() {
			this.coherentUiView.ReceivesInput = true;
		}

		//-----------------------------------------------------------------------------
		// Bind Methods to UI 
		//-----------------------------------------------------------------------------

		public void switchGameUI(){
			this.coherentUiView.View.TriggerEvent("switchGameUI");
		}

		public void setConnectionErrorMsg(string obj){
			this.coherentUiView.View.TriggerEvent("setConnectionErrorMsg", obj);
		}

        //-----------------------------------------------------------------------------


	}
}