/* 
 * @brief VehicleInstruments
 * @autor Oliver Kulas (oli@weltenbauer-se.com)
 * @date Aug 2015
 */

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
		public float maxRotation = 220f;
		public float dampingFactor = 1.0f;
		public bool isActive = true;
		public bool setOnlyOnce = false;


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
				this.setInstrumentsOnUpdate();


			}
		}

		//---------------------------------------------------------------------

		public void setInstrumentsOnUpdate(){

			float rpm = WbCompRPCWrapper.getInstance().getCurrentRPM();
			float speed = WbCompRPCWrapper.getInstance().getCurrentSpeed();

			if(instrumentType == vehicleInstrumentType.RPM){
				Quaternion prevRotation = gameObject.transform.localRotation;
				float rpmAngle = Mathf.Lerp(0, maxRotation, Mathf.InverseLerp(0, 3500, rpm));
				Quaternion targetRotation = Quaternion.Euler(0f, 0f, rpmAngle);
				gameObject.transform.localRotation = Quaternion.Lerp(prevRotation, targetRotation, Time.deltaTime * this.dampingFactor);

			}
			if(instrumentType == vehicleInstrumentType.Speed){
				Quaternion prevRotation = gameObject.transform.localRotation;
				float speedAngle = Mathf.Lerp(0, maxRotation, Mathf.InverseLerp(0, 100, speed));
				Quaternion targetRotation = Quaternion.Euler(0f, 0f, speedAngle);
				gameObject.transform.localRotation = Quaternion.Lerp(prevRotation, targetRotation, Time.deltaTime * this.dampingFactor);
			}

			if(instrumentType == vehicleInstrumentType.Fuel){
				float zValue = 55f;
				Quaternion prevRotation = gameObject.transform.localRotation;
				Quaternion targetRotation = Quaternion.Euler(0f, 0f, zValue);
				gameObject.transform.localRotation = Quaternion.Lerp(prevRotation, targetRotation, Time.deltaTime * this.dampingFactor);
				if(this.setOnlyOnce && prevRotation.z > targetRotation.z-(targetRotation.z*0.1)) this.isActive = false;
			}
			
			if(instrumentType == vehicleInstrumentType.BrakePressure){
				float zValue = 70f;
				Quaternion prevRotation = gameObject.transform.localRotation;
				Quaternion targetRotation = Quaternion.Euler(0f, 0f, zValue);
				gameObject.transform.localRotation = Quaternion.Lerp(prevRotation, targetRotation, Time.deltaTime * this.dampingFactor);
				if(this.setOnlyOnce && prevRotation.z > targetRotation.z-(targetRotation.z*0.1)) this.isActive = false;
			}
			
			if(instrumentType == vehicleInstrumentType.Temperature){
				float zValue = 48f;
				Quaternion prevRotation = gameObject.transform.localRotation;
				Quaternion targetRotation = Quaternion.Euler(0f, 0f, zValue);
				gameObject.transform.localRotation = Quaternion.Lerp(prevRotation, targetRotation, Time.deltaTime * this.dampingFactor);
				if(this.setOnlyOnce && prevRotation.z > targetRotation.z-(targetRotation.z*0.1)) this.isActive = false;
			}
		}

		//---------------------------------------------------------------------

		public void setInstrumentOnce(){
		


		}
	}

}
