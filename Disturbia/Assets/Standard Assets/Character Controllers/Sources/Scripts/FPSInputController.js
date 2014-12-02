private var motor : CharacterMotor;

private var walk;
private var run;
private var gui;
private var hitTag;
private var inside;

walk = motor.movement.maxForwardSpeed;
run = motor.movement.maxForwardSpeed*2.5;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
	
	walk = motor.movement.maxForwardSpeed;
	run = motor.movement.maxForwardSpeed*2;
	
	gui = GameObject.Find("gui");


}

// Update is called once per frame
function Update () {
	// Get the input vector from keyboard or analog stick
	var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	
	if (directionVector != Vector3.zero) {
		// Get the length of the directon vector and then normalize it
		// Dividing by the length is cheaper than normalizing when we already have the length anyway
		var directionLength = directionVector.magnitude;
		directionVector = directionVector / directionLength;
		
		// Make sure the length is no bigger than 1
		directionLength = Mathf.Min(1, directionLength);
		
		// Make the input vector more sensitive towards the extremes and less sensitive in the middle
		// This makes it easier to control slow speeds when using analog sticks
		directionLength = directionLength * directionLength;
		
		// Multiply the normalized direction vector by the modified length
		directionVector = directionVector * directionLength;
	}

	/*Run or move*/

	if (Input.GetKey(KeyCode.LeftShift)){
		motor.movement.maxForwardSpeed = run;
		motor.movement.maxSidewaysSpeed = run;
	}
	else {
		motor.movement.maxForwardSpeed = walk;
		motor.movement.maxSidewaysSpeed = walk;
	}
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	motor.inputJump = Input.GetButton("Jump");

	/*Raycast (controllo se è di fronte a un oggetto )*/
	
	// raycast direction
	var ray : Ray = Camera.main.ViewportPointToRay (Vector3(0.5,0.5,0));
	//hit object
	var hit : RaycastHit;
	
	if (Physics.Raycast(ray,hit,10)){
		hitTag = hit.transform.tag;
		if (hitTag == "Interactive" && inside) { //quando è di fronte e di vicine a un oggetto "interattivo"..
    		gui.SendMessage("Interacts",true);
    	}
    	else 
    		gui.SendMessage("Interacts",false);
	}
	
}

/*COLLISIONE*/

function OnTriggerEnter (other:Collider){
	inside = true;
}

function OnTriggerExit(collider:Collider ){
    inside = false;

}
// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
