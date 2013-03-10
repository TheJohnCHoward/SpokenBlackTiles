var speed = 20.0;
var e : Event = Event.current;

function OnGUI() {
    var e : Event = Event.current;
    if (e.isKey) {
        //Debug.Log("Detected character: " + e.character);
        if(e.character == "z")
        	speed += 5;
      	else if(e.character == "x")
      		speed -= 5;  
    }
    Debug.Log("Speed: " + speed);
}
/*
function Update () {
	//transform.Translate(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
	var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
	var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
	transform.Translate(x,0,z);
	
	//if(Input.GetButtonDown("Z"))
    //  speed++;

} */