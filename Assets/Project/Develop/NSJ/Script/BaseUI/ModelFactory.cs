using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModelFactory 
{
    /// <summary>
    /// 모델과 뷰모델을 생성하는 팩토리 메서드입니다.
    /// </summary>

    public static TModel CreateModel<TModel, TViewModel>()
        where TModel : BaseModel, new()
        where TViewModel : BaseViewModel<TModel>, new()
    {
        TModel model = new TModel();
        TViewModel viewModel = new TViewModel();
        viewModel.SetModel(model);

        // 알맞은 뷰에 뷰모델 의존성을 주입
        ViewResistry<TViewModel>.TryBind(viewModel);

        return model;
    }
}
