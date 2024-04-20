using UnityEngine;
using TMPro;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager Instance;

    void Start()
    {
        Instance = this;
        CalculateGrade();
    }

    void CalculateGrade()
    {
        int score = ScoreManager.comboScore; // Get the score from the ScoreManager
    }


}
