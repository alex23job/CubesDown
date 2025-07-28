using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private Text txtScore;
    [SerializeField] private Text txtLevel;
    [SerializeField] private Text txtLive;

    [SerializeField] private GameObject lossPanel;
    [SerializeField] private Text txtResult;

    [SerializeField] private Image imgSch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ViewLevel(1);
        ViewScore(0);
        ViewScherepacha(false);
    }

    public void ViewScore(int score)
    {
        string nmScore = (Language.Instance.CurrentLanguage == "ru") ? "Очки" : "Score";
        txtScore.text = $"{nmScore} : {score}";
    }

    public void ViewLevel(int level)
    {
        string nmLevel = (Language.Instance.CurrentLanguage == "ru") ? "Уровень" : "Level";
        txtLevel.text = $"{nmLevel} : {level}";
    }

    public void ViewLive(int live)
    {
        txtLive.text = live.ToString();
    }

    public void ViewLossPanel(int level, int result) 
    {
        //  Ваш результат : Уровень 1    Очки 23
        if (Language.Instance.CurrentLanguage == "ru")
        {
            txtResult.text = $"Ваш результат :\n Уровень {level}    Очки {result}";
        }
        else
        {
            txtResult.text = $"Your result :\n Level {level}    Score {result}";
        }
        lossPanel.SetActive(true);
    }

    public void ViewScherepacha(bool isView)
    {
        imgSch.gameObject.SetActive(isView);
    }
}
