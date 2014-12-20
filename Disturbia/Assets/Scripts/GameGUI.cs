using UnityEngine;
using System.Collections;


public class GameGUI : MonoBehaviour {

	public  Texture puntatore2;
	public  Texture puntatore;
	private  bool canInteract;
	private  string obj;
	public Player player;
	public ArrayList ciboinfo;
	public GUITexture guiTexture;
	public GUIText guiText;
	public GUIText fameGUIText;
	public GUIText vomitoGUIText;
	public GUIText pesoGUIText;
	public GUIText ansiaGUIText;
	public float timer; 
	public bool startTimer;
	public float vomitoTimer; 
	public bool startvomitoTimer;
	public float pesoTimer; 
	public bool startpesoTimer;
	public bool gameOverCountDown;
	public bool vomitoCount;
	public bool pesoCount;
	public bool suBilancia;


	public bool GetLeftMouse() //Controlla se ho premuto il tasto sinistro del mouse
	{
		return Input.GetKey(KeyCode.Mouse0);
	}

	public bool GetLeftMouseHold()
	{
		return Input.GetKeyDown(KeyCode.Mouse0);
	}

	public bool GetLeftMouseReleased()
	{
		return Input.GetKeyUp(KeyCode.Mouse0);
	}

	public void CanInteract(bool value) //il giocatore può interagire ? 
	{
		canInteract = value;
	}

	public void setObj(string o) {//specifica l'oggetto con cui interagire
		obj = o;
	}
	public void InfoCibo(ArrayList info) //abbiamo di fronte un cibo, mostriamo infografica
	{
		ciboinfo = info;
	}

	public void UpdateFameGUI(int value) {//aggiorna barra fame
		if (value == 0) 
			fameGUIText.color = Color.red;
		else if (value>0)
			fameGUIText.color = Color.green;
		fameGUIText.text = "Fame: " + value;
	}

	public void UpdateAnsiaGUI(int value) {//aggiorna barra fame
		if (value >= 80) 
			ansiaGUIText.color = Color.red;
		else
			ansiaGUIText.color = Color.green;
		ansiaGUIText.text = "Ansia: " + value;
	}

	public void GameOver(){
		guiText.text = "GAMEOVER";
		gameOverCountDown = true;
		startTimer = true;
		timer = 3;
	}

	public void Vittoria(){
		guiText.text = "VITTORIA! ";
		guiText.color = Color.green;
		gameOverCountDown = true;
		startTimer = true;
		timer = 3;
	}

	public void fameCountDown(float time){ //countdown al gameover per fame


			int minuti = (int)time / 60;
			int secondi = (int)time % 60;

			if (secondi >= 10)
					guiText.text = "\t\t\t\tHAI FAME!\n" + "MANGIA ENTRO: " + minuti + ":" + secondi;
			else 
					guiText.text = "\t\t\t\tHAI FAME!\n" + "MANGIA ENTRO: " + minuti + ":" + "0" + secondi;

	}

	public void vomitoCountDown(float time) {
		if (vomitoCount) { 
				int minuti = (int)time / 60;
				int secondi = (int)time % 60;

				if (secondi >= 10)
						vomitoGUIText.text = "VOMITA! " + minuti + ":" + secondi;
				else 
						vomitoGUIText.text = "VOMITA! " + minuti + ":" + "0" + secondi;
		} else 
				vomitoGUIText.text = null;
	}

	public void pesoCountDown(float time) {
		if (pesoCount) { 
			int minuti = (int)time / 60;
			int secondi = (int)time % 60;
			
			if (secondi >= 10)
				pesoGUIText.text = "PESATI! " + minuti + ":" + secondi;
			else 
				pesoGUIText.text = "PESATI! " + minuti + ":" + "0" + secondi;
		}
		else if(!suBilancia)
			pesoGUIText.text = null;
			
	}

	private void handleTimer(){
		if (startTimer || gameOverCountDown)
			timer -= Time.deltaTime;
		if (startvomitoTimer && vomitoCount)
			vomitoTimer -= Time.deltaTime;
		if (startpesoTimer && pesoCount)
			pesoTimer -= Time.deltaTime;

		if (timer < 0) 
			Application.Quit ();
		if (vomitoTimer < 0 && vomitoCount) {
			PlayerObject.getInstance.points++;
			PlayerObject.getInstance.ansia.setLevel (PlayerObject.getInstance.ansia.getLevel () + 15);
			vomitoCount = false;
			vomitoTimer = 30;
		}
			
		if (pesoTimer<0 && pesoCount) {
			PlayerObject.getInstance.points++;
			PlayerObject.getInstance.ansia.setLevel (PlayerObject.getInstance.ansia.getLevel () + 15);
			pesoCount = false;
			pesoTimer = 30;
		}
	}
	// Use this for initialization
	void Start ()
	{

		timer = 3;
		/*Carica le immagini del puntator0e come texture
		(L'immagin verrà ovviamente
		sostituita dagli artisti con una manina :) )*/

		puntatore =(Texture)Resources.Load("2D/puntatore");
		puntatore2 = (Texture)Resources.Load ("2D/puntatore_1");
		player = GameObject.Find("First Person Controller").GetComponent<Player>();

		guiTexture = GetComponent<GUITexture> ();
		guiText = GetComponent<GUIText> ();


		GameObject fameGUI = new GameObject("fameGUI");
		fameGUIText = (GUIText)fameGUI.AddComponent (typeof(GUIText));
		fameGUIText.transform.position = new Vector3 (0.1F, 0.9F, 0);
		fameGUIText.fontStyle = FontStyle.Bold;
		fameGUIText.color = Color.green;

		GameObject vomitoGUI = new GameObject("vomitoGUI");
		vomitoGUIText = (GUIText)vomitoGUI.AddComponent (typeof(GUIText));
		vomitoGUIText.transform.position = new Vector3 (0.5F, 0.4F, 0);
		vomitoGUIText.color = Color.red;
		vomitoGUIText.fontStyle = FontStyle.Bold;

		GameObject pesoGUI = new GameObject("pesoGUI");
		pesoGUIText = (GUIText)pesoGUI.AddComponent (typeof(GUIText));
		pesoGUIText.transform.position = new Vector3 (0.5F, 0.6F, 0);
		pesoGUIText.color = Color.red;
		pesoGUIText.fontStyle = FontStyle.Bold;

		GameObject ansiaGUI = new GameObject("ansiaGUI");
		ansiaGUIText = (GUIText)ansiaGUI.AddComponent (typeof(GUIText));
		ansiaGUIText.transform.position = new Vector3 (0.7F, 0.9F, 0);
		ansiaGUIText.color = Color.green;
		ansiaGUIText.fontStyle = FontStyle.Bold;

		suBilancia = false;
	} 



	// Update is called once per frame
	void Update () {
		if (canInteract) {//se posso interagire
			InteractiveObjectGUI ioGUI;

			if (obj == "Cibo"){ //Se è un cibo
				ioGUI = new CiboGUI((int)ciboinfo[0],(double)ciboinfo[1],(int)ciboinfo[2],(GameObject)ciboinfo[3]);
				ioGUI.handleInteraction();
			}
			if (obj == "Mobile"){//Se è un oggetto movibile
				ioGUI = new MobileGUI();
				ioGUI.handleInteraction();
			}

			if (obj == "Bilancia"){//Se è una bilancia
				ioGUI = new BilanciaGUI();
				ioGUI.handleInteraction();
			}

			if (obj == "Water"){//Se è un WATER
				ioGUI = new WaterGUI();
				ioGUI.handleInteraction();
			}
		} 
		else { 
				guiText.text = null;
				guiTexture.texture = null;
		}

		handleTimer ();
		vomitoCountDown (vomitoTimer);
		pesoCountDown(pesoTimer);
		//Debug.Log (canInteract);
		//Debug.Log (timer);
		//Debug.Log(vomitoGUIText.text);
	}


}


//Singleton che rappresenta la GUI

public class GUIObject{
	private static GameGUI instance;
	
	
	public static GameGUI getInstance {
		get {
			if (instance == null)
				instance = GameObject.Find("gui").GetComponent<GameGUI>();
			return instance;
		}
		
	}
}

abstract class InteractiveObjectGUI {
	public abstract void handleInteraction();	
}


class CiboGUI:InteractiveObjectGUI {
	private int soddisfazione;
	private double calorie;
	private int riempimento;
	private GameObject cibo;

	public CiboGUI(int s, double c, int r, GameObject cobj) {
		soddisfazione = s;
		calorie = c;
		riempimento = r;
		cibo = cobj;
	}

	public int Soddisfazione{
		set { soddisfazione = value;}
		get {return soddisfazione;}
	}
	
	public double Calorie{
		set {calorie = value;}
		get {return calorie;}
	}
	
	public int Riempimento{
		set {riempimento = value;}
		get {return riempimento;}
	}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture= GUIObject.getInstance.puntatore;
		/*Mostro infografica sul cibo*/
		GUIObject.getInstance.guiText.text = "Soddisfazione: " + soddisfazione + "\n" + 
			"Calorie: " + calorie + " Kcal" + "\n" + 
				"Riempimento: " + riempimento + "\n";

		if (GUIObject.getInstance.GetLeftMouseHold()) { //se sto premendo tasto sinistro....
			//GameGUI.guiTexture.texture = GameGUI.puntatore;//mostro puntatore tasto premuto

			/*Mangio cibo e aggiorno fame*/

			PlayerObject.getInstance.fame.Level = riempimento;
			PlayerObject.getInstance.numcibi++;

			/*Chiedo al giocatore di vomitare entro 30 secondi*/
			GUIObject.getInstance.startvomitoTimer = true;
			GUIObject.getInstance.vomitoTimer = 30;
			GUIObject.getInstance.vomitoCount = true;

			//Ogni cibo ha un peso e un livello d'ansia specifico
			Cibo ciboscript = cibo.GetComponent<Cibo>();

			PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel() + ciboscript.levelansia);

			PlayerObject.getInstance.peso.Level = PlayerObject.getInstance.peso.Level + ciboscript.levelpeso;
		
			cibo.SetActive(false);
			PlayerObject.getInstance.points+=1;
			//Debug.Log (ciboscript.levelansia);

	
		} 

	}
}

class MobileGUI:InteractiveObjectGUI{
	public MobileGUI() {}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouse ()) { //se sto premendo tasto sinistro....
			//GameGUI.guiTexture.texture = GameGUI.puntatore2;//puntatore tasto premuto
			PlayerObject.getInstance.objectInteraction.DragObject(true);//trascino
		} 
		else {
			//GameGUI.guiTexture.texture = GameGUI.puntatore;//puntatore normale (quando è vicino all'oggetto) 
			PlayerObject.getInstance.objectInteraction.DragObject(false);//a questo punto può trascinare
		}

	}
}

class BilanciaGUI:InteractiveObjectGUI{
	public BilanciaGUI () {}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouse()) { //se sto premendo tasto sinistro....ng
			string levelstring = PlayerObject.getInstance.peso.Level.ToString();



			GUIObject.getInstance.pesoGUIText.text = "Peso: " + levelstring.Substring(0,4); //levelstring.Substring(0,4);


			Debug.Log(GUIObject.getInstance.pesoGUIText.text);
			//Se mi peso risetto il timer
			GUIObject.getInstance.pesoCount = false;
			GUIObject.getInstance.startpesoTimer = false;
			GUIObject.getInstance.pesoTimer = 30;


			//se il mio peso sulla bilancia è più alto dell'ultima volta sono grasso e aumenta l'ansia !
			if (PlayerObject.getInstance.peso.Level > PlayerObject.getInstance.peso.LastLevel)
			{
				PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel() + 20);

			}
			//altrimenti sono magro e diminuisce l'ansia
			else if(PlayerObject.getInstance.peso.Level < PlayerObject.getInstance.peso.LastLevel)
				PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel()-25);


			//l'ultimo livello di peso pesato sulla bulancia
			PlayerObject.getInstance.peso.LastLevel = PlayerObject.getInstance.peso.Level;
			GUIObject.getInstance.suBilancia = true;
		}
		
	}
}

class WaterGUI:InteractiveObjectGUI{
	public WaterGUI() {}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouseHold ()) { //se sto premendo tasto sinistro vomito nel water



			//Se vomito risetto il timer
			GUIObject.getInstance.vomitoCount = false;
			GUIObject.getInstance.startvomitoTimer = false;
			GUIObject.getInstance.vomitoTimer= 30;

			//Conto numero di volte che ho vomitato
			//PlayerObject.getInstance.numvomiti++;

			//Vomito tante volte quanto ho mangiato: quando vomito l'ansia diminuisce di 5 (10? 15?)
			if (PlayerObject.getInstance.numcibi > 0){
				PlayerObject.getInstance.ansia.setLevel (PlayerObject.getInstance.ansia.getLevel () -10);
				Camera.main.particleSystem.Play();
				PlayerObject.getInstance.numcibi--;
			}
			

		}

		else if (GUIObject.getInstance.GetLeftMouseReleased()) { //se sto premendo tasto sinistro vomito nel water
			
			Camera.main.particleSystem.Stop ();
			
			
		}


	}

}