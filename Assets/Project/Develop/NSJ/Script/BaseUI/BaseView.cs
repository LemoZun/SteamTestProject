using UnityEngine;

namespace NSJ_MVVM
{
    public abstract class BaseView : BaseUI
    {
        [HideInInspector] public BaseGroup Group;
        [HideInInspector] public BasePanel Panel => Group.Panel;
        [HideInInspector] public BaseCanvas Canvas => Group.Panel.Canvas;
    }

    public abstract class BaseView<TViewModel> : BaseView, IView<TViewModel>
        where TViewModel : BaseViewModel
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
            Register();
        }
        protected virtual void Start()
        {
            InitStart();
            SubscribeEvents();
            if (Model == null)
                ClearView();
        }

        protected virtual void OnDestroy()
        {
            UnResister();
        }

        //public void SetViewModel()
        //{
        // ��ó�� �� ������ ��� �����ϸ� �ڵ� ���Եǰ�
        // �䰡 ��� ����� ��� ����ϴ� �� ���������� �ڵ����ԵǴ� �ʿ������?
        //}

        /// <summary>
        /// ������Ʈ���� ������ ����մϴ� 
        /// ����: ViewResistry.Resister(this);
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// ������Ʈ���� ������ �����մϴ�
        /// ViewResistry<View>.UnResister(this);
        /// </summary>
        public abstract void UnResister();

        /// <summary>
        /// ���� �信�� ����� ����� �����մϴ�
        /// ViewResistry.RemoveRebind(this);
        /// </summary>
        public abstract void RemoveViewModel();

        /// <summary>
        /// �ٸ� ��� ����� ��ü�մϴ�
        /// ViewResistry.ExchangeRebind(this, other);
        /// </summary>
        public abstract void ExchangeViewModel(IView<TViewModel> otherView);
        /// <summary>
        /// ��������� ���� ĳ����
        /// </summary>
        /// <param name="viewModel"></param>
        public void OnSetViewModel(IViewModel viewModel)
        {
            OnSetViewModel((TViewModel)viewModel);
        }
        /// <summary>
        /// ����� �����Ͽ�����, ������ �ʱ�ȭ�մϴ�. �̹� ������ ��쿡�� �ƹ� �۾��� ���� �ʽ��ϴ�.
        /// </summary>
        /// <param name="model"></param>
        public void OnSetViewModel(TViewModel model)
        {

            if (model == null) return;

            if (HasViewModel == true)
            {
                OnViewModelSet();
                return;
            }

            Model = model;
            HasViewModel = true;
            Model.HasViewID.Value = HasViewID;
            Model.ViewID.Value = ViewID;
            Model.OnDestroyEvent += DestroyModel;
            OnViewModelSet();
        }


        /// <summary>
        /// ����� ������ �Ŀ� ������ �ʱ�ȭ �մϴ�. ���� ����� �����Ǿ� ���� ������ �ƹ� �۾��� ���� �ʽ��ϴ�.
        /// </summary>
        public void OnRemoveViewModel()
        {
            if (HasViewModel == false) return;

            Model.OnDestroyEvent -= DestroyModel;
            ClearView();
            OnViewModelRemoved();
            Model = null;
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

        /// <summary>
        /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�.
        /// </summary>
        protected abstract void OnViewModelRemoved();

        /// <summary>
        /// �� �ı��ÿ� �̺�Ʈ�� ȣ��˴ϴ�
        /// </summary>
        private void DestroyModel()
        {
            ViewResistry<BaseView<TViewModel>>.RemoveRebind(this);
            ClearView();
        }
    }
}
