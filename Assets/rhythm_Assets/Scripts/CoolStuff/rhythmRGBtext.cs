using UnityEngine;
using TMPro;

public class rhythmRGBText : MonoBehaviour
{
    public float speed = 1f;
    private TextMeshProUGUI text;
    private float hue;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        hue = Random.value;
    }

    void Update()
    {
        hue += Time.deltaTime * speed;
        if (hue > 1f)
            hue -= 1f;

        Color color = Color.HSVToRGB(hue, 1f, 1f);
        text.color = color;
    }
}