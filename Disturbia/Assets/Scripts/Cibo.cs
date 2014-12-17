using UnityEngine;
using System.Collections;


public class Cibo : MonoBehaviour {

	public int soddisfazione;
	public double calorie;
	public int riempimento;
	public int levelansia;
	public double levelpeso;
	/*
	public static int LevelAnsia {
		set {levelansia = value;}

		get {return levelansia;}

	}
*/
	public int Soddisfazione{
		set {
			if (soddisfazione>= 0 && soddisfazione<=100) 
				soddisfazione = value;
			else {
				if (value > 100)
					soddisfazione = 100;
				if (value < 0)
					soddisfazione = 0;
			}
		}
		get {return soddisfazione;}
	}

	public double Calorie{
		set {if (calorie>= 0) calorie = value;
			else calorie = 0;
		}
		get {return calorie;}
	}

	public int Riempimento{
		set {
			if (riempimento>= 0 && riempimento<=100) 
				riempimento = value;
			else {
				if (value > 100)
					riempimento = 100;
				if (value < 0)
					riempimento = 0;
			}
		}
		get {return riempimento;}
	}
	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
	
	}
}
