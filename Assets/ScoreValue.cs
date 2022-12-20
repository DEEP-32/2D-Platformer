using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ScoreValue : MonoBehaviour
{
    [SerializeField]GameObject squareParent;
    [SerializeField] GameObject scoreTextParent;
 
    TileScoreValue[] squareSprite;
    Text[] scoreText;

    private void Awake()
    {
        squareSprite = squareParent.GetComponentsInChildren<TileScoreValue>();
        scoreText = scoreTextParent.GetComponentsInChildren<Text>();
        if(scoreText == null)
        {
            Debug.Log("Its empty");
        }

        for(int i = 0; i < squareSprite.Length; i++)
        {
            int score = Random.Range(2,squareSprite.Length);
            squareSprite[i].score = score;
            scoreText[i].text = ""+score;
        }
    }


}
