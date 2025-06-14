using NSJ_MVVM;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameView : BaseView<TestPlayerViewModel>
{
    TMP_Text _name;

    protected override void ClearView()
    {
        _name.text = "default";
    }

    protected override void InitAwake()
    {
        _name = GetUI<TMP_Text>("NameText");
    }

    protected override void InitStart()
    {
        
    }

    protected override void OnViewModelRemoved()
    {
        Model?.Name.UnBind(UpdateName);
    }

    protected override void OnViewModelSet()
    {
        Model?.Name.Bind(UpdateName);
        UpdateName(Model.Name.Value);
    }
    protected override void SubscribeEvents()
    {
        
    }

    private void UpdateName(string name)
    {
        _name.text = name;
    }
}
