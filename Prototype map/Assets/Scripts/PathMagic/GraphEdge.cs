using UnityEngine;
using System.Collections;

public class GraphEdge {
	private GraphEdge next; // For use in linked list.
	private double cost;
	private int toIndex;
	private int fromIndex;
	
	// Constructors
	public GraphEdge(int fromIndex, int toIndex, double cost){
		this.FromIndex = fromIndex;	
		this.ToIndex = toIndex;	
		this.Cost = cost;
	}
	public GraphEdge(double cost){
		this.FromIndex = GraphNode.INVALID_NODE_INDEX;	
		this.ToIndex = GraphNode.INVALID_NODE_INDEX;	
		this.Cost = cost;
	}
	
	// Properties
	public int FromIndex{
		get { return this.fromIndex; }
		set { this.fromIndex = value; }
	}
	public int ToIndex{
		get { return this.toIndex; }
		set { this.toIndex = value; }
	}
	public double Cost{
		get { return this.cost; }
		set { this.cost = value; }
	}
	
	public GraphEdge Next{ // Retrieve next edge in LinkedList.
		get { return this.next; }
		set { this.next = value; }
	}
}
