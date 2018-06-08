using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2 : Movement {

	public GameObject unity;
	public GameObject menu;

	public Button movementButton;

	static public bool beginMovement = false;
	static public bool moved = false;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate(){
		if (Input.GetMouseButtonDown(0) && Hover2.hover == false) {
			menu.SetActive (false);
		}
	}

	void OnMouseDown()
	{
		if (Movement.moving == false && Enemy.EnemyTurn == false) {
			menu.SetActive (true);
		}
	}
		
	public void BeginMovement(){
		beginMovement = true;
		menu.SetActive (false);
	}

	void Update () {

		if (moved){
			movementButton.interactable = false;
		} else { movementButton.interactable = true;
		}

		Debug.DrawRay (transform.position, transform.forward);

		if (Turns.active2 == false) {
			return;
		}

		if (beginMovement) {

			Initi ();

			if (!moving) {
				FindSelectableTiles ();
				CheckMouse ();
			} else {
				Move ();
				moved = true;
			}
		}
	}
	void CheckMouse (){

		if (beginMovement && !moved) {
			
			if (Input.GetMouseButtonUp (0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.tag == "tile") {
						Tile t = hit.collider.GetComponent<Tile> ();

						if (t.selectable && !t.current) {
							MoveToTile (t);
						}
					}
				}
			}
		}
	}
}
