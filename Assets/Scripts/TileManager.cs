﻿using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	private BoardTile blankTile;
	public GameObject GameBoardTile;
	public Sprite GameBoardSpriteDull;
	public Sprite GameBoardSpriteBright;
	public Sprite redGuy;
	public Sprite blueGuy;
	public GameObject GameBoardBlockedTL;
	public GameObject GameBoardBlockedTR;
	public GameObject GameBoardBlockedL;
	public GameObject GameBoardBlockedR;
	public GameObject GameBoardBlockedBL;
	public GameObject GameBoardBlockedBR;
	public GameObject Enemy;
	public Camera mainCamera;
	private XYPair highlightedPosition;
	private int currentPlayer;
	private int numPlayers;

	float boardTileWidth;
	float boardTileHeight;
	float baselineX;
	float baselineY;
	public const int BOARD_HEIGHT = 14;
	public const int BOARD_WIDTH = 14;

	GameObject[] players;
	XYPair[] playerLocations;

	public struct XYPair {
		public int x;
		public int y;
	}
	
	BoardTile[,] boardTiles;

	// Use this for initialization
	void Start () {
		baselineX = GameBoardTile.transform.position.x;
		baselineY = GameBoardTile.transform.position.y;

		boardTiles = new BoardTile[BOARD_HEIGHT, BOARD_WIDTH];

		boardTileWidth = (GameBoardSpriteDull.bounds.max - GameBoardSpriteDull.bounds.min).x;
		boardTileHeight = (GameBoardSpriteDull.bounds.max - GameBoardSpriteDull.bounds.min).y;
		//Create starting tile
		for (int i = 0; i < BOARD_WIDTH; i++){
			for(int j = 0; j < BOARD_HEIGHT; j++){
				CreateTile(i,j);
			}
		}
		highlightedPosition = new XYPair();
		highlightedPosition.x = BOARD_WIDTH / 2;
		highlightedPosition.y = BOARD_HEIGHT / 2;

		blankTile = (Instantiate (GameBoardTile, new Vector3(0f,0f), Quaternion.identity) as GameObject).GetComponent<BoardTile>();
		players = new GameObject[4];
		playerLocations = new XYPair[4];
		currentPlayer = 0;
	}

	public static XYPair MoveNW(XYPair pair){
		XYPair retval = pair;
		if(pair.x > 0)
			retval.x = pair.x - (pair.y % 2);
		if(pair.y < TileManager.BOARD_HEIGHT - 1)
			retval.y = pair.y + 1;
		return retval;
	}

	public static XYPair MoveNE(XYPair pair){
		XYPair retval = pair;
		if(pair.x < TileManager.BOARD_WIDTH - 1)
			retval.x = pair.x + ((pair.y+1) % 2);
		if(pair.y < TileManager.BOARD_HEIGHT - 1)
			retval.y = pair.y + 1;
		return retval;
	}

	public static XYPair MoveW(XYPair pair){
		XYPair retval = pair;
		if(pair.x > 0)
			retval.x = pair.x - 1;
		return retval;
	}

	public static XYPair MoveE(XYPair pair){
		XYPair retval = pair;
		if(pair.x < TileManager.BOARD_WIDTH - 1)
			retval.x = pair.x + 1;
		return retval;
	}
	public static XYPair MoveSW(XYPair pair){
		XYPair retval = pair;
		if(pair.x > 0)
			retval.x = pair.x - (pair.y % 2);
		if(pair.y > 0)
			retval.y = pair.y - 1;
		return retval;
	}
	public static XYPair MoveSE(XYPair pair){
		XYPair retval = pair;
		if(pair.x < TileManager.BOARD_WIDTH - 1)
			retval.x = pair.x + ((pair.y+1) % 2);
		if(pair.y > 0)
			retval.y = pair.y - 1;
		return retval;
	}
	public XYPair SetStartingPosition(int playerID){
		XYPair retval;
		retval.x = BOARD_WIDTH / 2;
		if(playerID == 0){
			retval.y = BOARD_HEIGHT - 1;
		}
		else if(playerID == 1){
			retval.y = 0;
		}
		else{
			retval.y = -1;
		}
		return retval;
	}
	public Vector3 ComputePosition (int x, int y) {
		float x_correction;
		if(y % 2 == 0)
			x_correction = 0.5f;
		else
			x_correction = 0f;
		
		return (new Vector3(baselineX + (x - BOARD_WIDTH / 2f)*boardTileWidth + x_correction*boardTileWidth,
		                    baselineY + (y - BOARD_HEIGHT / 2f)*boardTileHeight*.75f));
	}

	public XYPair ComputeXYFromPosition (Vector3 pos){
		XYPair pair = new XYPair();
		pair.y = (int) (BOARD_HEIGHT / 2 + ((pos.y - baselineY) / (boardTileHeight * .75f)) + .5f);
		
		float x_correction;
		if(pair.y % 2 == 0)
			x_correction = 0.5f;
		else
			x_correction = 0f;

		pair.x = (int) (BOARD_WIDTH / 2 + 0.5f - x_correction + (pos.x - baselineX)/boardTileWidth);
		if(pair.x < 0){
			pair.x = 0;
		}
		else if(pair.x >= BOARD_WIDTH){
			pair.x = BOARD_WIDTH - 1;
		}
		if(pair.y < 0){
			pair.y = 0;
		}
		else if(pair.y >= BOARD_HEIGHT){
			pair.y = BOARD_HEIGHT - 1;
		}
		return pair;
	}
	public void Update(){
		HighlightMousePosition();
	}

	//Returns an XYPair one step in the direction of end, starting from start on the hex grid
	public static XYPair PairAlongDirection(XYPair start, XYPair end){
		if(start.x == end.x && start.y == end.y){
			return start;
		}

		//equal y value means x moves left or right one space
		if(start.y == end.y){
			if(start.x > end.x){
				return MoveW(start);
			}
			else{
				return MoveE (start);
			}
		}

		if(start.y < end.y){
			if(start.y % 2 == 0){
				if(end.x <= start.x){
					return (MoveNW (start));
				}
				return (MoveNE (start));
			}
			if(end.x < start.x){
				return (MoveNW (start));
			}
			return (MoveNE (start));
		}
		if(start.y % 2 == 0){
			if(end.x <= start.x){
				return (MoveSW (start));
			}
			return (MoveSE (start));
		}
		if(end.x < start.x){
			return (MoveSW (start));
		}
		return (MoveSE (start));
	}

	public void HighlightMousePosition (){
		Vector3 positionToCheck = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		XYPair pair = ComputeXYFromPosition (positionToCheck);
		if(pair.x != highlightedPosition.x || pair.y != highlightedPosition.y){
			SpriteRenderer tileRenderer;
			if(boardTiles[pair.x, pair.y] != null)
				tileRenderer = boardTiles[pair.x, pair.y].GetComponent<SpriteRenderer>();
			else
				return;

			tileRenderer.sprite = GameBoardSpriteBright;
			if(boardTiles[highlightedPosition.x, highlightedPosition.y] != null)
				tileRenderer = boardTiles[highlightedPosition.x, highlightedPosition.y].GetComponent<SpriteRenderer>();
			tileRenderer.sprite = GameBoardSpriteDull;
			highlightedPosition = pair;
		}
	}

	public bool RegisterPlayer (int playerID, GameObject player){
		if(playerID < 0 || playerID > 3){
			return false;
		}
		numPlayers++;
		
		players[playerID] = player;
		playerLocations[playerID] = ComputeXYFromPosition(player.transform.position);
		return true;
	}
	
	public int GetCurrentPlayerID(){
		return currentPlayer;
	}
	
	public Vector3 UpdatePlayerPosition (int playerID, Vector3 p){
		XYPair pair = ComputeXYFromPosition (p);
		
		return UpdatePlayerPosition (playerID, pair);
	}
	public void AdvancePlayer(){
		print ("advancing player");
		currentPlayer = (currentPlayer + 1) % numPlayers;
	}
	public Vector3 UpdatePlayerPosition (int playerID, XYPair pair) {
		if(playerID >= numPlayers || playerID < 0)
			print ("playerID is out of bounds");

		if(playerID != currentPlayer)
			return players[playerID].transform.position;
		AdvancePlayer ();
		return GetTilesAroundPoint (pair, 2)[6].transform.position;
	}

	public BoardTile[] GetTilesAroundPoint (XYPair point, int radius){
		BoardTile[] retval = new BoardTile[((radius+1)*3)*(radius)+1];
		int i = 0;
		if(point.y % 2 == 0){
			retval[i++] = CreateTile (point.x + 1, point.y + 1);
			retval[i++] = CreateTile (point.x + 1, point.y - 1);
		}
		else {
			retval[i++] = CreateTile (point.x - 1, point.y - 1);
			retval[i++] = CreateTile (point.x - 1, point.y + 1);
		}
		
		retval[i++] = CreateTile (point.x - 1, point.y);
		retval[i++] = CreateTile (point.x + 1, point.y);
		
		retval[i++] = CreateTile (point.x, point.y - 1);
		retval[i++] = CreateTile (point.x, point.y + 1);
		retval[i++] = CreateTile (point.x, point.y);
		return retval;
	}

	public void SummonImp(int playerID, XYPair coords){
		GameObject temp = InstantiateAndDisplay (
			boardTiles[coords.x, coords.y].gameObject, 
			Enemy, 
			ComputePosition (coords.x, coords.y));
		CreatureController cc = temp.GetComponent<CreatureController>();
		cc.Init (playerID, CreatureController.CreatureType.Imp);
	}

	BoardTile CreateTile (int x, int y){
		if(x  >= BOARD_WIDTH || x < 0 || y >= BOARD_HEIGHT || y < 0){
			return blankTile;
		}
		//Only check within bounds of board matrices
		if(x >= 0 && y >= 0 && x < BOARD_WIDTH && y < BOARD_HEIGHT){
			if (!boardTiles [x, y]) {
				boardTiles[x, y] = InstantiateTileObject(x, y);
			}
		}
		return boardTiles[x,y];
	}

	GameObject InstantiateAndDisplay(GameObject parentTile, GameObject tile, Vector3 playerLoc){
		GameObject temp = Instantiate (tile, playerLoc, Quaternion.identity) as GameObject;
		temp.renderer.enabled = true;
		if(parentTile)
			temp.transform.parent = parentTile.transform;
		return temp;
	}

	BoardTile InstantiateTileObject(int x, int y){
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
			InstantiateAndDisplay (null, GameBoardBlockedTL, playerPosition);
		}
		if(closedTopRight){
			InstantiateAndDisplay (null, GameBoardBlockedTR, playerPosition);
		}
		if(closedLeft){
			InstantiateAndDisplay (null, GameBoardBlockedL, playerPosition);
		}
		if(closedRight){
			InstantiateAndDisplay (null, GameBoardBlockedR, playerPosition);
		}
		if(closedBottomLeft){
			InstantiateAndDisplay (null, GameBoardBlockedBL, playerPosition);
		}
		if(closedBottomRight){
			InstantiateAndDisplay (null, GameBoardBlockedBR, playerPosition);
		}
		return (tile.GetComponent<BoardTile>());
	}
}