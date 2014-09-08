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

	public enum SpellType{
		FIREBALL,
		SUMMON_IMP
	}

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
	public void CastSpell(TileManager.XYPair coords, SpellType spell){
		if(spell == SpellType.FIREBALL){
			//For now fireball affects a cluster of 7 tiles (e.g. radius = 1)
			BoardTile[] tiles = board.GetTilesAroundPoint(coords, 1);

			//Fireball deals 5 damage
			foreach (BoardTile tile in tiles){
				tile.PropegateDamage(5);
			}
		}
		else if(spell == SpellType.SUMMON_IMP){
			board.SummonImp(playerID, coords);
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
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown (1)){
			Vector3 positionToCheck = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			positionToCheck.z=0f;
			if(renderer.bounds.Contains (positionToCheck)){
				boundToMouse = true;
			}
			else {
				if(Input.GetMouseButton (0))
					CastSpell(board.ComputeXYFromPosition(positionToCheck), SpellType.FIREBALL);
				else
					CastSpell(board.ComputeXYFromPosition(positionToCheck), SpellType.SUMMON_IMP);
				board.AdvancePlayer();
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
