using System;
using UnityEngine;

namespace NSJ_MVVM
{
    public class BaseViewModel : IViewModel
    {
        private Bindable<bool> _isLoaded;
        private Bindable<bool> _hasViewID;
        private Bindable<int> _viewID;

        public Bindable<bool> IsLoaded { get { return _isLoaded; }set { _isLoaded = value; } }

        public Bindable<bool> HasViewID { get { return _hasViewID; } set { _hasViewID = value; } }
        public Bindable<int> ViewID { get { return _viewID; } set { _viewID = value; } }

        public event Action OnDestroyEvent;

        protected void DestroyViewModel()
        {

            OnDestroyEvent?.Invoke();
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


            Model.OnDestroyEvent += DestroyViewModel;
            // 뷰모델이 설정되었을 때 호출되는 메서드를 실행합니다.
            OnModelSet();
        }

        /// <summary>
        /// 뷰모델이 설정되었을 때 호출되는 메서드입니다. 이 메서드는 SetModel 메서드에서 호출됩니다.
        /// </summary>
        protected abstract void OnModelSet();
    }
}
