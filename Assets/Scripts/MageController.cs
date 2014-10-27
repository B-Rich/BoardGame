using UnityEngine;
using System.Collections;
using System.IO;

public class MageController : MonoBehaviour {
	public int HP;
	public string PlayerName;
	private int level;
	private int experience;
	public GameObject GameBoardObject;
	private TileManager board;
	public int PlayerID;
	public int Mana = 0;
	public PlayerInputHandler.SpellType[] Hand;
	private PlayerInputHandler.SpellType[] Deck;
	int NumberOfCards = 0;

	// Use this for initialization
	void Start () {
		board = GameBoardObject.GetComponent<TileManager>();
		print ("Adding player");
		if(!board.RegisterPlayer(PlayerID, gameObject)){
			print ("Invalid player id");
		}
		print ("Player name is " + PlayerName);
		Hand = new PlayerInputHandler.SpellType[5];
		Deck = new PlayerInputHandler.SpellType[30];
		
		AddCardsToHandFromPrefs("Imp", PlayerInputHandler.SpellType.IMP);
		AddCardsToHandFromPrefs("Caster", PlayerInputHandler.SpellType.CASTER);
		AddCardsToHandFromPrefs("Fireball", PlayerInputHandler.SpellType.FIREBALL);
		AddCardsToHandFromPrefs("Teleport", PlayerInputHandler.SpellType.TELEPORT);
		AddCardsToHandFromPrefs("Heal", PlayerInputHandler.SpellType.HEAL);
		AddCardsToHandFromPrefs("Ogre", PlayerInputHandler.SpellType.OGRE);
		for(int i = 0; i < 5; i++){
			Hand[i] = Deck[i];
		}
	}

	void AddCardsToHandFromPrefs(string CardName, PlayerInputHandler.SpellType spellType){
		print ("Adding cards for player named " + PlayerName);
		int cardsOfType = PlayerPrefs.GetInt (PlayerName + CardName);
		for(int i = NumberOfCards; i < NumberOfCards + cardsOfType; i++){
			Deck[i] = spellType;
		}
		NumberOfCards += cardsOfType;
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
