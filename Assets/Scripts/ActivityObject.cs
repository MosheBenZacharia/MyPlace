using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class ActivityObject : Element, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
	{
		//enum
		public enum ActivityObjectType {
			Paintball,
			Fire,
			Cats,
			Puppies
		}
		public enum ActivityObjectState {
			Inactive,
			Hovered,
			Active
		}

		//struct
		[System.Serializable]
		public struct ActivityObjectMesh {
			public ActivityObjectState activityObjectState;
			public GameObject meshGameObject;
		}

		//readonly

		//Serialized
		[SerializeField]
		protected ActivityObjectType activityObjectType;
		[SerializeField]
		protected ActivityObjectMesh[] activityObjectMeshes;
		
		/////Protected/////
		//References
		//Primitives
		protected ActivityObjectState currentState = ActivityObjectState.Inactive;
		protected bool isFocused;

		//Actions
		public Action<ActivityObjectType> SelectedAction {get;set;}


		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake () {

		}

		protected void Start () {

			SetState(ActivityObjectState.Inactive);
		}

		protected void Update () {
			
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// ActivityObject Functions
		//

		protected void SetState(ActivityObjectState activityObjectState) {

			for (int i = 0; i < activityObjectMeshes.Length; ++i) {

				bool isMatchingState = (activityObjectMeshes[i].activityObjectState == activityObjectState);
				activityObjectMeshes[i].meshGameObject.SetActive(isMatchingState);
			}

			currentState = activityObjectState;
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected void OnClicked (PointerEventData eventData) {
			
			Debug.Log ("Clicked: " + gameObject.name);

			SetState(ActivityObjectState.Active);

			if (this.SelectedAction != null)
				this.SelectedAction(activityObjectType);
			if (this.selectedActionSimple != null)
				this.selectedActionSimple ();
		}

		protected void Hovered (bool value) {

			if(currentState != ActivityObjectState.Active){
				if(value)
					SetState(ActivityObjectState.Hovered);
				if(!value)
					SetState(ActivityObjectState.Inactive);
			}	

			if (this.hoveredAction != null)
				this.hoveredAction (value);
		}

		protected void OnHovered () {
			
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Implementation for Element
		//

		public override string TooltipName {
			get {
				return gameObject.name;
			}
			set {
				
			}
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Implementation for IPointer
		//

		public void OnPointerEnter (PointerEventData eventData) {
			Hovered (true);
			isFocused = true;

			OnHovered ();

		}

		public void OnPointerExit (PointerEventData eventData) {
			Hovered (false);
			isFocused = false;
		}

		public void OnPointerClick (PointerEventData eventData) {
			OnClicked (eventData);

		}

		//event


		//ACtion

		protected Action<bool> hoveredAction;
		protected Action selectedActionSimple;

		////////////////////////////////////////
		//
		// Properties

		public Action<bool> HoveredAction {
			get {
				return this.hoveredAction;
			}
			set {
				this.hoveredAction = value;
			}
		}

		//		public Action<UISlider> SelectedAction
		//		{
		//			get
		//			{
		//				return this.selectedAction;
		//			}
		//			set
		//			{
		//				this.selectedAction = value;
		//			}
		//		}

		public Action SelectedActionSimple {
			get {
				return this.selectedActionSimple;
			}
			set {
				this.selectedActionSimple = value;
			}
		}

	}
}