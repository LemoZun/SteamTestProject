using System.Collections.Generic;
using System.Linq;
public class ViewResistry<TViewModel>
{
    private static ViewResistry<TViewModel> _instance;
    public static ViewResistry<TViewModel> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ViewResistry<TViewModel>();
            }
            return _instance;
        }
    }
    /// <summary>
    /// ��𵨰� �並 ���ε��ϴ� ��ųʸ��Դϴ�.
    /// </summary>
    private Dictionary<TViewModel, IView<TViewModel>> _bindings;
    /// <summary>
    /// ��𵨰� ���ε��� �� �ִ� ���� ����Դϴ�.
    /// </summary>
    private List<IView<TViewModel>> _views;

    /// <summary>
    /// ��𵨰� �並 ���ε��ϴ� ������Ʈ���Դϴ�.
    /// </summary>
    /// <param name="view"></param>
    public static void Resister(IView<TViewModel> view)
    {
        if(Instance._bindings == null)
        {
            Instance._bindings = new Dictionary<TViewModel, IView<TViewModel>>();
        }

        if(Instance._views == null)
        {
            Instance._views = new List<IView<TViewModel>>();
        }


        if (Instance._views.Contains(view) == false)
            Instance._views.Add(view);
    }

    /// <summary>
    /// ��𵨰� ���� ���ε��� �����ϴ� ������Ʈ���Դϴ�.
    /// </summary>
    /// <param name="view"></param>
    public static void UnResister(IView<TViewModel> view)
    {
        Instance._views.Remove(view);

        KeyValuePair<TViewModel, IView<TViewModel>> entry = Instance._bindings.FirstOrDefault(x => x.Value.Equals(view));

        if (entry.Equals(default(KeyValuePair<TViewModel, IView<TViewModel>>)) == false)
        {
            Instance._bindings.Remove(entry.Key);
        }
    }

    /// <summary>
    /// ����� �信 ���ε��մϴ�. ����� �̹� ���ε��Ǿ� ������ true�� ��ȯ�ϰ�, ���ο� �信 ���ε��Ǹ� true�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static bool TryBind(TViewModel viewModel)
    {
        if (Instance._bindings.ContainsKey(viewModel))
            return true;// �̹� ���ε��� ����̹Ƿ� true ��ȯ


        // ��𵨿� ���ε��� �並 ã���ϴ�.
        var targetView = Instance._views.FirstOrDefault(v => !v.HasViewModel);
        if (targetView == null) return false;

        // ����� ���ε��� �並 ã�����Ƿ�, �ش� �信 ����� �����մϴ�.
        targetView.SetViewModel(viewModel);
        Instance._bindings.Add(viewModel, targetView);
        Instance._views.Remove(targetView);

        return true;
    }

    /// <summary>
    /// ����� �信 ����ε��մϴ�. ����� ���ε��Ǿ� ������ �ش� �信 �ٽ� ���ε��ϰ� true�� ��ȯ�մϴ�.
    /// �������� ����� ������ �� ���˴ϴ�.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static bool TryRebind(TViewModel viewModel)
    {
        if (!Instance._bindings.TryGetValue(viewModel, out var view))
            return false;

        view.SetViewModel(viewModel);
        return true;
    }

    /// <summary>
    /// ���ε����� ���� �並 �����մϴ�. ���ε��� �䰡 ����, �䰡 �ִ� ��쿡�� ȣ��˴ϴ�.
    /// </summary>
    public static void ClearUnUsed()
    {
        if (Instance._bindings.Count == 0 && Instance._views.Count > 0)
        {
            // ���ε����� ���� �並 �����մϴ�.
            Instance._views = null;
            Instance._bindings = null;
        }
    }
}

