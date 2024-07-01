using UnityEngine;
using TMPro;

public class rhythmResultsManager : MonoBehaviour
{
    public static rhythmResultsManager Instance;

    void Start()
    {
        Instance = this;
        CalculateGrade();
    }

    void CalculateGrade()
    {
        int score = rhythmScoreManager.comboScore;
    }

}
