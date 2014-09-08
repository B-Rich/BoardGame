using UnityEngine;
using System.Collections;

public class BoardTile : MonoBehaviour {
	public enum TileType {
		Invisible,
		Grass
	}
	public TileType type;

	// Use this for initialization
	void Start () {
		//type = TileType.Invisible;
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
}
