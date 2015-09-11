/**
* @brief		BackView Camera
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			September 2015
*/
//-----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Wb.Companion.Core.WbNetwork;
using System;

//-----------------------------------------------------------------------------

namespace Wb.Companion.Core.Game {

	public class BackViewCamera : MonoBehaviour {

		private float timeSinceLastStart = 0;

		//---------------------------------------------------------------------
		// Mono Behaviour
		//---------------------------------------------------------------------


		void Start () {
		
		}

		void Update () {
		
            // send render texture data frame rate independent
			if(NetworkManager.getInstance().isActiveConnection){
				if(timeSinceLastStart >= 1f/NetworkManager.getInstance().globalRPCSendRate){

					this.setBackViewCamera();
					this.timeSinceLastStart = 0;
				}
				this.timeSinceLastStart += Time.deltaTime;
			}
		}

		//---------------------------------------------------------------------

		// convert b64 string to byte[]
		// decode byte[] to PNG texture
		// assign PNG texture to material
		private void setBackViewCamera(){

			string texBase64 = WbCompRPCWrapper.getInstance().getBackViewCameraFrameAsB64String();

			try{
				byte[] texBin = Convert.FromBase64String(texBase64);
				Texture2D tex = new Texture2D(200,200,TextureFormat.RGB24, false);
				tex.LoadImage(texBin);
				this.gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", tex);

			}catch(Exception e){
				print(e);
			}
		
		}
	}
}
