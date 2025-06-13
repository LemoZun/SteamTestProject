using NSJ_MVVM;
using NSJ_SaveUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] ScoreModel _model;


    private void Awake()
    {
        _model = ModelFactory.CreateModel<ScoreModel, ScoreViewModel>(this);
    }
    private void Start()
    {
        _model.LoadData<ScoreModel>();
    }
}


[System.Serializable]
public class ScoreModel : BaseModel, ICopyable<ScoreModel>
{
    [SerializeField]private int _score;
    public int Score { get { return _score; } set { _score = value; OnScoreChange?.Invoke(value); } }
    public Action<int> OnScoreChange;

    public void ChangeScore(int value)
    {
        Score += value;
    }

    public void CopyFrom(ScoreModel model)
    {
        Score = model.Score;
    }
}