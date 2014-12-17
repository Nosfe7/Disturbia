﻿using UnityEngine;
using System.Collections;



public class Player : MonoBehaviour {
	private static bool inside;
	private int cal;
	public Peso peso;
	public Fame fame;
	public Ansia ansia;
	public ObjectInteraction objectInteraction;
	public int numcibi;

	public bool isInside() {
		return inside;
	}


	// Use this for initialization
	void Start () {
		inside = false;
		peso = new Peso ();
		fame = new Fame ();
		ansia = new Ansia ();
		objectInteraction = new ObjectInteraction ();
		numcibi = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		/*BLUR E ANSIA*/
		ansia.updateBlur (); //aggiorno effetto blur in funzione dell'ansia ad ogni frame
		ansia.handleGameplay ();

		/*FAME*/
		fame.updateGUI (); //aggiorno GUI della fame
		fame.handleGameplay ();

		/*PESO*/
		peso.handleGameplay ();

		/*INTERAZIONE CON GLI OGGETTI*/
		objectInteraction.HandleCollisions ();

	
	}

	/*COLLISIONI/TRIGGER*/
	
	public void OnTriggerEnter (Collider other){
		inside = true;
	}
	
	public void OnTriggerExit(Collider collider ){
		inside = false;
	}
}



public class PlayerObject{
	private static Player instance;
	
	
	public static Player getInstance {
		get {
			if (instance == null)
				instance = GameObject.Find("First Person Controller").GetComponent<Player>();
			return instance;
		}
		
	}
}


//Classe per gestire l'interazione con gli oggetti
public class ObjectInteraction {
	
	private Vector3 offset; 
	private string hitTag;
	private bool dragObject;


	public void DragObject(bool value){
		dragObject = value;
	}

	private void HandleDrag(GameObject obj) //gestisce il trascinamento
	{
		
		if (dragObject){ //se posso trascinare, l'oggetto dev'essere davanti a me a una distanza fissa
			
			obj.transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width/2,Screen.height/2,0)) + offset;
			//print (obj.transform.position);
		}
		else {
			offset = obj.transform.position - Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width/2,Screen.height/2,0));
		}
	}

	public void HandleCollisions(){//scopro di fronte a quale oggetto mi trovo tramite raycast

		// raggio
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width/2,Screen.height/2,0));
		//oggetto colpito
		RaycastHit hit = new RaycastHit();
		
		//lancio raggio e controllo il tag di cosa ho colpito 
		if (Physics.Raycast(ray,out hit,100)){
			hitTag = hit.transform.tag;
			if ((hitTag == "Mobile" || hitTag == "Cibo" || hitTag == "Bilancia" || hitTag=="Water") && PlayerObject.getInstance.isInside()) { //quando sono di fronte e vicino a un mobile o cibo
				GUIObject.getInstance.CanInteract(true);
				
				if (hitTag == "Cibo"){//Se sono di fronte a un cibo...
					GUIObject.getInstance.setObj("Cibo");

					int sodd = hit.transform.gameObject.GetComponent<Cibo>().Soddisfazione;
					double cal = hit.transform.gameObject.GetComponent<Cibo>().Calorie;
					int riemp = hit.transform.gameObject.GetComponent<Cibo>().Riempimento;

					ArrayList info = new ArrayList();
					info.Add(sodd);
					info.Add(cal);
					info.Add(riemp);
					info.Add(hit.transform.gameObject);
					GUIObject.getInstance.InfoCibo(info); //invio informazioni utili sul cibo
				}
				
				else if (hitTag == "Mobile"){ //se sono di fronte a un mobile
					GUIObject.getInstance.setObj("Mobile");
					HandleDrag(hit.transform.gameObject);//gestisci trascinamento

				}

				else if (hitTag == "Bilancia"){
					GUIObject.getInstance.setObj("Bilancia");

				}

				else if (hitTag == "Water")
					GUIObject.getInstance.setObj("Water");
				
			}
			else
				GUIObject.getInstance.CanInteract(false);//se non sono di fronte e vicino ad un oggetto, non posso interagire
		}

	}

}

//Singleton che rappresenta l'ansia
public class Ansia {

	private int level; //livello d'ansia
	private MotionBlur blur = Camera.main.GetComponent<MotionBlur>(); //effetto blur 
	private float timer;
	private int oldtimeseconds;
	private int timeseconds;


	public Ansia() {
		level = 0;
		timer = 0;
		oldtimeseconds = 0;
		timeseconds = 0;
	}
	

	public int getLevel ()
	{
		return level;
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
			blur.blurAmount = (float)level/80;
		}

	}

	public void handleGameplay(){ //traduce ogni livello d'ansia in una conseguenza per il gameplay
		if (level >= 100)
			GUIObject.getInstance.GameOver(); //se l'ansia raggiunge 100 è game over

		Debug.Log (level);
	
	}
}

//Classe che rappresenta la fame
public class Fame {
	private int level;
	private float timer;


	public Fame() { 
		level = 30;
		timer = 5; //5 minuti (300 secondi)
	}
	

	public int Level {
		get {return level;}
		set {
			if (level<0) level = 0; 
			else{ 
				level = value;
				timer = 5;
			}
		}
	}

	public float Timer{
		get {return timer;}

	}

	public void updateGUI(){
		GUIObject.getInstance.UpdateFameGUI (level);
	}

	public void handleGameplay(){ //traduce ogni livello di fame in una conseguenza per il gameplay
		if (level >= 100)
			GUIObject.getInstance.GameOver ();//se ho mangiato troppo è gameover
		else
		{
			if (level == 0)
			{
				GUIObject.getInstance.fameCountDown(timer);
				timer=30;
				if (timer < 0) 
					GUIObject.getInstance.GameOver();//se non mangio entro 5 minuti è gameover
			}
			else
			{ 
				if (timer < 0)
				{ 
					level-=1; //ogni minuto svuoto la barra fame di 4
					timer = 5; //riset il timer a 5 minuti

				}

			}
			timer-=Time.deltaTime;
		}

	
	}
}


public class Peso {
	private double level;
	private double timer;
	private int oldtimeseconds;
	private int timeseconds;
	private double lastlevel;

	public Peso() {
		timer = 0;
		level = 55;
		lastlevel = 55;
	
	}

	public double Level {
		set {level = value;}
		get {return level;}
	}

	public double LastLevel {
		set {lastlevel = value;}
		get {return lastlevel;}
	}

	public void handleGameplay(){
		timer += Time.deltaTime;
		timeseconds = (int)timer;
		if (timeseconds > oldtimeseconds+60) 
		{
			level-=0.1;
			oldtimeseconds = timeseconds;
		}

		//
	}
}

/*public class Calorie {
	private int level;
	
	public Calorie() {
		level = 40;
	}
	
	public int Level {
		set {level = value;}
		get {return level;}
	}
	
	public void handleGameplay(){
		
	}


}*/