using UnityEngine;
using System.Collections;

public class MageController : MonoBehaviour {
	public int hp;
	public char playerName;
	private int level;
	private int experience;
	public GameObject GameBoardObject;
	private TileManager board;
	private GameObject boardLocation;
	private int x = 25;
	private int y = 25;
	private int playerID = 0;

	// Use this for initialization
	void Start () {
		board = GameBoardObject.GetComponent<TileManager>();
		board.UpdatePlayerPosition(playerID, x, y);
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

	// Update is called once per frame
	void Update () {

		//Move player based on key press. Q = NW, E = NE
		//A = W, D = E, Z = SW, X = SE
		if(Input.GetKeyDown (KeyCode.Q) || 
		   Input.GetKeyDown (KeyCode.E) || 
		   Input.GetKeyDown (KeyCode.A) ||
		   Input.GetKeyDown (KeyCode.D) ||
		   Input.GetKeyDown (KeyCode.Z) ||
		   Input.GetKeyDown (KeyCode.X)) {
			if (Input.GetKey (KeyCode.Q)) {
				if(x > 0)
					x-= (y % 2);
				if(y < TileManager.BOARD_HEIGHT - 1)
					y++;
			}
			if (Input.GetKey (KeyCode.E)) {
				if(x < TileManager.BOARD_WIDTH - 1)
					x+= ((y+1) % 2);
				if(y < TileManager.BOARD_HEIGHT - 1)
					y++;
			}
			if (Input.GetKey (KeyCode.A)) {
				if(x > 0)
					x--;
			}
			if (Input.GetKey (KeyCode.D)) {
				if(x < TileManager.BOARD_WIDTH - 1)
					x++;
			}
			if (Input.GetKey (KeyCode.Z)) {
				if(x > 0)
					x-= (y % 2);
				if(y > 0)
					y--;
			}
			if (Input.GetKey (KeyCode.X)) {
				if(x < TileManager.BOARD_WIDTH - 1)
					x+= ((y+1) % 2);
				if(y > 0)
					y--;
			}
			transform.position = board.UpdatePlayerPosition (playerID, x, y);
			if(board.HasEnemies(x, y)){

			}
		}
	}
}
