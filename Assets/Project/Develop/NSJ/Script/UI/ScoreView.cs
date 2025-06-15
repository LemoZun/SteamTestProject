using NSJ_MVVM;
using NSJ_SaveUtility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class ScoreView : BaseView<ScoreViewModel>
{

    private TMP_Text _score;

    protected override void InitAwake()
    {
        ViewResistry<ScoreView>.Resister(this);

        _score = GetUI<TMP_Text>("ScoreText");
    }
    protected override void InitStart()
    {
    }
    protected override void OnViewModelSet()
    {
        Model?.Score.Bind(UpdateText);
    }
    protected override void OnViewModelRemoved()
    {
        Model?.Score.UnBind(UpdateText);;
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



    public override void Register()
    {
        ViewResistry<ScoreView>.Resister(this);
    }

    public override void UnResister()
    {
        ViewResistry<ScoreView>.UnResister(this);
    }

    public override void RemoveViewModel()
    {
        ViewResistry<ScoreView>.RemoveRebind(this);
    }

    public override void ExchangeViewModel(IView<ScoreViewModel> otherView)
    {
        ViewResistry<ScoreView>.ExchangeRebind(this, otherView);
    }
}

public class RedScoreViewModel: ScoreViewModel
{

}

public class ScoreViewModel : BaseViewModel<ScoreModel>
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
