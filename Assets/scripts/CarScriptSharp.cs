using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
using System;

public class CarScriptSharp : MonoBehaviour {
	public WheelCollider rearRightTire;
	private int rearRightTorque = 0;
	public WheelCollider rearLeftTire;
	private int rearLeftTorque = 0;
	public WheelCollider frontRightTire;
	private int frontRightTorque = 0;
	public WheelCollider frontLeftTire;
	private int frontLeftTorque = 0;

	public Camera playerCamera;

	public int forwardTorque = 9000;
	public int reverseTorque = -9000;
	public int turnTorque = 9000;
	public int turnAngle = 50;
	public int brakeTorque = 20000;
	private int startBrakeTorque;

	private bool boostObject;
	private bool boostRamp;
	private int boostedTimes;
	public int boostCycles = 15;

	private bool braking = false;

	public Text scoreText;
	public int scoreCounter = 0000;

	public Text driftText;

	public double timeSaver;

	public Rigidbody playerBody;
	public Vector3 center;
	
	private string moveForward = "w"; //The strings for the controls
	private string moveLeft    = "a";
	private string moveBack    = "s";
	private string moveRight   = "d";
	
	private bool forward = false; //Booleans for controls
	private bool left    = false;
	private bool right   = false;
	private bool back    = false;

	private Vector3 playerReset = new Vector3 (50, 50, 50);

	
	void Start () {
		playerBody = GetComponent<Rigidbody>();
		playerBody.centerOfMass = center;
		Physics.gravity = new Vector3(0, -100.0F, 0);
		setCountText();
		driftText.enabled = false;
	}
	//END Start()//
	
	void FixedUpdate () {
		forward = Input.GetKey(moveForward); //Refreash the booleans based on their corresponding buttons state
		left    = Input.GetKey(moveLeft);
		right   = Input.GetKey(moveRight);
		back    = Input.GetKey(moveBack);

		resetTorqueAndAngles();

		if (forward && !back) {
			rearRightTorque = forwardTorque;
			rearLeftTorque = forwardTorque;
			braking = false;
		} else if (back && !forward) {
			rearRightTorque = reverseTorque;
			rearLeftTorque = reverseTorque;
			braking = false;
		}

		if (left && !right) { //If the left key is pressed
			frontLeftTire.steerAngle = -turnAngle;
			frontRightTire.steerAngle = -turnAngle;
			//rearRightTorque = turnTorque;
			//rearLeftTorque = -turnTorque;
			driftText.enabled = true;
			smoothIncrease();
			braking = false;
		} else if (right && !left) { //If the right key is pressed
			frontRightTire.steerAngle = turnAngle;
			frontLeftTire.steerAngle = turnAngle;
			//rearRightTorque = -turnTorque;
			//rearLeftTorque = turnTorque;
			driftText.enabled = true;
			smoothIncrease();
			braking = false;
		} else {
			driftText.enabled = false;
		}

		if (Input.GetKey("space")) {
			braking = true;
		} else if (!left && !right && !forward && !back) {
			braking = true;
		}

		if ((boostObject || boostRamp) && (boostedTimes <= boostCycles)) {
			if (boostRamp) {
				rearLeftTorque = rearLeftTorque + 7000;
				rearRightTorque= rearRightTorque + 7000;
				frontLeftTorque = rearLeftTorque + 7000;
				frontRightTorque = rearRightTorque + 7000;
			}
			if (boostObject) {
				rearLeftTorque = rearLeftTorque + 2000;
				rearRightTorque= rearRightTorque + 2000;
				frontLeftTorque = rearLeftTorque + 2000;
				frontRightTorque = rearRightTorque + 2000;
			}
			++boostedTimes;
		} else if (boostedTimes > boostCycles) {
			boostedTimes = 0;
			boostObject = false;
		}

		setCountText();
		setTorques();

		if (Input.GetKey("1")) {
			playerBody.MovePosition(playerReset);
		}
		if (Input.GetKey("e")) {
			playerCamera.transform.Rotate(0, -1, 0);
		}
		if (Input.GetKey("q")) {
			playerCamera.transform.Rotate(0, 1, 0);
		}
	}
	//END FixedUpdate()//

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("GoodCollectable"))
		{
			other.gameObject.SetActive (false);
			scoreCounter += 1000;
			boostObject = true;
			boostedTimes = 0;
		} else if (other.gameObject.CompareTag ("BadCollectable"))
		{
			other.gameObject.SetActive (false);
			scoreCounter -= 500;
			boostObject = false;
			boostRamp = false;
		}

		if (other.gameObject.CompareTag ("Boost")) {
			boostRamp = true;
			boostedTimes = 0;
		}
	}
	//END OnTriggerEnter()//

	void setCountText() {
		scoreText.text = "SCORE: " + scoreCounter.ToString();
	}
	//END setCountText()//

	void smoothIncrease() {
		timeSaver += Time.fixedDeltaTime;

		if (timeSaver >= 1) {
			scoreCounter += 10;
			timeSaver = 0;
		}
	}
	//END smoothIncrease()//

	void resetTorqueAndAngles() {
		rearRightTorque = 0;
		rearLeftTorque = 0;
		frontLeftTorque = 0;
		frontRightTorque = 0;
		frontLeftTire.steerAngle = 0;
		frontRightTire.steerAngle = 0;
		brakeTorque = startBrakeTorque;
	}
	//END resetTorqueAndAngles//

	void setTorques() {
		if (!braking) {
			rearLeftTire.motorTorque = rearLeftTorque;
			rearRightTire.motorTorque = rearRightTorque;
			frontLeftTire.motorTorque = frontLeftTorque;
			frontRightTire.motorTorque = frontRightTorque;
		} else {
			rearLeftTire.brakeTorque = brakeTorque;
			rearRightTire.brakeTorque = brakeTorque;
			frontLeftTire.brakeTorque = brakeTorque;
			frontRightTire.brakeTorque = brakeTorque;
		}
	}
	//END setTorques//
}
//END PlayerController Class//