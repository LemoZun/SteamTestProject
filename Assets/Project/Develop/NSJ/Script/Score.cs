using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] ScoreModel _model;


    private void Awake()
    {
        _model = ModelFactory.CreateModel<ScoreModel, ScoreViewModel>();
    }
    private void Start()
    {
        _model.OnScoreChange += UpdateScore;
    }

    private void UpdateScore(int value)
    {
        _model.Score = value;
    }
}


[System.Serializable]
public class ScoreModel : BaseModel
{
    public int Score;
    public Action<int> OnScoreChange;

    public void ChangeScore(int value)
    {
        Score += value;
        OnScoreChange?.Invoke(Score);
    }
}