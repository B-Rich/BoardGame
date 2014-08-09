﻿using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	//public GameObject player;
	public GameObject GameBoardTile;
	public Sprite GameBoardSprite;
	public GameObject GameBoardBlockedTL;
	public GameObject GameBoardBlockedTR;
	public GameObject GameBoardBlockedL;
	public GameObject GameBoardBlockedR;
	public GameObject GameBoardBlockedBL;
	public GameObject GameBoardBlockedBR;
	public GameObject Town;

	float boardTileWidth;
	float boardTileHeight;
	float baselineX;
	float baselineY;
	public const int BOARD_HEIGHT = 50;
	public const int BOARD_WIDTH = 50;
	
	public enum BoardTileType {
		Empty,
		Town,
		Ally,
		Grass,
		Enemy
	}

	//For now I'll keep track of the type of tile separately
	//from the tile objects, mostly because I don't want to run 
	//a script on every single tile object
	BoardTileType[,] boardTileTypes;
	GameObject[,] boardTileObjects;

	// Use this for initialization
	void Start () {
		baselineX = GameBoardTile.transform.position.x;
		baselineY = GameBoardTile.transform.position.y;

		boardTileTypes = new BoardTileType[BOARD_HEIGHT, BOARD_WIDTH];
		boardTileObjects = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];
		for (int i = 0; i < BOARD_HEIGHT; i++) {
			for (int j = 0; j < BOARD_WIDTH; j++) {
				boardTileTypes [i, j] = BoardTileType.Empty;
			}
		}
		boardTileWidth = (GameBoardSprite.bounds.max - GameBoardSprite.bounds.min).x;
		boardTileHeight = (GameBoardSprite.bounds.max - GameBoardSprite.bounds.min).y;
	}

	public bool HasEnemies(int x, int y){
		if(boardTileTypes[y, x] == BoardTileType.Enemy){
			return true;
		}
		return false;
	}

	public Vector3 ComputePlayerPosition (int playerX, int playerY) {
		float x_correction;
		if(playerY % 2 == 0)
			x_correction = 0.5f;
		else
			x_correction = 0f;
		
		return (new Vector3(baselineX + (playerX - 25)*boardTileWidth + x_correction*boardTileWidth,
		                    baselineY + (playerY - 25)*boardTileHeight*.75f));
	}

	public bool UpdatePlayerPosition (Vector3 playerPosition, int playerX, int playerY) {
		if (boardTileTypes [playerY, playerX] == BoardTileType.Empty) {
			boardTileObjects[playerY,playerX] = InstantiateTileObject(playerPosition, playerY,playerX);
			//Randomly determine if there should be an enemy here. If there is, create one and make it
			//the child of the tile GameObject here.
			float rng = Random.value;
			if(rng < .3f){
				print ("Town boardtype");
				boardTileTypes[playerY, playerX] = BoardTileType.Town;
				InstantiateAndDisplay (boardTileObjects[playerY,playerX],Town, playerPosition);
			}
			else if(rng < .4f){
				print("Ally boardtype");
				boardTileTypes[playerY, playerX] = BoardTileType.Ally;
			}
			else if(rng < .9f){
				print ("Grass boardtype");
				boardTileTypes[playerY, playerX] = BoardTileType.Grass;
			}
			else{
				print ("Enemy boardtype");
				boardTileTypes[playerY, playerX] = BoardTileType.Enemy;
			}
		}
		return (true);
	}

	void InstantiateAndDisplay(GameObject parentTile, GameObject tile, Vector3 playerLoc){
		GameObject temp = Instantiate (tile, playerLoc, Quaternion.identity) as GameObject;
		temp.renderer.enabled = true;
		temp.transform.parent = parentTile.transform;
	}

	GameObject InstantiateTileObject(Vector3 playerPosition, int playerY, int playerX){
		GameObject tile;
		bool closedLeft = false;
		bool closedTopLeft = false;
		bool closedBottomLeft = false;
		bool closedRight = false;
		bool closedTopRight = false;
		bool closedBottomRight = false;

		if(playerX == 0){
			closedLeft = true;
			if(playerY % 2 == 1){
				closedBottomLeft = true;
				closedTopLeft = true;
			}
		}

		if(playerY == 0){
			closedBottomLeft = true;
			closedBottomRight = true;
		}

		if(playerX == BOARD_WIDTH - 1){
			closedRight = true;
			if(playerY % 2 == 0){
				closedTopRight = true;
				closedBottomRight = true;
			}
		}

		if(playerY == BOARD_HEIGHT - 1){
			closedTopLeft = true;
			closedTopRight = true;
		}
		tile = Instantiate (GameBoardTile, playerPosition, Quaternion.identity) as GameObject;
		if(closedTopLeft){
			InstantiateAndDisplay(tile, GameBoardBlockedTL, playerPosition);
		}
		if(closedTopRight){
			InstantiateAndDisplay (tile, GameBoardBlockedTR, playerPosition);
		}
		if(closedLeft){
			InstantiateAndDisplay (tile, GameBoardBlockedL, playerPosition);
		}
		if(closedRight){
			InstantiateAndDisplay (tile, GameBoardBlockedR, playerPosition);
		}
		if(closedBottomLeft){
			InstantiateAndDisplay (tile, GameBoardBlockedBL, playerPosition);
		}
		if(closedBottomRight){
			InstantiateAndDisplay (tile, GameBoardBlockedBR, playerPosition);
		}
		return (tile);
	}
}
