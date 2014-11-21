private var motor : CharacterMotor;

var walk;
var run;

walk = motor.movement.maxForwardSpeed;
run = motor.movement.maxForwardSpeed*2.5;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
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
	
}

/*Carica le immagini della "manina" come texture
		(L'immagine della manina verrà ovviamente
		sostituita dagli artisti con una più decente :) )*/

function OnTriggerEnter (other:Collider){
	if (other.gameObject.tag == "Interactive") { //quando è in prossimità di un oggetto di tipo "interattivo"..
    	GameObject.Find("puntatore").SendMessage("playerNearObj");
    }
}

function OnTriggerExit(collider:Collider ){
    GameObject.Find("puntatore").SendMessage("playerFarObj");
}




// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")
