using UnityEngine;
using System.Collections;



public class Player : MonoBehaviour {
	private int cal;
	public Peso peso;
	public Fame fame;
	public Ansia ansia;
	public Calorie calorieAssunte;

	public Animator animator;

	public int numcibi;
	public int numallenamenti;

	public int points;

	private GameObject mostro;
	private NavMeshAgent agentMostro;
	Vector3 previousPosition;
	


	// Use this for initialization
	void Start () {
		peso = new Peso ();
		fame = new Fame ();
		ansia = new Ansia ();
		calorieAssunte = new Calorie ();

		numcibi = 0;
		//numvomiti = 0;
		points = 0;
		numallenamenti = 0;

		Screen.showCursor = false;

		animator = GetComponent<Animator> ();
		mostro = GameObject.Find ("Mostro");
		agentMostro = mostro.GetComponent<NavMeshAgent>();

		previousPosition=transform.position;

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

		/*MOSTRO*/
		mostroManager ();


		//Debug.Log(ansia.Level);
		//Debug.Log (GUIObject.getInstance.orologio);
		//Debug.Log (points);

		//GESTIONE PUNTEGGIO
		if (GUIObject.getInstance.orologio >1200) {
			if (points == 10 && (int)GUIObject.getInstance.timer == 3)
				GUIObject.getInstance.Vittoria ();
			else if ((points >= 5 && points < 10) && (int)GUIObject.getInstance.timer == 3)
				GUIObject.getInstance.SemiVittoria();
			else
				GUIObject.getInstance.GameOver ();
		}
		//Shortcut nel caso sia necessario uscire o riavviare forzatamente
		if (Input.GetKey(KeyCode.Escape))
			Application.LoadLevel ("level0");
		if (Input.GetKey (KeyCode.R))
			Application.LoadLevel ("level1");
	}

	public bool isMoving() { //il giocatore si sta muovendo ?
		bool moving;
		if (previousPosition != transform.position)
						moving = true;
				else
						moving = false;
		previousPosition = transform.position;

		return moving;


	}

	public void mostroManager() { //gestisce la presenza e vicinanza del mostro
		switch (points) {
		case 0:
			mostro.SetActive(false);
			break;
		case 1:
			mostro.SetActive(true);
			agentMostro.stoppingDistance = 4;
			break;
		case 2:
		case 3:
		case 4:
			agentMostro.stoppingDistance = 3;
			break;
		case 5:
		case 6:
		case 7:
			agentMostro.stoppingDistance = 2;
			break;
		case 8:
		case 9:
		case 10:
			agentMostro.stoppingDistance= 1;
			break;
		}

	}
	

	public void HandleCollisions(GameObject o){

		//scopro di fronte a quale oggetto mi trovo
		Vector2 pointInScreen = Camera.main.WorldToViewportPoint (o.transform.position);
		if (pointInScreen.x>=0 && pointInScreen.x<=1 && pointInScreen.y>=0 && pointInScreen.y<=1){
			GUIObject.getInstance.CanInteract(true);

			switch (o.tag) {

				case "Cibo"://Se sono di fronte a un cibo...
					GUIObject.getInstance.setObj("Cibo",o);
					
					int sodd = o.GetComponent<Cibo>().Soddisfazione;
					float cal = o.GetComponent<Cibo>().Calorie;
					int riemp = o.GetComponent<Cibo>().Riempimento;
					string nome = o.GetComponent<Cibo>().Nome;
					
					ArrayList info = new ArrayList();
					info.Add(sodd);
					info.Add(cal);
					info.Add(riemp);
					info.Add(nome);
					GUIObject.getInstance.InfoCibo(info); //invio informazioni utili sul cibo
					break;

				
				case "Bilancia":
					GUIObject.getInstance.setObj("Bilancia",o);
					break;
				
				case "Water":
					GUIObject.getInstance.setObj("Water",o);
					break;

				case "Pillola":
					GUIObject.getInstance.setObj("Pillola",o);
					break;

				case "Allenamento":
					GUIObject.getInstance.setObj("Allenamento",o);
					break;

				case "Libro":
					GUIObject.getInstance.setObj("Libro",o);
					break;
				case "Frigo":
					GUIObject.getInstance.setObj("Frigo",o);
					break;
				case "Armadio":
					GUIObject.getInstance.setObj("Armadio",o);
					break;
			}
		}
		
		else
			GUIObject.getInstance.CanInteract(false);//se non sono di fronte e vicino ad un oggetto, non posso interagire
		
	}
	

	public void OnTriggerEnter (Collider other){
		Debug.Log ("entrato");
		HandleCollisions (other.gameObject);
	}
	
	public void OnTriggerExit(Collider other ){
		GUIObject.getInstance.CanInteract (false);
		particleSystem.Stop ();
			
		if (other.tag == "Bilancia")
			GUIObject.getInstance.suBilancia = false;
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



//Singleton che rappresenta l'ansia
public class Ansia {

	private int level; //livello d'ansia
	private MotionBlur blur = Camera.main.GetComponent<MotionBlur>(); //effetto blur 


	public Ansia() {
		level = 0;
	}
	

	public int Level {
		set {
			if (value>=0 && value<=100)
			level = value;
			else {
				if (value> 100)
					level = 100;
				if (value < 0)
					level = 0;
			}
		}

		get {return level;}


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
		if (level >= 100 )
			GUIObject.getInstance.GameOver(); //se l'ansia raggiunge 100 è game over

		GUIObject.getInstance.UpdateAnsiaGUI (level);

	
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
				timer = 3;
			}
		}
	}

	public float Timer{
		get {return timer;}

	}

	public void updateGUI(){
		GUIObject.getInstance.UpdateFameGUI (level);
	}

	//traduce ogni livello di fame in una conseguenza per il gameplay
	public void handleGameplay(){ 

		if (level >= 100)
			GUIObject.getInstance.GameOver ();
		else
		{
			if (level == 1)
			{
				GUIObject.getInstance.startFameTimer = true;
				GUIObject.getInstance.fameTimer = 30;
				GUIObject.getInstance.stateOrologio = "Mangia!";
				level = 0;
			}
			else if (level >1)
			{ 
				if (timer < 0)
				{ 
					level-=1; //ogni minuto svuoto la barra fame di 4
					timer = 3; //riset il timer a 3 secondi

				}
				timer-=Time.deltaTime;
			}

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
		level = 55.0;
		lastlevel = 55.0;
	
	}

	public double Level {
		set {level = value;}
		get {return level;}
	}

	public double LastLevel {
		set {lastlevel = value;}
		get {return lastlevel;}
	}

	public double Timer {
		set {timer = value;
			oldtimeseconds = (int)value;
		}
		get {return timer;}
	}

	public void handleGameplay(){
		timer += Time.deltaTime;

		timeseconds = (int)timer;
		if (timeseconds > oldtimeseconds+60) 
		{
			level-=0.1;
			oldtimeseconds = timeseconds;
		}

		if ((int)timer%60 == 0 && (int)timer>0) {
			//setta il timer della GUI: se non ti pesi entro 30 secondi aumenta l'ansia	GUIObject.getInstance.startTimer = true;
			GUIObject.getInstance.startpesoTimer = true;
			GUIObject.getInstance.pesoTimer = 30;
			GUIObject.getInstance.stateOrologio = "Pesati!";
		}
	}
}

public class Calorie {
	private float level;
	private int segno;
	
	public Calorie() {
		level = 0;
	}
	
	public float Level {
		set {

			if (level<value) segno = 1;
			else segno = -1;


			if (value >= 1000 && value <1200)
				PlayerObject.getInstance.ansia.Level += segno*10;
			else if (value >=1200 && value <1600)
			    PlayerObject.getInstance.ansia.Level += segno*20;
			else if (value >=1600)
				PlayerObject.getInstance.ansia.Level += segno*30;

			level = value;
		}
		get {return level;}
	}
	


}
