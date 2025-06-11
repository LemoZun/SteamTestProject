using UnityEngine;

public class BaseView : BaseUI
{
    [HideInInspector] public BasePanel Panel;
    [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;
}

public class BaseView<TViewModel> : BaseView, IView<TViewModel>
{
    /// <summary>
    /// 뷰모델을 나타내는 필드입니다.
    /// </summary>
    protected TViewModel Model;

    /// <summary>
    /// 뷰모델이 설정되었는지 여부를 나타냅니다.
    /// </summary>
    public bool HasViewModel { get; set; } = false;

    protected override void Awake()
    {
        base.Awake();
        ViewResistry<TViewModel>.Resister(this);
    }

    private void OnDestroy()
    {
        ViewResistry<TViewModel>.UnResister(this);
        ViewResistry<TViewModel>.ClearUnUsed();
    }

    /// <summary>
    /// 뷰모델을 설정합니다. 이미 설정된 경우에는 아무 작업도 하지 않습니다.
    /// </summary>
    /// <param name="model"></param>
    public void SetViewModel(TViewModel model)
    {
        if (HasViewModel == true) return;

        Model = model;
        HasViewModel = true;
        OnViewModelSet();
    }

    /// <summary>
    /// 뷰모델을 제거합니다. 현재 뷰모델이 설정되어 있지 않으면 아무 작업도 하지 않습니다.
    /// </summary>
    public void RemoveViewModel()
    {
        if (HasViewModel == false) return;

        OnViewModelRemoved();
        Model = default;
        HasViewModel = false;
    }

    /// <summary>
    /// 뷰모델이 설정되었을 때 호출되는 메서드입니다.
    /// </summary>
    protected virtual void OnViewModelSet() { }

    protected virtual void OnViewModelRemoved() { }
}
