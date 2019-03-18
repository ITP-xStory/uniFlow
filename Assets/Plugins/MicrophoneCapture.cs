using System.Collections;
using System.Collections.Generic;
using System.Threading;
using JsonData;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class MicrophoneCapture : MonoBehaviour
{

	//A boolean that flags whether there's a connected microphone
	private bool micConnected = false;

	//The maximum and minimum available recording frequencies
	private int minFreq;
	private int maxFreq;

	//A handle to the attached AudioSource
	private AudioSource goAudioSource;

	//Public variable for saving recorded sound clip
	public AudioClip recordedClip;
	private float[] samples;
	private byte[] bytes;
	//dialogflow
	private AudioSource audioSource;
	private readonly object thisLock = new object();
	private volatile bool recordingActive;

	public Animator anim;
	public UnityEngine.UI.Text CharaText;


	//Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator> ();
		CharaText.text = "";
		//Check if there is at least one microphone connected
		if (Microphone.devices.Length <= 0)
		{
			//Throw a warning message at the console if there isn't
			Debug.LogWarning("Microphone not connected!");
		}
		else //At least one microphone is present
		{
			//Set 'micConnected' to true
			micConnected = true;

			//Get the default microphone recording capabilities
			Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

			//According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...
			if (minFreq == 0 && maxFreq == 0)
			{
				//...meaning 44100 Hz can be used as the recording sampling rate
				maxFreq = 44100;
			}

			//Get the attached AudioSource component
			goAudioSource = this.GetComponent<AudioSource>();
		}

	}

	void OnGUI()
	{
		//If there is a microphone
		if (micConnected)
		{
			//If the audio from any microphone isn't being captured
			if (!Microphone.IsRecording(null))
			{
				//Case the 'Record' button gets pressed
				if (GUI.Button(new Rect(Screen.width / 4 - 100, Screen.height / 4 - 25, 200, 50), "Record"))
				{
					//Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource
					//goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
					//recordedClip = goAudioSource.clip;
					//samples = new float[goAudioSource.clip.samples];
					//handle dialogflow
					CharaText.text = "...?";
					StartListening(goAudioSource);
				}
			}
			else //Recording is in progress
			{
				//Case the 'Stop and Play' button gets pressed
				if (GUI.Button(new Rect(Screen.width / 4 - 100, Screen.height / 4 - 25, 200, 50), "Stop and Play!"))
				{
					//Microphone.End(null); //Stop the audio recording
					//goAudioSource.Play(); //Playback the recorded audio
					//Debug.Log(recordedClip.length);
					//send out request
					StopListening();
				}

				GUI.Label(new Rect(Screen.width / 4 - 100, Screen.height / 4 + 25, 200, 50), "Recording in progress...");
			}
		}
		else // No microphone
		{
			//Print a red "Microphone not connected!" message at the center of the screen
			GUI.contentColor = Color.red;
			GUI.Label(new Rect(Screen.width / 4 - 100, Screen.height / 4 - 25, 200, 50), "Microphone not connected!");
		}

	}

	public void StartListening(AudioSource audioSource)
	{
		lock (thisLock)
		{
			if (!recordingActive)
			{
				this.audioSource = audioSource;
				StartRecording();
			}
			else
			{
				Debug.LogWarning("Can't start new recording session while another recording session active");
			}
		}
	}

	private void StartRecording()
	{
		audioSource.clip = Microphone.Start(null, true, 20, 16000);
		recordingActive = true;

		//FireOnListeningStarted();
	}

	public void StopListening()
	{
		if (recordingActive)
		{

			//float[] samples = null;

			lock (thisLock)
			{
				if (recordingActive)
				{
					StopRecording();
					//samples = new float[audioSource.clip.samples];

					//audioSource.clip.GetData(samples, 0);
					bytes = WavUtility.FromAudioClip(audioSource.clip);
					audioSource.Play();
					Debug.Log("This is the audiosource clip length: "+ bytes.Length);
					audioSource = null;
				}
			}

			//new Thread(StartVoiceRequest).Start(samples);
			StartCoroutine(StartVoiceRequest("https://dialogflow.googleapis.com/v2/projects/YOUR_PROJECT_ID/agent/sessions/34563:detectIntent",
				"YOUR_ACCESS_TOKEN",
				bytes));
		}
	}

	private void StopRecording()
	{
		Microphone.End(null);
		recordingActive = false;
	}

	IEnumerator StartVoiceRequest(String url, String AccessToken, object parameter)
	{
		byte[] samples = (byte[])parameter;
		//TODO: convert float[] samples into bytes[]
		//byte[] sampleByte = new byte[samples.Length * 4];
		//Buffer.BlockCopy(samples, 0, sampleByte, 0, sampleByte.Length);

		string sampleString = System.Convert.ToBase64String(samples);
		if (samples != null)
		{
			UnityWebRequest postRequest = new UnityWebRequest(url, "POST");
			RequestBody requestBody = new RequestBody();
			requestBody.queryInput = new QueryInput();
			requestBody.queryInput.audioConfig = new InputAudioConfig();
			requestBody.queryInput.audioConfig.audioEncoding = AudioEncoding.AUDIO_ENCODING_UNSPECIFIED;
			//TODO: check if that the sample rate hertz
			requestBody.queryInput.audioConfig.sampleRateHertz = 16000;
			requestBody.queryInput.audioConfig.languageCode = "en";
			requestBody.inputAudio = sampleString;

			string jsonRequestBody = JsonUtility.ToJson(requestBody, true);
			Debug.Log(jsonRequestBody);

			byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonRequestBody);
			postRequest.SetRequestHeader("Authorization", "Bearer " + AccessToken);
			postRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			postRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			//postRequest.SetRequestHeader("Content-Type", "application/json");

			yield return postRequest.SendWebRequest();

			if (postRequest.isNetworkError || postRequest.isHttpError)
			{
				Debug.Log(postRequest.responseCode);
				Debug.Log(postRequest.error);
			}
			else
			{

				Debug.Log("Response: " + postRequest.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] resultbyte = postRequest.downloadHandler.data;
				string result = System.Text.Encoding.UTF8.GetString(resultbyte);
				ResponseBody content = (ResponseBody)JsonUtility.FromJson<ResponseBody>(result);
				Debug.Log(content.queryResult.fulfillmentText);
			}
		}else{
			Debug.LogError("The audio file is null");
		}
	}

	private void StartVoiceRequest(object parameter)
	{
		float[] samples = (float[])parameter;
		if (samples != null)
		{
			/*try
             {
                 var aiResponse = apiAi.VoiceRequest(samples);
                 ProcessResult(aiResponse);
             }
             catch (Exception ex)
             {
                 FireOnError(ex);
             }*/
		}
	}
}
