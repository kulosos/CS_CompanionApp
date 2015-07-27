using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Wb.Companion.Core.WbNetwork;

public class DebugUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  {
	
	// --------------------------------------------------------------------

    public void OnPointerDown(PointerEventData eventData){
        NetworkManager.getInstance().holdFire("accerlerate", true);
    }

    public void OnPointerUp(PointerEventData eventData) {
        NetworkManager.getInstance().holdFire("accerlerate", false);
    }

}
