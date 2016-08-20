using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class ActivityObjectManager : MonoBehaviour
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

			ActivityObject[] activityObjects = FindObjectsOfType<ActivityObject>();
		}
		
		protected void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// ActivityObjectManager Functions
		//

		
		////////////////////////////////////////
		//
		// Function Functions

	}
}