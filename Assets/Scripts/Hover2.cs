using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover2 : MonoBehaviour {

static public bool hover;

	void FixedUpdate(){
		hover = false;
	}

	void OnMouseOver (){
		hover = true;
	}

	void OnMouseExit (){
		hover = false;
	}
}
