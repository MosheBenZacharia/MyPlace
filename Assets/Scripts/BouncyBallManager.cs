using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class BouncyBallManager : ActivityManager
	{
		//readonly

		//Serialized
		[SerializeField]
		protected GameObject bouncyBallPrefab;
		
		/////Protected/////
		//References
		//Primitives

		public override ActivityObject.ActivityObjectType ActivityType {
			get {
				return ActivityObject.ActivityObjectType.BouncyBall;
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

//		[UnityEngine.Networking.Command]
		public void CmdCreateBouncyBall() {
			
			Color bouncyBallColor = Color.HSVToRGB(UnityEngine.Random.value,.8f,1f);

			Vector3 spawnPosition = Input.Instance.GetCameraPosition() + Input.Instance.GetCameraTransform().right*.15f;
			GameObject bouncyBallGameObject = Instantiate (bouncyBallPrefab,spawnPosition,Quaternion.identity) as GameObject;
			Destroy(bouncyBallGameObject,10f);


			BouncyBall bouncyBall = bouncyBallGameObject.GetComponent<BouncyBall>();
			bouncyBall.Launch(Input.Instance.GetCameraForward(),bouncyBallColor);

//			NetworkServer.Spawn(bouncyBallGameObject);

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.WhooshSmall);
		}

		////////////////////////////////////////
		//
		// Function Functions

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from ActivityManager
		//
	

		public override void LaunchActivity () {
			

//			CmdCreateBouncyBall();
			FindObjectOfType<Player>().CallCreateBouncyBall(this);
		}

		public override void StartActivity () {


		}

		public override void StopActivity () {


		}


	}
}