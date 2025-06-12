using NSJ_MVVM;
using UnityEngine;

namespace NSJ_SaveUtility
{
    public static class AutoBinding
    {
        /// <summary>
        /// 모델을 자동으로 바인딩하는 메서드입니다. MonoBehaviour를 상속받는 클래스에서 사용합니다.
        /// </summary>
        public static void Bind<TModel>(TModel model, MonoBehaviour owner) where TModel : BaseModel, ICopyable<TModel>
        {
            // 세이브 매니저에 모델을 등록합니다
            SaveManager.RegisterModel(model);

            // 모델을 자동으로 언바인딩할 수 있도록 설정합니다.
            ModelAutoUnbinder unbinder = owner.GetComponentInChildren<ModelAutoUnbinder>();
            if (unbinder == null)
            {
                unbinder = owner.gameObject.AddComponent<ModelAutoUnbinder>();
                unbinder.Track(model);
            }
        }
    }
}