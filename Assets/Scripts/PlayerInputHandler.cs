using UnityEngine;
using System.Collections;

public class PlayerInputHandler : MonoBehaviour {
	TileManager Board;
	const int CARDWIDTH = 100;
	const int CARDHEIGHT = 100;
	const float HANDPOSFACTOR_X = 1.4f;
	const float HANDPOSFACTOR_Y = 1.1f;
	public enum SpellType{
		NONE,
		FIREBALL,
		IMP,
		OGRE,
		CASTER,
		TELEPORT,
		HEAL
	}

	SpellType Current = SpellType.NONE;

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
		if(DisplayCard(0, "Summon Imp")){
			Current = SpellType.IMP;
		}
		if(DisplayCard(1,"Teleport")){
			Current = SpellType.TELEPORT;
		}
		if(DisplayCard (2, "Heal")){
			Current = SpellType.HEAL;
		}
		if(DisplayCard (3, "Caster")){
			Current = SpellType.CASTER;
		}
		if(DisplayCard (4, "Fireball")){
			Current = SpellType.FIREBALL;
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
			Board.GetCurrentPlayer().Teleport(location);
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

	public void Teleport(TileManager.XYPair loc){
		transform.position = board.ComputePosition(loc.x, loc.y);
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
	void Update ()
	{
		if(Current != SpellType.NONE && Input.GetMouseButtonDown(0)){
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pos.z = 0f;
			CastSpell (Board.ComputeXYFromPosition(pos), Board.GetCurrentPlayerID(), Current);
			Current = SpellType.NONE;
		}
	}
}

