using NSJ_MVVM;
using NSJ_SaveUtility;
using UnityEngine;

public class TestEnemy : BaseController<TestEnemyModel, TestEnemyViewModel>
{


    protected override void OnAwake()
    {
        
    }

    void Start()
    {
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _model.name = $"{Random.Range(0, 10000)}";
        }
    }
}

[System.Serializable]
public class TestEnemyModel : BaseModel, ICopyable<TestEnemyModel>
{
    public string name;
    public void CopyFrom(TestEnemyModel model)
    {
        name = model.name;
    }

    public override void Init()
    {

    }
}

public class TestEnemyViewModel : BaseViewModel<TestEnemyModel, TestEnemyViewModel>
{
    protected override void OnModelSet()
    {

    }
}


