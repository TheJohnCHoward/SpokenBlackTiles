using UnityEngine;
using System.Collections;

//Calling Next returns the next-oldest entry. Works like a stack.
public class EdgeList{
	private int size;
	
	public EdgeList(){
		size = 0;
	}
	public int GetSize() {
		return this.size;
	}
	public GraphEdge Head; //The most recently-added item. Null if list is empty.
	
	
	public int Add(GraphEdge newItem){
		GraphEdge edgeToAdd = newItem;
		edgeToAdd.Next = Head;
		Head = edgeToAdd;
		return size++;
	}
	
	//Retrieves the edge at the given position in the list.
	public GraphEdge Retrieve(int position){
		GraphEdge current = Head;
		for(int i=0;i<position && current != null;i++){
			current=current.Next;
		}
		return current;
	}
	
	// Removes the most recently added entry.
	public bool Delete(){
		if( Head == null ){
			// The list is empty.
			return false;
		}
		GraphEdge current;
		current = Head.Next;
		Head.Next = current.Next;
		size--;
		return true;
	}
	
	// Removes entry at position.
	public bool Delete(int position){
		if( this.Retrieve(position) == null ) {
			return false;	
		}
		this.Retrieve(position - 1).Next = this.Retrieve(position + 1);
		size --;
		return true;
	}
	
	// Returns the edge leading to the specified index, or null if non-existant.
	public GraphEdge GetEdgeLeadingToIndex(int index){
		GraphEdge current = Head;
		while(current != null){
			if(current.ToIndex == index){
				return current;	
			}
			current = current.Next;
		}
		return null;
	}
}
