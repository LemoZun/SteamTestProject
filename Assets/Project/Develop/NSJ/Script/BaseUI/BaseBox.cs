using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBox : BaseUI
{
    [HideInInspector] public BasePanel Panel;
    [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;
}

public class BaseBox<TViewModel> : BaseBox
{
    protected TViewModel Model;

    public virtual void SetModel(TViewModel model)
    {
        Model = model;
        OnModelSet();
    }

    protected virtual void OnModelSet() { }
}
