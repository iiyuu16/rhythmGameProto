using UnityEngine;

public class rhythmBeatManager : MonoBehaviour
{
    [SerializeField] private float _bpm;
    [SerializeField] public AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;

    private void Update()
    {
        foreach (Intervals interval in _intervals)
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForNewInterval(sampledTime);
        }
    }
}
