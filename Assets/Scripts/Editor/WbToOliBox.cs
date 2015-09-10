using UnityEngine;
using System.Collections;
using UnityEditor;

public class WbToOliBox : ScriptableWizard {
	
	public int amount = 10;
	public string namePrefix = "EVOI_Site_";
	
	//-------------------------------------------------------------------------
	
	[MenuItem("WbEditorTools/Olis Toolbox/Create Empty SiteObjects")]
	static void CreateWizard() {
		ScriptableWizard.DisplayWizard<WbToOliBox>("Olis Toolbox", "Create", "Apply");
	}
	
	//-------------------------------------------------------------------------
	
	void OnWizardCreate() {
	}
	
	//-------------------------------------------------------------------------
	
	void OnWizardUpdate() {
	}
	
	//-------------------------------------------------------------------------
	
	void OnWizardOtherButton() {
		
		if(Selection.activeTransform != null){
			for (int i = 1; i <= this.amount; i++) {
				if (i < 100) {
					
				}
				GameObject go = new GameObject(namePrefix + i);
				go.transform.parent = Selection.activeTransform;
			}
		} else {
			Debug.LogError("No parent selected.");
		}
		
	}
}