using NSJ_MVVM;
using NSJ_SaveUtility;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    [SerializeField] private TestEnemyModel _model;

    private TestEnemyViewModel _vm;
    private void Awake()
    {
        _model = ModelFactory.CreateModel<TestEnemyModel,TestEnemyViewModel>(this, out _vm);
    }

    private void Start()
    {
        
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

    protected override void Init()
    {

    }

    protected override void OnLoadModel()
    {
       
    }
}

public class TestEnemyViewModel : BaseViewModel<TestEnemyModel>
{
    protected override void OnModelSet()
    {

    }
}


