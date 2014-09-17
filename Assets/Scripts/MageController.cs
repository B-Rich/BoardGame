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
	SpellHandler.SpellType[] hand;

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
				//if(Input.GetMouseButton (0))
					//caster.CastSpell(playerID, board.ComputeXYFromPosition(positionToCheck), SpellHandler.SpellType.FIREBALL);
				//else
					//caster.CastSpell(playerID, board.ComputeXYFromPosition(positionToCheck), SpellHandler.SpellType.IMP);
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
