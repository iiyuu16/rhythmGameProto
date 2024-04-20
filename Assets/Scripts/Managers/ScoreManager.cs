using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TextMeshPro scoreTextMesh;
    public TextMeshProUGUI resScoreText; // TextMeshProUGUI for displaying results
    public static int comboScore;

    void Start()
    {
        Instance = this;
        comboScore = 0;
    }

    public static void Hit()
    {
        comboScore += 1;
        Instance.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0;
        Instance.missSFX.Play();
    }

    private void Update()
    {
        UpdateScoreText(); // Update score text
        UpdateResultScoreText();
    }

    void UpdateScoreText()
    {
        // Update score text mesh
        scoreTextMesh.text = comboScore.ToString();
    }

    void UpdateResultScoreText()
    {
        // Update result score text with the final combo score
        resScoreText.text = ScoreManager.comboScore.ToString();
    }

}
