using NSJ_SaveUtility;
using UnityEngine;

namespace NSJ_MVVM
{
    public static class ModelFactory
    {
        /// <summary>
        /// 모델과 뷰모델을 생성하는 팩토리 메서드입니다.
        /// </summary>
        public static TModel CreateModel<TModel, TViewModel>(MonoBehaviour owner)
            where TModel : BaseModel, ICopyable<TModel>, new()
            where TViewModel : BaseViewModel<TModel>, new()
        {
            TModel model = new TModel();
            TViewModel viewModel = new TViewModel();
            // 뷰모델에 모델을 설정합니다.
            viewModel.SetModel(model);
            // 모델을 초기화합니다.
            model.InitModel();

            // 알맞은 뷰에 뷰모델 의존성을 주입
            ViewResistry<TViewModel>.TryBind(viewModel);

            AutoBinding.Bind(model, owner);

            return model;
        }
    }
}