using NSJ_MVVM;
using UnityEngine;

namespace NSJ_SaveUtility
{
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
                _model.DestroyModel();
            }
        }
    }
}