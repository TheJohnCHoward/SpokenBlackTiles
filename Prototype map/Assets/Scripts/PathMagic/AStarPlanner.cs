using UnityEngine;
using System.Collections.Generic;

public class AStarPlanner {
	private SparseGraph Graph;
	private List<GraphNode> OpenList;
	private List<GraphNode> ClosedList;
	private double[] FCosts;
	private double[] GCosts;
	private GraphEdge[] ParentEdges; // Stores the edge leading to each node. Used for distance and to track parent node connections.
	public bool visualization = false;
		
	public AStarPlanner(SparseGraph graph){
		this.Graph = graph;
		OpenList = new List<GraphNode>();
		ClosedList = new List<GraphNode>();
		
		int allNodesCount = graph.GetAllNodesCount();
		FCosts = new double[allNodesCount];
		GCosts = new double[allNodesCount];
		ParentEdges = new GraphEdge[allNodesCount]; 
	}
	public List<GraphNode> FindPath(Vector3 startLoc, Vector3 endLoc){
		int startIndex = Graph.GetNearestNode(startLoc).Index;
		int endIndex = Graph.GetNearestNode(endLoc).Index;
		return FindPath(startIndex, endIndex);
	} 
	
	public List<GraphNode> FindPath(int startIndex, int endIndex){
		// Initialize+
		OpenList = new List<GraphNode>();
		ClosedList = new List<GraphNode>();
		
		int allNodesCount = Graph.GetAllNodesCount();
		FCosts = new double[allNodesCount];
		GCosts = new double[allNodesCount];
		ParentEdges = new GraphEdge[allNodesCount];
		
		// Locate start node.
		GraphNode startNode = Graph.GetNode(startIndex);
		GraphNode endNode = Graph.GetNode(endIndex);
		
		//Get all edges leading from the start index;
		List<GraphEdge> adjacentEdges = Graph.GetEdges(startIndex);
		int destNodeIndex;
		foreach(GraphEdge edge in adjacentEdges){
			destNodeIndex = edge.ToIndex;// Get the index of the node the edge leads to.
			OpenList.Add(Graph.GetNode(destNodeIndex));	// Add it to the open list.
			ParentEdges[destNodeIndex] = edge; // Set the parent of the new node.
			if(visualization){
				Debug.DrawLine(Graph.GetNode(edge.ToIndex).Position, Graph.GetNode(edge.FromIndex).Position, Color.red, 10000F);
			}
		}
		// Add the start node to the closed list.
		ClosedList.Add(startNode);
		
		//Calculate F and G costs.
		double g, h;
		foreach(GraphNode node in OpenList){
			// Calculate g cost.
			g = 0;
			int index = node.Index;
			while (ParentEdges[index] != null) {
				g += ParentEdges[index].Cost;
				index = ParentEdges[index].FromIndex;
			}
			GCosts[node.Index] = g;
			
			// Calculate h cost.
			h = Vector3.Distance(node.Position, endNode.Position);
			FCosts[node.Index] = g + h;
		}
		bool pathFound = false;
		GraphNode bestNode;
		while(OpenList.Count > 0 && pathFound == false){ // While there are still nodes to search
			
			// Choose the lowest F cost node.
			bestNode = OpenList[0];
			double minCost = FCosts[bestNode.Index];
			foreach(GraphNode n in OpenList){
				if(FCosts[n.Index] < minCost){
					minCost = FCosts[n.Index];
					bestNode = n;
				}	
			}
			
			// Move the node to the closed list.
			OpenList.Remove(bestNode);
			ClosedList.Add(bestNode);
			if(bestNode.Index == endNode.Index){ // Check for end condition
				pathFound = true;
			}
			else {
				// Consider all the nodes around the new node.
				adjacentEdges = Graph.GetEdges(bestNode.Index);
				GraphNode curNode;
				foreach(GraphEdge edge in adjacentEdges){
					if(visualization){
						Debug.DrawLine(Graph.GetNode(edge.ToIndex).Position, Graph.GetNode(edge.FromIndex).Position, Color.red, 10000F);
					}
					curNode = Graph.GetNode(edge.ToIndex);
					if(!ClosedList.Contains(curNode)){// If the node is not on the closed list
						if(!OpenList.Contains(curNode)){// If the node is not on the open list, add it.
							OpenList.Add(curNode);	
							ParentEdges[curNode.Index] = edge;
							// Calculate F and G values.
							GCosts[curNode.Index] = edge.Cost + GCosts[edge.FromIndex];
							h = Vector3.Distance(curNode.Position, endNode.Position);
							FCosts[curNode.Index] = GCosts[curNode.Index] + h;
						}
						else { // Node was already on the open list. See if new path here is better.
							double newGCost = edge.Cost + GCosts[bestNode.Index];
							if(newGCost < GCosts[curNode.Index]){ // New path is better. 
								ParentEdges[curNode.Index] = edge;// Set new parent
								// Recalculate F and G.
								GCosts[curNode.Index] = newGCost;
								h = Vector3.Distance(curNode.Position, endNode.Position);
								FCosts[curNode.Index] = newGCost + h;
							}
							else {
								// Do nothing. The old path is better.
							}
						}
					}
				}
			}		
		} // End of main loop.
		if(!pathFound){
			Debug.Log("No path found.");
			return null; // Failed to find a path.	
		}
		else {
			Debug.Log("Path found.");
			// Build path and return 
			List<GraphNode> retVal = new List<GraphNode>();
			int curIndex = endNode.Index;
			while (ParentEdges[curIndex] != null) {
				//Debug.Log(ParentEdges[curIndex].ToIndex);
				retVal.Add(Graph.GetNode(curIndex));
				curIndex = ParentEdges[curIndex].FromIndex;
			}
			retVal.Add(Graph.GetNode(curIndex));//Finally, add the starting node.
			retVal.Reverse();
			return retVal;
		}
	}
}
