using UnityEngine;
using TMPro;


public class ScoreUI : MonoBehaviour
{
    float score = 0;
    TMP_Text scoreText = default;
    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        //Debug.Log("Connecting to OnCollect");
        BaseCollectable.OnCollect += UpdateScore;
    }

    private void OnDisable()
    {
        BaseCollectable.OnCollect -= UpdateScore;
    }

    private void UpdateScore(float currentScore)
    {
        //Debug.Log("Updating Score");
         score += currentScore;
        scoreText.text = "Score : "+score.ToString("00");
    }
}
