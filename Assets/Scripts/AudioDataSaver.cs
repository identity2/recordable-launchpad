using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AudioDataSaver
{
	//Load AudioClip from a local binary file and set it to audioSource
	public static void LoadAudioClipFromDisk(AudioSource audioSource, string filename)
	{
		if (File.Exists (Application.persistentDataPath + "/"+ filename)) {

			//deserialize local binary file to AudioClipSample
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + filename, FileMode.Open);
			AudioClipSample clipSample = (AudioClipSample) bf.Deserialize (file);
			file.Close ();

			//create new AudioClip instance, and set the (name, samples, channels, frequency, [stream] play immediately without fully loaded)
			AudioClip newClip = AudioClip.Create(filename, clipSample.samples, clipSample.channels, clipSample.frequency, false);

			//set the acutal audio sample to the AudioClip (sample, offset)
			newClip.SetData (clipSample.sample, 0);

			//set to the AudioSource
			audioSource.clip = newClip;
		}
		else
		{
			Debug.Log("File Not Found!");
		}
	}

	public static void SaveAudioClipToDisk(AudioClip audioClip, string filename)
	{
		//create file
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath+ "/" + filename);

		//serialize by setting the sample, frequency, samples, and channels to the new AudioClipSample instance
		AudioClipSample newSample = new AudioClipSample();
		newSample.sample = new float[audioClip.samples * audioClip.channels];
		newSample.frequency = audioClip.frequency;
		newSample.samples = audioClip.samples;
		newSample.channels = audioClip.channels;

		//get the actual sample from the AudioClip
		audioClip.GetData (newSample.sample, 0);


		bf.Serialize (file, newSample);
		file.Close ();
	}

	[Serializable]
	class AudioClipSample
	{
		public int frequency;
		public int samples;
		public int channels;
		public float[] sample;
	}
}
	//originally save file to .wav files
	/*
	const int HEADER_SIZE = 44;

	//should create an instance of AudioDataManager to use this coroutine
	public IEnumerator LoadAudioClipFromFile(AudioSource audioSource, string filename) 
	{
		//if customized audio exists
		if (File.Exists (filename)) {
			
			//request
			WWW www = new WWW ("file://" + filename);
			yield return www;

			//set clip
			audioSource.clip = www.GetAudioClip (false, false, AudioType.WAV);
		}
		yield return null;

	}

	public static void SaveAudioClipToFile(AudioClip audioClip, string filename) 
	{

		//create empty file
		//FileStream fileStream = new FileStream(Application.persistentDataPath + filename, FileMode.Create);
		FileStream fileStream = File.Create(Application.persistentDataPath + filename);
		byte emptyByte = new byte ();

		for (int i = 0; i < HEADER_SIZE; i++) {
			fileStream.WriteByte (emptyByte);
		}

		//convert and write (I just copy them from github...dunno y it works)
		float[] samples = new float[audioClip.samples];
		audioClip.GetData (samples, 0);
		Int16[] intData = new Int16[samples.Length];
		Byte[] bytesData = new Byte[samples.Length * 2];

		int rescaleFactor = 32767;

		for (int i = 0; i < samples.Length; i++) {
			intData [i] = (short)(samples [i] * rescaleFactor);
			Byte[] byteArr = new Byte[2];
			byteArr = BitConverter.GetBytes (intData [i]);
			byteArr.CopyTo (bytesData, i * 2);
		}

		fileStream.Write (bytesData, 0, bytesData.Length);

		//write header (dunno wut this chunk of code means either)
		int hz = audioClip.frequency;
		int channels = audioClip.channels;
		int sample = audioClip.samples;

		fileStream.Seek (0, SeekOrigin.Begin);

		Byte[] riff = System.Text.Encoding.UTF8.GetBytes ("RIFF");
		fileStream.Write (riff, 0, 4);

		Byte[] chunkSize = BitConverter.GetBytes (fileStream.Length - 8);
		fileStream.Write (chunkSize, 0, 4);

		Byte[] wave = System.Text.Encoding.UTF8.GetBytes ("WAVE");
		fileStream.Write (wave, 0, 4);

		Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
		fileStream.Write(fmt, 0, 4);

		Byte[] subChunk1 = BitConverter.GetBytes(16);
		fileStream.Write(subChunk1, 0, 4);

//		UInt16 two = 2;
		UInt16 one = 1;

		Byte[] audioFormat = BitConverter.GetBytes(one);
		fileStream.Write(audioFormat, 0, 2);

		Byte[] numChannels = BitConverter.GetBytes(channels);
		fileStream.Write(numChannels, 0, 2);

		Byte[] sampleRate = BitConverter.GetBytes(hz);
		fileStream.Write(sampleRate, 0, 4);

		Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
		fileStream.Write(byteRate, 0, 4);

		UInt16 blockAlign = (ushort) (channels * 2);
		fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

		UInt16 bps = 16;
		Byte[] bitsPerSample = BitConverter.GetBytes(bps);
		fileStream.Write(bitsPerSample, 0, 2);

		Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
		fileStream.Write(datastring, 0, 4);

		Byte[] subChunk2 = BitConverter.GetBytes(sample * channels * 2);
		fileStream.Write(subChunk2, 0, 4);

		fileStream.Close ();
	}
	*/
