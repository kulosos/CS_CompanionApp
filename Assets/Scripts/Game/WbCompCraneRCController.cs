using UnityEngine;
using System.Collections.Generic;
using Wb.Companion.Core.UI;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.Game {

	public class WbCompCraneRCController : MonoBehaviour {

		public List<WbCraneRCButton> craneRCBtnInGame = new List<WbCraneRCButton>();
		private List<WbCraneRCButton> craneRCBtnUI = new List<WbCraneRCButton>();
		private UIManager uiManager;

		//-----------------------------------------------------------------------------
		// Mono Behaviour
		//-----------------------------------------------------------------------------

		void Awake () {

			//Register WbCompCraneRCController at UIManager
			this.uiManager = UIManager.FindObjectOfType(typeof(UIManager)) as UIManager;
			this.uiManager.craneRCController = this;

			foreach(UIElement uie in this.uiManager.getUIElements()){

				if(uie.gameObject.GetComponent<WbCraneRCButton>() != null){
					if(uie.gameObject.GetComponent<WbCraneRCButton>().craneBtnType.Equals(WbCraneRCButtonType.UI)){
						this.craneRCBtnUI.Add(uie.GetComponent<WbCraneRCButton>());
					}
				}
			}
		}

		//----------------------------------------------------------------------------

		public void setFunctionBtnPositions() {

			// set positions of Crane RC Function Buttons
			foreach(WbCraneRCButton inGameBtn in this.craneRCBtnInGame){
				foreach(WbCraneRCButton uiBtn in this.craneRCBtnUI){
					if(inGameBtn.functionButton.Equals(uiBtn.functionButton)){
						uiBtn.transform.position = inGameBtn.transform.position;
					}
				}
			}
		}
	
		//----------------------------------------------------------------------------

	}
	
	// ENUM ---------------------------------------------------------------------------
	
	public enum WbCraneRCFunctions { 
		F1,
		F2,
		F3, 
		F4,
		F5,
		F6,
		F7,
		F8,
		F9,
		F10,
		FPower,
		FLeft,
		FRight,
		FSpecial
	}
}
