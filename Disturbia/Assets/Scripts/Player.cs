using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	private string hitTag;
	private bool inside;
	private GameObject gui;
	private Ansia ansia;


	// Use this for initialization
	void Start () {
		gui = GameObject.Find("gui");
		ansia = Ansia.getInstance ();
	}
	// Update is called once per frame
	void Update () {

		/*BLUR E ANSIA*/
		ansia.updateBlur (); //aggiorno effetto blur in funzione dell'ansia ad ogni frame


		/*RAYCAST (controllo se è di fronte a un oggetto )*/

		// raggio
		Ray ray = Camera.main.ViewportPointToRay (new Vector3 (0.5F, 0.5F, 0));
		//oggetto colpito
		RaycastHit hit = new RaycastHit();

		//lancio raggio e controllo il tag di cosa ho colpito
		if (Physics.Raycast(ray,out hit,100)){
			hitTag = hit.transform.tag;
			if (hitTag == "Interactive" && inside) { //quando è di fronte e vicino a un oggetto "interattivo"..
				gui.SendMessage("CanInteract",true);
			}
			else 
				gui.SendMessage("CanInteract",false);
		}
	
	}

	/*COLLISIONI/TRIGGER*/
	
	public void OnTriggerEnter (Collider other){
		inside = true;
		ansia.setLevel (80);
	}
	
	public void OnTriggerExit(Collider collider ){
		inside = false;
		ansia.setLevel (0);
	}
}

public class Ansia {

	private static Ansia instance;
	private int level; //livello d'ansia
	private MotionBlur blur; //effetto blur 

	private Ansia()
	{
		level = 0;
		blur = Camera.main.GetComponent<MotionBlur>();
	}

	public static Ansia getInstance()
	{
		if (instance == null){
			instance = new Ansia();
		}
		
		return instance;
	}

	public void setLevel(int value)
	{
		if (value>=0 && value<=100)
			level = value;
		else {
			if (value > 100)
				value = 100;
			if (value < 0)
				value = 0;
		}
	}

	public void updateBlur() //aggiorna l'effetto blur all'aumentare dell'ansia: più alta è l'ansia più evidente è l'effetto
	{
		if (level >= 0 && level < 10)
			blur.enabled = false;
		else if (level >= 10){
			blur.enabled = true;
			blur.blurAmount = (float)level/100;
		}

	}
}
