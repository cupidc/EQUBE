using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI TotalDisplayScore;
    public TextMeshProUGUI ChainDisplayScore;
    int totalScore = 0;
    int chainScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChainDisplayScore.text = $"Chain Score = {chainScore}";
        TotalDisplayScore.text = $"Total Score = {totalScore}";
    }

    public void AddPoints(int _points)
    {
        chainScore += _points;
    }

    public void SetMultiplier(int _multiplier)
    {
        totalScore += chainScore * _multiplier;
        chainScore = 0;
    }
}
