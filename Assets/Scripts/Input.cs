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

	public class Input : Singleton<MyPlace.Input>
	{
		

		//readonly


		//Serialized
		[SerializeField]
		protected GazeInputModule gazeInputModule;
		[SerializeField] 
		protected Crosshair crosshair;
		[SerializeField]
		protected Button[] buttonsToRegister;
		
		/////Protected/////
		//References
		protected List<GameObject> interactiveGazeListeners;
		protected Audio audio;
		protected Action<bool> buttonHoveredAction;
		protected Action<Button> buttonSelectedAction;
		//Primitives

		//public static
		public static bool allowButtonHoverFill  {get; set;}

		//Actions
		public Action GlobalTriggerAction {get; set;}
//		public Action<ITHB.Application.QualityLevel> QualityLevelSelectedAction;
		
		////////////////////////////////////////
		//
		// Properties
		
		public Action<bool> ButtonHoveredAction
		{
			set {
				this.buttonHoveredAction = value;
			}
		}
		
		public Action<Button> ButtonSelectedAction
		{
			set {
				this.buttonSelectedAction = value;
			}
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected virtual void Awake()
		{
			
			this.audio = Audio.Instance;

			interactiveGazeListeners = new List<GameObject> ();
//			distScaleListeners = new List<GameObject> ();

			if (gazeInputModule == null)
				gazeInputModule = FindObjectOfType<GazeInputModule> ();

			gazeInputModule.InteractiveGazeAction += OnInteractiveGaze;
			gazeInputModule.GlobalTriggerAction += OnGlobalTrigger;

			if (crosshair == null)
				crosshair = GetComponentInChildren<Crosshair> ();

//			crosshair.PositionChangedAction += OnCrosshairPositionChanged;

		}

		protected virtual void Start()
		{
			RegisterButtons(buttonsToRegister);
		}

        ///////////////////////////////////////////////////////////////////////////
        //
        // Input Functions
        //

        public virtual void RegisterSlider(UIPrimitives.UISlider slider)
        {
            if (slider == null)
                return;
            slider.HoveredAction += OnButtonHovered;
            slider.SelectedAction += OnSliderSelected;
        }

        public virtual void RegisterButton(Button button)
		{
			if(button==null)
				return;
			button.HoveredAction	+=	OnButtonHovered;
			button.SelectedAction	+=	OnButtonSelected;
		}

		public virtual void RegisterButtons(Button[] buttons)
		{
			if(buttons==null)
				return;
			for (int i = 0; i < buttons.Length; ++i)
				RegisterButton(buttons[i]);
		}

		////////////////////////////////////////
		//
		// Event Functions

		protected virtual void OnButtonHovered(bool value)
		{
			if(this.buttonHoveredAction!=null)
				this.buttonHoveredAction(value);
			
			
			PlayHoverSound(value);
		}

        protected virtual void OnButtonSelected(Button button)
        {
            if (this.buttonSelectedAction != null)
                this.buttonSelectedAction(button);

            PlaySelectSound();
        }

        protected virtual void OnSliderSelected(UIPrimitives.UISlider slider)
        {

            PlaySelectSound();
        }

		////////////////////////////////////////
		//
		// Audio Functions

		protected void PlayHoverSound(bool value)
		{
			if(value)
				this.audio.PlaySoundEffect(Audio.SoundEffect.UIHoverOn);
			else
				this.audio.PlaySoundEffect(Audio.SoundEffect.UIHoverOff);
		}
		
		protected void PlaySelectSound()
		{
			this.audio.PlaySoundEffect(Audio.SoundEffect.UISelect);
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

		protected void OnGlobalTrigger() {

			if (GlobalTriggerAction != null)
				GlobalTriggerAction ();
		}


	}
}