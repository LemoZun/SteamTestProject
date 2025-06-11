using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleView : BaseView<TitleBoxModel>
{
    private Button _start;
    private Button _option;
    private Button _exit;


    void Start()
    {
        Init();
        SubscribesEvent();
    }

    private void Init()
    {
        _start = GetUI<Button>("StartButton");
        _option = GetUI<Button>("OptionButton");
        _exit = GetUI<Button>("ExitButton");
    }
    private void SubscribesEvent()
    {
        _start.onClick.AddListener(() => Panel.ChangeView(TitlePanel.View.Start));
        _exit.onClick.AddListener(ExitGame);
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif  
    }
}

public class TitleBoxModel 
{
    
}
