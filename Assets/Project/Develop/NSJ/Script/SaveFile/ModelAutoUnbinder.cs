using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelAutoUnbinder : MonoBehaviour
{
    private BaseModel _model;

    public void Track<TModel>(TModel model) where TModel : BaseModel, ICopyable<TModel>
    {
        _model = model;
    }

    void OnDestroy()
    {
        if (_model != null)
        {
            SaveManager.UnRegisterModel(_model);
        }
    }
}
