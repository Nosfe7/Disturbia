using UnityEngine;
using System.Collections;



public class Filmato : MonoBehaviour {
	MovieTexture movText;

	public float timer;

	// Use this for initialization
	void Start () {
		timer = Time.time;
		movText = (MovieTexture)renderer.material.mainTexture;
		movText.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!movText.isPlaying)
			Application.LoadLevel ("level0");
	}
}
