using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {
	TileManager Board;
	const int CARDWIDTH = 100;
	const int CARDHEIGHT = 100;
	const float HANDPOSFACTOR_X = 1.4f;
	const float HANDPOSFACTOR_Y = 1.1f;
	bool BoundToMouse;
	public enum SpellType{
		NONE,
		FIREBALL,
		IMP,
		OGRE,
		CASTER,
		TELEPORT,
		HEAL
	}

	SpellType CurrentSpell = SpellType.NONE;

	public bool DisplayCard(int x, string cardName){
		if(GUI.Button (new Rect(Screen.width / HANDPOSFACTOR_X - CARDWIDTH*(5-x), 
		                        Screen.height / HANDPOSFACTOR_Y - CARDHEIGHT, 
		                        CARDWIDTH, CARDHEIGHT), cardName)){
			return true;
		}
		return false;
	}

	public void OnGUI(){
		//Find some way to determine when spell was clicked on, but is no longer being dragged. Maybe check GUI coords against expected when mouse is not down
		//What the heck is going on here?
		MageController currentPlayer = Board.GetCurrentPlayer ();
		SpellType[] localHand = currentPlayer.Hand;
		int HandSize = localHand.GetLength(0);

		for(int i = 0; i < HandSize; i++){
			if(DisplayCard(i, localHand[i].ToString ())){
				CurrentSpell = localHand[i];
			}
		}
		if(GUI.Button (new Rect (10, 10, 100, 90), "Finish Turn")){
			Board.AdvancePlayer();
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
		MageController currentPlayer = Board.GetCurrentPlayer();
		GameObject currentPlayerObject = currentPlayer.gameObject;
		if(BoundToMouse){
			if(Input.GetMouseButton(0)){
				Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pos.z = 0f;
				currentPlayerObject.transform.position = pos;
			}
			else {
				int playerID = Board.GetCurrentPlayerID();
				TileManager.XYPair coords = Board.GetPlayerPosition(playerID);
				print ("Current position of player is");
				print (coords.x);
				print (coords.y);
				TileManager.XYPair mousePosPair = Board.ComputeXYFromPosition(currentPlayerObject.transform.position);
				coords = TileManager.PairAlongDirection(coords, mousePosPair);
				print ("Moving player to");
				print (coords.x);
				print (coords.y);
				currentPlayerObject.transform.position = Board.UpdatePlayerPosition(playerID, coords);
				BoundToMouse = false;
			}
		}
		if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown (1)){
			Vector3 positionToCheck = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			positionToCheck.z=0f;
			if(currentPlayerObject.renderer.bounds.Contains (positionToCheck)){
				BoundToMouse = true;
			}
		}
		else if(Input.GetKeyDown (KeyCode.Space)){
			Vector3 mainCamPos = currentPlayerObject.transform.position;
			mainCamPos.z -= 10f;
			Camera.main.transform.position = mainCamPos;
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

