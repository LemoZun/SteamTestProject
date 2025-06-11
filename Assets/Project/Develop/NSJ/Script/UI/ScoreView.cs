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
    void Start()
    {
        Init();
        SubscribesEvnet();
    }

    void OnEnable()
    {
        SubscribesTempEvent();
    }
    void OnDisable()
    {
        UnsubcribesTempEvent();
    }

    protected override void OnViewModelSet()
    {
        Model?.Score.Bind(UpdateText);
    }
    protected override void OnViewModelRemoved()
    {
        Model?.Score.UnBind(UpdateText);
    }

    private void Init()
    {
        _score = GetUI<TMP_Text>("ScoreText");
        _save = GetUI<Button>("SaveButton");
        _scoreUp = GetUI<Button>("ScoreUpButton");
        _scoreDown = GetUI<Button>("ScoreDownButton");
    }
    private void SubscribesEvnet()
    {
        _save.onClick.AddListener(() => SaveData());
        _scoreUp.onClick.AddListener(() => Model.AddScore(1));
        _scoreDown.onClick.AddListener(() => Model.AddScore(-1));
    }
    private void SubscribesTempEvent()
    {
        if (TestGameManager.Instance == null)
            return;
    }

    private void UnsubcribesTempEvent()
    {
        if (TestGameManager.Instance == null)
            return;
    }

    private void SaveData()
    {
        TestGameManager.Instance.SaveData();
        SceneManager.LoadScene("TitleScene");
    }

    private void UpdateText(int value)
    {
        _score.text = $"{value}";
    }
}

public class ScoreViewModel : BaseViewModel<ScoreModel>
{
    public Bindable<int> Score = new Bindable<int>(0);

    public void AddScore(int value)
    {
        Model.ChangeScore(value);
    }

    protected override void OnModelSet()
    {
        Score.Value = Model.Score;

        Model.OnScoreChange += (score) => Score.Value = score;
    }
}
