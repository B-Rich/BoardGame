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
	public int mana = 0;
	public PlayerInputHandler.SpellType[] hand;

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
		hand = new PlayerInputHandler.SpellType[5];
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
		if(board.GetCurrentPlayerID() == playerID){
			HandlePlayerInput();
		}
	}
}
