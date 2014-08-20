﻿using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	//public GameObject player;
	public GameObject GameBoardTile;
	public Sprite GameBoardSpriteDull;
	public Sprite GameBoardSpriteBright;
	public GameObject GameBoardBlockedTL;
	public GameObject GameBoardBlockedTR;
	public GameObject GameBoardBlockedL;
	public GameObject GameBoardBlockedR;
	public GameObject GameBoardBlockedBL;
	public GameObject GameBoardBlockedBR;
	public GameObject Town;
	public GameObject Ally;
	public GameObject Enemy;
	public Camera mainCamera;
	private SmoothFollow cameraMovementScript;
	private XYPair highlightedPosition;

	float boardTileWidth;
	float boardTileHeight;
	float baselineX;
	float baselineY;
	public const int BOARD_HEIGHT = 50;
	public const int BOARD_WIDTH = 50;
	
	public enum BoardTileType {
		Invisible,
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
				boardTileTypes [i, j] = BoardTileType.Invisible;
			}
		}
		boardTileWidth = (GameBoardSpriteDull.bounds.max - GameBoardSpriteDull.bounds.min).x;
		boardTileHeight = (GameBoardSpriteDull.bounds.max - GameBoardSpriteDull.bounds.min).y;
		//Create starting tile
		CreateTile(25,25);
		cameraMovementScript = mainCamera.GetComponent<SmoothFollow>();
		highlightedPosition = new XYPair();
		highlightedPosition.x = 25;
		highlightedPosition.y = 25;
	}

	public bool HasEnemies(int x, int y){
		if(boardTileTypes[y, x] == BoardTileType.Enemy){
			return true;
		}
		return false;
	}

	public Vector3 ComputePosition (int x, int y) {
		float x_correction;
		if(y % 2 == 0)
			x_correction = 0.5f;
		else
			x_correction = 0f;
		
		return (new Vector3(baselineX + (x - 25)*boardTileWidth + x_correction*boardTileWidth,
		                    baselineY + (y - 25)*boardTileHeight*.75f));
	}

	public Vector3 UpdatePlayerPosition (int playerID, Vector3 p){
		//xV = baselineX + (x - 25)* boardTileWidth + x_correction*boardTileWidth;
		//xV - baselineX = x * boardTileWidth - 25 * boardTileWidth + x_correction * boardTileWidth;
		//xV - x * boardTileWidth = -25 * boardTileWidth + x_correction * boardTileWidth + baselineX;
		//-x * boardTileWidth = -xV -25 * boardTileWidth + x_correction * boardTileWidth + baselineX;
		//x = xV / boardTileWidth + 25 - x_correction - baselineX / boardTileWidth
		//x = 25 - x_correction + (xV - baselineX)/boardTileWidth;

		//yV = y * boardTileHeight - 25 * boardTileHeight * .75f + baselineY
		//yV -y * boardTileHeight = -25 * boardTileHeight * .75f + baselineY
		//-y * boardTileHeight = -yV - 25 * boardTileHeight * .75f + baselineY
		//-y = -yV / boardTileHeight - 25* .75f + baselineY / boardTileHeight
		//y = yV / boardTileHeight + 25 * .75f - baselineY / boardTileHeight
		//y = 25 * .75f + (yV - baselineY) / boardTileHeight
		XYPair pair = ComputeXYFromPosition (p);

		/*print ("x_correction = " + x_correction);
		print("p.x = " + p.x);
		print ("baselineX = " + baselineX);
		print ("boardTileWidth = " +boardTileWidth);
		print("p.y = " + p.y);
		print ("baselineY = " + baselineY);
		print ("boardTileHeight = " +boardTileHeight);

		print("x = " + x);
		print("y = " + y);*/
		return UpdatePlayerPosition (playerID, pair.x, pair.y);
	}
	public struct XYPair {
		public int x;
		public int y;
	}
	private XYPair ComputeXYFromPosition (Vector3 pos){
		XYPair pair = new XYPair();
		pair.y = (int) (25 + ((pos.y - baselineY) / (boardTileHeight * .75f)) + .5f);
		
		float x_correction;
		if(pair.y % 2 == 0)
			x_correction = 0.5f;
		else
			x_correction = 0f;

		pair.x = (int) (25 + 0.5f - x_correction + (pos.x - baselineX)/boardTileWidth);
		return pair;
	}
	public void Update(){
		HighlightMousePosition();
	}

	public void HighlightMousePosition (){
		Vector3 positionToCheck = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		XYPair pair = ComputeXYFromPosition (positionToCheck);
		if(pair.x != highlightedPosition.x || pair.y != highlightedPosition.y){
			print("Highlighting mouse position");
			SpriteRenderer tileRenderer;
			if(boardTileObjects[pair.x, pair.y] != null)
				tileRenderer = boardTileObjects[pair.x, pair.y].GetComponent<SpriteRenderer>();
			else
				return;

			tileRenderer.sprite = GameBoardSpriteBright;
			if(boardTileObjects[highlightedPosition.x, highlightedPosition.y] != null)
				tileRenderer = boardTileObjects[highlightedPosition.x, highlightedPosition.y].GetComponent<SpriteRenderer>();
			tileRenderer.sprite = GameBoardSpriteDull;
			highlightedPosition = pair;
		}
	}

	public Vector3 UpdatePlayerPosition (int playerID, int x, int y) {
		//Extra tiles go here?
		if(y % 2 == 0){
			CreateTile (x + 1, y + 1);
			CreateTile (x + 1, y - 1);
		}
		else {
			CreateTile (x - 1, y - 1);
			CreateTile (x - 1, y + 1);
		}

		CreateTile (x - 1, y);
		CreateTile (x + 1, y);

		CreateTile (x, y - 1);
		CreateTile (x, y + 1);

		cameraMovementScript.target = boardTileObjects[x,y].transform;
		return CreateTile (x, y);
	}

	Vector3 CreateTile (int x, int y){
		Vector3 tilePosition = ComputePosition(x, y);

		//Only check within bounds of board matrices
		if(x >= 0 && y >= 0 && x < BOARD_WIDTH && y < BOARD_HEIGHT){
			if (boardTileTypes [x, y] == BoardTileType.Invisible) {
				boardTileObjects[x, y] = InstantiateTileObject(x, y);
				//Determine which extra crap goes on this tile (ally, town, enemy, or grass for now)
				float rng = Random.value;
				if(rng < .3f){
					print ("Town boardtype");
					boardTileTypes[x, y] = BoardTileType.Town;
					InstantiateAndDisplay (boardTileObjects[x, y],Town, tilePosition);
				}
				else if(rng < .4f){
					print("Ally boardtype");
					boardTileTypes[x, y] = BoardTileType.Ally;
					InstantiateAndDisplay (boardTileObjects[x, y],Ally, tilePosition);
				}
				else if(rng < .9f){
					print ("Grass boardtype");
					boardTileTypes[x, y] = BoardTileType.Grass;
				}
				else{
					print ("Enemy boardtype");
					boardTileTypes[x, y] = BoardTileType.Enemy;
					InstantiateAndDisplay (boardTileObjects[x, y],Enemy, tilePosition);
				}
			}
		}
		return tilePosition;
	}

	void InstantiateAndDisplay(GameObject parentTile, GameObject tile, Vector3 playerLoc){
		GameObject temp = Instantiate (tile, playerLoc, Quaternion.identity) as GameObject;
		temp.renderer.enabled = true;
		temp.transform.parent = parentTile.transform;
	}

	GameObject InstantiateTileObject(int x, int y){
		Vector3 playerPosition = ComputePosition (x, y);
		GameObject tile;
		bool closedLeft = false;
		bool closedTopLeft = false;
		bool closedBottomLeft = false;
		bool closedRight = false;
		bool closedTopRight = false;
		bool closedBottomRight = false;

		if(x == 0){
			closedLeft = true;
			if(y % 2 == 1){
				closedBottomLeft = true;
				closedTopLeft = true;
			}
		}

		if(y == 0){
			closedBottomLeft = true;
			closedBottomRight = true;
		}

		if(x == BOARD_WIDTH - 1){
			closedRight = true;
			if(y % 2 == 0){
				closedTopRight = true;
				closedBottomRight = true;
			}
		}

		if(y == BOARD_HEIGHT - 1){
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
