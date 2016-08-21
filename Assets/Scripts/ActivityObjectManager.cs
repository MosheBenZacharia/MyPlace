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
		protected Dictionary<ActivityObject.ActivityObjectType,ActivityManager> activityTypeToManager;
		//Primitives
		protected ActivityObject.ActivityObjectType currentActivityObjectType = ActivityObject.ActivityObjectType.None;
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{
			activityTypeToManager = new Dictionary<ActivityObject.ActivityObjectType, ActivityManager>();
			foreach (ActivityManager activityManager in GetComponentsInChildren<ActivityManager>()) {

				activityTypeToManager.Add(activityManager.ActivityType,activityManager);
			}
			Input.Instance.GlobalTriggerAction += OnGlobalTrigger;
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

		protected void ClearCurrentActivity() {

			switch(currentActivityObjectType) {

			case ActivityObject.ActivityObjectType.Paintball:

				Input.Instance.PaintballActivityCompleted();

				break;
			}

			if(activityTypeToManager.ContainsKey(currentActivityObjectType))
				activityTypeToManager[currentActivityObjectType].StopActivity();

			currentActivityObjectType = ActivityObject.ActivityObjectType.None;
		}
		
		////////////////////////////////////////
		//
		// Event Functions

		protected void OnGlobalTrigger() {

			if(activityTypeToManager.ContainsKey(currentActivityObjectType))
				activityTypeToManager[currentActivityObjectType].LaunchActivity();
		}

		protected void OnActivityObjectSelected(ActivityObject.ActivityObjectType activityObjectType) {

			switch(activityObjectType) {

			case ActivityObject.ActivityObjectType.Paintball:

				Input.Instance.PaintballActivityStarted();

				break;
			}

			if(activityTypeToManager.ContainsKey(currentActivityObjectType))
				activityTypeToManager[currentActivityObjectType].StartActivity();

			currentActivityObjectType = activityObjectType;
		}

	}
}