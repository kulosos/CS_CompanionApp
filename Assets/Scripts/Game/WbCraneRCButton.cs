using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Wb.Companion.Core.WbNetwork;
using UnityEngine.UI;
using Wb.Companion.Core.Inputs;
using Wb.Companion.Core.UI;
using Wb.Companion.Core.WbAdministration;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.Game {

	public enum WbCraneRCButtonType { UI, InGame };

    public class WbCraneRCButton : MonoBehaviour {

		public WbCraneRCFunctions functionButton;
		public WbCraneRCButtonType craneBtnType = WbCraneRCButtonType.UI;

        //-----------------------------------------------------------------------------
        // Mono Behaviour
        //-----------------------------------------------------------------------------

        void Start() {
        }

    }



}
