using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBox : BaseBox
{
    private Button _save => GetUI<Button>("SaveButton");

    private void Start()
    {
        Init();
        SubscribesEvnet();
    }

    private void Init()
    {

    }
    private void SubscribesEvnet()
    {
        _save.onClick.AddListener(() => SaveData());
    }

    private void SaveData()
    {
        TestGameManager.Instance.SaveData();
        Canvas.ChangePanel((int)TitleCanvas.Panel.Title);
    }

}
