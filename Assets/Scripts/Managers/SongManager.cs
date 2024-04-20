using System.Collections;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // Default margin of error = 0.1
    public double MarginOfError
    {
        get { return marginOfError; }
        set { marginOfError = Math.Max(0, value); } // Ensure margin of error is not negative
    }

    public int inputDelayInMilliseconds;
    public float inputSensitivity = 0.5f; // Increased sensitivity multiplier for input detection

    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    {
        get { return noteTapY - (noteSpawnY - noteTapY); }
    }

    public static MidiFile midiFile;

    private bool isSongFinished = false;
    public GameObject resultsGameObject;

    void Start()
    {
        Instance = this;
        resultsGameObject.SetActive(false);
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    void Update()
    {
        if (!isSongFinished && audioSource.isPlaying)
        {
            // Check if the audio source has finished playing the song
            if (audioSource.time >= audioSource.clip.length - 0.01f) // Allow a small buffer for precision
            {
                SongFinished();
            }
        }
    }


    private void SongFinished()
    {
        // Do something when the song is finished
        Debug.Log("Song finished!");
        isSongFinished = true;

        // Enable the results GameObject
        resultsGameObject.SetActive(true);
    }

    // Method to adjust the margin of error based on input sensitivity
    public double AdjustedMarginOfError()
    {
        return MarginOfError * inputSensitivity;
    }
}
