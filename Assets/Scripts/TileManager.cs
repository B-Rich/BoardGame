using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	BoardTile blankTile;
	public GameObject GameBoardTile;
	public Sprite GameBoardSpriteDull;
	public Sprite GameBoardSpriteBright;
	public GameObject GameBoardBlockedTL;
	public GameObject GameBoardBlockedTR;
	public GameObject GameBoardBlockedL;
	public GameObject GameBoardBlockedR;
	public GameObject GameBoardBlockedBL;
	public GameObject GameBoardBlockedBR;
	public GameObject Imp;
	public GameObject Caster;
	public GameObject Ogre;
	public GameObject Player;
	XYPair highlightedPosition;
	int currentPlayer;
	int numPlayers = 2;
	int turn = 0;

	float boardTileWidth;
	float boardTileHeight;
	float baselineX;
	float baselineY;
	public const int BOARD_HEIGHT = 14;
	public const int BOARD_WIDTH = 14;

	MageController[] players;
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
				GetTile(i,j);
			}
		}
		highlightedPosition = new XYPair();
		highlightedPosition.x = BOARD_WIDTH / 2;
		highlightedPosition.y = BOARD_HEIGHT / 2;

		blankTile = (Instantiate (GameBoardTile, new Vector3(1000f,1000f), Quaternion.identity) as GameObject).GetComponent<BoardTile>();
		players = new MageController[4];
		playerLocations = new XYPair[4];
		currentPlayer = 0;
		for (int i = 0; i < numPlayers; i++){
			XYPair playerLocation = GetStartingPosition(i);
			playerLocations[i] = playerLocation;
			print("player " + i +" Location" + "x: " + playerLocation.x + "y: " + playerLocation.y);
			GameObject tempPlayer = Instantiate (Player, ComputePosition(playerLocation.x, playerLocation.y), Quaternion.identity) as GameObject;
			tempPlayer.SetActive (true);
			players[i] = tempPlayer.GetComponent<MageController>();
			if(i == 0)
				players[0].SetPlayerName("Aaron");
			players[i].PlayerID = i;
			players[i].Mana = 1;
		}
		PlayerInputHandler.GameState = PlayerInputHandler.GameStateType.PLAYING;
	}

	/*public static XYPair MoveNW(XYPair pair){
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
	}*/
	public XYPair GetStartingPosition(int playerID){
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
		//TODO: Fix up anywhere else that uses coordinates. Converting to y axes = diagonal up-right
		return (new Vector3(baselineX + (x - BOARD_WIDTH / 2f)*boardTileWidth + y*boardTileWidth*.5f,
		                    baselineY + (y - BOARD_HEIGHT / 2f)*boardTileHeight*.75f));
	}

	public XYPair ComputeXYFromPosition (Vector3 pos){
		XYPair pair = new XYPair();


		pair.y = (int) (BOARD_HEIGHT / 2 + ((pos.y - baselineY) / (boardTileHeight * .75f)) + .5f);


		pair.x = (int) (BOARD_WIDTH / 2 + 0.5f + (pos.x - baselineX)/boardTileWidth - 0.5f * pair.y);
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
		XYPair retval = start;
		if(start.x < end.x){
			retval.x = start.x + 1;
		}
		else if(start.x > end.x){
			retval.x = start.x - 1;
		}

		if(start.y < end.y){
			retval.y = start.y + 1;
		}
		else if(start.y > end.y){
			retval.y = end.y - 1;
		}
		return retval;
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

	public bool RegisterPlayer (int playerID, MageController player){
		print("In RegisterPlayer function");
		if(playerID < 0 || playerID > 3){
			return false;
		}
		numPlayers++;
		
		players[playerID] = player;
		playerLocations[playerID] = GetStartingPosition(playerID);
		player.gameObject.transform.position = ComputePosition(playerLocations[playerID].x, playerLocations[playerID].y);
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
		print ("Number of players is" + numPlayers);
		if((currentPlayer + 1) == numPlayers){
			turn++;
		}
		print ("Current player is " + currentPlayer);
		currentPlayer = (currentPlayer + 1) % numPlayers;
		print ("New player is " + currentPlayer);
		players[currentPlayer].GetComponent<MageController>().Mana = turn;
		print (players[currentPlayer].GetComponent<MageController>().GetPlayerName() + "'s turn");
		print (players[currentPlayer].GetComponent<MageController>().Mana + "amount of mana");
	}
	public Vector3 UpdatePlayerPosition (int playerID, XYPair pair) {
		if(playerID >= numPlayers || playerID < 0)
			print ("playerID is out of bounds");

		if(playerID != currentPlayer)
			return players[playerID].transform.position;
		playerLocations[playerID] = pair;
		players[playerID].transform.position = GetTilesAroundPoint (pair, 2)[6].transform.position;
		//AdvancePlayer ();
		return players[playerID].transform.position;
	}

	//TODO: This is just awful. You need to roll all this into the same logical place instead of spreading it between three classes
	public Vector3 TeleportPlayer (int playerID, XYPair pair){
		if(playerID != currentPlayer)
			return players[playerID].transform.position;
		playerLocations[playerID] = pair;
		return ComputePosition(pair.x, pair.y);
	}
	public XYPair GetPlayerPosition(int playerID){
		return playerLocations[playerID];
	}
	public BoardTile GetTileAtPoint(XYPair p){
		return boardTiles[p.x, p.y];
	}
	public BoardTile[] GetTilesAroundPoint (XYPair point, int radius){
		BoardTile[] retval = new BoardTile[((radius+1)*3)*(radius)+1];
		int i = 0;
		if(point.y % 2 == 0){
			retval[i++] = GetTile (point.x + 1, point.y + 1);
			retval[i++] = GetTile (point.x + 1, point.y - 1);
		}
		else {
			retval[i++] = GetTile (point.x - 1, point.y - 1);
			retval[i++] = GetTile (point.x - 1, point.y + 1);
		}
		
		retval[i++] = GetTile (point.x - 1, point.y);
		retval[i++] = GetTile (point.x + 1, point.y);
		
		retval[i++] = GetTile (point.x, point.y - 1);
		retval[i++] = GetTile (point.x, point.y + 1);
		retval[i++] = GetTile (point.x, point.y);
		return retval;
	}

	public void Summon(int playerID, XYPair coords, CreatureController.CreatureType crtype){
		GameObject creatureToSummon;
		switch(crtype){
		case CreatureController.CreatureType.Caster: 
			creatureToSummon = Caster;
			break;
		case CreatureController.CreatureType.Imp:
			creatureToSummon = Imp;
			break;
		case CreatureController.CreatureType.Ogre:
			creatureToSummon = Ogre;
			break;
		default:
			return;
		}
		GameObject temp = InstantiateAndDisplay (
			boardTiles[coords.x, coords.y].gameObject, 
			creatureToSummon, 
			ComputePosition (coords.x, coords.y));
		CreatureController cc = temp.GetComponent<CreatureController>();
		cc.Init (playerID, crtype);
		cc.Board = this;
		cc.Location = coords;
	}

	public BoardTile GetTile (int x, int y){
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

	public BoardTile GetTile(XYPair p){
		return GetTile (p.x, p.y);
	}

	GameObject InstantiateAndDisplay(GameObject parentTile, GameObject tile, Vector3 playerLoc){
		GameObject temp = Instantiate (tile, playerLoc, Quaternion.identity) as GameObject;
		temp.renderer.enabled = true;
		if(parentTile)
			temp.transform.parent = parentTile.transform;
		return temp;
	}

	public MageController GetCurrentPlayer(){
		return players[currentPlayer].GetComponent<MageController>();
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
