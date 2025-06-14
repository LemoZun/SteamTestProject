using NSJ_MVVM;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class ModelAutoUnbinder : MonoBehaviour
    {
        private BaseModel _model;

        /// <summary>
        /// ���� ĳ���ϰ� �����մϴ�
        /// </summary>
        public void Track<TModel>(TModel model) where TModel : BaseModel, ICopyable<TModel>
        {
            _model = model;
        }

        /// <summary>
        /// ��ü�� �ı��� �� �𵨿��� ��ü�� �ı��Ǿ����� �˸��ϴ�
        /// </summary>
        void OnDestroy()
        {
            if (_model != null)
            {
                _model.DestroyModel();
            }
        }
    }
}