using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {

	private Texture manina2;
	private Texture manina;
	private bool canInteract;


	bool GetLeftMouse() //Controlla se sto premendo il tasto sinistro del mouse
	{
		return Input.GetKey(KeyCode.Mouse0);
	}

	public void CanInteract(bool value) //il giocatore può interagire ? 
	{
		canInteract = value;
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



	// Update is called once per frame
	void Update () {
		if (canInteract){

			if (GetLeftMouse ())
				guiTexture.texture = manina2; //tenere premuto trascinare oggetti 
			else 
				guiTexture.texture = manina; 
		}
		else 
			guiTexture.texture = null;
	}


}
