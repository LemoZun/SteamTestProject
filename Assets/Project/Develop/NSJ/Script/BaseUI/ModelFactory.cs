using NSJ_SaveUtility;
using UnityEngine;

namespace NSJ_MVVM
{
    public static class ModelFactory
    {
        /// <summary>
        /// �𵨰� ����� �����ϴ� ���丮 �޼����Դϴ�.
        /// </summary>
        public static TModel CreateModel<TModel, TViewModel>(MonoBehaviour owner, out TViewModel vm)
            where TModel : BaseModel, ICopyable<TModel>, new()
            where TViewModel : BaseViewModel<TModel>, new()
        {
     

            TModel model = new TModel();
            TViewModel viewModel = new TViewModel();
            // ��𵨿� ���� �����մϴ�.
            viewModel.SetModel(model);
            // ���� �ʱ�ȭ�մϴ�.
            model.InitModel();

            // �˸��� �信 ��� �������� ����

            //ViewResistry<TViewModel>.TryBind(viewModel);

            AutoBinding.Bind(model, owner);

            vm = viewModel;
            return model;
        }
    }
}