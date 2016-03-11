using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class CarScriptSharp : MonoBehaviour {
	public WheelCollider rearRightTire;
	public WheelCollider rearLeftTire;
	public WheelCollider frontRightTire;
	public WheelCollider frontLeftTire;

	public Text scoreText;
	public int scoreCounter = 0000;

	public Text driftText;

	double playerXLast;

	public double timeSaver;

	public Rigidbody playerBody;
	public Vector3 center;
	
	private string moveForward = "w"; //The strings for the controls
	private string moveLeft    = "a";
	private string moveRight   = "d";
	private string moveBack    = "s";
	
	private bool forward = false; //Booleans for controls
	private bool left    = false;
	private bool right   = false;
	private bool back    = false;
	
	void Start () {
		playerBody = GetComponent<Rigidbody>();
		playerBody.centerOfMass = center;
		Physics.gravity = new Vector3(0, -100.0F, 0);
		setCountText();
		driftText.enabled = false;
	}
	////END Start()////
	
	void FixedUpdate () {
		forward = Input.GetKey(moveForward); //Refreash the booleans based on their corresponding buttons state
		left    = Input.GetKey(moveLeft);
		right   = Input.GetKey(moveRight);
		back    = Input.GetKey(moveBack);

		if (forward && !back) {
			rearRightTire.motorTorque = 90000;
			rearLeftTire.motorTorque = 90000;
		} else if (back && !forward) {
			rearRightTire.motorTorque = -9000;
			rearLeftTire.motorTorque = -9000;
		} else {
			rearRightTire.motorTorque = 0;
			rearLeftTire.motorTorque = 0;
		}

		if (left && !right) { //If the left key is pressed
			frontLeftTire.steerAngle = -50;
			frontRightTire.steerAngle = -50;
			rearRightTire.motorTorque = 90000;
			rearLeftTire.motorTorque = -90000;
			scoreCounter += 1;
			driftText.enabled = true;
			smoothIncrease();
		} else if (right && !left) { //If the right key is pressed
			frontRightTire.steerAngle = 50;
			frontLeftTire.steerAngle = 50;
			rearRightTire.motorTorque = -90000;
			rearLeftTire.motorTorque = 90000;
			scoreCounter += 1;
			driftText.enabled = true;
			smoothIncrease();
		} else {
			frontLeftTire.steerAngle = 0;
			frontRightTire.steerAngle = 0;
			driftText.enabled = false;
		}

		if (Input.GetKey("space")) {
			rearLeftTire.brakeTorque = 200;
			rearRightTire.brakeTorque = 200;
			frontLeftTire.brakeTorque = 200;
			frontRightTire.brakeTorque = 200;
		} else {
			frontLeftTire.brakeTorque = 0;
			frontRightTire.brakeTorque = 0;
			rearLeftTire.brakeTorque = 0;
			rearRightTire.brakeTorque = 0;
		}
		setCountText();
	}
	////END FixedUpdate()////

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("GoodCollectable"))
		{
			other.gameObject.SetActive (false);
			scoreCounter += 1000;
		}

		if (other.gameObject.CompareTag ("Boost")) {
			rearLeftTire.motorTorque = rearLeftTire.motorTorque + 80000;
			rearRightTire.motorTorque = rearRightTire.motorTorque + 80000;
		}
	}

	void setCountText() {
		scoreText.text = "SCORE: " + scoreCounter.ToString();
	}

	void smoothIncrease() {
		bool shouldRun = (Time.fixedDeltaTime >= 1) ? true : false;
		if (shouldRun) {
			++scoreCounter;
		}
	}
}
////END PlayerController Class////