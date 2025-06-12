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
        /// 뷰모델을 설정합니다. 이미 설정된 경우에는 아무 작업도 하지 않습니다.
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
        /// 뷰모델을 제거합니다. 현재 뷰모델이 설정되어 있지 않으면 아무 작업도 하지 않습니다.
        /// </summary>
        public void RemoveViewModel()
        {
            if (HasViewModel == false) return;

            OnViewModelRemoved();
            Model = default;
            HasViewModel = false;
        }

        /// <summary>
        /// 뷰가 Awake 단계에서 초기화되는 메서드입니다.
        /// </summary>
        protected virtual void InitAwake() { }

        /// <summary>
        /// 뷰가 Start 단계에서 초기화되는 메서드입니다.
        /// </summary>
        protected virtual void InitStart() { }
        /// <summary>
        /// 뷰가 이벤트를 구독하는 메서드입니다. 이 메서드는 뷰가 Start 단계에서 호출됩니다.
        /// </summary>
        protected virtual void SubscribeEvents() { }


        /// <summary>
        /// 뷰모델이 설정되었을 때 호출되는 메서드입니다.
        /// </summary>
        protected virtual void OnViewModelSet() { }

        protected virtual void OnViewModelRemoved() { }
    }
}
