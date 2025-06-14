using NSJ_MVVM;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public class ModelAutoUnbinder : MonoBehaviour
    {
        private BaseModel _model;

        /// <summary>
        /// 모델을 캐싱하고 추적합니다
        /// </summary>
        public void Track<TModel>(TModel model) where TModel : BaseModel, ICopyable<TModel>
        {
            _model = model;
        }

        /// <summary>
        /// 객체가 파괴될 때 모델에게 객체가 파괴되었음을 알립니다
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