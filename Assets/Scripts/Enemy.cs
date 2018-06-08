using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyMovement {

	static public bool EnemyTurn = false;

	static public bool beginMovement = false;
	static public bool moved = false;

	void Update () {

		if (!moved) {
			beginMovement = true;
		}

		Debug.DrawRay (transform.position, transform.forward);

		if (EnemyTurn && beginMovement) {

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

		if (EnemyTurn && beginMovement && !moved) {

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
