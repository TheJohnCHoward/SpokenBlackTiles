using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIBehavior : MonoBehaviour {
	private List<GraphNode> path;
	private Vector2 nextDestination;
	private Vector2 myPosition;
	private GraphGenerator pathfinder;
	private Transform you;
	private Vector3 forward;
	private Vector3 vert;
	private Vector3 translationVec;
	private CharacterController controller;
	private float speed = 18;
	private float gravity = 100;
	private GameObject[] waypoints;
	private float pathTimer;
	private float chaseTimer;
	public float playerChaseDuration = 5F;
	RaycastHit hit;
	public float visionRange = 90F;
	public float pathDelay = 3F; // Number of seconds to wait before re-pathing.
	
	public GameObject target;
	
	// Use this for initialization
	void Start () {
		Random.seed = (int)Time.time;
		this.pathfinder = GameObject.Find("Core").GetComponent<GraphGenerator>();
		if(pathfinder == null){
			Debug.LogError("Problem: Failed to locate GraphGenerator script in scene. Make sure it is attached to a GameObject called 'Pathfinder'.");	
		}
		you = this.gameObject.transform;
		controller = (CharacterController)you.gameObject.GetComponent(typeof(CharacterController));
		target = GameObject.FindGameObjectWithTag("Cultist");
		waypoints = GameObject.FindGameObjectsWithTag("waypoint");
		chaseTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		pathTimer += Time.deltaTime;
		myPosition = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z); // Update my position, ignoring vertical.
		
		if(chaseTimer > 0) {
			chaseTimer -= Time.deltaTime;
			moveToTarget();
		}
		else{ // Proceed normally.
			// Look around for the player.
			if(pathTimer > pathDelay){
				Debug.Log("Raycasting...");
				if(Vector3.Distance(gameObject.transform.position, target.transform.position)<visionRange){
					if(Physics.Raycast(gameObject.transform.position,target.transform.position-gameObject.transform.position, out hit, visionRange)){// If raycast spots player
						Debug.Log("Hit: "+hit.transform.gameObject.tag);
						
						if(hit.transform.gameObject.tag.Equals("Cultist")){
							getNewPathToTarget();
							chaseTimer = playerChaseDuration;
							pathTimer = 0;
						}else {
							
						}
					}	
				}
			}
			// If I have nowhere to go, generate a new path.
			if(path == null) {
				getNewPath();
			}
			// If I've still got path left to traverse, traverse it.
			else if(path.Count>0){
				if(Vector3.Distance(new Vector3(myPosition.x,path[0].Position.y,myPosition.y), path[0].Position) < 5.0F){// I'm at the node I was headed towards. Now grab the next one and go there.
					path.RemoveAt(0);
					Debug.Log("Removing node.");
					if(path.Count<=0){
						getNewPath();	
					}
				}
				else {
					//Debug.Log("Distance from target node: "+Vector3.Distance(myPosition, path[0].Position));
					//Debug.Log("Distance from last node: "+Vector3.Distance(myPosition, path[path.Count-1].Position));
					Debug.Log("Next Node Position: "+ path[0].Position);
					//Debug.Log("Target Location: "+path[path.Count-1].Position);
					//Debug.Log("My Location: "+myPosition);
					moveToNextNode();	
				}
			}
			// If I've completed my path, get a new path. 
			else {
				getNewPath();	
			}
		}
	}
	
	private void getNewPath(){
		int index = Random.Range(0,waypoints.Length-1);
		Vector3 targetLoc = waypoints[index].transform.position; // Get the next place you want to go. 
	//	Debug.Log("My Location: "+ this.gameObject.transform.position);
		path = pathfinder.path(this.gameObject.transform.position, targetLoc);
	}
	
	private void getNewPathToTarget(){
		Vector3 targetLoc = target.transform.position; // Get path to the target (player).
		path = pathfinder.path(this.gameObject.transform.position, targetLoc);
	}
	//private int count = 0;
	private void moveToNextNode(){
		//count++;
		//Debug.Log("Moving!"+count);
		// Movement control
		Vector3 horizontalDirection = (new Vector3(path[0].Position.x-myPosition.x, 0, path[0].Position.z-myPosition.y)).normalized;
		translationVec = horizontalDirection * speed;
		translationVec *= Time.deltaTime;
		vert = Vector3.up * gravity * Time.deltaTime;
		controller.Move(translationVec - vert);
		gameObject.transform.LookAt(gameObject.transform.position+translationVec);
	}
	private void moveToTarget(){
		// Movement control
		//forward = you.TransformDirection(Vector3.forward);
		Vector3 horizontalDirection = (new Vector3(target.transform.position.x-myPosition.x, 0, target.transform.position.z-myPosition.y)).normalized;
		translationVec = horizontalDirection * speed;
		translationVec *= Time.deltaTime;
		vert = Vector3.up * gravity * Time.deltaTime;
		controller.Move(translationVec - vert);
		gameObject.transform.LookAt(gameObject.transform.position+translationVec);
	}
}
