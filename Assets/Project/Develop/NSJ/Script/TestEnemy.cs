using NSJ_MVVM;
using NSJ_SaveUtility;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] TestEnemyModel _model;

    private void Awake()
    {
        _model = ModelFactory.CreateModel<TestEnemyModel, TestEnemyViewModel>(this);
    }

    void Start()
    {
        _model.LoadData<TestEnemyModel>();
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


