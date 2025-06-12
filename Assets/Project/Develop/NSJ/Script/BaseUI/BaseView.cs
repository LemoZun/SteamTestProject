using UnityEngine;

namespace NSJ_MVVM
{
    public class BaseView : BaseUI
    {
        [HideInInspector] public BasePanel Panel;
        [HideInInspector] public BaseCanvas Canvas => Panel.Canvas;
    }

    public class BaseView<TViewModel> : BaseView, IView<TViewModel> where TViewModel : BaseViewModel
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
            ViewResistry<TViewModel>.Resister(this);
            InitAwake();
        }
        protected virtual void Start()
        {
            InitStart();
            SubscribeEvents();
        }

        private void OnDestroy()
        {
            ViewResistry<TViewModel>.UnResister(this);
            ViewResistry<TViewModel>.ClearUnUsed();
        }

        /// <summary>
        /// ����� �����մϴ�. �̹� ������ ��쿡�� �ƹ� �۾��� ���� �ʽ��ϴ�.
        /// </summary>
        /// <param name="model"></param>
        public void SetViewModel(TViewModel model)
        {
            if (HasViewModel == true) return;

            Model = model;
            HasViewModel = true;
            model.HasViewID.Value = HasViewID;
            model.ViewID.Value = ViewID;
            OnViewModelSet();
        }

        /// <summary>
        /// ����� �����մϴ�. ���� ����� �����Ǿ� ���� ������ �ƹ� �۾��� ���� �ʽ��ϴ�.
        /// </summary>
        public void RemoveViewModel()
        {
            if (HasViewModel == false) return;

            OnViewModelRemoved();
            Model = default;
            HasViewModel = false;
        }

        /// <summary>
        /// �䰡 Awake �ܰ迡�� �ʱ�ȭ�Ǵ� �޼����Դϴ�.
        /// </summary>
        protected virtual void InitAwake() { }

        /// <summary>
        /// �䰡 Start �ܰ迡�� �ʱ�ȭ�Ǵ� �޼����Դϴ�.
        /// </summary>
        protected virtual void InitStart() { }
        /// <summary>
        /// �䰡 �̺�Ʈ�� �����ϴ� �޼����Դϴ�. �� �޼���� �䰡 Start �ܰ迡�� ȣ��˴ϴ�.
        /// </summary>
        protected virtual void SubscribeEvents() { }


        /// <summary>
        /// ����� �����Ǿ��� �� ȣ��Ǵ� �޼����Դϴ�.
        /// </summary>
        protected virtual void OnViewModelSet() { }

        protected virtual void OnViewModelRemoved() { }
    }
}
