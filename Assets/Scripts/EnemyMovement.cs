﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	List<Tile> selectableTiles = new List<Tile> ();
	GameObject[] tiles;

	Stack<Tile> path = new Stack<Tile> ();
	Tile currentTile;

	static public bool moving = false;
	public int move = 5;
	public float jumpHeight = 2;
	public float moveSpeed = 2;
	public float jumpVelocity = 4.5f;

	Vector3 velocity = new Vector3();
	Vector3 heading = new Vector3();

	float halfHeight = 0;

	bool fallingDown = false;
	bool jumpingUp = false;
	bool movingEdge = false;
	Vector3 jumpTarget;

	protected void Initi(){
		tiles = GameObject.FindGameObjectsWithTag ("tile");
		halfHeight = GetComponent<Collider> ().bounds.extents.y;
	}

	public void GetCurrentTile(){
		currentTile = GetTargetTile (gameObject);
		currentTile.current = true;
	}

	public Tile GetTargetTile(GameObject target){
		RaycastHit hit;
		Tile tile = null;

		if (Physics.Raycast(target.transform.position, - Vector3.up, out hit, 1)){
			tile = hit.collider.GetComponent<Tile> ();
	}
		return tile;
	}

	public void ComputedAdjacencyLists(){
		foreach (GameObject tile in tiles) {
			Tile t = tile.GetComponent<Tile> ();
			t.FindNeighbors (jumpHeight);
		}
	}

	public void FindSelectableTiles(){
		ComputedAdjacencyLists ();
		GetCurrentTile ();

		Queue<Tile> process = new Queue<Tile> ();

		process.Enqueue (currentTile);
		currentTile.visited = true;

		while (process.Count > 0) {
			Tile t = process.Dequeue ();

			selectableTiles.Add (t);
			t.selectable = true;

			if (t.distance < move) {
				foreach (Tile tile in t.adjacencyList) {
					
					if (!tile.visited) {
						tile.parent = t;
						tile.visited = true;
						tile.distance = 1 + t.distance;
						process.Enqueue (tile);
					}
				}
			}
		}
	}

	public void MoveToTile(Tile tile){
		path.Clear ();
		tile.target = true;
		moving = true;

		Tile next = tile;
		while (next != null) {
			path.Push (next);
			next = next.parent;
		}
	}

	public void Move(){
		if (path.Count > 0) {
			Tile t = path.Peek ();
			Vector3 target = t.transform.position;

			target.y += halfHeight + t.GetComponent<Collider> ().bounds.extents.y;

			if (Vector3.Distance (transform.position, target) >= 0.05f) {
				bool jump = transform.position.y != target.y;

				if (jump) {
					Jump (target);
				} else {
					CalculateHeading (target);
					SetHorizontalVelocity ();
				}

				transform.forward = heading;
				transform.position += velocity * Time.deltaTime;
			} else {
				transform.position = target;
				path.Pop ();
				fallingDown = false;
				jumpingUp = false;
				movingEdge = false;
				jumpTarget = Vector3.zero;
			}

		} else {
			RemoveSelectableTiles ();
			moving = false;

			Enemy.beginMovement = false;
			Turns.enemyTurnEnded = true;
		}
	}

	protected void RemoveSelectableTiles(){
		if (currentTile != null) {
			currentTile.current = false;
			currentTile = null;
		}
		foreach (Tile tile in selectableTiles) {
			tile.Reset ();
		}

		selectableTiles.Clear ();
	}

	void CalculateHeading(Vector3 target){
		heading = target - transform.position;
		heading.Normalize ();
	}

	void SetHorizontalVelocity(){
		velocity = heading * moveSpeed;
	}

	void Jump(Vector3 target){
		if (fallingDown) {
			FallDownward (target);
		} else if (jumpingUp) {
			JumpUpward (target);
		} else if (movingEdge) {
			MoveToEdge ();
		} else {
			PrepareJump (target);
		}
	}

	void PrepareJump(Vector3 target){
		float targetY = target.y;

		target.y = transform.position.y;

		CalculateHeading (target);

		if (transform.position.y > targetY) {
			fallingDown = false;
			jumpingUp = false;
			movingEdge = true;

			jumpTarget = transform.position + (target - transform.position) / 2.0f; 
		} else {
			fallingDown = false;
			jumpingUp = true;
			movingEdge = false;

			velocity = heading * moveSpeed / 3.0f;

			float difference = targetY - transform.position.y;

			velocity.y = jumpVelocity * (0.5f + difference / 2.0f);
		}
	}

	void FallDownward (Vector3 target){
		velocity += Physics.gravity * Time.deltaTime;

		if (transform.position.y <= target.y) {
			fallingDown = false;
			jumpingUp = false;
			movingEdge = false;

			Vector3 p = transform.position;
			p.y = target.y;
			transform.position = p;

			velocity = new Vector3 ();
		}
	}

	void JumpUpward (Vector3 target){
		velocity += Physics.gravity * Time.deltaTime;

		if (transform.position.y > target.y) {
			jumpingUp = false;
			fallingDown = true;
		}
	}

	void MoveToEdge (){
		if (Vector3.Distance (transform.position, jumpTarget) >= 0.05f) {
			SetHorizontalVelocity ();
		} else {
			movingEdge = false;
			fallingDown = true;

			velocity /= 5.0f;
			velocity.y = 1.5f;
		}
	}
}