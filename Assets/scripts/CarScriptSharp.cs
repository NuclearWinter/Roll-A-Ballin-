using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;
using System;

//TODO lerp player speed between what it should be

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

	public int deltaSpeed = 20;

	public int forwardTorque = 9000;
	public int reverseTorque = -9000;
	public int turnTorque = 9000;
	public int turnAngle = 50;
	public int brakeTorque = 20000;

	private bool boostObject = false;
	private bool endObject = false;
	private bool boostRamp = false;
	private bool endRamp = false;
	private int boostsLeft;
	public int maxBoosts = 15;

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

	private Vector3 playerReset = new Vector3 (60, 5, 60);

	
	void Start () {
		playerBody = GetComponent<Rigidbody>();
		playerBody.centerOfMass = center;
		Physics.gravity = new Vector3(0, -90.0F, 0);
		setCountText();
		driftText.enabled = false;
		reverseTorque *= -1;
	}
	//END Start()//
	
	void FixedUpdate () {
		forward = Input.GetKey(moveForward); //Refreash the booleans based on their corresponding buttons state
		left    = Input.GetKey(moveLeft);
		right   = Input.GetKey(moveRight);
		back    = Input.GetKey(moveBack);

		resetTorqueAndAngles();
		braking = false;

		if (forward && !back) {
			rearRightTorque = applySpeedChange (forwardTorque, rearRightTorque);
			rearLeftTorque = applySpeedChange (forwardTorque, rearRightTorque);
		} else if (back && !forward) {
			rearRightTorque = applySpeedChange (reverseTorque, rearRightTorque);
			rearLeftTorque = applySpeedChange(reverseTorque, rearLeftTorque);
		}

		if (left && !right) { //If the left key is pressed
			frontLeftTire.steerAngle = -turnAngle;
			frontRightTire.steerAngle = -turnAngle;
			driftText.enabled = true;
			smoothIncrease();
		} else if (right && !left) { //If the right key is pressed
			frontRightTire.steerAngle = turnAngle;
			frontLeftTire.steerAngle = turnAngle;
			driftText.enabled = true;
			smoothIncrease();
		} else {
			driftText.enabled = false;
		}

		if (Input.GetKey("space")) {
			braking = true;
		}

		if ((boostsLeft <= maxBoosts)) {
			if (boostRamp) {
				applyBoost(100000);
			} else if (boostObject) {
				applyBoost(2000);
			}
		} else if (boostsLeft > maxBoosts) {
			boostsLeft = 0;
			boostObject = false;
			endObject = true;
			boostRamp = false;
			endRamp = true;
		}

		if (endRamp) {
			applyBoost(-20000000);
			endRamp = false;
		}
		if (endObject) {
			applyBoost(-2000);
			endRamp = false;
		}

		if (Input.GetKey("1")) {
			playerBody.MovePosition(playerReset);
		}
		if (Input.GetKey("e")) {
			playerCamera.transform.Rotate(0, -1, 0);
		}
		if (Input.GetKey("q")) {
			playerCamera.transform.Rotate(0, 1, 0);
		}

		brakeCar(braking);
		setCountText();
		setTorques();
	}
	//END FixedUpdate()//

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("GoodCollectable"))
		{
			other.gameObject.SetActive (false);
			scoreCounter += 1000;
			boostObject = true;
			boostsLeft = 0;
		} else if (other.gameObject.CompareTag ("BadCollectable")) {
			other.gameObject.SetActive (false);
			scoreCounter -= 500;
			boostObject = false;
			boostRamp = false;
		}

		if (other.gameObject.CompareTag ("Boost")) {
			boostRamp = true;
			boostsLeft = 0;
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
	}
	//END resetTorqueAndAngles//

	void setTorques() {
		rearLeftTire.motorTorque = rearLeftTorque;
		rearRightTire.motorTorque = rearRightTorque;
		frontLeftTire.motorTorque = frontLeftTorque;
		frontRightTire.motorTorque = frontRightTorque;
	}
	//END setTorques//

	void brakeCar (bool brake) {
		if (brake) {
			rearLeftTorque = applySpeedChange (0, rearLeftTorque);
			rearRightTorque = applySpeedChange (0, rearLeftTorque);
			frontLeftTorque = applySpeedChange (0, rearLeftTorque);
			frontRightTorque = applySpeedChange (0, rearLeftTorque);

			rearRightTire.brakeTorque = 1800;
			rearLeftTire.brakeTorque = 1800;
			frontLeftTire.brakeTorque = 1800;
			frontRightTire.brakeTorque = 1800;
		} else {
			rearRightTire.brakeTorque = 0;
			rearLeftTire.brakeTorque = 0;
			frontLeftTire.brakeTorque = 0;
			frontRightTire.brakeTorque = 0;
		}
	}
	//END brakeCar()//

	void applyBoost(int boostLevel) {
		if (boostLevel > 0) {
			rearLeftTorque = applySpeedChange (rearLeftTorque + boostLevel, rearLeftTorque);
			rearRightTorque= applySpeedChange (rearRightTorque + boostLevel, rearRightTorque);
			frontLeftTorque = applySpeedChange (rearLeftTorque + boostLevel, rearLeftTorque);
			frontRightTorque = applySpeedChange (rearRightTorque + boostLevel, rearRightTorque);
			++boostsLeft;
		} else {
			rearRightTire.brakeTorque = boostLevel;
			rearLeftTire.brakeTorque = boostLevel;
			frontLeftTire.brakeTorque = boostLevel;
			frontRightTire.brakeTorque = boostLevel;
		}
	}
	//END applyBoost()//

	int applySpeedChange (int target, int current) {
		int returnValue = (int) Mathf.Lerp (current, target, deltaSpeed);
		return returnValue;
	}
	//END applySpeedChange()//app
}
//END PlayerController Class//