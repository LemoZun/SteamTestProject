using NSJ_MVVM;
using NSJ_SaveUtility;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartView : BaseView<StartVIewModel>
{
    public struct SaveSlot
    {
        public Button Button;
        public TMP_Text Text;
    }

    private Button _exit;

    private List<SaveSlot> _saves = new List<SaveSlot>();



    protected override void InitAwake()
    {
        InitSaveButton();
        _exit = GetUI<Button>("ExitButton");
    }

    private void OnEnable()
    {
        RefreshSaveSlot();
    }


    protected override void InitStart()
    {
    }


    protected override void SubscribeEvents()
    {
        _exit.onClick.AddListener(() => Panel.ChangeGroup(TitlePanel.Group.Title));
        SubscribeSaveButton();
    }

    private void InitSaveButton()
    {
        BindSaveStruct("1SaveButton", "1SaveText");
        BindSaveStruct("2SaveButton", "2SaveText");
        BindSaveStruct("3SaveButton", "3SaveText");
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
        for (int i = 0; i < _saves.Count; i++)
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
        SaveManager.Instance.LoadData(number);
        SceneManager.LoadScene("GameScene");
    }

    private void RefreshSaveSlot()
    {
        for (int i = 0; i < _saves.Count; i++)
        {
            if (SaveManager.Instance.LoadData(i))
            {
                _saves[i].Text.text = SaveManager.Instance.Data.LastSaveTime;
            }
        }
    }


    protected override void ClearView()
    {
    }

    protected override void OnViewModelSet()
    {
    }

    protected override void OnViewModelRemoved()
    {
    }

    public override void Register()
    {
        ViewResistry<StartView>.Resister(this);
    }

    public override void UnResister()
    {
        ViewResistry<StartView>.UnResister(this);
    }

    public override void RemoveViewModel()
    {
        ViewResistry<StartView>.RemoveRebind(this);
    }

    public override void ExchangeViewModel(IView<StartVIewModel> otherView)
    {
        ViewResistry<StartView>.ExchangeRebind(this, otherView);
    }
}

public class StartVIewModel : BaseViewModel
{

}
