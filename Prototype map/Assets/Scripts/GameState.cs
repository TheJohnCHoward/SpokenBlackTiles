using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
	public AudioClip clip;
	private int collectibles;
	private bool isCultist, ending;
	private float time;
	
	// Use this for initialization
	void Start () {
		collectibles = 0;
		ending = false;
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (ending)
			time += Time.deltaTime;
		
		if (time >= clip.length) {
			if (isCultist)
				Application.LoadLevel(3);
			else
				Application.LoadLevel(4);
		}
	}
	
	// Pick up an item.
	public void addCollectible(){
		collectibles++;
		if (collectibles == 5) {
			networkView.RPC("secondLayer", RPCMode.AllBuffered);
		}
		else if(collectibles == 6){
			activateAudioLayer();
		}
	}
	
	private void activateAudioLayer(){
		// Do stuff.
		networkView.RPC("cultistWin", RPCMode.AllBuffered);
	}
	
	void OnGUI() {
		if (isCultist) {
			GUILayout.BeginArea(new Rect(10, 10, 100, 100));
			GUILayout.Label(collectibles + "/6 tiles");
			GUILayout.EndArea();
		}
	}
	
	void setCultist() {
		isCultist = true;
	}
	
	[RPC]
	void cultistWin() {
		AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, 0));
		ending = true;
	}
	
	[RPC]
	void secondLayer() {
		GameObject camera = GameObject.FindGameObjectWithTag("Camera");
		foreach (AudioSource audio in camera.GetComponents<AudioSource>()) {
			audio.mute = false;
		}
	}
	
}
