using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using System;

public class rhythmLane : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem = default;

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<rhythmNote> notes = new List<rhythmNote>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    List<int> inputIndices = new List<int>();

    public List<string> noteRestrictions = new List<string>();

    void Start()
    {

    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, rhythmSongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    void Update()
    {
        while (spawnIndex < timeStamps.Count && rhythmSongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - rhythmSongManager.Instance.noteTime)
        {
            var note = Instantiate(notePrefab, transform);
            notes.Add(note.GetComponent<rhythmNote>());
            note.GetComponent<rhythmNote>().assignedTime = (float)timeStamps[spawnIndex];
            spawnIndex++;
        }

        for (int i = 0; i < timeStamps.Count; i++)
        {
            double timeStamp = timeStamps[i];
            double marginOfError = rhythmSongManager.Instance.MarginOfError;
            double audioTime = rhythmSongManager.GetAudioSourceTime() - (rhythmSongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (!inputIndices.Contains(i))
            {
                if (Input.GetKeyDown(input) && Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit(i);
                    print($"Hit on {i} note");
                    inputIndices.Add(i);
                }
                else if (timeStamp + marginOfError <= audioTime)
                {
                    Miss(i);
                    print($"Missed {i} note");
                    inputIndices.Add(i);
                }
            }
        }
    }

    private void Hit(int index)
    {
        rhythmScoreManager.Hit();
        _particleSystem.Play();
        Destroy(notes[index].gameObject);
    }

    private void Miss(int index)
    {
        rhythmScoreManager.Miss();
    }
}
