public class BaseViewModel { }

public class BaseViewModel<TModel> : BaseViewModel where TModel : BaseModel
{
    protected TModel Model { get; set; }

    public virtual void SetModel(TModel model)
    {
        Model = model;
        OnModelSet();
    }

    protected virtual void OnModelSet() { }
}
