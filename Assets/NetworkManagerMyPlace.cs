using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class NetworkManagerMyPlace : MonoBehaviour
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
			if(UnityEngine.Application.platform == RuntimePlatform.Android) {

				GetComponent<UnityEngine.Networking.NetworkManager>().StartHost();
			}
		}
		
		protected void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// NetworkManagerMyPlace Functions
		//

		
		////////////////////////////////////////
		//
		// Function Functions

	}
}