using NSJ_MVVM;
using UnityEngine.UI;

public class TitleView : BaseView<TitleViewModel>
{
    private Button _start;
    private Button _option;
    private Button _exit;


    protected override void InitAwake()
    {
        _start = GetUI<Button>("StartButton");
        _option = GetUI<Button>("OptionButton");
        _exit = GetUI<Button>("ExitButton");
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

    protected override void ClearView()
    {

    }

    protected override void SubscribeEvents()
    {
        _start.onClick.AddListener(() => Panel.ChangeGroup(TitlePanel.Group.Start));
        _exit.onClick.AddListener(ExitGame);
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif  
    }

    public override void Register()
    {
        ViewResistry<TitleView>.Resister(this);
    }

    public override void UnResister()
    {
        ViewResistry<TitleView>.UnResister(this);
    }

    public override void RemoveViewModel()
    {
        ViewResistry<TitleView>.RemoveRebind(this);
    }

    public override void ExchangeViewModel(IView<TitleViewModel> otherView)
    {
        ViewResistry<TitleView>.ExchangeRebind(this, otherView);
    }
}

public class TitleViewModel : BaseViewModel
{

}
