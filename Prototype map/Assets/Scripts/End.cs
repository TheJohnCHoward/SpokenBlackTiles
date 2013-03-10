using UnityEngine;
using System.Collections;

public class End : MonoBehaviour {
	
	private bool isCultist = false;
	
	public void OnTriggerEnter(Collider other) {
		if (isCultist & other.tag == "Player") {
			networkView.RPC("cultistLoss", RPCMode.AllBuffered);
		}
	}
	
	void setCultist() {
		isCultist = true;
	}
	
	[RPC]
	void cultistLoss() {
		if (isCultist)
			Application.LoadLevel(4);
		else
			Application.LoadLevel(3);
	}
	
}
