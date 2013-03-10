using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	
	private GameState state;
	
	// Use this for initialization
	void Start () {
		state = GameObject.Find("Core").GetComponent<GameState>();
		if(state == null){
			Debug.LogError("Failed to locate GameState script.");	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider collider){
		// checks if the collision was triggered by the player, 
		// and if so plays the pickup sound, adds a collectible 
		// to the game state and destroys the collectible
		if(collider.gameObject.tag == "Cultist"){
			state.addCollectible();
			Destroy(this.gameObject);
		}
	}
}