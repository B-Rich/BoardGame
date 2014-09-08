using UnityEngine;
using System.Collections;

public class CreatureController : MonoBehaviour {
	private int hp;
	private int ownerID;

	public enum CreatureType {
		Imp
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Init(int playerID, CreatureType type){
		ownerID = playerID;
		if(type == CreatureType.Imp){
			hp = 1;
		}
		SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		if(ownerID == 0){
			spriteRenderer.color = Color.red;
		}
		if(ownerID == 1){
			spriteRenderer.color = Color.blue;
		}
	}

	public void TakeDamage(int damage) {
		print ("I am taking damage");
		hp -= damage;
		if (hp <= 0){
			print ("I was destroyed");
			Destroy (gameObject);
		}
	}
}
