﻿using UnityEngine;
using System.Collections;

public class MageStats : MonoBehaviour {
	public int hp;
	public char playerName;
	private int level;
	private int experience;
	public GameObject GameBoardObject;
	private TileManager board;
	private GameObject boardLocation;
	private int x = 25;
	private int y = 25;

	// Use this for initialization
	void Start () {
		board = GameBoardObject.GetComponent<TileManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Q) || 
		   Input.GetKeyDown (KeyCode.E) || 
		   Input.GetKeyDown (KeyCode.A) ||
		   Input.GetKeyDown (KeyCode.D) ||
		   Input.GetKeyDown (KeyCode.Z) ||
		   Input.GetKeyDown (KeyCode.X)) {
			if (Input.GetKey (KeyCode.Q)) {
				if(x > 0)
					x-= (y % 2);
				if(y < TileManager.BOARD_HEIGHT - 1)
					y++;
			}
			if (Input.GetKey (KeyCode.E)) {
				if(x < TileManager.BOARD_WIDTH - 1)
					x+= ((y+1) % 2);
				if(y < TileManager.BOARD_HEIGHT - 1)
					y++;
			}
			if (Input.GetKey (KeyCode.A)) {
				if(x > 0)
					x--;
			}
			if (Input.GetKey (KeyCode.D)) {
				if(x < TileManager.BOARD_WIDTH - 1)
					x++;
			}
			if (Input.GetKey (KeyCode.Z)) {
				if(x > 0)
					x-= (y % 2);
				if(y > 0)
					y--;
			}
			if (Input.GetKey (KeyCode.X)) {
				if(x < TileManager.BOARD_WIDTH - 1)
					x+= ((y+1) % 2);
				if(y > 0)
					y--;
			}
			Vector3 nextPlayerPosition = board.ComputePlayerPosition(x, y);
			if(board.UpdatePlayerPosition (nextPlayerPosition, x, y))
			   transform.position = nextPlayerPosition;
		}
	}
}
