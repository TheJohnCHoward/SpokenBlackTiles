var speed = 50.0;
var rotateSpeed = 1.0;
var jumpSpeed = 20.0;
var gravity = 20.0;
var controls = 1;	//swap control methods
private var moveDirection : Vector3 = Vector3.zero;

var e : Event = Event.current;

function OnGUI() {
    var e : Event = Event.current;
    if (e.isKey) {
        //Debug.Log("Detected character: " + e.character);
        if(e.character == "z")
        	speed += 5;
      	else if(e.character == "x")
      		speed -= 5;
      	else if(e.character == "c")
      		rotateSpeed += 1;
      	else if(e.character == "v")
      		rotateSpeed -= 1;
      	else if(e.character == "t")
      		controls = 1; //t for tank
      	else if(e.character == "y")
      		controls = 0; //y for *shrug	 
    }
    Debug.Log("Speed: " + speed);
    Debug.Log("rotateSpeed: " + rotateSpeed);
}

function Update () {
	var controller : CharacterController = GetComponent(CharacterController);
	
	if(controls == 1) {
		//rotate around y axis
		transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0);
		
		//move forward/backward
		var forward = transform.TransformDirection(Vector3.forward);
		var curSpeed = speed * Input.GetAxis ("Vertical");
		controller.SimpleMove(forward * curSpeed);
	}
	if(controls == 0) {
		//transform.Translate(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
		transform.Translate(x,0,z);	
	}
	
	
	//jump
	if(Input.GetButton("Jump")) {		//if jump is pressed
		if(controller.isGrounded) 	 //if jump is pressed while the player is on the ground
			moveDirection.y = jumpSpeed;	//move in the y direction by jumpSpeed units
	}
	moveDirection.y -= gravity * Time.deltaTime;
	controller.Move(moveDirection * Time.deltaTime);
}

@script RequireComponent(CharacterController)