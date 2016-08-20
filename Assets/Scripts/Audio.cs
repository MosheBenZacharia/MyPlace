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
			ComfortRotation,
			Voiceover_ResettingOrientation,
			Voiceover_SelectTheScenes,
			Virus_Hit,
			Virus_Whoosh,
			Virus_WhooshFast
		}


		protected Dictionary<SoundEffect,string> soundEffectToResourcePath = new Dictionary<SoundEffect, string>()
		{
			{SoundEffect.UIHoverOn,"UI_Hover_On"},
			{SoundEffect.UIHoverOff,"UI_Hover_Off"},
			{SoundEffect.UISelect,"UI_Select"},
			{SoundEffect.ComfortRotation,"ComfortRotation"},
			{SoundEffect.Voiceover_ResettingOrientation,"VO_ResettingOrientation"},
			{SoundEffect.Voiceover_SelectTheScenes,"VO_SelectTheScenes"},
			{SoundEffect.Virus_Hit,"C9_Hit_Audio"},
			{SoundEffect.Virus_Whoosh,"C9_Whoosh_Audio"},
			{SoundEffect.Virus_WhooshFast,"C9_WhooshFast_Audio"}
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

		public void PlaySoundEffect(SoundEffect soundEffect)
		{
			if(this.soundEffectToAudioClip.ContainsKey(soundEffect))
			{
				PlayClip(this.soundEffectToAudioClip[soundEffect]);
			}
			else
			{
				string soundEffectResourcePath = this.soundEffectToResourcePath[soundEffect];

				AudioClip audioClip = Resources.Load (soundEffectResourcePath) as AudioClip;
				if(!soundEffect.ToString().StartsWith("Voiceover"))
					this.soundEffectToAudioClip[soundEffect] = audioClip;
				PlayClip(audioClip);
			}
		}

		protected void PlayClip(AudioClip audioClip)
		{

			if(this.audioSource.isPlaying)
			{
				AudioSource.PlayClipAtPoint(audioClip,Vector3.zero);
			}
			else
			{
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