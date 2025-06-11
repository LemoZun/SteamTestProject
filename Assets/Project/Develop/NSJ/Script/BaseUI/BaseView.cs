using UnityEngine;

public class BaseView : BaseUI
{
    [HideInInspector] public BasePanel Panel;
    [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;
}

public class BaseView<TViewModel> : BaseView, IView<TViewModel>
{
    /// <summary>
    /// ����� ��Ÿ���� �ʵ��Դϴ�.
    /// </summary>
    protected TViewModel Model;

    /// <summary>
    /// ����� �����Ǿ����� ���θ� ��Ÿ���ϴ�.
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
    /// ����� �����մϴ�. �̹� ������ ��쿡�� �ƹ� �۾��� ���� �ʽ��ϴ�.
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
    /// ����� �����մϴ�. ���� ����� �����Ǿ� ���� ������ �ƹ� �۾��� ���� �ʽ��ϴ�.
    /// </summary>
    public void RemoveViewModel()
    {
        if (HasViewModel == false) return;

        OnViewModelRemoved();
        Model = default;
        HasViewModel = false;
    }

    /// <summary>
    /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�.
    /// </summary>
    protected virtual void OnViewModelSet() { }

    protected virtual void OnViewModelRemoved() { }
}
