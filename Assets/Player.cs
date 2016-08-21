using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class Player : NetworkBehaviour
	{
		
		//readonly

		//Serialized
		
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
		
			if(isLocalPlayer) {
				transform.parent = GameObject.Find("CenterEyeAnchor").transform;
				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;

				GetComponentInChildren<MeshRenderer>().enabled = false;
			}
		}
		
		protected void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// Player Functions
		//

		
		////////////////////////////////////////
		//
		// Function Functions

	}
}