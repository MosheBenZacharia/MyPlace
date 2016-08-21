using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public enum InteractiveGazeEventType
	{
		HitInteractable,
		HitCollider,
		HitNone
	}

	public struct InteractiveGazeEventData
	{
		public InteractiveGazeEventType eventType;
		public RaycastResult raycastResult;

		public InteractiveGazeEventData (InteractiveGazeEventType eventType, RaycastResult raycastResult)
		{
			this.eventType = eventType;
			this.raycastResult = raycastResult;
		}
	}

	public interface IInteractiveGazeHandler : IEventSystemHandler
	{
		void OnInteractiveGaze (System.Collections.Generic.List<InteractiveGazeEventData> data);
	}

	/// <summary>
	/// An element that scales (size/color/etc) based on distance from the crosshair.
	/// </summary>
	public interface IDistScaleElement : IEventSystemHandler
	{
		/// <summary>
		/// The position of the crosshair has changed, this is where the object would scale.
		/// </summary>
		/// <param name="position">Position.</param>
		void OnDistScalePositionChanged (Vector3 position);
	}

	/// <summary>
	/// Input does a bunch of input related tasks.
	/// Also manages the crosshair.
	/// </summary>
	public class Input : Singleton<Input>
	{
		//enum
		public enum UserInteractionState
		{
			Enabled,
			Gaze,
			Disabled
		}

		public static bool allowButtonHoverFill=false;

		//Serialized
		[SerializeField]
		protected GazeInputModule gazeInputModule;
		[SerializeField] 
		protected Crosshair crosshair;
		
		/////Protected/////
		//References
		protected List<GameObject> interactiveGazeListeners;
		protected List<GameObject> distScaleListeners;
		//Primitives
		protected UserInteractionState currentUserInteractionState = UserInteractionState.Enabled;
		
		//Actions
		public Action GlobalTriggerAction {get; set;}
		public Action BackAction { get; set; }

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		protected void Awake () {
			interactiveGazeListeners = new List<GameObject> ();
			distScaleListeners = new List<GameObject> ();
		}

		protected void Start () {
			if (gazeInputModule == null)
				gazeInputModule = GetComponentInChildren<GazeInputModule> ();

			gazeInputModule.InteractiveGazeAction += OnInteractiveGaze;
			gazeInputModule.GlobalTriggerAction += OnGlobalTrigger;

			if (crosshair == null)
				crosshair = GetComponentInChildren<Crosshair> ();

			crosshair.PositionChangedAction += OnCrosshairPositionChanged;

		}

		protected void Update () {

			if (UnityEngine.Input.GetKeyDown (KeyCode.Escape) || OVRInput.GetDown (OVRInput.Button.Two)) {
				OnBack ();
			}
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Input Functions
		//

		public void EnableUserInput() {

			currentUserInteractionState = UserInteractionState.Enabled;
			OnUserInteractionStateUpdated ();
		}

		public void DisableUserInput() {
			
			currentUserInteractionState = UserInteractionState.Gaze;
			OnUserInteractionStateUpdated ();
		}

		protected void OnUserInteractionStateUpdated() {

//			switch (currentUserInteractionState) {
//
//			case UserInteractionState.Enabled:
//				
//				gazeInputModule.CanCastRayFromGaze = true;
//				gazeInputModule.CanHandleTrigger = true;
//				break;
//
//			case UserInteractionState.Gaze:
//
//				gazeInputModule.CanCastRayFromGaze = true;
//				gazeInputModule.CanHandleTrigger = false;
//				break;
//			}
		}

		////////////////////////////////////////
		//
		// Scrolling Functions

		public void OnScrollingStarted() {

			this.crosshair.SetState (Crosshair.CrosshairState.Scrolling);
		}

		public void OnScrollingStopped() {

			this.crosshair.SetState (Crosshair.CrosshairState.Open);
		}

		////////////////////////////////////////
		//
		// Waypoint Functions

		public void SetWaypoint(Transform transform) {

			crosshair.SetWaypoint (transform);
		}

		public void SetWaypoint(Vector3 position) {

			crosshair.SetWaypoint (position);
		}

		public void ClearWaypoint() {

			crosshair.ClearWaypoint ();
		}

		////////////////////////////////////////
		//
		// Like Functions

		public void OnLikeLaunchingStarted() {

			this.crosshair.SetState (Crosshair.CrosshairState.Liking);
		}

		public void OnLikeLaunchingStopped() {

			this.crosshair.SetState (Crosshair.CrosshairState.Open);
		}

		////////////////////////////////////////
		//
		// Activity Functions

		public void PaintballActivityStarted() {

			crosshair.SetState (Crosshair.CrosshairState.Paintball);
		}

		public void PaintballActivityCompleted() {

			crosshair.SetState (Crosshair.CrosshairState.Open);
		}

		////////////////////////////////////////
		//
		// Wipe Swipe Functions

		public void WipeSwipeStarted() {

			crosshair.SetState (Crosshair.CrosshairState.WipeSwipe);
		}

		public void WipeSwipeCompleted() {

			crosshair.SetState (Crosshair.CrosshairState.Open);
		}

		public void SetWipeSwipePercent(float percent) {

			crosshair.SetWipeSwipePercent(percent);
		}

		////////////////////////////////////////
		//
		// Event Functions

		/// <summary>
		/// Forces the global trigger, used for on rails onboarding.
		/// </summary>
		public void ForceGlobalTrigger() {

			OnGlobalTrigger();
		}

		protected void OnGlobalTrigger() {

			if (GlobalTriggerAction != null)
				GlobalTriggerAction ();
		}

		protected void OnBack() {
			if (currentUserInteractionState != UserInteractionState.Enabled)
				return;

			if (BackAction != null)
				BackAction ();
		}

		////////////////////////////////////////
		//
		// Waypoint Functions

		public void SetWaypointFocus(Vector3 position) {


		}

		////////////////////////////////////////
		//
		// Gaze Functions

		public void RegisterInteractiveGazeGameObject (GameObject gameObjectToRegister) {
			if (!interactiveGazeListeners.Contains (gameObjectToRegister))
				interactiveGazeListeners.Add (gameObjectToRegister);
		}

		protected void OnInteractiveGaze (List<InteractiveGazeEventData> interactiveGazeEventDataList) {
			
			//Fire the interactive gaze events
			foreach (GameObject swipeListenerGameObject in interactiveGazeListeners)
				ExecuteEvents.Execute<IInteractiveGazeHandler> (swipeListenerGameObject, null, (x, y) => x.OnInteractiveGaze (interactiveGazeEventDataList));
		}

		////////////////////////////////////////
		//
		// Crosshair Functions

		public Vector3 GetCameraPosition() {

			return Camera.main.transform.position;
		}

		public Vector3 GetCameraForward() {

			return Camera.main.transform.forward;
		}

		public Transform GetCameraTransform() {

			return Camera.main.transform;
		}

		public void RegisterDistScaleGameObject (GameObject gameObjectToRegister,bool deregister = false) {
			if(distScaleListeners ==null)
				distScaleListeners = new List<GameObject> ();
			if (deregister) {
				if (distScaleListeners.Contains (gameObjectToRegister))
					distScaleListeners.Remove (gameObjectToRegister);
			} else {
				if (!distScaleListeners.Contains (gameObjectToRegister))
					distScaleListeners.Add (gameObjectToRegister);
			}
		}

		protected void OnCrosshairPositionChanged (Vector3 position) {
			//Fire the distance scale events
			foreach (GameObject swipeListenerGameObject in distScaleListeners)
				ExecuteEvents.Execute<IDistScaleElement> (swipeListenerGameObject, null, (x, y) => x.OnDistScalePositionChanged (position));
		}

	}
}