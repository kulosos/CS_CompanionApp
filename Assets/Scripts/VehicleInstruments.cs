using UnityEngine;
using System.Collections;
using Wb.Companion.Core.WbNetwork;

namespace Wb.Companion.InGame {

	
public class VehicleInstruments : MonoBehaviour {

		public enum vehicleInstrumentType { 
			Speed = 0, 
			RPM = 1, 
			Fuel = 2, 
			Temperature = 3, 
			BrakePressure = 4
		};

		//---------------------------------------------------------------------

		public Transform gameObject;
		public vehicleInstrumentType instrumentType;
		public bool isActive = true;
		public float maxMajorInstrumentRotation = 220f;
		public float maxMinorInstrumentRotation = 90f;

		void Awake() {
			this.gameObject = GetComponent<VehicleInstruments>().transform;
		}

		//---------------------------------------------------------------------
		// Mono Behaviour
		//---------------------------------------------------------------------

		void Start () {
			this.setInstrumentOnce();
		}

		void Update () {

			if(NetworkManager.getInstance().isActiveConnection && this.isActive){
				this.setInstrumentsOnUpdate();
			}
		}

		//---------------------------------------------------------------------

		public void setInstrumentsOnUpdate(){

			float rpm = WbCompRPCWrapper.getInstance().getCurrentRPM();
			float speed = WbCompRPCWrapper.getInstance().getCurrentSpeed();
			rpm = 2500f;
			speed = 88f;

			if(instrumentType == vehicleInstrumentType.RPM){
				float rpmAngle = Mathf.Lerp(0, maxMajorInstrumentRotation, Mathf.InverseLerp(0, 3500, rpm));
				Quaternion rotation = Quaternion.Euler(0f, 0f, rpmAngle);
				gameObject.transform.localRotation = rotation;
			}
			if(instrumentType == vehicleInstrumentType.Speed){
				float speedAngle = Mathf.Lerp(0, maxMajorInstrumentRotation, Mathf.InverseLerp(0, 100, speed));
				Quaternion rotation = Quaternion.Euler(0f, 0f, speedAngle);
				gameObject.transform.localRotation = rotation;
			}
		}

		//---------------------------------------------------------------------

		public void setInstrumentOnce(){
			
			if(instrumentType == vehicleInstrumentType.Fuel){
				Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(30f, 80f));
				gameObject.transform.localRotation = rotation;
			}
			
			if(instrumentType == vehicleInstrumentType.BrakePressure){
				Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(50f, 75f));
				gameObject.transform.localRotation = rotation;
			}
			
			if(instrumentType == vehicleInstrumentType.Temperature){
				Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(40f, 50f));
				gameObject.transform.localRotation = rotation;
			}
		}


	}

}
