using UnityEngine;
using System.Collections;





public class Menu : MonoBehaviour {
	public states state;
	public states previous;
	public enum states {NONE,HOME, CONTINUE, MAP, REFERENCES, EXTRA, CREDITS, REWARDS, GAME};
	private MenuType menu;

	public Texture home;
	public Texture cont;
	public Texture references;
	public Texture map;
	public Texture extra;
	public Texture credits;
	public Texture rewards;

	public GameObject b1;
	public GameObject b4;
	public GameObject b5;
	public GameObject b6;
	public GameObject b7;
	public GameObject b8;
	public GameObject b9;
	public GameObject b10;
	public GameObject b11;


	// Use this for initialization
	void Start () {
		state = states.HOME;
		home = (Texture)Resources.Load ("2D/Menu/Home");
		cont = (Texture)Resources.Load ("2D/Menu/Continue");
		references = (Texture)Resources.Load ("2D/Menu/RefBack");
		map = (Texture)Resources.Load ("2D/Menu/MapBack");
		extra = (Texture)Resources.Load ("2D/Menu/Extra");
		credits = (Texture)Resources.Load ("2D/Menu/Credits");
		rewards = (Texture)Resources.Load ("2D/Menu/Rewards");

		previous = states.NONE;
		b1 = GameObject.Find ("Button1");
		b4 = GameObject.Find ("Button4");
		b5 = GameObject.Find ("Button5");
		b6 = GameObject.Find ("Button6");
		b7 = GameObject.Find ("Button7");
		b8 = GameObject.Find ("Button8");
		b9 = GameObject.Find ("Button9");
		b10 = GameObject.Find ("Button10");
		b11 = GameObject.Find ("Button11");
	}
	
	// Update is called once per frame
	void Update () {
		if (state != previous) {
			switch (state) { //da uno stato indico a quali altri stati posso giungere
				case states.HOME:
					menu = new MenuType(home,state, new states[2]{states.CONTINUE, states.EXTRA});
					break;
				case states.CONTINUE:
					menu = new MenuType(cont,state, new states[4]{states.GAME,states.MAP,states.REFERENCES, states.HOME});
					break;
				case states.MAP:
					menu = new MenuType(map,state, new states[1]{states.CONTINUE});
					break;
				case states.REFERENCES:
					menu = new MenuType(references,state, new states[1]{states.CONTINUE});;
					break;
				case states.EXTRA:
					menu = new MenuType(extra,state, new states[3]{states.CREDITS, states.REWARDS, states.HOME});
					break;
				case states.CREDITS:
					menu = new MenuType(credits,state, new states[1]{states.EXTRA});
					break;
				case states.REWARDS:
					menu = new MenuType(rewards,state, new states[1]{states.EXTRA});
					break;
				case states.GAME:
					Application.LoadLevel("level1");
					break;	
			}

			previous = state;
		}

		if (Input.GetKey (KeyCode.Escape)) 
			Application.Quit ();


		menu.handleMenu ();
	}

	public bool GetDownArrow(){ //Controlla se ho premuto freccia giù
		return Input.GetKeyDown(KeyCode.DownArrow);
	}

	public bool GetUpArrow() //Controlla se ho premuto freccia su
	{
		return Input.GetKeyDown(KeyCode.UpArrow);
	}

	public bool GetEnter() //Controlla se ho premuto Enter
	{
		return Input.GetKeyDown(KeyCode.Return);
	}
}

public class MenuSingletone{
	private static Menu instance;
	
	
	public static Menu getInstance {
		get {
			if (instance == null)
				instance = GameObject.Find("Menu").GetComponent<Menu>();
			return instance;
		}
		
	}
}


class MenuType{
	private int selIndex;
	private Button[] buttons = {new Button (), new Button(), new Button(), new Arrow()};
	private Menu.states  state;
	private Menu.states[] nextStates;
	public GameObject[] objs;

	public MenuType(Texture texture, Menu.states s, Menu.states[] nstates) {
		GameObject menuObj = MenuSingletone.getInstance.gameObject;
		menuObj.renderer.material.SetTexture ("_MainTex", texture);
		state = s;

		if (state == Menu.states.HOME) {
			MenuSingletone.getInstance.b1.renderer.enabled = true;
			MenuSingletone.getInstance.b4.renderer.enabled = false;
			MenuSingletone.getInstance.b5.renderer.enabled = false;
			MenuSingletone.getInstance.b6.renderer.enabled = false;
			MenuSingletone.getInstance.b7.renderer.enabled = false;
			MenuSingletone.getInstance.b8.renderer.enabled = true;
			MenuSingletone.getInstance.b9.renderer.enabled = true;
			MenuSingletone.getInstance.b10.renderer.enabled = false;
			MenuSingletone.getInstance.b11.renderer.enabled = false;

			buttons = new Button[3] {new Button(),new Button(), new Button()};


			objs = new GameObject[3] { MenuSingletone.getInstance.b1, MenuSingletone.getInstance.b8, MenuSingletone.getInstance.b9};
		} 

		else if (state == Menu.states.CONTINUE) {
			MenuSingletone.getInstance.b1.renderer.enabled = false;
			MenuSingletone.getInstance.b4.renderer.enabled = true;
			MenuSingletone.getInstance.b5.renderer.enabled = true;
			MenuSingletone.getInstance.b6.renderer.enabled = true;
			MenuSingletone.getInstance.b7.renderer.enabled = true;
			MenuSingletone.getInstance.b8.renderer.enabled = false;
			MenuSingletone.getInstance.b9.renderer.enabled = false;
			MenuSingletone.getInstance.b10.renderer.enabled = false;
			MenuSingletone.getInstance.b11.renderer.enabled = false;

			objs = new GameObject[4] { 
				MenuSingletone.getInstance.b4,
				MenuSingletone.getInstance.b5,
				MenuSingletone.getInstance.b6,
				MenuSingletone.getInstance.b7
			};
		}

		else if (state == Menu.states.MAP) {
			MenuSingletone.getInstance.b1.renderer.enabled = false;
			MenuSingletone.getInstance.b4.renderer.enabled = false;
			MenuSingletone.getInstance.b5.renderer.enabled = false;
			MenuSingletone.getInstance.b6.renderer.enabled = false;
			MenuSingletone.getInstance.b7.renderer.enabled = true;
			MenuSingletone.getInstance.b8.renderer.enabled = false;
			MenuSingletone.getInstance.b10.renderer.enabled = false;
			MenuSingletone.getInstance.b11.renderer.enabled = false;
			
			objs = new GameObject[1] { 
				MenuSingletone.getInstance.b7
			};

			buttons = new Button[1] {new Arrow()};

		}

		else if (state == Menu.states.REFERENCES) {
			MenuSingletone.getInstance.b1.renderer.enabled = false;
			MenuSingletone.getInstance.b4.renderer.enabled = false;
			MenuSingletone.getInstance.b5.renderer.enabled = false;
			MenuSingletone.getInstance.b6.renderer.enabled = false;
			MenuSingletone.getInstance.b7.renderer.enabled = true;
			
			objs = new GameObject[1] { 
				MenuSingletone.getInstance.b7
			};
			
			buttons = new Button[1] {new Arrow()};
			
		}

		else if (state == Menu.states.EXTRA) {
			MenuSingletone.getInstance.b1.renderer.enabled = false;
			MenuSingletone.getInstance.b4.renderer.enabled = false;
			MenuSingletone.getInstance.b5.renderer.enabled = false;
			MenuSingletone.getInstance.b6.renderer.enabled = false;
			MenuSingletone.getInstance.b7.renderer.enabled = true;
			MenuSingletone.getInstance.b8.renderer.enabled = false;
			MenuSingletone.getInstance.b9.renderer.enabled = false;
			MenuSingletone.getInstance.b10.renderer.enabled = true;
			MenuSingletone.getInstance.b11.renderer.enabled = true;



			objs = new GameObject[3] { 
				MenuSingletone.getInstance.b10,
				MenuSingletone.getInstance.b11,
				MenuSingletone.getInstance.b7
			};

			buttons = new Button[3] {new Button(), new Button(), new Arrow()};
			
		}

		else if (state == Menu.states.CREDITS) {
			MenuSingletone.getInstance.b1.renderer.enabled = false;
			MenuSingletone.getInstance.b4.renderer.enabled = false;
			MenuSingletone.getInstance.b5.renderer.enabled = false;
			MenuSingletone.getInstance.b6.renderer.enabled = false;
			MenuSingletone.getInstance.b7.renderer.enabled = true;
			MenuSingletone.getInstance.b8.renderer.enabled = false;
			MenuSingletone.getInstance.b9.renderer.enabled = false;
			MenuSingletone.getInstance.b10.renderer.enabled = false;
			MenuSingletone.getInstance.b11.renderer.enabled = false;
			
			
			objs = new GameObject[1] { 
				MenuSingletone.getInstance.b7
			};
			
			buttons = new Button[1] {new Arrow()};
			
		}

		else if (state == Menu.states.REWARDS) {
			MenuSingletone.getInstance.b1.renderer.enabled = false;
			MenuSingletone.getInstance.b4.renderer.enabled = false;
			MenuSingletone.getInstance.b5.renderer.enabled = false;
			MenuSingletone.getInstance.b6.renderer.enabled = false;
			MenuSingletone.getInstance.b7.renderer.enabled = true;
			MenuSingletone.getInstance.b8.renderer.enabled = false;
			MenuSingletone.getInstance.b9.renderer.enabled = false;
			MenuSingletone.getInstance.b10.renderer.enabled = false;
			MenuSingletone.getInstance.b11.renderer.enabled = false;
			
			
			objs = new GameObject[1] { 
				MenuSingletone.getInstance.b7
			};
			
			buttons = new Button[1] {new Arrow()};
			
		}



		int i=0;
		foreach (Button button in buttons) {
			button.obj = objs[i];
			i++;
		}

		selIndex = 0;
		nextStates = nstates;
	}
	
	public void handleMenu() {

		foreach (Button button in buttons) {
			button.handleColor();
		}

		buttons[selIndex].isSelected = true;

		if (MenuSingletone.getInstance.GetDownArrow()) {
			buttons[selIndex].isSelected = false;
			selIndex++;
			if (selIndex >buttons.Length-1)
				selIndex = 0;


		}

		if (MenuSingletone.getInstance.GetUpArrow ()) {
			buttons[selIndex].isSelected = false;
			selIndex--;
			if (selIndex <0)
				selIndex = buttons.Length-1;
		}

		if (MenuSingletone.getInstance.GetEnter()) {
			MenuSingletone.getInstance.state = nextStates[selIndex];

		}
	}
}

class Button {
	public bool isSelected;
	public GameObject obj; 

	public Button() {

	}
	
	public void handleColor(){
		if (isSelected) { 
						obj.renderer.material.SetColor ("_Color", Color.red);
				} else { 
						obj.renderer.material.SetColor ("_Color", Color.gray);
				}
	}
}

class Arrow:Button {
	private Texture frecciaIdle;
	private Texture frecciaClick;


	public Arrow() {
		frecciaIdle = (Texture)Resources.Load("2D/Menu/Freccia1");
		frecciaClick = (Texture)Resources.Load("2D/Menu/Freccia2");
	}

	public new void handleColor(){
		if (isSelected) { 
			obj.renderer.material.SetTexture("_MainTex",frecciaClick); 
		} else { 
			obj.renderer.material.SetTexture("_MainTex",frecciaIdle);
		}
	}
}


