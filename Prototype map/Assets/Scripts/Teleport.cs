using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Cultist" | other.tag == "Player") {
			if (this.name == "ChurchEntrance") {
				other.transform.position = GameObject.Find("ChurchSpawnIn").transform.position;
				other.transform.rotation = GameObject.Find("ChurchSpawnIn").transform.rotation;
			}
			else if (this.name == "ChurchExit") {
				other.transform.position = GameObject.Find("ChurchSpawnOut").transform.position;
				other.transform.rotation = GameObject.Find("ChurchSpawnOut").transform.rotation;
			}
			else if (this.name == "HouseEntrance") {
				other.transform.position = GameObject.Find("HouseSpawnIn").transform.position;
				other.transform.rotation = GameObject.Find("HouseSpawnIn").transform.rotation;
			}
			else if (this.name == "HouseExit") {
				other.transform.position = GameObject.Find("HouseSpawnOut").transform.position;
				other.transform.rotation = GameObject.Find("HouseSpawnOut").transform.rotation;
			}
		}
	}
		
}
