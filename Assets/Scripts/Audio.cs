using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace MyPlace
{
	public class Audio : Singleton<Audio>
	{
		public enum SoundEffect
		{
			UIHoverOn,
			UIHoverOff,
			UISelect,
			WhooshSmall,
			WhooshMedium,
			BounceCartoony,
			ImpactPunchRegular,
			Splat
		}

		protected Dictionary<SoundEffect,string> soundEffectToResourcePath = new Dictionary<SoundEffect, string>()
		{
			{SoundEffect.UIHoverOn,"UI_Hover_On"},
			{SoundEffect.UIHoverOff,"UI_Hover_Off"},
			{SoundEffect.UISelect,"UI_Select"},
			{SoundEffect.WhooshSmall,"interaction_whoosh_small_03"},
			{SoundEffect.WhooshMedium,"interaction_whoosh_medium_03"},
			{SoundEffect.BounceCartoony,"bounce_cartoony_04"},
			{SoundEffect.ImpactPunchRegular,"impact_punch_regular_01"},
			{SoundEffect.Splat,"splat"},
		};

		protected Dictionary<SoundEffect,AudioClip> soundEffectToAudioClip = new  Dictionary<SoundEffect, AudioClip>();
		//readonly

		//Serialized
		
		/////Protected/////
		//References
		protected AudioSource audioSource;
		protected AudioMixer audioMixer;

		//Primitives
		protected static float musicVolume = 1f;
		protected static float narrationVolume = 1f;
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected virtual void Awake()
		{
			base.SaveInstance(this);

			this.audioSource = GetComponent<AudioSource>();
			if(this.audioSource==null)
				this.audioSource = this.gameObject.AddComponent<AudioSource>();
			
		}
		
		protected virtual void Start ()
		{
			
		}
		
		protected virtual void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// Audio Functions
		//

		public void PlaySoundEffect(SoundEffect soundEffect, Vector3 position = default(Vector3)) {

			if(this.soundEffectToAudioClip.ContainsKey(soundEffect))
			{
				PlayClip(this.soundEffectToAudioClip[soundEffect],position);
			}
			else
			{
				string soundEffectResourcePath = this.soundEffectToResourcePath[soundEffect];

				AudioClip audioClip = Resources.Load (soundEffectResourcePath) as AudioClip;
				if(!soundEffect.ToString().StartsWith("Voiceover"))
					this.soundEffectToAudioClip[soundEffect] = audioClip;
				PlayClip(audioClip,position);
			}
		}

		protected void PlayClip(AudioClip audioClip, Vector3 position = default(Vector3))
		{

			if(this.audioSource.isPlaying)
			{
				AudioSource.PlayClipAtPoint(audioClip,position);
			}
			else
			{
				this.transform.position = position;
				this.audioSource.clip = audioClip;
				this.audioSource.Play();
			}
		}


		////////////////////////////////////////
		//
		// Volume Functions

		public void SetNarrationVolume(float volume) {

			narrationVolume = volume;
			audioMixer.SetFloat("NarrationVolume",PercentToDecibel(volume));
		}

		public void SetMusicVolume(float volume) {

			musicVolume = volume;
			audioMixer.SetFloat("AmbienceVolume",PercentToDecibel(volume));
		}

		public float GetNarrationVolume() {
			
			return narrationVolume;
		}

		public float GetMusicVolume() {

			return musicVolume;
		}

		protected float PercentToDecibel(float percent) {

			return 80f*((1f-Mathf.Pow(1f-percent,2f))-1f);
		}
	}
}