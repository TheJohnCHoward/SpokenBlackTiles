using UnityEngine;
using System.Collections;

public class GraphNode {
	public const int INVALID_NODE_INDEX = -1;
	private int index;
	private Vector3 position;
	
	// Properties
	public int Index{
		get { return this.index; }
		set { this.index = value; }
	}
	public Vector3 Position{
		get { return this.position; }
		set { this.position = value; }
	}
	
	
	// Constructors
	public GraphNode(int ind){
		this.Index = ind;	
	}
	public GraphNode(int ind, Vector3 pos){
		this.Index = ind;
		this.Position = pos;
	}
	public GraphNode(Vector3 pos){
		this.Position = pos;
		this.Index = INVALID_NODE_INDEX;
	}
}
