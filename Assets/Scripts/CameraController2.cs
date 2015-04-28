using UnityEngine;
//using UnityEditor;
using System.Collections;

public class CameraController2 : MonoBehaviour{

	public Transform target;
	public float currentDistance = 5.0f;
	public float targetDistance;
	public float currentX = 0.0f;
	public float targetX;
	public float currentY = 15.0f;
	public float targetY;
	
	private bool initialized = false;
	
	public float xSpeed = 1.0f;
	public float ySpeed = 1.0f;
	public float zoomSpeed = 1.0f;

	public float dampSpeed = 1.0f;

	//private GUIManager guiManager;
	
	// ------------------------------------------------------------------------
	
	public void init(){
		//guiManager = GameObject.Find("Admin").GetComponent<GUIManager>();
		this.targetX = this.currentX;
		this.targetY = this.currentY;
		this.targetDistance = this.currentDistance;
		this.initialized = true;
		//this.setInitCameraPosition();
	}
	
	//-------------------------------------------------------------------------
	/*
	public void setInitCameraPosition(){

		Vector3 position = new Vector3 (this.initPostionX, this.initPostionY, this.initPostionZ);
		//Vector3 rotation = new Vector3 (this.initRotationX, this.initRotationY, this.initRotationZ);

		this.transform.Translate (position);
		//this.transform.eulerAngles (rotation);

	}
	*/
	// ------------------------------------------------------------------------

	public void Update(){
		
		if(!initialized){
			return;
		}

		if (Input.mousePosition.y > Screen.height/2 && Input.GetMouseButton (0)) {
			
				this.targetX += Input.GetAxis ("Mouse X") * this.xSpeed;
				this.targetY -= Input.GetAxis ("Mouse Y") * this.ySpeed;
				this.targetY = Mathf.Clamp (targetY, 2.0f, 85f);
			}
		
			this.targetDistance -= this.zoomSpeed * Input.GetAxis ("Mouse ScrollWheel");
			this.targetDistance = Mathf.Clamp (targetDistance, 5.0f, 20.0f);
	
			this.currentDistance = Mathf.Lerp (this.currentDistance, this.targetDistance, Time.deltaTime * this.dampSpeed);
			this.currentX = Mathf.Lerp (this.currentX, this.targetX, Time.deltaTime * this.dampSpeed);
			this.currentY = Mathf.Lerp (this.currentY, this.targetY, Time.deltaTime * this.dampSpeed);
	
			this.updateCameraTransform ();

	}
	
	// ------------------------------------------------------------------------
	
	private void updateCameraTransform(){
	
		Quaternion rotation = Quaternion.Euler(this.currentY, this.currentX, 0);
		Vector3 position = rotation * new Vector3(0.0f, 0.0f, -this.currentDistance) + this.target.position;
		this.transform.rotation = rotation;
		this.transform.position = position;
	}
}
