using UnityEngine;
using System.Collections;


public class GameGUI : MonoBehaviour {
	

	private  bool canInteract;
	private  string obj_string;
	private GameObject obj;
	public Player player;
	public ArrayList ciboinfo;

	public GUITexture guiTexture;
	public GUIText guiText;
	public GUIText fameGUIText;
	public GUIText vomitoGUIText;
	public GUIText pesoGUIText;
	public GUITexture ansiaGUITexture;
	public GUITexture fameGUITexture;
	public GUIText pillolaGUIText;
	public GUIText orologioGUIText;
	public GUIText allenamentoGUIText;

	public float orologio;
	public float timer; 
	public bool startTimer;
	public float vomitoTimer; 
	public bool startvomitoTimer;
	public float pesoTimer; 
	public bool startpesoTimer;
	public bool gameOverCountDown;
	public float allenamentoTimer;
	public bool startAllenamentoTimer;
	

	public bool suBilancia;

	private Texture ansia1;
	private Texture ansia2;
	private Texture ansia3;
	private Texture ansia4;
	private Texture ansia5;
	private Texture ansia6;
	private Texture ansia7;
	private Texture ansia8;
	private Texture ansia9;
	private Texture ansia10;
	private Texture ansia11;

	private Texture fame1;
	private Texture fame2;
	private Texture fame3;
	private Texture fame4;

	public Texture puntatore;

	public CiboGUI cibogui;
	public WaterGUI watergui;
	public BilanciaGUI bilanciagui;
	public PillolaGUI pillolagui;
	public AllenamentoGUI allenamentogui;
	public MobileGUI mobilegui;


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

	public void setObj(string s,GameObject o) {//specifica l'oggetto con cui interagire
		obj_string = s;
		obj = o;
	}
	public void InfoCibo(ArrayList info) //abbiamo di fronte un cibo, mostriamo infografica
	{
		ciboinfo = info;
	}

	public void UpdateFameGUI(int value) {//aggiorna barra fame
		if (value == 0)
			fameGUITexture.texture = fame1;
		else if (value > 0 && value < 10)
			fameGUITexture.texture = fame2;
		else if (value >=10 && value < 20)
			fameGUITexture.texture = fame3;
		else if (value >=20)
			fameGUITexture.texture = fame4;
	}

	public void UpdateAnsiaGUI(int value) {//aggiorna barra fame
		if (value >= 0 && value<10)
			ansiaGUITexture.texture = ansia1;
			
		else if (value >= 10 && value<20)
			ansiaGUITexture.texture = ansia2;
		else if (value >= 20 && value<30) 
			ansiaGUITexture.texture = ansia3;
		else if (value >= 30 && value<40)
			ansiaGUITexture.texture = ansia4;
		else if (value >= 40 && value<50)
			ansiaGUITexture.texture = ansia5;
		else if (value >= 50 && value<60)
			ansiaGUITexture.texture = ansia6;
		else if (value >= 60 && value<70)
			ansiaGUITexture.texture = ansia7;
		else if (value >= 70 && value<80)
			ansiaGUITexture.texture = ansia8;
		else if (value >= 80 && value<90)
			ansiaGUITexture.texture = ansia9;
		else if (value >= 90 && value<100)
			ansiaGUITexture.texture = ansia10;
		else if (value >= 100 )
			ansiaGUITexture.texture = ansia11;

	}

	public void UpdateOrologioGUI(float value) {//aggiorna barra fame
		int minuti = (int)value / 60;
		int secondi = (int)value % 60;
		
		if (secondi >= 10)
			orologioGUIText.text = "" + minuti + ":" + secondi;
		else 
			orologioGUIText.text = "" + minuti + ":" + "0" + secondi;
	}

	public void GameOver(){
		guiText.text = "GAME OVER\n" + "La malattia ti ha sopraffatto..";
		gameOverCountDown = true;
		startTimer = true;
	}

	public void Vittoria(){
		guiText.text = "VITTORIA! \n" + "Hai sconfitto la malattia!";
		guiText.color = Color.green;
		gameOverCountDown = true;
		startTimer = true;
	}

	public void SemiVittoria(){
		guiText.text = "COMPLIMENTI\n" + 
					   "Hai riconosciuto la malattia, anche se non l'hai sconfitta...\n";
		guiText.color = Color.yellow;
		gameOverCountDown = true;
		startTimer = true;
	}

	public void fameCountDown(float time){ //countdown al gameover per fame


			int minuti = (int)time / 60;
			int secondi = (int)time % 60;

			if (secondi >= 10)
					guiText.text = "\t\t\t\tHAI FAME!\n" + "MANGIA ENTRO: " + minuti + ":" + secondi;
			else 
					guiText.text = "\t\t\t\tHAI FAME!\n" + "MANGIA ENTRO: " + minuti + ":" + "0" + secondi;

	}

	public void vomitoGUICountDown(float time) {
		if (startvomitoTimer) { 
				int minuti = (int)time / 60;
				int secondi = (int)time % 60;

				if (secondi >= 10)
						vomitoGUIText.text = "VOMITA! " + minuti + ":" + secondi;
				else 
						vomitoGUIText.text = "VOMITA! " + minuti + ":" + "0" + secondi;
		} else 
				vomitoGUIText.text = null;
	}

	public void pesoGUICountDown(float time) {
		if (startpesoTimer) { 
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

	private void handleTimers(){

		if (gameOverCountDown) {
						orologio = 0;

						if (startTimer)
								timer -= Time.deltaTime;
						
				} else 
						orologio -= Time.deltaTime;

		if (startvomitoTimer){
			vomitoTimer -= Time.deltaTime;
			if (vomitoTimer < 0){ 
				PlayerObject.getInstance.points++;
				PlayerObject.getInstance.ansia.Level += 15;
				startvomitoTimer = false;
				vomitoTimer = 30;

				//Se non vomito entro il tempo richiesto mi viene chiesto di allenamri
				allenamentoTimer = 30;
				startAllenamentoTimer = true;
			}
		}

		if (startpesoTimer) {
			pesoTimer -= Time.deltaTime;

			if (pesoTimer<0) {
				PlayerObject.getInstance.points++;
				PlayerObject.getInstance.ansia.Level += 15;
				startpesoTimer = false;
				pesoTimer = 30;
			}
		}

		if (startAllenamentoTimer) {
			Debug.Log (allenamentoTimer);
			allenamentoTimer -= Time.deltaTime;

			if (allenamentoTimer<30) {
				allenamentoGUIText.text = "\tALLENATI!\n" + "0:" + (int)allenamentoTimer;
			}

			else if (allenamentoTimer<0) {
				PlayerObject.getInstance.ansia.Level+=15;
				PlayerObject.getInstance.points++;
				startAllenamentoTimer = false;
			}
		}

		if (timer < 0) 
			Application.Quit ();

	}
	// Use this for initialization
	void Start ()
	{

		timer = 3;
		orologio = 1200;


		/*Carica le immagini del puntator0e come texture
		(L'immagin verrà ovviamente
		sostituita dagli artisti con una manina :) )*/

		puntatore =(Texture)Resources.Load("2D/puntatore");
		player = GameObject.Find("First Person Controller").GetComponent<Player>();


		ansia1 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia1");
		ansia2 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia2");
		ansia3 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia3");
		ansia4 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia4");
		ansia5 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia5");
		ansia6 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia6");
		ansia7 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia7");
		ansia8 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia8");
		ansia9 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia9");
		ansia10 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia10");
		ansia11 = (Texture)Resources.Load ("2D/AnsiaGUI/ansia11");

		fame1 = (Texture)Resources.Load ("2D/FameGUI/fame1");
		fame2 = (Texture)Resources.Load ("2D/FameGUI/fame2");
		fame3 = (Texture)Resources.Load ("2D/FameGUI/fame3");
		fame4 = (Texture)Resources.Load ("2D/FameGUI/fame4");

		guiTexture = GetComponent<GUITexture> ();
		guiText = GetComponent<GUIText> ();


		GameObject fameGUI = new GameObject("fameGUI");
		fameGUITexture = (GUITexture)fameGUI.AddComponent (typeof(GUITexture));
		fameGUITexture.transform.position = new Vector3 (0.1F, 0.9F, 0);
		fameGUITexture.transform.localScale = new Vector3 (0.1F, 0.2F, 0.2F);

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
		ansiaGUITexture = (GUITexture)ansiaGUI.AddComponent (typeof(GUITexture));
		ansiaGUITexture.transform.position = new Vector3 (0.8F, 0.9F, 0);
		ansiaGUITexture.transform.localScale = new Vector3 (0.2F, 0.2F, 0.2F);

		GameObject orologioGUI = new GameObject("orologioGUI");
		orologioGUIText = (GUIText)orologioGUI.AddComponent (typeof(GUIText));
		orologioGUIText.transform.position = new Vector3 (0.5F, 0.9F, 0);
		orologioGUIText.color = Color.red;
		orologioGUIText.fontStyle = FontStyle.Bold;
		orologioGUIText.fontSize = 20;


		GameObject pillolaGUI = new GameObject("pillolaGUI");
		pillolaGUIText = (GUIText)pillolaGUI.AddComponent (typeof(GUIText));
		pillolaGUIText.transform.position = new Vector3 (0.5F, 0.7F, 0);
		pillolaGUIText.color = Color.green;
		pillolaGUIText.fontStyle = FontStyle.Bold;

		GameObject allenamentoGUI = new GameObject("allenamentoGUI");
		allenamentoGUIText = (GUIText)allenamentoGUI.AddComponent (typeof(GUIText));
		allenamentoGUIText.transform.position = new Vector3 (0.5F, 0.7F, 0);
		allenamentoGUIText.color = Color.red;
		allenamentoGUIText.fontStyle = FontStyle.Bold;

		suBilancia = false;

		cibogui = new CiboGUI ();
		mobilegui = new MobileGUI ();
		bilanciagui = new BilanciaGUI ();
		watergui = new WaterGUI ();
		pillolagui = new PillolaGUI ();
		allenamentogui = new AllenamentoGUI ();

	} 



	// Update is called once per frame
	void Update () {
		if (canInteract) {//se posso interagire

			if (obj_string == "Cibo"){ //Se è un cibo
				cibogui.setInfo(ciboinfo);
				cibogui.setObj(obj);
				cibogui.handleInteraction();
			}
			else if (obj_string == "Mobile"){//Se è un oggetto movibile
				mobilegui.handleInteraction();
			}

			else if (obj_string == "Bilancia"){//Se è una bilancia
				bilanciagui.handleInteraction();
			}

			else if (obj_string == "Water"){//Se è un WATER
				watergui.handleInteraction();
			}

			else if (obj_string == "Pillola") {
				pillolagui.setObj(obj);
				pillolagui.handleInteraction();
			}

			else if (obj_string == "Allenamento") {
				allenamentogui.handleInteraction();
			}
		} 
		else { 
				guiText.text = null;
				guiTexture.texture = null;
				pillolaGUIText.text = null;
				allenamentoGUIText.text = null;
		}
		handleTimers();
		vomitoGUICountDown (vomitoTimer);
		pesoGUICountDown(pesoTimer);

		UpdateOrologioGUI (orologio);
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

public abstract class InteractiveObjectGUI {
	public abstract void handleInteraction();	
}


public class CiboGUI:InteractiveObjectGUI {
	private string nome;
	private int soddisfazione;
	private double calorie;
	private int riempimento;
	private GameObject cibo;

	public CiboGUI() {}

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


	public void setInfo(ArrayList info) {
		soddisfazione = (int)info[0];
		calorie = (double)info[1];
		riempimento = (int)info[2];
		nome = (string)info[3];

	}

	public void setObj(GameObject o) {
		cibo = o;
	}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture= GUIObject.getInstance.puntatore;
		/*Mostro infografica sul cibo*/
		GUIObject.getInstance.guiText.text =
			"\t\t\t" + nome + "\n\n" +
			"Soddisfazione: " + soddisfazione + "\n" + 
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

			//Ogni cibo ha un peso e un livello d'ansia specifico
			Cibo ciboscript = cibo.GetComponent<Cibo>();

			PlayerObject.getInstance.ansia.Level += ciboscript.levelansia;

			PlayerObject.getInstance.peso.Level = PlayerObject.getInstance.peso.Level + ciboscript.levelpeso;
		
			cibo.SetActive(false);
			PlayerObject.getInstance.points+=1;
			//Debug.Log (ciboscript.levelansia);

			GUIObject.getInstance.CanInteract(false);

	
		} 

	}
}

public class MobileGUI:InteractiveObjectGUI{
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

public class BilanciaGUI:InteractiveObjectGUI{
	public BilanciaGUI () {}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouse()) { //se sto premendo tasto sinistro....ng
			string levelstring = PlayerObject.getInstance.peso.Level.ToString();



			GUIObject.getInstance.pesoGUIText.text = "Peso: " + levelstring.Substring(0,4); //levelstring.Substring(0,4);


			//Debug.Log(GUIObject.getInstance.pesoGUIText.text);
			//Se mi peso blocco il countdown
			GUIObject.getInstance.startpesoTimer = false;
			GUIObject.getInstance.pesoTimer = 30;

			//risetto il timer 
			PlayerObject.getInstance.peso.Timer = (((int)(PlayerObject.getInstance.peso.Timer/180))*180)+1;


			//se il mio peso sulla bilancia è più alto dell'ultima volta sono grasso e aumenta l'ansia !
			if (PlayerObject.getInstance.peso.Level > PlayerObject.getInstance.peso.LastLevel)
			{
				PlayerObject.getInstance.ansia.Level += 20;

			}
			//altrimenti sono magro e diminuisce l'ansia
			else if(PlayerObject.getInstance.peso.Level < PlayerObject.getInstance.peso.LastLevel)
				PlayerObject.getInstance.ansia.Level -= 25;


			//l'ultimo livello di peso pesato sulla bulancia
			PlayerObject.getInstance.peso.LastLevel = PlayerObject.getInstance.peso.Level;
			GUIObject.getInstance.suBilancia = true;
		}
		
	}
}

public class WaterGUI:InteractiveObjectGUI{
	public WaterGUI() {}

	override public void handleInteraction(){
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouseHold ()) { //se sto premendo tasto sinistro vomito nel water



			//Se vomito risetto il timer del vomito
			GUIObject.getInstance.startvomitoTimer = false;
			GUIObject.getInstance.vomitoTimer= 30;




			//Vomito tante volte quanto ho mangiato: quando vomito l'ansia diminuisce di 5 (10? 15?)
			if (PlayerObject.getInstance.numcibi > 0){
				PlayerObject.getInstance.ansia.Level -=10;
				PlayerObject.getInstance.particleSystem.Play();
				PlayerObject.getInstance.numcibi--;
			}
			

		}

		else if (GUIObject.getInstance.GetLeftMouseReleased()) { //se sto premendo tasto sinistro vomito nel water
			
			Camera.main.particleSystem.Stop ();
			
			
		}


	}

}

public class PillolaGUI:InteractiveObjectGUI{
	private GameObject pillola;
	private Animator camerAnimator;

	public PillolaGUI() {}

	public void setObj(GameObject o) {
		pillola = o;
	}

	override public void handleInteraction(){
		GUIObject.getInstance.pillolaGUIText.text = "Antidepressivo";
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;
		if (GUIObject.getInstance.GetLeftMouseHold ()) { //se sto premendo tasto sinistro prendo la pillola
			PlayerObject.getInstance.numpillole++;

			//una volta presa la pillola scompare
			pillola.SetActive(false);

			GUIObject.getInstance.CanInteract(false);

			//Se prendo due pillole ansia -5
			PlayerObject.getInstance.ansia.Level -=PlayerObject.getInstance.numpillole*5;
			
		}
		
		
	}
	
}

public class AllenamentoGUI:InteractiveObjectGUI{
	private Animator camerAnimator;
	private bool maxFlessioni;
	private int numflessioni;
	private bool trainingMode;
	
	public AllenamentoGUI() {

		camerAnimator = Camera.main.GetComponent<Animator> ();
		numflessioni = 0;
	}
	
	override public void handleInteraction(){
		GUIObject.getInstance.allenamentoGUIText.text = "Tappetino: 10 flessioni!";
		GUIObject.getInstance.guiTexture.texture = GUIObject.getInstance.puntatore;

		Debug.Log (numflessioni);

		if (GUIObject.getInstance.GetLeftMouse ()) {
			if (trainingMode==true) { //se schiaccio tasto sinistro e sono in allenamento faccio una flessione 

				camerAnimator.Play("training");
				numflessioni=(int)camerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
				
				if (numflessioni == 10 &&  maxFlessioni == false) {
					PlayerObject.getInstance.ansia.Level-=5;
					maxFlessioni = true;
				}
				
				else if (numflessioni<10) {
					maxFlessioni = false;
					
				}
			}		
		
			else {
				trainingMode = true; //se non sono ancora in allenamento entro in allenamento col tasto sinistro
				camerAnimator.enabled = true;
				
				GUIObject.getInstance.startAllenamentoTimer = false;

			}	

		}

		else {

			camerAnimator.Play("trainingexit");
			
			if ((camerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime* 10) % 10 >=9) {
				trainingMode = false;
				camerAnimator.enabled = false;
			}
			
		}

	}
		
}
	
