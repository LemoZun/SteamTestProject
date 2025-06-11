using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModelFactory 
{
    /// <summary>
    /// �𵨰� ����� �����ϴ� ���丮 �޼����Դϴ�.
    /// </summary>

    public static TModel CreateModel<TModel, TViewModel>()
        where TModel : BaseModel, new()
        where TViewModel : BaseViewModel<TModel>, new()
    {
        TModel model = new TModel();
        TViewModel viewModel = new TViewModel();
        viewModel.SetModel(model);

        // �˸��� �信 ��� �������� ����
        ViewResistry<TViewModel>.TryBind(viewModel);

        return model;
    }
}
