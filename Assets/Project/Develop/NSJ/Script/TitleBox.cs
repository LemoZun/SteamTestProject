using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleBox : BaseBox
{
    private Button _start => GetUI<Button>("StartButton");
    private Button _option => GetUI<Button>("OptionButton");
    private Button _exit => GetUI<Button>("ExitButton");


    void Start()
    {
        SubscribesEvent();
    }

    private void SubscribesEvent()
    {
        _start.onClick.AddListener(() => Panel.ChangeBox((int)TitlePanel.Box.Start));
    }
}
