using NSJ_MVVM;
using NSJ_SaveUtility;
using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] bool isSecond;
    [SerializeField] ScoreModel _model;
    private void Awake()
    {
        _model = ModelFactory.CreateModel<ScoreModel, ScoreViewModel>(this);
        _model.CanSave = true;
    }

    private void Start()
    {
        _model.LoadData<ScoreModel>();
    }
    private void Update()
    {


        if (isSecond)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _model.Score -= 1;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                _model.Score += 1;
            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _model.Score -= 1;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _model.Score += 1;
            }
        }
    }
}


[System.Serializable]
public class ScoreModel : BaseModel, ICopyable<ScoreModel>
{
    [SerializeField] private int _score;
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

    protected override void Init() { }

    protected override void OnLoadModel()
    {
        
    }
}
