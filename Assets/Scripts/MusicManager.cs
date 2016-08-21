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
		//fft
		protected const int SPECTRUM_MAX = 128;
		protected const float VOLUME_MULTIPLIER = 100f;
		protected const int VOLUME_INTERVAL = 4;
		protected readonly float VOLUME_COUNT = SPECTRUM_MAX / VOLUME_INTERVAL;
		public static float volume;

		//Serialized
		[SerializeField]
		protected Button speakersButton;
		[SerializeField]
		protected GameObject musicTextPrefab;
		[SerializeField]
		protected MeshRenderer visualizerQuadRenderer;
		[SerializeField]
		protected Transform canvasTransform;
		[Header("Songs")]
		[SerializeField]
		protected SongData[] songData;
		
		/////Protected/////
		//References
		protected AudioSource audioSource;
		protected GameObject lastMusicTextGameObject;
		protected Material visualizerMaterial;
		//Primitives
		protected float[] samples = new float[SPECTRUM_MAX];
		protected int currentSongDataIndex;
		protected int masterBrightnessPropertyID;
		

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake()
		{
			masterBrightnessPropertyID = Shader.PropertyToID("_MasterBrightness");
			visualizerMaterial = visualizerQuadRenderer.material;
			audioSource = GetComponent<AudioSource>();
			speakersButton.SelectedActionSimple += OnSpeakersButtonSelected;
		}
		
		protected void Start ()
		{
			UpdateSong();	
		}
		
		protected void Update ()
		{
			AnalyzeAudio();
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// MusicManager Functions
		//

		protected void UpdateSong() {
			if(lastMusicTextGameObject!=null){
				Destroy(lastMusicTextGameObject);
				lastMusicTextGameObject = null;
			}

			SongData currentSongData = songData[currentSongDataIndex];
			string musicText = string.Format(musicTextFormatString,currentSongData.songArtist,currentSongData.songTitle);
			GameObject musicTextGameObject = Instantiate(musicTextPrefab) as GameObject;
			musicTextGameObject.transform.parent = canvasTransform;
			musicTextGameObject.GetComponent<UnityEngine.UI.Text>().text = musicText;
			audioSource.clip = currentSongData.songClip;
			audioSource.Play();

			lastMusicTextGameObject = musicTextGameObject;

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

		////////////////////////////////////////
		//
		// FFT Functions

		/// <summary>
		/// Analyzes the audio to animate an orb/mascot based on frequency volume.
		/// </summary>
		protected void AnalyzeAudio ()
		{
			if (!this.audioSource.isPlaying)
				return;

			this.audioSource.GetSpectrumData (this.samples, 0, FFTWindow.Rectangular);

			//FFT
			float sum = 0;

			for (int i=0; i<SPECTRUM_MAX; i+=VOLUME_INTERVAL)
			{
				float sumMultiplier = 1f;
				sum += this.samples [i] * sumMultiplier;
			}

			volume = Mathf.Clamp01 ((sum / VOLUME_COUNT) * VOLUME_MULTIPLIER);

			visualizerMaterial.SetFloat(masterBrightnessPropertyID,volume);

		}
	}
}