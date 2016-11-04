using UnityEngine;
using System.Collections;

public class Mostro : MonoBehaviour {
	private NavMeshAgent agent;
	private Vector3 playerPos;
	private Vector3 relativePos;
	private ArrayList animations;
	private AnimationClip idle;
	private AnimationClip walking;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		animations = new ArrayList ();

		foreach(AnimationState state in animation)
		{
			Debug.Log(state.name);
			animations.Add(state.name);
		}
	}
	
	// Update is called once per frame
	void Update () {

		playerPos = PlayerObject.getInstance.transform.position;

		agent.SetDestination (playerPos);
		agent.updateRotation = true;

		//Il mostro si gira verso il giocatore
		relativePos = new Vector3 (transform.position.x - playerPos.x, 0.0f, transform.position.z - playerPos.z);
		
		Quaternion targetRotation = Quaternion.LookRotation (relativePos);	
		transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRotation, Time.deltaTime*10);


		if (agent.remainingDistance<agent.stoppingDistance && !PlayerObject.getInstance.isMoving()) {//Se il mostro è vicino e il giocatore è fermo..
			//Il mostro è fermo
			animation.Play((string)animations[0]);

		} else if (agent.remainingDistance>=agent.stoppingDistance) {//Se invece è lontano cammina
			animation.Play((string)animations[4]);
		}
		
		
	}

}
