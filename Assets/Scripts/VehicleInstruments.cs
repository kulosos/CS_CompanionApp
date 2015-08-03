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
		[SerializeField]
		private vehicleInstrumentType instrumentType;
		public bool isActive = true;

		void Awake() {
			this.gameObject = GetComponent<VehicleInstruments>().transform;
		}

		//---------------------------------------------------------------------
		// Mono Behaviour
		//---------------------------------------------------------------------

		void Start () {
		
		}

		void Update () {

			if(NetworkManager.getInstance().isActiveConnection && this.isActive){
				this.setInstruments();
			}
		}

		//---------------------------------------------------------------------

		public void setInstruments(){

			float rpm = WbCompRPCWrapper.getInstance().getCurrentRPM();
			float speed = WbCompRPCWrapper.getInstance().getCurrentSpeed();
			rpm = 2500f;
			speed = 88f;

			if(instrumentType == vehicleInstrumentType.RPM){
				float rpmAngle = Mathf.Lerp(0, 220, Mathf.InverseLerp(0, 3500, rpm));
				Quaternion rotation = Quaternion.Euler(0f, 0f, rpmAngle);
				gameObject.transform.localRotation = rotation;
			}
			if(instrumentType == vehicleInstrumentType.Speed){
				float speedAngle = Mathf.Lerp(0, 220, Mathf.InverseLerp(0, 100, speed));
				Quaternion rotation = Quaternion.Euler(0f, 0f, speedAngle);
				gameObject.transform.localRotation = rotation;
			}
		}

		//---SETTER / GETTER --------------------------------------------------

		public vehicleInstrumentType getVehicleInstrumentType(){
			return instrumentType;
		}
		/*
		public int getVehicleInstrumentTypeInt(){

			if(this.instrumentType == vehicleInstrumentType.Speed) return 0;
			if(this.instrumentType == vehicleInstrumentType.RPM) return 1;

			return -1;
		}*/

		public void setVehicleInstrumentType(vehicleInstrumentType type){
			this.instrumentType = type;
		}
		/*
		public void setVehicleInstrumentTypeInt(int type){
			if(type == 0) this.instrumentType = vehicleInstrumentType.Speed;
			if(type == 1) this.instrumentType = vehicleInstrumentType.RPM;
		}*/
	}

}
