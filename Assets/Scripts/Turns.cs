using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turns : MonoBehaviour {

	public GameObject Menu1;
	public GameObject Menu2;

	static public bool active1;
	static public bool active2;

	public Button endTurnButton;
	public Text enemyTurn;

	static public bool enemyTurnEnded = false;

	void Update () {

		//SO FUNCIONA PARA DUAS UNIDADES, FAZER CONTROLE DE SITUACOES COM ARRAY
		
		if (Menu1.activeInHierarchy == true) {
			active1 = true;
			active2 = false;
		}

		if (Menu2.activeInHierarchy == true) {
			active2 = true;
			active1 = false;
		}

		//ATE AQUI. CRIAR MULTIPLAS INSTANCIAS

		if (enemyTurnEnded) {
			Player.moved = false;
			Player2.moved = false;
			//HABILITAR LINHAS COMENTADAS QUANDO OS REFERIDOS SCRIPTS FOREM CRIADOS
			//Player3.moved = false;
			//Player4.moved = false;
			//Player5.moved = false;
			//Player6.moved = false;
			//Player7.moved = false;
			//Player8.moved = false;

			Enemy.moved = false;
			enemyTurn.gameObject.SetActive (false);
			endTurnButton.interactable = true;
			Enemy.EnemyTurn = false;

			enemyTurnEnded = false;
		}
	}

	public void NextTurn (){
		Enemy.EnemyTurn = true;
		endTurnButton.interactable = false;
		enemyTurn.gameObject.SetActive (true);
	}
}
