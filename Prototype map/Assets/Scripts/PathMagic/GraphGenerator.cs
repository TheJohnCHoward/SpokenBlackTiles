using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GraphGenerator : MonoBehaviour {
	public int nodeCols = 150;
	public int nodeRows = 150;
	public float xStart = 0F;
	public float zStart = 0F;
	public float gridSpacing = 1.0F;
	public float maxVerticalOffset = 2000.0F;
	public Vector3 startLoc;
	private AStarPlanner planner;
	public bool visualization = true;
	
	// Use this for initialization
	void Start () {
		
		SparseGraph graph = new SparseGraph();
		RaycastHit hit;
		int newNodeIndex;
		int[,] structureMap = new int[nodeCols, nodeRows];
		for(int x=0;x<nodeCols;x++){
			for(int z=0;z<nodeRows;z++){
				if(Physics.Raycast(new Vector3((x*gridSpacing)+xStart,1000,(z*gridSpacing)+zStart),-Vector3.up, out hit, 9999.0F)){
					if(hit.collider.gameObject.tag == "walkable"){
						newNodeIndex = graph.AddNode(new Vector3((x*gridSpacing)+xStart,1000-hit.distance,(z*gridSpacing)+zStart));
					}
					else {
						newNodeIndex = GraphNode.INVALID_NODE_INDEX;	
					}
				}
				else {
					//newNodeIndex = graph.AddNode(new Vector3((x*gridSpacing)+xStart,0,(z*gridSpacing)+zStart));	
					newNodeIndex = GraphNode.INVALID_NODE_INDEX;	
				}
				structureMap[x,z] = newNodeIndex;
			}
		}
		float maxCostOrthogonal = Mathf.Sqrt(gridSpacing*gridSpacing + maxVerticalOffset*maxVerticalOffset);
		float maxCostDiagonal = Mathf.Sqrt(Mathf.Sqrt(2*gridSpacing*gridSpacing) + maxVerticalOffset*maxVerticalOffset);
		float cost;
		//Used in determining whether slope is too steep.
		for(int x=0;x<nodeCols;x++){
			for(int z=0;z<nodeRows;z++){
				int nodeIndex = structureMap[x,z];
				if(nodeIndex != GraphNode.INVALID_NODE_INDEX) {
					try{
						if(structureMap[x+1,z] != GraphNode.INVALID_NODE_INDEX){
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x+1,z]).Position);
							if(cost<maxCostOrthogonal){
								graph.AddEdge(nodeIndex,structureMap[x+1,z],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x+1,z]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x,z+1] != GraphNode.INVALID_NODE_INDEX) {
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x,z+1]).Position);
							if(cost<maxCostOrthogonal){
								graph.AddEdge(nodeIndex,structureMap[x,z+1],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x,z+1]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x+1,z+1] != GraphNode.INVALID_NODE_INDEX){
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x+1,z+1]).Position);
							if(cost<maxCostDiagonal){
								graph.AddEdge(nodeIndex,structureMap[x+1,z+1],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x+1,z+1]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x+1,z-1] != GraphNode.INVALID_NODE_INDEX){
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x+1,z-1]).Position);
							if(cost<maxCostDiagonal){
								graph.AddEdge(nodeIndex,structureMap[x+1,z-1],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x+1,z-1]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x-1,z+1] != GraphNode.INVALID_NODE_INDEX){
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x-1,z+1]).Position);
							if(cost<maxCostDiagonal){
								graph.AddEdge(nodeIndex,structureMap[x-1,z+1],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x-1,z+1]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x-1,z] != GraphNode.INVALID_NODE_INDEX){
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x-1,z]).Position);
							if(cost<maxCostOrthogonal){
								graph.AddEdge(nodeIndex,structureMap[x-1,z],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x-1,z]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x-1,z-1] != GraphNode.INVALID_NODE_INDEX) {
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x-1,z-1]).Position);
							if(cost<maxCostDiagonal){
								graph.AddEdge(nodeIndex,structureMap[x-1,z-1],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x-1,z-1]).Position,Color.green,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
					try{
						if(structureMap[x,z-1] != GraphNode.INVALID_NODE_INDEX){
							cost = Vector3.Distance(graph.GetNode(nodeIndex).Position, graph.GetNode(structureMap[x,z-1]).Position);
							if(cost<maxCostOrthogonal){
								graph.AddEdge(nodeIndex,structureMap[x,z-1],cost);
								if(visualization){
									Debug.DrawLine(graph.GetNode(nodeIndex).Position,graph.GetNode(structureMap[x,z-1]).Position,Color.blue,10000F);
								}
							}
						}
					}catch(System.IndexOutOfRangeException e){}
				}
			}
		}
		Debug.Log("Isolated nodes: " + graph.GetIsolatedNodesCount());
		planner = new AStarPlanner(graph);
		/*
		List <GraphNode> path = planner.FindPath(new Vector3(100,0,600), new Vector3(1100,0,1000));
		for(int i=0;i<path.Count-1;i++){
			Debug.DrawLine(path[i].Position,path[i+1].Position,Color.yellow, 10000F);	
		}
		*/
	}
	
	// Update is called once per frame
	void Update() {
	}
	
	// Call this to get a path to follow.
	public List<GraphNode> path(Vector3 startLoc, Vector3 endLoc){
		return planner.FindPath(startLoc, endLoc);
	}
}
