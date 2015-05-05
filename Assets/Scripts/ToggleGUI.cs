using UnityEngine;
using System.Collections;

public class ToggleGUI : MonoBehaviour {

	private bool isActive = true;
	public GameObject gameObject;
	
	// --------------------------------------------------------------------

	void OnGUI() {

		if (GUI.Button(new Rect(5, 5, 160, 50), "Toggle UI")){
			if(isActive){
				gameObject.SetActive(false);
				this.isActive = false;
			}else{
				gameObject.SetActive(true);
				this.isActive = true;
			}
		}
	}

}
