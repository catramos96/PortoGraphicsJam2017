﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController2D : MonoBehaviour
{





	/*Colors
		(0, 0, 0, 1)
		(0, 0, 1, 1)
 		(0, 1, 1, 1)
 		(0, 1, 0, 1)
		(1, 0, 1, 1)
		(1, 0, 0, 1)
		(1, 0.92, 0.016, 1)
	*/




	private Rigidbody2D Rb2D;
	private Transform player_transform;
	private bool in_range;

	public float engageRange;
	public float velocity;
	public float minDist;

	int loboLife = 3;

	public DialogueScriptManager dsm;

	public Animator animator;

	public PlayerController2D script;

	SpriteRenderer renderer;

	GamePlayAudioManagement audioManager;

	// Use this for initialization
	void Start ()
	{

		if (gameObject.tag == "lobo") {
			animator = GetComponent<Animator> ();
		}

		audioManager = GameObject.FindGameObjectWithTag ("audio").GetComponent<GamePlayAudioManagement> ();
		Color[] colors = new Color[] 
		{	
			new Color(0, 0, 0, 1),
			new Color(0, 0, 1, 1),
			new Color(0, 1, 1, 1),
			new Color(0, 1, 0, 1),
			new Color(1, 0, 0, 1),
			new Color(1, 0.92f, 0.016f, 1),
		};
			

		player_transform = FindObjectOfType<PlayerController2D> ().transform;
		Rb2D = GetComponent<Rigidbody2D> ();

		renderer = GetComponent<SpriteRenderer> ();



		if (gameObject.tag == "Enemy") {
			renderer.color = new Color ((float)Random.Range(0,99)/100, 
				(float)Random.Range(0,99)/100, 
				(float)Random.Range(0,99)/100, 1);
		}


		renderer.enabled = true;


	


		if (player_transform.Equals(null))
			Debug.Log ("Couldn't find player transform");
	}
	
	// Update is called once per frame
	void Update ()
	{

		if(gameObject.tag == "lobo"){
			animator.SetFloat ("velocity", Rb2D.velocity.magnitude);


			animator.SetFloat ("x", Rb2D.velocity.x);
			animator.SetFloat ("y", Rb2D.velocity.y);
			
		}

		float distance = Vector3.Distance (transform.position, player_transform.position);

		if (distance <= engageRange && distance > minDist)
		{
			Vector2 vector_dir = (player_transform.position - transform.position) / distance;
			Rb2D.velocity = (vector_dir * velocity);

			//Audio
			audioManager.playFightMusic();
		}
		else
			Rb2D.velocity = Vector2.zero;
	}

	void OnTriggerEnter2D(Collider2D other){

		if(other.gameObject.tag == "PlayerAttack"){
			WasAttackedByPlayer ();

		}
		else if (other.gameObject.tag == "Player")
			audioManager.playMissPunchMusic ();

	}

	void WasAttackedByPlayer()
	{
		if (gameObject.tag == "lobo") {
			loboLife--;

			if (loboLife == 0) {
				script.endGame ();
				Destroy (gameObject);
				return;
			}
		} else {
			
			Destroy (gameObject);

		}
		audioManager.playMonsterDieMusic ();

	}
}