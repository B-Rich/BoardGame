using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {
	TileManager Board;
	const int CARDWIDTH = 100;
	const int CARDHEIGHT = 100;
	const float HANDPOSFACTOR_X = 1.4f;
	const float HANDPOSFACTOR_Y = 1.1f;
	bool CreatingDeck = false;
	string PlayerName = "";
	public enum SpellType{
		NONE,
		FIREBALL,
		IMP,
		OGRE,
		CASTER,
		TELEPORT,
		HEAL
	}
	public enum GameStateType{
		ESCMENU,
		PLAYING,
		INIT
	}
	public static GameStateType GameState = GameStateType.INIT;

	SpellType CurrentSpell = SpellType.NONE;

	public bool DisplayCard(int x, string cardName){
		if(GUI.Button (new Rect(Screen.width / HANDPOSFACTOR_X - CARDWIDTH*(5-x), 
		                        Screen.height / HANDPOSFACTOR_Y - CARDHEIGHT, 
		                        CARDWIDTH, CARDHEIGHT), cardName)){
			return true;
		}
		return false;
	}

	private void SetCardForPlayer(string playerName, string card){
		int numCards = PlayerPrefs.GetInt(playerName + card);
		PlayerPrefs.SetInt (playerName + card, numCards+1);
		print (card + numCards+1 + " for " + playerName + "added");
	}

	private void RemoveCardForPlayer(string playerName, string card){
		int numCards = PlayerPrefs.GetInt(playerName + card);
		if(numCards > 0){
			PlayerPrefs.SetInt (playerName + card, numCards-1);
			print (card + numCards + " for " + playerName + "removed");
		}
		else{
			print(card + " for " + playerName + "did not exist");
		}
	}

	public void OnGUI(){
		switch(GameState){
		case GameStateType.ESCMENU:
			//print ("Escape menu is active");
			if(GUI.Button (new Rect (Screen.width/2 - 120, Screen.height/4 - 20, 120, 20), "Create Your Deck")){
				CreatingDeck = !CreatingDeck;
			}
			if(CreatingDeck){
				int cardAddHorizPos = Screen.width / 4;
				int cardAddVertStartingPos = Screen.height / 6;
				PlayerName = GUI.TextField(new Rect (cardAddHorizPos, cardAddVertStartingPos - 20, 200, 20), PlayerName, 25);
				if (GUI.Button(new Rect(cardAddHorizPos, cardAddVertStartingPos, 60, 20), "Imp"))
					SetCardForPlayer(PlayerName, "Imp");
				if (GUI.Button(new Rect(cardAddHorizPos + 60, cardAddVertStartingPos, 60, 20), "Imp"))
					RemoveCardForPlayer(PlayerName, "Imp");

				if (GUI.Button(new Rect(cardAddHorizPos, cardAddVertStartingPos + 20, 60, 20), "Caster"))
					SetCardForPlayer(PlayerName, "Caster");
				if (GUI.Button(new Rect(cardAddHorizPos + 60, cardAddVertStartingPos + 20, 60, 20), "Caster"))
					RemoveCardForPlayer(PlayerName, "Caster");

				if (GUI.Button(new Rect(cardAddHorizPos, cardAddVertStartingPos + 40, 60, 20), "Ogre"))
					SetCardForPlayer(PlayerName, "Ogre");
				if (GUI.Button(new Rect(cardAddHorizPos + 60, cardAddVertStartingPos + 40, 60, 20), "Ogre"))
					RemoveCardForPlayer(PlayerName, "Ogre");

				if (GUI.Button(new Rect(cardAddHorizPos, cardAddVertStartingPos + 60, 60, 20), "Teleport"))
					SetCardForPlayer(PlayerName, "Teleport");
				if (GUI.Button(new Rect(cardAddHorizPos + 60, cardAddVertStartingPos + 60, 60, 20), "Teleport"))
					RemoveCardForPlayer(PlayerName, "Teleport");

				if (GUI.Button(new Rect(cardAddHorizPos, cardAddVertStartingPos + 80, 60, 20), "Heal"))
					SetCardForPlayer(PlayerName, "Heal");
				if (GUI.Button(new Rect(cardAddHorizPos + 60, cardAddVertStartingPos + 80, 60, 20), "Heal"))
					RemoveCardForPlayer(PlayerName, "Heal");

				if (GUI.Button(new Rect(cardAddHorizPos, cardAddVertStartingPos + 100, 60, 20), "Fireball"))
					SetCardForPlayer(PlayerName, "Fireball");
				if (GUI.Button(new Rect(cardAddHorizPos + 60, cardAddVertStartingPos + 100, 60, 20), "Fireball"))
					RemoveCardForPlayer(PlayerName, "Fireball");
			}
			break;
		case GameStateType.PLAYING:
			//print ("GameState Playing GUI stuff");
			MageController currentPlayer = Board.GetCurrentPlayer ();
			SpellType[] localHand = currentPlayer.Hand;
			int HandSize = localHand.GetLength(0);
			
			for(int i = 0; i < HandSize; i++){
				if(DisplayCard(i, localHand[i].ToString ())){
					CurrentSpell = localHand[i];
				}
			}
			if(GUI.Button (new Rect (10, 10, 100, 90), "Finish Turn")){
				print ("Calling advance player function from gui");
				Board.AdvancePlayer();
			}
			break;
		case GameStateType.INIT:
			print ("INIT state in PlayerInputHandler.OnGUI");
			break;
		}

	}
	
	public void CastSpell(TileManager.XYPair[] locations, int playerID, SpellType spell){
		foreach(TileManager.XYPair loc in locations){
			CastSpell(loc, playerID, spell);
		}
	}

	public void CastSpell(TileManager.XYPair location, int playerID, SpellType spell){
		switch(spell){
		case SpellType.FIREBALL:
			Board.GetTile(location).PropegateDamage(5);
			break;
		case SpellType.IMP:
			Board.Summon(playerID, location, CreatureController.CreatureType.Imp);
			break;
		case SpellType.HEAL:
			Board.GetTile(location).PropegateDamage(-10);
			break;
		case SpellType.CASTER:
			Board.Summon(playerID, location, CreatureController.CreatureType.Caster);
			break;
		case SpellType.OGRE:
			Board.Summon(playerID, location, CreatureController.CreatureType.Ogre);
			break;
		case SpellType.TELEPORT:
			print (location.x);
			print (location.y);
			Board.UpdatePlayerPosition(playerID, location);
			break;
		case SpellType.NONE:
			return;
		}
	}
	// Use this for initialization
	void Start ()
	{
		Board = gameObject.GetComponent<TileManager>();
	}

	void HandlePlayerInput(){
		if(Input.GetKeyDown (KeyCode.Space)){
			Vector3 mainCamPos = Board.GetCurrentPlayer().gameObject.transform.position;
			mainCamPos.z -= 10f;
			Camera.main.transform.position = mainCamPos;
		}
		if(Input.GetKeyDown (KeyCode.Escape)){
			if(GameState == GameStateType.PLAYING)
				GameState = GameStateType.ESCMENU;
			else if(GameState == GameStateType.ESCMENU)
				GameState = GameStateType.PLAYING;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if(CurrentSpell != SpellType.NONE && Input.GetMouseButtonDown(0)){
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0f;
			CastSpell (Board.ComputeXYFromPosition(pos), Board.GetCurrentPlayerID(), CurrentSpell);

			CurrentSpell = SpellType.NONE;
		}
		HandlePlayerInput();
	}
}

