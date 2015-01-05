using UnityEngine;
using System.Collections;



public class Player : MonoBehaviour {
	private static bool inside;
	private int cal;
	public Peso peso;
	public Fame fame;
	public Ansia ansia;

	public Animator animator;

	public ObjectInteraction objectInteraction;

	public int numcibi;
	//public int numvomiti;
	public int numpillole;

	public int points;
	private Vector3 lastPos;
	


	// Use this for initialization
	void Start () {
		inside = false;
		peso = new Peso ();
		fame = new Fame ();
		ansia = new Ansia ();
		objectInteraction = new ObjectInteraction ();
		numcibi = 0;
		//numvomiti = 0;
		points = 0;
		numpillole = 0;
		Screen.showCursor = false;
		lastPos = transform.position;
		animator = GetComponent<Animator> ();

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



		//Debug.Log (points);
		//Debug.Log (ansia.Level);

		//GESTIONE PUNTEGGIO
		if (GUIObject.getInstance.orologio < 0) {
			if (points == 10 && (int)GUIObject.getInstance.timer == 3)
				GUIObject.getInstance.Vittoria ();
			else if (points == 5 && (int)GUIObject.getInstance.timer == 3)
				GUIObject.getInstance.SemiVittoria();
			else
				GUIObject.getInstance.GameOver ();
		}
		//Debug.Log (points);
		//Shortcut nel caso sia necessario uscire o riavviare forzatamente
		if (Input.GetKey(KeyCode.Escape))
		    Application.Quit();
		if (Input.GetKey (KeyCode.R))
			Application.LoadLevel ("level1");
	}

	public bool isMoving() {
		if (lastPos!=transform.position) {
			lastPos = transform.position;
			return true; 
		}

		return false;
	}

	/*COLLISIONI/TRIGGER*/
	
	public void OnTriggerEnter (Collider other){
		/*INTERAZIONE CON GLI OGGETTI*/
		objectInteraction.HandleCollisions (other.gameObject);

	}
	
	public void OnTriggerExit(Collider other ){
		GUIObject.getInstance.CanInteract (false);
				
			/*//Vomito 1 volta : aumenta l'ansia 
			if (PlayerObject.getInstance.numvomiti == 1) {
				PlayerObject.getInstance.ansia.setLevel (PlayerObject.getInstance.ansia.getLevel () + 15);
				//Debug.Log("CIAO");
				GUIObject.getInstance.vomitoGUIText.text = "VOMITA ANCORA!";
			}

			else if (PlayerObject.getInstance.numvomiti == 2){
				GUIObject.getInstance.vomitoGUIText.text = "VOMITA ANCORA!";
			}

			//Vomito 3 volte: diminuisce l'ansia
			else if (PlayerObject.getInstance.numvomiti == 3){
				PlayerObject.getInstance.ansia.setLevel (PlayerObject.getInstance.ansia.getLevel () - 20);
				PlayerObject.getInstance.numvomiti = 0;
			}
			*/
		particleSystem.Stop ();
			
		if (other.tag == "Bilancia")
			GUIObject.getInstance.suBilancia = false;
			//PlayerObject.getInstance.transform.position.z
			

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

	public void HandleCollisions(GameObject o){//scopro di fronte a quale oggetto mi trovo tramite raycast

		/*// raggio
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width/2,Screen.height/2,0));
		//oggetto colpito
		RaycastHit hit = new RaycastHit();*/


		//lancio raggio e controllo il tag di cosa ho colpito 
		if (o.renderer.isVisible){
			hitTag = o.tag;
			GUIObject.getInstance.CanInteract(true);

			if (hitTag == "Cibo"){//Se sono di fronte a un cibo...
				GUIObject.getInstance.setObj("Cibo",o);

				int sodd = o.GetComponent<Cibo>().Soddisfazione;
				double cal = o.GetComponent<Cibo>().Calorie;
				int riemp = o.GetComponent<Cibo>().Riempimento;
				string nome = o.GetComponent<Cibo>().Nome;

				ArrayList info = new ArrayList();
				info.Add(sodd);
				info.Add(cal);
				info.Add(riemp);
				info.Add(nome);
				GUIObject.getInstance.InfoCibo(info); //invio informazioni utili sul cibo
			}
			
			else if (hitTag == "Mobile"){ //se sono di fronte a un mobile
				GUIObject.getInstance.setObj("Mobile",o);
				HandleDrag(o);//gestisci trascinamento

			}

			else if (hitTag == "Bilancia"){
				GUIObject.getInstance.setObj("Bilancia",o);

			}

			else if (hitTag == "Water")
				GUIObject.getInstance.setObj("Water",o);

			else if (hitTag == "Pillola")
				GUIObject.getInstance.setObj("Pillola",o);

			else if (hitTag == "Allenamento")
				GUIObject.getInstance.setObj("Allenamento",o);
				
		}

		else
			GUIObject.getInstance.CanInteract(false);//se non sono di fronte e vicino ad un oggetto, non posso interagire
		//Debug.Log (o.renderer.isVisible);

	}
	

	public void HandleExit(){
		GUIObject.getInstance.CanInteract (false);

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
				if (timer < 0 ) 
					GUIObject.getInstance.GameOver();//se non mangio entro 5 minuti è gameover
			}
			else
			{ 
				if (timer < 0)
				{ 
					level-=1; //ogni minuto svuoto la barra fame di 4
					timer = 3; //riset il timer a 5 minuti

				}
				if (level==0)
					timer = 30;
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
		}
		//Debug.Log (level);
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