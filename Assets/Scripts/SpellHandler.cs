using UnityEngine;
using System.Collections;

public class SpellHandler : MonoBehaviour {
	public TileManager Board;
	public enum SpellType{
		FIREBALL,
		IMP,
		OGRE,
		CASTER,
		TELEPORT,
		HEAL
	}

	public void OnGUI(){
		GUI.Box (new Rect (400, 290, 100, 90), "Hand");
		
		if(GUI.Button (new Rect(410, 300, 80, 20), "Summon Imp")){

		}
		if(GUI.Button (new Rect(500, 300, 80, 20), "Teleport")){
			
		}
	}
	
	public void CastSpell(TileManager.XYPair[] locations, int playerID, TileManager.XYPair coords, SpellType spell){
		//TODO: Instead of directly using the board, just handle the tiles passed in to this function
		//and apply the specified spell to those tiles
		foreach(TileManager.XYPair loc in locations){
			switch(spell){
				case SpellType.FIREBALL:
					Board.GetTile(loc).PropegateDamage(5);
					break;
				case SpellType.IMP:
					Board.Summon(playerID, loc, CreatureController.CreatureType.Imp);
					break;
				case SpellType.HEAL:
					Board.GetTile(loc).PropegateDamage(-10);
					break;
				case SpellType.CASTER:
					Board.Summon(playerID, loc, CreatureController.CreatureType.Caster);
					break;
				case SpellType.OGRE:
					Board.Summon(playerID, loc, CreatureController.CreatureType.Ogre);
					break;
				/*case SpellType.TELEPORT:
					transform.position = Board.UpdatePlayerPosition(playerID, coords);
					break;*/
			}
		}
	}
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}

