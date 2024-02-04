using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : " + 0;
    }

    // Update is called once per frame
    public void UpdateScore()
    {
        score += 1;
        scoreText.text = "Score : " + score;
    }
}
