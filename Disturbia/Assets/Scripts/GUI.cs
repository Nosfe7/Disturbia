using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {

	private Texture2D manina2;
	private Texture2D manina;
	private bool playerEnters;


	bool GetLeftMouse() //Controlla se sto premendo il tasto sinistro del mouse
	{
		return Input.GetKey(KeyCode.Mouse0);
	}

	// Use this for initialization
	void Start ()
	{
		/*Carica le immagini della "manina" come texture
		(L'immagine della manina verrà ovviamente
		sostituita dagli artisti con una più decente :) )*/

		manina = (Texture2D)Resources.Load("manina");
		manina2 = (Texture2D)Resources.Load ("manina2");
	} 

	void playerNearObj() //il giocatore è vicino all'oggetto
	{
		playerEnters = true; 
	}

	void playerFarObj() //il giocatore è lontano dall'oggetto
	{
		playerEnters = false; 
	}
	// Update is called once per frame
	void Update () {
		if (playerEnters) {
			if (GetLeftMouse ())
				this.guiTexture.texture = manina2;
			else 
				this.guiTexture.texture = manina;
		}
		else 
			this.guiTexture.texture = null;
	}


}
