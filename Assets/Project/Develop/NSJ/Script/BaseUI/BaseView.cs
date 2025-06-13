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
            // 어처피 뷰 있을떄 뷰모델 생성하면 자동 삽입되고
            // 뷰가 없어도 뷰모델이 잠깐 대기하다 뷰 생성됬을때 자동삽입되니 필요없을듯?
        }
        /// <summary>
        /// 뷰모델 지우기
        /// </summary>
        public void RemoveViewModel()
        {
            ViewResistry<TViewModel>.TryRebind(this);
        }

        /// <summary>
        /// 두 뷰의 뷰모델 교체하기
        /// </summary>
        public void ExchangeViewModel(IView<TViewModel> otherView)
        {
            ViewResistry<TViewModel>.TryRebind(this, otherView);
        }

        /// <summary>
        /// 뷰모델을 설정하였을때, 설정을 초기화합니다. 이미 설정된 경우에는 아무 작업도 하지 않습니다.
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
        /// 뷰모델을 제거한 후에 설정을 초기화 합니다. 현재 뷰모델이 설정되어 있지 않으면 아무 작업도 하지 않습니다.
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

        protected abstract void OnViewModelRemoved();


        private void TryRebind()
        {
            ViewResistry<TViewModel>.TryRebind(Model);
        }
    }
}
