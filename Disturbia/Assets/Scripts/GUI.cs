using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {

	private Texture manina2;
	private Texture manina;
	private bool interacts;




	bool GetLeftMouse() //Controlla se sto premendo il tasto sinistro del mouse
	{
		return Input.GetKey(KeyCode.Mouse0);
	}

	// Use this for initialization
	void Start ()
	{

		/*Carica le immagini del puntatore come texture
		(L'immagin verrà ovviamente
		sostituita dagli artisti con una manina :) )*/

		manina =(Texture)Resources.Load("2D/puntatore_0");
		manina2 = (Texture)Resources.Load ("2D/puntatore_1");
	} 

	void Interacts(bool value) //il giocatore può interagire ? 
	{
		interacts = value;
	}

	// Update is called once per frame
	void Update () {
		if (interacts){

			if (GetLeftMouse ())
				this.guiTexture.texture = manina2;
			else 
				this.guiTexture.texture = manina;
		}
		else 
			this.guiTexture.texture = null;
	}


}
