using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UIPrimitives;

namespace MyPlace
{
	public class FarmAnimal : MonoBehaviour
	{
		//readonly

		//Serialized


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
			
//			this.endPosition = position;
			this.transform.position = position;
			this.transform.rotation = Quaternion.Euler(0,UnityEngine.Random.Range(0,360f),0);
//			this.myColor = color;
			GetComponent<UIPrimitives.UITransformAnimator>().AddScaleEndAnimation(Vector3.zero,10f,UIAnimationUtility.EaseType.easeInCirc,DestroyMe);
//			StartCoroutine(Splat(animationDuration));
//			SetColor (color);
		}

		protected void DestroyMe() {
			Destroy(this.gameObject);
		}

//		protected void SetColor (Color color) {
//
//			GetComponentInChildren<ParticleSystem> ().startColor = Color.Lerp(color,Color.white,.5f);
//			GetComponentInChildren<MeshRenderer> ().material.SetColor ("_EmissionColor", color);
//		}

		////////////////////////////////////////
		//
		// Function Functions

		protected void OnCollisionEnter(Collision collision) {

			Audio.Instance.PlaySoundEffect(Audio.SoundEffect.ImpactPunchRegular,transform.position);
		}
	}
}