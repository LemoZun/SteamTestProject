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
        /// ����� �����ϴ� �޼����Դϴ�. �� �޼���� ����� ������ �� ȣ��˴ϴ�.
        /// </summary>
        public void SetModel(TModel model)
        {
            Model = model;

            // ����� �Ӽ��� Bindable�� �ʱ�ȭ�մϴ�.
            IsLoaded = new Bindable<bool>(model.IsLoaded);
            HasViewID = new Bindable<bool>(model.HasViewID);
            ViewID = new Bindable<int>(model.ViewID);

            // ����� �Ӽ� ���� �̺�Ʈ�� �����մϴ�.
            Model.OnIsLoadedChanged += (isLoaded) => IsLoaded.Value = isLoaded;
            Model.OnHasViewIDChanged += (hasViewID) => HasViewID.Value = hasViewID;
            Model.OnViewIDChanged += (viewID) => ViewID.Value = viewID;

            // ���� �Ӽ��� Bindable�� �����մϴ�.
            IsLoaded.Bind(x => Model.IsLoaded = IsLoaded.Value);
            HasViewID.Bind(x => Model.HasViewID = HasViewID.Value);
            ViewID.Bind(x => Model.ViewID = ViewID.Value);


            Model.OnDestroyEvent += DestroyViewModel;
            // ����� �����Ǿ��� �� ȣ��Ǵ� �޼��带 �����մϴ�.
            OnModelSet();
        }

        /// <summary>
        /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�. �� �޼���� SetModel �޼��忡�� ȣ��˴ϴ�.
        /// </summary>
        protected abstract void OnModelSet();
    }
}
