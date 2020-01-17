using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CappyAudioRecorder : MonoBehaviour {

		public TextAsset audioData;
		public AudioClip[] clips;

		private int frameNum = 0;

		private List<AudioSource> audioSources;

		private bool playback = false;

		void Start(){
				StartPlayback();
		}


		// Update is called once per frame
		void Update () {
				//if (Input.GetKeyDown("space")) StartPlayback();

				if (playback){

						playbackTime += Time.deltaTime;

						//if (frameNum==0) StartRecording();
						if (playbackTime>=0f && !recording) StartRecording();
						frameNum++;

						//if (frameNum>=0 && frameNum<camTimes.Count){
								//transform.position = camPositions[frameNum];
								//transform.eulerAngles = camRotations[frameNum];
						//}
						for (int i=0; i<camTimesF.Count; i++){
								if (playbackTime>=camTimesF[i]){
										transform.position = camPositions[i];
										transform.eulerAngles = camRotations[i];
								}
						}
						//for (int i=0; i<audioTimes.Count; i++){
						//		if (audioTimes[i] == frameNum){
						//				GameObject newGo = new GameObject();
						//				newGo.transform.position = audioPositions[i];
						//				newGo.AddComponent<AudioSource>();
						//				audioSources.Add(newGo.GetComponent<AudioSource>());
						//				for (int k=0; k<clips.Length; k++){
						//						if (audioClips[i]==clips[k].name) audioSources[audioSources.Count-1].PlayOneShot(clips[k]);
						//				}
						//		}
						//}
						for (int i=0; i<audioTimesF.Count; i++){
								if (playbackTime>=audioTimesF[i] && !playedAudioYet[i]){
										GameObject newGo = new GameObject();
										newGo.transform.position = audioPositions[i];
										newGo.AddComponent<AudioSource>();
										audioSources.Add(newGo.GetComponent<AudioSource>());
										for (int k=0; k<clips.Length; k++){
												if (audioClips[i]==clips[k].name) audioSources[audioSources.Count-1].PlayOneShot(clips[k]);
										}
										playedAudioYet[i] = true;
								}
						}
						if (playbackTime>endingTime){
								StopPlayback();
						}
				}
		}

		void StartPlayback(){
				frameNum = -1;
				playback = true;
				playbackTime = -0.5f;

				//clear old audio sources
				if (audioSources != null){
						for (int i=0;i<audioSources.Count; i++) Destroy(audioSources[i].gameObject);
				}
				audioSources = new List<AudioSource>();

				//load audio data
				LoadAudioData();

				//check every audio clip is in clips list & cancel playback if not
				bool missingClip = false;
				for (int i=0; i<audioClips.Count; i++){
						bool foundMatch = false;
						for (int k=0; k<clips.Length; k++){
								if (clips[k].name == audioClips[i]) foundMatch = true;
						}
						if (!foundMatch){
								missingClip = true;
								Debug.Log("Missing audio clip: '" + audioClips[i] + "'.");
						}
				}
				if (missingClip){
						Debug.Log("Cancelling playback due to missing audio clip(s).");
						playback = false;
						return;
				}

				transform.position = camPositions[0];
				transform.eulerAngles = camRotations[0];

		}
		void StopPlayback(){
				playback = false;
				StopRecording();
		}

		void StartRecording(){
				Debug.Log("Recording start");
				recordedData = new List<float>();
				recording = true;
		}
		void StopRecording(){
				Debug.Log("Recording End");
				float[] recData = recordedData.ToArray();

				recData = NormaliseSamples(recData);

				recClip = AudioClip.Create("recClip", recData.Length, recChannels, recFreq, false);
				recClip.SetData(recData,0);

				if (debugAudioPlayer){
						//playback recorded audio, just to confirm it went well
						debugAudioPlayer.PlayOneShot(recClip);
				}

				SavWav.Save(savePath+"/Audio", recClip);
				recording = false;
		}

		float[] NormaliseSamples(float[] samples){
				float max = 0f;
				float min = 0f;
				for (int i=0; i<samples.Length; i++){
						if (samples[i]>max) max = samples[i];
						if (samples[i]<min) min = samples[i];
				}
				min *= -1f;
				if (min>max) max = min;

				float scalar = 1f/max;
				for (int i=0; i<samples.Length; i++){
						samples[i] *= scalar;
				}
				return samples;
		}

		private List<bool> playedAudioYet = new List<bool>();
		private List<int> audioTimes = new List<int>();
		private List<float> audioTimesF = new List<float>();
		private List<string> audioClips = new List<string>();
		private List<Vector3> audioPositions = new List<Vector3>();
		private List<int> camTimes = new List<int>();
		private List<float> camTimesF = new List<float>();
		private List<Vector3> camPositions = new List<Vector3>();
		private List<Vector3> camRotations = new List<Vector3>();
		private int endingFrame;
		private float endingTime;
		private string savePath = "";
		private float playbackTime = -0.5f;

		void LoadAudioData(){

				string sourceData = PlayerPrefs.GetString("CappyRecordedAudioDataPath","");
				if (audioData != null){
						sourceData = audioData.text;
				}else{
						sourceData = System.IO.File.ReadAllText(sourceData);
				}



				playedAudioYet = new List<bool>();
				audioTimes = new List<int>();
				audioTimesF = new List<float>();
				audioClips = new List<string>();
				audioPositions = new List<Vector3>();
				camTimes = new List<int>();
				camTimesF = new List<float>();
				camPositions = new List<Vector3>();
				camRotations = new List<Vector3>();

				string[] lines = sourceData.Split('\n');
				for (int i=0; i<lines.Length; i++){
						string[] lineData = lines[i].Split(',');
						if (lineData.Length>1){
								if (lineData[1] == "CAM"){
										camTimes.Add( MakeInt(lineData[0]) );
										camTimesF.Add( MakeFloat(lineData[8]) );
										camPositions.Add( MakeVector(lineData[2],lineData[3],lineData[4]) );
										camRotations.Add( MakeVector(lineData[5],lineData[6],lineData[7]) );		
								}
								if (lineData[1] == "AUDIO"){
										audioTimes.Add( MakeInt(lineData[0]) );
										audioTimesF.Add( MakeFloat(lineData[6]) );
										audioClips.Add( lineData[2] );
										audioPositions.Add( MakeVector(lineData[3],lineData[4],lineData[5]) );
										playedAudioYet.Add(false);
								}
								if (lineData[1] == "END"){
										endingFrame = MakeInt(lineData[0]);
										endingTime = MakeFloat(lineData[2]);
								}
								if (lineData[1] == "SAVEPATH"){
										savePath = lineData[2];
								}
						}
				}
		}

		int MakeInt(string s){
				return int.Parse(s);
		}
		float MakeFloat(string s){
				return float.Parse(s);
		}
		Vector3 MakeVector(string s1, string s2, string s3){
				return new Vector3( MakeFloat(s1), MakeFloat(s2), MakeFloat(s3) );
		}



		private bool recording = false;
		private AudioClip recClip;

		private List<float> recordedData;
		private int recChannels;
		private int recFreq = 44100;

		public AudioSource debugAudioPlayer;

		void OnAudioFilterRead(float[] data, int channels) {
				if (!recording) return;

				int i=0;
				while (i<data.Length){
						recordedData.Add(data[i]);
						i++;
				}
				recChannels = channels;
		}
				
}
