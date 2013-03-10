using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	private bool menu;
	private bool dc;
	
	// Use this for initialization
	void Start () {
		menu = false;
		dc = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Jump")) {
			menu = !menu;
		}
	}
	
	void OnGUI () {
		if (menu) {
			GUI.Box(new Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 200), "Menu");
			if (GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 100, 100), "Disconnect")) {
				Network.Disconnect();
			}
		}
		if (dc) {
			GUI.Label(new Rect(Screen.width/2 - 100, Screen.height/2 - 100, 200, 200), "Player Disconnected");
		}
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Application.LoadLevel(0);
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
		networkView.RPC("playerDisconnected", RPCMode.AllBuffered);
	}
	
	[RPC]
	void playerDisconnected() {
		dc = true;
	}

}
