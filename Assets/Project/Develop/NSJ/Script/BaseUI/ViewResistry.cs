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
    /// 뷰모델과 뷰를 바인딩하는 딕셔너리입니다.
    /// </summary>
    private Dictionary<TViewModel, IView<TViewModel>> _bindings;
    /// <summary>
    /// 뷰모델과 바인딩할 수 있는 뷰의 목록입니다.
    /// </summary>
    private List<IView<TViewModel>> _views;

    /// <summary>
    /// 뷰모델과 뷰를 바인딩하는 레지스트리입니다.
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
    /// 뷰모델과 뷰의 바인딩을 해제하는 레지스트리입니다.
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
    /// 뷰모델을 뷰에 바인딩합니다. 뷰모델이 이미 바인딩되어 있으면 true를 반환하고, 새로운 뷰에 바인딩되면 true를 반환합니다.
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static bool TryBind(TViewModel viewModel)
    {
        if (Instance._bindings.ContainsKey(viewModel))
            return true;// 이미 바인딩된 뷰모델이므로 true 반환


        // 뷰모델에 바인딩된 뷰를 찾습니다.
        var targetView = Instance._views.FirstOrDefault(v => !v.HasViewModel);
        if (targetView == null) return false;

        // 뷰모델이 바인딩된 뷰를 찾았으므로, 해당 뷰에 뷰모델을 설정합니다.
        targetView.SetViewModel(viewModel);
        Instance._bindings.Add(viewModel, targetView);
        Instance._views.Remove(targetView);

        return true;
    }

    /// <summary>
    /// 뷰모델을 뷰에 재바인딩합니다. 뷰모델이 바인딩되어 있으면 해당 뷰에 다시 바인딩하고 true를 반환합니다.
    /// 동적으로 뷰모델을 변경할 때 사용됩니다.
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
    /// 바인딩되지 않은 뷰를 제거합니다. 바인딩된 뷰가 없고, 뷰가 있는 경우에만 호출됩니다.
    /// </summary>
    public static void ClearUnUsed()
    {
        if (Instance._bindings.Count == 0 && Instance._views.Count > 0)
        {
            // 바인딩되지 않은 뷰를 제거합니다.
            Instance._views = null;
            Instance._bindings = null;
        }
    }
}

