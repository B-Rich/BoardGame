using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Smooth Follow")]

public class MouseEdgeMove : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(PlayerInputHandler.GameState == PlayerInputHandler.GameStateType.PLAYING){
			Camera thisCamera = Camera.main;
			Vector3 mousePos = Input.mousePosition;
			Vector3 cameraPos = thisCamera.transform.position;

			if(mousePos.x > thisCamera.pixelWidth * .9){
				cameraPos.x += .3f;
			}
			if(mousePos.y > thisCamera.pixelHeight * .9){
				cameraPos.y += .3f;
			}
			if(mousePos.x < thisCamera.pixelWidth * .1){
				cameraPos.x -= .3f;
			}
			if(mousePos.y < thisCamera.pixelHeight * .1){
				cameraPos.y -= .3f;
			}
			thisCamera.transform.position = cameraPos;
		}
	}
}
