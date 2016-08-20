using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Utility
{
	public class AnimatedSpriteUtility : MonoBehaviour
	{
		//readonly

		//Serialized
		public float updateInterval = .013f;
		public bool sharedMaterial;
		public bool pingPong;
		public string propertyName = "_MainTex";
		public bool autoIncrement = true;
		
		/////Protected/////
		//References
		protected Material material;
		//Primitives
		protected Vector2 offsetInterval;
		protected int totalIntervals;
		protected float lastUpdateTime;
		protected int resetIndex;
		protected int direction = 1;
		public int myIndex = 0;
		
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{
			if(sharedMaterial)
				material = GetComponent<MeshRenderer>().sharedMaterial;
			else
				material = GetComponent<MeshRenderer>().material;

			offsetInterval = material.GetTextureScale(propertyName);
			totalIntervals = (int) (1f/offsetInterval.x);
			resetIndex = totalIntervals*totalIntervals;
		}
		
		protected void Start ()
		{

		}
		
		protected void Update ()
		{
			
			if (autoIncrement && Time.time - lastUpdateTime > updateInterval) {

				float timeElapsed = Time.time - this.lastUpdateTime;
				
				this.myIndex += direction*((int) (timeElapsed/updateInterval));
				
				if (this.myIndex >= this.resetIndex && direction > 0) {
					if(this.pingPong)
					{
						this.direction = -1;
					}
					else
					{
						this.myIndex = 0;
					}
				}
				if (this.myIndex <= 0 && direction < 0) {
					if(this.pingPong)
					{
						this.direction = 1;
					}
				}

				OnIndexUpdated();

				this.lastUpdateTime = Time.time;
			}
//			Debug.Log (this.myIndex);
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// Utility_AnimatedSprite Functions
		//

		protected void OnIndexUpdated() {

			int x = 0;
			int y = 0;

			if(this.myIndex!=0)
			{
				x = this.myIndex%(totalIntervals);
				y = this.myIndex/(totalIntervals);
			}

			SetTextureOffset(Vector2.Scale(offsetInterval,new Vector2(x,totalIntervals- (y+1))));
		}

		protected void SetTextureOffset(Vector2 offset) {
			this.material.SetTextureOffset(propertyName, offset);
		}

		public void SetPercent(float percent) {

//			Debug.Log(percent);
			this.myIndex = (int) (percent*((float) (this.resetIndex-1)));
			OnIndexUpdated();
		}
	}
}