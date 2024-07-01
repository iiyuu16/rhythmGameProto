using System.Collections;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class rhythmSongManager : MonoBehaviour
{
    public static rhythmSongManager Instance;
    public AudioSource audioSource;
    public rhythmLane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError;
    public double MarginOfError
    {
        get { return marginOfError; }
        set { marginOfError = Math.Max(0, value); }
    }

    public int inputDelayInMilliseconds;
    public float inputSensitivity = 0.5f; 

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
            if (audioSource.time >= audioSource.clip.length - 0.01f)
            {
                SongFinished();
            }
        }
    }


    private void SongFinished()
    {
        Debug.Log("Song finished!");
        isSongFinished = true;

        resultsGameObject.SetActive(true);
    }

    public double AdjustedMarginOfError()
    {
        return MarginOfError * inputSensitivity;
    }
}
