using System;
using UnityEngine;

namespace NSJ_MVVM
{
    public class BaseViewModel
    {
        public Bindable<bool> IsLoaded;
        public Bindable<bool> HasViewID;
        public Bindable<int> ViewID;

        public event Action OnRebindEvent;

        /// <summary>
        /// 모델이 로드되었을 때 뷰모델의 속성을 다시 바인딩하는 메서드입니다.
        /// </summary>
        protected void TryRebind()
        {
            OnRebindEvent?.Invoke();
        }
    }

    public abstract class BaseViewModel<TModel> : BaseViewModel where TModel : BaseModel
    {
        protected TModel Model { get; set; }

        /// <summary>
        /// 뷰모델을 설정하는 메서드입니다. 이 메서드는 뷰모델이 설정될 때 호출됩니다.
        /// </summary>
        public void SetModel(TModel model)
        {
            Model = model;

            // 뷰모델의 속성을 Bindable로 초기화합니다.
            IsLoaded = new Bindable<bool>(model.IsLoaded);
            HasViewID = new Bindable<bool>(model.HasViewID);
            ViewID = new Bindable<int>(model.ViewID);

            // 뷰모델의 속성 변경 이벤트를 구독합니다.
            Model.OnIsLoadedChanged += (isLoaded) => IsLoaded.Value = isLoaded;
            Model.OnHasViewIDChanged += (hasViewID) => HasViewID.Value = hasViewID;
            Model.OnViewIDChanged += (viewID) => ViewID.Value = viewID;

            // 모델의 속성을 Bindable로 설정합니다.
            IsLoaded.Bind(x => Model.IsLoaded = IsLoaded.Value);
            HasViewID.Bind(x => Model.HasViewID = HasViewID.Value);
            ViewID.Bind(x => Model.ViewID = ViewID.Value);

            // 로드 되었을 때의 이벤트를 구독합니다.
            Model.OnLoadEvent += TryRebind;

            // 뷰모델이 설정되었을 때 호출되는 메서드를 실행합니다.
            OnModelSet();
        }

        /// <summary>
        /// 뷰모델이 설정되었을 때 호출되는 메서드입니다. 이 메서드는 SetModel 메서드에서 호출됩니다.
        /// </summary>
        protected abstract void OnModelSet();

    }
}
