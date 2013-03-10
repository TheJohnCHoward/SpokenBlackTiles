using UnityEngine;
using System.Collections;

public class Instantiate : MonoBehaviour {
	
	public Transform detective;
	public Transform cultist;
	public Transform hat;
	public Transform mainCamera;
	public Transform tile;
	public GameObject core;
	public Transform bsp1, bsp2;
	private Transform you;
	private Transform yourCamera;
	private Transform sp;
	private Transform yourHat;
	private Rect windowRect = new Rect(Screen.width/2 - 50, Screen.height/2 - 50, 100, 100);
	private bool isCultist = false;
	private int numPlayers;
	private bool started = false;
	private int count = 0;
	private Transform bot1, bot2;
	
	void Awake() {
		// Select random cultist and spawn bots
		if (Network.isServer) {
			bot1 = (Transform)Network.Instantiate(detective, bsp1.position, bsp1.rotation, 0);
			bot2 = (Transform)Network.Instantiate(detective, bsp2.position, bsp2.rotation, 0);
			networkView.RPC("checkCultist", RPCMode.AllBuffered, Random.Range(0, Network.connections.Length + 1), Network.connections.Length + 1);
		}
		Physics.IgnoreLayerCollision(9, 10);
	}
	
	void Start() {
	}
	
	void spawn(bool isCultist) {
		this.isCultist = isCultist;
		// Select cultist spawn pt and apply variables
		if (isCultist) {
			int spawn = Random.Range(0, 1);
			sp = GameObject.Find("Csp" + spawn).transform;
			you = (Transform) Network.Instantiate(cultist, sp.position, sp.rotation, 0);
		}
		// Select detective spawn pt
		else {
			string num = Network.player.ToString();
			sp = GameObject.Find("Sp" + num).transform;
			you = (Transform) Network.Instantiate(detective, sp.position, sp.rotation, 0);
			yourHat = (Transform) Network.Instantiate(hat, sp.position + sp.up * 3.5f, sp.rotation, 0);
			yourHat.parent = you;
		}
		// Spawn player
		yourCamera = (Transform) Instantiate(mainCamera, sp.position + sp.up * 3 + sp.forward * -10 + sp.right * 0, sp.rotation);
		core.SendMessage("setPlayer", you);
		yourCamera.parent = you;
		SphereCollider trigger = (SphereCollider) you.gameObject.AddComponent("SphereCollider");
		trigger.isTrigger = true;
		trigger.radius = 0.2f;
		if (isCultist) {
			core.SendMessage("setCultist");
			you.SendMessage ("setCultist");
			RenderSettings.fogDensity = 0.005f;
			GameObject[] tileSpawns = GameObject.FindGameObjectsWithTag("TileSpawn");
			foreach (GameObject tileSpawn in tileSpawns) {
				GameObject.Instantiate(tile, tileSpawn.transform.position, tileSpawn.transform.rotation);
			}
		}
		networkView.RPC("spawned", RPCMode.AllBuffered);
	}
	
	/*
	// Allow for disconnection
	void OnPlayerDisconnected(NetworkPlayer player) {
		Network.RemoveRPCs(player, 0);
		Network.DestroyPlayerObjects(player);
	}
	*/
	
	// GUI
	void OnGUI() {
		if (!started) {
			GUILayout.Label("Loading...");
		}
	}
	
	// Determine if client is cultist
	[RPC]
	void checkCultist(int cultist, int numPlayers) {
		this.numPlayers = numPlayers;
		if (cultist.ToString().Equals(Network.player.ToString())) {
			spawn(true);
		}
		else {
			spawn(false);
		}
	}
	
	void start() {
		started = true;
		if (Network.isServer) {
			bot1.GetComponent<AIBehavior>().enabled = true;
			bot2.GetComponent<AIBehavior>().enabled = true;
		}
	}
	
	// Wait for all players to spawn
	[RPC]
	void spawned() {
		count++;
		if (count == numPlayers) {
			core.SendMessage("start");
		}
	}
	
}
