using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Wb.Companion.Core.WbNetwork;

public class DebugUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  {

	//private bool isActive = true;
    bool pressed = false;
    int i = 0;

	
	// --------------------------------------------------------------------

    void Update() {
        if (pressed) {
            i += 1;
           // Debug.Log(i);
			NetworkManager.getInstance().holdFire("fire " + i.ToString());
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        pressed = false;
    }

}
