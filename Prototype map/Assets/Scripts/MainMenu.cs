using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	public GUISkin skin;
	public GUITexture texture;
	public Texture2D title, howTo;
	private bool main;
	
	void Awake() {
		main = true;
	}
	
	void OnGUI () {
		GUI.skin = skin;
		if (main) {
			if (GUI.Button(new Rect(Screen.width*.36f, Screen.height*.82f, Screen.width*.27f, Screen.height*.08f), ""))
				Application.LoadLevel("Lobby");
			if (GUI.Button(new Rect(Screen.width*.36f, Screen.height*.91f, Screen.width*.27f, Screen.height*.08f), "")) {
				texture.texture = howTo;
				main = false;
			}
		}
		else {
			if (GUI.Button(new Rect(Screen.width*.36f, Screen.height*.91f, Screen.width*.27f, Screen.height*.08f), "")) {
				texture.texture = title;
				main = true;
			}
		}
	}

}
