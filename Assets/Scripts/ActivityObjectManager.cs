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
			for (int i = 0; i < activityObjects.Length; ++i) {
				activityObjects[i].SelectedAction += OnActivityObjectSelected;
			}
		}
		
		protected void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// ActivityObjectManager Functions
		//

		protected void OnActivityObjectSelected(ActivityObject.ActivityObjectType activityObjectType) {

			switch(activityObjectType) {

			case ActivityObject.ActivityObjectType.Paintball:



				break;
			}
		}
		
		////////////////////////////////////////
		//
		// Function Functions

	}
}