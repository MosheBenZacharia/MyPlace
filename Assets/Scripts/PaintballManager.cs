using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class PaintballManager : ActivityManager
	{
		//readonly

		//Serialized
		[SerializeField]
		protected GameObject paintballPrefab;
		
		/////Protected/////
		//References
		//Primitives

		public override ActivityObject.ActivityObjectType ActivityType {
			get {
				return ActivityObject.ActivityObjectType.Paintball;
			}
		}

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
		// PaintballManager Functions
		//

		protected void CreatePaintball() {
			
			Color paintballColor = Color.HSVToRGB(UnityEngine.Random.value,.8f,1f);

			Vector3 spawnPosition = Input.Instance.GetCameraPosition() + Input.Instance.GetCameraTransform().right*.15f;
			GameObject paintballGameObject = Instantiate (paintballPrefab,spawnPosition,Quaternion.identity) as GameObject;

			Paintball paintball = paintballGameObject.GetComponent<Paintball>();
			paintball.Launch(Input.Instance.GetCameraForward(),paintballColor);
		}

		////////////////////////////////////////
		//
		// Function Functions

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from ActivityManager
		//
	

		public override void LaunchActivity () {

			CreatePaintball();
			
		}

		public override void StartActivity () {


		}

		public override void StopActivity () {


		}


	}
}