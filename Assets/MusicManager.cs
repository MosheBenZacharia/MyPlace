using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyPlace
{
	public class MusicManager : MonoBehaviour
	{
		

		//struct
		[System.Serializable]
		public struct SongData {
			public string songTitle;
			public string songArtist;
			public AudioClip songClip;
		}

		//readonly
		protected readonly string musicTextFormatString = "Now Playing:\n{0} - {1}";

		//Serialized
		[SerializeField]
		protected Button speakersButton;
		[SerializeField]
		protected Transform canvasTransform;
		[Header("Songs")]
		[SerializeField]
		protected SongData[] songData;
		
		/////Protected/////
		//References
		protected AudioSource audioSource;
		//Primitives
		protected int currentSongDataIndex;
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			speakersButton.SelectedActionSimple += OnSpeakersButtonSelected;
		}
		
		protected void Start ()
		{
			UpdateSong();	
		}
		
		protected void Update ()
		{
			
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// MusicManager Functions
		//

		protected void UpdateSong() {

			SongData currentSongData = songData[currentSongDataIndex];
			string musicText = string.Format(musicTextFormatString,currentSongData.songArtist,currentSongData.songTitle);
			audioSource.clip = currentSongData.songClip;
			audioSource.Play();
		}
		protected void OnSpeakersButtonSelected() {

			IncrementSongDataIndex();
			UpdateSong();
		}

		protected void IncrementSongDataIndex() {

			currentSongDataIndex++;
			if(currentSongDataIndex>=songData.Length)
				currentSongDataIndex = 0;
		}
		
		////////////////////////////////////////
		//
		// Function Functions

	}
}