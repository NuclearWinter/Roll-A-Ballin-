/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour {
	public Rigidbody playerRidgidBody; //Player's body
	
	public int speed = 5; //Speed multiplier

	private GameObject topRightCamera 	 = null; //Creation of Camera Objects
	private GameObject topLeftCamera	 = null;
	private GameObject middleRightCamera = null;
	private GameObject middleLeftCamera  = null;
	private GameObject bottomRightCamera = null;
	private GameObject bottomLeftCamera  = null;

	private bool playerZPositive = false; //Booleans to tell where the player is located
	private bool playerXMiddle 	 = false;
	private bool playerXBottom 	 = false;
	private bool playerXTop 	 = false;

	private Vector3 rawDirection; //The private direction to move the player
	public Vector3 directionVector; //The public direction to move the player (will be implimented more later)

	private string moveForward = "w"; //The strings for the controls
	private string moveLeft    = "a";
	private string moveRight   = "d";
	private string moveBack    = "s";

	private bool forward = false; //Booleans for controls
	private bool left    = false;
	private bool right   = false;
	private bool back    = false;
	
	void Start () {
		playerRidgidBody = GetComponent<Rigidbody> (); //Assign the RidgidBody

		topRightCamera 	  = GameObject.Find ("TopRight Camera"); //Initialize the Camera Objects
		topLeftCamera 	  = GameObject.Find ("TopLeft Camera");
		middleRightCamera = GameObject.Find ("MiddleRight Camera");
		middleLeftCamera  = GameObject.Find ("MiddleLeft Camera");
		bottomRightCamera = GameObject.Find ("BottomRight Camera");
		bottomLeftCamera  = GameObject.Find ("BottomLeft Camera");
	}
	////END Start()////

	void FixedUpdate () {

		float weight = 0.5f;

		forward = Input.GetKey(moveForward); //Refreash the booleans based on their corresponding buttons state
		left    = Input.GetKey(moveLeft);
		right   = Input.GetKey(moveRight);
		back    = Input.GetKey(moveBack);

		int invertX = -1;
		int invertZ = -1;

		Vector3 targetVector = Vector3.zero; //The vector of where to move the player to

		if (left && !right) { //If the left key is pressed
			targetVector = new Vector3(targetVector.x * invertX, 0, -1 * invertZ); //The target vector is moved left
		} else if (right && !left) { //If the right key is pressed
			targetVector = new Vector3(targetVector.x * invertX, 0, 1 * invertZ);  //The target vector is moved right
		}

		if (forward && !back) {
			targetVector = new Vector3(-1 * invertX, 0, targetVector.z * invertZ);
		} else if (back && !forward) {
			targetVector = new Vector3(1 * invertX ,0,targetVector.z * invertZ);
		}

		rawDirection = Vector3.MoveTowards(rawDirection,targetVector,weight); //Set the local direction variable
		
		directionVector = rawDirection; //Set the public variable equal to the local one
		
		if (directionVector != Vector3.zero) { //If the direction vector is not 0 (the player is moving)
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			directionLength = Mathf.Min(1, directionLength);
			directionLength *= directionLength;
			directionVector = directionVector * directionLength;

			playerRidgidBody.AddForce((directionVector) * speed);
		}
	}
	////END FixedUpdate()////

	//
	  The method LateUpdate is used mainly for controlling the cameras
	  The player position booleans within here may be used elsewhere in this class at a later date

	void LateUpdate() {
		float playerX = playerRidgidBody.transform.position.x; //Get the player position on the X and Z axis
		float playerZ = playerRidgidBody.transform.position.z;

		playerZPositive = (playerZ >= 0) ? true: false; 					//Check if the player is in the positive or negative side of the Z axis
		playerXMiddle 	= (playerX > 0 && playerX < 500) ? true: false;		//Check if the player is in the middle third
		playerXBottom 	= (playerX < 0) ? true: false;						//Check if the player is in the bottom third
		playerXTop 		= (!playerXMiddle && !playerXBottom) ? true: false; //Check if the player is in the top third

		clearCams(); //Turn off all cameras (will not be rendered)

		if (playerXMiddle) { 						//If the player is in the middle third
			if (playerZPositive) { 					//If the player is on the right (positive) side, turn on that camera
				middleRightCamera.SetActive(true);
			} else {
				middleLeftCamera.SetActive(true); 	//Otherwise turn on the left camera
			}
		} else if (playerXBottom) {					//If the player is in the bottom third
			if (playerZPositive) {
				bottomRightCamera.SetActive(true);	//If the player is on the right (positive) side, turn on that camera
			} else {
				bottomLeftCamera.SetActive(true);	//Otherwise turn on the left camera
			}
		} else if (playerXTop) {
			if (playerZPositive) {
				topRightCamera.SetActive(true);		//If the player is on the right (positive) side, turn on that camera
			} else {
				topLeftCamera.SetActive(true);		//Otherwise turn on the left camera
			}
		}
	}
	////END LateUpdate()////


	private void clearCams() {
		middleRightCamera.SetActive(false);
		middleLeftCamera.SetActive(false);
		bottomRightCamera.SetActive(false);
		bottomLeftCamera.SetActive(false);
		topRightCamera.SetActive(false);
		topLeftCamera.SetActive(false);
	}
	////END clearCams()////


	void OnTriggerEnter (Collider other) {
		if (other.gameObject.CompareTag("")) {

		}
	}
	////END OnTriggerEnter()////
}
////END PlayerController Class////
*/