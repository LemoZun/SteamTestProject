

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
    /// 뷰모델을 설정하는 메서드입니다. 이 메서드는 뷰모델이 설정될 때 호출됩니다.
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
    /// 뷰모델이 설정되었을 때 호출되는 메서드입니다. 이 메서드는 SetModel 메서드에서 호출됩니다.
    /// </summary>
    protected virtual void OnModelSet() { }


}
