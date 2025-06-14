using NSJ_MVVM;
using NSJ_SaveUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : BaseController<TestPlayerModel, TestPlayerViewModel>
{
    protected override void OnAwake()
    {
        EnableSave();
    }

    protected override void OnStart()
    {
        
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _model.Name = $"{Random.Range(0,10000)}";
        }
    }

}

[System.Serializable]
public class TestPlayerModel : BaseModel, ICopyable<TestPlayerModel>
{
    public string Name;

    public void CopyFrom(TestPlayerModel model)
    {
        Name = model.Name;
    }

    protected override void Init()
    {
        
    }

    protected override void OnLoadModel()
    {
        
    }
}

public class TestPlayerViewModel : BaseViewModel<TestPlayerModel, TestPlayerViewModel>
{
    protected override void OnModelSet()
    {
        
    }
}
