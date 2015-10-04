using UnityEngine;
using System.Collections;


namespace Wb.Companion.Core.UI 
{   
	// ENUM TYPE ------------------------------------------------------------------
	public enum WbCompRetailSubMenu { Bricks, ConstructionMaterial, PipesCables, SteelElements, Equipment }
	//-----------------------------------------------------------------------------

	public class UIRetailSubMenu : MonoBehaviour {

		public WbLocationName location;
		public WbCompRetailSubMenu retailSubMenu;

		//-------------------------------------------------------------------------
		// MonoBehaviour
		//-------------------------------------------------------------------------

		void Start () {
		}

		void Update () {
		}

		//-------------------------------------------------------------------------


	}

}
