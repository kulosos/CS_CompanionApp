/*
 * @brief	Constant Summary of Input Keys
 * @author	Oliver Kulas (oli@weltenbauer-se.com)
 * @date	August 2015
 */

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace Wb.Companion.Core.Inputs {

	public static class InputKeys {

		// DRIVING 
		public const string DRIVING_ACCELERATE 	= "accelerate";
		public const string DRIVING_BRAKE 		= "brake";
		public const string DRIVING_STEER_LEFT 	= "steerLeft";
		public const string DRIVING_STEER_RIGHT	= "steerRight";

		// TRUCK CRANE
		public const string TRUCKCRANE_BOOM_01_UP 			= "boom01-up";
		public const string TRUCKCRANE_BOOM_01_DOWN 		= "boom01-down";
		public const string TRUCKCRANE_BOOM_MAIN_LEFT 		= "boomMainLeft";
		public const string TRUCKCRANE_BOOM_MAIN_RIGHT 		= "boomMainRight";
		public const string TRUCKCRANE_BOOM_FORWARD 		= "boom02-03-04-05-06-forward";
		public const string TRUCKCRANE_BOOM_BACKWARD 		= "boom02-03-04-05-06-backward";
		public const string TRUCKCRANE_CABIN_UP 			= "tcCabinUp";
		public const string TRUCKCRANE_CABIN_DOWN 			= "tcCabinDown";
		public const string TRUCKCRANE_ROPE_UP 				= "wbtruckCrane-rope-up";
		public const string TRUCKCRANE_ROPE_DOWN 			= "wbtruckCrane-rope-down";
		public const string TRUCKCRANE_SUPPORTLEGS_IN 		= "wbTruckCrane-supportlegs-in";
		public const string TRUCKCRANE_SUPPORTLEGS_OUT 		= "wbTruckCrane-supportlegs-out";
		public const string TRUCKCRANE_ROTATE_HOOK_LEFT		= "trcTurnHookLeft";
		public const string TRUCKCRANE_ROTATE_HOOK_RIGHT	= "trcTurnHookRight";
		public const string TRUCKCRANE_CONNECT_HOOK_CARGO 	= "trcConnectHook";

		// -----------------------------------------------------------------

		static InputKeys(){

		}

	}
}
