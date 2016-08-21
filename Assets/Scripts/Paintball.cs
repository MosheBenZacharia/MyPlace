using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using UIPrimitives;

namespace MyPlace
{
	public class Paintball : UnityEngine.Networking.NetworkBehaviour
	{
		//readonly

		//Serialized
		[SerializeField]
		protected float animationDuration = 2f;
		[SerializeField]
		protected GameObject splatPrefab;


		/////Protected/////
		//References
		protected Vector3 endPosition;

		//Primitives
		protected Color myColor;

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
		// Paintball Functions
		//

		public void Launch (Vector3 position, Color color) {
			
			this.endPosition = position;
			this.myColor = color;
			GetComponent<UIPrimitives.UITransformAnimator>().AddPositionEndAnimation(position,animationDuration,UIAnimationUtility.EaseType.easeInCirc,CmdSplat);
//			StartCoroutine(Splat(animationDuration));
			SetColor (color);
		}

		protected void SetColor (Color color) {

			GetComponentInChildren<ParticleSystem> ().startColor = Color.Lerp(color,Color.white,.5f);
			GetComponentInChildren<MeshRenderer> ().material.SetColor ("_EmissionColor", color);
		}

		[UnityEngine.Networking.Command]
		protected void CmdSplat() {
			
//			yield return new WaitForSeconds(duration);

			float splatDuration = 10f;

			Vector3 splatPosition = Vector3.Lerp(endPosition,Input.Instance.GetCameraPosition(),.05f);

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.Splat,splatPosition);

			GameObject splatGameObject = Instantiate(splatPrefab,splatPosition,Quaternion.identity) as GameObject;
			splatGameObject.transform.LookAt(Input.Instance.GetCameraPosition());
			splatGameObject.transform.Rotate(0,180f,0);
			splatGameObject.GetComponentInChildren<MeshRenderer>().sharedMaterial.color = this.myColor;
			splatGameObject.GetComponentInChildren<MaterialAnimator>().AddColorEndAnimation(this.myColor.Alpha(0),duration:splatDuration,easeType:UIAnimationUtility.EaseType.easeInCirc);

			NetworkServer.Spawn(splatGameObject);

			Destroy(splatGameObject,splatDuration);
			Destroy(this.gameObject);
		}

		////////////////////////////////////////
		//
		// Function Functions

		protected void OnCollisionEnter(Collision collision) {

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.ImpactPunchRegular,transform.position);
		}
	}
}