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
        /// 뷰모델이 설정되었는지 여부를 나타냅니다.
        /// </summary>
        public bool HasViewModel { get; set; } = false;

        /// <summary>
        /// 뷰의 ViewID가 설정되었는지 여부를 나타냅니다.
        /// </summary>
        public bool HasViewID { get { return _hasViewID; } set { _hasViewID = value; } }
        /// <summary>
        /// 뷰의 ViewID를 나타내는 필드입니다.
        /// </summary>
        public int ViewID { get { return _viewID; } set { _viewID = value; } }

        [Header("ViewID")]
        /// <summary>
        /// 뷰의 ViewID가 설정되었는지 여부를 나타냅니다.
        /// </summary>
        [SerializeField] protected bool _hasViewID;
        /// <summary>
        /// 뷰의 ViewID를 나타내는 필드입니다.
        /// </summary>
        [SerializeField] protected int _viewID;

        /// <summary>
        /// 뷰모델을 나타내는 필드입니다.
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
        // 어처피 뷰 있을떄 뷰모델 생성하면 자동 삽입되고
        // 뷰가 없어도 뷰모델이 잠깐 대기하다 뷰 생성됬을때 자동삽입되니 필요없을듯?
        //}

        /// <summary>
        /// 레지스트리에 본인을 등록합니다 
        /// 예시: ViewResistry.Resister(this);
        /// </summary>
        public abstract void Register();

        /// <summary>
        /// 레지스트리에 본인을 해제합니다
        /// ViewResistry<View>.UnResister(this);
        /// </summary>
        public abstract void UnResister();

        /// <summary>
        /// 현재 뷰에서 연결된 뷰모델을 제거합니다
        /// ViewResistry.RemoveRebind(this);
        /// </summary>
        public abstract void RemoveViewModel();

        /// <summary>
        /// 다른 뷰와 뷰모델을 교체합니다
        /// ViewResistry.ExchangeRebind(this, other);
        /// </summary>
        public abstract void ExchangeViewModel(IView<TViewModel> otherView);
        /// <summary>
        /// 명시적으로 강제 캐스팅
        /// </summary>
        /// <param name="viewModel"></param>
        public void OnSetViewModel(IViewModel viewModel)
        {
            OnSetViewModel((TViewModel)viewModel);
        }
        /// <summary>
        /// 뷰모델을 설정하였을때, 설정을 초기화합니다. 이미 설정된 경우에는 아무 작업도 하지 않습니다.
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
        /// 뷰모델을 제거한 후에 설정을 초기화 합니다. 현재 뷰모델이 설정되어 있지 않으면 아무 작업도 하지 않습니다.
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
        /// 뷰가 Awake 단계에서 초기화되는 메서드입니다.
        /// </summary>
        protected abstract void InitAwake();

        /// <summary>
        /// 뷰가 Start 단계에서 초기화되는 메서드입니다.
        /// </summary>
        protected abstract void InitStart();
        /// <summary>
        /// 뷰가 이벤트를 구독하는 메서드입니다. 이 메서드는 뷰가 Start 단계에서 호출됩니다.
        /// </summary>
        protected abstract void SubscribeEvents();

        /// <summary>
        /// 뷰를 초기화합니다
        /// </summary>
        protected abstract void ClearView();

        /// <summary>
        /// 뷰모델이 설정되었을 때 호출되는 메서드입니다.
        /// </summary>
        protected abstract void OnViewModelSet();

        /// <summary>
        /// 뷰모델이 해제되었을 때 호출되는 메서드입니다.
        /// </summary>
        protected abstract void OnViewModelRemoved();

        /// <summary>
        /// 모델 파괴시에 이벤트로 호출됩니다
        /// </summary>
        private void DestroyModel()
        {
            ViewResistry<BaseView<TViewModel>>.RemoveRebind(this);
            ClearView();
        }
    }
}
