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
	public  GUIText fameGUIText;
	private float timer; 
	private bool startTimer;


	public bool GetLeftMouse() //Controlla se sto premendo il tasto sinistro del mouse
	{
		return Input.GetKey(KeyCode.Mouse0);
	}

	public bool GetLeftMouseHold()
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
		fameGUIText.text = "Fame: " + value;
	}

	public void GameOver(){
		guiText.text = "GAME OVER";
		startTimer = true;
	}

	public void fameCountDown(float time){ //countdown al gameover per fame
		int minuti = (int)time / 60;
		int secondi = (int)time % 60;

		if (secondi>=10)
			guiText.text = "\t\tHai fame!\n" + "Mangia entro: " + minuti + ":" + secondi;
		else 
			guiText.text = "\t\tHai fame!\n" + "Mangia entro: " + minuti + ":" + "0" + secondi;
	}

	private void handleTimer(){
		if (startTimer)
			timer -= Time.deltaTime;
		if (timer < 0)
			Application.Quit ();
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
		fameGUIText.color = Color.green;
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
		if (GUIObject.getInstance.GetLeftMouse ()) { //se sto premendo tasto sinistro....
			//GameGUI.guiTexture.texture = GameGUI.puntatore;//mostro puntatore tasto premuto

			/*Mangio cibo e aggiorno fame e numero cibi mangiat*/

			PlayerObject.getInstance.fame.Level = riempimento;
			PlayerObject.getInstance.numcibi++;

			/*Mostro infografica sul cibo mangiato*/
			GUIObject.getInstance.guiText.text = "Soddisfazione: " + soddisfazione + "\n" + 
								"Calorie: " + calorie + " Kcal" + "\n" + 
								"Riempimento: " + riempimento + "\n";

			//Ogni cibo ha un peso e un livello d'ansia specifico
			Cibo ciboscript = cibo.GetComponent<Cibo>();

			PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel() + ciboscript.levelansia);

			PlayerObject.getInstance.peso.Level = PlayerObject.getInstance.peso.Level + ciboscript.levelpeso;
		
			cibo.SetActive(false);
			//Debug.Log (Ansia.getInstance.getLevel());
	
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
		if (GUIObject.getInstance.GetLeftMouse ()) { //se sto premendo tasto sinistro....ng
			string levelstring = PlayerObject.getInstance.peso.Level.ToString();

			GUIObject.getInstance.guiText.text = "Peso: " + levelstring.Substring(0,4); //levelstring.Substring(0,4);

			//Debug.Log("PESO="+GameGUI.player.peso.LastLevel);

			//se il mio peso sulla bilancia è più alto dell'ultima volta sono grasso e aumenta l'ansia !
			if (PlayerObject.getInstance.peso.Level > PlayerObject.getInstance.peso.LastLevel)
			{
				PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel() + 20);

			}
			//altrimenti sono magro e diminuisce l'ansia
			else if(PlayerObject.getInstance.peso.Level < PlayerObject.getInstance.peso.Level)
				PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel()-25);


			//l'ultimo livello di peso pesato sulla bulancia
			PlayerObject.getInstance.peso.LastLevel = PlayerObject.getInstance.peso.Level;
		}
		
	}
}

class WaterGUI:InteractiveObjectGUI{
	public WaterGUI() {}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouseHold ()) { //se sto premendo tasto sinistro....ng
			//string levelstring = GameGUI.player.peso.Level.ToString();

			//Vomito tante volte quanti cibi ho mangiato
			if(GUIObject.getInstance.player.numcibi>0)
			{
				//Diminuisce l'ansia quando vomiti
				PlayerObject.getInstance.ansia.setLevel(PlayerObject.getInstance.ansia.getLevel()-5);
				PlayerObject.getInstance.numcibi--;
			}
			
		} 
		
	}

}