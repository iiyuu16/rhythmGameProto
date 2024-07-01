using UnityEngine;

public class rhythmNote : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    void Start()
    {
        timeInstantiated = rhythmSongManager.GetAudioSourceTime();
    }

    void Update()
    {
        double timeSinceInstantiated = rhythmSongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (rhythmSongManager.Instance.noteTime * 2));

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * rhythmSongManager.Instance.noteSpawnY, Vector3.up * rhythmSongManager.Instance.noteDespawnY, t);

            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var renderer in spriteRenderers)
            {
                renderer.enabled = true;
            }
        }
    }
}
