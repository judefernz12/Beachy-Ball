using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public int highscore;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI highscoreUI;

    void Start()
    {
        highscore=PlayerPrefs.GetInt("highscore");
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text=score.ToString();
        highscoreUI.text=highscore.ToString();
        if(score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("highscore",highscore);
        }
    }

    void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag=="scoreup"){
        score++;
       }
    }
}
