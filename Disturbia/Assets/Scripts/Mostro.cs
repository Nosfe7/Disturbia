using UnityEngine;
using System.Collections;

public class Mostro : MonoBehaviour {
	private Vector3 offset;
	//Vector3 previous_direction;
	// Use this for initialization
	void Start () {
		offset =  transform.position - Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width/2,Screen.height/2,0));

	}
	
	// Update is called once per frame
	void Update () {

		/*if (!Camera.main.transform.forward.Equals(previous_direction)){
		    transform.RotateAround (Camera.main.transform.position, Camera.main.transform.up, Camera.main.transform.rotation.eulerAngles.y);
		}
		previous_direction = Camera.main.transform.forward;
		 */
		transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Screen.width/2,Screen.height/2,0)) + offset;
			

	}

}
