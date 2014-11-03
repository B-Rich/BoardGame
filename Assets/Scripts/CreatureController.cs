using UnityEngine;
using System.Collections;

public class CreatureController : MonoBehaviour {
	private int hp;
	private int ownerID;
	public TileManager Board;
	public TileManager.XYPair Location;

	public enum CreatureType {
		Imp,
		Caster,
		Ogre
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Init(int playerID, CreatureType type){
		ownerID = playerID;
		if(type == CreatureType.Imp){
			hp = 1;
		}
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if(ownerID == 0){
			spriteRenderer.color = Color.red;
		}
		if(ownerID == 1){
			spriteRenderer.color = Color.blue;
		}
	}
	
	void OnMouseDrag(){
		print ("Entering creature controller on mouse drag");
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
		print ("I am having my position reassigned");
		TileManager.XYPair mousePosPair = Board.ComputeXYFromPosition(gameObject.transform.position);
		Location = TileManager.PairAlongDirection(Location, mousePosPair);
		gameObject.transform.position = Board.GetTileAtPoint (Location).transform.position;
	}
	public void TakeDamage(int damage) {
		print ("I am taking damage");
		hp -= damage;
		if (hp <= 0){
			print ("I was destroyed");
			Destroy (gameObject);
		}
	}
}
