using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;


namespace MyPlace
{
	public class Application : Singleton<Application>
	{
		//readonly

		//Serialized
		[SerializeField]
		protected bool captureSceenshots;

		/////Protected/////
		//References
		//Primitives
		//Static

		////////////////////////////////////////
		//
		// Properties


		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		protected void Awake () {

		}

		protected void Start () {
			
		}

		protected void Update () {
			
			if (UnityEngine.Input.GetKeyDown (KeyCode.Space) && captureSceenshots) {
				DateTime currentDateTime = System.DateTime.Now;
				string dateTimeString = "on_" + currentDateTime.ToShortDateString ().Replace ('/', '_') + "_at_" + currentDateTime.ToLongTimeString ().Replace (':', '_');
				string screenshotPath = string.Format ("Screenshot_{0}.png", dateTimeString);
				UnityEngine.Application.CaptureScreenshot (screenshotPath, 2);
			}
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// Application Functions
		//



	}
}