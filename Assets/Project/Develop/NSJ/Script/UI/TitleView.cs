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
}

public class TitleViewModel : BaseViewModel
{

}
