using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class BouncyBall : MonoBehaviour
	{
		//readonly

		//Serialized
		[SerializeField]
		protected float launchPower=100f;
		
		/////Protected/////
		//References
		//Primitives
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{

		}
		
		protected void Start ()
		{
			
		}
		
		protected void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// Paintball Functions
		//

		public void Launch(Vector3 direction, Color color) {

			GetComponent<Rigidbody>().AddForce(launchPower*direction);

			SetColor(color);
		}

		protected void SetColor(Color color) {

			GetComponentInChildren<MeshRenderer>().material.SetColor("_EmissionColor",color);
		}

		////////////////////////////////////////
		//
		// Function Functions

		protected void OnCollisionEnter(Collision collision) {

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.ImpactPunchRegular,transform.position);
		}

	}
}