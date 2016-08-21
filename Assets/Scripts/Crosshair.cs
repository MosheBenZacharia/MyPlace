using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class Crosshair : Singleton<Crosshair>, IInteractiveGazeHandler
	{
		//readonly
		private readonly float CROSSHAIR_MIN_DISTANCE = .62f;
		protected readonly float OFFSET_DISTANCE = .1f;
		protected readonly int MAX_CHECK_COUNT = 2;
		//How many parents to check after hitting a collider for a UIElement
		protected readonly float SCALE_MULTIPLIER = 0.02741935484f;
		protected static readonly float FADE_DURATION = 2f;

		//enum
		public enum CrosshairState
		{
			Open,
			Closed,
			Connecting,
			Paintball,
			Liking,
			Waypoint,
			Scrolling,
			WipeSwipe
		}

		//
		public enum WaypointMode
		{
			Transform,
			Position
		}

		//Serialized
		[Header ("Textures")]
		[SerializeField]
		protected GameObject normalGameObject;
		[SerializeField]
		protected GameObject pulsingGameObject;
		[SerializeField]
		protected GameObject paintballGameObject;
		[SerializeField]
		protected GameObject waypointGameObject;
		[SerializeField]
		protected GameObject likingGameObject;
		[SerializeField]
		protected GameObject scrollingGameObject;
		[SerializeField]
		protected GameObject wipeSwipeGameObject;

		/////Protected//
		//References
		protected Utility.AnimatedSpriteUtility wipeSwipeAnimatedSpriteUtility;
		protected Transform mainCameraTransform;
		[SerializeField]
		protected Transform crosshairTransform;
		protected Transform arrowPivotTransform;
		protected SpriteRenderer crosshairSpriteRenderer;
		protected CrosshairState crosshairState = CrosshairState.Open;
		protected Canvas canvas;
		protected UnityEngine.UI.Text canvasText;
		//Primitives
		/////Waypoint/////
		protected WaypointMode waypointMode;
		protected Transform waypointTargetTransform;
		protected Vector3 waypointTargetPosition;
		/////Protected_Connector//
		//References
		protected Transform connectorTransform;
		protected Material connectorMaterial;
		protected AnimationCurve fadeAnimationCurve;
		//Primitives
		protected bool isConnecting;
		protected Vector3 connectingStartPosition;
		protected float currentCrosshairDistance;
		protected int tilingMultiplierXPropertyID;
		protected int alphaMultiplierPropertyID;
		protected float lastValidCollisionTime = -FADE_DURATION;
		protected bool isVisible;

		//Actions
		public Action<Vector3> PositionChangedAction { get; set; }


		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		protected void Awake () {
			
			base.SaveInstance (this);

			this.mainCameraTransform = Camera.main.transform;

			this.crosshairSpriteRenderer = this.crosshairTransform.GetComponentInChildren<SpriteRenderer>();

			this.tilingMultiplierXPropertyID = Shader.PropertyToID ("_TilingMultiplierX");
			this.alphaMultiplierPropertyID = Shader.PropertyToID ("_AlphaMultiplier");

			this.currentCrosshairDistance = CROSSHAIR_MIN_DISTANCE;

			this.fadeAnimationCurve = UIPrimitives.UIAnimationUtility.GetCurve (UIPrimitives.UIAnimationUtility.EaseType.easeOutSine);

		}

		protected void Start () {
//			SetState (CrosshairState.Waypoint);

			Input.Instance.RegisterInteractiveGazeGameObject (this.gameObject);

		}

		protected void Update () {
//			UpdateRaycast();

			if (this.isConnecting)
				UpdateConnector ();

			UpdateFadePercent ();

			if (this.crosshairState == CrosshairState.Waypoint)
				UpdateWaypoint ();
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Crosshair functions
		//

		public void OnInteractiveGaze (List<InteractiveGazeEventData> dataList) {
			bool hitInteractable = false;
			bool hitCollider = false;
			float minDistance = float.MaxValue;
			float minDistanceCollider = float.MaxValue;

			foreach (InteractiveGazeEventData data in dataList) {
				if (data.eventType != InteractiveGazeEventType.HitNone) {
					if (data.eventType == InteractiveGazeEventType.HitInteractable && data.raycastResult.distance < minDistance)
						minDistance = data.raycastResult.distance;
					if (data.eventType == InteractiveGazeEventType.HitCollider && data.raycastResult.distance < minDistanceCollider)
						minDistanceCollider = data.raycastResult.distance;

					hitInteractable |= data.eventType == InteractiveGazeEventType.HitInteractable;
					hitCollider |= data.eventType == InteractiveGazeEventType.HitCollider;
				}

//				if (data.raycastResult.distance > 50f)
//					Debug.Log (data.raycastResult.gameObject.name);
			}

			if (!hitInteractable)
				minDistance = minDistanceCollider;

			bool hitNothing = (!hitInteractable && !hitCollider);


			if (hitInteractable) {

				LerpCrosshair (minDistance - OFFSET_DISTANCE);
				SetLastValidCollisionTime ();
				if (!IsStateLocked())
					SetState (CrosshairState.Closed);
			} else if (hitCollider) {
				LerpCrosshair (minDistance - OFFSET_DISTANCE);
				SetLastValidCollisionTime ();
//				LerpCrosshair (currentCrosshairDistance);
				if (!IsStateLocked())
					SetState (CrosshairState.Open);
			} else {
				LerpCrosshair (currentCrosshairDistance);
				if (!IsStateLocked())
					SetState (CrosshairState.Open);
			}
		}

		protected void UpdateFadePercent () {
			float fadePercent = 1f - Mathf.Clamp01 ((Time.time - lastValidCollisionTime) / FADE_DURATION);
			if (fadePercent != 0 && fadePercent != 1f)
				fadePercent = this.fadeAnimationCurve.Evaluate (fadePercent);

			this.isVisible = (fadePercent > 0);

			this.crosshairSpriteRenderer.enabled = this.isVisible;
			this.crosshairSpriteRenderer.material.SetFloat (alphaMultiplierPropertyID, fadePercent);
			
		}

		////////////////////////////////////////
		//
		// Transform Functions
		
		protected void LerpCrosshair (float distance) {
			Vector3 targetPosition = GetTargetPosition (distance);
			Vector3 targetScale = GetTargetScale (distance);
			if (this.isVisible) {
				Vector3 targetPositionDirection = targetPosition - transform.position;
				this.transform.position += targetPositionDirection * 10f * Time.deltaTime;
				
				Vector3 targetScaleDirection = targetScale - this.crosshairTransform.localScale;
				this.crosshairTransform.localScale += targetScaleDirection * 10f * Time.deltaTime;
			} else {
				this.transform.position = targetPosition;
				this.crosshairTransform.localScale = targetScale;
			}
			
			this.transform.forward = GetForwardDirection ();
			
			this.currentCrosshairDistance = distance;

			if (PositionChangedAction != null)
				PositionChangedAction (this.transform.position);
		}

		protected void UpdateConnector () {
			Vector3 startPosition = this.connectingStartPosition;
			Vector3 endPosition = this.transform.position;

			this.connectorTransform.position = Vector3.Lerp (startPosition, endPosition, .5f);

			float localScaleX = Vector3.Distance (startPosition, endPosition);

			Vector3 connectorLocalScale = this.connectorTransform.localScale;
			connectorLocalScale.x = localScaleX;
			this.connectorTransform.localScale = connectorLocalScale;

			this.connectorTransform.right = (endPosition - startPosition);

			this.connectorMaterial.SetFloat (tilingMultiplierXPropertyID, connectorLocalScale.x / connectorLocalScale.y);
		}

		protected void SetLastValidCollisionTime () {
			lastValidCollisionTime = Time.time;
		}

		//These states can't just be changed depending on gaze data
		protected bool IsStateLocked() {

			return (crosshairState == CrosshairState.Paintball) || (crosshairState == CrosshairState.Waypoint) || (crosshairState == CrosshairState.Liking) || (crosshairState == CrosshairState.Scrolling) || (crosshairState == CrosshairState.WipeSwipe);
		}

		////////////////////////////////////////
		//
		// Connecting Functions

		public void StartConnecting (Vector3 position) {
			SetState (CrosshairState.Connecting);
			this.connectingStartPosition = position - mainCameraTransform.forward * OFFSET_DISTANCE * .5f;
			this.isConnecting = true;
			//Center it
			this.connectorTransform.localScale = Vector3.one * this.connectorTransform.localScale.y;
			this.connectorTransform.localPosition = this.crosshairTransform.localPosition;
		}

		public void StopConnecting () {
			this.isConnecting = false;
			SetState (CrosshairState.Open);
		}

		////////////////////////////////////////
		//
		// Waypoint Functions

		public void SetWaypoint(Transform transform) {

			SetState (CrosshairState.Waypoint);
			waypointMode = WaypointMode.Transform;
			waypointTargetTransform = transform;
		}

		public void SetWaypoint(Vector3 position) {

			SetState (CrosshairState.Waypoint);
			waypointMode = WaypointMode.Position;
			waypointTargetPosition = position;
		}

		public void ClearWaypoint() {

			SetState (CrosshairState.Open);
		}

		protected void UpdateWaypoint() {

			if ((waypointMode == WaypointMode.Transform) && waypointTargetTransform == null) { 
				ClearWaypoint ();
				return;
			}

			Vector3 targetPosition = (waypointMode == WaypointMode.Transform) ? waypointTargetTransform.position : waypointTargetPosition;

			Vector3 cameraDirection = Input.Instance.GetCameraForward ();
			cameraDirection.Normalize ();
			Vector3 waypointDirection = targetPosition - Input.Instance.GetCameraPosition ();
			waypointDirection.Normalize ();

			Vector2 cameraCoordinates = DirectionToCoordinates (cameraDirection);
			Vector2 waypointCoordinates = DirectionToCoordinates (waypointDirection);

			Vector2 differenceCoordinates = (waypointCoordinates - cameraCoordinates);//TODO: Make sure this loops around

			float radian = Mathf.PI * 2f;

			float angle = Mathf.Atan2 (differenceCoordinates.x*radian,differenceCoordinates.y*radian);
			
			arrowPivotTransform.transform.localEulerAngles = new Vector3(0,0,angle*Mathf.Rad2Deg);
		}

		protected Vector2 DirectionToCoordinates(Vector3 direction) {

			Vector2 coordinates = new Vector2 ();

			coordinates.x = 0.5f + Mathf.Atan2 (direction.z, direction.x)/(2f*Mathf.PI);
			coordinates.y = 0.5f + Mathf.Asin (direction.y) / Mathf.PI;

			return coordinates;
		}

		////////////////////////////////////////
		//
		// State Functions

		public void SetState (CrosshairState state) {
			if (this.isConnecting)
				return;

			this.crosshairState = state;

			SetConnectorVisible (state == CrosshairState.Connecting);

			switch (this.crosshairState) {
			case CrosshairState.Open:

				SetActiveMeshGameObject (normalGameObject);
				break;
			case CrosshairState.Closed:

				SetActiveMeshGameObject (pulsingGameObject);
				break;
			case CrosshairState.Connecting:
				break;
			case CrosshairState.Paintball:

				SetActiveMeshGameObject (paintballGameObject);
				break;
			case CrosshairState.Waypoint:

				SetActiveMeshGameObject (waypointGameObject);
				break;

			case CrosshairState.Liking:

				SetActiveMeshGameObject (likingGameObject);
				break;

			case CrosshairState.Scrolling:

				SetActiveMeshGameObject (scrollingGameObject);
				break;
			case CrosshairState.WipeSwipe:

				SetActiveMeshGameObject (wipeSwipeGameObject);
				break;
			}
		}

		////////////////////////////////////////
		//
		// Tooltip Functions
		
		protected void SetTooltip (string text) {
			if (canvas == null || canvasText == null)
				return;
			this.canvas.enabled = true;
			canvasText.text = text;
		}

		protected void ClearTooltip () {
			if (canvas == null || canvasText == null)
				return;
			canvasText.text = "";
			this.canvas.enabled = false;
		}

		////////////////////////////////////////
		//
		// Get Functions

		protected Vector3 GetTargetPosition (float distance) {
			return this.mainCameraTransform.position + this.mainCameraTransform.forward * distance;
		}

		protected Vector3 GetTargetScale (float distance) {
			return Vector3.one * (distance / SCALE_MULTIPLIER) * (GetMeshScalePercent () / 1.5f + .5f);
		}

		protected Vector3 GetForwardDirection () {
			return this.mainCameraTransform.forward;
		}

		protected float GetMeshScalePercent () {
			switch (this.crosshairState) {
			case CrosshairState.Open:

				return 0;

				break;
			case CrosshairState.Closed:

				return 1f;

				break;
			case CrosshairState.Paintball:

				return 1f;

				break;
			case CrosshairState.Liking:

				return 1f;

				break;
			case CrosshairState.Scrolling:

				return 1f;

				break;
			case CrosshairState.WipeSwipe:

				return 1f;
				break;

			}

			return 0;
		}

		////////////////////////////////////////
		//
		// Set Functions

		protected void SetActiveMeshGameObject (GameObject gameObject) {

			Transform meshParentTransform = crosshairTransform;
			for (int i = 0; i < meshParentTransform.childCount; i++) {

				GameObject childGameObject = meshParentTransform.GetChild (i).gameObject;
				childGameObject.SetActive (gameObject == childGameObject);
			}
		}

		public void SetVisible (bool value) {
			this.crosshairSpriteRenderer.enabled = value;
		}

		protected void SetTexture (Texture2D texture) {
			this.crosshairSpriteRenderer.sharedMaterial.mainTexture = texture;
		}

		protected void SetMaterial (Material material) {
			this.crosshairSpriteRenderer.sharedMaterial = material;
		}

		protected void SetConnectorVisible (bool value) {
			if (this.connectorTransform != null)
				this.connectorTransform.GetComponent<MeshRenderer> ().enabled = value;
		}

		public void SetWipeSwipePercent(float percent) {

			wipeSwipeAnimatedSpriteUtility.SetPercent(percent);
		}
	}
}