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
        /// ���� �ε�Ǿ��� �� ����� �Ӽ��� �ٽ� ���ε��ϴ� �޼����Դϴ�.
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

            // �ε� �Ǿ��� ���� �̺�Ʈ�� �����մϴ�.
            Model.OnLoadEvent += TryRebind;

            // ����� �����Ǿ��� �� ȣ��Ǵ� �޼��带 �����մϴ�.
            OnModelSet();
        }

        /// <summary>
        /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�. �� �޼���� SetModel �޼��忡�� ȣ��˴ϴ�.
        /// </summary>
        protected abstract void OnModelSet();

    }
}
