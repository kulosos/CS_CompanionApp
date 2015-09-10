/**
* @brief		InputManager (Inputs from Companion App for ConSim PC Version)
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			September 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Wb.Companion.Core;
using Wb.Companion.Core.WbNetwork;
using UnityEngine.UI;
using Wb.Companion.Core.WbCamera;
using  Wb.Companion.Core.WbAdministration;

namespace Wb.Companion.Core.Inputs {

    //-------------------------------------------------------------------------

    public class InputManager : MonoBehaviour {

        public enum TiltAxis {
            forward = 0,
            sideways = 1
        }

		public static InputManager instance;

        // TiltInput
        public float forwardCenterAngleOffset = 0f;
        public float sidewaysCenterAngleOffset = 0f;
        public float forwardFullTiltAngle = 42f;
        public float sidewaysFullTiltAngle = 42f;
		public float tiltSteeringDamping = 1.0f;

		public bool isActiveTiltInput = false;

		private float currentTiltValue = 0.0f;
		public Text labelSliderTiltValue;
		public Text labelSliderTiltDamping;

		//---------------------------------------------------------------------
        // Mono Behaviour
        //---------------------------------------------------------------------

		public void Awake() {
			InputManager.instance = this;
		}

		public static InputManager getInstance(){
			return InputManager.instance;
		}

        //---------------------------------------------------------------------

        public void Start() {
        }

        //---------------------------------------------------------------------

        void Update() {

            // Tilt Input
            if (NetworkManager.getInstance().isActiveConnection && InputManager.getInstance().isActiveTiltInput) {
                WbCompRPCWrapper.getInstance().setTiltInput(this.getSmoothAxisValues());
            }

        }

        // --------------------------------------------------------------------
        // Tilt Input
        // --------------------------------------------------------------------

        private float ForwardTiltAngle() {
            return Mathf.Atan2(Input.acceleration.z, -Input.acceleration.y) * Mathf.Rad2Deg + this.forwardCenterAngleOffset;
        }

        //-----------------------------------------------------------------------------
        /// <summary>Tilt in angle around y-Axis</summary>
        private float SideWaysTiltAngle() {
            return Mathf.Atan2(Input.acceleration.x, -Input.acceleration.y) * Mathf.Rad2Deg + this.sidewaysCenterAngleOffset;
        }

        //-----------------------------------------------------------------------------

        public float CalcAxisValue(TiltAxis axis) {

        #if UNITY_EDITOR
            // Axes return strange default values in Editor
            if (Input.acceleration == Vector3.zero) { return 0f; }
        #endif

            float angle = axis == TiltAxis.forward ? this.ForwardTiltAngle() : this.SideWaysTiltAngle();
            float fullTiltAngle = axis == TiltAxis.forward ? this.forwardFullTiltAngle : this.sidewaysFullTiltAngle;
            return Mathf.InverseLerp(-fullTiltAngle, fullTiltAngle, angle) * 2 - 1;
        }

		//-----------------------------------------------------------------------------

        public void toggleActiveTiltInput(string scene) {

            // e.g. for RemoteControlDriving Scene
            if (scene.Equals(SceneList.RemoteControlDriving)) {
                InputManager.getInstance().isActiveTiltInput = true;
            } else {
                InputManager.getInstance().isActiveTiltInput = false;
            }

        }

        //-----------------------------------------------------------------------------

		public float getSmoothAxisValues(){

			float targetTiltValue = InputManager.getInstance().CalcAxisValue(InputManager.TiltAxis.sideways);
			float targetValue = Mathf.Lerp(this.currentTiltValue, targetTiltValue, Time.deltaTime * tiltSteeringDamping);
			this.currentTiltValue = targetTiltValue;
			return targetValue;
		}

		// ----------------------------------------------------------------------------

		public void toggleTiltInput(){
			if(InputManager.getInstance().isActiveTiltInput){
				InputManager.getInstance().isActiveTiltInput = false;
			}else{
				InputManager.getInstance().isActiveTiltInput = true;
			}
		}

		// ----------------------------------------------------------------------------

		public void setMaxTiltAngle(float tiltAngle){
			sidewaysFullTiltAngle = tiltAngle;

			labelSliderTiltValue.text = tiltAngle.ToString();
		}

		public float getMaxTiltAngleSideways(){
			return sidewaysFullTiltAngle;
		}

		// ----------------------------------------------------------------------------

		public void setTiltSteeringDamping(float damping){
			tiltSteeringDamping = damping;

			labelSliderTiltDamping.text = damping.ToString();
		}
		
		public float getTiltSteeringDamping(){
			return tiltSteeringDamping;
		}
	}
}
