using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Control : MonoBehaviour {
	
	private static readonly System.Random RAND = new System.Random();
	public float speed = 40;
	public float rotationSpeed = 150;
	public float gravity = 100;
	private float stepDelay = 0;
	public AudioClip jason, footstep, waterFootstep;
	private Transform you = null;
	private CharacterController controller;
	private bool isCultist = false;
	private bool started = false;
	private bool water = false;
	private enum Powerup {Barrier, Arrows, None};
	private Powerup powerup;
	private Array values;
	private GameObject[] doors, detectives;
	private GameObject cultist;
	private List<Transform> arrows;
	public Transform barrier, decoy, arrow;
	private bool radar;
	private float radarTime;
	
	void Awake() {
		powerup = Powerup.None;
		values = Enum.GetValues(typeof(Powerup));
		doors = GameObject.FindGameObjectsWithTag("Door");
		arrows = new List<Transform>();
		radar = false;
	}
		
	// Allow for character movement
	void Update() {
		if (you != null & started) {
			// Allow for cultist/non-cultist specific controls
			/*
			if(isCultist) {
			}
			else {
			}
			*/
			
			// Movement control
			Vector3 forward = you.TransformDirection(Vector3.forward);
			float translation = Input.GetAxis("Vertical") * speed;
			translation *= Time.deltaTime;
			Vector3 vert = Vector3.up * gravity * Time.deltaTime;
			controller.Move(forward*translation - vert);
			
			// Rotation control
			float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
			rotation *= Time.deltaTime;
			you.Rotate(0, rotation, 0);
			
			// Footsteps
			if (Input.GetAxis("Vertical") != 0 & stepDelay == 0) {
				stepDelay += Time.deltaTime;
				if (water) {
					networkView.RPC("playWaterFootstep", RPCMode.AllBuffered, you.position);
				}
				else {
					networkView.RPC("playFootstep", RPCMode.AllBuffered, you.position);
				}
			}
			else if (Input.GetAxis("Vertical") != 0 & stepDelay < .25) {
				stepDelay += Time.deltaTime;
			}
			else {
				stepDelay = 0;
			}
			
			// test give powerup
			if (Input.GetButtonDown("Speech")) {
				powerup = (Powerup)values.GetValue(RAND.Next(values.Length - 1));
			}
			
			// Powerups
			/*
			switch (powerup) {
			case Powerup.Barrier:
				*/
				if (Input.GetButtonDown("Speech"))  {
					if (isCultist) {
						Network.Instantiate(decoy, you.position - 5*you.forward, you.rotation, 0);
					}
					else {
						bool canSpawn = true;
						foreach (GameObject door in doors) {
							if (Vector3.Distance(door.transform.position, you.position) < 30) {
								canSpawn = false;
							}
						}
						if (canSpawn) {
							Network.Instantiate(barrier, you.position - 5*you.forward, you.rotation, 0);
						}
					}
				}
			/*
				break;
			case Powerup.Arrows:
				*/
				if (Input.GetButtonDown("Jason")) {
					if (radar == false) {
						radar = true;
						radarTime = 0;
						if (isCultist) {
							detectives = GameObject.FindGameObjectsWithTag("Player");
							for (int i = 0; i < detectives.Length; i++) {
								arrows.Add((Transform) GameObject.Instantiate(arrow, you.position + (detectives[i].transform.position - you.position).normalized*3, you.rotation));
								//arrows.Add((Transform) GameObject.Instantiate(arrow, you.position, you.rotation));
								arrows[i].LookAt(detectives[i].transform);
								arrows[i].transform.Rotate(Vector3.right * 180);
							}
						}
						else {
							cultist = GameObject.FindGameObjectWithTag("Cultist");
							arrows.Add((Transform) GameObject.Instantiate(arrow, you.position + (cultist.transform.position - you.position).normalized*3, you.rotation));
							//arrows.Add((Transform) GameObject.Instantiate(arrow, you.position, you.rotation));
							arrows[0].LookAt(cultist.transform);
							arrows[0].transform.Rotate(Vector3.right * 180);
						}
					}
				}
			/*
				break;		
			}
			*/
			
			// Rotate arrows
			if (isCultist & radar == true) {
				for (int i = 0; i < arrows.Count; i++) {
					arrows[i].transform.position = you.position + (detectives[i].transform.position - you.position).normalized*3;
					//arrows[i].transform.position = you.position;
					arrows[i].LookAt(detectives[i].transform);
					arrows[i].transform.Rotate(Vector3.right * 180); 
				}
				radarTime += Time.deltaTime;
			}
			else if (radar == true) {
				arrows[0].transform.position = you.position + (cultist.transform.position - you.position).normalized*3;
				//arrows[0].transform.position = you.position;
				arrows[0].LookAt(cultist.transform);
				arrows[0].transform.Rotate(Vector3.right * 180);
				radarTime += Time.deltaTime;	
			}
			
			if (radarTime > 10) {
				radar = false;
				for (int i = 0; i < arrows.Count; i++) {
					Destroy(arrows[i].gameObject);
				}
				arrows.Clear();
			}
					
				
		}
	}
	
	// Get powerups
	public void OnTriggerEnter(Collider other) {
		/*
		if (other.name == "ChurchEntrance") {
			you.position = GameObject.Find("ChurchSpawn").transform.position;
			print("test");
		}
		else if (other.name == "ChurchSpawn") {
			you.position = GameObject.Find("ChurchEntrance").transform.position;
		}
		*/
		
		// Detect water
		if (other.tag == "water") {
			water = true;
		}
		
		// Get powerup
		if (other.tag == "powerup") {
			powerup = (Powerup)values.GetValue(RAND.Next(values.Length - 1));
		}
	}
	
	public void OnTriggerExit(Collider other) {
		if (other.tag == "water") {
			water = false;
		}
	}
	
	// Set cultist value and apply initial cultist conditions
	void setCultist() {
		isCultist = true;
		// cultist conditions
	}
	
	void setPlayer(Transform you) {
		this.you = you;
		controller = (CharacterController) you.gameObject.GetComponent(typeof(CharacterController));
	}
	
	void start() {
		started = true;
	}
	
	[RPC]
	void playFootstep(Vector3 pos) {
		AudioSource.PlayClipAtPoint(footstep, pos);
	}
	
	[RPC]
	void playWaterFootstep(Vector3 pos) {
		AudioSource.PlayClipAtPoint(waterFootstep, pos);
	}
	
}
