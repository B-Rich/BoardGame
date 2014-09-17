using UnityEngine;
using System.Collections;

public class BoardTile : MonoBehaviour {
	public enum TileType {
		Invisible,
		Grass
	}
	public TileType type;
	//TileManager Board;

	// Use this for initialization
	void Start () {
		//type = TileType.Invisible;
		//Board = GameObject.Find("GameBoard").GetComponent<TileManager>();
	}

	void Awake() {
		//this.gameObject.SetActive (false);
	}
	public void PropegateDamage(int damage){
		CreatureController[] creatures = GetComponentsInChildren<CreatureController>();
		foreach (CreatureController creature in creatures){
			creature.TakeDamage(damage);
		}
	}
	public void Summon(int playerID, CreatureController.CreatureType crType){

	}
}
