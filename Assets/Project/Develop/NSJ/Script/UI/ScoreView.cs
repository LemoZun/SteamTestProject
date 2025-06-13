using NSJ_MVVM;
using NSJ_SaveUtility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreView : BaseView<ScoreViewModel>
{

    private TMP_Text _score;

    protected override void OnViewModelSet()
    {
        Model?.Score.Bind(UpdateText);
    }
    protected override void OnViewModelRemoved()
    {
        Model?.Score.UnBind(UpdateText);;
    }

    protected override void InitAwake()
    {
        _score = GetUI<TMP_Text>("ScoreText");
    }
    protected override void SubscribeEvents()
    {

    }

    private void UpdateText(int value)
    {
        _score.text = $"{value}";
    }

    protected override void ClearView()
    {
        _score.text = "0";
    }

    protected override void InitStart()
    {
    }
}

public class ScoreViewModel : BaseViewModel<ScoreModel, ScoreViewModel>
{
    public Bindable<int> Score;

    public void AddScore(int value)
    {
        Model.ChangeScore(value);
    }

    protected override void OnModelSet()
    {
        Score = new Bindable<int>(Model.Score);

        Model.OnScoreChange += (score) => Score.Value = score;
    }
}
