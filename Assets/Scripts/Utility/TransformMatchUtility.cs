using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
	public class TransformMatchUtility : MonoBehaviour
	{
		//enum

		//readonly

		//Serialized
		[SerializeField]
		protected Transform targetTransform;
		[Header("Settings")]
		[SerializeField]
		protected Space sourceSpace;
		[SerializeField]
		protected Space targetSpace;
		[Header("Match")]
		[SerializeField]
		protected bool matchPosition;
		[SerializeField]
		protected bool matchRotation;
		[SerializeField]
		protected bool matchScale;


		
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
			
		}
		
		protected void Update ()
		{

			if(matchPosition) {

				Vector3 targetPosition = Vector3.zero;
				//Get target value
				if(targetSpace == Space.World) {
					targetPosition = targetTransform.position;
				} else if (targetSpace == Space.Self) {
					targetPosition = targetTransform.localPosition;
				}
				//Set my value
				if(sourceSpace == Space.World) {
					this.transform.position = targetPosition;
				} else if (sourceSpace == Space.Self) {
					this.transform.localPosition = targetPosition;
				}
			}
			if(matchRotation) {

				Quaternion targetRotation = Quaternion.identity;
				//Get target value
				if(targetSpace == Space.World) {
					targetRotation = targetTransform.rotation;
				} else if (targetSpace == Space.Self) {
					targetRotation = targetTransform.localRotation;
				}
				//Set my value
				if(sourceSpace == Space.World) {
					this.transform.rotation = targetRotation;
				} else if (sourceSpace == Space.Self) {
					this.transform.localRotation = targetRotation;
				}
			}
			if(matchScale) {

				Vector3 targetScale = Vector3.one;
				//Get target value
				if(targetSpace == Space.World) {
					targetScale = targetTransform.lossyScale;
				} else if (targetSpace == Space.Self) {
					targetScale = targetTransform.localScale;
				}
				//Set my value (can't set lossy scale)
				this.transform.localScale = targetScale;
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// TransformMatchUtility Functions
		//

		
		////////////////////////////////////////
		//
		// Function Functions

	}
}