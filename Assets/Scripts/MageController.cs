using UnityEngine;
using System.Collections;

public class MageController : MonoBehaviour {
	public int hp;
	public char playerName;
	private int level;
	private int experience;
	public GameObject GameBoardObject;
	private TileManager board;
	private TileManager.XYPair coords;
	public int playerID;
	private bool boundToMouse = false;

	// Use this for initialization
	void Start () {
		board = GameBoardObject.GetComponent<TileManager>();
		coords = new TileManager.XYPair();
		coords.x = TileManager.BOARD_WIDTH / 2;
		coords.y = TileManager.BOARD_HEIGHT / 2;
		if(!board.RegisterPlayer(playerID, gameObject)){
			print ("Invalid player id");
		}
		coords = board.SetStartingPosition(playerID);
		transform.position = board.ComputePosition(coords.x, coords.y);
	}

	//Blows up all objects on tiles around coords
	public void CastSpell(TileManager.XYPair coords, int radius){
		BoardTile[] tiles = board.GetTilesAroundPoint(coords, radius);

		foreach (BoardTile tile in tiles){
			if(tile.type != BoardTile.TileType.Grass && tile.type != BoardTile.TileType.Invisible){
				tile.type = BoardTile.TileType.Grass;
				Destroy(tile.gameObject.transform.GetChild(0).gameObject);
			}
		}
	}

	public bool TakeDamage(int dmg){
		hp -= dmg;
		if (hp < 0){
			hp = 0;
			return true;
		}
		else
			return false;
	}

	void HandlePlayerInput(){
		if(boundToMouse){
			if(Input.GetMouseButton(0)){
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0f;
				transform.position = pos;
			}
			else {
				TileManager.XYPair mousePosPair = board.ComputeXYFromPosition(transform.position);
				coords = TileManager.PairAlongDirection(coords, mousePosPair);
				transform.position = board.UpdatePlayerPosition(playerID, coords);
				boundToMouse = false;
			}
		}
		
		//Move player based on key press. Q = NW, E = NE
		//A = W, D = E, Z = SW, X = SE
		if(Input.GetKeyDown (KeyCode.Q) || 
		   Input.GetKeyDown (KeyCode.E) || 
		   Input.GetKeyDown (KeyCode.A) ||
		   Input.GetKeyDown (KeyCode.D) ||
		   Input.GetKeyDown (KeyCode.Z) ||
		   Input.GetKeyDown (KeyCode.X)) {
			if (Input.GetKey (KeyCode.Q)) {
				coords = TileManager.MoveNW(coords);
				/*if(x > 0)
					x-= (y % 2);
				if(y < TileManager.BOARD_HEIGHT - 1)
					y++;*/
			}
			if (Input.GetKey (KeyCode.E)) {
				coords = TileManager.MoveNE(coords);
				/*if(x < TileManager.BOARD_WIDTH - 1)
					x+= ((y+1) % 2);
				if(y < TileManager.BOARD_HEIGHT - 1)
					y++;*/
			}
			if (Input.GetKey (KeyCode.A)) {
				coords = TileManager.MoveW(coords);
				/*if(x > 0)
					x--;*/
			}
			if (Input.GetKey (KeyCode.D)) {
				coords = TileManager.MoveE (coords);
				/*if(x < TileManager.BOARD_WIDTH - 1)
					x++;*/
			}
			if (Input.GetKey (KeyCode.Z)) {
				coords = TileManager.MoveSW(coords);
				/*if(x > 0)
					x-= (y % 2);
				if(y > 0)
					y--;*/
			}
			if (Input.GetKey (KeyCode.X)) {
				coords = TileManager.MoveSE (coords);
				/*if(x < TileManager.BOARD_WIDTH - 1)
					x+= ((y+1) % 2);
				if(y > 0)
					y--;*/
			}
			transform.position = board.UpdatePlayerPosition (playerID, coords);
			if(board.HasEnemies(coords.x, coords.y)){
				
			}
		}
		else if(Input.GetMouseButtonDown(0)){
			Vector3 positionToCheck = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			positionToCheck.z=0f;
			if(renderer.bounds.Contains (positionToCheck)){
				boundToMouse = true;
			}
			else {
				CastSpell(board.ComputeXYFromPosition(positionToCheck), 1);
			}
		}
		else if(Input.GetKeyDown (KeyCode.Space)){
			Vector3 mainCamPos = transform.position;
			mainCamPos.z -= 10f;
			Camera.main.transform.position = mainCamPos;
		}
	}

	// Update is called once per frame
	void Update () {
		if(board.GetCurrentPlayerID() == playerID){
			HandlePlayerInput();
		}
	}
}
