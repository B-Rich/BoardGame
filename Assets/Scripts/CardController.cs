using UnityEngine;
using System.Collections;

public class CardController : MonoBehaviour {
	TileManager Board;
	int ownerID;

	// Use this for initialization
	void Start () {
		Board = GameObject.Find("GameBoard").GetComponent<TileManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Init(int playerID){
		ownerID = playerID;
	}
	void OnMouseDrag(){
		int playerID = Board.GetCurrentPlayerID();
		if(playerID == ownerID){
			print ("I AM BEING DRAGGED. HELP!");
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0f;
			gameObject.transform.position = pos;
		}
		else{
			print ("I am not being dragged");
		}
	}
	void OnMouseUpAsButton(){
		/*print ("I am having my position reassigned");
		TileManager.XYPair mousePosPair = Board.ComputeXYFromPosition(gameObject.transform.position);
		Location = TileManager.PairAlongDirection(Location, mousePosPair);
		gameObject.transform.position = Board.GetTileAtPoint (Location).transform.position;*/
	}
}
