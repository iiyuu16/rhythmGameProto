using System.Collections;
using UnityEngine;
using TMPro;

public class rhythmCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject objectToDisable;

    public float scaleDuration = 0.5f;
    public float maxScale = 1.5f;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return ScaleText(countdownText);
        }

        countdownText.text = "Go!";
        yield return ScaleText(countdownText);

        countdownText.text = "";

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }
    }

    IEnumerator ScaleText(TextMeshProUGUI text)
    {
        float timer = 0f;
        Vector3 originalScale = text.transform.localScale;
        Vector3 targetScale = originalScale * maxScale;

        while (timer < scaleDuration)
        {
            text.transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / scaleDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = targetScale;

        timer = 0f;
        while (timer < scaleDuration)
        {
            text.transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / scaleDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = originalScale;
    }
}
