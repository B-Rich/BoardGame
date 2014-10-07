using UnityEngine;
using System.Collections;

public class MageController : MonoBehaviour {
	public int HP;
	public char PlayerName;
	private int level;
	private int experience;
	public GameObject GameBoardObject;
	private TileManager board;
	public int PlayerID;
	public int Mana = 0;
	public PlayerInputHandler.SpellType[] Hand;
	private PlayerInputHandler.SpellType[] deck;

	// Use this for initialization
	void Start () {
		board = GameBoardObject.GetComponent<TileManager>();
		if(!board.RegisterPlayer(PlayerID, gameObject)){
			print ("Invalid player id");
		}
		Hand = new PlayerInputHandler.SpellType[5];
		deck = new PlayerInputHandler.SpellType[30];
		for(int i = 0; i < 30; i++){
			deck[i] = PlayerInputHandler.SpellType.IMP;
		}
		for(int i = 0; i < 5; i++){
			Hand[i] = deck[i];
		}
	}

	public bool TakeDamage(int dmg){
		HP -= dmg;
		if (HP < 0){
			HP = 0;
			return true;
		}
		else
			return false;
	}
}
