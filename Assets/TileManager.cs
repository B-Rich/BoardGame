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
	float baselineX;
	float baselineY;
	// Use this for initialization
	public enum BoardTileType {
		Empty,
		Town,
		Enemy
	}
	BoardTileType[,] boardTiles;
	GameObject[,] boardTileImages;

	void Start () {
		baselineX = GameBoardTile.transform.position.x;
		baselineY = GameBoardTile.transform.position.y;

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

	Vector3 updatedPlayerPosition (int playerX, int playerY) {
		float x_correction;
		if(playerY % 2 == 0)
			x_correction = 0.5f;
		else
			x_correction = 0f;

		return (new Vector3(baselineX + (playerX - 25)*boardTileWidth + x_correction*boardTileWidth,
		                    baselineY + (playerY - 25)*boardTileHeight*.75f));
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Q)) {
			playerX-= (playerY % 2);
			playerY++;
			player.transform.position = updatedPlayerPosition(playerX, playerY);
		}
		if (Input.GetKeyDown (KeyCode.E)) {
			playerX+= ((playerY+1) % 2);
			playerY++;
			player.transform.position = updatedPlayerPosition(playerX, playerY);
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			playerX--;
			player.transform.position = updatedPlayerPosition(playerX, playerY);
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			playerX++;
			player.transform.position = updatedPlayerPosition(playerX, playerY);
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			playerX-= (playerY % 2);
			playerY--;
			player.transform.position = updatedPlayerPosition(playerX, playerY);
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			playerX+= ((playerY+1) % 2);
			playerY--;
			player.transform.position = updatedPlayerPosition(playerX, playerY);
		}
		Vector3 playerLoc = player.transform.position;
		if (boardTiles [playerY, playerX] == BoardTileType.Empty) {
			boardTileImages[playerY,playerX] = Instantiate (GameBoardTile, playerLoc, Quaternion.identity) as GameObject;
			boardTiles[playerY, playerX] = BoardTileType.Town;
		}
	}
}
