using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBox : BaseBox
{
    public struct SaveSlot
    {
        public Button Button;
        public TMP_Text Text;
    }

    private Button _exit => GetUI<Button>("ExitButton");

    private List<SaveSlot> _saves = new List<SaveSlot>();

    void Start()
    {
        Init();
        SubscribesEvent();
    }

    private void OnEnable()
    {
        RefreshSaveSlot();
    }

    private void Init()
    {
        InitSaveButton();
    }

    private void SubscribesEvent()
    {
        _exit.onClick.AddListener(() => Panel.ChangeBox(TitlePanel.Box.Title));
        SubscribeSaveButton();
    }

    private void InitSaveButton()
    {
        BindSaveStruct("1SaveButton", "1SaveText");
        BindSaveStruct("2SaveButton", "2SaveText");
        BindSaveStruct("3SaveButton", "3SaveText");

        RefreshSaveSlot();
    }

    private void BindSaveStruct(string buttonName, string textName)
    {
        SaveSlot saveStruct = new SaveSlot
        {
            Button = GetUI<Button>(buttonName),
            Text = GetUI<TMP_Text>(textName),

        };
        _saves.Add(saveStruct);
    }

    private void SubscribeSaveButton()
    {
        for(int i = 0; i < _saves.Count; i++)
        {
            int curNum = i;
            _saves[curNum].Button.onClick.AddListener(() => 
            {
                ClickSave(curNum);
            });
        }   
    }

    private void ClickSave(int number)
    {
        TestGameManager.Instance.LoadData(number);
        SceneManager.LoadScene("GameScene");
    }

    private void RefreshSaveSlot()
    {
        for (int i = 0; i < _saves.Count; i++)
        {
            if (TestGameManager.Instance.LoadData(i))
            {
                _saves[i].Text.text = TestGameManager.Instance.Data.LastSaveTime;
            }
        }
    }
}
