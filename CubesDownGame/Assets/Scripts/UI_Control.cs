using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ViewLevel(1);
        ViewScore(0);
    }

    public void ViewScore(int score)
    {
        string nmScore = "Очки";
        txtScore.text = $"{nmScore} : {score}";
    }

    public void ViewLevel(int level)
    {
        string nmLevel = "Уровень";
        txtLevel.text = $"{nmLevel} : {level}";
    }

}
