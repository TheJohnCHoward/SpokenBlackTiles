using UnityEngine;
using System.Collections;

public class EndMenu : MonoBehaviour {
	
	public GUISkin skin;
	public GUITexture texture;
	public Texture2D title;
	
	void OnGUI () {
		GUI.skin = skin;
		if (GUI.Button(new Rect(Screen.width*.36f, Screen.height*.72f, Screen.width*.27f, Screen.height*.08f), ""))
			Application.LoadLevel("Lobby");
		if (GUI.Button(new Rect(Screen.width*.3f, Screen.height*.81f, Screen.width*.4f, Screen.height*.08f), "")) {
			Network.Disconnect();
			Application.LoadLevel("MainMenu");
		}
	}

}
