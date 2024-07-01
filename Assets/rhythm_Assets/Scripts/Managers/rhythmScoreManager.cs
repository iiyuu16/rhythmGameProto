using UnityEngine;
using TMPro;

public class rhythmScoreManager : MonoBehaviour
{
    public static rhythmScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TextMeshPro scoreTextMesh;
    public TextMeshProUGUI resScoreText;
    public static int comboScore;
    public int healthDeduction = 10;

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
        DeductHealthOnMiss();
    }

    private static void DeductHealthOnMiss()
    {
        if (rhythmHealthManager.Instance != null)
        {
            rhythmHealthManager.Instance.DeductHealth(Instance.healthDeduction);
        }
        else
        {
            Debug.LogWarning("HealthManager instance not found.");
        }
    }

    private void Update()
    {
        UpdateScoreText();
        UpdateResultScoreText();
    }

    void UpdateScoreText()
    {
        scoreTextMesh.text = comboScore.ToString();
    }

    void UpdateResultScoreText()
    {
        resScoreText.text = rhythmScoreManager.comboScore.ToString();
    }
}
