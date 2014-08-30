using UnityEngine;
using System.Collections;

public class BoardTile : MonoBehaviour {
	public enum TileType {
		Invisible,
		Town,
		Ally,
		Grass,
		Enemy
	}
	public TileType type;

	// Use this for initialization
	void Start () {
		//type = TileType.Invisible;
	}

	void Awake() {
		//this.gameObject.SetActive (false);
	}
}
