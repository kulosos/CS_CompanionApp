/**
* @brief		
* @author		Oliver Kulas (oli@weltenbauer-se.com)
* @date			Sep 2015
*/

//-----------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public class WbCompBillboard : MonoBehaviour {

    public Camera mainCam;

    //---------------------------------------------------------------------
    // MonoBehaviour
    //---------------------------------------------------------------------

    void Start() {
        this.mainCam = Camera.main;
    }

    void Update() {
        //transform.LookAt(transform.position + this.mainCam.transform.rotation * Vector3.back, this.mainCam.transform.rotation * Vector3.up);
    }
    void LateUpdate() {

        transform.LookAt(Camera.main.transform, Camera.main.transform.rotation * Vector3.up);
    }
}

