using UnityEngine;
using System.Collections.Generic;

public class SparseGraph {
	private List<GraphNode> Nodes;
	private List<List<GraphEdge>> Edges;
	private int NextNodeIndex;
	
	// Constructor
	public SparseGraph(){
		this.NextNodeIndex = 0;
		Nodes = new List<GraphNode>();
		Edges = new List<List<GraphEdge>>();
	}
	
	// Returns the node at the specified index.
	public GraphNode GetNode(int index){
		if(index <= Nodes.Count - 1){
			GraphNode retVal = Nodes[index];
			if(retVal.Index != GraphNode.INVALID_NODE_INDEX){
				return retVal;
			}
		}
		return null;
	}
	
	// Returns the edge, given a set of from and to node indices, or null if non-existant.
	public GraphEdge GetEdge(int fromIndex, int toIndex){
		List<GraphEdge> nodeEdges = Edges[fromIndex];
		foreach(GraphEdge edge in nodeEdges){
			if(edge.ToIndex == toIndex){
				return edge;	
			}
		}
		return null;
	}
	
	//Returns a list of GraphEdge objects leading away from the given node index.
	public List<GraphEdge> GetEdges(int fromIndex){
		if(Nodes[fromIndex].Index == GraphNode.INVALID_NODE_INDEX){
			return null;	
		}
		List<GraphEdge> retVal = Edges[fromIndex];
		for(int i=0;i<retVal.Count;i++){
			if(Nodes[retVal[i].ToIndex].Index == GraphNode.INVALID_NODE_INDEX){
				retVal.RemoveAt(i);
				i--;
			}
		}
		return retVal;
	}
	
	// Returns the number of active and inactive nodes in the graph.
	public int GetAllNodesCount(){
		return Nodes.Count;
	}
	
	// Returns the number of active nodes in the graph. 
	public int GetActiveNodesCount(){
		int count = 0;
		foreach( GraphNode n in Nodes ){
			if(n.Index != GraphNode.INVALID_NODE_INDEX) {
					count ++;
			}
		}
		return count;
	}
	public int AddNode(Vector3 position){
		Nodes.Add(new GraphNode(NextNodeIndex, position));
		Edges.Add(new List<GraphEdge>());
		GraphNode n = new GraphNode(NextNodeIndex, position);
		NextNodeIndex++;
		return NextNodeIndex - 1;
	}
	public bool AddEdge(int fromIndex, int toIndex, double cost){
		if(Nodes[fromIndex].Index == GraphNode.INVALID_NODE_INDEX
		   || Nodes[toIndex].Index == GraphNode.INVALID_NODE_INDEX ) {
			return false;
		}
		else { 
			Edges[fromIndex].Add(new GraphEdge(fromIndex, toIndex, cost));
			return true;
		}
	}
	public GraphNode GetNearestNode(Vector3 loc){
		float minDistance = float.MaxValue;
		int bestIndex = 0;
		float curDistance;
		foreach(GraphNode n in Nodes){
			if(n.Index != GraphNode.INVALID_NODE_INDEX){
				curDistance = Vector3.Distance(loc, n.Position);
				if(curDistance < minDistance){
					minDistance = curDistance;
					bestIndex = n.Index;
				}
			}
		}
		return Nodes[bestIndex];
	}
	public int GetIsolatedNodesCount(){
		int count = 0;
		foreach(List<GraphEdge> edgz in	Edges){
			if(edgz.Count == 0) {
				count++;	
			}
		}
		return count;
	}
}
