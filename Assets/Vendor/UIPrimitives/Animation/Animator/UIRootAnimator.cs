using UnityEngine;
using System.Collections;
using UIPrimitives;

namespace UIPrimitives
{
	[DisallowMultipleComponent]
	public class UIRootAnimator : MonoBehaviour
	{
		public enum AnimationDirection
		{
			Forward,
			Reverse
		}
		public bool playOnAwake;
		public float startDelay;
		public bool ignoreChildDelay = false;

		protected void Start ()
		{
			if (playOnAwake) {
				if (startDelay == 0) {
					Animate ();
				} else {
					AnimateWithDelay();
				}
			}
		}

		/// <summary>
		/// Animate this instance.
		/// </summary>
		public void Animate (AnimationDirection animationDirection = AnimationDirection.Forward)
		{
			bool reversed = animationDirection == AnimationDirection.Reverse;

			UIAnimation[] myUIAnimations = GetComponentsInChildren<UIAnimation> ();
			for (int i = 0; i < myUIAnimations.Length; i++) {
				if (!myUIAnimations [i].enabled)
					continue;
				if (ignoreChildDelay) {
					if(reversed)
						myUIAnimations [i].StartAnimationReversed();
					else
						myUIAnimations [i].StartAnimation ();
				} else {
					if(reversed)
						myUIAnimations [i].StartAnimationReversed();
					else
						myUIAnimations [i].StartAnimationWithDelay ();
				}
			}
			UIText[] myUITextAnimations = GetComponentsInChildren<UIText> ();
			for (int i = 0; i < myUITextAnimations.Length; i++) {
				if (!myUITextAnimations [i].enabled)
					continue;
				if (ignoreChildDelay) {
					myUITextAnimations [i].ExpandText ();
				} else {
					if(reversed)
						myUITextAnimations[i].RetractText();
					else
						myUITextAnimations [i].ExpandTextWithDelay ();
				}
				
			}
			UINumber[] myUINumberAnimations = GetComponentsInChildren<UINumber> ();
			for (int i = 0; i < myUINumberAnimations.Length; i++) {
				if (!myUINumberAnimations [i].enabled)
					continue;
				if (ignoreChildDelay) {
					myUINumberAnimations [i].StartAnimation ();
				} else {
					myUINumberAnimations [i].StartAnimation ();
				}
			}
		}

		public void AnimateWithDelay()
		{
//			Invoke ("Animate", startDelay);
			StartCoroutine(AnimateCoroutine(startDelay));
		}

		protected IEnumerator AnimateCoroutine(float delay)
		{
			yield return new WaitForSeconds(delay);
			Animate();
		}
	}
}