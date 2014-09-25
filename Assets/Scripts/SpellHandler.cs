using UnityEngine;
using System.Collections;

public class SpellHandler : MonoBehaviour {
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
		//TODO: Instead of directly using the board, just handle the tiles passed in to this function
		//and apply the specified spell to those tiles
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
		case SpellType.NONE:
			return;
			/*case SpellType.TELEPORT:
					transform.position = Board.UpdatePlayerPosition(playerID, coords);
					break;*/
		}
	}
	// Use this for initialization
	void Start ()
	{
		Board = gameObject.GetComponent<TileManager>();
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

