using NSJ_MVVM;
using TMPro;

public class PlayerNameView : BaseView<TestPlayerViewModel>
{
    TMP_Text _name;



    public override void Register()
    {
        ViewResistry<PlayerNameView>.Resister(this);
    }
    public override void UnResister()
    {
        ViewResistry<PlayerNameView>.UnResister(this);
    }
    public override void RemoveViewModel()
    {
        ViewResistry<PlayerNameView>.RemoveRebind(this);
    }
    public override void ExchangeViewModel(IView<TestPlayerViewModel> otherView)
    {
        ViewResistry<PlayerNameView>.ExchangeRebind(this, otherView);
    }


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
