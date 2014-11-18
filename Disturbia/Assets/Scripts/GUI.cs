using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {

	public Texture2D manina2;
	public Texture2D manina;
	

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
	
	// Update is called once per frame
	void Update () {
		if (GetLeftMouse ()) //Se sto premendo il tasto sinistro...
			//carico manina2
			GameObject.Find ("puntatore").guiTexture.texture = manina2;
		else //Altrimenti carico manina 1
			GameObject.Find ("puntatore").guiTexture.texture = manina;
	}
}
