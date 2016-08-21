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
		[Header("PaintballManager")]
		[SerializeField]
		protected GameObject paintballPrefab;
		[SerializeField]
		protected LayerMask layerMask;
		[Header("BouncyBallManager")]
		[SerializeField]
		protected GameObject bouncyBallPrefab;
		
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

		public void CallCreatePaintball(PaintballManager paintballManager) {

			paintballManager.CmdCreatePaintball();

			foreach(PaintballManager paintballManagerFound in FindObjectsOfType<PaintballManager>() ) {
				paintballManagerFound.CmdCreatePaintball();
			}
//			CmdCreatePaintball();
		}

		[UnityEngine.Networking.Command]
		public void CmdCreatePaintball() {

			Vector3 spawnPosition = Input.Instance.GetCameraPosition() + Input.Instance.GetCameraTransform().right*.15f;
			Vector3 spawnDirection = Input.Instance.GetCameraForward();

			Ray ray = new Ray(spawnPosition,spawnDirection);
			RaycastHit raycastHit;
			if(Physics.Raycast(ray,out raycastHit,10f,layerMask)) {


			}else {
				Debug.Log("Could not find collider for paintball.");
				return;
			}


			Color paintballColor = Color.HSVToRGB(UnityEngine.Random.value,.8f,1f);

			GameObject paintballGameObject = Instantiate (paintballPrefab,spawnPosition,Quaternion.identity) as GameObject;
			//			Destroy(paintballGameObject,10f);


			Paintball paintball = paintballGameObject.GetComponent<Paintball>();
			paintball.Launch(raycastHit.point,paintballColor);

			NetworkServer.Spawn(paintballGameObject);

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.WhooshMedium);
		}

		public void CallCreateBouncyBall(BouncyBallManager bouncyBallManager) {

			bouncyBallManager.CmdCreateBouncyBall();
		}

		[UnityEngine.Networking.Command]
		public void CmdCreateBouncyBall() {

			Color bouncyBallColor = Color.HSVToRGB(UnityEngine.Random.value,.8f,1f);

			Vector3 spawnPosition = Input.Instance.GetCameraPosition() + Input.Instance.GetCameraTransform().right*.15f;
			GameObject bouncyBallGameObject = Instantiate (bouncyBallPrefab,spawnPosition,Quaternion.identity) as GameObject;
			Destroy(bouncyBallGameObject,10f);


			BouncyBall bouncyBall = bouncyBallGameObject.GetComponent<BouncyBall>();
			bouncyBall.Launch(Input.Instance.GetCameraForward(),bouncyBallColor);

						NetworkServer.Spawn(bouncyBallGameObject);

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.WhooshSmall);
		}
		
		////////////////////////////////////////
		//
		// Function Functions

	}
}