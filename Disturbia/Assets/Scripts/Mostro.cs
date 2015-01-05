using UnityEngine;
using System.Collections;

public class Mostro : MonoBehaviour {
	private NavMeshAgent agent;
	private Vector3 playerPos;
	private Vector3 relativePos;
	private Animator animator;
	//Vector3 previous_direction;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		playerPos = PlayerObject.getInstance.transform.position;

		agent.SetDestination (playerPos);
		agent.updateRotation = true;


		if (agent.remainingDistance<agent.stoppingDistance) {//Se il mostro è vicino..
			//Il mostro è fermo
			animator.Play("idle");

			//Il mostro si gira verso il giocatore
			relativePos = new Vector3 (transform.position.x - playerPos.x, 0.0f, transform.position.z - playerPos.z);

			Quaternion targetRotation = Quaternion.LookRotation (-relativePos);

			transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotation, Time.deltaTime*10);

				} else {//Se invece è lontano cammina
						animator.Play("walking");

				}
		
		
	}

}
