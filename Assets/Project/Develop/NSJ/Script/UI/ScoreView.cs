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
    private Button _save;
    private Button _scoreUp;
    private Button _scoreDown;

    public event UnityAction<int> OnScoreButtonClick;

    protected override void OnViewModelSet()
    {
        Model?.Score.Bind(UpdateText);
    }
    protected override void OnViewModelRemoved()
    {
        Model?.Score.UnBind(UpdateText);
    }

    protected override void InitAwake()
    {
        _score = GetUI<TMP_Text>("ScoreText");
        _save = GetUI<Button>("SaveButton");
        _scoreUp = GetUI<Button>("ScoreUpButton");
        _scoreDown = GetUI<Button>("ScoreDownButton");
    }
    protected override void SubscribeEvents()
    {
        _save.onClick.AddListener(() => SaveData());
        _scoreUp.onClick.AddListener(() => Model.AddScore(1));
        _scoreDown.onClick.AddListener(() => Model.AddScore(-1));
    }

    private void SaveData()
    {
        SaveManager.Instance.SaveData();
        SceneManager.LoadScene("TitleScene");
    }

    private void UpdateText(int value)
    {
        _score.text = $"{value}";
    }
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

       // Model.OnScoreChange += (score) => Score.Value = score;
    }
}
