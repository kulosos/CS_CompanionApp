using UnityEngine;
using UnityEditor;
using System.Collections;
using Wb.Companion.InGame;
using System;

namespace Wb.Companion.WbEditor {

	[CustomEditor(typeof(VehicleInstruments))]
    public class VehicleInstrumentsEdtiorGUI : Editor {

		private VehicleInstruments editorTarget;
		private string[] vehicleInstrumentType;
		private int selectedItem;

        //---------------------------------------------------------------------

        public void Awake() {
			this.editorTarget = ((VehicleInstruments)target);
			this.vehicleInstrumentType = new string[] { VehicleInstruments.vehicleInstrumentType.Speed.ToString(), 
														VehicleInstruments.vehicleInstrumentType.RPM.ToString(), 
														VehicleInstruments.vehicleInstrumentType.Fuel.ToString(),
														VehicleInstruments.vehicleInstrumentType.Temperature.ToString(),
														VehicleInstruments.vehicleInstrumentType.BrakePressure.ToString()  };
			this.selectedItem = (int)this.editorTarget.instrumentType;
        }

        //---------------------------------------------------------------------
        
        public override void OnInspectorGUI() {

            DrawDefaultInspector();
			/*
            EditorGUI.BeginChangeCheck();	

			this.selectedItem = EditorGUILayout.Popup("Vehicle Instrument Type", this.selectedItem, this.vehicleInstrumentType);
            if (EditorGUI.EndChangeCheck()) {	
               this.editorTarget.instrumentType = ((VehicleInstruments.vehicleInstrumentType)this.selectedItem);
            }
			*/
        }



    }
}
