using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameBox : BaseBox
{

    private TMP_Text _score => GetUI<TMP_Text>("ScoreText");
    private Button _save => GetUI<Button>("SaveButton");

    private Button _scoreUp => GetUI<Button>("ScoreUpButton");
    private Button _scoreDown => GetUI<Button>("ScoreDownButton");

    void Start()
    {
        Init();
        SubscribesEvnet();
    }

    void OnEnable()
    {
        SubscribesTempEvent();
        RefreshScore();
    }
    void OnDisable()
    {
        UnsubcribesTempEvent();
    }

    private void Init()
    {

    }
    private void SubscribesEvnet()
    {
        _save.onClick.AddListener(() => SaveData());
        _scoreUp.onClick.AddListener(() => TestGameManager.Instance.AddScore(1));
        _scoreDown.onClick.AddListener(() => TestGameManager.Instance.AddScore(-1));
    }
    private void SubscribesTempEvent()
    {
        if (TestGameManager.Instance == null)
            return;
        TestGameManager.Instance.OnScoreChangeEvent += RefreshScore;
    }

    private void UnsubcribesTempEvent()
    {
        if(TestGameManager.Instance == null)
            return;
        TestGameManager.Instance.OnScoreChangeEvent -= RefreshScore;
    }

    private void SaveData()
    {
        TestGameManager.Instance.SaveData();
        SceneManager.LoadScene("TitleScene");
    }

    private void RefreshScore(int score = int.MinValue)
    {
        if(score == int.MinValue)
        {
            if (TestGameManager.Instance.Data == null)
                return;
            score = TestGameManager.Instance.Data.Score;
        }
        _score.text = $"{score}";
    }
}
