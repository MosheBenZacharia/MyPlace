using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public abstract class ActivityManager : MonoBehaviour
	{
		//readonly


		//Serialized
		
		/////Protected/////
		//References
		//Primitives

		public abstract ActivityObject.ActivityObjectType ActivityType {
			get;
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake () {

		}

		protected void Start () {
			
		}

		protected void Update () {
			
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// ActivityManager Functions
		//

		public abstract void StartActivity ();

		public abstract void StopActivity () ;
		public abstract void LaunchActivity() ;
		
		////////////////////////////////////////
		//
		// Function Functions

	}
}