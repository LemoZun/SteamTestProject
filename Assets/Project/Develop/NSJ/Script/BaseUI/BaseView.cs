using UnityEngine;

namespace NSJ_MVVM
{
    public class BaseView : BaseUI
    {
        [HideInInspector] public BasePanel Panel;
        [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;
    }

    public abstract class BaseView<TViewModel> : BaseView, IView<TViewModel> where TViewModel : BaseViewModel
    {

        /// <summary>
        /// ����� �����Ǿ����� ���θ� ��Ÿ���ϴ�.
        /// </summary>
        public bool HasViewModel { get; set; } = false;

        /// <summary>
        /// ���� ViewID�� �����Ǿ����� ���θ� ��Ÿ���ϴ�.
        /// </summary>
        public bool HasViewID { get { return _hasViewID; } set { _hasViewID = value; } }
        /// <summary>
        /// ���� ViewID�� ��Ÿ���� �ʵ��Դϴ�.
        /// </summary>
        public int ViewID { get { return _viewID; } set { _viewID = value; } }

        [Header("ViewID")]
        /// <summary>
        /// ���� ViewID�� �����Ǿ����� ���θ� ��Ÿ���ϴ�.
        /// </summary>
        [SerializeField] protected bool _hasViewID;
        /// <summary>
        /// ���� ViewID�� ��Ÿ���� �ʵ��Դϴ�.
        /// </summary>
        [SerializeField] protected int _viewID;

        /// <summary>
        /// ����� ��Ÿ���� �ʵ��Դϴ�.
        /// </summary>
        protected TViewModel Model;


        protected override void Awake()
        {
            base.Awake();
            InitAwake();
            ViewResistry<TViewModel>.Resister(this);
        }
        protected virtual void Start()
        {
            InitStart();
            SubscribeEvents();
            if (Model == null)
                ClearView();
        }

        private void OnDestroy()
        {
            ViewResistry<TViewModel>.UnResister(this);
        }

        public void SetViewModel()
        {
            // ��ó�� �� ������ ��� �����ϸ� �ڵ� ���Եǰ�
            // �䰡 ��� ����� ��� ����ϴ� �� ���������� �ڵ����ԵǴ� �ʿ������?
        }
        /// <summary>
        /// ��� �����
        /// </summary>
        public void RemoveViewModel()
        {
            ViewResistry<TViewModel>.TryRebind(this);
        }

        /// <summary>
        /// �� ���� ��� ��ü�ϱ�
        /// </summary>
        public void ExchangeViewModel(IView<TViewModel> otherView)
        {
            ViewResistry<TViewModel>.TryRebind(this, otherView);
        }

        /// <summary>
        /// ����� �����Ͽ�����, ������ �ʱ�ȭ�մϴ�. �̹� ������ ��쿡�� �ƹ� �۾��� ���� �ʽ��ϴ�.
        /// </summary>
        /// <param name="model"></param>
        public void OnSetViewModel(TViewModel model)
        {
            if (model == null) return;
            if (HasViewModel == true) return;

            Model = model;
            HasViewModel = true;
            Model.HasViewID.Value = HasViewID;
            Model.ViewID.Value = ViewID;

            Model.OnRebindEvent += TryRebind;
            OnViewModelSet();
        }


        /// <summary>
        /// ����� ������ �Ŀ� ������ �ʱ�ȭ �մϴ�. ���� ����� �����Ǿ� ���� ������ �ƹ� �۾��� ���� �ʽ��ϴ�.
        /// </summary>
        public void OnRemoveViewModel()
        {
            if (HasViewModel == false) return;

            Model.OnRebindEvent -= TryRebind;
            ClearView();
            OnViewModelRemoved();
            Model = default;
            HasViewModel = false;
        }

 

        /// <summary>
        /// �䰡 Awake �ܰ迡�� �ʱ�ȭ�Ǵ� �޼����Դϴ�.
        /// </summary>
        protected abstract void InitAwake();

        /// <summary>
        /// �䰡 Start �ܰ迡�� �ʱ�ȭ�Ǵ� �޼����Դϴ�.
        /// </summary>
        protected abstract void InitStart();
        /// <summary>
        /// �䰡 �̺�Ʈ�� �����ϴ� �޼����Դϴ�. �� �޼���� �䰡 Start �ܰ迡�� ȣ��˴ϴ�.
        /// </summary>
        protected abstract void SubscribeEvents();

        /// <summary>
        /// �並 �ʱ�ȭ�մϴ�
        /// </summary>
        protected abstract void ClearView();

        /// <summary>
        /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�.
        /// </summary>
        protected abstract void OnViewModelSet();

        protected abstract void OnViewModelRemoved();


        private void TryRebind()
        {
            ViewResistry<TViewModel>.TryRebind(Model);
        }
    }
}
