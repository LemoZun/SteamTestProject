

using UnityEngine;

public class BaseViewModel 
{
    public Bindable<bool> HasViewID;
    public Bindable<int> ViewID;
}

public class BaseViewModel<TModel> : BaseViewModel where TModel : BaseModel
{
    protected TModel Model { get; set; }

    /// <summary>
    /// ����� �����ϴ� �޼����Դϴ�. �� �޼���� ����� ������ �� ȣ��˴ϴ�.
    /// </summary>
    public virtual void SetModel(TModel model)
    {
        Model = model;

        HasViewID = new Bindable<bool>(model.HasViewID);
        ViewID = new Bindable<int>(model.ViewID);

        HasViewID.Bind(x => Model.HasViewID = HasViewID.Value);
        ViewID.Bind(x => Model.ViewID = ViewID.Value);


        if(TestGameManager.Instance != null)
        {
            Model.SubscribeSaveEvent<TModel>();
        }

        OnModelSet();
    }

    /// <summary>
    /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�. �� �޼���� SetModel �޼��忡�� ȣ��˴ϴ�.
    /// </summary>
    protected virtual void OnModelSet() { }


}
