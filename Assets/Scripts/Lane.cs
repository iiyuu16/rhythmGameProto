using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Interaction;
using System;

public class Lane : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem = default;

    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    int spawnIndex = 0;
    List<int> inputIndices = new List<int>();

    public List<string> noteRestrictions = new List<string>();

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn notes
        while (spawnIndex < timeStamps.Count && SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
        {
            var note = Instantiate(notePrefab, transform);
            notes.Add(note.GetComponent<Note>());
            note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
            spawnIndex++;
        }

        // Check input for each note
        for (int i = 0; i < timeStamps.Count; i++)
        {
            double timeStamp = timeStamps[i];
            double marginOfError = SongManager.Instance.MarginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (!inputIndices.Contains(i))
            {
                // If note hasn't been processed yet
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
        ScoreManager.Hit();
        _particleSystem.Play();
        Destroy(notes[index].gameObject);
    }

    private void Miss(int index)
    {
        ScoreManager.Miss();
    }
}
