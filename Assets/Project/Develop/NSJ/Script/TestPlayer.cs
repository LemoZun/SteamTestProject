using NSJ_MVVM;
using NSJ_SaveUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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


    public event UnityAction<string> OnNameChanged; 
    public string Name { get { return _name; } set { _name = value; OnNameChanged?.Invoke(value); } }
    [SerializeField]private string _name;

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
    public Bindable<string> Name;

    protected override void OnModelSet()
    {
        Name = new Bindable<string>(Model.Name);

        Model.OnNameChanged += (name) => Name.Value = name;
    }
}
