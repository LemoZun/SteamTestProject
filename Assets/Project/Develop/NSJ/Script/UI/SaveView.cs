using NSJ_MVVM;
using NSJ_SaveUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveView : BaseView<SaveViewModel>
{
    private Button _save;

    protected override void ClearView()
    {
        
    }

    protected override void InitAwake()
    {
        _save = GetUI<Button>("SaveButton");
    }

    protected override void InitStart()
    {
       
    }

    protected override void OnViewModelRemoved()
    {
        
    }

    protected override void OnViewModelSet()
    {
        
    }

    protected override void SubscribeEvents()
    {
        _save.onClick.AddListener(() => Save());
    }

    private void Save()
    {
        SaveManager.Instance.SaveData();
        SceneManager.LoadScene("TitleScene");
    }
}

public class SaveViewModel : BaseViewModel
{

}