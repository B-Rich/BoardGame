using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	public GameObject player;
	public GameObject GameBoardTile;
	public Sprite GameBoardSprite;
	public Camera mainCamera;

	float boardTileWidth;
	float boardTileHeight;
	int playerX = 25;
	int playerY = 25;
	// Use this for initialization
	public enum BoardTileType {
		Empty,
		Town,
		Enemy
	}
	BoardTileType[,] boardTiles;
	GameObject[,] boardTileImages;

	void Start () {
		boardTiles = new BoardTileType[50, 50];
		boardTileImages = new GameObject[50, 50];
		for (int i = 0; i < 50; i++) {
			for (int j = 0; j < 50; j++) {
				boardTiles [i, j] = BoardTileType.Empty;
			}
		}
		boardTileWidth = (GameBoardSprite.bounds.max - GameBoardSprite.bounds.min).x;
		boardTileHeight = (GameBoardSprite.bounds.max - GameBoardSprite.bounds.min).y;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			player.transform.Translate(Vector3.up * boardTileHeight, Space.World);
			playerY++;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			player.transform.Translate(Vector3.down * boardTileHeight, Space.World);
			playerY--;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			player.transform.Translate(Vector3.left * boardTileWidth, Space.World);
			playerX--;
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			player.transform.Translate(Vector3.right * boardTileWidth, Space.World);
			playerX++;
		}
		Vector3 playerLoc = player.transform.position;
		if (boardTiles [playerY, playerX] == BoardTileType.Empty) {
			boardTileImages[playerY,playerX] = Instantiate (GameBoardTile, playerLoc, Quaternion.identity) as GameObject;
			boardTiles[playerY, playerX] = BoardTileType.Town;
		}
	}
}
